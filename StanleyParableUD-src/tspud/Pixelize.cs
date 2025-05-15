using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
[RequireComponent(typeof(Camera))]
[ImageEffectAllowedInSceneView]
[ExecuteInEditMode]
public class Pixelize : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000045 RID: 69 RVA: 0x00004526 File Offset: 0x00002726
	private Shader ScreenAndMaskShader
	{
		get
		{
			if (this._screenAndMaskShader == null)
			{
				this._screenAndMaskShader = Shader.Find("Hidden/PostProcess/Pixelize/ScreenAndMask");
			}
			return this._screenAndMaskShader;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000046 RID: 70 RVA: 0x0000454C File Offset: 0x0000274C
	private Material ScreenAndMaskMaterial
	{
		get
		{
			if (this._screenAndMaskMaterial == null)
			{
				this._screenAndMaskMaterial = new Material(this.ScreenAndMaskShader);
			}
			return this._screenAndMaskMaterial;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00004573 File Offset: 0x00002773
	private RenderTexture TemporaryRenderTarget
	{
		get
		{
			if (this._temporaryRenderTexture == null)
			{
				this.CreateTemporaryRenderTarget();
			}
			return this._temporaryRenderTexture;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000048 RID: 72 RVA: 0x0000458F File Offset: 0x0000278F
	private Shader CombineLayersShader
	{
		get
		{
			if (this._combineLayersShader == null)
			{
				this._combineLayersShader = Shader.Find("Hidden/PostProcess/Pixelize/CombineLayers");
			}
			return this._combineLayersShader;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000049 RID: 73 RVA: 0x000045B5 File Offset: 0x000027B5
	private Material CombineLayersMaterial
	{
		get
		{
			if (this._combineLayersMaterial == null)
			{
				this._combineLayersMaterial = new Material(this.CombineLayersShader);
			}
			return this._combineLayersMaterial;
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000045DC File Offset: 0x000027DC
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		this.CheckTemporaryRenderTarget();
		Graphics.Blit(src, this.TemporaryRenderTarget, this.ScreenAndMaskMaterial);
		Graphics.Blit(this.TemporaryRenderTarget, dest, this.CombineLayersMaterial);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00004608 File Offset: 0x00002808
	private void CreateTemporaryRenderTarget()
	{
		this._temporaryRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		this._temporaryRenderTexture.useMipMap = true;
		this._temporaryRenderTexture.autoGenerateMips = true;
		this._temporaryRenderTexture.wrapMode = TextureWrapMode.Clamp;
		this._temporaryRenderTexture.filterMode = FilterMode.Point;
		this._temporaryRenderTexture.Create();
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00004669 File Offset: 0x00002869
	private void CheckTemporaryRenderTarget()
	{
		if (this.TemporaryRenderTarget.width != Screen.width || this.TemporaryRenderTarget.width != Screen.height)
		{
			this.ReleaseTemporaryRenderTarget();
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00004695 File Offset: 0x00002895
	private void ReleaseTemporaryRenderTarget()
	{
		this._temporaryRenderTexture.Release();
		this._temporaryRenderTexture = null;
	}

	// Token: 0x04000067 RID: 103
	private Shader _screenAndMaskShader;

	// Token: 0x04000068 RID: 104
	private Material _screenAndMaskMaterial;

	// Token: 0x04000069 RID: 105
	private RenderTexture _temporaryRenderTexture;

	// Token: 0x0400006A RID: 106
	private Shader _combineLayersShader;

	// Token: 0x0400006B RID: 107
	private Material _combineLayersMaterial;
}
