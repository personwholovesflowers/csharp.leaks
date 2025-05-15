using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x0200066E RID: 1646
	public struct Activity
	{
		// Token: 0x04002F5C RID: 12124
		public ActivityType Type;

		// Token: 0x04002F5D RID: 12125
		public long ApplicationId;

		// Token: 0x04002F5E RID: 12126
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Name;

		// Token: 0x04002F5F RID: 12127
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string State;

		// Token: 0x04002F60 RID: 12128
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string Details;

		// Token: 0x04002F61 RID: 12129
		public ActivityTimestamps Timestamps;

		// Token: 0x04002F62 RID: 12130
		public ActivityAssets Assets;

		// Token: 0x04002F63 RID: 12131
		public ActivityParty Party;

		// Token: 0x04002F64 RID: 12132
		public ActivitySecrets Secrets;

		// Token: 0x04002F65 RID: 12133
		public bool Instance;
	}
}
