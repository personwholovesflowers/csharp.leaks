using System;
using UnityEngine;

// Token: 0x02000235 RID: 565
public class GreedDoorTorch : MonoBehaviour
{
	// Token: 0x06000C0B RID: 3083 RVA: 0x0005457A File Offset: 0x0005277A
	private void Start()
	{
		this.dr = base.GetComponentInParent<Door>();
		this.lt = base.GetComponent<Light>();
		this.sprs = base.GetComponentsInChildren<SpriteRenderer>();
		this.UpdateColor();
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x000545A6 File Offset: 0x000527A6
	private void Update()
	{
		if (this.clr != this.dr.currentLightsColor)
		{
			this.UpdateColor();
		}
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x000545C8 File Offset: 0x000527C8
	private void UpdateColor()
	{
		this.clr = this.dr.currentLightsColor;
		if (this.sprs.Length != 0)
		{
			foreach (SpriteRenderer spriteRenderer in this.sprs)
			{
				spriteRenderer.color = new Color(this.clr.r, this.clr.g, this.clr.b, spriteRenderer.color.a);
			}
		}
		if (this.lt)
		{
			this.lt.color = this.clr;
		}
	}

	// Token: 0x04000FCC RID: 4044
	private Door dr;

	// Token: 0x04000FCD RID: 4045
	private Light lt;

	// Token: 0x04000FCE RID: 4046
	private SpriteRenderer[] sprs;

	// Token: 0x04000FCF RID: 4047
	private Color clr;
}
