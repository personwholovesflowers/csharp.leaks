using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200066D RID: 1645
	public struct ActivitySecrets
	{
		// Token: 0x04002F59 RID: 12121
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Match;

		// Token: 0x04002F5A RID: 12122
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Join;

		// Token: 0x04002F5B RID: 12123
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Spectate;
	}
}
