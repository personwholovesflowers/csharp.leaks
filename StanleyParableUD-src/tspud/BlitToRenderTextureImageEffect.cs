using System;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class BlitToRenderTextureImageEffect : MonoBehaviour
{
	// Token: 0x0600068C RID: 1676 RVA: 0x0002337C File Offset: 0x0002157C
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (this.renderTexture == null || this.renderTexture.height != Screen.height || this.renderTexture.width != Screen.width)
		{
			if (this.renderTexture == null)
			{
				Object.Destroy(this.renderTexture);
			}
			this.renderTexture = new RenderTexture(Screen.width, Screen.height, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			this.targetMaterial.SetTexture("_MainTex", this.renderTexture);
		}
		Graphics.Blit(src, this.renderTexture);
		Graphics.Blit(src, dst);
	}

	// Token: 0x040006DF RID: 1759
	public Material targetMaterial;

	// Token: 0x040006E0 RID: 1760
	private RenderTexture renderTexture;
}
