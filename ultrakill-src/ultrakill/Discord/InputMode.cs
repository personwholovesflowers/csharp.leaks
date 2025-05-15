using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000676 RID: 1654
	public struct InputMode
	{
		// Token: 0x04002F7D RID: 12157
		public InputModeType Type;

		// Token: 0x04002F7E RID: 12158
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string Shortcut;
	}
}
