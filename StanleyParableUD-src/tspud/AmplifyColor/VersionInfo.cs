using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x0200031C RID: 796
	[Serializable]
	public class VersionInfo
	{
		// Token: 0x06001421 RID: 5153 RVA: 0x0006B9F1 File Offset: 0x00069BF1
		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 1, 8, 0) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0006BA1E File Offset: 0x00069C1E
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", this.m_major, this.m_minor, this.m_release) + VersionInfo.StageSuffix + VersionInfo.TrialSuffix;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x0006BA5A File Offset: 0x00069C5A
		public int Number
		{
			get
			{
				return this.m_major * 100 + this.m_minor * 10 + this.m_release;
			}
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0006BA76 File Offset: 0x00069C76
		private VersionInfo()
		{
			this.m_major = 1;
			this.m_minor = 8;
			this.m_release = 0;
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0006BA93 File Offset: 0x00069C93
		private VersionInfo(byte major, byte minor, byte release)
		{
			this.m_major = (int)major;
			this.m_minor = (int)minor;
			this.m_release = (int)release;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0006BAB0 File Offset: 0x00069CB0
		public static VersionInfo Current()
		{
			return new VersionInfo(1, 8, 0);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0006BABA File Offset: 0x00069CBA
		public static bool Matches(VersionInfo version)
		{
			return 1 == version.m_major && 8 == version.m_minor && version.m_release == 0;
		}

		// Token: 0x0400103C RID: 4156
		public const byte Major = 1;

		// Token: 0x0400103D RID: 4157
		public const byte Minor = 8;

		// Token: 0x0400103E RID: 4158
		public const byte Release = 0;

		// Token: 0x0400103F RID: 4159
		private static string StageSuffix = "_dev004";

		// Token: 0x04001040 RID: 4160
		private static string TrialSuffix = "";

		// Token: 0x04001041 RID: 4161
		[SerializeField]
		private int m_major;

		// Token: 0x04001042 RID: 4162
		[SerializeField]
		private int m_minor;

		// Token: 0x04001043 RID: 4163
		[SerializeField]
		private int m_release;
	}
}
