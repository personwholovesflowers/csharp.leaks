using System;
using UnityEngine;

namespace AmplifyBloom
{
	// Token: 0x0200033B RID: 827
	[Serializable]
	public class VersionInfo
	{
		// Token: 0x06001540 RID: 5440 RVA: 0x000708CC File Offset: 0x0006EACC
		public static string StaticToString()
		{
			return string.Format("{0}.{1}.{2}", 1, 2, 0) + VersionInfo.StageSuffix;
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x000708F4 File Offset: 0x0006EAF4
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}", this.m_major, this.m_minor, this.m_release) + VersionInfo.StageSuffix;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x0007092B File Offset: 0x0006EB2B
		public int Number
		{
			get
			{
				return this.m_major * 100 + this.m_minor * 10 + this.m_release;
			}
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00070947 File Offset: 0x0006EB47
		private VersionInfo()
		{
			this.m_major = 1;
			this.m_minor = 2;
			this.m_release = 0;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00070964 File Offset: 0x0006EB64
		private VersionInfo(byte major, byte minor, byte release)
		{
			this.m_major = (int)major;
			this.m_minor = (int)minor;
			this.m_release = (int)release;
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x00070981 File Offset: 0x0006EB81
		public static VersionInfo Current()
		{
			return new VersionInfo(1, 2, 0);
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0007098B File Offset: 0x0006EB8B
		public static bool Matches(VersionInfo version)
		{
			return 1 == version.m_major && 2 == version.m_minor && version.m_release == 0;
		}

		// Token: 0x04001155 RID: 4437
		public const byte Major = 1;

		// Token: 0x04001156 RID: 4438
		public const byte Minor = 2;

		// Token: 0x04001157 RID: 4439
		public const byte Release = 0;

		// Token: 0x04001158 RID: 4440
		private static string StageSuffix = "_dev001";

		// Token: 0x04001159 RID: 4441
		[SerializeField]
		private int m_major;

		// Token: 0x0400115A RID: 4442
		[SerializeField]
		private int m_minor;

		// Token: 0x0400115B RID: 4443
		[SerializeField]
		private int m_release;
	}
}
