using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000330 RID: 816
	[Serializable]
	public sealed class AmplifyGlare : IAmplifyItem
	{
		// Token: 0x060014CE RID: 5326 RVA: 0x0006E9D0 File Offset: 0x0006CBD0
		public AmplifyGlare()
		{
			this.m_currentGlareIdx = (int)this.m_currentGlareType;
			this.m_cromaticAberrationGrad = new Gradient();
			this._rtBuffer = new RenderTexture[16];
			this.m_weigthsMat = new Matrix4x4[4];
			this.m_offsetsMat = new Matrix4x4[4];
			this.m_amplifyGlareCache = new AmplifyGlareCache();
			this.m_whiteReference = new Color(0.63f, 0.63f, 0.63f, 0f);
			this.m_aTanFoV = Mathf.Atan(0.3926991f);
			this.m_starDefArr = new StarDefData[]
			{
				new StarDefData(StarLibType.Cross, "Cross", 2, 4, 1f, 0.85f, 0f, 0.5f, -1f, 90f),
				new StarDefData(StarLibType.Cross_Filter, "CrossFilter", 2, 4, 1f, 0.95f, 0f, 0.5f, -1f, 90f),
				new StarDefData(StarLibType.Snow_Cross, "snowCross", 3, 4, 1f, 0.96f, 0.349f, 0.5f, -1f, -1f),
				new StarDefData(StarLibType.Vertical, "Vertical", 1, 4, 1f, 0.96f, 0f, 0f, -1f, -1f),
				new StarDefData(StarLibType.Sunny_Cross, "SunnyCross", 4, 4, 1f, 0.88f, 0f, 0f, 0.95f, 45f)
			};
			this.m_glareDefArr = new GlareDefData[]
			{
				new GlareDefData(StarLibType.Cross, 0f, 0.5f),
				new GlareDefData(StarLibType.Cross_Filter, 0.44f, 0.5f),
				new GlareDefData(StarLibType.Cross_Filter, 1.22f, 1.5f),
				new GlareDefData(StarLibType.Snow_Cross, 0.17f, 0.5f),
				new GlareDefData(StarLibType.Snow_Cross, 0.7f, 1.5f),
				new GlareDefData(StarLibType.Sunny_Cross, 0f, 0.5f),
				new GlareDefData(StarLibType.Sunny_Cross, 0.79f, 1.5f),
				new GlareDefData(StarLibType.Vertical, 1.57f, 0.5f),
				new GlareDefData(StarLibType.Vertical, 0f, 0.5f)
			};
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0006EC44 File Offset: 0x0006CE44
		public void Init()
		{
			if (this.m_cromaticAberrationGrad.alphaKeys.Length == 0 && this.m_cromaticAberrationGrad.colorKeys.Length == 0)
			{
				GradientColorKey[] array = new GradientColorKey[]
				{
					new GradientColorKey(Color.white, 0f),
					new GradientColorKey(Color.blue, 0.25f),
					new GradientColorKey(Color.green, 0.5f),
					new GradientColorKey(Color.yellow, 0.75f),
					new GradientColorKey(Color.red, 1f)
				};
				GradientAlphaKey[] array2 = new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(1f, 0.25f),
					new GradientAlphaKey(1f, 0.5f),
					new GradientAlphaKey(1f, 0.75f),
					new GradientAlphaKey(1f, 1f)
				};
				this.m_cromaticAberrationGrad.SetKeys(array, array2);
			}
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0006ED6C File Offset: 0x0006CF6C
		public void Destroy()
		{
			for (int i = 0; i < this.m_starDefArr.Length; i++)
			{
				this.m_starDefArr[i].Destroy();
			}
			this.m_glareDefArr = null;
			this.m_weigthsMat = null;
			this.m_offsetsMat = null;
			for (int j = 0; j < this._rtBuffer.Length; j++)
			{
				if (this._rtBuffer[j] != null)
				{
					AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[j]);
					this._rtBuffer[j] = null;
				}
			}
			this._rtBuffer = null;
			this.m_amplifyGlareCache.Destroy();
			this.m_amplifyGlareCache = null;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0006EE00 File Offset: 0x0006D000
		public void SetDirty()
		{
			this.m_isDirty = true;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0006EE0C File Offset: 0x0006D00C
		public void OnRenderFromCache(RenderTexture source, RenderTexture dest, Material material, float glareIntensity, float cameraRotation)
		{
			for (int i = 0; i < this.m_amplifyGlareCache.TotalRT; i++)
			{
				this._rtBuffer[i] = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
			}
			int j = 0;
			for (int k = 0; k < this.m_amplifyGlareCache.StarDef.StarlinesCount; k++)
			{
				for (int l = 0; l < this.m_amplifyGlareCache.CurrentPassCount; l++)
				{
					this.UpdateMatrixesForPass(material, this.m_amplifyGlareCache.Starlines[k].Passes[l].Offsets, this.m_amplifyGlareCache.Starlines[k].Passes[l].Weights, glareIntensity, cameraRotation * this.m_amplifyGlareCache.StarDef.CameraRotInfluence);
					if (l == 0)
					{
						Graphics.Blit(source, this._rtBuffer[j], material, 2);
					}
					else
					{
						Graphics.Blit(this._rtBuffer[j - 1], this._rtBuffer[j], material, 2);
					}
					j++;
				}
			}
			for (int m = 0; m < this.m_amplifyGlareCache.StarDef.StarlinesCount; m++)
			{
				material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[m], this.m_amplifyGlareCache.AverageWeight);
				int num = (m + 1) * this.m_amplifyGlareCache.CurrentPassCount - 1;
				material.SetTexture(AmplifyUtils.AnamorphicRTS[m], this._rtBuffer[num]);
			}
			int num2 = 19 + this.m_amplifyGlareCache.StarDef.StarlinesCount - 1;
			dest.DiscardContents();
			Graphics.Blit(this._rtBuffer[0], dest, material, num2);
			for (j = 0; j < this._rtBuffer.Length; j++)
			{
				AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[j]);
				this._rtBuffer[j] = null;
			}
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x0006EFC8 File Offset: 0x0006D1C8
		public void UpdateMatrixesForPass(Material material, Vector4[] offsets, Vector4[] weights, float glareIntensity, float rotation)
		{
			float num = Mathf.Cos(rotation);
			float num2 = Mathf.Sin(rotation);
			for (int i = 0; i < 16; i++)
			{
				int num3 = i >> 2;
				int num4 = i & 3;
				this.m_offsetsMat[num3][num4, 0] = offsets[i].x * num - offsets[i].y * num2;
				this.m_offsetsMat[num3][num4, 1] = offsets[i].x * num2 + offsets[i].y * num;
				this.m_weigthsMat[num3][num4, 0] = glareIntensity * weights[i].x;
				this.m_weigthsMat[num3][num4, 1] = glareIntensity * weights[i].y;
				this.m_weigthsMat[num3][num4, 2] = glareIntensity * weights[i].z;
			}
			for (int j = 0; j < 4; j++)
			{
				material.SetMatrix(AmplifyUtils.AnamorphicGlareOffsetsMatStr[j], this.m_offsetsMat[j]);
				material.SetMatrix(AmplifyUtils.AnamorphicGlareWeightsMatStr[j], this.m_weigthsMat[j]);
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x0006F114 File Offset: 0x0006D314
		public void OnRenderImage(Material material, RenderTexture source, RenderTexture dest, float cameraRot)
		{
			Graphics.Blit(Texture2D.blackTexture, dest);
			if (this.m_isDirty || this.m_currentWidth != source.width || this.m_currentHeight != source.height)
			{
				this.m_isDirty = false;
				this.m_currentWidth = source.width;
				this.m_currentHeight = source.height;
				bool flag = false;
				GlareDefData glareDefData;
				if (this.m_currentGlareType == GlareLibType.Custom)
				{
					if (this.m_customGlareDef != null && this.m_customGlareDef.Length != 0)
					{
						glareDefData = this.m_customGlareDef[this.m_customGlareDefIdx];
						flag = true;
					}
					else
					{
						glareDefData = this.m_glareDefArr[0];
					}
				}
				else
				{
					glareDefData = this.m_glareDefArr[this.m_currentGlareIdx];
				}
				this.m_amplifyGlareCache.GlareDef = glareDefData;
				float num = (float)source.width;
				float num2 = (float)source.height;
				StarDefData starDefData = (flag ? glareDefData.CustomStarData : this.m_starDefArr[(int)glareDefData.StarType]);
				this.m_amplifyGlareCache.StarDef = starDefData;
				int num3 = ((this.m_glareMaxPassCount < starDefData.PassCount) ? this.m_glareMaxPassCount : starDefData.PassCount);
				this.m_amplifyGlareCache.CurrentPassCount = num3;
				float num4 = glareDefData.StarInclination + starDefData.Inclination;
				for (int i = 0; i < this.m_glareMaxPassCount; i++)
				{
					float num5 = (float)(i + 1) / (float)this.m_glareMaxPassCount;
					for (int j = 0; j < 8; j++)
					{
						Color color = this._overallTint * Color.Lerp(this.m_cromaticAberrationGrad.Evaluate((float)j / 7f), this.m_whiteReference, num5);
						this.m_amplifyGlareCache.CromaticAberrationMat[i, j] = Color.Lerp(this.m_whiteReference, color, glareDefData.ChromaticAberration);
					}
				}
				this.m_amplifyGlareCache.TotalRT = starDefData.StarlinesCount * num3;
				for (int k = 0; k < this.m_amplifyGlareCache.TotalRT; k++)
				{
					this._rtBuffer[k] = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
				}
				int l = 0;
				for (int m = 0; m < starDefData.StarlinesCount; m++)
				{
					StarLineData starLineData = starDefData.StarLinesArr[m];
					float num6 = num4 + starLineData.Inclination;
					float num7 = Mathf.Sin(num6);
					float num8 = Mathf.Cos(num6);
					Vector2 vector = default(Vector2);
					vector.x = num8 / num * (starLineData.SampleLength * this.m_overallStreakScale);
					vector.y = num7 / num2 * (starLineData.SampleLength * this.m_overallStreakScale);
					float num9 = (this.m_aTanFoV + 0.1f) * 280f / (num + num2) * 1.2f;
					for (int n = 0; n < num3; n++)
					{
						for (int num10 = 0; num10 < 8; num10++)
						{
							float num11 = Mathf.Pow(starLineData.Attenuation, num9 * (float)num10);
							this.m_amplifyGlareCache.Starlines[m].Passes[n].Weights[num10] = this.m_amplifyGlareCache.CromaticAberrationMat[num3 - 1 - n, num10] * num11 * ((float)n + 1f) * 0.5f;
							this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].x = vector.x * (float)num10;
							this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].y = vector.y * (float)num10;
							if (Mathf.Abs(this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].x) >= 0.9f || Mathf.Abs(this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].y) >= 0.9f)
							{
								this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].x = 0f;
								this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num10].y = 0f;
								this.m_amplifyGlareCache.Starlines[m].Passes[n].Weights[num10] *= 0f;
							}
						}
						for (int num12 = 8; num12 < 16; num12++)
						{
							this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num12] = -this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets[num12 - 8];
							this.m_amplifyGlareCache.Starlines[m].Passes[n].Weights[num12] = this.m_amplifyGlareCache.Starlines[m].Passes[n].Weights[num12 - 8];
						}
						this.UpdateMatrixesForPass(material, this.m_amplifyGlareCache.Starlines[m].Passes[n].Offsets, this.m_amplifyGlareCache.Starlines[m].Passes[n].Weights, this.m_intensity, starDefData.CameraRotInfluence * cameraRot);
						if (n == 0)
						{
							Graphics.Blit(source, this._rtBuffer[l], material, 2);
						}
						else
						{
							Graphics.Blit(this._rtBuffer[l - 1], this._rtBuffer[l], material, 2);
						}
						l++;
						vector *= this.m_perPassDisplacement;
						num9 *= this.m_perPassDisplacement;
					}
				}
				this.m_amplifyGlareCache.AverageWeight = Vector4.one / (float)starDefData.StarlinesCount;
				for (int num13 = 0; num13 < starDefData.StarlinesCount; num13++)
				{
					material.SetVector(AmplifyUtils.AnamorphicGlareWeightsStr[num13], this.m_amplifyGlareCache.AverageWeight);
					int num14 = (num13 + 1) * num3 - 1;
					material.SetTexture(AmplifyUtils.AnamorphicRTS[num13], this._rtBuffer[num14]);
				}
				int num15 = 19 + starDefData.StarlinesCount - 1;
				dest.DiscardContents();
				Graphics.Blit(this._rtBuffer[0], dest, material, num15);
				for (l = 0; l < this._rtBuffer.Length; l++)
				{
					AmplifyUtils.ReleaseTempRenderTarget(this._rtBuffer[l]);
					this._rtBuffer[l] = null;
				}
				return;
			}
			this.OnRenderFromCache(source, dest, material, this.m_intensity, cameraRot);
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060014D5 RID: 5333 RVA: 0x0006F7C5 File Offset: 0x0006D9C5
		// (set) Token: 0x060014D6 RID: 5334 RVA: 0x0006F7CD File Offset: 0x0006D9CD
		public GlareLibType CurrentGlare
		{
			get
			{
				return this.m_currentGlareType;
			}
			set
			{
				if (this.m_currentGlareType != value)
				{
					this.m_currentGlareType = value;
					this.m_currentGlareIdx = (int)value;
					this.m_isDirty = true;
				}
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x0006F7ED File Offset: 0x0006D9ED
		// (set) Token: 0x060014D8 RID: 5336 RVA: 0x0006F7F5 File Offset: 0x0006D9F5
		public int GlareMaxPassCount
		{
			get
			{
				return this.m_glareMaxPassCount;
			}
			set
			{
				this.m_glareMaxPassCount = value;
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0006F805 File Offset: 0x0006DA05
		// (set) Token: 0x060014DA RID: 5338 RVA: 0x0006F80D File Offset: 0x0006DA0D
		public float PerPassDisplacement
		{
			get
			{
				return this.m_perPassDisplacement;
			}
			set
			{
				this.m_perPassDisplacement = value;
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x0006F81D File Offset: 0x0006DA1D
		// (set) Token: 0x060014DC RID: 5340 RVA: 0x0006F825 File Offset: 0x0006DA25
		public float Intensity
		{
			get
			{
				return this.m_intensity;
			}
			set
			{
				this.m_intensity = ((value < 0f) ? 0f : value);
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x0006F844 File Offset: 0x0006DA44
		// (set) Token: 0x060014DE RID: 5342 RVA: 0x0006F84C File Offset: 0x0006DA4C
		public Color OverallTint
		{
			get
			{
				return this._overallTint;
			}
			set
			{
				this._overallTint = value;
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x0006F85C File Offset: 0x0006DA5C
		// (set) Token: 0x060014E0 RID: 5344 RVA: 0x0006F864 File Offset: 0x0006DA64
		public bool ApplyLensGlare
		{
			get
			{
				return this.m_applyGlare;
			}
			set
			{
				this.m_applyGlare = value;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060014E1 RID: 5345 RVA: 0x0006F86D File Offset: 0x0006DA6D
		// (set) Token: 0x060014E2 RID: 5346 RVA: 0x0006F875 File Offset: 0x0006DA75
		public Gradient CromaticColorGradient
		{
			get
			{
				return this.m_cromaticAberrationGrad;
			}
			set
			{
				this.m_cromaticAberrationGrad = value;
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060014E3 RID: 5347 RVA: 0x0006F885 File Offset: 0x0006DA85
		// (set) Token: 0x060014E4 RID: 5348 RVA: 0x0006F88D File Offset: 0x0006DA8D
		public float OverallStreakScale
		{
			get
			{
				return this.m_overallStreakScale;
			}
			set
			{
				this.m_overallStreakScale = value;
				this.m_isDirty = true;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060014E5 RID: 5349 RVA: 0x0006F89D File Offset: 0x0006DA9D
		// (set) Token: 0x060014E6 RID: 5350 RVA: 0x0006F8A5 File Offset: 0x0006DAA5
		public GlareDefData[] CustomGlareDef
		{
			get
			{
				return this.m_customGlareDef;
			}
			set
			{
				this.m_customGlareDef = value;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x0006F8AE File Offset: 0x0006DAAE
		// (set) Token: 0x060014E8 RID: 5352 RVA: 0x0006F8B6 File Offset: 0x0006DAB6
		public int CustomGlareDefIdx
		{
			get
			{
				return this.m_customGlareDefIdx;
			}
			set
			{
				this.m_customGlareDefIdx = value;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060014E9 RID: 5353 RVA: 0x0006F8BF File Offset: 0x0006DABF
		// (set) Token: 0x060014EA RID: 5354 RVA: 0x0006F8C8 File Offset: 0x0006DAC8
		public int CustomGlareDefAmount
		{
			get
			{
				return this.m_customGlareDefAmount;
			}
			set
			{
				if (value == this.m_customGlareDefAmount)
				{
					return;
				}
				if (value == 0)
				{
					this.m_customGlareDef = null;
					this.m_customGlareDefIdx = 0;
					this.m_customGlareDefAmount = 0;
					return;
				}
				GlareDefData[] array = new GlareDefData[value];
				for (int i = 0; i < value; i++)
				{
					if (i < this.m_customGlareDefAmount)
					{
						array[i] = this.m_customGlareDef[i];
					}
					else
					{
						array[i] = new GlareDefData();
					}
				}
				this.m_customGlareDefIdx = Mathf.Clamp(this.m_customGlareDefIdx, 0, value - 1);
				this.m_customGlareDef = array;
				this.m_customGlareDefAmount = value;
			}
		}

		// Token: 0x040010E2 RID: 4322
		public const int MaxLineSamples = 8;

		// Token: 0x040010E3 RID: 4323
		public const int MaxTotalSamples = 16;

		// Token: 0x040010E4 RID: 4324
		public const int MaxStarLines = 4;

		// Token: 0x040010E5 RID: 4325
		public const int MaxPasses = 4;

		// Token: 0x040010E6 RID: 4326
		public const int MaxCustomGlare = 32;

		// Token: 0x040010E7 RID: 4327
		[SerializeField]
		private GlareDefData[] m_customGlareDef;

		// Token: 0x040010E8 RID: 4328
		[SerializeField]
		private int m_customGlareDefIdx;

		// Token: 0x040010E9 RID: 4329
		[SerializeField]
		private int m_customGlareDefAmount;

		// Token: 0x040010EA RID: 4330
		[SerializeField]
		private bool m_applyGlare = true;

		// Token: 0x040010EB RID: 4331
		[SerializeField]
		private Color _overallTint = Color.white;

		// Token: 0x040010EC RID: 4332
		[SerializeField]
		private Gradient m_cromaticAberrationGrad;

		// Token: 0x040010ED RID: 4333
		[SerializeField]
		private int m_glareMaxPassCount = 4;

		// Token: 0x040010EE RID: 4334
		private StarDefData[] m_starDefArr;

		// Token: 0x040010EF RID: 4335
		private GlareDefData[] m_glareDefArr;

		// Token: 0x040010F0 RID: 4336
		private Matrix4x4[] m_weigthsMat;

		// Token: 0x040010F1 RID: 4337
		private Matrix4x4[] m_offsetsMat;

		// Token: 0x040010F2 RID: 4338
		private Color m_whiteReference;

		// Token: 0x040010F3 RID: 4339
		private float m_aTanFoV;

		// Token: 0x040010F4 RID: 4340
		private AmplifyGlareCache m_amplifyGlareCache;

		// Token: 0x040010F5 RID: 4341
		[SerializeField]
		private int m_currentWidth;

		// Token: 0x040010F6 RID: 4342
		[SerializeField]
		private int m_currentHeight;

		// Token: 0x040010F7 RID: 4343
		[SerializeField]
		private GlareLibType m_currentGlareType;

		// Token: 0x040010F8 RID: 4344
		[SerializeField]
		private int m_currentGlareIdx;

		// Token: 0x040010F9 RID: 4345
		[SerializeField]
		private float m_perPassDisplacement = 4f;

		// Token: 0x040010FA RID: 4346
		[SerializeField]
		private float m_intensity = 0.17f;

		// Token: 0x040010FB RID: 4347
		[SerializeField]
		private float m_overallStreakScale = 1f;

		// Token: 0x040010FC RID: 4348
		private bool m_isDirty = true;

		// Token: 0x040010FD RID: 4349
		private RenderTexture[] _rtBuffer;
	}
}
