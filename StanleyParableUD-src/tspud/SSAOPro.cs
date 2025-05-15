using System;
using UnityEngine;

// Token: 0x020000CD RID: 205
[ImageEffectAllowedInSceneView]
[HelpURL("http://www.thomashourdel.com/ssaopro/doc/")]
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/SSAO Pro")]
[RequireComponent(typeof(Camera))]
public class SSAOPro : MonoBehaviour
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001B755 File Offset: 0x00019955
	public Material Material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.ShaderSSAO)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			return this.m_Material;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001B784 File Offset: 0x00019984
	public Shader ShaderSSAO
	{
		get
		{
			if (this.m_ShaderSSAO == null)
			{
				this.m_ShaderSSAO = Shader.Find("Hidden/SSAO Pro V2");
			}
			return this.m_ShaderSSAO;
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001B7AC File Offset: 0x000199AC
	public void UpdateQualitySetting(int mode)
	{
		if (mode == 0)
		{
			base.enabled = false;
			return;
		}
		if (mode == 1)
		{
			base.enabled = true;
			this.Samples = SSAOPro.SampleCount.VeryLow;
			this.Downsampling = 4;
			this.BlurDownsampling = true;
			this.Blur = SSAOPro.BlurMode.Gaussian;
			return;
		}
		if (mode == 2)
		{
			base.enabled = true;
			this.Samples = SSAOPro.SampleCount.Low;
			this.Downsampling = 2;
			this.Blur = SSAOPro.BlurMode.HighQualityBilateral;
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001B80C File Offset: 0x00019A0C
	private void OnEnable()
	{
		this.m_Camera = base.GetComponent<Camera>();
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning("Image Effects are not supported on this device.");
			base.enabled = false;
			return;
		}
		if (!SystemInfo.supportsRenderTextures)
		{
			Debug.LogWarning("RenderTextures are not supported on this platform.");
			base.enabled = false;
			return;
		}
		if (this.ShaderSSAO == null)
		{
			Debug.LogWarning("Missing shader (SSAO).");
			base.enabled = false;
			return;
		}
		if (!this.ShaderSSAO.isSupported)
		{
			Debug.LogWarning("Unsupported shader (SSAO).");
			base.enabled = false;
			return;
		}
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			Debug.LogWarning("Depth textures aren't supported on this device.");
			base.enabled = false;
			return;
		}
		this.m_Camera.depthTextureMode |= DepthTextureMode.Depth;
		this.m_Camera.depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001B8D6 File Offset: 0x00019AD6
	private void OnDisable()
	{
		if (this.m_Material != null)
		{
			Object.DestroyImmediate(this.m_Material);
		}
		this.m_Material = null;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001B8F8 File Offset: 0x00019AF8
	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.ShaderSSAO == null || Mathf.Approximately(this.Intensity, 0f))
		{
			Graphics.Blit(source, destination);
			return;
		}
		this.Material.shaderKeywords = null;
		switch (this.Samples)
		{
		case SSAOPro.SampleCount.Low:
			this.Material.EnableKeyword("SAMPLES_LOW");
			break;
		case SSAOPro.SampleCount.Medium:
			this.Material.EnableKeyword("SAMPLES_MEDIUM");
			break;
		case SSAOPro.SampleCount.High:
			this.Material.EnableKeyword("SAMPLES_HIGH");
			break;
		case SSAOPro.SampleCount.Ultra:
			this.Material.EnableKeyword("SAMPLES_ULTRA");
			break;
		}
		int num = 0;
		if (this.NoiseTexture != null)
		{
			num = 1;
		}
		if (!Mathf.Approximately(this.LumContribution, 0f))
		{
			num += 2;
		}
		num++;
		this.Material.SetMatrix("_InverseViewProject", (this.m_Camera.projectionMatrix * this.m_Camera.worldToCameraMatrix).inverse);
		this.Material.SetMatrix("_CameraModelView", this.m_Camera.cameraToWorldMatrix);
		this.Material.SetTexture("_NoiseTex", this.NoiseTexture);
		this.Material.SetVector("_Params1", new Vector4((this.NoiseTexture == null) ? 0f : ((float)this.NoiseTexture.width), this.Radius, this.Intensity, this.Distance));
		this.Material.SetVector("_Params2", new Vector4(this.Bias, this.LumContribution, this.CutoffDistance, this.CutoffFalloff));
		this.Material.SetColor("_OcclusionColor", this.OcclusionColor);
		if (this.Blur != SSAOPro.BlurMode.None)
		{
			SSAOPro.Pass pass = ((this.Blur == SSAOPro.BlurMode.HighQualityBilateral) ? SSAOPro.Pass.HighQualityBilateralBlur : SSAOPro.Pass.GaussianBlur);
			int num2 = (this.BlurDownsampling ? this.Downsampling : 1);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / num2, source.height / num2, 0, RenderTextureFormat.ARGB32);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / this.Downsampling, source.height / this.Downsampling, 0, RenderTextureFormat.ARGB32);
			Graphics.Blit(temporary, temporary, this.Material, 0);
			Graphics.Blit(source, temporary, this.Material, num);
			this.Material.SetFloat("_BilateralThreshold", this.BlurBilateralThreshold * 5f);
			for (int i = 0; i < this.BlurPasses; i++)
			{
				this.Material.SetVector("_Direction", new Vector2(1f / (float)source.width, 0f));
				Graphics.Blit(temporary, temporary2, this.Material, (int)pass);
				temporary.DiscardContents();
				this.Material.SetVector("_Direction", new Vector2(0f, 1f / (float)source.height));
				Graphics.Blit(temporary2, temporary, this.Material, (int)pass);
				temporary2.DiscardContents();
			}
			if (!this.DebugAO)
			{
				this.Material.SetTexture("_SSAOTex", temporary);
				Graphics.Blit(source, destination, this.Material, 7);
			}
			else
			{
				Graphics.Blit(temporary, destination);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			return;
		}
		RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / this.Downsampling, source.height / this.Downsampling, 0, RenderTextureFormat.ARGB32);
		Graphics.Blit(temporary3, temporary3, this.Material, 0);
		if (this.DebugAO)
		{
			Graphics.Blit(source, temporary3, this.Material, num);
			Graphics.Blit(temporary3, destination);
			RenderTexture.ReleaseTemporary(temporary3);
			return;
		}
		Graphics.Blit(source, temporary3, this.Material, num);
		this.Material.SetTexture("_SSAOTex", temporary3);
		Graphics.Blit(source, destination, this.Material, 7);
		RenderTexture.ReleaseTemporary(temporary3);
	}

	// Token: 0x04000488 RID: 1160
	public Texture2D NoiseTexture;

	// Token: 0x04000489 RID: 1161
	public bool UseHighPrecisionDepthMap;

	// Token: 0x0400048A RID: 1162
	public SSAOPro.SampleCount Samples = SSAOPro.SampleCount.Medium;

	// Token: 0x0400048B RID: 1163
	[Range(1f, 4f)]
	public int Downsampling = 1;

	// Token: 0x0400048C RID: 1164
	[Range(0.01f, 1.25f)]
	public float Radius = 0.12f;

	// Token: 0x0400048D RID: 1165
	[Range(0f, 16f)]
	public float Intensity = 2.5f;

	// Token: 0x0400048E RID: 1166
	[Range(0f, 10f)]
	public float Distance = 1f;

	// Token: 0x0400048F RID: 1167
	[Range(0f, 1f)]
	public float Bias = 0.1f;

	// Token: 0x04000490 RID: 1168
	[Range(0f, 1f)]
	public float LumContribution = 0.5f;

	// Token: 0x04000491 RID: 1169
	[ColorUsage(false)]
	public Color OcclusionColor = Color.black;

	// Token: 0x04000492 RID: 1170
	public float CutoffDistance = 150f;

	// Token: 0x04000493 RID: 1171
	public float CutoffFalloff = 50f;

	// Token: 0x04000494 RID: 1172
	public SSAOPro.BlurMode Blur = SSAOPro.BlurMode.HighQualityBilateral;

	// Token: 0x04000495 RID: 1173
	public bool BlurDownsampling;

	// Token: 0x04000496 RID: 1174
	[Range(1f, 4f)]
	public int BlurPasses = 1;

	// Token: 0x04000497 RID: 1175
	[Range(1f, 20f)]
	public float BlurBilateralThreshold = 10f;

	// Token: 0x04000498 RID: 1176
	public bool DebugAO;

	// Token: 0x04000499 RID: 1177
	protected Shader m_ShaderSSAO;

	// Token: 0x0400049A RID: 1178
	protected Material m_Material;

	// Token: 0x0400049B RID: 1179
	protected Camera m_Camera;

	// Token: 0x020003A2 RID: 930
	public enum BlurMode
	{
		// Token: 0x04001354 RID: 4948
		None,
		// Token: 0x04001355 RID: 4949
		Gaussian,
		// Token: 0x04001356 RID: 4950
		HighQualityBilateral
	}

	// Token: 0x020003A3 RID: 931
	public enum SampleCount
	{
		// Token: 0x04001358 RID: 4952
		VeryLow,
		// Token: 0x04001359 RID: 4953
		Low,
		// Token: 0x0400135A RID: 4954
		Medium,
		// Token: 0x0400135B RID: 4955
		High,
		// Token: 0x0400135C RID: 4956
		Ultra
	}

	// Token: 0x020003A4 RID: 932
	protected enum Pass
	{
		// Token: 0x0400135E RID: 4958
		Clear,
		// Token: 0x0400135F RID: 4959
		GaussianBlur = 5,
		// Token: 0x04001360 RID: 4960
		HighQualityBilateralBlur,
		// Token: 0x04001361 RID: 4961
		Composite
	}
}
