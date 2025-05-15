using System;

namespace Polybrush
{
	// Token: 0x02000218 RID: 536
	[Flags]
	public enum z_MeshChannel
	{
		// Token: 0x04000B9C RID: 2972
		Null = 0,
		// Token: 0x04000B9D RID: 2973
		Position = 0,
		// Token: 0x04000B9E RID: 2974
		Normal = 1,
		// Token: 0x04000B9F RID: 2975
		Color = 2,
		// Token: 0x04000BA0 RID: 2976
		Tangent = 4,
		// Token: 0x04000BA1 RID: 2977
		UV0 = 8,
		// Token: 0x04000BA2 RID: 2978
		UV2 = 16,
		// Token: 0x04000BA3 RID: 2979
		UV3 = 32,
		// Token: 0x04000BA4 RID: 2980
		UV4 = 64,
		// Token: 0x04000BA5 RID: 2981
		All = 255
	}
}
