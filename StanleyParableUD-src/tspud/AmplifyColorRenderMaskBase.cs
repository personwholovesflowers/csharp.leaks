using System;
using AmplifyColor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

// Token: 0x02000005 RID: 5
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("")]
public class AmplifyColorRenderMaskBase : MonoBehaviour
{
	// Token: 0x06000028 RID: 40 RVA: 0x0000361C File Offset: 0x0000181C
	private void OnEnable()
	{
		if (this.maskCamera == null)
		{
			GameObject gameObject = new GameObject("Mask Camera", new Type[] { typeof(Camera) })
			{
				hideFlags = HideFlags.HideAndDontSave
			};
			gameObject.transform.parent = base.gameObject.transform;
			this.maskCamera = gameObject.GetComponent<Camera>();
		}
		this.referenceCamera = base.GetComponent<Camera>();
		this.colorEffect = base.GetComponent<AmplifyColorBase>();
		this.colorMaskShader = Shader.Find("Hidden/RenderMask");
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000036A7 File Offset: 0x000018A7
	private void OnDisable()
	{
		this.DestroyCamera();
		this.DestroyRenderTextures();
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000036B5 File Offset: 0x000018B5
	private void DestroyCamera()
	{
		if (this.maskCamera != null)
		{
			Object.DestroyImmediate(this.maskCamera.gameObject);
			this.maskCamera = null;
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000036DC File Offset: 0x000018DC
	private void DestroyRenderTextures()
	{
		if (this.maskTexture != null)
		{
			RenderTexture.active = null;
			Object.DestroyImmediate(this.maskTexture);
			this.maskTexture = null;
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00003704 File Offset: 0x00001904
	private void UpdateRenderTextures(bool singlePassStereo)
	{
		int num = this.referenceCamera.pixelWidth;
		int num2 = this.referenceCamera.pixelHeight;
		if (this.maskTexture == null || this.width != num || this.height != num2 || !this.maskTexture.IsCreated() || this.singlePassStereo != singlePassStereo)
		{
			this.width = num;
			this.height = num2;
			this.DestroyRenderTextures();
			if (XRSettings.enabled)
			{
				num = XRSettings.eyeTextureWidth * (singlePassStereo ? 2 : 1);
				num2 = XRSettings.eyeTextureHeight;
			}
			if (this.maskTexture == null)
			{
				this.maskTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB)
				{
					hideFlags = HideFlags.HideAndDontSave,
					name = "MaskTexture"
				};
				this.maskTexture.name = "AmplifyColorMaskTexture";
				bool allowMSAA = this.maskCamera.allowMSAA;
				this.maskTexture.antiAliasing = ((allowMSAA && QualitySettings.antiAliasing > 0) ? QualitySettings.antiAliasing : 1);
			}
			this.maskTexture.Create();
			this.singlePassStereo = singlePassStereo;
		}
		if (this.colorEffect != null)
		{
			this.colorEffect.MaskTexture = this.maskTexture;
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00003830 File Offset: 0x00001A30
	private void UpdateCameraProperties()
	{
		this.maskCamera.CopyFrom(this.referenceCamera);
		this.maskCamera.targetTexture = this.maskTexture;
		this.maskCamera.clearFlags = CameraClearFlags.Nothing;
		this.maskCamera.renderingPath = RenderingPath.VertexLit;
		this.maskCamera.pixelRect = new Rect(0f, 0f, (float)this.width, (float)this.height);
		this.maskCamera.depthTextureMode = DepthTextureMode.None;
		this.maskCamera.allowHDR = false;
		this.maskCamera.enabled = false;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000038C4 File Offset: 0x00001AC4
	private void OnPreRender()
	{
		if (this.maskCamera != null)
		{
			RenderBuffer activeColorBuffer = Graphics.activeColorBuffer;
			RenderBuffer activeDepthBuffer = Graphics.activeDepthBuffer;
			bool flag = false;
			if (this.referenceCamera.stereoEnabled)
			{
				flag = XRSettings.eyeTextureDesc.vrUsage == VRTextureUsage.TwoEyes;
				this.maskCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Left, this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left));
				this.maskCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Right, this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Right));
				this.maskCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Left, this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left));
				this.maskCamera.SetStereoProjectionMatrix(Camera.StereoscopicEye.Right, this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right));
			}
			this.UpdateRenderTextures(flag);
			this.UpdateCameraProperties();
			Graphics.SetRenderTarget(this.maskTexture);
			GL.Clear(true, true, this.ClearColor);
			if (flag)
			{
				this.maskCamera.worldToCameraMatrix = this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Left);
				this.maskCamera.projectionMatrix = this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
				this.maskCamera.rect = new Rect(0f, 0f, 0.5f, 1f);
			}
			foreach (RenderLayer renderLayer in this.RenderLayers)
			{
				Shader.SetGlobalColor("_COLORMASK_Color", renderLayer.color);
				this.maskCamera.cullingMask = renderLayer.mask;
				this.maskCamera.RenderWithShader(this.colorMaskShader, "RenderType");
			}
			if (flag)
			{
				this.maskCamera.worldToCameraMatrix = this.referenceCamera.GetStereoViewMatrix(Camera.StereoscopicEye.Right);
				this.maskCamera.projectionMatrix = this.referenceCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
				this.maskCamera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
				foreach (RenderLayer renderLayer2 in this.RenderLayers)
				{
					Shader.SetGlobalColor("_COLORMASK_Color", renderLayer2.color);
					this.maskCamera.cullingMask = renderLayer2.mask;
					this.maskCamera.RenderWithShader(this.colorMaskShader, "RenderType");
				}
			}
			Graphics.SetRenderTarget(activeColorBuffer, activeDepthBuffer);
		}
	}

	// Token: 0x04000045 RID: 69
	[FormerlySerializedAs("clearColor")]
	public Color ClearColor = Color.black;

	// Token: 0x04000046 RID: 70
	[FormerlySerializedAs("renderLayers")]
	public RenderLayer[] RenderLayers = new RenderLayer[0];

	// Token: 0x04000047 RID: 71
	[FormerlySerializedAs("debug")]
	public bool DebugMask;

	// Token: 0x04000048 RID: 72
	private Camera referenceCamera;

	// Token: 0x04000049 RID: 73
	private Camera maskCamera;

	// Token: 0x0400004A RID: 74
	private AmplifyColorBase colorEffect;

	// Token: 0x0400004B RID: 75
	private int width;

	// Token: 0x0400004C RID: 76
	private int height;

	// Token: 0x0400004D RID: 77
	private RenderTexture maskTexture;

	// Token: 0x0400004E RID: 78
	private Shader colorMaskShader;

	// Token: 0x0400004F RID: 79
	private bool singlePassStereo;
}
