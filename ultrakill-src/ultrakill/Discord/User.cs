using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000665 RID: 1637
	public struct User
	{
		// Token: 0x04002F42 RID: 12098
		public long Id;

		// Token: 0x04002F43 RID: 12099
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Username;

		// Token: 0x04002F44 RID: 12100
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string Discriminator;

		// Token: 0x04002F45 RID: 12101
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Avatar;

		// Token: 0x04002F46 RID: 12102
		public bool Bot;
	}
}
