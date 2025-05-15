using System;
using System.Runtime.InteropServices;

namespace Discord
{
	// Token: 0x02000677 RID: 1655
	public struct UserAchievement
	{
		// Token: 0x04002F7F RID: 12159
		public long UserId;

		// Token: 0x04002F80 RID: 12160
		public long AchievementId;

		// Token: 0x04002F81 RID: 12161
		public byte PercentComplete;

		// Token: 0x04002F82 RID: 12162
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string UnlockedAt;
	}
}
