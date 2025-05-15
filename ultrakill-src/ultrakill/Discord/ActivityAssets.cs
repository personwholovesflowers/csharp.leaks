using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200066A RID: 1642
	public struct ActivityAssets
	{
		// Token: 0x04002F51 RID: 12113
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeImage;

		// Token: 0x04002F52 RID: 12114
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string LargeText;

		// Token: 0x04002F53 RID: 12115
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallImage;

		// Token: 0x04002F54 RID: 12116
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string SmallText;
	}
}
