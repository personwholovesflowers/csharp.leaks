using System;
using Steamworks;

// Token: 0x02000153 RID: 339
public class PCAchievements : IPlatformAchievements
{
	// Token: 0x060007E7 RID: 2023 RVA: 0x000276FD File Offset: 0x000258FD
	public PCAchievements()
	{
		this.Init();
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00005444 File Offset: 0x00003644
	public void Init()
	{
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0002770C File Offset: 0x0002590C
	public bool IsAchievementUnlocked(AchievementID achievement)
	{
		string achievementSteamID = this.GetAchievementSteamID(achievement);
		bool flag;
		try
		{
			SteamUserStats.GetAchievement(achievementSteamID, out flag);
		}
		catch (InvalidOperationException)
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00027744 File Offset: 0x00025944
	public void UnlockAchievement(AchievementID achievement)
	{
		if (this.IsAchievementUnlocked(achievement))
		{
			return;
		}
		string achievementSteamID = this.GetAchievementSteamID(achievement);
		try
		{
			SteamUserStats.SetAchievement(achievementSteamID);
			SteamUserStats.StoreStats();
		}
		catch (InvalidOperationException)
		{
		}
		if (PlatformAchievements.AchievementUnlocked != null)
		{
			PlatformAchievements.AchievementUnlocked(achievement);
		}
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00027798 File Offset: 0x00025998
	private string GetAchievementSteamID(AchievementID id)
	{
		string text = "";
		switch (id)
		{
		case AchievementID.First:
			text = "first";
			break;
		case AchievementID.BeatTheGame:
			text = "beatgame";
			break;
		case AchievementID.TestPlsIgnore:
			text = "testplsignore";
			break;
		case AchievementID.WelcomeBack:
			text = "welcomeback";
			break;
		case AchievementID.YouCantJump:
			text = "nojump";
			break;
		case AchievementID.Tuesday:
			text = "tuesday";
			break;
		case AchievementID.EightEightEightEight:
			text = "8888";
			break;
		case AchievementID.Click430FiveTimes:
			text = "430";
			break;
		case AchievementID.SpeedRun:
			text = "speedrun";
			break;
		case AchievementID.SettingsWorldChampion:
			text = "settingsworldchamp";
			break;
		case AchievementID.SuperGoOutside:
			text = "supergooutside";
			break;
		}
		return text;
	}

	// Token: 0x04000816 RID: 2070
	private bool initialized;
}
