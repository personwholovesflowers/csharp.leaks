using System;
using AmplifyBloom;
using UnityEngine;

// Token: 0x0200007E RID: 126
[RequireComponent(typeof(Camera))]
public class PostEffectsCamera : ProfileInterpolator
{
	// Token: 0x1700003C RID: 60
	// (get) Token: 0x06000304 RID: 772 RVA: 0x00014A4E File Offset: 0x00012C4E
	public float ExposureMultipler
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x06000305 RID: 773 RVA: 0x00014A55 File Offset: 0x00012C55
	// (set) Token: 0x06000306 RID: 774 RVA: 0x00014A5D File Offset: 0x00012C5D
	private float AmplifyColor_Exposure
	{
		get
		{
			return this.lastSavedExposure;
		}
		set
		{
			this.lastSavedExposure = value;
			this.amplifyColor.Exposure = this.lastSavedExposure * this.ExposureMultipler;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x00014A7E File Offset: 0x00012C7E
	private void ResetExposure(LiveData ld)
	{
		this.AmplifyColor_Exposure = this.AmplifyColor_Exposure;
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x06000308 RID: 776 RVA: 0x00014A8C File Offset: 0x00012C8C
	// (set) Token: 0x06000309 RID: 777 RVA: 0x00014A94 File Offset: 0x00012C94
	public PostEffectsProfile LastIntorpolatedEffectsProfile { get; private set; }

	// Token: 0x0600030A RID: 778 RVA: 0x00014AA0 File Offset: 0x00012CA0
	private void Awake()
	{
		this.mobileBloom = base.GetComponent<MobileBloom>();
		this.amplifyColor = base.GetComponent<AmplifyColorEffect>();
		this.amplifyBloom = base.GetComponent<AmplifyBloomEffect>();
		base.SetupResetOnLevelLoad(true);
		PostEffectsVolume.OnEnterVolume = (Action<PostEffectsVolume>)Delegate.Combine(PostEffectsVolume.OnEnterVolume, new Action<PostEffectsVolume>(base.OnEnterVolume));
		PostEffectsVolume.OnExitVolume = (Action<PostEffectsVolume>)Delegate.Combine(PostEffectsVolume.OnExitVolume, new Action<PostEffectsVolume>(base.OnExitVolume));
		if (this.brightnessFloat != null)
		{
			FloatConfigurable floatConfigurable = this.brightnessFloat;
			floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(floatConfigurable.OnValueChanged, new Action<LiveData>(this.ResetExposure));
		}
		if (this.defaultProfile != null)
		{
			this.SetProfileInstant(this.defaultProfile);
		}
	}

	// Token: 0x0600030B RID: 779 RVA: 0x00014B68 File Offset: 0x00012D68
	private void OnDestroy()
	{
		base.SetupResetOnLevelLoad(false);
		PostEffectsVolume.OnEnterVolume = (Action<PostEffectsVolume>)Delegate.Remove(PostEffectsVolume.OnEnterVolume, new Action<PostEffectsVolume>(base.OnEnterVolume));
		PostEffectsVolume.OnExitVolume = (Action<PostEffectsVolume>)Delegate.Remove(PostEffectsVolume.OnExitVolume, new Action<PostEffectsVolume>(base.OnExitVolume));
		if (this.brightnessFloat != null)
		{
			FloatConfigurable floatConfigurable = this.brightnessFloat;
			floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(floatConfigurable.OnValueChanged, new Action<LiveData>(this.ResetExposure));
		}
	}

	// Token: 0x0600030C RID: 780 RVA: 0x00014BF4 File Offset: 0x00012DF4
	public override void Interpolate(float lerpValue)
	{
		if (this.endData.SetAmplifyColor)
		{
			this.amplifyColor.BlendAmount = lerpValue;
			this.AmplifyColor_Exposure = Mathf.Lerp(this.startData.Exposure, this.endData.Exposure, lerpValue);
			this.amplifyColor.Tonemapper = this.endData.Tonemapper;
			this.amplifyColor.LinearWhitePoint = Mathf.Lerp(this.startData.LinearWhitePoint, this.endData.LinearWhitePoint, lerpValue);
		}
		if (this.endData.SetFog)
		{
			RenderSettings.fog = true;
			RenderSettings.fogMode = this.endData.FogMode;
			RenderSettings.fogColor = Color.Lerp(this.startData.FogColor, this.endData.FogColor, lerpValue);
			RenderSettings.fogStartDistance = Mathf.Lerp(this.startData.FogStartDistance, this.endData.FogStartDistance, lerpValue);
			RenderSettings.fogEndDistance = Mathf.Lerp(this.startData.FogEndDistance, this.endData.FogEndDistance, lerpValue);
			RenderSettings.fogDensity = Mathf.Lerp(this.startData.FogStartDistance, this.endData.FogEndDistance, lerpValue);
		}
		if (this.endData.SetAmplifyBloom)
		{
			this.amplifyBloom.OverallIntensity = Mathf.Lerp(this.startData.BloomIntensity, this.endData.BloomIntensity, lerpValue);
			this.amplifyBloom.OverallThreshold = Mathf.Lerp(this.startData.BloomThreshold, this.endData.BloomThreshold, lerpValue);
			if (this.mobileBloom != null)
			{
				this.mobileBloom.BloomAmount = Mathf.Lerp(this.startData.BloomIntensity * this.mobileBloomIntensityMultiplier, this.endData.BloomIntensity * this.mobileBloomIntensityMultiplier, lerpValue);
			}
			if (this.mobileBloom != null)
			{
				this.mobileBloom.BloomThreshold = Mathf.Lerp(this.startData.BloomThreshold, this.endData.BloomThreshold, lerpValue);
			}
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00014DF9 File Offset: 0x00012FF9
	public override void LinearInterpolationComplete()
	{
		this.amplifyColor.LutBlendTexture = null;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00014E08 File Offset: 0x00013008
	public override void InterpolateToProfile(ProfileBase profile, float duration, AnimationCurve curve)
	{
		PostEffectsProfile postEffectsProfile = profile as PostEffectsProfile;
		this.LastIntorpolatedEffectsProfile = postEffectsProfile;
		if (this.interpolationRoutine != null)
		{
			base.StopCoroutine(this.interpolationRoutine);
			this.interpolationRoutine = null;
		}
		if (duration == 0f)
		{
			this.SetProfileInstant(profile);
			return;
		}
		if (postEffectsProfile.Data.SetAmplifyColor)
		{
			if (this.amplifyColor.LutBlendTexture != null)
			{
				this.amplifyColor.LutTexture = this.amplifyColor.LutBlendTexture;
			}
			if (postEffectsProfile.Data.LutTexture != null)
			{
				this.amplifyColor.LutBlendTexture = postEffectsProfile.Data.LutTexture;
			}
			this.amplifyColor.BlendAmount = 0f;
		}
		this.startData.Exposure = this.AmplifyColor_Exposure;
		this.startData.LinearWhitePoint = this.amplifyColor.LinearWhitePoint;
		this.startData.BloomIntensity = this.amplifyBloom.OverallIntensity;
		this.startData.BloomThreshold = this.amplifyBloom.OverallThreshold;
		this.startData.FogColor = RenderSettings.fogColor;
		this.startData.FogStartDistance = RenderSettings.fogStartDistance;
		this.startData.FogEndDistance = RenderSettings.fogEndDistance;
		this.startData.FogDensity = RenderSettings.fogDensity;
		this.startData.FogMode = RenderSettings.fogMode;
		this.endData = postEffectsProfile.Data;
		this.interpolationRoutine = base.StartCoroutine(base.LinearInterpolation(duration, curve));
	}

	// Token: 0x0600030F RID: 783 RVA: 0x00014F80 File Offset: 0x00013180
	public override void SetProfileInstant(ProfileBase profile)
	{
		PostEffectsProfile postEffectsProfile = profile as PostEffectsProfile;
		if (postEffectsProfile.Data.SetAmplifyColor)
		{
			this.amplifyColor.BlendAmount = 0f;
			this.amplifyColor.LutTexture = postEffectsProfile.Data.LutTexture;
			this.AmplifyColor_Exposure = postEffectsProfile.Data.Exposure;
			this.amplifyColor.LinearWhitePoint = postEffectsProfile.Data.LinearWhitePoint;
		}
		if (postEffectsProfile.Data.SetFog)
		{
			RenderSettings.fogColor = postEffectsProfile.Data.FogColor;
			RenderSettings.fogStartDistance = postEffectsProfile.Data.FogStartDistance;
			RenderSettings.fogEndDistance = postEffectsProfile.Data.FogEndDistance;
			RenderSettings.fogDensity = postEffectsProfile.Data.FogDensity;
			RenderSettings.fogMode = postEffectsProfile.Data.FogMode;
		}
		if (postEffectsProfile.Data.SetAmplifyBloom)
		{
			this.amplifyBloom.OverallIntensity = postEffectsProfile.Data.BloomIntensity;
			this.amplifyBloom.OverallThreshold = postEffectsProfile.Data.BloomThreshold;
			if (this.mobileBloom != null)
			{
				this.mobileBloom.BloomAmount = postEffectsProfile.Data.BloomIntensity * this.mobileBloomIntensityMultiplier;
			}
			if (this.mobileBloom != null)
			{
				this.mobileBloom.BloomThreshold = postEffectsProfile.Data.BloomThreshold;
			}
		}
		if (this.amplifyColor != null)
		{
			this.amplifyColor.LutBlendTexture = null;
		}
		if (this.currentProfile != null)
		{
			this.previousProfile = this.currentProfile;
		}
		this.currentProfile = postEffectsProfile;
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00015110 File Offset: 0x00013310
	public override void FeatherToProfile(ProfileBase profile, VolumeBase volume, AnimationCurve curve)
	{
		PostEffectsProfile postEffectsProfile = profile as PostEffectsProfile;
		if (this.interpolationRoutine != null)
		{
			base.StopCoroutine(this.interpolationRoutine);
			this.interpolationRoutine = null;
		}
		if (postEffectsProfile.Data.SetAmplifyColor)
		{
			if (this.amplifyColor.LutBlendTexture != null)
			{
				this.amplifyColor.LutTexture = this.amplifyColor.LutBlendTexture;
			}
			if (postEffectsProfile.Data.LutTexture != null)
			{
				this.amplifyColor.LutBlendTexture = postEffectsProfile.Data.LutTexture;
			}
			this.amplifyColor.BlendAmount = 0f;
		}
		this.startData.Exposure = this.AmplifyColor_Exposure;
		this.startData.BloomIntensity = this.amplifyBloom.OverallIntensity;
		this.startData.BloomThreshold = this.amplifyBloom.OverallThreshold;
		this.startData.FogColor = RenderSettings.fogColor;
		this.startData.FogStartDistance = RenderSettings.fogStartDistance;
		this.startData.FogEndDistance = RenderSettings.fogEndDistance;
		this.startData.FogDensity = RenderSettings.fogDensity;
		this.startData.FogMode = RenderSettings.fogMode;
		this.endData = postEffectsProfile.Data;
		this.interpolationRoutine = base.StartCoroutine(base.DistanceBasedInterpolation(volume, curve));
	}

	// Token: 0x04000318 RID: 792
	private AmplifyColorEffect amplifyColor;

	// Token: 0x04000319 RID: 793
	private AmplifyBloomEffect amplifyBloom;

	// Token: 0x0400031A RID: 794
	private MobileBloom mobileBloom;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private float mobileBloomIntensityMultiplier = 2f;

	// Token: 0x0400031C RID: 796
	public FloatConfigurable brightnessFloat;

	// Token: 0x0400031D RID: 797
	private float lastSavedExposure;

	// Token: 0x0400031E RID: 798
	private PostEffectsProfile.InterpolationData startData;

	// Token: 0x0400031F RID: 799
	private PostEffectsProfile.InterpolationData endData;
}
