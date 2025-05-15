using System;

// Token: 0x0200020E RID: 526
public static class GameDifficultyExtensions
{
	// Token: 0x06000B1D RID: 2845 RVA: 0x0004FF08 File Offset: 0x0004E108
	public static string GetDifficultyName(this GameDifficulty difficulty)
	{
		string text;
		switch (difficulty)
		{
		case GameDifficulty.Harmless:
			text = "Harmless";
			break;
		case GameDifficulty.Lenient:
			text = "Lenient";
			break;
		case GameDifficulty.Standard:
			text = "Standard";
			break;
		case GameDifficulty.Violent:
			text = "Violent";
			break;
		case GameDifficulty.Brutal:
			text = "Brutal";
			break;
		case GameDifficulty.UKMD:
			text = "Ultrakill Must Die";
			break;
		default:
			text = "None";
			break;
		}
		return text;
	}
}
