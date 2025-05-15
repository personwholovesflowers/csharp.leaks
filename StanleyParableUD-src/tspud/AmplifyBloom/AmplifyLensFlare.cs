using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x02000335 RID: 821
	[Serializable]
	public class AmplifyLensFlare : IAmplifyItem
	{
		// Token: 0x060014F2 RID: 5362 RVA: 0x0006FA64 File Offset: 0x0006DC64
		public AmplifyLensFlare()
		{
			this.m_lensGradient = new Gradient();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0006FB1C File Offset: 0x0006DD1C
		public void Init()
		{
			if (this.m_lensGradient.alphaKeys.Length == 0 && this.m_lensGradient.colorKeys.Length == 0)
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
				this.m_lensGradient.SetKeys(array, array2);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0006FC42 File Offset: 0x0006DE42
		public void Destroy()
		{
			if (this.m_lensFlareGradTexture != null)
			{
				Object.DestroyImmediate(this.m_lensFlareGradTexture);
				this.m_lensFlareGradTexture = null;
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0006FC64 File Offset: 0x0006DE64
		public void CreateLUTexture()
		{
			this.m_lensFlareGradTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false);
			this.m_lensFlareGradTexture.filterMode = FilterMode.Bilinear;
			this.TextureFromGradient();
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0006FC8C File Offset: 0x0006DE8C
		public RenderTexture ApplyFlare(Material material, RenderTexture source)
		{
			RenderTexture tempRenderTarget = AmplifyUtils.GetTempRenderTarget(source.width, source.height);
			material.SetVector(AmplifyUtils.LensFlareGhostsParamsId, this.m_lensFlareGhostsParams);
			material.SetTexture(AmplifyUtils.LensFlareLUTId, this.m_lensFlareGradTexture);
			material.SetVector(AmplifyUtils.LensFlareHaloParamsId, this.m_lensFlareHaloParams);
			material.SetFloat(AmplifyUtils.LensFlareGhostChrDistortionId, this.m_lensFlareGhostChrDistortion);
			material.SetFloat(AmplifyUtils.LensFlareHaloChrDistortionId, this.m_lensFlareHaloChrDistortion);
			Graphics.Blit(source, tempRenderTarget, material, 3 + this.m_lensFlareGhostAmount);
			return tempRenderTarget;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0006FD14 File Offset: 0x0006DF14
		public void TextureFromGradient()
		{
			for (int i = 0; i < 256; i++)
			{
				this.m_lensFlareGradColor[i] = this.m_lensGradient.Evaluate((float)i / 255f);
			}
			this.m_lensFlareGradTexture.SetPixels(this.m_lensFlareGradColor);
			this.m_lensFlareGradTexture.Apply();
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0006FD6C File Offset: 0x0006DF6C
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0006FD74 File Offset: 0x0006DF74
		public bool ApplyLensFlare
		{
			get
			{
				return this.m_applyLensFlare;
			}
			set
			{
				this.m_applyLensFlare = value;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x0006FD7D File Offset: 0x0006DF7D
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x0006FD85 File Offset: 0x0006DF85
		public float OverallIntensity
		{
			get
			{
				return this.m_overallIntensity;
			}
			set
			{
				this.m_overallIntensity = ((value < 0f) ? 0f : value);
				this.m_lensFlareGhostsParams.x = value * this.m_normalizedGhostIntensity;
				this.m_lensFlareHaloParams.x = value * this.m_normalizedHaloIntensity;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0006FDC3 File Offset: 0x0006DFC3
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x0006FDCB File Offset: 0x0006DFCB
		public int LensFlareGhostAmount
		{
			get
			{
				return this.m_lensFlareGhostAmount;
			}
			set
			{
				this.m_lensFlareGhostAmount = value;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x0006FDD4 File Offset: 0x0006DFD4
		// (set) Token: 0x060014FF RID: 5375 RVA: 0x0006FDDC File Offset: 0x0006DFDC
		public Vector4 LensFlareGhostsParams
		{
			get
			{
				return this.m_lensFlareGhostsParams;
			}
			set
			{
				this.m_lensFlareGhostsParams = value;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001500 RID: 5376 RVA: 0x0006FDE5 File Offset: 0x0006DFE5
		// (set) Token: 0x06001501 RID: 5377 RVA: 0x0006FDED File Offset: 0x0006DFED
		public float LensFlareNormalizedGhostsIntensity
		{
			get
			{
				return this.m_normalizedGhostIntensity;
			}
			set
			{
				this.m_normalizedGhostIntensity = ((value < 0f) ? 0f : value);
				this.m_lensFlareGhostsParams.x = this.m_overallIntensity * this.m_normalizedGhostIntensity;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x0006FE1D File Offset: 0x0006E01D
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x0006FE2A File Offset: 0x0006E02A
		public float LensFlareGhostsIntensity
		{
			get
			{
				return this.m_lensFlareGhostsParams.x;
			}
			set
			{
				this.m_lensFlareGhostsParams.x = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001504 RID: 5380 RVA: 0x0006FE47 File Offset: 0x0006E047
		// (set) Token: 0x06001505 RID: 5381 RVA: 0x0006FE54 File Offset: 0x0006E054
		public float LensFlareGhostsDispersal
		{
			get
			{
				return this.m_lensFlareGhostsParams.y;
			}
			set
			{
				this.m_lensFlareGhostsParams.y = value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x0006FE62 File Offset: 0x0006E062
		// (set) Token: 0x06001507 RID: 5383 RVA: 0x0006FE6F File Offset: 0x0006E06F
		public float LensFlareGhostsPowerFactor
		{
			get
			{
				return this.m_lensFlareGhostsParams.z;
			}
			set
			{
				this.m_lensFlareGhostsParams.z = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x0006FE7D File Offset: 0x0006E07D
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x0006FE8A File Offset: 0x0006E08A
		public float LensFlareGhostsPowerFalloff
		{
			get
			{
				return this.m_lensFlareGhostsParams.w;
			}
			set
			{
				this.m_lensFlareGhostsParams.w = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600150A RID: 5386 RVA: 0x0006FE98 File Offset: 0x0006E098
		// (set) Token: 0x0600150B RID: 5387 RVA: 0x0006FEA0 File Offset: 0x0006E0A0
		public Gradient LensFlareGradient
		{
			get
			{
				return this.m_lensGradient;
			}
			set
			{
				this.m_lensGradient = value;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600150C RID: 5388 RVA: 0x0006FEA9 File Offset: 0x0006E0A9
		// (set) Token: 0x0600150D RID: 5389 RVA: 0x0006FEB1 File Offset: 0x0006E0B1
		public Vector4 LensFlareHaloParams
		{
			get
			{
				return this.m_lensFlareHaloParams;
			}
			set
			{
				this.m_lensFlareHaloParams = value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600150E RID: 5390 RVA: 0x0006FEBA File Offset: 0x0006E0BA
		// (set) Token: 0x0600150F RID: 5391 RVA: 0x0006FEC2 File Offset: 0x0006E0C2
		public float LensFlareNormalizedHaloIntensity
		{
			get
			{
				return this.m_normalizedHaloIntensity;
			}
			set
			{
				this.m_normalizedHaloIntensity = ((value < 0f) ? 0f : value);
				this.m_lensFlareHaloParams.x = this.m_overallIntensity * this.m_normalizedHaloIntensity;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06001510 RID: 5392 RVA: 0x0006FEF2 File Offset: 0x0006E0F2
		// (set) Token: 0x06001511 RID: 5393 RVA: 0x0006FEFF File Offset: 0x0006E0FF
		public float LensFlareHaloIntensity
		{
			get
			{
				return this.m_lensFlareHaloParams.x;
			}
			set
			{
				this.m_lensFlareHaloParams.x = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001512 RID: 5394 RVA: 0x0006FF1C File Offset: 0x0006E11C
		// (set) Token: 0x06001513 RID: 5395 RVA: 0x0006FF29 File Offset: 0x0006E129
		public float LensFlareHaloWidth
		{
			get
			{
				return this.m_lensFlareHaloParams.y;
			}
			set
			{
				this.m_lensFlareHaloParams.y = value;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001514 RID: 5396 RVA: 0x0006FF37 File Offset: 0x0006E137
		// (set) Token: 0x06001515 RID: 5397 RVA: 0x0006FF44 File Offset: 0x0006E144
		public float LensFlareHaloPowerFactor
		{
			get
			{
				return this.m_lensFlareHaloParams.z;
			}
			set
			{
				this.m_lensFlareHaloParams.z = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001516 RID: 5398 RVA: 0x0006FF52 File Offset: 0x0006E152
		// (set) Token: 0x06001517 RID: 5399 RVA: 0x0006FF5F File Offset: 0x0006E15F
		public float LensFlareHaloPowerFalloff
		{
			get
			{
				return this.m_lensFlareHaloParams.w;
			}
			set
			{
				this.m_lensFlareHaloParams.w = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0006FF6D File Offset: 0x0006E16D
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x0006FF75 File Offset: 0x0006E175
		public float LensFlareGhostChrDistortion
		{
			get
			{
				return this.m_lensFlareGhostChrDistortion;
			}
			set
			{
				this.m_lensFlareGhostChrDistortion = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0006FF7E File Offset: 0x0006E17E
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x0006FF86 File Offset: 0x0006E186
		public float LensFlareHaloChrDistortion
		{
			get
			{
				return this.m_lensFlareHaloChrDistortion;
			}
			set
			{
				this.m_lensFlareHaloChrDistortion = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600151C RID: 5404 RVA: 0x0006FF8F File Offset: 0x0006E18F
		// (set) Token: 0x0600151D RID: 5405 RVA: 0x0006FF97 File Offset: 0x0006E197
		public int LensFlareGaussianBlurAmount
		{
			get
			{
				return this.m_lensFlareGaussianBlurAmount;
			}
			set
			{
				this.m_lensFlareGaussianBlurAmount = value;
			}
		}

		// Token: 0x04001108 RID: 4360
		private const int LUTTextureWidth = 256;

		// Token: 0x04001109 RID: 4361
		[SerializeField]
		private float m_overallIntensity = 1f;

		// Token: 0x0400110A RID: 4362
		[SerializeField]
		private float m_normalizedGhostIntensity = 0.8f;

		// Token: 0x0400110B RID: 4363
		[SerializeField]
		private float m_normalizedHaloIntensity = 0.1f;

		// Token: 0x0400110C RID: 4364
		[SerializeField]
		private bool m_applyLensFlare = true;

		// Token: 0x0400110D RID: 4365
		[SerializeField]
		private int m_lensFlareGhostAmount = 3;

		// Token: 0x0400110E RID: 4366
		[SerializeField]
		private Vector4 m_lensFlareGhostsParams = new Vector4(0.8f, 0.228f, 1f, 4f);

		// Token: 0x0400110F RID: 4367
		[SerializeField]
		private float m_lensFlareGhostChrDistortion = 2f;

		// Token: 0x04001110 RID: 4368
		[SerializeField]
		private Gradient m_lensGradient;

		// Token: 0x04001111 RID: 4369
		[SerializeField]
		private Texture2D m_lensFlareGradTexture;

		// Token: 0x04001112 RID: 4370
		private Color[] m_lensFlareGradColor = new Color[256];

		// Token: 0x04001113 RID: 4371
		[SerializeField]
		private Vector4 m_lensFlareHaloParams = new Vector4(0.1f, 0.573f, 1f, 128f);

		// Token: 0x04001114 RID: 4372
		[SerializeField]
		private float m_lensFlareHaloChrDistortion = 1.51f;

		// Token: 0x04001115 RID: 4373
		[SerializeField]
		private int m_lensFlareGaussianBlurAmount = 1;
	}
}
