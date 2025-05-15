using System;

// Token: 0x020002FE RID: 766
[Flags]
public enum SingletonFlags
{
	// Token: 0x040017BD RID: 6077
	None = 0,
	// Token: 0x040017BE RID: 6078
	NoAutoInstance = 1,
	// Token: 0x040017BF RID: 6079
	HideAutoInstance = 2,
	// Token: 0x040017C0 RID: 6080
	NoAwakeInstance = 4,
	// Token: 0x040017C1 RID: 6081
	PersistAutoInstance = 8,
	// Token: 0x040017C2 RID: 6082
	DestroyDuplicates = 16
}
