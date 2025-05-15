using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
[Serializable]
public class Ferr2DT_SegmentDescription
{
	// Token: 0x0600023E RID: 574 RVA: 0x000104A0 File Offset: 0x0000E6A0
	public Ferr2DT_SegmentDescription()
	{
		this.body = new Rect[]
		{
			new Rect(0f, 0f, 50f, 50f)
		};
		this.applyTo = Ferr2DT_TerrainDirection.Top;
	}

	// Token: 0x0400025C RID: 604
	public Ferr2DT_TerrainDirection applyTo;

	// Token: 0x0400025D RID: 605
	public float zOffset;

	// Token: 0x0400025E RID: 606
	public float yOffset;

	// Token: 0x0400025F RID: 607
	public Rect leftCap;

	// Token: 0x04000260 RID: 608
	public Rect innerLeftCap;

	// Token: 0x04000261 RID: 609
	public Rect rightCap;

	// Token: 0x04000262 RID: 610
	public Rect innerRightCap;

	// Token: 0x04000263 RID: 611
	public Rect[] body;

	// Token: 0x04000264 RID: 612
	public float capOffset;
}
