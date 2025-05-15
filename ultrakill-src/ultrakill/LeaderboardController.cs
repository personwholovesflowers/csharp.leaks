using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

// Token: 0x0200029B RID: 667
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class LeaderboardController : MonoSingleton<LeaderboardController>
{
	// Token: 0x06000EAF RID: 3759 RVA: 0x0006CF34 File Offset: 0x0006B134
	public async void SubmitCyberGrindScore(int difficulty, float wave, int kills, int style, float seconds)
	{
		if (SteamClient.IsValid)
		{
			if (GameStateManager.CanSubmitScores)
			{
				int majorVersion = -1;
				int minorVersion = -1;
				string[] array = Application.version.Split('.', StringSplitOptions.None);
				int num;
				if (int.TryParse(array[0], out num))
				{
					majorVersion = num;
				}
				int num2;
				if (array.Length > 1 && int.TryParse(array[1], out num2))
				{
					minorVersion = num2;
				}
				int startWave = MonoSingleton<EndlessGrid>.Instance.startWave;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("Cyber Grind Wave ");
				stringBuilder.Append(LeaderboardProperties.Difficulties[difficulty]);
				Leaderboard? leaderboard = await this.FetchLeaderboard(stringBuilder.ToString(), false, LeaderboardSort.Descending);
				if (leaderboard != null)
				{
					await leaderboard.Value.SubmitScoreAsync(Mathf.FloorToInt(wave), new int[]
					{
						kills,
						style,
						Mathf.FloorToInt(seconds * 1000f),
						majorVersion,
						minorVersion,
						DateTimeOffset.UtcNow.Millisecond,
						startWave
					});
					stringBuilder.Append(" Precise");
					Leaderboard? leaderboard2 = await this.FetchLeaderboard(stringBuilder.ToString(), false, LeaderboardSort.Descending);
					if (leaderboard2 != null)
					{
						await leaderboard2.Value.SubmitScoreAsync(Mathf.FloorToInt(wave * 1000f), new int[]
						{
							kills,
							style,
							Mathf.FloorToInt(seconds * 1000f),
							majorVersion,
							minorVersion,
							DateTimeOffset.UtcNow.Millisecond,
							startWave
						});
						Debug.Log(string.Format("Score {0} submitted to Steamworks", wave));
					}
				}
			}
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0006CF98 File Offset: 0x0006B198
	public async void SubmitLevelScore(string levelName, int difficulty, float seconds, int kills, int style, int restartCount, bool pRank = false)
	{
		if (SteamClient.IsValid)
		{
			int majorVersion = -1;
			int minorVersion = -1;
			string[] array = Application.version.Split('.', StringSplitOptions.None);
			int num;
			if (int.TryParse(array[0], out num))
			{
				majorVersion = num;
			}
			int num2;
			if (array.Length > 1 && int.TryParse(array[1], out num2))
			{
				minorVersion = num2;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(levelName);
			if (pRank)
			{
				stringBuilder.Append(" PRank");
			}
			else
			{
				stringBuilder.Append(" Any%");
			}
			Leaderboard? leaderboard = await this.FetchLeaderboard(stringBuilder.ToString(), true, LeaderboardSort.Ascending);
			if (leaderboard != null)
			{
				Leaderboard value = leaderboard.Value;
				int num3 = Mathf.FloorToInt(seconds * 1000f + 0.5f);
				await value.SubmitScoreAsync(num3, new int[]
				{
					difficulty,
					kills,
					style,
					restartCount,
					majorVersion,
					minorVersion,
					DateTimeOffset.UtcNow.Millisecond
				});
				Debug.Log(string.Format("Score {0}s submitted to {1} Steamworks", seconds, stringBuilder));
			}
		}
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0006D00C File Offset: 0x0006B20C
	public async Task<LeaderboardEntry[]> GetLevelScores(string levelName, bool pRank)
	{
		LeaderboardEntry[] array;
		if (!SteamClient.IsValid)
		{
			array = null;
		}
		else if (!levelName.StartsWith("Level "))
		{
			array = null;
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(levelName);
			if (pRank)
			{
				stringBuilder.Append(" PRank");
			}
			else
			{
				stringBuilder.Append(" Any%");
			}
			array = await this.FetchLeaderboardEntries(stringBuilder.ToString(), LeaderboardType.Friends, 15, true, LeaderboardSort.Ascending);
		}
		return array;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0006D060 File Offset: 0x0006B260
	public async Task<LeaderboardEntry[]> GetCyberGrindScores(int difficulty, LeaderboardType type)
	{
		LeaderboardEntry[] array;
		if (!SteamClient.IsValid)
		{
			array = null;
		}
		else
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Cyber Grind Wave ");
			stringBuilder.Append(LeaderboardProperties.Difficulties[difficulty]);
			stringBuilder.Append(" Precise");
			array = await this.FetchLeaderboardEntries(stringBuilder.ToString(), type, 10, false, LeaderboardSort.Descending);
		}
		return array;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0006D0B4 File Offset: 0x0006B2B4
	public async Task<LeaderboardEntry[]> GetFishScores(LeaderboardType type)
	{
		LeaderboardEntry[] array;
		if (!SteamClient.IsValid)
		{
			array = null;
		}
		else
		{
			array = (await this.FetchLeaderboardEntries("Fish Size", type, 20, false, LeaderboardSort.Descending)).Where((LeaderboardEntry fs) => fs.Score == 1 || SteamController.BuiltInVerifiedSteamIds.Contains(fs.User.Id.Value)).Take(20).ToArray<LeaderboardEntry>();
		}
		return array;
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0006D100 File Offset: 0x0006B300
	public async void SubmitFishSize(int fishSize)
	{
		if (SteamClient.IsValid)
		{
			int majorVersion = -1;
			int minorVersion = -1;
			string[] array = Application.version.Split('.', StringSplitOptions.None);
			int num;
			if (int.TryParse(array[0], out num))
			{
				majorVersion = num;
			}
			int num2;
			if (array.Length > 1 && int.TryParse(array[1], out num2))
			{
				minorVersion = num2;
			}
			Leaderboard? leaderboard = await this.FetchLeaderboard("Fish Size", false, LeaderboardSort.Descending);
			if (leaderboard != null)
			{
				Leaderboard value = leaderboard.Value;
				if (SteamController.BuiltInVerifiedSteamIds.Contains(SteamClient.SteamId))
				{
					await value.ReplaceScore(fishSize, new int[]
					{
						majorVersion,
						minorVersion,
						DateTimeOffset.UtcNow.Millisecond
					});
				}
				else
				{
					await value.ReplaceScore(Mathf.FloorToInt((float)1), new int[]
					{
						majorVersion,
						minorVersion,
						DateTimeOffset.UtcNow.Millisecond
					});
				}
				Debug.Log("Fish submitted to Steamworks");
			}
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0006D140 File Offset: 0x0006B340
	private async Task<LeaderboardEntry[]> FetchLeaderboardEntries(string key, LeaderboardType type, int count = 10, bool createIfNotFound = false, LeaderboardSort defaultSortMode = LeaderboardSort.Descending)
	{
		LeaderboardEntry[] array;
		if (!SteamClient.IsValid)
		{
			array = null;
		}
		else
		{
			Leaderboard? leaderboard = await this.FetchLeaderboard(key, createIfNotFound, defaultSortMode);
			if (leaderboard == null)
			{
				array = null;
			}
			else
			{
				Leaderboard value = leaderboard.Value;
				LeaderboardEntry[] array2;
				switch (type)
				{
				case LeaderboardType.GlobalAround:
					array2 = await value.GetScoresAroundUserAsync(-4, 3);
					break;
				case LeaderboardType.Global:
					array2 = await value.GetScoresAsync(count, 1);
					break;
				case LeaderboardType.Friends:
					array2 = await value.GetScoresFromFriendsAsync();
					break;
				default:
					throw new ArgumentOutOfRangeException("type", type, null);
				}
				if (array2 == null)
				{
					array = new LeaderboardEntry[0];
				}
				else
				{
					array2 = array2.Take(count).ToArray<LeaderboardEntry>();
					array = array2;
				}
			}
		}
		return array;
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x0006D1B0 File Offset: 0x0006B3B0
	private async Task<Leaderboard?> FetchLeaderboard(string key, bool createIfNotFound = false, LeaderboardSort defaultSortMode = LeaderboardSort.Descending)
	{
		Leaderboard leaderboard;
		Leaderboard? leaderboard2;
		if (this.cachedLeaderboards.TryGetValue(key, out leaderboard))
		{
			Debug.Log("Resolved leaderboard '" + key + "' from cache");
			leaderboard2 = new Leaderboard?(leaderboard);
		}
		else
		{
			Leaderboard? leaderboard3 = await SteamController.FetchSteamLeaderboard(key, createIfNotFound, defaultSortMode, LeaderboardDisplay.TimeMilliSeconds);
			if (leaderboard3 == null)
			{
				Debug.LogError("Failed to resolve leaderboard '" + key + "' from Steamworks");
				leaderboard2 = null;
			}
			else
			{
				Leaderboard value = leaderboard3.Value;
				this.cachedLeaderboards.Add(key, value);
				Debug.Log("Resolved leaderboard '" + key + "' from Steamworks");
				leaderboard2 = new Leaderboard?(value);
			}
		}
		return leaderboard2;
	}

	// Token: 0x0400137F RID: 4991
	private readonly Dictionary<string, Leaderboard> cachedLeaderboards = new Dictionary<string, Leaderboard>();
}
