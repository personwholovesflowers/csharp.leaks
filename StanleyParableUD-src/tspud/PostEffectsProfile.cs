using System;
using AmplifyBloom;
using AmplifyColor;
using UnityEngine;

// Token: 0x02000080 RID: 128
[CreateAssetMenu(fileName = "New Post Effects Profile", menuName = "Post Effects Control/Create new Post Effects Profile")]
public class PostEffectsProfile : ProfileBase
{
	// Token: 0x06000315 RID: 789 RVA: 0x00015270 File Offset: 0x00013470
	public PostEffectsProfile(AmplifyColorEffect colorSettings, AmplifyBloomEffect bloomSettings)
	{
		if (colorSettings != null)
		{
			this.Data.SetAmplifyColor = true;
			if (colorSettings.LutTexture != null)
			{
				this.Data.LutTexture = colorSettings.LutTexture;
			}
			this.Data.Exposure = colorSettings.Exposure;
			this.Data.Tonemapper = colorSettings.Tonemapper;
		}
		if (bloomSettings != null)
		{
			this.Data.SetAmplifyBloom = true;
			this.Data.BloomIntensity = bloomSettings.OverallIntensity;
			this.Data.BloomThreshold = bloomSettings.OverallThreshold;
		}
		this.Data.SetFog = true;
		this.Data.FogColor = RenderSettings.fogColor;
		this.Data.FogStartDistance = RenderSettings.fogStartDistance;
		this.Data.FogEndDistance = RenderSettings.fogEndDistance;
		this.Data.FogDensity = RenderSettings.fogDensity;
		this.Data.FogMode = RenderSettings.fogMode;
	}

	// Token: 0x04000321 RID: 801
	[SerializeField]
	public PostEffectsProfile.InterpolationData Data;

	// Token: 0x02000383 RID: 899
	[Serializable]
	public struct InterpolationData
	{
		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600163C RID: 5692 RVA: 0x00076470 File Offset: 0x00074670
		// (set) Token: 0x0600163D RID: 5693 RVA: 0x00076478 File Offset: 0x00074678
		public float fogEndDistance { get; internal set; }

		// Token: 0x040012BA RID: 4794
		[Header("Amplify Color")]
		public bool SetAmplifyColor;

		// Token: 0x040012BB RID: 4795
		public Tonemapping Tonemapper;

		// Token: 0x040012BC RID: 4796
		public Texture LutTexture;

		// Token: 0x040012BD RID: 4797
		public float Exposure;

		// Token: 0x040012BE RID: 4798
		public float LinearWhitePoint;

		// Token: 0x040012BF RID: 4799
		public bool SetAmplifyBloom;

		// Token: 0x040012C0 RID: 4800
		public float BloomIntensity;

		// Token: 0x040012C1 RID: 4801
		public float BloomThreshold;

		// Token: 0x040012C2 RID: 4802
		[Header("Fog")]
		public bool SetFog;

		// Token: 0x040012C3 RID: 4803
		public Color FogColor;

		// Token: 0x040012C4 RID: 4804
		public float FogStartDistance;

		// Token: 0x040012C5 RID: 4805
		public float FogEndDistance;

		// Token: 0x040012C6 RID: 4806
		public float FogDensity;

		// Token: 0x040012C7 RID: 4807
		public FogMode FogMode;
	}
}
