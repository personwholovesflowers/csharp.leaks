using System;

namespace AmplifyImpostors
{
	// Token: 0x02000318 RID: 792
	[Serializable]
	public class VersionInfo
	{
		// Token: 0x0600141B RID: 5147 RVA: 0x0006B964 File Offset: 0x00069B64
		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 0, 9, 7) + ((VersionInfo.Revision > 0) ? ("r" + VersionInfo.Revision.ToString()) : "");
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x0006B9B6 File Offset: 0x00069BB6
		public static int FullNumber
		{
			get
			{
				return 9700 + (int)VersionInfo.Revision;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0006B9C3 File Offset: 0x00069BC3
		public static string FullLabel
		{
			get
			{
				return "Version=" + VersionInfo.FullNumber;
			}
		}

		// Token: 0x0400102E RID: 4142
		public const byte Major = 0;

		// Token: 0x0400102F RID: 4143
		public const byte Minor = 9;

		// Token: 0x04001030 RID: 4144
		public const byte Release = 7;

		// Token: 0x04001031 RID: 4145
		public static byte Revision = 1;
	}
}
