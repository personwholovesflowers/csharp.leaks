using System;
using Discord;
using plog;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class DiscordController : MonoBehaviour
{
	// Token: 0x0600009C RID: 156 RVA: 0x00004450 File Offset: 0x00002650
	private void Awake()
	{
		if (DiscordController.Instance)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		DiscordController.Instance = this;
		base.transform.SetParent(null);
		Object.DontDestroyOnLoad(base.gameObject);
		bool @bool = MonoSingleton<PrefsManager>.Instance.GetBool("discordIntegration", false);
		if (@bool)
		{
			DiscordController.Enable();
		}
		this.disabled = !@bool;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000044B5 File Offset: 0x000026B5
	private void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000044D7 File Offset: 0x000026D7
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000044FC File Offset: 0x000026FC
	private void OnPrefChanged(string id, object value)
	{
		if (id != "discordIntegration" || !(value is bool))
		{
			return;
		}
		bool flag = (bool)value;
		if (flag)
		{
			DiscordController.Enable();
			return;
		}
		DiscordController.Disable();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004538 File Offset: 0x00002738
	private void Update()
	{
		if (this.discord == null || this.disabled)
		{
			return;
		}
		try
		{
			this.discord.RunCallbacks();
		}
		catch (Exception)
		{
			DiscordController.Log.Warning("Discord lost", null, null, null);
			this.disabled = true;
			this.discord.Dispose();
		}
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000459C File Offset: 0x0000279C
	private void OnApplicationQuit()
	{
		if (this.discord == null)
		{
			return;
		}
		if (this.disabled)
		{
			return;
		}
		this.discord.Dispose();
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000045BC File Offset: 0x000027BC
	public static void UpdateRank(int rank)
	{
		if (!DiscordController.Instance)
		{
			return;
		}
		if (DiscordController.Instance.disabled)
		{
			return;
		}
		if (DiscordController.Instance.rankIcons.Length <= rank)
		{
			DiscordController.Log.Error("Discord Controller is missing rank names/icons!", null, null, null);
			return;
		}
		DiscordController.Instance.cachedActivity.Assets.SmallText = DiscordController.Instance.rankIcons[rank].Text;
		DiscordController.Instance.cachedActivity.Assets.SmallImage = DiscordController.Instance.rankIcons[rank].Image;
		DiscordController.Instance.SendActivity();
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00004664 File Offset: 0x00002864
	public static void UpdateStyle(int points)
	{
		if (!DiscordController.Instance)
		{
			return;
		}
		if (DiscordController.Instance.disabled)
		{
			return;
		}
		if (DiscordController.Instance.lastPoints == points)
		{
			return;
		}
		DiscordController.Instance.lastPoints = points;
		DiscordController.Instance.cachedActivity.Details = "STYLE: " + points.ToString();
		DiscordController.Instance.SendActivity();
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000046D0 File Offset: 0x000028D0
	public static void UpdateWave(int wave)
	{
		if (!DiscordController.Instance)
		{
			return;
		}
		if (DiscordController.Instance.disabled)
		{
			return;
		}
		if (DiscordController.Instance.lastPoints == wave)
		{
			return;
		}
		DiscordController.Instance.lastPoints = wave;
		DiscordController.Instance.cachedActivity.Details = "WAVE: " + wave.ToString();
		DiscordController.Instance.SendActivity();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000473C File Offset: 0x0000293C
	public static void Disable()
	{
		if (!DiscordController.Instance || DiscordController.Instance.discord == null || DiscordController.Instance.disabled)
		{
			return;
		}
		DiscordController.Instance.disabled = true;
		DiscordController.Instance.activityManager.ClearActivity(delegate(Result result)
		{
		});
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x000047A8 File Offset: 0x000029A8
	public static void Enable()
	{
		if (!DiscordController.Instance)
		{
			return;
		}
		if (DiscordController.Instance.discord == null)
		{
			try
			{
				DiscordController.Instance.discord = new global::Discord.Discord(DiscordController.Instance.discordClientId, 1UL);
				DiscordController.Instance.activityManager = DiscordController.Instance.discord.GetActivityManager();
				DiscordController.Log.Info("Discord initialized!", null, null, null);
				DiscordController.Instance.disabled = false;
				DiscordController.Instance.ResetActivityCache();
			}
			catch (Exception)
			{
				DiscordController.Log.Info("Couldn't initialize Discord", null, null, null);
				DiscordController.Instance.disabled = true;
			}
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000485C File Offset: 0x00002A5C
	private void ResetActivityCache()
	{
		Activity activity = default(Activity);
		activity.State = "LOADING";
		activity.Assets.LargeImage = "generic";
		activity.Assets.LargeText = "LOADING";
		activity.Instance = true;
		this.cachedActivity = activity;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000048B0 File Offset: 0x00002AB0
	public void FetchSceneActivity(string scene)
	{
		if (!DiscordController.Instance || DiscordController.Instance.disabled || DiscordController.Instance.discord == null)
		{
			return;
		}
		this.ResetActivityCache();
		if (SceneHelper.IsPlayingCustom)
		{
			this.cachedActivity.State = "Playing Custom Level";
			this.cachedActivity.Assets = this.customLevelActivityAssets.Deserialize();
		}
		else
		{
			StockMapInfo instance = StockMapInfo.Instance;
			if (instance)
			{
				this.cachedActivity.Assets = instance.assets.Deserialize();
				if (string.IsNullOrEmpty(this.cachedActivity.Assets.LargeImage))
				{
					this.cachedActivity.Assets.LargeImage = this.missingActivityAssets.Deserialize().LargeImage;
				}
				if (string.IsNullOrEmpty(this.cachedActivity.Assets.LargeText))
				{
					this.cachedActivity.Assets.LargeText = this.missingActivityAssets.Deserialize().LargeText;
				}
			}
			else
			{
				this.cachedActivity.Assets = this.missingActivityAssets.Deserialize();
			}
			if (scene == "Main Menu")
			{
				this.cachedActivity.State = "Main Menu";
			}
			else
			{
				this.cachedActivity.State = "DIFFICULTY: " + MonoSingleton<PresenceController>.Instance.diffNames[MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0)];
			}
		}
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		long num = (long)(DateTime.UtcNow - dateTime).TotalSeconds;
		this.cachedActivity.Timestamps = new ActivityTimestamps
		{
			Start = num
		};
		this.SendActivity();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00004A60 File Offset: 0x00002C60
	private void SendActivity()
	{
		if (this.discord == null || this.activityManager == null || this.disabled)
		{
			return;
		}
		this.activityManager.UpdateActivity(this.cachedActivity, delegate(Result result)
		{
		});
	}

	// Token: 0x0400003B RID: 59
	private static readonly global::plog.Logger Log = new global::plog.Logger("Discord");

	// Token: 0x0400003C RID: 60
	public static DiscordController Instance;

	// Token: 0x0400003D RID: 61
	[SerializeField]
	private long discordClientId;

	// Token: 0x0400003E RID: 62
	[Space]
	[SerializeField]
	private SerializedActivityAssets customLevelActivityAssets;

	// Token: 0x0400003F RID: 63
	[SerializeField]
	private SerializedActivityAssets missingActivityAssets;

	// Token: 0x04000040 RID: 64
	[SerializeField]
	private ActivityRankIcon[] rankIcons;

	// Token: 0x04000041 RID: 65
	private global::Discord.Discord discord;

	// Token: 0x04000042 RID: 66
	private ActivityManager activityManager;

	// Token: 0x04000043 RID: 67
	private int lastPoints;

	// Token: 0x04000044 RID: 68
	private bool disabled;

	// Token: 0x04000045 RID: 69
	private Activity cachedActivity;
}
