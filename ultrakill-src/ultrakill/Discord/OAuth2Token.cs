using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000666 RID: 1638
	public struct OAuth2Token
	{
		// Token: 0x04002F47 RID: 12103
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string AccessToken;

		// Token: 0x04002F48 RID: 12104
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public string Scopes;

		// Token: 0x04002F49 RID: 12105
		public long Expires;
	}
}
