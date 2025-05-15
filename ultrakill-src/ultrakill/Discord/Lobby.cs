using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000671 RID: 1649
	public struct Lobby
	{
		// Token: 0x04002F6B RID: 12139
		public long Id;

		// Token: 0x04002F6C RID: 12140
		public LobbyType Type;

		// Token: 0x04002F6D RID: 12141
		public long OwnerId;

		// Token: 0x04002F6E RID: 12142
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Secret;

		// Token: 0x04002F6F RID: 12143
		public uint Capacity;

		// Token: 0x04002F70 RID: 12144
		public bool Locked;
	}
}
