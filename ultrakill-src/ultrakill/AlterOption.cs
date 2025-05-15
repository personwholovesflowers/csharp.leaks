using System;
using UnityEngine;

// Token: 0x02000424 RID: 1060
[Serializable]
public struct AlterOption
{
	// Token: 0x04002186 RID: 8582
	public string targetKey;

	// Token: 0x04002187 RID: 8583
	[Space]
	public bool useInt;

	// Token: 0x04002188 RID: 8584
	public int intValue;

	// Token: 0x04002189 RID: 8585
	[Space]
	public bool useFloat;

	// Token: 0x0400218A RID: 8586
	public float floatValue;

	// Token: 0x0400218B RID: 8587
	[Space]
	public bool useBool;

	// Token: 0x0400218C RID: 8588
	public bool boolValue;

	// Token: 0x0400218D RID: 8589
	[Space]
	public bool useVector;

	// Token: 0x0400218E RID: 8590
	public Vector3 vectorValue;
}
