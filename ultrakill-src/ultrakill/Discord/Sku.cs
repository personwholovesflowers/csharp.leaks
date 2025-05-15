using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000675 RID: 1653
	public struct Sku
	{
		// Token: 0x04002F79 RID: 12153
		public long Id;

		// Token: 0x04002F7A RID: 12154
		public SkuType Type;

		// Token: 0x04002F7B RID: 12155
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Name;

		// Token: 0x04002F7C RID: 12156
		public SkuPrice Price;
	}
}
