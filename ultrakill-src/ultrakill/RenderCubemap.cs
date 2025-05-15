using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class RenderCubemap : MonoBehaviour
{
	// Token: 0x06001DB2 RID: 7602 RVA: 0x000F7C14 File Offset: 0x000F5E14
	private void Update()
	{
		if (this.manualRender)
		{
			this.manualRender = false;
			if (this.cam != null && this.cubemap != null)
			{
				this.cam.RenderToCubemap(this.cubemap);
				return;
			}
			Debug.LogError("Camera and/or Cubemap not assigned.");
		}
	}

	// Token: 0x04002A11 RID: 10769
	public bool manualRender;

	// Token: 0x04002A12 RID: 10770
	public Camera cam;

	// Token: 0x04002A13 RID: 10771
	public Cubemap cubemap;
}
