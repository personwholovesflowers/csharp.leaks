using System;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class LevelStatsEnabler : MonoBehaviour
{
	// Token: 0x06000F58 RID: 3928 RVA: 0x00071758 File Offset: 0x0006F958
	private void Start()
	{
		if (!this.canAlwaysEnable)
		{
			if (this.secretLevel < 0)
			{
				StatsManager instance = MonoSingleton<StatsManager>.Instance;
				RankData rankData = null;
				if (instance.levelNumber != 0 && !Debug.isDebugBuild)
				{
					rankData = GameProgressSaver.GetRank(true, -1);
				}
				if ((instance.levelNumber == 0 || ((rankData == null || rankData.levelNumber != instance.levelNumber) && !Debug.isDebugBuild)) && !SceneHelper.IsPlayingCustom)
				{
					PlayerPrefs.SetInt("LevStaOpe", 0);
					base.gameObject.SetActive(false);
				}
				else if (PlayerPrefs.GetInt("LevStaTut", 0) == 0)
				{
					base.Invoke("LevelStatsTutorial", 1.5f);
				}
			}
			else if (GameProgressSaver.GetSecretMission(this.secretLevel) < 2)
			{
				PlayerPrefs.SetInt("LevStaOpe", 0);
				base.gameObject.SetActive(false);
			}
		}
		this.levelStats = base.transform.GetChild(0).gameObject;
		if (PlayerPrefs.GetInt("LevStaOpe", 0) == 0)
		{
			this.levelStats.SetActive(false);
			return;
		}
		this.keepOpen = true;
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00071854 File Offset: 0x0006FA54
	private void Update()
	{
		if (!this.keepOpen)
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.Stats.WasPerformedThisFrame)
			{
				if (!this.keepOpen)
				{
					if (this.doubleTap > 0f)
					{
						PlayerPrefs.SetInt("LevStaOpe", 1);
						this.keepOpen = true;
					}
					else
					{
						this.doubleTap = 0.5f;
					}
				}
				this.levelStats.SetActive(true);
			}
			else if (MonoSingleton<InputManager>.Instance.InputSource.Stats.WasCanceledThisFrame)
			{
				this.levelStats.SetActive(false);
			}
		}
		else if (MonoSingleton<InputManager>.Instance.InputSource.Stats.WasPerformedThisFrame)
		{
			this.keepOpen = false;
			PlayerPrefs.SetInt("LevStaOpe", 0);
			this.levelStats.SetActive(false);
		}
		if (this.doubleTap > 0f)
		{
			this.doubleTap = Mathf.MoveTowards(this.doubleTap, 0f, Time.deltaTime);
		}
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00071941 File Offset: 0x0006FB41
	private void LevelStatsTutorial()
	{
		PlayerPrefs.SetInt("LevStaTut", 1);
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Hold <color=orange>TAB</color> to see current stats when <color=orange>REPLAYING</color> a level.\n<color=orange>DOUBLE TAP</color> to keep open.", "", "", 0, false, false, true);
	}

	// Token: 0x0400149E RID: 5278
	private GameObject levelStats;

	// Token: 0x0400149F RID: 5279
	private bool keepOpen;

	// Token: 0x040014A0 RID: 5280
	private float doubleTap;

	// Token: 0x040014A1 RID: 5281
	public int secretLevel = -1;

	// Token: 0x040014A2 RID: 5282
	public bool canAlwaysEnable;
}
