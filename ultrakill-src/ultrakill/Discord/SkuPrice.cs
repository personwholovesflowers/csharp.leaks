using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000674 RID: 1652
	public struct SkuPrice
	{
		// Token: 0x04002F77 RID: 12151
		public uint Amount;

		// Token: 0x04002F78 RID: 12152
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Currency;
	}
}
