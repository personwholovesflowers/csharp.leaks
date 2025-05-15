using System;

// Token: 0x020000E1 RID: 225
[Serializable]
public enum SourceRenderMode
{
	// Token: 0x040005BF RID: 1471
	Normal,
	// Token: 0x040005C0 RID: 1472
	Color,
	// Token: 0x040005C1 RID: 1473
	Texture,
	// Token: 0x040005C2 RID: 1474
	Glow,
	// Token: 0x040005C3 RID: 1475
	Solid,
	// Token: 0x040005C4 RID: 1476
	Additive,
	// Token: 0x040005C5 RID: 1477
	AdditiveFractionalFrame = 7,
	// Token: 0x040005C6 RID: 1478
	WorldSpaceGlow = 9,
	// Token: 0x040005C7 RID: 1479
	DontRender
}
