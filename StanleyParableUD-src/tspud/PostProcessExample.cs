using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[ExecuteInEditMode]
public class PostProcessExample : MonoBehaviour
{
	// Token: 0x0600005C RID: 92 RVA: 0x00004AC9 File Offset: 0x00002CC9
	private void Awake()
	{
		if (this.PostProcessMat == null)
		{
			base.enabled = false;
			return;
		}
		this.PostProcessMat.mainTexture = this.PostProcessMat.mainTexture;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004AF7 File Offset: 0x00002CF7
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, this.PostProcessMat);
	}

	// Token: 0x0400007D RID: 125
	public Material PostProcessMat;
}
