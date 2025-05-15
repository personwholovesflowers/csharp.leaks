using System;
using SettingsMenu.Components.Pages;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000353 RID: 851
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PostProcessV2_Handler : MonoSingleton<PostProcessV2_Handler>
{
	// Token: 0x060013B6 RID: 5046 RVA: 0x0009D4D0 File Offset: 0x0009B6D0
	private void OnValidate()
	{
		this.Fooled(this.debugFooled);
		if (this.useHeightFog)
		{
			Shader.EnableKeyword("HEIGHT_FOG");
			Shader.SetGlobalFloat(this.heightFogStartID, this.heightFogStart);
			Shader.SetGlobalFloat(this.heightFogEndID, this.heightFogEnd);
		}
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x0009D520 File Offset: 0x0009B720
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
		if (this.useHeightFog)
		{
			Shader.EnableKeyword("HEIGHT_FOG");
			return;
		}
		Shader.DisableKeyword("HEIGHT_FOG");
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x0009D570 File Offset: 0x0009B770
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x0009D592 File Offset: 0x0009B792
	private void Update()
	{
		if (this.useHeightFog)
		{
			Shader.SetGlobalFloat(this.heightFogStartID, this.heightFogStart);
		}
		Shader.SetGlobalFloat(this.heightFogEndID, this.heightFogEnd);
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x0009D5C0 File Offset: 0x0009B7C0
	private void OnPrefChanged(string key, object value)
	{
		if (!(key == "outlineThickness"))
		{
			if (!(key == "pixelization"))
			{
				if (!(key == "simplifyEnemies"))
				{
					return;
				}
				if (value is int)
				{
					if ((int)value == 0)
					{
						this.SetupOutlines(true);
						return;
					}
					this.SetupOutlines(false);
				}
			}
			else if (value is int)
			{
				int num = (int)value;
				this.SetPixelization(num);
				return;
			}
		}
		else if (value is int)
		{
			int num2 = (int)value;
			this.distance = num2;
			this.SetupOutlines(false);
			return;
		}
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x0009D650 File Offset: 0x0009B850
	private void SetPixelization(int option)
	{
		float pixelizationValue = SettingsMenu.Components.Pages.GraphicsSettings.GetPixelizationValue(option);
		Shader.SetGlobalFloat("_ResY", pixelizationValue);
		this.downscaleResolution = pixelizationValue;
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x0009D678 File Offset: 0x0009B878
	private void Start()
	{
		this.usedComputeShadersAtStart = !SettingsMenu.Components.Pages.GraphicsSettings.disabledComputeShaders;
		this.Vignette(false);
		this.postProcessV2_VSRM.DisableKeyword("UNDERWATER");
		this.DeathEffect(false);
		Shader.SetGlobalFloat("_Sharpness", 0f);
		Shader.SetGlobalFloat("_Deathness", 0f);
		this.WickedEffect(false);
		Shader.SetGlobalFloat("_RandomNoiseStrength", 0f);
		DateTime now = DateTime.Now;
		bool flag = now.Month == 4 && now.Day == 1;
		flag |= this.debugFooled;
		this.Fooled(flag);
		this.isGLCore = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore;
		this.mainCam = MonoSingleton<CameraController>.Instance.cam;
		this.hudCam = this.mainCam.transform.Find("HUD Camera").GetComponent<Camera>();
		this.virtualCam = this.mainCam.transform.Find("Virtual Camera").GetComponent<Camera>();
		this.ReinitializeCameras();
		this.postProcessV2_VSRM.SetTexture("_Dither", this.ditherTexture);
		this.postProcessV2_VSRM.SetTexture("_VignetteTex", this.vignetteTexture);
		if (this.oilTex != null)
		{
			Shader.SetGlobalTexture("_OilSlick", this.oilTex);
		}
		if (this.sandTex != null)
		{
			Shader.SetGlobalTexture("_SandTex", this.sandTex);
		}
		if (this.buffTex != null)
		{
			Shader.SetGlobalTexture("_BuffTex", this.buffTex);
		}
		this.oman = MonoSingleton<OptionsManager>.Instance;
		this.distance = MonoSingleton<PrefsManager>.Instance.GetInt("outlineThickness", 0);
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("pixelization", 0);
		this.SetPixelization(@int);
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x0009D837 File Offset: 0x0009BA37
	public void DeathEffect(bool isDead)
	{
		if (isDead)
		{
			this.postProcessV2_VSRM.EnableKeyword("DEAD");
			return;
		}
		this.postProcessV2_VSRM.DisableKeyword("DEAD");
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x0009D85D File Offset: 0x0009BA5D
	public void WickedEffect(bool doWicked)
	{
		if (doWicked)
		{
			this.postProcessV2_VSRM.EnableKeyword("WICKED");
			return;
		}
		this.postProcessV2_VSRM.DisableKeyword("WICKED");
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x0009D883 File Offset: 0x0009BA83
	public void Vignette(bool doVignette)
	{
		if (doVignette)
		{
			this.postProcessV2_VSRM.EnableKeyword("VIGNETTE");
			return;
		}
		this.postProcessV2_VSRM.DisableKeyword("VIGNETTE");
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x0009D8A9 File Offset: 0x0009BAA9
	public void Fooled(bool doEnable)
	{
		if (doEnable)
		{
			Shader.EnableKeyword("Fooled");
			Shader.EnableKeyword("FOOLED");
			return;
		}
		Shader.DisableKeyword("Fooled");
		Shader.DisableKeyword("FOOLED");
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x0009D8D8 File Offset: 0x0009BAD8
	public void ColorPalette(bool stuff)
	{
		if (this.CurrentMapPaletteOverride != null)
		{
			return;
		}
		if (stuff && this.CurrentTexture != null)
		{
			Shader.EnableKeyword("PALETTIZE");
			Shader.SetGlobalInt("_ColorPrecision", 2048);
			MonoSingleton<ConvertPaletteToLUT>.Instance.ConvertPalette((Texture2D)this.CurrentTexture, this.paletteCompute, this.paletteCalc);
			return;
		}
		Shader.DisableKeyword("PALETTIZE");
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x0009D94C File Offset: 0x0009BB4C
	public void ApplyUserColorPalette(Texture tex)
	{
		if (!MonoSingleton<PrefsManager>.Instance.GetBoolLocal("colorPalette", false))
		{
			MonoSingleton<PrefsManager>.Instance.SetBoolLocal("colorPalette", true);
		}
		this.CurrentTexture = tex;
		if (this.CurrentMapPaletteOverride != null)
		{
			return;
		}
		MonoSingleton<ConvertPaletteToLUT>.Instance.ConvertPalette((Texture2D)tex, this.paletteCompute, this.paletteCalc);
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x0009D9B0 File Offset: 0x0009BBB0
	public void ApplyMapColorPalette(Texture tex)
	{
		if (tex == null)
		{
			this.CurrentMapPaletteOverride = null;
			this.ColorPalette(MonoSingleton<PrefsManager>.Instance.GetBoolLocal("colorPalette", false));
			return;
		}
		MonoSingleton<ConvertPaletteToLUT>.Instance.ConvertPalette((Texture2D)tex, this.paletteCompute, this.paletteCalc);
		this.CurrentMapPaletteOverride = tex;
		Shader.SetGlobalTexture("_PaletteTex", tex);
		Shader.EnableKeyword("PALETTIZE");
		Shader.SetGlobalInt("_ColorPrecision", 2048);
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x0009DA2B File Offset: 0x0009BC2B
	private void ReinitializeCameras()
	{
		if (Application.isPlaying)
		{
			Camera.onPreRender = null;
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
		}
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x0009DA5C File Offset: 0x0009BC5C
	private void SetupRTs()
	{
		this.width = Screen.width;
		this.height = Screen.height;
		if (this.downscaleResolution != 0f)
		{
			float num = (float)this.width;
			float num2 = (float)this.height;
			float num3 = Mathf.Min(num, num2);
			Vector2 vector = new Vector2(num / num3, num2 / num3) * this.downscaleResolution;
			this.width = (int)vector.x;
			this.height = (int)vector.y;
		}
		bool flag = this.width != this.lastWidth || this.height != this.lastHeight;
		this.reinitializeTextures = this.reinitializeTextures || flag;
		this.lastWidth = this.width;
		this.lastHeight = this.height;
		Vector2 vector2 = new Vector2((float)this.width, (float)this.height);
		this.postProcessV2_VSRM.SetVector("_VirtualRes", vector2);
		if (this.reinitializeTextures)
		{
			MonoSingleton<OptionsManager>.Instance.SetSimplifyEnemies(MonoSingleton<PrefsManager>.Instance.GetInt("simplifyEnemies", 0));
			if (this.mainCam == null)
			{
				this.mainCam = MonoSingleton<CameraController>.Instance.cam;
				this.hudCam = this.mainCam.transform.Find("HUD Camera").GetComponent<Camera>();
				this.virtualCam = this.mainCam.transform.Find("Virtual Camera").GetComponent<Camera>();
			}
			this.mainCam.targetTexture = null;
			this.hudCam.targetTexture = null;
			this.ReleaseTextures();
			this.mainTex = new RenderTexture(this.width, this.height, 0, RenderTextureFormat.ARGB32)
			{
				name = "Main",
				antiAliasing = 1,
				filterMode = FilterMode.Point
			};
			this.depthBuffer = new RenderTexture(this.width, this.height, 32, RenderTextureFormat.Depth)
			{
				name = "Depth Buffer",
				antiAliasing = 1,
				filterMode = FilterMode.Point
			};
			this.reusableBufferA = new RenderTexture(this.width, this.height, 0, RenderTextureFormat.RG16)
			{
				name = "Reusable A",
				antiAliasing = 1,
				filterMode = FilterMode.Point
			};
			this.reusableBufferB = new RenderTexture(this.width, this.height, 0, RenderTextureFormat.ARGB32)
			{
				name = "Reusable B",
				antiAliasing = 1,
				filterMode = FilterMode.Point
			};
			this.viewNormal = new RenderTexture(this.width, this.height, 0, RenderTextureFormat.RG16)
			{
				name = "View Normal",
				antiAliasing = 1,
				filterMode = FilterMode.Point
			};
			this.buffers[0] = this.mainTex.colorBuffer;
			this.buffers[1] = this.reusableBufferA.colorBuffer;
			this.mainCam.SetTargetBuffers(this.buffers, this.depthBuffer.depthBuffer);
			this.mainCam.RemoveCommandBuffers(CameraEvent.BeforeForwardAlpha);
			CommandBuffer commandBuffer = new CommandBuffer();
			commandBuffer.name = "Screen Normal";
			this.screenNormal.SetTexture("_DepthBuffer", this.depthBuffer);
			commandBuffer.Blit(this.depthBuffer, this.viewNormal, this.screenNormal);
			this.mainCam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, commandBuffer);
			MonoSingleton<BloodsplatterManager>.Instance.SetupBloodCommandBuffer(this.mainCam, this.mainTex, this.reusableBufferB, this.depthBuffer, this.viewNormal);
			if (this.usedComputeShadersAtStart)
			{
				MonoSingleton<StainVoxelManager>.Instance.SetupStainCommandBuffer(this.mainCam, this.mainTex, this.reusableBufferB, this.depthBuffer, this.viewNormal);
			}
			this.mainCam.RemoveCommandBuffers(CameraEvent.AfterForwardAlpha);
			this.SetupOutlines(false);
			this.hudCam.SetTargetBuffers(this.mainTex.colorBuffer, this.depthBuffer.depthBuffer);
			this.postProcessV2_VSRM.SetTexture("_MainTex", this.mainTex);
			this.reinitializeTextures = false;
			Action<bool> action = this.onReinitialize;
			if (action == null)
			{
				return;
			}
			action(true);
		}
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x0009DE50 File Offset: 0x0009C050
	private void ReleaseTextures()
	{
		if (this.mainTex)
		{
			this.mainTex.Release();
			if (this.mainTex)
			{
				Object.Destroy(this.mainTex);
			}
		}
		if (this.reusableBufferA)
		{
			this.reusableBufferA.Release();
			if (this.reusableBufferA)
			{
				Object.Destroy(this.reusableBufferA);
			}
		}
		if (this.reusableBufferB)
		{
			this.reusableBufferB.Release();
			if (this.reusableBufferB)
			{
				Object.Destroy(this.reusableBufferB);
			}
		}
		if (this.depthBuffer)
		{
			this.depthBuffer.Release();
			if (this.depthBuffer)
			{
				Object.Destroy(this.depthBuffer);
			}
		}
		if (this.viewNormal)
		{
			this.viewNormal.Release();
			if (this.viewNormal)
			{
				Object.Destroy(this.viewNormal);
			}
		}
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x0009DF50 File Offset: 0x0009C150
	public void HeatWaves()
	{
		if (this.heatCB == null)
		{
			this.heatCB = new CommandBuffer();
			this.heatCB.name = "HeatWaves";
		}
		this.heatCB.Clear();
		this.heatCB.SetRenderTarget(null, null);
		this.heatCB.SetGlobalTexture("_Stencil", this.depthBuffer, RenderTextureSubElement.Stencil);
		this.heatCB.CopyTexture(this.mainTex, this.reusableBufferB);
		this.heatCB.Blit(this.reusableBufferB, this.mainTex, this.heatWaveMat, 0);
		this.mainCam.AddCommandBuffer(CameraEvent.AfterForwardAlpha, this.heatCB);
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0009E018 File Offset: 0x0009C218
	public void SetupOutlines(bool forceOnePixelOutline = false)
	{
		Debug.Log("Simplify: " + MonoSingleton<PrefsManager>.Instance.GetInt("simplifyEnemies", 0).ToString());
		Debug.Log("Distance: " + MonoSingleton<PrefsManager>.Instance.GetInt("simplifyEnemiesDistance", 0).ToString());
		Debug.Log("Thickness: " + MonoSingleton<PrefsManager>.Instance.GetInt("outlineThickness", 0).ToString());
		this.distance = MonoSingleton<PrefsManager>.Instance.GetInt("outlineThickness", 0);
		if (this.mainCam == null)
		{
			this.SetupRTs();
			return;
		}
		this.mainCam.RemoveCommandBuffers(CameraEvent.BeforeForwardOpaque);
		if (this.mainTex == null)
		{
			this.SetupRTs();
			return;
		}
		if (this.isGLCore)
		{
			return;
		}
		if (this.outlineCB == null)
		{
			this.outlineCB = new CommandBuffer();
			this.outlineCB.name = "Outlines";
		}
		Vector2 vector = new Vector2((float)this.mainTex.width, (float)this.mainTex.height);
		Vector2 vector2 = vector / new Vector2((float)Screen.width, (float)Screen.height);
		float num = (float)this.distance;
		if (this.distance > 1)
		{
			num = (float)this.distance * Mathf.Max(vector2.x, vector2.y);
		}
		this.outlineCB.Clear();
		if (this.outlineMat == null)
		{
			this.outlineMat = new Material(this.outlinePx);
		}
		this.outlineCB.SetGlobalVector("_Resolution", vector);
		this.outlineCB.SetGlobalVector("_ResolutionDiff", vector2);
		Mathf.CeilToInt((float)this.width / 8f);
		Mathf.CeilToInt((float)this.height / 8f);
		if (!forceOnePixelOutline && this.distance > 1 && num > 1f && this.oman.simplifyEnemies)
		{
			this.distance = Mathf.Min(this.distance, 16);
			this.outlineCB.Blit(this.reusableBufferA, this.reusableBufferB, this.outlineMat, 0);
			float num2 = 8f;
			int num3 = 0;
			while (num2 >= 0.5f || this.reusableBufferA.name == "Reusable B")
			{
				this.outlineCB.SetGlobalFloat("_TestDistance", num2);
				this.outlineCB.Blit(this.reusableBufferB, this.reusableBufferA, this.outlineMat, 1);
				RenderTexture renderTexture = this.reusableBufferB;
				RenderTexture renderTexture2 = this.reusableBufferA;
				this.reusableBufferA = renderTexture;
				this.reusableBufferB = renderTexture2;
				num2 *= 0.5f;
				num3++;
			}
			this.outlineCB.SetGlobalFloat("_OutlineDistance", (float)this.distance);
			this.outlineCB.SetGlobalFloat("_TestDistance", num2);
			this.outlineCB.Blit(this.reusableBufferB, this.mainTex, this.outlineMat, 2);
			this.outlineCB.SetRenderTarget(this.reusableBufferA);
			this.outlineCB.ClearRenderTarget(false, true, Color.black);
			this.outlineCB.SetRenderTarget(this.reusableBufferB);
			this.outlineCB.ClearRenderTarget(false, true, Color.black);
		}
		else
		{
			this.outlineCB.Blit(this.reusableBufferA, this.mainTex, this.outlineMat, 3);
			this.outlineCB.SetRenderTarget(this.reusableBufferA);
			this.outlineCB.ClearRenderTarget(false, true, Color.black);
		}
		this.mainCam.AddCommandBuffer(CameraEvent.AfterForwardAlpha, this.outlineCB);
		CommandBuffer commandBuffer = new CommandBuffer();
		commandBuffer.name = "Clear Before Draw";
		commandBuffer.SetRenderTarget(this.reusableBufferA);
		commandBuffer.ClearRenderTarget(false, true, Color.black);
		this.mainCam.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, commandBuffer);
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x0009E424 File Offset: 0x0009C624
	public void ChangeCamera(bool hudless)
	{
		this.mainCameraOnly = hudless;
		this.mainCam.targetTexture = null;
		MonoSingleton<CameraController>.Instance.cam.clearFlags = this.mainCam.clearFlags;
		this.mainCam = MonoSingleton<CameraController>.Instance.cam;
		this.virtualCam = this.mainCam.transform.Find("Virtual Camera").GetComponent<Camera>();
		this.reinitializeTextures = true;
		this.SetupRTs();
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0009E49C File Offset: 0x0009C69C
	public void OnPreRenderCallback(Camera cam)
	{
		if (cam == this.mainCam)
		{
			this.SetupRTs();
		}
		if (cam == this.hudCam)
		{
			this.hudCam.SetTargetBuffers(this.mainTex.colorBuffer, this.depthBuffer.depthBuffer);
		}
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x0009E4EC File Offset: 0x0009C6EC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnPreRenderCallback));
	}

	// Token: 0x04001B04 RID: 6916
	public Material postProcessV2_VSRM;

	// Token: 0x04001B05 RID: 6917
	public Material screenNormal;

	// Token: 0x04001B06 RID: 6918
	public Material heatWaveMat;

	// Token: 0x04001B07 RID: 6919
	private CommandBuffer heatCB;

	// Token: 0x04001B08 RID: 6920
	public Shader outlinePx;

	// Token: 0x04001B09 RID: 6921
	private Material outlineMat;

	// Token: 0x04001B0A RID: 6922
	private CommandBuffer outlineCB;

	// Token: 0x04001B0B RID: 6923
	[Space(10f)]
	public bool useHeightFog;

	// Token: 0x04001B0C RID: 6924
	public float heightFogStart = 10f;

	// Token: 0x04001B0D RID: 6925
	public float heightFogEnd = 100f;

	// Token: 0x04001B0E RID: 6926
	public Texture oilTex;

	// Token: 0x04001B0F RID: 6927
	public Texture sandTex;

	// Token: 0x04001B10 RID: 6928
	public Texture buffTex;

	// Token: 0x04001B11 RID: 6929
	public Texture ditherTexture;

	// Token: 0x04001B12 RID: 6930
	public Texture vignetteTexture;

	// Token: 0x04001B13 RID: 6931
	public int distance = 1;

	// Token: 0x04001B14 RID: 6932
	public Camera mainCam;

	// Token: 0x04001B15 RID: 6933
	public Camera hudCam;

	// Token: 0x04001B16 RID: 6934
	public Camera virtualCam;

	// Token: 0x04001B17 RID: 6935
	private RenderBuffer[] buffers = new RenderBuffer[2];

	// Token: 0x04001B18 RID: 6936
	private RenderTexture mainTex;

	// Token: 0x04001B19 RID: 6937
	private RenderTexture reusableBufferA;

	// Token: 0x04001B1A RID: 6938
	private RenderTexture reusableBufferB;

	// Token: 0x04001B1B RID: 6939
	public RenderTexture depthBuffer;

	// Token: 0x04001B1C RID: 6940
	public RenderTexture viewNormal;

	// Token: 0x04001B1D RID: 6941
	private int width;

	// Token: 0x04001B1E RID: 6942
	private int height;

	// Token: 0x04001B1F RID: 6943
	private int lastWidth;

	// Token: 0x04001B20 RID: 6944
	private int lastHeight;

	// Token: 0x04001B21 RID: 6945
	private bool reinitializeTextures;

	// Token: 0x04001B22 RID: 6946
	private bool mainCameraOnly;

	// Token: 0x04001B23 RID: 6947
	[HideInInspector]
	public float downscaleResolution;

	// Token: 0x04001B24 RID: 6948
	public Texture CurrentTexture;

	// Token: 0x04001B25 RID: 6949
	public Texture CurrentMapPaletteOverride;

	// Token: 0x04001B26 RID: 6950
	public Material radiantBuff;

	// Token: 0x04001B27 RID: 6951
	private OptionsManager oman;

	// Token: 0x04001B28 RID: 6952
	public bool debugFooled;

	// Token: 0x04001B29 RID: 6953
	[SerializeField]
	private ComputeShader paletteCompute;

	// Token: 0x04001B2A RID: 6954
	[SerializeField]
	private Shader paletteCalc;

	// Token: 0x04001B2B RID: 6955
	private bool isGLCore;

	// Token: 0x04001B2C RID: 6956
	private float realDist;

	// Token: 0x04001B2D RID: 6957
	public Action<bool> onReinitialize;

	// Token: 0x04001B2E RID: 6958
	private int heightFogStartID = Shader.PropertyToID("_HeightFogStart");

	// Token: 0x04001B2F RID: 6959
	private int heightFogEndID = Shader.PropertyToID("_HeightFogEnd");

	// Token: 0x04001B30 RID: 6960
	public bool usedComputeShadersAtStart = true;
}
