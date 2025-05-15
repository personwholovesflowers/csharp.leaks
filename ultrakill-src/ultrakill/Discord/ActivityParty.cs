using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200066C RID: 1644
	public struct ActivityParty
	{
		// Token: 0x04002F57 RID: 12119
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Id;

		// Token: 0x04002F58 RID: 12120
		public PartySize Size;
	}
}
