using System;
using System.Collections;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001C9 RID: 457
public class FinalCyberRank : MonoBehaviour
{
	// Token: 0x06000951 RID: 2385 RVA: 0x0003EDE4 File Offset: 0x0003CFE4
	private void Start()
	{
		this.sman = MonoSingleton<StatsManager>.Instance;
		GameObject[] array = this.toAppear;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		MonoSingleton<NewMovement>.Instance.endlessMode = true;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0003EE28 File Offset: 0x0003D028
	public void GameOver()
	{
		if (!this.gameOver)
		{
			if (this.sman == null)
			{
				this.sman = MonoSingleton<StatsManager>.Instance;
			}
			int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
			this.gameOver = true;
			this.sman.StopTimer();
			this.sman.HideShit();
			MonoSingleton<TimeController>.Instance.controlTimeScale = false;
			this.savedTime = this.sman.seconds;
			this.savedKills = this.sman.kills;
			this.savedStyle = this.sman.stylePoints;
			if (this.savedStyle < 0)
			{
				this.savedStyle = 0;
			}
			ActivateNextWave activateNextWave = Object.FindObjectOfType<ActivateNextWave>();
			this.savedWaves = (float)MonoSingleton<EndlessGrid>.Instance.currentWave + (float)activateNextWave.deadEnemies / (float)MonoSingleton<EndlessGrid>.Instance.enemyAmount;
			this.previousBest = GameProgressSaver.GetBestCyber();
			this.bestWaveText.text = Mathf.FloorToInt(this.previousBest.preciseWavesByDifficulty[@int]).ToString() + string.Format("\n<color=#616161><size=20>{0}%</size></color>", this.CalculatePerc(this.previousBest.preciseWavesByDifficulty[@int]));
			this.bestKillsText.text = this.previousBest.kills[@int].ToString() ?? "";
			this.bestStyleText.text = this.previousBest.style[@int].ToString() ?? "";
			int num = 0;
			float num2;
			for (num2 = this.previousBest.time[@int]; num2 >= 60f; num2 -= 60f)
			{
				num++;
			}
			this.bestTimeText.text = num.ToString() + ":" + num2.ToString("00.000");
			if (this.sman.majorUsed || MonoSingleton<AssistController>.Instance.cheatsEnabled || MonoSingleton<EndlessGrid>.Instance.customPatternMode)
			{
				return;
			}
			if (SteamController.Instance && GameStateManager.CanSubmitScores)
			{
				MonoSingleton<LeaderboardController>.Instance.SubmitCyberGrindScore(@int, this.savedWaves, this.savedKills, this.savedStyle, this.sman.seconds);
			}
			if (this.savedWaves > this.previousBest.preciseWavesByDifficulty[@int])
			{
				this.NewBest();
				return;
			}
			if (this.savedWaves < this.previousBest.preciseWavesByDifficulty[@int])
			{
				return;
			}
			if (this.savedKills > this.previousBest.kills[@int])
			{
				this.NewBest();
				return;
			}
			if (this.savedKills < this.previousBest.kills[@int])
			{
				return;
			}
			if (this.savedStyle > this.previousBest.style[@int])
			{
				this.NewBest();
				return;
			}
			if (this.savedStyle < this.previousBest.style[@int])
			{
				return;
			}
			if (this.savedTime > this.previousBest.time[@int])
			{
				this.NewBest();
				return;
			}
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003F10D File Offset: 0x0003D30D
	private void NewBest()
	{
		GameProgressSaver.SetBestCyber(this);
		this.newBest = true;
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003F11C File Offset: 0x0003D31C
	private void Update()
	{
		if (this.gameOver)
		{
			if (this.timeController == null)
			{
				this.timeController = MonoSingleton<TimeController>.Instance;
			}
			if (this.timeController.timeScale > 0f)
			{
				this.timeController.timeScale = Mathf.MoveTowards(this.timeController.timeScale, 0f, Time.unscaledDeltaTime * (this.timeController.timeScale + 0.01f));
				Time.timeScale = this.timeController.timeScale * this.timeController.timeScaleModifier;
				MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", this.timeController.timeScale);
				if (this.timeController.timeScale < 0.1f)
				{
					MonoSingleton<AudioMixerController>.Instance.forceOff = true;
					MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allVolume", MonoSingleton<AudioMixerController>.Instance.CalculateVolume(this.timeController.timeScale * 10f * MonoSingleton<AudioMixerController>.Instance.sfxVolume));
					MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("allVolume", MonoSingleton<AudioMixerController>.Instance.CalculateVolume(this.timeController.timeScale * 10f * MonoSingleton<AudioMixerController>.Instance.musicVolume));
				}
				MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("allPitch", this.timeController.timeScale);
				MonoSingleton<MusicManager>.Instance.volume = 0.5f + this.timeController.timeScale / 2f;
				if (this.timeController.timeScale <= 0f)
				{
					this.Appear();
					MonoSingleton<MusicManager>.Instance.forcedOff = true;
					MonoSingleton<MusicManager>.Instance.StopMusic();
				}
			}
		}
		if (this.countTime)
		{
			if (this.savedTime >= this.checkedSeconds)
			{
				if (this.savedTime > this.checkedSeconds)
				{
					float num = this.savedTime - this.checkedSeconds;
					this.checkedSeconds += Time.unscaledDeltaTime * 20f + Time.unscaledDeltaTime * num * 1.5f;
					this.seconds += Time.unscaledDeltaTime * 20f + Time.unscaledDeltaTime * num * 1.5f;
				}
				if (this.checkedSeconds >= this.savedTime || this.skipping)
				{
					this.checkedSeconds = this.savedTime;
					this.seconds = this.savedTime;
					this.minutes = 0f;
					while (this.seconds >= 60f)
					{
						this.seconds -= 60f;
						this.minutes += 1f;
					}
					this.countTime = false;
					this.timeText.GetComponent<AudioSource>().Stop();
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
				}
				if (this.seconds >= 60f)
				{
					this.seconds -= 60f;
					this.minutes += 1f;
				}
				this.timeText.text = this.minutes.ToString() + ":" + this.seconds.ToString("00.000");
			}
		}
		else if (this.countWaves)
		{
			if (this.savedWaves >= this.checkedWaves)
			{
				if (this.savedWaves > this.checkedWaves)
				{
					this.checkedWaves += Time.unscaledDeltaTime * 20f + Time.unscaledDeltaTime * (this.savedWaves - this.checkedWaves) * 1.5f;
				}
				if (this.checkedWaves >= this.savedWaves || this.skipping)
				{
					this.checkedWaves = this.savedWaves;
					this.countWaves = false;
					this.waveText.GetComponent<AudioSource>().Stop();
					this.totalPoints += Mathf.FloorToInt(this.savedWaves) * 100;
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
				}
				else
				{
					int i = this.totalPoints + Mathf.RoundToInt(this.checkedWaves) * 100;
					int num2 = 0;
					while (i >= 1000)
					{
						num2++;
						i -= 1000;
					}
					if (num2 > 0)
					{
						if (i < 10)
						{
							this.pointsText.text = string.Concat(new string[]
							{
								"+",
								num2.ToString(),
								",00",
								i.ToString(),
								"<color=orange>P</color>"
							});
						}
						else if (i < 100)
						{
							this.pointsText.text = string.Concat(new string[]
							{
								"+",
								num2.ToString(),
								",0",
								i.ToString(),
								"<color=orange>P</color>"
							});
						}
						else
						{
							this.pointsText.text = string.Concat(new string[]
							{
								"+",
								num2.ToString(),
								",",
								i.ToString(),
								"<color=orange>P</color>"
							});
						}
					}
					else
					{
						this.pointsText.text = "+" + i.ToString() + "<color=orange>P</color>";
					}
				}
				this.waveText.text = Mathf.FloorToInt(this.checkedWaves).ToString() + string.Format("\n<color=#616161><size=20>{0}%</size></color>", this.CalculatePerc(this.savedWaves));
			}
		}
		else if (this.countKills)
		{
			if ((float)this.savedKills >= this.checkedKills)
			{
				if ((float)this.savedKills > this.checkedKills)
				{
					this.checkedKills += Time.unscaledDeltaTime * 20f + Time.unscaledDeltaTime * ((float)this.savedKills - this.checkedKills) * 1.5f;
				}
				if (this.checkedKills >= (float)this.savedKills || this.skipping)
				{
					this.checkedKills = (float)this.savedKills;
					this.countKills = false;
					this.killsText.GetComponent<AudioSource>().Stop();
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
				}
				this.killsText.text = this.checkedKills.ToString("0");
			}
		}
		else if (this.countStyle && (float)this.savedStyle >= this.checkedStyle)
		{
			float num3 = this.checkedStyle;
			if ((float)this.savedStyle > this.checkedStyle)
			{
				this.checkedStyle += Time.unscaledDeltaTime * 2500f + Time.unscaledDeltaTime * ((float)this.savedStyle - this.checkedStyle) * 1.5f;
			}
			if (this.checkedStyle >= (float)this.savedStyle || this.skipping)
			{
				this.checkedStyle = (float)this.savedStyle;
				this.countStyle = false;
				this.styleText.GetComponent<AudioSource>().Stop();
				base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
				this.totalPoints += this.savedStyle;
				this.PointsShow();
			}
			else
			{
				int j = this.totalPoints + Mathf.RoundToInt(this.checkedStyle);
				int num4 = 0;
				while (j >= 1000)
				{
					num4++;
					j -= 1000;
				}
				if (num4 > 0)
				{
					if (j < 10)
					{
						this.pointsText.text = string.Concat(new string[]
						{
							"+",
							num4.ToString(),
							",00",
							j.ToString(),
							"<color=orange>P</color>"
						});
					}
					else if (j < 100)
					{
						this.pointsText.text = string.Concat(new string[]
						{
							"+",
							num4.ToString(),
							",0",
							j.ToString(),
							"<color=orange>P</color>"
						});
					}
					else
					{
						this.pointsText.text = string.Concat(new string[]
						{
							"+",
							num4.ToString(),
							",",
							j.ToString(),
							"<color=orange>P</color>"
						});
					}
				}
				else
				{
					this.pointsText.text = "+" + j.ToString() + "<color=orange>P</color>";
				}
			}
			this.styleText.text = this.checkedStyle.ToString("0");
		}
		if (this.flashFade)
		{
			this.flashColor.a = Mathf.MoveTowards(this.flashColor.a, 0f, Time.unscaledDeltaTime * 0.5f);
			this.flashPanel.color = this.flashColor;
			if (this.flashColor.a <= 0f)
			{
				this.flashFade = false;
			}
		}
		if (this.gameOver)
		{
			if (this.timeController == null)
			{
				this.timeController = MonoSingleton<TimeController>.Instance;
			}
			if (this.opm == null)
			{
				this.opm = MonoSingleton<OptionsManager>.Instance;
			}
			if (this.opm.paused && !this.wasPaused)
			{
				this.wasPaused = true;
			}
			else if (!this.opm.paused && this.wasPaused)
			{
				this.wasPaused = false;
				MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", 0f);
				MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allVolume", MonoSingleton<AudioMixerController>.Instance.CalculateVolume(this.timeController.timeScale * 10f * MonoSingleton<AudioMixerController>.Instance.sfxVolume));
				MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("allPitch", 0f);
				MonoSingleton<AudioMixerController>.Instance.musicSound.SetFloat("allVolume", MonoSingleton<AudioMixerController>.Instance.CalculateVolume(this.timeController.timeScale * 10f * MonoSingleton<AudioMixerController>.Instance.musicVolume));
			}
			if (!GameStateManager.ShowLeaderboards || MonoSingleton<EndlessGrid>.Instance.customPatternMode)
			{
				this.highScoresDisplayed = true;
			}
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame) && this.complete && !this.opm.paused)
			{
				if (this.highScoresDisplayed)
				{
					SceneHelper.RestartScene();
					return;
				}
				this.highScoresDisplayed = true;
				GameObject[] array = this.previousElements;
				for (int k = 0; k < array.Length; k++)
				{
					array[k].SetActive(false);
				}
				this.highScoreElement.SetActive(true);
				this.FetchTheScores();
				return;
			}
			else if (this.timeController.timeScale <= 0f && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame || MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame) && !this.complete && !this.opm.paused)
			{
				this.skipping = true;
				this.timeBetween = 0.01f;
			}
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0003FC79 File Offset: 0x0003DE79
	private int CalculatePerc(float value)
	{
		return Mathf.FloorToInt((value - (float)Mathf.FloorToInt(value)) * 100f);
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003FC90 File Offset: 0x0003DE90
	private async void FetchTheScores()
	{
		int difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		LeaderboardEntry[] array = await MonoSingleton<LeaderboardController>.Instance.GetCyberGrindScores(difficulty, LeaderboardType.Friends);
		if (this.template)
		{
			int num = 1;
			foreach (LeaderboardEntry leaderboardEntry in array)
			{
				TMP_Text tmp_Text = this.tUsername;
				Friend friend = leaderboardEntry.User;
				tmp_Text.text = FinalCyberRank.TruncateUsername(friend.Name, 18);
				this.tScore.text = Mathf.FloorToInt((float)leaderboardEntry.Score / 1000f).ToString();
				this.tPercent.text = string.Format("<color=#616161>{0}%</color>", this.CalculatePerc((float)leaderboardEntry.Score / 1000f));
				this.tRank.text = num.ToString();
				GameObject gameObject = Object.Instantiate<GameObject>(this.template, this.friendContainer.transform);
				SteamController.FetchAvatar(gameObject.GetComponentInChildren<RawImage>(), leaderboardEntry.User);
				gameObject.SetActive(true);
				num++;
			}
			this.friendPlaceholder.SetActive(false);
			this.friendContainer.SetActive(true);
			array = await MonoSingleton<LeaderboardController>.Instance.GetCyberGrindScores(difficulty, LeaderboardType.GlobalAround);
			if (this.template)
			{
				foreach (LeaderboardEntry leaderboardEntry2 in array)
				{
					TMP_Text tmp_Text2 = this.tUsername;
					Friend friend = leaderboardEntry2.User;
					tmp_Text2.text = FinalCyberRank.TruncateUsername(friend.Name, 18);
					this.tScore.text = Mathf.FloorToInt((float)leaderboardEntry2.Score / 1000f).ToString();
					this.tPercent.text = string.Format("<color=#616161>{0}%</color>", this.CalculatePerc((float)leaderboardEntry2.Score / 1000f));
					this.tRank.text = leaderboardEntry2.GlobalRank.ToString();
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.template, this.globalContainer.transform);
					SteamController.FetchAvatar(gameObject2.GetComponentInChildren<RawImage>(), leaderboardEntry2.User);
					gameObject2.SetActive(true);
				}
				this.globalPlaceholder.SetActive(false);
				this.globalContainer.SetActive(true);
			}
		}
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0003FCC7 File Offset: 0x0003DEC7
	private static string TruncateUsername(string value, int maxChars)
	{
		if (value.Length > maxChars)
		{
			return value.Substring(0, maxChars);
		}
		return value;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003FCDC File Offset: 0x0003DEDC
	public void Appear()
	{
		if (this.i < this.toAppear.Length)
		{
			if (this.skipping)
			{
				HudOpenEffect component = this.toAppear[this.i].GetComponent<HudOpenEffect>();
				if (component != null)
				{
					component.skip = true;
				}
			}
			if (this.toAppear[this.i] == this.timeText.gameObject)
			{
				if (this.skipping)
				{
					this.checkedSeconds = this.savedTime;
					this.seconds = this.savedTime;
					this.minutes = 0f;
					while (this.seconds >= 60f)
					{
						this.seconds -= 60f;
						this.minutes += 1f;
					}
					this.timeText.GetComponent<AudioSource>().playOnAwake = false;
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
					this.timeText.text = this.minutes.ToString() + ":" + this.seconds.ToString("00.000");
				}
				else
				{
					this.countTime = true;
				}
			}
			else if (this.toAppear[this.i] == this.killsText.gameObject)
			{
				if (this.skipping)
				{
					this.checkedKills = (float)this.savedKills;
					this.killsText.GetComponent<AudioSource>().playOnAwake = false;
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
					this.killsText.text = this.checkedKills.ToString("0");
				}
				else
				{
					this.countKills = true;
				}
			}
			else if (this.toAppear[this.i] == this.waveText.gameObject)
			{
				if (this.skipping)
				{
					this.checkedWaves = this.savedWaves;
					this.waveText.GetComponent<AudioSource>().playOnAwake = false;
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
					this.waveText.text = Mathf.FloorToInt(this.savedWaves).ToString() + string.Format("\n<color=#616161><size=20>{0}%</size></color>", this.CalculatePerc(this.savedWaves));
				}
				else
				{
					this.countWaves = true;
				}
			}
			else if (this.toAppear[this.i] == this.styleText.gameObject)
			{
				if (this.skipping)
				{
					this.checkedStyle = (float)this.savedStyle;
					this.styleText.text = this.checkedStyle.ToString("0");
					this.styleText.GetComponent<AudioSource>().playOnAwake = false;
					base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween * 2f));
					this.totalPoints += this.savedStyle;
					this.PointsShow();
				}
				else
				{
					this.countStyle = true;
				}
			}
			else
			{
				base.StartCoroutine(this.InvokeRealtimeCoroutine(new UnityAction(this.Appear), this.timeBetween));
			}
			this.toAppear[this.i].gameObject.SetActive(true);
			this.i++;
			return;
		}
		if (this.newBest)
		{
			GameObject gameObject = this.bestWaveText.transform.parent.parent.parent.GetChild(1).gameObject;
			this.FlashPanel(gameObject);
			gameObject.GetComponent<AudioSource>().Play();
			this.bestWaveText.text = this.waveText.text;
			this.bestKillsText.text = this.killsText.text;
			this.bestStyleText.text = this.styleText.text;
			this.bestTimeText.text = this.timeText.text;
		}
		if (!this.complete)
		{
			this.complete = true;
			GameProgressSaver.AddMoney(this.totalPoints);
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0004011C File Offset: 0x0003E31C
	public void FlashPanel(GameObject panel)
	{
		if (this.flashFade)
		{
			this.flashColor.a = 0f;
			this.flashPanel.color = this.flashColor;
		}
		this.flashPanel = panel.GetComponent<global::UnityEngine.UI.Image>();
		this.flashColor = this.flashPanel.color;
		this.flashColor.a = 1f;
		this.flashPanel.color = this.flashColor;
		this.flashFade = true;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x00040198 File Offset: 0x0003E398
	private void PointsShow()
	{
		int i = this.totalPoints;
		int num = 0;
		while (i >= 1000)
		{
			num++;
			i -= 1000;
		}
		if (num <= 0)
		{
			this.pointsText.text = "+" + i.ToString() + "<color=orange>P</color>";
			return;
		}
		if (i < 10)
		{
			this.pointsText.text = string.Concat(new string[]
			{
				"+",
				num.ToString(),
				",00",
				i.ToString(),
				"<color=orange>P</color>"
			});
			return;
		}
		if (i < 100)
		{
			this.pointsText.text = string.Concat(new string[]
			{
				"+",
				num.ToString(),
				",0",
				i.ToString(),
				"<color=orange>P</color>"
			});
			return;
		}
		this.pointsText.text = string.Concat(new string[]
		{
			"+",
			num.ToString(),
			",",
			i.ToString(),
			"<color=orange>P</color>"
		});
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x000402BF File Offset: 0x0003E4BF
	private IEnumerator InvokeRealtimeCoroutine(UnityAction action, float seconds)
	{
		yield return new WaitForSecondsRealtime(seconds);
		if (action != null)
		{
			action();
		}
		yield break;
	}

	// Token: 0x04000BD2 RID: 3026
	public TMP_Text waveText;

	// Token: 0x04000BD3 RID: 3027
	public TMP_Text killsText;

	// Token: 0x04000BD4 RID: 3028
	public TMP_Text styleText;

	// Token: 0x04000BD5 RID: 3029
	public TMP_Text timeText;

	// Token: 0x04000BD6 RID: 3030
	public TMP_Text bestWaveText;

	// Token: 0x04000BD7 RID: 3031
	public TMP_Text bestKillsText;

	// Token: 0x04000BD8 RID: 3032
	public TMP_Text bestStyleText;

	// Token: 0x04000BD9 RID: 3033
	public TMP_Text bestTimeText;

	// Token: 0x04000BDA RID: 3034
	public TMP_Text pointsText;

	// Token: 0x04000BDB RID: 3035
	public int totalPoints;

	// Token: 0x04000BDC RID: 3036
	public GameObject[] toAppear;

	// Token: 0x04000BDD RID: 3037
	private bool skipping;

	// Token: 0x04000BDE RID: 3038
	private float timeBetween = 0.25f;

	// Token: 0x04000BDF RID: 3039
	private bool countTime;

	// Token: 0x04000BE0 RID: 3040
	public float savedTime;

	// Token: 0x04000BE1 RID: 3041
	private float checkedSeconds;

	// Token: 0x04000BE2 RID: 3042
	private float seconds;

	// Token: 0x04000BE3 RID: 3043
	private float minutes;

	// Token: 0x04000BE4 RID: 3044
	private bool countWaves;

	// Token: 0x04000BE5 RID: 3045
	public float savedWaves;

	// Token: 0x04000BE6 RID: 3046
	private float checkedWaves;

	// Token: 0x04000BE7 RID: 3047
	private bool countKills;

	// Token: 0x04000BE8 RID: 3048
	public int savedKills;

	// Token: 0x04000BE9 RID: 3049
	private float checkedKills;

	// Token: 0x04000BEA RID: 3050
	private bool countStyle;

	// Token: 0x04000BEB RID: 3051
	public int savedStyle;

	// Token: 0x04000BEC RID: 3052
	private float checkedStyle;

	// Token: 0x04000BED RID: 3053
	private bool flashFade;

	// Token: 0x04000BEE RID: 3054
	private global::UnityEngine.Color flashColor;

	// Token: 0x04000BEF RID: 3055
	private global::UnityEngine.UI.Image flashPanel;

	// Token: 0x04000BF0 RID: 3056
	private int i;

	// Token: 0x04000BF1 RID: 3057
	private bool gameOver;

	// Token: 0x04000BF2 RID: 3058
	private bool complete;

	// Token: 0x04000BF3 RID: 3059
	private CyberRankData previousBest;

	// Token: 0x04000BF4 RID: 3060
	private bool newBest;

	// Token: 0x04000BF5 RID: 3061
	private TimeController timeController;

	// Token: 0x04000BF6 RID: 3062
	private OptionsManager opm;

	// Token: 0x04000BF7 RID: 3063
	private bool wasPaused;

	// Token: 0x04000BF8 RID: 3064
	private StatsManager sman;

	// Token: 0x04000BF9 RID: 3065
	private bool highScoresDisplayed;

	// Token: 0x04000BFA RID: 3066
	[SerializeField]
	private GameObject[] previousElements;

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private GameObject highScoreElement;

	// Token: 0x04000BFC RID: 3068
	[SerializeField]
	private GameObject friendContainer;

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private GameObject globalContainer;

	// Token: 0x04000BFE RID: 3070
	[SerializeField]
	private GameObject friendPlaceholder;

	// Token: 0x04000BFF RID: 3071
	[SerializeField]
	private GameObject globalPlaceholder;

	// Token: 0x04000C00 RID: 3072
	[SerializeField]
	private GameObject template;

	// Token: 0x04000C01 RID: 3073
	[SerializeField]
	private TMP_Text tRank;

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	private TMP_Text tUsername;

	// Token: 0x04000C03 RID: 3075
	[SerializeField]
	private TMP_Text tScore;

	// Token: 0x04000C04 RID: 3076
	[SerializeField]
	private TMP_Text tPercent;
}
