using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x0200033A RID: 826
	public class AmplifyUtils
	{
		// Token: 0x06001538 RID: 5432 RVA: 0x00070264 File Offset: 0x0006E464
		public static void InitializeIds()
		{
			AmplifyUtils.IsInitialized = true;
			AmplifyUtils.MaskTextureId = Shader.PropertyToID("_MaskTex");
			AmplifyUtils.MipResultsRTS = new int[]
			{
				Shader.PropertyToID("_MipResultsRTS0"),
				Shader.PropertyToID("_MipResultsRTS1"),
				Shader.PropertyToID("_MipResultsRTS2"),
				Shader.PropertyToID("_MipResultsRTS3"),
				Shader.PropertyToID("_MipResultsRTS4"),
				Shader.PropertyToID("_MipResultsRTS5")
			};
			AmplifyUtils.AnamorphicRTS = new int[]
			{
				Shader.PropertyToID("_AnamorphicRTS0"),
				Shader.PropertyToID("_AnamorphicRTS1"),
				Shader.PropertyToID("_AnamorphicRTS2"),
				Shader.PropertyToID("_AnamorphicRTS3"),
				Shader.PropertyToID("_AnamorphicRTS4"),
				Shader.PropertyToID("_AnamorphicRTS5"),
				Shader.PropertyToID("_AnamorphicRTS6"),
				Shader.PropertyToID("_AnamorphicRTS7")
			};
			AmplifyUtils.AnamorphicGlareWeightsMatStr = new int[]
			{
				Shader.PropertyToID("_AnamorphicGlareWeightsMat0"),
				Shader.PropertyToID("_AnamorphicGlareWeightsMat1"),
				Shader.PropertyToID("_AnamorphicGlareWeightsMat2"),
				Shader.PropertyToID("_AnamorphicGlareWeightsMat3")
			};
			AmplifyUtils.AnamorphicGlareOffsetsMatStr = new int[]
			{
				Shader.PropertyToID("_AnamorphicGlareOffsetsMat0"),
				Shader.PropertyToID("_AnamorphicGlareOffsetsMat1"),
				Shader.PropertyToID("_AnamorphicGlareOffsetsMat2"),
				Shader.PropertyToID("_AnamorphicGlareOffsetsMat3")
			};
			AmplifyUtils.AnamorphicGlareWeightsStr = new int[]
			{
				Shader.PropertyToID("_AnamorphicGlareWeights0"),
				Shader.PropertyToID("_AnamorphicGlareWeights1"),
				Shader.PropertyToID("_AnamorphicGlareWeights2"),
				Shader.PropertyToID("_AnamorphicGlareWeights3"),
				Shader.PropertyToID("_AnamorphicGlareWeights4"),
				Shader.PropertyToID("_AnamorphicGlareWeights5"),
				Shader.PropertyToID("_AnamorphicGlareWeights6"),
				Shader.PropertyToID("_AnamorphicGlareWeights7"),
				Shader.PropertyToID("_AnamorphicGlareWeights8"),
				Shader.PropertyToID("_AnamorphicGlareWeights9"),
				Shader.PropertyToID("_AnamorphicGlareWeights10"),
				Shader.PropertyToID("_AnamorphicGlareWeights11"),
				Shader.PropertyToID("_AnamorphicGlareWeights12"),
				Shader.PropertyToID("_AnamorphicGlareWeights13"),
				Shader.PropertyToID("_AnamorphicGlareWeights14"),
				Shader.PropertyToID("_AnamorphicGlareWeights15")
			};
			AmplifyUtils.UpscaleWeightsStr = new int[]
			{
				Shader.PropertyToID("_UpscaleWeights0"),
				Shader.PropertyToID("_UpscaleWeights1"),
				Shader.PropertyToID("_UpscaleWeights2"),
				Shader.PropertyToID("_UpscaleWeights3"),
				Shader.PropertyToID("_UpscaleWeights4"),
				Shader.PropertyToID("_UpscaleWeights5"),
				Shader.PropertyToID("_UpscaleWeights6"),
				Shader.PropertyToID("_UpscaleWeights7")
			};
			AmplifyUtils.LensDirtWeightsStr = new int[]
			{
				Shader.PropertyToID("_LensDirtWeights0"),
				Shader.PropertyToID("_LensDirtWeights1"),
				Shader.PropertyToID("_LensDirtWeights2"),
				Shader.PropertyToID("_LensDirtWeights3"),
				Shader.PropertyToID("_LensDirtWeights4"),
				Shader.PropertyToID("_LensDirtWeights5"),
				Shader.PropertyToID("_LensDirtWeights6"),
				Shader.PropertyToID("_LensDirtWeights7")
			};
			AmplifyUtils.LensStarburstWeightsStr = new int[]
			{
				Shader.PropertyToID("_LensStarburstWeights0"),
				Shader.PropertyToID("_LensStarburstWeights1"),
				Shader.PropertyToID("_LensStarburstWeights2"),
				Shader.PropertyToID("_LensStarburstWeights3"),
				Shader.PropertyToID("_LensStarburstWeights4"),
				Shader.PropertyToID("_LensStarburstWeights5"),
				Shader.PropertyToID("_LensStarburstWeights6"),
				Shader.PropertyToID("_LensStarburstWeights7")
			};
			AmplifyUtils.BloomRangeId = Shader.PropertyToID("_BloomRange");
			AmplifyUtils.LensDirtStrengthId = Shader.PropertyToID("_LensDirtStrength");
			AmplifyUtils.BloomParamsId = Shader.PropertyToID("_BloomParams");
			AmplifyUtils.TempFilterValueId = Shader.PropertyToID("_TempFilterValue");
			AmplifyUtils.LensFlareStarMatrixId = Shader.PropertyToID("_LensFlareStarMatrix");
			AmplifyUtils.LensFlareStarburstStrengthId = Shader.PropertyToID("_LensFlareStarburstStrength");
			AmplifyUtils.LensFlareGhostsParamsId = Shader.PropertyToID("_LensFlareGhostsParams");
			AmplifyUtils.LensFlareLUTId = Shader.PropertyToID("_LensFlareLUT");
			AmplifyUtils.LensFlareHaloParamsId = Shader.PropertyToID("_LensFlareHaloParams");
			AmplifyUtils.LensFlareGhostChrDistortionId = Shader.PropertyToID("_LensFlareGhostChrDistortion");
			AmplifyUtils.LensFlareHaloChrDistortionId = Shader.PropertyToID("_LensFlareHaloChrDistortion");
			AmplifyUtils.BokehParamsId = Shader.PropertyToID("_BokehParams");
			AmplifyUtils.BlurRadiusId = Shader.PropertyToID("_BlurRadius");
			AmplifyUtils.LensStarburstRTId = Shader.PropertyToID("_LensStarburst");
			AmplifyUtils.LensDirtRTId = Shader.PropertyToID("_LensDirt");
			AmplifyUtils.LensFlareRTId = Shader.PropertyToID("_LensFlare");
			AmplifyUtils.LensGlareRTId = Shader.PropertyToID("_LensGlare");
			AmplifyUtils.SourceContributionId = Shader.PropertyToID("_SourceContribution");
			AmplifyUtils.UpscaleContributionId = Shader.PropertyToID("_UpscaleContribution");
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x0007072C File Offset: 0x0006E92C
		public static void DebugLog(string value, LogType type)
		{
			switch (type)
			{
			case LogType.Normal:
				Debug.Log(AmplifyUtils.DebugStr + value);
				return;
			case LogType.Warning:
				Debug.LogWarning(AmplifyUtils.DebugStr + value);
				return;
			case LogType.Error:
				Debug.LogError(AmplifyUtils.DebugStr + value);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00070780 File Offset: 0x0006E980
		public static RenderTexture GetTempRenderTarget(int width, int height)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, AmplifyUtils.CurrentRTFormat, AmplifyUtils.CurrentReadWriteMode);
			temporary.filterMode = AmplifyUtils.CurrentFilterMode;
			temporary.wrapMode = AmplifyUtils.CurrentWrapMode;
			AmplifyUtils._allocatedRT.Add(temporary);
			return temporary;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x000707C2 File Offset: 0x0006E9C2
		public static void ReleaseTempRenderTarget(RenderTexture renderTarget)
		{
			if (renderTarget != null && AmplifyUtils._allocatedRT.Remove(renderTarget))
			{
				renderTarget.DiscardContents();
				RenderTexture.ReleaseTemporary(renderTarget);
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x000707E8 File Offset: 0x0006E9E8
		public static void ReleaseAllRT()
		{
			for (int i = 0; i < AmplifyUtils._allocatedRT.Count; i++)
			{
				AmplifyUtils._allocatedRT[i].DiscardContents();
				RenderTexture.ReleaseTemporary(AmplifyUtils._allocatedRT[i]);
			}
			AmplifyUtils._allocatedRT.Clear();
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00070834 File Offset: 0x0006EA34
		public static void EnsureKeywordEnabled(Material mat, string keyword, bool state)
		{
			if (mat != null)
			{
				if (state && !mat.IsKeywordEnabled(keyword))
				{
					mat.EnableKeyword(keyword);
					return;
				}
				if (!state && mat.IsKeywordEnabled(keyword))
				{
					mat.DisableKeyword(keyword);
				}
			}
		}

		// Token: 0x0400112F RID: 4399
		public static int MaskTextureId;

		// Token: 0x04001130 RID: 4400
		public static int BlurRadiusId;

		// Token: 0x04001131 RID: 4401
		public static string HighPrecisionKeyword = "AB_HIGH_PRECISION";

		// Token: 0x04001132 RID: 4402
		public static string ShaderModeTag = "Mode";

		// Token: 0x04001133 RID: 4403
		public static string ShaderModeValue = "Full";

		// Token: 0x04001134 RID: 4404
		public static string DebugStr = "[AmplifyBloom] ";

		// Token: 0x04001135 RID: 4405
		public static int UpscaleContributionId;

		// Token: 0x04001136 RID: 4406
		public static int SourceContributionId;

		// Token: 0x04001137 RID: 4407
		public static int LensStarburstRTId;

		// Token: 0x04001138 RID: 4408
		public static int LensDirtRTId;

		// Token: 0x04001139 RID: 4409
		public static int LensFlareRTId;

		// Token: 0x0400113A RID: 4410
		public static int LensGlareRTId;

		// Token: 0x0400113B RID: 4411
		public static int[] MipResultsRTS;

		// Token: 0x0400113C RID: 4412
		public static int[] AnamorphicRTS;

		// Token: 0x0400113D RID: 4413
		public static int[] AnamorphicGlareWeightsMatStr;

		// Token: 0x0400113E RID: 4414
		public static int[] AnamorphicGlareOffsetsMatStr;

		// Token: 0x0400113F RID: 4415
		public static int[] AnamorphicGlareWeightsStr;

		// Token: 0x04001140 RID: 4416
		public static int[] UpscaleWeightsStr;

		// Token: 0x04001141 RID: 4417
		public static int[] LensDirtWeightsStr;

		// Token: 0x04001142 RID: 4418
		public static int[] LensStarburstWeightsStr;

		// Token: 0x04001143 RID: 4419
		public static int BloomRangeId;

		// Token: 0x04001144 RID: 4420
		public static int LensDirtStrengthId;

		// Token: 0x04001145 RID: 4421
		public static int BloomParamsId;

		// Token: 0x04001146 RID: 4422
		public static int TempFilterValueId;

		// Token: 0x04001147 RID: 4423
		public static int LensFlareStarMatrixId;

		// Token: 0x04001148 RID: 4424
		public static int LensFlareStarburstStrengthId;

		// Token: 0x04001149 RID: 4425
		public static int LensFlareGhostsParamsId;

		// Token: 0x0400114A RID: 4426
		public static int LensFlareLUTId;

		// Token: 0x0400114B RID: 4427
		public static int LensFlareHaloParamsId;

		// Token: 0x0400114C RID: 4428
		public static int LensFlareGhostChrDistortionId;

		// Token: 0x0400114D RID: 4429
		public static int LensFlareHaloChrDistortionId;

		// Token: 0x0400114E RID: 4430
		public static int BokehParamsId = -1;

		// Token: 0x0400114F RID: 4431
		public static RenderTextureFormat CurrentRTFormat = RenderTextureFormat.DefaultHDR;

		// Token: 0x04001150 RID: 4432
		public static FilterMode CurrentFilterMode = FilterMode.Bilinear;

		// Token: 0x04001151 RID: 4433
		public static TextureWrapMode CurrentWrapMode = TextureWrapMode.Clamp;

		// Token: 0x04001152 RID: 4434
		public static RenderTextureReadWrite CurrentReadWriteMode = RenderTextureReadWrite.sRGB;

		// Token: 0x04001153 RID: 4435
		public static bool IsInitialized = false;

		// Token: 0x04001154 RID: 4436
		private static List<RenderTexture> _allocatedRT = new List<RenderTexture>();
	}
}
