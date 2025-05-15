using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002C7 RID: 711
public class LevelStats : MonoBehaviour
{
	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000F53 RID: 3923 RVA: 0x000712CC File Offset: 0x0006F4CC
	private StatsManager sman
	{
		get
		{
			return MonoSingleton<StatsManager>.Instance;
		}
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000712D4 File Offset: 0x0006F4D4
	private void Start()
	{
		if (this.secretLevel || this.cyberGrind)
		{
			this.levelName.text = (this.cyberGrind ? "THE CYBER GRIND" : "SECRET MISSION");
			this.ready = true;
			this.CheckStats();
			return;
		}
		if (SceneHelper.IsPlayingCustom)
		{
			MapInfo instance = MapInfo.Instance;
			this.levelName.text = ((instance != null) ? instance.levelName : "???");
			this.ready = true;
			this.CheckStats();
		}
		RankData rankData = null;
		if (this.sman.levelNumber != 0 && !Debug.isDebugBuild)
		{
			rankData = GameProgressSaver.GetRank(true, -1);
		}
		if (this.sman.levelNumber != 0 && (Debug.isDebugBuild || (rankData != null && rankData.levelNumber == this.sman.levelNumber)))
		{
			StockMapInfo instance2 = StockMapInfo.Instance;
			if (instance2 != null)
			{
				this.levelName.text = instance2.assets.LargeText;
			}
			else
			{
				this.levelName.text = "???";
			}
			this.ready = true;
			this.CheckStats();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		if (this.sman.secretObjects.Length < this.secrets.Length)
		{
			for (int i = this.secrets.Length - 1; i >= this.sman.secretObjects.Length; i--)
			{
				this.secrets[i].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x0007143E File Offset: 0x0006F63E
	private void Update()
	{
		if (this.ready)
		{
			this.CheckStats();
		}
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00071450 File Offset: 0x0006F650
	private void CheckStats()
	{
		if (this.time)
		{
			this.seconds = this.sman.seconds;
			this.minutes = 0f;
			while (this.seconds >= 60f)
			{
				this.seconds -= 60f;
				this.minutes += 1f;
			}
			this.time.text = this.minutes.ToString() + ":" + this.seconds.ToString("00.000");
		}
		if (this.timeRank)
		{
			this.timeRank.text = this.sman.GetRanks(this.sman.timeRanks, this.sman.seconds, true, false);
		}
		if (this.cyberGrind)
		{
			if (this.wave)
			{
				this.wave.text = MonoSingleton<EndlessGrid>.Instance.waveNumberText.text;
			}
			if (this.enemiesLeft)
			{
				this.enemiesLeft.text = MonoSingleton<EndlessGrid>.Instance.enemiesLeftText.text;
			}
			return;
		}
		if (this.kills)
		{
			this.kills.text = this.sman.kills.ToString();
		}
		if (this.killsRank)
		{
			this.killsRank.text = this.sman.GetRanks(this.sman.killRanks, (float)this.sman.kills, false, false);
		}
		if (this.style)
		{
			this.style.text = this.sman.stylePoints.ToString();
		}
		if (this.styleRank)
		{
			this.styleRank.text = this.sman.GetRanks(this.sman.styleRanks, (float)this.sman.stylePoints, false, false);
		}
		if (this.checkSecrets && this.secrets != null && this.secrets.Length != 0)
		{
			bool flag = true;
			int num = 0;
			for (int i = this.sman.secretObjects.Length - 1; i >= 0; i--)
			{
				if (this.sman.prevSecrets.Contains(num) || this.sman.newSecrets.Contains(num))
				{
					this.secrets[i].sprite = this.filledSecret;
				}
				else
				{
					flag = false;
				}
				num++;
			}
			if (flag)
			{
				this.checkSecrets = false;
			}
		}
		if (this.challenge)
		{
			if (MonoSingleton<ChallengeManager>.Instance.challengeDone && !MonoSingleton<ChallengeManager>.Instance.challengeFailed)
			{
				this.challenge.text = "<color=#FFAF00>YES</color>";
			}
			else
			{
				this.challenge.text = "NO";
			}
		}
		if (this.majorAssists)
		{
			if (this.sman.majorUsed)
			{
				this.majorAssists.text = "<color=#4C99E6>YES</color>";
				return;
			}
			this.majorAssists.text = "NO";
		}
	}

	// Token: 0x0400148B RID: 5259
	public bool cyberGrind;

	// Token: 0x0400148C RID: 5260
	public bool secretLevel;

	// Token: 0x0400148D RID: 5261
	public TMP_Text levelName;

	// Token: 0x0400148E RID: 5262
	private bool ready;

	// Token: 0x0400148F RID: 5263
	public TMP_Text time;

	// Token: 0x04001490 RID: 5264
	public TMP_Text timeRank;

	// Token: 0x04001491 RID: 5265
	private float seconds;

	// Token: 0x04001492 RID: 5266
	private float minutes;

	// Token: 0x04001493 RID: 5267
	public TMP_Text kills;

	// Token: 0x04001494 RID: 5268
	public TMP_Text killsRank;

	// Token: 0x04001495 RID: 5269
	public TMP_Text style;

	// Token: 0x04001496 RID: 5270
	public TMP_Text styleRank;

	// Token: 0x04001497 RID: 5271
	public Image[] secrets;

	// Token: 0x04001498 RID: 5272
	private bool checkSecrets = true;

	// Token: 0x04001499 RID: 5273
	public Sprite filledSecret;

	// Token: 0x0400149A RID: 5274
	public TMP_Text challenge;

	// Token: 0x0400149B RID: 5275
	public TMP_Text majorAssists;

	// Token: 0x0400149C RID: 5276
	[Header("Cyber Grind")]
	public TMP_Text wave;

	// Token: 0x0400149D RID: 5277
	public TMP_Text enemiesLeft;
}
