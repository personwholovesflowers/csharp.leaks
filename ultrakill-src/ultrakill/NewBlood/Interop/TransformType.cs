using System;

namespace NewBlood.Interop
{
	// Token: 0x020005FC RID: 1532
	[Flags]
	public enum TransformType : byte
	{
		// Token: 0x04002DE8 RID: 11752
		kNoScaleTransform = 0,
		// Token: 0x04002DE9 RID: 11753
		kUniformScaleTransform = 1,
		// Token: 0x04002DEA RID: 11754
		kNonUniformScaleTransform = 2,
		// Token: 0x04002DEB RID: 11755
		kOddNegativeScaleTransform = 4
	}
}
