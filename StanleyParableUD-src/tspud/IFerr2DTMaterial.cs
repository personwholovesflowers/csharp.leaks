using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
public interface IFerr2DTMaterial
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x0600023F RID: 575
	string name { get; }

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000240 RID: 576
	// (set) Token: 0x06000241 RID: 577
	Material fillMaterial { get; set; }

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000242 RID: 578
	// (set) Token: 0x06000243 RID: 579
	Material edgeMaterial { get; set; }

	// Token: 0x06000244 RID: 580
	Ferr2DT_SegmentDescription GetDescriptor(Ferr2DT_TerrainDirection aDirection);

	// Token: 0x06000245 RID: 581
	bool Has(Ferr2DT_TerrainDirection aDirection);

	// Token: 0x06000246 RID: 582
	void Set(Ferr2DT_TerrainDirection aDirection, bool aActive);

	// Token: 0x06000247 RID: 583
	Rect GetBody(Ferr2DT_TerrainDirection aDirection, int aBodyID);

	// Token: 0x06000248 RID: 584
	Rect ToUV(Rect aNativeRect);

	// Token: 0x06000249 RID: 585
	Rect ToScreen(Rect aNativeRect);

	// Token: 0x0600024A RID: 586
	Rect ToNative(Rect aPixelRect);

	// Token: 0x0600024B RID: 587
	Rect ToPixels(Rect aNativeRect);
}
