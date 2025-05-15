using System;

// Token: 0x0200015C RID: 348
public interface IPlatformAchievements
{
	// Token: 0x06000831 RID: 2097
	void UnlockAchievement(AchievementID achievement);

	// Token: 0x06000832 RID: 2098
	bool IsAchievementUnlocked(AchievementID achievement);
}
