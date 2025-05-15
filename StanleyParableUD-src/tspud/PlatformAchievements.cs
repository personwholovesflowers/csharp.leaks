using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200014C RID: 332
public static class PlatformAchievements
{
	// Token: 0x1700008D RID: 141
	// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00027289 File Offset: 0x00025489
	public static IPlatformAchievements DebugInstance
	{
		get
		{
			return PlatformAchievements.platformAch;
		}
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060007C1 RID: 1985 RVA: 0x00027290 File Offset: 0x00025490
	// (remove) Token: 0x060007C2 RID: 1986 RVA: 0x000272C4 File Offset: 0x000254C4
	public static event Action<AchievementID> OnAchievementUnlockedFirstTime;

	// Token: 0x060007C3 RID: 1987 RVA: 0x000272F7 File Offset: 0x000254F7
	public static void InitPlatformAchievements(IPlatformAchievements achievements)
	{
		PlatformAchievements.platformAch = achievements;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x000272FF File Offset: 0x000254FF
	public static void UnlockAchievement(AchievementID achievement)
	{
		if (!PlatformAchievements.IsAchievementUnlocked(achievement))
		{
			Action<AchievementID> onAchievementUnlockedFirstTime = PlatformAchievements.OnAchievementUnlockedFirstTime;
			if (onAchievementUnlockedFirstTime != null)
			{
				onAchievementUnlockedFirstTime(achievement);
			}
		}
		IPlatformAchievements platformAchievements = PlatformAchievements.platformAch;
		if (platformAchievements == null)
		{
			return;
		}
		platformAchievements.UnlockAchievement(achievement);
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x0002732A File Offset: 0x0002552A
	public static bool IsAchievementUnlocked(AchievementID achievement)
	{
		IPlatformAchievements platformAchievements = PlatformAchievements.platformAch;
		return platformAchievements != null && platformAchievements.IsAchievementUnlocked(achievement);
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x060007C6 RID: 1990 RVA: 0x0002733D File Offset: 0x0002553D
	public static IEnumerable<AchievementID> AllAchievmentIDs
	{
		get
		{
			foreach (object obj in Enum.GetValues(typeof(AchievementID)))
			{
				AchievementID achievementID = (AchievementID)obj;
				if (achievementID != AchievementID.NumAchievements)
				{
					yield return achievementID;
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x00027348 File Offset: 0x00025548
	public static bool AllAchievementsUnlocked()
	{
		using (IEnumerator<AchievementID> enumerator = PlatformAchievements.AllAchievmentIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!PlatformAchievements.IsAchievementUnlocked(enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0002739C File Offset: 0x0002559C
	public static int AchievementsUnlockedCount
	{
		get
		{
			int num = 0;
			using (IEnumerator<AchievementID> enumerator = PlatformAchievements.AllAchievmentIDs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (PlatformAchievements.IsAchievementUnlocked(enumerator.Current))
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000273F0 File Offset: 0x000255F0
	public static int AchievementsCount
	{
		get
		{
			return 11;
		}
	}

	// Token: 0x040007D7 RID: 2007
	private static IPlatformAchievements platformAch;

	// Token: 0x040007D8 RID: 2008
	public static Action<AchievementID> AchievementUnlocked;
}
