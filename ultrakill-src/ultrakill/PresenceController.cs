using System;
using plog;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000013 RID: 19
[ConfigureSingleton(SingletonFlags.NoAutoInstance | SingletonFlags.DestroyDuplicates)]
public class PresenceController : MonoSingleton<PresenceController>
{
	// Token: 0x060000B1 RID: 177 RVA: 0x00004B18 File Offset: 0x00002D18
	private void Start()
	{
		base.transform.SetParent(null);
		Object.DontDestroyOnLoad(base.gameObject);
		SceneManager.sceneLoaded += this.SceneManagerOnsceneLoaded;
		this.SceneManagerOnsceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
		if (SteamClient.IsValid)
		{
			SteamUGC.StopPlaytimeTrackingForAllItems();
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004B66 File Offset: 0x00002D66
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.SceneManagerOnsceneLoaded;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00004B79 File Offset: 0x00002D79
	public static void UpdateCyberGrindWave(int wave)
	{
		DiscordController.UpdateWave(wave);
		SteamController.Instance.UpdateWave(wave);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00004B8C File Offset: 0x00002D8C
	private void SceneManagerOnsceneLoaded(Scene _, LoadSceneMode mode)
	{
		if (mode == LoadSceneMode.Additive)
		{
			return;
		}
		string currentScene = SceneHelper.CurrentScene;
		PresenceController.Log.Info("Scene loaded: " + currentScene, null, null, null);
		DiscordController.Instance.FetchSceneActivity(currentScene);
		SteamController.Instance.FetchSceneActivity(currentScene);
		if (MapInfoBase.Instance != null && MapInfoBase.Instance.sandboxTools)
		{
			if (this.trackingTimeInSandbox)
			{
				return;
			}
			PresenceController.Log.Info("Starting sandbox time tracking", null, null, null);
			this.trackingTimeInSandbox = true;
			this.timeInSandbox = 0f;
			return;
		}
		else
		{
			if (!this.trackingTimeInSandbox)
			{
				return;
			}
			PresenceController.Log.Info("Submitting sandbox time", null, null, null);
			this.trackingTimeInSandbox = false;
			SteamController.Instance.UpdateTimeInSandbox(this.timeInSandbox);
			return;
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00004C55 File Offset: 0x00002E55
	public void AddToStatInt(string statKey, int amount)
	{
		SteamController.Instance.AddToStatInt(statKey, amount);
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00004C63 File Offset: 0x00002E63
	private void OnApplicationQuit()
	{
		if (this.trackingTimeInSandbox)
		{
			SteamController.Instance.UpdateTimeInSandbox(this.timeInSandbox);
		}
	}

	// Token: 0x0400004D RID: 77
	private static readonly global::plog.Logger Log = new global::plog.Logger("Presence");

	// Token: 0x0400004E RID: 78
	public string[] diffNames;

	// Token: 0x0400004F RID: 79
	private bool trackingTimeInSandbox;

	// Token: 0x04000050 RID: 80
	private TimeSince timeInSandbox;
}
