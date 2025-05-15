using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public static class Consts
{
	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000444 RID: 1092 RVA: 0x0001D44E File Offset: 0x0001B64E
	public static bool CONSOLE_ERROR_BADGE
	{
		get
		{
			return Debug.isDebugBuild;
		}
	}

	// Token: 0x040005C8 RID: 1480
	public const bool STEAM_SUPPORTED = true;

	// Token: 0x040005C9 RID: 1481
	public const bool DISCORD_SUPPORTED = true;

	// Token: 0x040005CA RID: 1482
	public const bool LEADERBOARDS_SUPPORTED = true;

	// Token: 0x040005CB RID: 1483
	public const bool UNITY_MANAGED_SAVES = false;

	// Token: 0x040005CC RID: 1484
	public const bool ENSURE_NO_FREEZE = false;

	// Token: 0x040005CD RID: 1485
	public const bool AGONY_BUILD = false;

	// Token: 0x040005CE RID: 1486
	public const bool AMAZON_LUNA_BUILD = false;
}
