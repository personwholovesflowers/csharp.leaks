using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class EasyPortalPostEffect : MonoBehaviour
{
	// Token: 0x060001B9 RID: 441 RVA: 0x0000C934 File Offset: 0x0000AB34
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, this.postprocessMaterial);
	}

	// Token: 0x04000204 RID: 516
	public Material postprocessMaterial;
}
