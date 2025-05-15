using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200013A RID: 314
[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Blur/Motion Blur (Color Accumulation)")]
[RequireComponent(typeof(Camera))]
public class MotionBlur : ImageEffectBase
{
	// Token: 0x0600075E RID: 1886 RVA: 0x000260DA File Offset: 0x000242DA
	protected override void Start()
	{
		if (!SystemInfo.supportsRenderTextures)
		{
			base.enabled = false;
			return;
		}
		base.Start();
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x000260F1 File Offset: 0x000242F1
	protected override void OnDisable()
	{
		base.OnDisable();
		Object.DestroyImmediate(this.accumTexture);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00026104 File Offset: 0x00024304
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (this.accumTexture == null || this.accumTexture.width != source.width || this.accumTexture.height != source.height)
		{
			Object.DestroyImmediate(this.accumTexture);
			this.accumTexture = new RenderTexture(source.width, source.height, 0);
			this.accumTexture.hideFlags = HideFlags.HideAndDontSave;
			Graphics.Blit(source, this.accumTexture);
		}
		if (this.extraBlur)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0);
			this.accumTexture.MarkRestoreExpected();
			Graphics.Blit(this.accumTexture, temporary);
			Graphics.Blit(temporary, this.accumTexture);
			RenderTexture.ReleaseTemporary(temporary);
		}
		this.blurAmount = Mathf.Clamp(this.blurAmount, 0f, 0.92f);
		base.material.SetTexture("_MainTex", this.accumTexture);
		base.material.SetFloat("_AccumOrig", 1f - this.blurAmount);
		this.accumTexture.MarkRestoreExpected();
		Graphics.Blit(source, this.accumTexture, base.material);
		Graphics.Blit(this.accumTexture, destination);
	}

	// Token: 0x0400078B RID: 1931
	public float blurAmount = 0.8f;

	// Token: 0x0400078C RID: 1932
	public bool extraBlur;

	// Token: 0x0400078D RID: 1933
	private RenderTexture accumTexture;
}
