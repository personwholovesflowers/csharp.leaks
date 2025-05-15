using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace AmplifyBloom
{
	// Token: 0x02000329 RID: 809
	[AddComponentMenu("")]
	[Serializable]
	public class AmplifyBloomBase : MonoBehaviour
	{
		// Token: 0x06001455 RID: 5205 RVA: 0x0006CE9C File Offset: 0x0006B09C
		private void Awake()
		{
			if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
			{
				AmplifyUtils.DebugLog("Null graphics device detected. Skipping effect silently.", LogType.Error);
				this.silentError = true;
				return;
			}
			if (!AmplifyUtils.IsInitialized)
			{
				AmplifyUtils.InitializeIds();
			}
			this.m_anamorphicGlare.Init();
			this.m_lensFlare.Init();
			for (int i = 0; i < 6; i++)
			{
				this.m_tempDownsamplesSizes[i] = new Vector2(0f, 0f);
			}
			this.m_cameraTransform = base.transform;
			this.m_tempFilterBuffer = null;
			this.m_starburstMat = Matrix4x4.identity;
			if (this.m_temporalFilteringCurve == null)
			{
				this.m_temporalFilteringCurve = new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(1f, 0.999f)
				});
			}
			this.m_bloomShader = Shader.Find("Hidden/AmplifyBloom");
			if (this.m_bloomShader != null)
			{
				this.m_bloomMaterial = new Material(this.m_bloomShader);
				this.m_bloomMaterial.hideFlags = HideFlags.DontSave;
			}
			else
			{
				AmplifyUtils.DebugLog("Main Bloom shader not found", LogType.Error);
				base.gameObject.SetActive(false);
			}
			this.m_finalCompositionShader = Shader.Find("Hidden/BloomFinal");
			if (this.m_finalCompositionShader != null)
			{
				this.m_finalCompositionMaterial = new Material(this.m_finalCompositionShader);
				if (!this.m_finalCompositionMaterial.GetTag(AmplifyUtils.ShaderModeTag, false).Equals(AmplifyUtils.ShaderModeValue))
				{
					if (this.m_showDebugMessages)
					{
						AmplifyUtils.DebugLog("Amplify Bloom is running on a limited hardware and may lead to a decrease on its visual quality.", LogType.Warning);
					}
				}
				else
				{
					this.m_softMaxdownscales = 6;
				}
				this.m_finalCompositionMaterial.hideFlags = HideFlags.DontSave;
				if (this.m_lensDirtTexture == null)
				{
					this.m_lensDirtTexture = this.m_finalCompositionMaterial.GetTexture(AmplifyUtils.LensDirtRTId);
				}
				if (this.m_lensStardurstTex == null)
				{
					this.m_lensStardurstTex = this.m_finalCompositionMaterial.GetTexture(AmplifyUtils.LensStarburstRTId);
				}
			}
			else
			{
				AmplifyUtils.DebugLog("Bloom Composition shader not found", LogType.Error);
				base.gameObject.SetActive(false);
			}
			this.m_camera = base.GetComponent<Camera>();
			this.m_camera.depthTextureMode |= DepthTextureMode.Depth;
			this.m_lensFlare.CreateLUTexture();
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0006D0CC File Offset: 0x0006B2CC
		private void OnDestroy()
		{
			if (this.m_bokehFilter != null)
			{
				this.m_bokehFilter.Destroy();
				this.m_bokehFilter = null;
			}
			if (this.m_anamorphicGlare != null)
			{
				this.m_anamorphicGlare.Destroy();
				this.m_anamorphicGlare = null;
			}
			if (this.m_lensFlare != null)
			{
				this.m_lensFlare.Destroy();
				this.m_lensFlare = null;
			}
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0006D128 File Offset: 0x0006B328
		private void ApplyGaussianBlur(RenderTexture renderTexture, int amount, float radius = 1f, bool applyTemporal = false)
		{
			if (amount == 0)
			{
				return;
			}
			this.m_bloomMaterial.SetFloat(AmplifyUtils.BlurRadiusId, radius);
			RenderTexture tempRenderTarget = AmplifyUtils.GetTempRenderTarget(renderTexture.width, renderTexture.height);
			for (int i = 0; i < amount; i++)
			{
				tempRenderTarget.DiscardContents();
				Graphics.Blit(renderTexture, tempRenderTarget, this.m_bloomMaterial, 14);
				if (this.m_temporalFilteringActive && applyTemporal && i == amount - 1)
				{
					if (this.m_tempFilterBuffer != null && this.m_temporalFilteringActive)
					{
						float num = this.m_temporalFilteringCurve.Evaluate(this.m_temporalFilteringValue);
						this.m_bloomMaterial.SetFloat(AmplifyUtils.TempFilterValueId, num);
						this.m_bloomMaterial.SetTexture(AmplifyUtils.AnamorphicRTS[0], this.m_tempFilterBuffer);
						renderTexture.DiscardContents();
						Graphics.Blit(tempRenderTarget, renderTexture, this.m_bloomMaterial, 16);
					}
					else
					{
						renderTexture.DiscardContents();
						Graphics.Blit(tempRenderTarget, renderTexture, this.m_bloomMaterial, 15);
					}
					bool flag = false;
					if (this.m_tempFilterBuffer != null)
					{
						if (this.m_tempFilterBuffer.format != renderTexture.format || this.m_tempFilterBuffer.width != renderTexture.width || this.m_tempFilterBuffer.height != renderTexture.height)
						{
							this.CleanTempFilterRT();
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						this.CreateTempFilterRT(renderTexture);
					}
					this.m_tempFilterBuffer.DiscardContents();
					Graphics.Blit(renderTexture, this.m_tempFilterBuffer);
				}
				else
				{
					renderTexture.DiscardContents();
					Graphics.Blit(tempRenderTarget, renderTexture, this.m_bloomMaterial, 15);
				}
			}
			AmplifyUtils.ReleaseTempRenderTarget(tempRenderTarget);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0006D2AC File Offset: 0x0006B4AC
		private void CreateTempFilterRT(RenderTexture source)
		{
			if (this.m_tempFilterBuffer != null)
			{
				this.CleanTempFilterRT();
			}
			this.m_tempFilterBuffer = new RenderTexture(source.width, source.height, 0, source.format, AmplifyUtils.CurrentReadWriteMode);
			this.m_tempFilterBuffer.filterMode = AmplifyUtils.CurrentFilterMode;
			this.m_tempFilterBuffer.wrapMode = AmplifyUtils.CurrentWrapMode;
			this.m_tempFilterBuffer.Create();
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0006D31C File Offset: 0x0006B51C
		private void CleanTempFilterRT()
		{
			if (this.m_tempFilterBuffer != null)
			{
				RenderTexture.active = null;
				this.m_tempFilterBuffer.Release();
				Object.DestroyImmediate(this.m_tempFilterBuffer);
				this.m_tempFilterBuffer = null;
			}
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0006D350 File Offset: 0x0006B550
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			if (this.silentError)
			{
				return;
			}
			if (!AmplifyUtils.IsInitialized)
			{
				AmplifyUtils.InitializeIds();
			}
			if (this.m_highPrecision)
			{
				AmplifyUtils.EnsureKeywordEnabled(this.m_bloomMaterial, AmplifyUtils.HighPrecisionKeyword, true);
				AmplifyUtils.EnsureKeywordEnabled(this.m_finalCompositionMaterial, AmplifyUtils.HighPrecisionKeyword, true);
				AmplifyUtils.CurrentRTFormat = RenderTextureFormat.DefaultHDR;
			}
			else
			{
				AmplifyUtils.EnsureKeywordEnabled(this.m_bloomMaterial, AmplifyUtils.HighPrecisionKeyword, false);
				AmplifyUtils.EnsureKeywordEnabled(this.m_finalCompositionMaterial, AmplifyUtils.HighPrecisionKeyword, false);
				AmplifyUtils.CurrentRTFormat = RenderTextureFormat.Default;
			}
			float num = Mathf.Acos(Vector3.Dot(this.m_cameraTransform.right, Vector3.right));
			if (Vector3.Cross(this.m_cameraTransform.right, Vector3.right).y > 0f)
			{
				num = -num;
			}
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (!this.m_highPrecision)
			{
				this.m_bloomRange.y = 1f / this.m_bloomRange.x;
				this.m_bloomMaterial.SetVector(AmplifyUtils.BloomRangeId, this.m_bloomRange);
				this.m_finalCompositionMaterial.SetVector(AmplifyUtils.BloomRangeId, this.m_bloomRange);
			}
			this.m_bloomParams.y = this.m_overallThreshold;
			this.m_bloomMaterial.SetVector(AmplifyUtils.BloomParamsId, this.m_bloomParams);
			this.m_finalCompositionMaterial.SetVector(AmplifyUtils.BloomParamsId, this.m_bloomParams);
			int num2 = 1;
			MainThresholdSizeEnum mainThresholdSize = this.m_mainThresholdSize;
			if (mainThresholdSize != MainThresholdSizeEnum.Half)
			{
				if (mainThresholdSize == MainThresholdSizeEnum.Quarter)
				{
					num2 = 4;
				}
			}
			else
			{
				num2 = 2;
			}
			RenderTexture tempRenderTarget = AmplifyUtils.GetTempRenderTarget(src.width / num2, src.height / num2);
			if (this.m_maskTexture != null)
			{
				this.m_bloomMaterial.SetTexture(AmplifyUtils.MaskTextureId, this.m_maskTexture);
				Graphics.Blit(src, tempRenderTarget, this.m_bloomMaterial, 1);
			}
			else
			{
				Graphics.Blit(src, tempRenderTarget, this.m_bloomMaterial, 0);
			}
			if (this.m_debugToScreen == DebugToScreenEnum.MainThreshold)
			{
				Graphics.Blit(tempRenderTarget, dest, this.m_bloomMaterial, 33);
				AmplifyUtils.ReleaseAllRT();
				return;
			}
			bool flag = true;
			RenderTexture renderTexture3 = tempRenderTarget;
			if (this.m_bloomDownsampleCount > 0)
			{
				flag = false;
				int num3 = tempRenderTarget.width;
				int num4 = tempRenderTarget.height;
				for (int i = 0; i < this.m_bloomDownsampleCount; i++)
				{
					this.m_tempDownsamplesSizes[i].x = (float)num3;
					this.m_tempDownsamplesSizes[i].y = (float)num4;
					num3 = num3 + 1 >> 1;
					num4 = num4 + 1 >> 1;
					this.m_tempAuxDownsampleRTs[i] = AmplifyUtils.GetTempRenderTarget(num3, num4);
					if (i == 0)
					{
						if (!this.m_temporalFilteringActive || this.m_gaussianSteps[i] != 0)
						{
							if (this.m_upscaleQuality == UpscaleQualityEnum.Realistic)
							{
								Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 10);
							}
							else
							{
								Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 11);
							}
						}
						else
						{
							if (this.m_tempFilterBuffer != null && this.m_temporalFilteringActive)
							{
								float num5 = this.m_temporalFilteringCurve.Evaluate(this.m_temporalFilteringValue);
								this.m_bloomMaterial.SetFloat(AmplifyUtils.TempFilterValueId, num5);
								this.m_bloomMaterial.SetTexture(AmplifyUtils.AnamorphicRTS[0], this.m_tempFilterBuffer);
								if (this.m_upscaleQuality == UpscaleQualityEnum.Realistic)
								{
									Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 12);
								}
								else
								{
									Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 13);
								}
							}
							else if (this.m_upscaleQuality == UpscaleQualityEnum.Realistic)
							{
								Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 10);
							}
							else
							{
								Graphics.Blit(renderTexture3, this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 11);
							}
							bool flag2 = false;
							if (this.m_tempFilterBuffer != null)
							{
								if (this.m_tempFilterBuffer.format != this.m_tempAuxDownsampleRTs[i].format || this.m_tempFilterBuffer.width != this.m_tempAuxDownsampleRTs[i].width || this.m_tempFilterBuffer.height != this.m_tempAuxDownsampleRTs[i].height)
								{
									this.CleanTempFilterRT();
									flag2 = true;
								}
							}
							else
							{
								flag2 = true;
							}
							if (flag2)
							{
								this.CreateTempFilterRT(this.m_tempAuxDownsampleRTs[i]);
							}
							this.m_tempFilterBuffer.DiscardContents();
							Graphics.Blit(this.m_tempAuxDownsampleRTs[i], this.m_tempFilterBuffer);
							if (this.m_debugToScreen == DebugToScreenEnum.TemporalFilter)
							{
								Graphics.Blit(this.m_tempAuxDownsampleRTs[i], dest);
								AmplifyUtils.ReleaseAllRT();
								return;
							}
						}
					}
					else
					{
						Graphics.Blit(this.m_tempAuxDownsampleRTs[i - 1], this.m_tempAuxDownsampleRTs[i], this.m_bloomMaterial, 9);
					}
					if (this.m_gaussianSteps[i] > 0)
					{
						this.ApplyGaussianBlur(this.m_tempAuxDownsampleRTs[i], this.m_gaussianSteps[i], this.m_gaussianRadius[i], i == 0);
						if (this.m_temporalFilteringActive && this.m_debugToScreen == DebugToScreenEnum.TemporalFilter)
						{
							Graphics.Blit(this.m_tempAuxDownsampleRTs[i], dest);
							AmplifyUtils.ReleaseAllRT();
							return;
						}
					}
				}
				renderTexture3 = this.m_tempAuxDownsampleRTs[this.m_featuresSourceId];
				AmplifyUtils.ReleaseTempRenderTarget(tempRenderTarget);
			}
			if (this.m_bokehFilter.ApplyBokeh && this.m_bokehFilter.ApplyOnBloomSource)
			{
				this.m_bokehFilter.ApplyBokehFilter(renderTexture3, this.m_bloomMaterial);
				if (this.m_debugToScreen == DebugToScreenEnum.BokehFilter)
				{
					Graphics.Blit(renderTexture3, dest);
					AmplifyUtils.ReleaseAllRT();
					return;
				}
			}
			bool flag3 = false;
			RenderTexture renderTexture4;
			if (this.m_separateFeaturesThreshold)
			{
				this.m_bloomParams.y = this.m_featuresThreshold;
				this.m_bloomMaterial.SetVector(AmplifyUtils.BloomParamsId, this.m_bloomParams);
				this.m_finalCompositionMaterial.SetVector(AmplifyUtils.BloomParamsId, this.m_bloomParams);
				renderTexture4 = AmplifyUtils.GetTempRenderTarget(renderTexture3.width, renderTexture3.height);
				flag3 = true;
				Graphics.Blit(renderTexture3, renderTexture4, this.m_bloomMaterial, 0);
				if (this.m_debugToScreen == DebugToScreenEnum.FeaturesThreshold)
				{
					Graphics.Blit(renderTexture4, dest);
					AmplifyUtils.ReleaseAllRT();
					return;
				}
			}
			else
			{
				renderTexture4 = renderTexture3;
			}
			if (this.m_bokehFilter.ApplyBokeh && !this.m_bokehFilter.ApplyOnBloomSource)
			{
				if (!flag3)
				{
					flag3 = true;
					renderTexture4 = AmplifyUtils.GetTempRenderTarget(renderTexture3.width, renderTexture3.height);
					Graphics.Blit(renderTexture3, renderTexture4);
				}
				this.m_bokehFilter.ApplyBokehFilter(renderTexture4, this.m_bloomMaterial);
				if (this.m_debugToScreen == DebugToScreenEnum.BokehFilter)
				{
					Graphics.Blit(renderTexture4, dest);
					AmplifyUtils.ReleaseAllRT();
					return;
				}
			}
			if (this.m_lensFlare.ApplyLensFlare && this.m_debugToScreen != DebugToScreenEnum.Bloom)
			{
				renderTexture = this.m_lensFlare.ApplyFlare(this.m_bloomMaterial, renderTexture4);
				this.ApplyGaussianBlur(renderTexture, this.m_lensFlare.LensFlareGaussianBlurAmount, 1f, false);
				if (this.m_debugToScreen == DebugToScreenEnum.LensFlare)
				{
					Graphics.Blit(renderTexture, dest);
					AmplifyUtils.ReleaseAllRT();
					return;
				}
			}
			if (this.m_anamorphicGlare.ApplyLensGlare && this.m_debugToScreen != DebugToScreenEnum.Bloom)
			{
				renderTexture2 = AmplifyUtils.GetTempRenderTarget(renderTexture3.width, renderTexture3.height);
				this.m_anamorphicGlare.OnRenderImage(this.m_bloomMaterial, renderTexture4, renderTexture2, num);
				if (this.m_debugToScreen == DebugToScreenEnum.LensGlare)
				{
					Graphics.Blit(renderTexture2, dest);
					AmplifyUtils.ReleaseAllRT();
					return;
				}
			}
			if (flag3)
			{
				AmplifyUtils.ReleaseTempRenderTarget(renderTexture4);
			}
			if (flag)
			{
				this.ApplyGaussianBlur(renderTexture3, this.m_gaussianSteps[0], this.m_gaussianRadius[0], false);
			}
			if (this.m_bloomDownsampleCount > 0)
			{
				if (this.m_bloomDownsampleCount == 1)
				{
					if (this.m_upscaleQuality == UpscaleQualityEnum.Realistic)
					{
						this.ApplyUpscale();
						this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.MipResultsRTS[0], this.m_tempUpscaleRTs[0]);
					}
					else
					{
						this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.MipResultsRTS[0], this.m_tempAuxDownsampleRTs[0]);
					}
					this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.UpscaleWeightsStr[0], this.m_upscaleWeights[0]);
				}
				else if (this.m_upscaleQuality == UpscaleQualityEnum.Realistic)
				{
					this.ApplyUpscale();
					for (int j = 0; j < this.m_bloomDownsampleCount; j++)
					{
						int num6 = this.m_bloomDownsampleCount - j - 1;
						this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.MipResultsRTS[num6], this.m_tempUpscaleRTs[j]);
						this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.UpscaleWeightsStr[num6], this.m_upscaleWeights[j]);
					}
				}
				else
				{
					for (int k = 0; k < this.m_bloomDownsampleCount; k++)
					{
						int num7 = this.m_bloomDownsampleCount - 1 - k;
						this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.MipResultsRTS[num7], this.m_tempAuxDownsampleRTs[num7]);
						this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.UpscaleWeightsStr[num7], this.m_upscaleWeights[k]);
					}
				}
			}
			else
			{
				this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.MipResultsRTS[0], renderTexture3);
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.UpscaleWeightsStr[0], 1f);
			}
			if (this.m_debugToScreen == DebugToScreenEnum.Bloom)
			{
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.SourceContributionId, 0f);
				this.FinalComposition(0f, 1f, src, dest, 0);
				return;
			}
			if (this.m_bloomDownsampleCount > 1)
			{
				for (int l = 0; l < this.m_bloomDownsampleCount; l++)
				{
					this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensDirtWeightsStr[this.m_bloomDownsampleCount - l - 1], this.m_lensDirtWeights[l]);
					this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensStarburstWeightsStr[this.m_bloomDownsampleCount - l - 1], this.m_lensStarburstWeights[l]);
				}
			}
			else
			{
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensDirtWeightsStr[0], this.m_lensDirtWeights[0]);
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensStarburstWeightsStr[0], this.m_lensStarburstWeights[0]);
			}
			if (this.m_lensFlare.ApplyLensFlare)
			{
				this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.LensFlareRTId, renderTexture);
			}
			if (this.m_anamorphicGlare.ApplyLensGlare)
			{
				this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.LensGlareRTId, renderTexture2);
			}
			if (this.m_applyLensDirt)
			{
				this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.LensDirtRTId, this.m_lensDirtTexture);
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensDirtStrengthId, this.m_lensDirtStrength * 1f);
				if (this.m_debugToScreen == DebugToScreenEnum.LensDirt)
				{
					this.FinalComposition(0f, 0f, src, dest, 2);
					return;
				}
			}
			if (this.m_applyLensStardurst)
			{
				this.m_starburstMat[0, 0] = Mathf.Cos(num);
				this.m_starburstMat[0, 1] = -Mathf.Sin(num);
				this.m_starburstMat[1, 0] = Mathf.Sin(num);
				this.m_starburstMat[1, 1] = Mathf.Cos(num);
				this.m_finalCompositionMaterial.SetMatrix(AmplifyUtils.LensFlareStarMatrixId, this.m_starburstMat);
				this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.LensFlareStarburstStrengthId, this.m_lensStarburstStrength * 1f);
				this.m_finalCompositionMaterial.SetTexture(AmplifyUtils.LensStarburstRTId, this.m_lensStardurstTex);
				if (this.m_debugToScreen == DebugToScreenEnum.LensStarburst)
				{
					this.FinalComposition(0f, 0f, src, dest, 1);
					return;
				}
			}
			if (this.m_targetTexture != null)
			{
				this.m_targetTexture.DiscardContents();
				this.FinalComposition(0f, 1f, src, this.m_targetTexture, -1);
				Graphics.Blit(src, dest);
				return;
			}
			this.FinalComposition(1f, 1f, src, dest, -1);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0006DE24 File Offset: 0x0006C024
		private void FinalComposition(float srcContribution, float upscaleContribution, RenderTexture src, RenderTexture dest, int forcePassId)
		{
			this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.SourceContributionId, srcContribution);
			this.m_finalCompositionMaterial.SetFloat(AmplifyUtils.UpscaleContributionId, upscaleContribution);
			int num = 0;
			if (forcePassId > -1)
			{
				num = forcePassId;
			}
			else
			{
				if (this.LensFlareInstance.ApplyLensFlare)
				{
					num |= 8;
				}
				if (this.LensGlareInstance.ApplyLensGlare)
				{
					num |= 4;
				}
				if (this.m_applyLensDirt)
				{
					num |= 2;
				}
				if (this.m_applyLensStardurst)
				{
					num |= 1;
				}
			}
			num += (this.m_bloomDownsampleCount - 1) * 16;
			Graphics.Blit(src, dest, this.m_finalCompositionMaterial, num);
			AmplifyUtils.ReleaseAllRT();
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0006DEBC File Offset: 0x0006C0BC
		private void ApplyUpscale()
		{
			int num = this.m_bloomDownsampleCount - 1;
			int num2 = 0;
			for (int i = num; i > -1; i--)
			{
				this.m_tempUpscaleRTs[num2] = AmplifyUtils.GetTempRenderTarget((int)this.m_tempDownsamplesSizes[i].x, (int)this.m_tempDownsamplesSizes[i].y);
				if (i == num)
				{
					Graphics.Blit(this.m_tempAuxDownsampleRTs[num], this.m_tempUpscaleRTs[num2], this.m_bloomMaterial, 17);
				}
				else
				{
					this.m_bloomMaterial.SetTexture(AmplifyUtils.AnamorphicRTS[0], this.m_tempUpscaleRTs[num2 - 1]);
					Graphics.Blit(this.m_tempAuxDownsampleRTs[i], this.m_tempUpscaleRTs[num2], this.m_bloomMaterial, 18);
				}
				num2++;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x0006DF77 File Offset: 0x0006C177
		public AmplifyGlare LensGlareInstance
		{
			get
			{
				return this.m_anamorphicGlare;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0006DF7F File Offset: 0x0006C17F
		public AmplifyBokeh BokehFilterInstance
		{
			get
			{
				return this.m_bokehFilter;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0006DF87 File Offset: 0x0006C187
		public AmplifyLensFlare LensFlareInstance
		{
			get
			{
				return this.m_lensFlare;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001460 RID: 5216 RVA: 0x0006DF8F File Offset: 0x0006C18F
		// (set) Token: 0x06001461 RID: 5217 RVA: 0x0006DF97 File Offset: 0x0006C197
		public bool ApplyLensDirt
		{
			get
			{
				return this.m_applyLensDirt;
			}
			set
			{
				this.m_applyLensDirt = value;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x0006DFA0 File Offset: 0x0006C1A0
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x0006DFA8 File Offset: 0x0006C1A8
		public float LensDirtStrength
		{
			get
			{
				return this.m_lensDirtStrength;
			}
			set
			{
				this.m_lensDirtStrength = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0006DFC0 File Offset: 0x0006C1C0
		// (set) Token: 0x06001465 RID: 5221 RVA: 0x0006DFC8 File Offset: 0x0006C1C8
		public Texture LensDirtTexture
		{
			get
			{
				return this.m_lensDirtTexture;
			}
			set
			{
				this.m_lensDirtTexture = value;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0006DFD1 File Offset: 0x0006C1D1
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x0006DFD9 File Offset: 0x0006C1D9
		public bool ApplyLensStardurst
		{
			get
			{
				return this.m_applyLensStardurst;
			}
			set
			{
				this.m_applyLensStardurst = value;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0006DFE2 File Offset: 0x0006C1E2
		// (set) Token: 0x06001469 RID: 5225 RVA: 0x0006DFEA File Offset: 0x0006C1EA
		public Texture LensStardurstTex
		{
			get
			{
				return this.m_lensStardurstTex;
			}
			set
			{
				this.m_lensStardurstTex = value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0006DFF3 File Offset: 0x0006C1F3
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x0006DFFB File Offset: 0x0006C1FB
		public float LensStarburstStrength
		{
			get
			{
				return this.m_lensStarburstStrength;
			}
			set
			{
				this.m_lensStarburstStrength = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0006E013 File Offset: 0x0006C213
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x0006E020 File Offset: 0x0006C220
		public PrecisionModes CurrentPrecisionMode
		{
			get
			{
				if (this.m_highPrecision)
				{
					return PrecisionModes.High;
				}
				return PrecisionModes.Low;
			}
			set
			{
				this.HighPrecision = value == PrecisionModes.High;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x0006E02C File Offset: 0x0006C22C
		// (set) Token: 0x0600146F RID: 5231 RVA: 0x0006E034 File Offset: 0x0006C234
		public bool HighPrecision
		{
			get
			{
				return this.m_highPrecision;
			}
			set
			{
				if (this.m_highPrecision != value)
				{
					this.m_highPrecision = value;
					this.CleanTempFilterRT();
				}
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x0006E04C File Offset: 0x0006C24C
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x0006E059 File Offset: 0x0006C259
		public float BloomRange
		{
			get
			{
				return this.m_bloomRange.x;
			}
			set
			{
				this.m_bloomRange.x = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x0006E076 File Offset: 0x0006C276
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x0006E07E File Offset: 0x0006C27E
		public float OverallThreshold
		{
			get
			{
				return this.m_overallThreshold;
			}
			set
			{
				this.m_overallThreshold = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0006E096 File Offset: 0x0006C296
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x0006E09E File Offset: 0x0006C29E
		public Vector4 BloomParams
		{
			get
			{
				return this.m_bloomParams;
			}
			set
			{
				this.m_bloomParams = value;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0006E0A7 File Offset: 0x0006C2A7
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x0006E0B4 File Offset: 0x0006C2B4
		public float OverallIntensity
		{
			get
			{
				return this.m_bloomParams.x;
			}
			set
			{
				this.m_bloomParams.x = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0006E0D1 File Offset: 0x0006C2D1
		// (set) Token: 0x06001479 RID: 5241 RVA: 0x0006E0DE File Offset: 0x0006C2DE
		public float BloomScale
		{
			get
			{
				return this.m_bloomParams.w;
			}
			set
			{
				this.m_bloomParams.w = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0006E0FB File Offset: 0x0006C2FB
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x0006E108 File Offset: 0x0006C308
		public float UpscaleBlurRadius
		{
			get
			{
				return this.m_bloomParams.z;
			}
			set
			{
				this.m_bloomParams.z = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0006E116 File Offset: 0x0006C316
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x0006E11E File Offset: 0x0006C31E
		public bool TemporalFilteringActive
		{
			get
			{
				return this.m_temporalFilteringActive;
			}
			set
			{
				if (this.m_temporalFilteringActive != value)
				{
					this.CleanTempFilterRT();
				}
				this.m_temporalFilteringActive = value;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0006E136 File Offset: 0x0006C336
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x0006E13E File Offset: 0x0006C33E
		public float TemporalFilteringValue
		{
			get
			{
				return this.m_temporalFilteringValue;
			}
			set
			{
				this.m_temporalFilteringValue = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x0006E147 File Offset: 0x0006C347
		public int SoftMaxdownscales
		{
			get
			{
				return this.m_softMaxdownscales;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0006E14F File Offset: 0x0006C34F
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x0006E157 File Offset: 0x0006C357
		public int BloomDownsampleCount
		{
			get
			{
				return this.m_bloomDownsampleCount;
			}
			set
			{
				this.m_bloomDownsampleCount = Mathf.Clamp(value, 1, this.m_softMaxdownscales);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0006E16C File Offset: 0x0006C36C
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x0006E174 File Offset: 0x0006C374
		public int FeaturesSourceId
		{
			get
			{
				return this.m_featuresSourceId;
			}
			set
			{
				this.m_featuresSourceId = Mathf.Clamp(value, 0, this.m_bloomDownsampleCount - 1);
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0006E18B File Offset: 0x0006C38B
		public bool[] DownscaleSettingsFoldout
		{
			get
			{
				return this.m_downscaleSettingsFoldout;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0006E193 File Offset: 0x0006C393
		public float[] UpscaleWeights
		{
			get
			{
				return this.m_upscaleWeights;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0006E19B File Offset: 0x0006C39B
		public float[] LensDirtWeights
		{
			get
			{
				return this.m_lensDirtWeights;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0006E1A3 File Offset: 0x0006C3A3
		public float[] LensStarburstWeights
		{
			get
			{
				return this.m_lensStarburstWeights;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0006E1AB File Offset: 0x0006C3AB
		public float[] GaussianRadius
		{
			get
			{
				return this.m_gaussianRadius;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x0006E1B3 File Offset: 0x0006C3B3
		public int[] GaussianSteps
		{
			get
			{
				return this.m_gaussianSteps;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0006E1BB File Offset: 0x0006C3BB
		// (set) Token: 0x0600148C RID: 5260 RVA: 0x0006E1C3 File Offset: 0x0006C3C3
		public AnimationCurve TemporalFilteringCurve
		{
			get
			{
				return this.m_temporalFilteringCurve;
			}
			set
			{
				this.m_temporalFilteringCurve = value;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0006E1CC File Offset: 0x0006C3CC
		// (set) Token: 0x0600148E RID: 5262 RVA: 0x0006E1D4 File Offset: 0x0006C3D4
		public bool SeparateFeaturesThreshold
		{
			get
			{
				return this.m_separateFeaturesThreshold;
			}
			set
			{
				this.m_separateFeaturesThreshold = value;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0006E1DD File Offset: 0x0006C3DD
		// (set) Token: 0x06001490 RID: 5264 RVA: 0x0006E1E5 File Offset: 0x0006C3E5
		public float FeaturesThreshold
		{
			get
			{
				return this.m_featuresThreshold;
			}
			set
			{
				this.m_featuresThreshold = ((value < 0f) ? 0f : value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0006E1FD File Offset: 0x0006C3FD
		// (set) Token: 0x06001492 RID: 5266 RVA: 0x0006E205 File Offset: 0x0006C405
		public DebugToScreenEnum DebugToScreen
		{
			get
			{
				return this.m_debugToScreen;
			}
			set
			{
				this.m_debugToScreen = value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x0006E20E File Offset: 0x0006C40E
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x0006E216 File Offset: 0x0006C416
		public UpscaleQualityEnum UpscaleQuality
		{
			get
			{
				return this.m_upscaleQuality;
			}
			set
			{
				this.m_upscaleQuality = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x0006E21F File Offset: 0x0006C41F
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x0006E227 File Offset: 0x0006C427
		public bool ShowDebugMessages
		{
			get
			{
				return this.m_showDebugMessages;
			}
			set
			{
				this.m_showDebugMessages = value;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x0006E230 File Offset: 0x0006C430
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x0006E238 File Offset: 0x0006C438
		public MainThresholdSizeEnum MainThresholdSize
		{
			get
			{
				return this.m_mainThresholdSize;
			}
			set
			{
				this.m_mainThresholdSize = value;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x0006E241 File Offset: 0x0006C441
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x0006E249 File Offset: 0x0006C449
		public RenderTexture TargetTexture
		{
			get
			{
				return this.m_targetTexture;
			}
			set
			{
				this.m_targetTexture = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x0006E252 File Offset: 0x0006C452
		// (set) Token: 0x0600149C RID: 5276 RVA: 0x0006E25A File Offset: 0x0006C45A
		public Texture MaskTexture
		{
			get
			{
				return this.m_maskTexture;
			}
			set
			{
				this.m_maskTexture = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x0006E263 File Offset: 0x0006C463
		// (set) Token: 0x0600149E RID: 5278 RVA: 0x0006E270 File Offset: 0x0006C470
		public bool ApplyBokehFilter
		{
			get
			{
				return this.m_bokehFilter.ApplyBokeh;
			}
			set
			{
				this.m_bokehFilter.ApplyBokeh = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x0006E27E File Offset: 0x0006C47E
		// (set) Token: 0x060014A0 RID: 5280 RVA: 0x0006E28B File Offset: 0x0006C48B
		public bool ApplyLensFlare
		{
			get
			{
				return this.m_lensFlare.ApplyLensFlare;
			}
			set
			{
				this.m_lensFlare.ApplyLensFlare = value;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x0006E299 File Offset: 0x0006C499
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x0006E2A6 File Offset: 0x0006C4A6
		public bool ApplyLensGlare
		{
			get
			{
				return this.m_anamorphicGlare.ApplyLensGlare;
			}
			set
			{
				this.m_anamorphicGlare.ApplyLensGlare = value;
			}
		}

		// Token: 0x04001091 RID: 4241
		public const int MaxGhosts = 5;

		// Token: 0x04001092 RID: 4242
		public const int MinDownscales = 1;

		// Token: 0x04001093 RID: 4243
		public const int MaxDownscales = 6;

		// Token: 0x04001094 RID: 4244
		public const int MaxGaussian = 8;

		// Token: 0x04001095 RID: 4245
		private const float MaxDirtIntensity = 1f;

		// Token: 0x04001096 RID: 4246
		private const float MaxStarburstIntensity = 1f;

		// Token: 0x04001097 RID: 4247
		[SerializeField]
		private Texture m_maskTexture;

		// Token: 0x04001098 RID: 4248
		[SerializeField]
		private RenderTexture m_targetTexture;

		// Token: 0x04001099 RID: 4249
		[SerializeField]
		private bool m_showDebugMessages = true;

		// Token: 0x0400109A RID: 4250
		[SerializeField]
		private int m_softMaxdownscales = 6;

		// Token: 0x0400109B RID: 4251
		[SerializeField]
		private DebugToScreenEnum m_debugToScreen;

		// Token: 0x0400109C RID: 4252
		[SerializeField]
		private bool m_highPrecision;

		// Token: 0x0400109D RID: 4253
		[SerializeField]
		private Vector4 m_bloomRange = new Vector4(500f, 1f, 0f, 0f);

		// Token: 0x0400109E RID: 4254
		[SerializeField]
		private float m_overallThreshold = 0.53f;

		// Token: 0x0400109F RID: 4255
		[SerializeField]
		private Vector4 m_bloomParams = new Vector4(0.8f, 1f, 1f, 1f);

		// Token: 0x040010A0 RID: 4256
		[SerializeField]
		private bool m_temporalFilteringActive;

		// Token: 0x040010A1 RID: 4257
		[SerializeField]
		private float m_temporalFilteringValue = 0.05f;

		// Token: 0x040010A2 RID: 4258
		[SerializeField]
		private int m_bloomDownsampleCount = 6;

		// Token: 0x040010A3 RID: 4259
		[SerializeField]
		private AnimationCurve m_temporalFilteringCurve;

		// Token: 0x040010A4 RID: 4260
		[SerializeField]
		private bool m_separateFeaturesThreshold;

		// Token: 0x040010A5 RID: 4261
		[SerializeField]
		private float m_featuresThreshold = 0.05f;

		// Token: 0x040010A6 RID: 4262
		[SerializeField]
		private AmplifyLensFlare m_lensFlare = new AmplifyLensFlare();

		// Token: 0x040010A7 RID: 4263
		[SerializeField]
		private bool m_applyLensDirt = true;

		// Token: 0x040010A8 RID: 4264
		[SerializeField]
		private float m_lensDirtStrength = 2f;

		// Token: 0x040010A9 RID: 4265
		[SerializeField]
		private Texture m_lensDirtTexture;

		// Token: 0x040010AA RID: 4266
		[SerializeField]
		private bool m_applyLensStardurst = true;

		// Token: 0x040010AB RID: 4267
		[SerializeField]
		private Texture m_lensStardurstTex;

		// Token: 0x040010AC RID: 4268
		[SerializeField]
		private float m_lensStarburstStrength = 2f;

		// Token: 0x040010AD RID: 4269
		[SerializeField]
		private AmplifyGlare m_anamorphicGlare = new AmplifyGlare();

		// Token: 0x040010AE RID: 4270
		[SerializeField]
		private AmplifyBokeh m_bokehFilter = new AmplifyBokeh();

		// Token: 0x040010AF RID: 4271
		[SerializeField]
		private float[] m_upscaleWeights = new float[] { 0.0842f, 0.1282f, 0.1648f, 0.2197f, 0.2197f, 0.1831f };

		// Token: 0x040010B0 RID: 4272
		[SerializeField]
		private float[] m_gaussianRadius = new float[] { 1f, 1f, 1f, 1f, 1f, 1f };

		// Token: 0x040010B1 RID: 4273
		[SerializeField]
		private int[] m_gaussianSteps = new int[] { 1, 1, 1, 1, 1, 1 };

		// Token: 0x040010B2 RID: 4274
		[SerializeField]
		private float[] m_lensDirtWeights = new float[] { 0.067f, 0.102f, 0.1311f, 0.1749f, 0.2332f, 0.3f };

		// Token: 0x040010B3 RID: 4275
		[SerializeField]
		private float[] m_lensStarburstWeights = new float[] { 0.067f, 0.102f, 0.1311f, 0.1749f, 0.2332f, 0.3f };

		// Token: 0x040010B4 RID: 4276
		[SerializeField]
		private bool[] m_downscaleSettingsFoldout = new bool[6];

		// Token: 0x040010B5 RID: 4277
		[SerializeField]
		private int m_featuresSourceId;

		// Token: 0x040010B6 RID: 4278
		[SerializeField]
		private UpscaleQualityEnum m_upscaleQuality;

		// Token: 0x040010B7 RID: 4279
		[SerializeField]
		private MainThresholdSizeEnum m_mainThresholdSize;

		// Token: 0x040010B8 RID: 4280
		private Transform m_cameraTransform;

		// Token: 0x040010B9 RID: 4281
		private Matrix4x4 m_starburstMat;

		// Token: 0x040010BA RID: 4282
		private Shader m_bloomShader;

		// Token: 0x040010BB RID: 4283
		private Material m_bloomMaterial;

		// Token: 0x040010BC RID: 4284
		private Shader m_finalCompositionShader;

		// Token: 0x040010BD RID: 4285
		private Material m_finalCompositionMaterial;

		// Token: 0x040010BE RID: 4286
		private RenderTexture m_tempFilterBuffer;

		// Token: 0x040010BF RID: 4287
		private Camera m_camera;

		// Token: 0x040010C0 RID: 4288
		private RenderTexture[] m_tempUpscaleRTs = new RenderTexture[6];

		// Token: 0x040010C1 RID: 4289
		private RenderTexture[] m_tempAuxDownsampleRTs = new RenderTexture[6];

		// Token: 0x040010C2 RID: 4290
		private Vector2[] m_tempDownsamplesSizes = new Vector2[6];

		// Token: 0x040010C3 RID: 4291
		private bool silentError;
	}
}
