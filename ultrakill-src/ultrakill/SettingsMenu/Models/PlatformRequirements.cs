using System;

namespace SettingsMenu.Models
{
	// Token: 0x02000529 RID: 1321
	[Serializable]
	public class PlatformRequirements
	{
		// Token: 0x06001E17 RID: 7703 RVA: 0x000FA348 File Offset: 0x000F8548
		public bool Check()
		{
			bool flag = this.requiresSteam;
			bool flag2 = this.requiresDiscord;
			bool flag3 = this.requiresFileSystemAccess;
			return !this.hideInCloudManaged || !PlatformRequirements.IsCloudManagedRelease();
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000FA371 File Offset: 0x000F8571
		public static bool IsCloudManagedRelease()
		{
			return Environment.GetEnvironmentVariable("SOLSTICE_LAUNCH_MODE") == "RELEASE";
		}

		// Token: 0x04002A89 RID: 10889
		public bool requiresSteam;

		// Token: 0x04002A8A RID: 10890
		public bool requiresDiscord;

		// Token: 0x04002A8B RID: 10891
		public bool requiresFileSystemAccess;

		// Token: 0x04002A8C RID: 10892
		public bool hideInCloudManaged;
	}
}
