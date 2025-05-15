using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020001CF RID: 463
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class FinalRank : MonoSingleton<FinalRank>
{
	// Token: 0x06000976 RID: 2422 RVA: 0x000410EC File Offset: 0x0003F2EC
	private void Start()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		GameObject[] array = this.toAppear;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.startingPos = base.transform.parent.localPosition;
		this.maxPos = new Vector3(this.startingPos.x, this.startingPos.y - 0.15f, this.startingPos.z);
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00041174 File Offset: 0x0003F374
	private void Update()
	{
		if (base.transform.parent.localPosition == this.maxPos)
		{
			this.goalPos = this.startingPos;
		}
		else if (base.transform.parent.localPosition == this.startingPos)
		{
			this.goalPos = this.maxPos;
		}
		base.transform.parent.localPosition = Vector3.MoveTowards(base.transform.parent.localPosition, this.goalPos, Time.deltaTime / 100f);
		if (this.countTime)
		{
			if (this.savedTime >= this.checkedSeconds)
			{
				if (this.savedTime > this.checkedSeconds)
				{
					float num = this.savedTime - this.checkedSeconds;
					this.checkedSeconds += Time.deltaTime * 20f + Time.deltaTime * num * 1.5f;
					this.seconds += Time.deltaTime * 20f + Time.deltaTime * num * 1.5f;
				}
				if (this.checkedSeconds >= this.savedTime || this.skipping)
				{
					this.checkedSeconds = this.savedTime;
					this.seconds = this.savedTime;
					this.minutes = 0;
					while (this.seconds >= 60f)
					{
						this.seconds -= 60f;
						this.minutes++;
					}
					this.countTime = false;
					this.time.GetComponent<AudioSource>().Stop();
					base.Invoke("Appear", this.timeBetween * 2f);
				}
				if (this.seconds >= 60f)
				{
					this.seconds -= 60f;
					this.minutes++;
				}
				this.time.text = this.minutes.ToString() + ":" + this.seconds.ToString("00.000");
			}
		}
		else if (this.countKills)
		{
			if ((float)this.savedKills >= this.checkedKills)
			{
				if ((float)this.savedKills > this.checkedKills)
				{
					this.checkedKills += Time.deltaTime * 45f;
				}
				if (this.checkedKills >= (float)this.savedKills || this.skipping)
				{
					this.checkedKills = (float)this.savedKills;
					this.countKills = false;
					this.kills.GetComponent<AudioSource>().Stop();
					base.Invoke("Appear", this.timeBetween * 2f);
				}
				this.kills.text = this.checkedKills.ToString("0");
			}
		}
		else if (this.countStyle && (float)this.savedStyle >= this.checkedStyle)
		{
			float num2 = this.checkedStyle;
			if ((float)this.savedStyle > this.checkedStyle)
			{
				this.checkedStyle += Time.deltaTime * 4500f;
			}
			if (this.checkedStyle >= (float)this.savedStyle || this.skipping)
			{
				this.checkedStyle = (float)this.savedStyle;
				this.countStyle = false;
				this.style.GetComponent<AudioSource>().Stop();
				base.Invoke("Appear", this.timeBetween * 2f);
				this.totalPoints += this.savedStyle;
				this.PointsShow();
			}
			else
			{
				int i = this.totalPoints + Mathf.RoundToInt(this.checkedStyle);
				int num3 = 0;
				while (i >= 1000)
				{
					num3++;
					i -= 1000;
				}
				if (num3 > 0)
				{
					if (i < 10)
					{
						this.pointsText.text = string.Concat(new string[]
						{
							"+",
							num3.ToString(),
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
							num3.ToString(),
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
							num3.ToString(),
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
			this.style.text = this.checkedStyle.ToString("0");
		}
		if (this.flashFade)
		{
			this.flashColor.a = this.flashColor.a - Time.deltaTime * this.flashMultiplier;
			this.flashPanel.color = this.flashColor;
			if (this.flashColor.a <= 0f)
			{
				this.flashFade = false;
			}
		}
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && this.complete && Time.timeScale != 0f && this.reachedSecondPit)
		{
			this.LevelChange(false);
		}
		else if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && !this.complete && Time.timeScale != 0f)
		{
			this.skipping = true;
			this.timeBetween = 0.01f;
		}
		if (this.rankless && this.asyncLoad != null && this.asyncLoad.progress >= 0.9f)
		{
			this.LevelChange(false);
		}
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00041740 File Offset: 0x0003F940
	public void SetTime(float seconds, string rank)
	{
		this.savedTime = seconds;
		this.timeRank.text = rank;
		SceneManager.GetSceneByName(this.targetLevelName);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00041761 File Offset: 0x0003F961
	public void SetKills(int killAmount, string rank)
	{
		this.savedKills = killAmount;
		this.killsRank.text = rank;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00041776 File Offset: 0x0003F976
	public void SetStyle(int styleAmount, string rank)
	{
		this.savedStyle = styleAmount;
		this.styleRank.text = rank;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0004178C File Offset: 0x0003F98C
	public void SetInfo(int restarts, bool damage, bool majorUsed, bool cheatsUsed)
	{
		this.extraInfo.text = "";
		int num = 1;
		if (!damage)
		{
			num++;
		}
		if (majorUsed)
		{
			num++;
		}
		if (cheatsUsed)
		{
			num++;
		}
		if (cheatsUsed)
		{
			TMP_Text tmp_Text = this.extraInfo;
			tmp_Text.text += "- <color=#44FF45>CHEATS USED</color>\n";
		}
		if (majorUsed)
		{
			if (!MonoSingleton<AssistController>.Instance.hidePopup)
			{
				TMP_Text tmp_Text2 = this.extraInfo;
				tmp_Text2.text += "- <color=#4C99E6>MAJOR ASSISTS USED</color>\n";
			}
			this.majorAssists = true;
		}
		if (restarts == 0)
		{
			if (num >= 3)
			{
				TMP_Text tmp_Text3 = this.extraInfo;
				tmp_Text3.text += "+ NO RESTARTS\n";
			}
			else
			{
				TMP_Text tmp_Text4 = this.extraInfo;
				tmp_Text4.text += "+ NO RESTARTS\n  (+500<color=orange>P</color>)\n";
			}
			this.noRestarts = true;
		}
		else
		{
			TMP_Text tmp_Text5 = this.extraInfo;
			tmp_Text5.text = tmp_Text5.text + "- <color=red>" + restarts.ToString() + "</color> RESTARTS\n";
		}
		if (!damage)
		{
			if (num >= 3)
			{
				TMP_Text tmp_Text6 = this.extraInfo;
				tmp_Text6.text += "+ <color=orange>NO DAMAGE</color>\n";
			}
			else
			{
				TMP_Text tmp_Text7 = this.extraInfo;
				tmp_Text7.text += "+ <color=orange>NO DAMAGE\n  (</color>+5,000<color=orange>P)</color>\n";
			}
			this.noDamage = true;
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x000418C6 File Offset: 0x0003FAC6
	public void SetRank(string rank)
	{
		this.totalRank.text = rank;
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x000418D4 File Offset: 0x0003FAD4
	public void SetSecrets(int secretsAmount, int maxSecrets)
	{
		this.secrets.text = 0.ToString() + " / " + maxSecrets.ToString();
		this.allSecrets = maxSecrets;
		this.secretsFound = secretsAmount;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00041914 File Offset: 0x0003FB14
	public void Appear()
	{
		if (this.i < this.toAppear.Length)
		{
			if (!this.casual)
			{
				if (this.skipping)
				{
					HudOpenEffect component = this.toAppear[this.i].GetComponent<HudOpenEffect>();
					if (component != null)
					{
						component.skip = true;
					}
				}
				if (this.toAppear[this.i] == this.time.gameObject)
				{
					if (this.skipping)
					{
						this.checkedSeconds = this.savedTime;
						this.seconds = this.savedTime;
						this.minutes = 0;
						while (this.seconds >= 60f)
						{
							this.seconds -= 60f;
							this.minutes++;
						}
						this.time.GetComponent<AudioSource>().playOnAwake = false;
						base.Invoke("Appear", this.timeBetween * 2f);
						this.time.text = this.minutes.ToString() + ":" + this.seconds.ToString("00.000");
					}
					else
					{
						this.countTime = true;
					}
				}
				else if (this.toAppear[this.i] == this.kills.gameObject)
				{
					if (this.skipping)
					{
						this.checkedKills = (float)this.savedKills;
						this.kills.GetComponent<AudioSource>().playOnAwake = false;
						base.Invoke("Appear", this.timeBetween * 2f);
						this.kills.text = this.checkedKills.ToString("0");
					}
					else
					{
						this.countKills = true;
					}
				}
				else if (this.toAppear[this.i] == this.style.gameObject)
				{
					if (this.skipping)
					{
						this.checkedStyle = (float)this.savedStyle;
						this.style.text = this.checkedStyle.ToString("0");
						this.style.GetComponent<AudioSource>().playOnAwake = false;
						base.Invoke("Appear", this.timeBetween * 2f);
						this.totalPoints += this.savedStyle;
						this.PointsShow();
					}
					else
					{
						this.countStyle = true;
					}
				}
				else if (this.toAppear[this.i] == this.secrets.gameObject)
				{
					if (this.prevSecrets.Count > 0)
					{
						foreach (int num in this.prevSecrets)
						{
							this.secretsInfo[num].color = new Color(0.5f, 0.5f, 0.5f);
							this.checkedSecrets++;
							this.secrets.text = this.checkedSecrets.ToString() + " / " + this.levelSecrets.Length.ToString();
						}
					}
					this.toAppear[this.i].gameObject.SetActive(true);
					base.Invoke("CountSecrets", this.timeBetween);
				}
				else if (this.toAppear[this.i] == this.timeRank.gameObject || this.toAppear[this.i] == this.killsRank.gameObject || this.toAppear[this.i] == this.styleRank.gameObject)
				{
					string text = this.toAppear[this.i].GetComponent<TMP_Text>().text;
					if (!(text == "<color=#0094FF>D</color>"))
					{
						if (!(text == "<color=#4CFF00>C</color>"))
						{
							if (!(text == "<color=#FFD800>B</color>"))
							{
								if (!(text == "<color=#FF6A00>A</color>"))
								{
									if (text == "<color=#FF0000>S</color>")
									{
										this.AddPoints(2500);
									}
								}
								else
								{
									this.AddPoints(2000);
								}
							}
							else
							{
								this.AddPoints(1500);
							}
						}
						else
						{
							this.AddPoints(1000);
						}
					}
					else
					{
						this.AddPoints(500);
					}
					this.FlashPanel(this.toAppear[this.i].transform.parent.GetChild(1).gameObject);
					base.Invoke("Appear", this.timeBetween * 2f);
					if (this.skipping)
					{
						this.toAppear[this.i].GetComponentInChildren<AudioSource>().playOnAwake = false;
					}
				}
				else if (this.toAppear[this.i] == this.totalRank.gameObject)
				{
					this.FlashPanel(this.toAppear[this.i].transform.parent.GetChild(1).gameObject);
					this.flashMultiplier = 0.5f;
					base.Invoke("Appear", this.timeBetween * 4f);
				}
				else if (this.toAppear[this.i] == this.extraInfo.gameObject)
				{
					if (this.noRestarts)
					{
						this.AddPoints(500);
					}
					if (this.noDamage)
					{
						this.AddPoints(5000);
					}
					base.Invoke("Appear", this.timeBetween);
				}
				else
				{
					base.Invoke("Appear", this.timeBetween);
				}
			}
			else
			{
				base.Invoke("Appear", this.timeBetween);
			}
			this.toAppear[this.i].gameObject.SetActive(true);
			this.i++;
			if (!this.toAppear[0].gameObject.activeSelf)
			{
				this.toAppear[0].gameObject.SetActive(true);
			}
			if (this.i >= this.toAppear.Length && !this.complete)
			{
				this.complete = true;
				GameProgressSaver.AddMoney(this.totalPoints);
				return;
			}
		}
		else if (!this.complete)
		{
			this.complete = true;
			GameProgressSaver.AddMoney(this.totalPoints);
		}
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00041F5C File Offset: 0x0004015C
	public void FlashPanel(GameObject panel)
	{
		if (this.flashFade)
		{
			this.flashColor.a = 0f;
			this.flashPanel.color = this.flashColor;
		}
		this.flashPanel = panel.GetComponent<Image>();
		this.flashColor = this.flashPanel.color;
		this.flashColor.a = 1f;
		this.flashPanel.color = this.flashColor;
		this.flashFade = true;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00041FD8 File Offset: 0x000401D8
	private void CountSecrets()
	{
		if (this.levelSecrets.Length == 0)
		{
			base.Invoke("Appear", this.timeBetween);
			return;
		}
		if (this.levelSecrets[this.secretsCheckProgress] == null && !this.prevSecrets.Contains(this.secretsCheckProgress))
		{
			this.checkedSecrets++;
			this.secrets.text = this.checkedSecrets.ToString() + " / " + this.levelSecrets.Length.ToString();
			this.secrets.GetComponent<AudioSource>().Play();
			this.secretsInfo[this.secretsCheckProgress].color = Color.white;
			this.secretsCheckProgress++;
			this.AddPoints(1000);
			if (this.secretsCheckProgress < this.levelSecrets.Length)
			{
				base.Invoke("CountSecrets", this.timeBetween);
				return;
			}
			base.Invoke("Appear", this.timeBetween);
			return;
		}
		else
		{
			if (this.secretsCheckProgress < this.levelSecrets.Length - 1)
			{
				this.secretsCheckProgress++;
				this.CountSecrets();
				return;
			}
			this.secretsCheckProgress++;
			base.Invoke("Appear", this.timeBetween);
			return;
		}
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00042129 File Offset: 0x00040329
	public void RanklessNextLevel(string lvlname)
	{
		if (lvlname != "")
		{
			this.rankless = true;
			this.targetLevelName = lvlname;
			SceneHelper.LoadScene(this.targetLevelName, true);
		}
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00042154 File Offset: 0x00040354
	public void LevelChange(bool force = false)
	{
		if (!SceneHelper.IsPlayingCustom)
		{
			if (this.playerPosInfo != null)
			{
				if (this.ppiObject == null)
				{
					this.ppiObject = Object.Instantiate<GameObject>(this.playerPosInfo);
				}
				PlayerPosInfo component = this.ppiObject.GetComponent<PlayerPosInfo>();
				Rigidbody component2 = MonoSingleton<NewMovement>.Instance.gameObject.GetComponent<Rigidbody>();
				component.velocity = component2.velocity;
				component.position = component2.transform.position - this.finalPitPos;
				component.position = new Vector3(component.position.x, component.position.y, component.position.z - 990f);
				component.wooshTime = component2.GetComponentInChildren<WallCheck>().GetComponent<AudioSource>().time;
				if (this.dontSavePos || this.targetLevelName == "Main Menu")
				{
					component.noPosition = true;
				}
			}
			base.gameObject.SetActive(false);
			SceneHelper.LoadScene(this.targetLevelName, true);
			return;
		}
		if (force)
		{
			MonoSingleton<OptionsManager>.Instance.QuitMission();
			return;
		}
		if (MonoSingleton<AdditionalMapDetails>.Instance && MonoSingleton<AdditionalMapDetails>.Instance.hasAuthorLinks)
		{
			base.gameObject.SetActive(false);
			MonoSingleton<WorkshopMapEndLinks>.Instance.Show();
			return;
		}
		if (GameStateManager.Instance.currentCustomGame != null && GameStateManager.Instance.currentCustomGame.workshopId != null)
		{
			MonoSingleton<WorkshopMapEndRating>.Instance.enabled = true;
			base.gameObject.SetActive(false);
			return;
		}
		MonoSingleton<OptionsManager>.Instance.QuitMission();
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x000422E2 File Offset: 0x000404E2
	public void AddPoints(int points)
	{
		this.totalPoints += points;
		this.PointsShow();
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x000422F8 File Offset: 0x000404F8
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

	// Token: 0x04000C2A RID: 3114
	public bool casual;

	// Token: 0x04000C2B RID: 3115
	public bool dontSavePos;

	// Token: 0x04000C2C RID: 3116
	public bool reachedSecondPit;

	// Token: 0x04000C2D RID: 3117
	public TMP_Text time;

	// Token: 0x04000C2E RID: 3118
	private float savedTime;

	// Token: 0x04000C2F RID: 3119
	public TMP_Text timeRank;

	// Token: 0x04000C30 RID: 3120
	private bool countTime;

	// Token: 0x04000C31 RID: 3121
	private int minutes;

	// Token: 0x04000C32 RID: 3122
	private float seconds;

	// Token: 0x04000C33 RID: 3123
	private float checkedSeconds;

	// Token: 0x04000C34 RID: 3124
	public TMP_Text kills;

	// Token: 0x04000C35 RID: 3125
	private int savedKills;

	// Token: 0x04000C36 RID: 3126
	public TMP_Text killsRank;

	// Token: 0x04000C37 RID: 3127
	private bool countKills;

	// Token: 0x04000C38 RID: 3128
	private float checkedKills;

	// Token: 0x04000C39 RID: 3129
	public TMP_Text style;

	// Token: 0x04000C3A RID: 3130
	private int savedStyle;

	// Token: 0x04000C3B RID: 3131
	public TMP_Text styleRank;

	// Token: 0x04000C3C RID: 3132
	private bool countStyle;

	// Token: 0x04000C3D RID: 3133
	private float checkedStyle;

	// Token: 0x04000C3E RID: 3134
	public TMP_Text extraInfo;

	// Token: 0x04000C3F RID: 3135
	public TMP_Text totalRank;

	// Token: 0x04000C40 RID: 3136
	public TMP_Text secrets;

	// Token: 0x04000C41 RID: 3137
	public Image[] secretsInfo;

	// Token: 0x04000C42 RID: 3138
	private int secretsFound;

	// Token: 0x04000C43 RID: 3139
	public GameObject[] levelSecrets;

	// Token: 0x04000C44 RID: 3140
	private int checkedSecrets;

	// Token: 0x04000C45 RID: 3141
	private int secretsCheckProgress;

	// Token: 0x04000C46 RID: 3142
	private int allSecrets;

	// Token: 0x04000C47 RID: 3143
	public List<int> prevSecrets;

	// Token: 0x04000C48 RID: 3144
	public Image[] challenges;

	// Token: 0x04000C49 RID: 3145
	public GameObject[] toAppear;

	// Token: 0x04000C4A RID: 3146
	private int i;

	// Token: 0x04000C4B RID: 3147
	private bool flashFade;

	// Token: 0x04000C4C RID: 3148
	private Image flashPanel;

	// Token: 0x04000C4D RID: 3149
	private Color flashColor;

	// Token: 0x04000C4E RID: 3150
	private float flashMultiplier = 1f;

	// Token: 0x04000C4F RID: 3151
	private Vector3 maxPos;

	// Token: 0x04000C50 RID: 3152
	private Vector3 startingPos;

	// Token: 0x04000C51 RID: 3153
	private Vector3 goalPos;

	// Token: 0x04000C52 RID: 3154
	public bool complete;

	// Token: 0x04000C53 RID: 3155
	public GameObject playerPosInfo;

	// Token: 0x04000C54 RID: 3156
	public Vector3 finalPitPos;

	// Token: 0x04000C55 RID: 3157
	private AsyncOperation asyncLoad;

	// Token: 0x04000C56 RID: 3158
	private string oldBundle;

	// Token: 0x04000C57 RID: 3159
	private bool rankless;

	// Token: 0x04000C58 RID: 3160
	public GameObject ppiObject;

	// Token: 0x04000C59 RID: 3161
	public string targetLevelName;

	// Token: 0x04000C5A RID: 3162
	public TMP_Text pointsText;

	// Token: 0x04000C5B RID: 3163
	public int totalPoints;

	// Token: 0x04000C5C RID: 3164
	private bool loadAndActivateScene;

	// Token: 0x04000C5D RID: 3165
	public bool dependenciesLoaded;

	// Token: 0x04000C5E RID: 3166
	private bool sceneBundleLoaded;

	// Token: 0x04000C5F RID: 3167
	private bool skipping;

	// Token: 0x04000C60 RID: 3168
	private float timeBetween = 0.25f;

	// Token: 0x04000C61 RID: 3169
	private bool noRestarts;

	// Token: 0x04000C62 RID: 3170
	private bool noDamage;

	// Token: 0x04000C63 RID: 3171
	private bool majorAssists;
}
