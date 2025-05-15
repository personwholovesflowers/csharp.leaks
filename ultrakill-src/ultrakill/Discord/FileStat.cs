using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000672 RID: 1650
	public struct FileStat
	{
		// Token: 0x04002F71 RID: 12145
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Filename;

		// Token: 0x04002F72 RID: 12146
		public ulong Size;

		// Token: 0x04002F73 RID: 12147
		public ulong LastModified;
	}
}
