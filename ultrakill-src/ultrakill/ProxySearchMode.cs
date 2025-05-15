using System;

// Token: 0x0200043C RID: 1084
[Flags]
public enum ProxySearchMode
{
	// Token: 0x04002236 RID: 8758
	None = 0,
	// Token: 0x04002237 RID: 8759
	IncludeStatic = 1,
	// Token: 0x04002238 RID: 8760
	IncludeDynamic = 2,
	// Token: 0x04002239 RID: 8761
	FloorOnly = 4,
	// Token: 0x0400223A RID: 8762
	IncludeBurning = 8,
	// Token: 0x0400223B RID: 8763
	IncludeNotBurning = 16,
	// Token: 0x0400223C RID: 8764
	Any = 27,
	// Token: 0x0400223D RID: 8765
	AnyFloor = 31,
	// Token: 0x0400223E RID: 8766
	AnyNotBurning = 19,
	// Token: 0x0400223F RID: 8767
	AnyBurning = 11
}
