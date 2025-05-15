using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class InfoOverlay : HammerEntity
{
	// Token: 0x060005BD RID: 1469 RVA: 0x0001FF22 File Offset: 0x0001E122
	private void Awake()
	{
		if (this.render == null)
		{
			this.render = base.GetComponent<MeshRenderer>();
		}
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x0001FF3E File Offset: 0x0001E13E
	public void IncrementTextureIndex()
	{
		this.render.material.SetInt("_Index", this.render.material.GetInt("_Index") + 1);
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0001FF6C File Offset: 0x0001E16C
	public void SetTextureIndex(int index)
	{
		this.render.material.SetInt("_Index", index);
	}

	// Token: 0x040005FF RID: 1535
	public Renderer render;
}
