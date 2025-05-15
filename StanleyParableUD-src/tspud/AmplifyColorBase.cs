using System;
using System.Collections.Generic;
using AmplifyColor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x02000002 RID: 2
[AddComponentMenu("")]
public class AmplifyColorBase : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public Texture2D DefaultLut
	{
		get
		{
			if (!(this.defaultLut == null))
			{
				return this.defaultLut;
			}
			return this.CreateDefaultLut();
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206D File Offset: 0x0000026D
	public bool IsBlending
	{
		get
		{
			return this.blending;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00002075 File Offset: 0x00000275
	private float effectVolumesBlendAdjusted
	{
		get
		{
			return Mathf.Clamp01((this.effectVolumesBlendAdjust < 0.99f) ? ((this.volumesBlendAmount - this.effectVolumesBlendAdjust) / (1f - this.effectVolumesBlendAdjust)) : 1f);
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000004 RID: 4 RVA: 0x000020AA File Offset: 0x000002AA
	public string SharedInstanceID
	{
		get
		{
			return this.sharedInstanceID;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000005 RID: 5 RVA: 0x000020B2 File Offset: 0x000002B2
	public bool WillItBlend
	{
		get
		{
			return this.LutTexture != null && this.LutBlendTexture != null && !this.blending;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020DC File Offset: 0x000002DC
	public void NewSharedInstanceID()
	{
		this.sharedInstanceID = Guid.NewGuid().ToString();
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002102 File Offset: 0x00000302
	private void ReportMissingShaders()
	{
		Debug.LogError("[AmplifyColor] Failed to initialize shaders. Please attempt to re-enable the Amplify Color Effect component. If that fails, please reinstall Amplify Color.");
		base.enabled = false;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002115 File Offset: 0x00000315
	private void ReportNotSupported()
	{
		Debug.LogError("[AmplifyColor] This image effect is not supported on this platform.");
		base.enabled = false;
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00002128 File Offset: 0x00000328
	private bool CheckShader(Shader s)
	{
		if (s == null)
		{
			this.ReportMissingShaders();
			return false;
		}
		if (!s.isSupported)
		{
			this.ReportNotSupported();
			return false;
		}
		return true;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x0000214C File Offset: 0x0000034C
	private bool CheckShaders()
	{
		return this.CheckShader(this.shaderBase) && this.CheckShader(this.shaderBlend) && this.CheckShader(this.shaderBlendCache) && this.CheckShader(this.shaderMask) && this.CheckShader(this.shaderMaskBlend) && this.CheckShader(this.shaderProcessOnly);
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000021AD File Offset: 0x000003AD
	private bool CheckSupport()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			this.ReportNotSupported();
			return false;
		}
		return true;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000021C0 File Offset: 0x000003C0
	private void OnEnable()
	{
		if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null)
		{
			Debug.LogWarning("[AmplifyColor] Null graphics device detected. Skipping effect silently.");
			this.silentError = true;
			return;
		}
		if (!this.CheckSupport())
		{
			return;
		}
		if (!this.CreateMaterials())
		{
			return;
		}
		Texture2D texture2D = this.LutTexture as Texture2D;
		Texture2D texture2D2 = this.LutBlendTexture as Texture2D;
		if ((texture2D != null && texture2D.mipmapCount > 1) || (texture2D2 != null && texture2D2.mipmapCount > 1))
		{
			Debug.LogError("[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
		}
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002241 File Offset: 0x00000441
	private void OnDisable()
	{
		if (this.actualTriggerProxy != null)
		{
			Object.DestroyImmediate(this.actualTriggerProxy.gameObject);
			this.actualTriggerProxy = null;
		}
		this.ReleaseMaterials();
		this.ReleaseTextures();
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002274 File Offset: 0x00000474
	private void VolumesBlendTo(Texture blendTargetLUT, float blendTimeInSec)
	{
		this.volumesLutBlendTexture = blendTargetLUT;
		this.volumesBlendAmount = 0f;
		this.volumesBlendingTime = blendTimeInSec;
		this.volumesBlendingTimeCountdown = blendTimeInSec;
		this.volumesBlending = true;
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000229D File Offset: 0x0000049D
	public void BlendTo(Texture blendTargetLUT, float blendTimeInSec, Action onFinishBlend)
	{
		this.LutBlendTexture = blendTargetLUT;
		this.BlendAmount = 0f;
		this.onFinishBlend = onFinishBlend;
		this.blendingTime = blendTimeInSec;
		this.blendingTimeCountdown = blendTimeInSec;
		this.blending = true;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000022D0 File Offset: 0x000004D0
	private void CheckCamera()
	{
		if (this.ownerCamera == null)
		{
			this.ownerCamera = base.GetComponent<Camera>();
		}
		if (this.UseDepthMask && (this.ownerCamera.depthTextureMode & DepthTextureMode.Depth) == DepthTextureMode.None)
		{
			this.ownerCamera.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002324 File Offset: 0x00000524
	private void Start()
	{
		if (this.silentError)
		{
			return;
		}
		this.CheckCamera();
		this.worldLUT = this.LutTexture;
		this.worldVolumeEffects = this.EffectFlags.GenerateEffectData(this);
		this.blendVolumeEffects = (this.currentVolumeEffects = this.worldVolumeEffects);
		this.worldExposure = this.Exposure;
		this.blendExposure = (this.currentExposure = this.worldExposure);
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002394 File Offset: 0x00000594
	public void EnterVolume(AmplifyColorVolumeBase volume)
	{
		if (!this.enteredVolumes.Contains(volume))
		{
			this.enteredVolumes.Insert(0, volume);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000023B1 File Offset: 0x000005B1
	public void ExitVolume(AmplifyColorVolumeBase volume)
	{
		if (this.enteredVolumes.Contains(volume))
		{
			this.enteredVolumes.Remove(volume);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000023D0 File Offset: 0x000005D0
	private void UpdateVolumes()
	{
		if (this.volumesBlending)
		{
			this.currentVolumeEffects.BlendValues(this, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
		}
		if (this.volumesBlending)
		{
			this.Exposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
		}
		Transform transform = ((this.TriggerVolumeProxy == null) ? base.transform : this.TriggerVolumeProxy);
		if (this.actualTriggerProxy.transform.parent != transform)
		{
			this.actualTriggerProxy.Reference = transform;
			this.actualTriggerProxy.gameObject.layer = transform.gameObject.layer;
		}
		AmplifyColorVolumeBase amplifyColorVolumeBase = null;
		int num = int.MinValue;
		for (int i = 0; i < this.enteredVolumes.Count; i++)
		{
			AmplifyColorVolumeBase amplifyColorVolumeBase2 = this.enteredVolumes[i];
			if (amplifyColorVolumeBase2.Priority > num)
			{
				amplifyColorVolumeBase = amplifyColorVolumeBase2;
				num = amplifyColorVolumeBase2.Priority;
			}
		}
		if (amplifyColorVolumeBase != this.currentVolumeLut)
		{
			this.currentVolumeLut = amplifyColorVolumeBase;
			Texture texture = ((amplifyColorVolumeBase == null) ? this.worldLUT : amplifyColorVolumeBase.LutTexture);
			float num2 = ((amplifyColorVolumeBase == null) ? this.ExitVolumeBlendTime : amplifyColorVolumeBase.EnterBlendTime);
			if (this.volumesBlending && !this.blendingFromMidBlend && texture == this.LutTexture)
			{
				this.LutTexture = this.volumesLutBlendTexture;
				this.volumesLutBlendTexture = texture;
				this.volumesBlendingTimeCountdown = num2 * ((this.volumesBlendingTime - this.volumesBlendingTimeCountdown) / this.volumesBlendingTime);
				this.volumesBlendingTime = num2;
				this.currentVolumeEffects = VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
				this.currentExposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
				this.effectVolumesBlendAdjust = 1f - this.volumesBlendAmount;
				this.volumesBlendAmount = 1f - this.volumesBlendAmount;
			}
			else
			{
				if (this.volumesBlending)
				{
					this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
					if (this.blendingFromMidBlend)
					{
						Graphics.Blit(this.midBlendLUT, this.blendCacheLut);
						this.materialBlendCache.SetTexture("_RgbTex", this.blendCacheLut);
					}
					else
					{
						this.materialBlendCache.SetTexture("_RgbTex", this.LutTexture);
					}
					this.materialBlendCache.SetTexture("_LerpRgbTex", (this.volumesLutBlendTexture != null) ? this.volumesLutBlendTexture : this.defaultLut);
					Graphics.Blit(this.midBlendLUT, this.midBlendLUT, this.materialBlendCache);
					this.blendCacheLut.DiscardContents();
					this.currentVolumeEffects = VolumeEffect.BlendValuesToVolumeEffect(this.EffectFlags, this.currentVolumeEffects, this.blendVolumeEffects, this.effectVolumesBlendAdjusted);
					this.currentExposure = Mathf.Lerp(this.currentExposure, this.blendExposure, this.effectVolumesBlendAdjusted);
					this.effectVolumesBlendAdjust = 0f;
					this.blendingFromMidBlend = true;
				}
				this.VolumesBlendTo(texture, num2);
			}
			this.blendVolumeEffects = ((amplifyColorVolumeBase == null) ? this.worldVolumeEffects : amplifyColorVolumeBase.EffectContainer.FindVolumeEffect(this));
			this.blendExposure = ((amplifyColorVolumeBase == null) ? this.worldExposure : amplifyColorVolumeBase.Exposure);
			if (this.blendVolumeEffects == null)
			{
				this.blendVolumeEffects = this.worldVolumeEffects;
			}
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000273C File Offset: 0x0000093C
	private void SetupShader()
	{
		this.colorSpace = QualitySettings.activeColorSpace;
		this.qualityLevel = this.QualityLevel;
		this.shaderBase = Shader.Find("Hidden/Amplify Color/Base");
		this.shaderBlend = Shader.Find("Hidden/Amplify Color/Blend");
		this.shaderBlendCache = Shader.Find("Hidden/Amplify Color/BlendCache");
		this.shaderMask = Shader.Find("Hidden/Amplify Color/Mask");
		this.shaderMaskBlend = Shader.Find("Hidden/Amplify Color/MaskBlend");
		this.shaderDepthMask = Shader.Find("Hidden/Amplify Color/DepthMask");
		this.shaderDepthMaskBlend = Shader.Find("Hidden/Amplify Color/DepthMaskBlend");
		this.shaderProcessOnly = Shader.Find("Hidden/Amplify Color/ProcessOnly");
	}

	// Token: 0x06000016 RID: 22 RVA: 0x000027E0 File Offset: 0x000009E0
	private void ReleaseMaterials()
	{
		this.SafeRelease<Material>(ref this.materialBase);
		this.SafeRelease<Material>(ref this.materialBlend);
		this.SafeRelease<Material>(ref this.materialBlendCache);
		this.SafeRelease<Material>(ref this.materialMask);
		this.SafeRelease<Material>(ref this.materialMaskBlend);
		this.SafeRelease<Material>(ref this.materialDepthMask);
		this.SafeRelease<Material>(ref this.materialDepthMaskBlend);
		this.SafeRelease<Material>(ref this.materialProcessOnly);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002850 File Offset: 0x00000A50
	private Texture2D CreateDefaultLut()
	{
		this.defaultLut = new Texture2D(1024, 32, TextureFormat.RGB24, false, true)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		this.defaultLut.name = "DefaultLut";
		this.defaultLut.hideFlags = HideFlags.DontSave;
		this.defaultLut.anisoLevel = 1;
		this.defaultLut.filterMode = FilterMode.Bilinear;
		Color32[] array = new Color32[32768];
		for (int i = 0; i < 32; i++)
		{
			int num = i * 32;
			for (int j = 0; j < 32; j++)
			{
				int num2 = num + j * 1024;
				for (int k = 0; k < 32; k++)
				{
					float num3 = (float)k / 31f;
					float num4 = (float)j / 31f;
					float num5 = (float)i / 31f;
					byte b = (byte)(num3 * 255f);
					byte b2 = (byte)(num4 * 255f);
					byte b3 = (byte)(num5 * 255f);
					array[num2 + k] = new Color32(b, b2, b3, byte.MaxValue);
				}
			}
		}
		this.defaultLut.SetPixels32(array);
		this.defaultLut.Apply();
		return this.defaultLut;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002970 File Offset: 0x00000B70
	private Texture2D CreateDepthCurveLut()
	{
		this.SafeRelease<Texture2D>(ref this.depthCurveLut);
		this.depthCurveLut = new Texture2D(1024, 1, TextureFormat.Alpha8, false, true)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		this.depthCurveLut.name = "DepthCurveLut";
		this.depthCurveLut.hideFlags = HideFlags.DontSave;
		this.depthCurveLut.anisoLevel = 1;
		this.depthCurveLut.wrapMode = TextureWrapMode.Clamp;
		this.depthCurveLut.filterMode = FilterMode.Bilinear;
		this.depthCurveColors = new Color32[1024];
		return this.depthCurveLut;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000029FC File Offset: 0x00000BFC
	private void UpdateDepthCurveLut()
	{
		if (this.depthCurveLut == null)
		{
			this.CreateDepthCurveLut();
		}
		float num = 0f;
		int i = 0;
		while (i < 1024)
		{
			this.depthCurveColors[i].a = (byte)Mathf.FloorToInt(Mathf.Clamp01(this.DepthMaskCurve.Evaluate(num)) * 255f);
			i++;
			num += 0.0009775171f;
		}
		this.depthCurveLut.SetPixels32(this.depthCurveColors);
		this.depthCurveLut.Apply();
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002A88 File Offset: 0x00000C88
	private void CheckUpdateDepthCurveLut()
	{
		bool flag = false;
		if (this.DepthMaskCurve.length != this.prevDepthMaskCurve.length)
		{
			flag = true;
		}
		else
		{
			float num = 0f;
			int i = 0;
			while (i < this.DepthMaskCurve.length)
			{
				if (Mathf.Abs(this.DepthMaskCurve.Evaluate(num) - this.prevDepthMaskCurve.Evaluate(num)) > 1E-45f)
				{
					flag = true;
					break;
				}
				i++;
				num += 0.0009775171f;
			}
		}
		if (this.depthCurveLut == null || flag)
		{
			this.UpdateDepthCurveLut();
			this.prevDepthMaskCurve = new AnimationCurve(this.DepthMaskCurve.keys);
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002B2C File Offset: 0x00000D2C
	private void CreateHelperTextures()
	{
		this.ReleaseTextures();
		this.blendCacheLut = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		this.blendCacheLut.name = "BlendCacheLut";
		this.blendCacheLut.wrapMode = TextureWrapMode.Clamp;
		this.blendCacheLut.useMipMap = false;
		this.blendCacheLut.anisoLevel = 0;
		this.blendCacheLut.Create();
		this.midBlendLUT = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		this.midBlendLUT.name = "MidBlendLut";
		this.midBlendLUT.wrapMode = TextureWrapMode.Clamp;
		this.midBlendLUT.useMipMap = false;
		this.midBlendLUT.anisoLevel = 0;
		this.midBlendLUT.Create();
		this.CreateDefaultLut();
		if (this.UseDepthMask)
		{
			this.CreateDepthCurveLut();
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C10 File Offset: 0x00000E10
	private bool CheckMaterialAndShader(Material material, string name)
	{
		if (material == null || material.shader == null)
		{
			Debug.LogWarning("[AmplifyColor] Error creating " + name + " material. Effect disabled.");
			base.enabled = false;
		}
		else if (!material.shader.isSupported)
		{
			Debug.LogWarning("[AmplifyColor] " + name + " shader not supported on this platform. Effect disabled.");
			base.enabled = false;
		}
		else
		{
			material.hideFlags = HideFlags.HideAndDontSave;
		}
		return base.enabled;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002C8C File Offset: 0x00000E8C
	private bool CreateMaterials()
	{
		this.SetupShader();
		if (!this.CheckShaders())
		{
			return false;
		}
		this.ReleaseMaterials();
		this.materialBase = new Material(this.shaderBase);
		this.materialBlend = new Material(this.shaderBlend);
		this.materialBlendCache = new Material(this.shaderBlendCache);
		this.materialMask = new Material(this.shaderMask);
		this.materialMaskBlend = new Material(this.shaderMaskBlend);
		this.materialDepthMask = new Material(this.shaderDepthMask);
		this.materialDepthMaskBlend = new Material(this.shaderDepthMaskBlend);
		this.materialProcessOnly = new Material(this.shaderProcessOnly);
		if (!true || !this.CheckMaterialAndShader(this.materialBase, "BaseMaterial") || !this.CheckMaterialAndShader(this.materialBlend, "BlendMaterial") || !this.CheckMaterialAndShader(this.materialBlendCache, "BlendCacheMaterial") || !this.CheckMaterialAndShader(this.materialMask, "MaskMaterial") || !this.CheckMaterialAndShader(this.materialMaskBlend, "MaskBlendMaterial") || !this.CheckMaterialAndShader(this.materialDepthMask, "DepthMaskMaterial") || !this.CheckMaterialAndShader(this.materialDepthMaskBlend, "DepthMaskBlendMaterial") || !this.CheckMaterialAndShader(this.materialProcessOnly, "ProcessOnlyMaterial"))
		{
			return false;
		}
		this.CreateHelperTextures();
		return true;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002DF4 File Offset: 0x00000FF4
	private void SetMaterialKeyword(string keyword, bool state)
	{
		bool flag = this.materialBase.IsKeywordEnabled(keyword);
		if (state && !flag)
		{
			this.materialBase.EnableKeyword(keyword);
			this.materialBlend.EnableKeyword(keyword);
			this.materialBlendCache.EnableKeyword(keyword);
			this.materialMask.EnableKeyword(keyword);
			this.materialMaskBlend.EnableKeyword(keyword);
			this.materialDepthMask.EnableKeyword(keyword);
			this.materialDepthMaskBlend.EnableKeyword(keyword);
			this.materialProcessOnly.EnableKeyword(keyword);
			return;
		}
		if (!state && this.materialBase.IsKeywordEnabled(keyword))
		{
			this.materialBase.DisableKeyword(keyword);
			this.materialBlend.DisableKeyword(keyword);
			this.materialBlendCache.DisableKeyword(keyword);
			this.materialMask.DisableKeyword(keyword);
			this.materialMaskBlend.DisableKeyword(keyword);
			this.materialDepthMask.DisableKeyword(keyword);
			this.materialDepthMaskBlend.DisableKeyword(keyword);
			this.materialProcessOnly.DisableKeyword(keyword);
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002EE8 File Offset: 0x000010E8
	private void SafeRelease<T>(ref T obj) where T : Object
	{
		if (obj != null)
		{
			if (obj.GetType() == typeof(RenderTexture))
			{
				(obj as RenderTexture).Release();
			}
			Object.DestroyImmediate(obj);
			obj = default(T);
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002F51 File Offset: 0x00001151
	private void ReleaseTextures()
	{
		RenderTexture.active = null;
		this.SafeRelease<RenderTexture>(ref this.blendCacheLut);
		this.SafeRelease<RenderTexture>(ref this.midBlendLUT);
		this.SafeRelease<Texture2D>(ref this.defaultLut);
		this.SafeRelease<Texture2D>(ref this.depthCurveLut);
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002F8C File Offset: 0x0000118C
	public static bool ValidateLutDimensions(Texture lut)
	{
		bool flag = true;
		if (lut != null)
		{
			if (lut.width / lut.height != lut.height)
			{
				Debug.LogWarning("[AmplifyColor] Lut " + lut.name + " has invalid dimensions.");
				flag = false;
			}
			else if (lut.anisoLevel != 0)
			{
				lut.anisoLevel = 0;
			}
		}
		return flag;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002FE7 File Offset: 0x000011E7
	private void UpdatePostEffectParams()
	{
		if (this.UseDepthMask)
		{
			this.CheckUpdateDepthCurveLut();
		}
		this.Exposure = Mathf.Max(this.Exposure, 0f);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00003010 File Offset: 0x00001210
	private int ComputeShaderPass()
	{
		bool flag = this.QualityLevel == Quality.Mobile;
		bool flag2 = this.colorSpace == ColorSpace.Linear;
		bool allowHDR = this.ownerCamera.allowHDR;
		int num = (flag ? 18 : 0);
		if (allowHDR)
		{
			num += 2;
			num += (flag2 ? 8 : 0);
			num += (this.ApplyDithering ? 4 : 0);
			num = (int)(num + this.Tonemapper);
		}
		else
		{
			num += (flag2 ? 1 : 0);
		}
		return num;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000307C File Offset: 0x0000127C
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.silentError)
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.BlendAmount = Mathf.Clamp01(this.BlendAmount);
		if (this.colorSpace != QualitySettings.activeColorSpace || this.qualityLevel != this.QualityLevel)
		{
			this.CreateMaterials();
		}
		this.UpdatePostEffectParams();
		bool flag = AmplifyColorBase.ValidateLutDimensions(this.LutTexture);
		bool flag2 = AmplifyColorBase.ValidateLutDimensions(this.LutBlendTexture);
		bool flag3 = this.LutTexture == null && this.LutBlendTexture == null && this.volumesLutBlendTexture == null;
		Texture texture = ((this.LutTexture == null) ? this.defaultLut : this.LutTexture);
		Texture lutBlendTexture = this.LutBlendTexture;
		int num = this.ComputeShaderPass();
		bool flag4 = this.BlendAmount != 0f || this.blending;
		bool flag5 = flag4 || (flag4 && lutBlendTexture != null);
		bool flag6 = flag5;
		bool flag7 = !flag || !flag2 || flag3;
		Material material;
		if (flag7)
		{
			material = this.materialProcessOnly;
		}
		else if (flag5 || this.volumesBlending)
		{
			if (this.UseDepthMask)
			{
				material = this.materialDepthMaskBlend;
			}
			else
			{
				material = ((this.MaskTexture != null) ? this.materialMaskBlend : this.materialBlend);
			}
		}
		else if (this.UseDepthMask)
		{
			material = this.materialDepthMask;
		}
		else
		{
			material = ((this.MaskTexture != null) ? this.materialMask : this.materialBase);
		}
		material.SetFloat("_Exposure", this.Exposure);
		material.SetFloat("_ShoulderStrength", 0.22f);
		material.SetFloat("_LinearStrength", 0.3f);
		material.SetFloat("_LinearAngle", 0.1f);
		material.SetFloat("_ToeStrength", 0.2f);
		material.SetFloat("_ToeNumerator", 0.01f);
		material.SetFloat("_ToeDenominator", 0.3f);
		material.SetFloat("_LinearWhite", this.LinearWhitePoint);
		material.SetFloat("_LerpAmount", this.BlendAmount);
		if (this.MaskTexture != null)
		{
			material.SetTexture("_MaskTex", this.MaskTexture);
		}
		if (this.UseDepthMask)
		{
			material.SetTexture("_DepthCurveLut", this.depthCurveLut);
		}
		if (this.MaskTexture != null && source.dimension == TextureDimension.Tex2DArray)
		{
			material.SetVector("_StereoScale", new Vector4(0.5f, 1f, 0.5f, 0f));
		}
		else
		{
			material.SetVector("_StereoScale", new Vector4(1f, 1f, 0f, 0f));
		}
		if (!flag7)
		{
			if (this.volumesBlending)
			{
				this.volumesBlendAmount = Mathf.Clamp01(this.volumesBlendAmount);
				this.materialBlendCache.SetFloat("_LerpAmount", this.volumesBlendAmount);
				if (this.blendingFromMidBlend)
				{
					this.materialBlendCache.SetTexture("_RgbTex", this.midBlendLUT);
				}
				else
				{
					this.materialBlendCache.SetTexture("_RgbTex", texture);
				}
				this.materialBlendCache.SetTexture("_LerpRgbTex", (this.volumesLutBlendTexture != null) ? this.volumesLutBlendTexture : this.defaultLut);
				Graphics.Blit(texture, this.blendCacheLut, this.materialBlendCache);
			}
			if (flag6)
			{
				this.materialBlendCache.SetFloat("_LerpAmount", this.BlendAmount);
				RenderTexture renderTexture = null;
				if (this.volumesBlending)
				{
					renderTexture = RenderTexture.GetTemporary(this.blendCacheLut.width, this.blendCacheLut.height, this.blendCacheLut.depth, this.blendCacheLut.format, RenderTextureReadWrite.Linear);
					Graphics.Blit(this.blendCacheLut, renderTexture);
					this.materialBlendCache.SetTexture("_RgbTex", renderTexture);
				}
				else
				{
					this.materialBlendCache.SetTexture("_RgbTex", texture);
				}
				this.materialBlendCache.SetTexture("_LerpRgbTex", (lutBlendTexture != null) ? lutBlendTexture : this.defaultLut);
				Graphics.Blit(texture, this.blendCacheLut, this.materialBlendCache);
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				material.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
			}
			else if (this.volumesBlending)
			{
				material.SetTexture("_RgbBlendCacheTex", this.blendCacheLut);
			}
			else
			{
				if (texture != null)
				{
					material.SetTexture("_RgbTex", texture);
				}
				if (lutBlendTexture != null)
				{
					material.SetTexture("_LerpRgbTex", lutBlendTexture);
				}
			}
		}
		Graphics.Blit(source, destination, material, num);
		if (flag6 || this.volumesBlending)
		{
			this.blendCacheLut.DiscardContents();
		}
	}

	// Token: 0x04000001 RID: 1
	public const int LutSize = 32;

	// Token: 0x04000002 RID: 2
	public const int LutWidth = 1024;

	// Token: 0x04000003 RID: 3
	public const int LutHeight = 32;

	// Token: 0x04000004 RID: 4
	private const int DepthCurveLutRange = 1024;

	// Token: 0x04000005 RID: 5
	public Tonemapping Tonemapper;

	// Token: 0x04000006 RID: 6
	public float Exposure = 1f;

	// Token: 0x04000007 RID: 7
	public float LinearWhitePoint = 11.2f;

	// Token: 0x04000008 RID: 8
	[FormerlySerializedAs("UseDithering")]
	public bool ApplyDithering;

	// Token: 0x04000009 RID: 9
	public Quality QualityLevel = Quality.Standard;

	// Token: 0x0400000A RID: 10
	public float BlendAmount;

	// Token: 0x0400000B RID: 11
	public Texture LutTexture;

	// Token: 0x0400000C RID: 12
	public Texture LutBlendTexture;

	// Token: 0x0400000D RID: 13
	public Texture MaskTexture;

	// Token: 0x0400000E RID: 14
	public bool UseDepthMask;

	// Token: 0x0400000F RID: 15
	public AnimationCurve DepthMaskCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04000010 RID: 16
	public bool UseVolumes;

	// Token: 0x04000011 RID: 17
	public float ExitVolumeBlendTime = 1f;

	// Token: 0x04000012 RID: 18
	public Transform TriggerVolumeProxy;

	// Token: 0x04000013 RID: 19
	public LayerMask VolumeCollisionMask = -1;

	// Token: 0x04000014 RID: 20
	private Camera ownerCamera;

	// Token: 0x04000015 RID: 21
	private Shader shaderBase;

	// Token: 0x04000016 RID: 22
	private Shader shaderBlend;

	// Token: 0x04000017 RID: 23
	private Shader shaderBlendCache;

	// Token: 0x04000018 RID: 24
	private Shader shaderMask;

	// Token: 0x04000019 RID: 25
	private Shader shaderMaskBlend;

	// Token: 0x0400001A RID: 26
	private Shader shaderDepthMask;

	// Token: 0x0400001B RID: 27
	private Shader shaderDepthMaskBlend;

	// Token: 0x0400001C RID: 28
	private Shader shaderProcessOnly;

	// Token: 0x0400001D RID: 29
	private RenderTexture blendCacheLut;

	// Token: 0x0400001E RID: 30
	private Texture2D defaultLut;

	// Token: 0x0400001F RID: 31
	private Texture2D depthCurveLut;

	// Token: 0x04000020 RID: 32
	private Color32[] depthCurveColors;

	// Token: 0x04000021 RID: 33
	private ColorSpace colorSpace = ColorSpace.Uninitialized;

	// Token: 0x04000022 RID: 34
	private Quality qualityLevel = Quality.Standard;

	// Token: 0x04000023 RID: 35
	private Material materialBase;

	// Token: 0x04000024 RID: 36
	private Material materialBlend;

	// Token: 0x04000025 RID: 37
	private Material materialBlendCache;

	// Token: 0x04000026 RID: 38
	private Material materialMask;

	// Token: 0x04000027 RID: 39
	private Material materialMaskBlend;

	// Token: 0x04000028 RID: 40
	private Material materialDepthMask;

	// Token: 0x04000029 RID: 41
	private Material materialDepthMaskBlend;

	// Token: 0x0400002A RID: 42
	private Material materialProcessOnly;

	// Token: 0x0400002B RID: 43
	private bool blending;

	// Token: 0x0400002C RID: 44
	private float blendingTime;

	// Token: 0x0400002D RID: 45
	private float blendingTimeCountdown;

	// Token: 0x0400002E RID: 46
	private Action onFinishBlend;

	// Token: 0x0400002F RID: 47
	private AnimationCurve prevDepthMaskCurve = new AnimationCurve();

	// Token: 0x04000030 RID: 48
	private bool volumesBlending;

	// Token: 0x04000031 RID: 49
	private float volumesBlendingTime;

	// Token: 0x04000032 RID: 50
	private float volumesBlendingTimeCountdown;

	// Token: 0x04000033 RID: 51
	private Texture volumesLutBlendTexture;

	// Token: 0x04000034 RID: 52
	private float volumesBlendAmount;

	// Token: 0x04000035 RID: 53
	private Texture worldLUT;

	// Token: 0x04000036 RID: 54
	private AmplifyColorVolumeBase currentVolumeLut;

	// Token: 0x04000037 RID: 55
	private RenderTexture midBlendLUT;

	// Token: 0x04000038 RID: 56
	private bool blendingFromMidBlend;

	// Token: 0x04000039 RID: 57
	private VolumeEffect worldVolumeEffects;

	// Token: 0x0400003A RID: 58
	private VolumeEffect currentVolumeEffects;

	// Token: 0x0400003B RID: 59
	private VolumeEffect blendVolumeEffects;

	// Token: 0x0400003C RID: 60
	private float worldExposure = 1f;

	// Token: 0x0400003D RID: 61
	private float currentExposure = 1f;

	// Token: 0x0400003E RID: 62
	private float blendExposure = 1f;

	// Token: 0x0400003F RID: 63
	private float effectVolumesBlendAdjust;

	// Token: 0x04000040 RID: 64
	private List<AmplifyColorVolumeBase> enteredVolumes = new List<AmplifyColorVolumeBase>();

	// Token: 0x04000041 RID: 65
	private AmplifyColorTriggerProxyBase actualTriggerProxy;

	// Token: 0x04000042 RID: 66
	[HideInInspector]
	public VolumeEffectFlags EffectFlags = new VolumeEffectFlags();

	// Token: 0x04000043 RID: 67
	[SerializeField]
	[HideInInspector]
	private string sharedInstanceID = "";

	// Token: 0x04000044 RID: 68
	private bool silentError;
}
