using System;
using System.Linq;
using System.Threading.Tasks;
using plog;
using Sandbox;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000014 RID: 20
public class SteamController : MonoBehaviour
{
	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004C9B File Offset: 0x00002E9B
	public static int FishSizeMulti
	{
		get
		{
			if (!SteamClient.IsValid)
			{
				return 1;
			}
			if (!SteamController.BuiltInVerifiedSteamIds.Contains(SteamClient.SteamId))
			{
				return 1;
			}
			return 2;
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00004CC0 File Offset: 0x00002EC0
	private void Awake()
	{
		if (SteamController.Instance)
		{
			Object.Destroy(this);
			return;
		}
		SteamController.Instance = this;
		base.transform.SetParent(null);
		Object.DontDestroyOnLoad(base.gameObject);
		try
		{
			SteamClient.Init(this.appId, true);
			SteamController.Log.Info("Steam initialized!", null, null, null);
		}
		catch (Exception)
		{
			SteamController.Log.Info("Couldn't initialize steam", null, null, null);
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00004D44 File Offset: 0x00002F44
	public static async void FetchAvatar(RawImage target, Friend user)
	{
		global::Steamworks.Data.Image? image = await user.GetMediumAvatarAsync();
		if (image != null)
		{
			Texture2D texture2D = new Texture2D((int)image.Value.Width, (int)image.Value.Height, TextureFormat.RGBA32, false);
			texture2D.LoadRawTextureData(image.Value.Data);
			texture2D.Apply();
			target.texture = texture2D;
		}
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00004D83 File Offset: 0x00002F83
	public void UpdateTimeInSandbox(float deltaTime)
	{
		if (!SteamClient.IsValid)
		{
			return;
		}
		deltaTime /= 3600f;
		SteamUserStats.AddStat("sandbox_total_time", deltaTime);
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00004DA2 File Offset: 0x00002FA2
	public void AddToStatInt(string statKey, int amount)
	{
		if (!SteamClient.IsValid)
		{
			return;
		}
		SteamUserStats.AddStat(statKey, amount);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004DB4 File Offset: 0x00002FB4
	public SandboxStats GetSandboxStats()
	{
		if (!SteamClient.IsValid)
		{
			return new SandboxStats();
		}
		return new SandboxStats
		{
			brushesBuilt = SteamUserStats.GetStatInt("sandbox_built_brushes"),
			propsSpawned = SteamUserStats.GetStatInt("sandbox_spawned_props"),
			enemiesSpawned = SteamUserStats.GetStatInt("sandbox_spawned_enemies"),
			hoursSpend = SteamUserStats.GetStatFloat("sandbox_total_time")
		};
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00004E13 File Offset: 0x00003013
	public void UpdateWave(int wave)
	{
		if (!SteamClient.IsValid)
		{
			return;
		}
		SteamFriends.SetRichPresence("wave", wave.ToString());
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00004E30 File Offset: 0x00003030
	public static async Task<Leaderboard?> FetchSteamLeaderboard(string key, bool createIfNotFound = false, LeaderboardSort sort = LeaderboardSort.Descending, LeaderboardDisplay display = LeaderboardDisplay.TimeMilliSeconds)
	{
		Leaderboard? leaderboard;
		if (createIfNotFound)
		{
			leaderboard = await SteamUserStats.FindOrCreateLeaderboardAsync(key, sort, display);
		}
		else
		{
			leaderboard = await SteamUserStats.FindLeaderboardAsync(key);
		}
		return leaderboard;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00004E8C File Offset: 0x0000308C
	public void FetchSceneActivity(string scene)
	{
		if (!SteamClient.IsValid)
		{
			return;
		}
		if (SceneHelper.IsPlayingCustom)
		{
			SteamFriends.SetRichPresence("steam_display", "#AtCustomLevel");
			return;
		}
		StockMapInfo instance = StockMapInfo.Instance;
		if (scene == "Main Menu")
		{
			SteamFriends.SetRichPresence("steam_display", "#AtMainMenu");
			return;
		}
		if (scene == "Endless")
		{
			SteamFriends.SetRichPresence("steam_display", "#AtCyberGrind");
			SteamFriends.SetRichPresence("difficulty", MonoSingleton<PresenceController>.Instance.diffNames[MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0)]);
			SteamFriends.SetRichPresence("wave", "0");
			return;
		}
		if (instance != null && !string.IsNullOrEmpty(instance.assets.Deserialize().LargeText))
		{
			SteamFriends.SetRichPresence("steam_display", "#AtStandardLevel");
			SteamFriends.SetRichPresence("difficulty", MonoSingleton<PresenceController>.Instance.diffNames[MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0)]);
			SteamFriends.SetRichPresence("level", instance.assets.Deserialize().LargeText);
			return;
		}
		SteamFriends.SetRichPresence("steam_display", "#UnknownLevel");
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00004FB3 File Offset: 0x000031B3
	private void OnApplicationQuit()
	{
		if (SteamClient.IsValid)
		{
			SteamClient.Shutdown();
		}
	}

	// Token: 0x04000051 RID: 81
	private static readonly global::plog.Logger Log = new global::plog.Logger("Steam");

	// Token: 0x04000052 RID: 82
	public static SteamController Instance;

	// Token: 0x04000053 RID: 83
	private Leaderboard? fishBoard;

	// Token: 0x04000054 RID: 84
	[SerializeField]
	private uint appId;

	// Token: 0x04000055 RID: 85
	private static string fishLeaderboard = "Fish Size";

	// Token: 0x04000056 RID: 86
	public static readonly ulong[] BuiltInVerifiedSteamIds = new ulong[] { 76561198135929436UL, 76561197998177443UL };
}
