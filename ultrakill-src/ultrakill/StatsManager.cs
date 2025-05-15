using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200044D RID: 1101
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class StatsManager : MonoSingleton<StatsManager>
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x060018EE RID: 6382 RVA: 0x000CA52C File Offset: 0x000C872C
	// (remove) Token: 0x060018EF RID: 6383 RVA: 0x000CA560 File Offset: 0x000C8760
	public static event Action checkpointRestart;

	// Token: 0x060018F0 RID: 6384 RVA: 0x000CA594 File Offset: 0x000C8794
	protected override void Awake()
	{
		base.Awake();
		GameStateManager.Instance.RegisterState(new GameState("game", base.gameObject)
		{
			playerInputLock = LockMode.Unlock,
			cameraInputLock = LockMode.Unlock,
			cursorLock = LockMode.Lock
		});
		int num = -1;
		if (this.levelNumber == 0)
		{
			int? levelIndexAfterIntermission = MonoSingleton<SceneHelper>.Instance.GetLevelIndexAfterIntermission(SceneHelper.CurrentScene);
			if (levelIndexAfterIntermission != null)
			{
				num = levelIndexAfterIntermission.Value;
			}
		}
		RankData rank2 = GameProgressSaver.GetRank(true, num);
		if (SceneHelper.IsSceneRankless)
		{
			this.firstPlayThrough = false;
		}
		else
		{
			bool flag;
			if (rank2 != null)
			{
				if (rank2.ranks != null)
				{
					flag = rank2.ranks.All((int rank) => rank < 0);
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			this.firstPlayThrough = flag;
		}
		if (rank2 != null && rank2.levelNumber == this.levelNumber)
		{
			for (int i = 0; i < this.secretObjects.Length; i++)
			{
				if (!(this.secretObjects[i] == null))
				{
					Bonus bonus2;
					if (rank2.secretsFound.Length > i && rank2.secretsFound[i])
					{
						Bonus bonus;
						if (this.secretObjects[i].TryGetComponent<Bonus>(out bonus))
						{
							bonus.beenFound = true;
							bonus.BeenFound();
						}
						this.secretObjects[i] = null;
						this.prevSecrets.Add(i);
					}
					else if (this.secretObjects[i].TryGetComponent<Bonus>(out bonus2))
					{
						bonus2.secretNumber = i;
					}
				}
			}
			if (rank2.challenge)
			{
				this.challengeComplete = true;
				return;
			}
		}
		else
		{
			bool flag2 = false;
			for (int j = 0; j < this.secretObjects.Length; j++)
			{
				if (!(this.secretObjects[j] != null))
				{
					flag2 = true;
					break;
				}
			}
			if (this.secretObjects == null || (this.secretObjects.Length != 0 && flag2))
			{
				this.secretObjects = (from b in Object.FindObjectsOfType<Bonus>()
					select b.gameObject).ToArray<GameObject>();
				if (this.secretObjects.Length == 0)
				{
					Debug.Log("No secret objects found!");
					this.secretObjects = Array.Empty<GameObject>();
				}
			}
			for (int k = 0; k < this.secretObjects.Length; k++)
			{
				Bonus bonus3;
				if (this.secretObjects[k].TryGetComponent<Bonus>(out bonus3))
				{
					bonus3.secretNumber = k;
				}
			}
		}
	}

	// Token: 0x060018F1 RID: 6385 RVA: 0x000CA7DA File Offset: 0x000C89DA
	protected override void OnDestroy()
	{
		GameStateManager.Instance.PopState("game");
	}

	// Token: 0x060018F2 RID: 6386 RVA: 0x000CA7EC File Offset: 0x000C89EC
	private void Start()
	{
		this.nm = MonoSingleton<NewMovement>.Instance;
		this.player = this.nm.gameObject;
		this.spawnPos = this.player.transform.position;
		this.asscon = MonoSingleton<AssistController>.Instance;
		this.fr = MonoSingleton<FinalRank>.Instance;
		if (this.fr != null)
		{
			this.fr.gameObject.SetActive(false);
		}
		this.spawnPos = this.player.transform.position;
		GameObject gameObject = GameObject.FindWithTag("PlayerPosInfo");
		if (gameObject != null)
		{
			if (!SceneHelper.IsPlayingCustom)
			{
				PlayerPosInfo component = gameObject.GetComponent<PlayerPosInfo>();
				if (!component.noPosition)
				{
					this.player.transform.position = component.position;
				}
				this.player.GetComponent<Rigidbody>().velocity = component.velocity;
				this.player.GetComponentInChildren<WallCheck>().GetComponent<AudioSource>().time = component.wooshTime;
			}
			Object.Destroy(gameObject);
		}
		else
		{
			this.player.GetComponent<Rigidbody>().velocity = Vector3.down * 100f;
		}
		bool isPlayingCustom = SceneHelper.IsPlayingCustom;
		this.rankScore = -1;
	}

	// Token: 0x060018F3 RID: 6387 RVA: 0x000CA91C File Offset: 0x000C8B1C
	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.R) || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)) && this.nm.hp <= 0 && !this.nm.endlessMode && !MonoSingleton<OptionsManager>.Instance.paused)
		{
			this.Restart();
		}
		if (this.timer)
		{
			this.seconds += Time.deltaTime * GameStateManager.Instance.TimerModifier;
		}
		if (this.stylePoints < 0)
		{
			this.stylePoints = 0;
		}
		if (!this.endlessMode)
		{
			DiscordController.UpdateStyle(this.stylePoints);
		}
	}

	// Token: 0x060018F4 RID: 6388 RVA: 0x000CA9C9 File Offset: 0x000C8BC9
	public void GetCheckPoint(Vector3 position)
	{
		this.spawnPos = position;
	}

	// Token: 0x060018F5 RID: 6389 RVA: 0x000CA9D4 File Offset: 0x000C8BD4
	public void Restart()
	{
		MonoSingleton<MusicManager>.Instance.ArenaMusicEnd();
		this.timer = true;
		if (this.currentCheckPoint == null)
		{
			if (MonoSingleton<MapVarManager>.Instance)
			{
				MonoSingleton<MapVarManager>.Instance.ResetStores();
			}
			SceneHelper.RestartScene();
			return;
		}
		this.currentCheckPoint.OnRespawn();
		this.restarts++;
		Action action = StatsManager.checkpointRestart;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x060018F6 RID: 6390 RVA: 0x000CAA44 File Offset: 0x000C8C44
	public void StartTimer()
	{
		this.timer = true;
		if (!this.timerOnOnce)
		{
			if (this.asscon.majorEnabled)
			{
				if (!MonoSingleton<AssistController>.Instance.hidePopup)
				{
					MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=#4C99E6>MAJOR ASSISTS ARE ENABLED.</color>", "", "", 0, true, false, true);
				}
				this.MajorUsed();
			}
			MonoSingleton<PlayerTracker>.Instance.LevelStart();
			this.timerOnOnce = true;
		}
	}

	// Token: 0x060018F7 RID: 6391 RVA: 0x000CAAAD File Offset: 0x000C8CAD
	public void StopTimer()
	{
		this.timer = false;
	}

	// Token: 0x060018F8 RID: 6392 RVA: 0x000CAAB8 File Offset: 0x000C8CB8
	public void HideShit()
	{
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		this.shud.ComboOver();
		if (this.gunc == null)
		{
			this.gunc = MonoSingleton<GunControl>.Instance;
		}
		this.gunc.NoWeapon();
		if (this.crosshair)
		{
			this.crosshair.transform.parent.gameObject.SetActive(false);
		}
		HudController.Instance.gunCanvas.SetActive(false);
	}

	// Token: 0x060018F9 RID: 6393 RVA: 0x000CAB48 File Offset: 0x000C8D48
	public void SendInfo()
	{
		if (!this.infoSent)
		{
			this.infoSent = true;
			this.rankScore = 0;
			if (!this.fr)
			{
				this.fr = MonoSingleton<FinalRank>.Instance;
			}
			this.fr.gameObject.SetActive(true);
			if (this.fr.casual)
			{
				this.casualFR = true;
			}
			if (!this.casualFR)
			{
				string text = this.GetRanks(this.timeRanks, this.seconds, true, true);
				this.SetRankSound(text, this.fr.timeRank.gameObject);
				this.fr.SetTime(this.seconds, text);
				Debug.Log("Rankscore after time: " + this.rankScore.ToString());
				text = this.GetRanks(this.killRanks, (float)this.kills, false, true);
				this.SetRankSound(text, this.fr.killsRank.gameObject);
				this.fr.SetKills(this.kills, text);
				Debug.Log("Rankscore after kills: " + this.rankScore.ToString());
				text = this.GetRanks(this.styleRanks, (float)this.stylePoints, false, true);
				this.SetRankSound(text, this.fr.styleRank.gameObject);
				this.fr.SetStyle(this.stylePoints, text);
				Debug.Log("Rankscore after style: " + this.rankScore.ToString());
				this.fr.SetInfo(this.restarts, this.tookDamage, this.majorUsed, this.asscon.cheatsEnabled);
				this.GetFinalRank();
				GameProgressSaver.SaveRank();
				this.fr.SetSecrets(this.secrets, this.secretObjects.Length);
				this.fr.levelSecrets = this.secretObjects;
				this.fr.prevSecrets = this.prevSecrets;
				if (GameStateManager.CanSubmitScores)
				{
					string currentScene = SceneHelper.CurrentScene;
					if (!string.IsNullOrEmpty(currentScene) && SceneHelper.CurrentScene.StartsWith("Level ") && MonoSingleton<PrefsManager>.Instance.GetBool("levelLeaderboards", false))
					{
						int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
						MonoSingleton<LeaderboardController>.Instance.SubmitLevelScore(currentScene, @int, this.seconds, this.kills, this.stylePoints, this.restarts, false);
						if (this.rankScore == 12)
						{
							MonoSingleton<LeaderboardController>.Instance.SubmitLevelScore(currentScene, @int, this.seconds, this.kills, this.stylePoints, this.restarts, true);
						}
					}
				}
			}
			this.fr.Appear();
		}
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x000CADE4 File Offset: 0x000C8FE4
	public string GetRanks(int[] ranksToCheck, float value, bool reverse, bool addToRankScore = false)
	{
		int num = 0;
		bool flag = true;
		while (flag)
		{
			if (num >= ranksToCheck.Length)
			{
				if (addToRankScore)
				{
					this.rankScore += 4;
				}
				return "<color=#FF0000>S</color>";
			}
			if ((reverse && value <= (float)ranksToCheck[num]) || (!reverse && value >= (float)ranksToCheck[num]))
			{
				num++;
			}
			else
			{
				if (addToRankScore)
				{
					this.rankScore += num;
				}
				switch (num)
				{
				case 0:
					return "<color=#0094FF>D</color>";
				case 1:
					return "<color=#4CFF00>C</color>";
				case 2:
					return "<color=#FFD800>B</color>";
				case 3:
					return "<color=#FF6A00>A</color>";
				}
			}
		}
		return "X";
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x000CAE7C File Offset: 0x000C907C
	private void GetFinalRank()
	{
		if (this.restarts != 0)
		{
			this.rankScore -= this.restarts;
		}
		if (this.rankScore < 0)
		{
			this.rankScore = 0;
		}
		if (this.majorUsed)
		{
			if (this.rankScore == 12)
			{
				this.rankScore = 11;
			}
			this.fr.totalRank.transform.parent.GetComponent<Image>().color = new Color(0.3f, 0.6f, 0.9f, 1f);
		}
		string text;
		if (this.rankScore == 12 && !this.asscon.cheatsEnabled)
		{
			text = "<color=#FFFFFF>P</color>";
			this.fr.totalRank.transform.parent.GetComponent<Image>().color = new Color(1f, 0.686f, 0f, 1f);
		}
		else
		{
			float num = (float)this.rankScore / 3f;
			Debug.Log("Float: " + num.ToString());
			Debug.Log("PreInt: " + this.rankScore.ToString());
			this.rankScore = Mathf.RoundToInt(num);
			Debug.Log("PostInt: " + this.rankScore.ToString());
			if (this.asscon.cheatsEnabled)
			{
				text = "-";
			}
			else if (this.majorUsed)
			{
				switch (this.rankScore)
				{
				case 1:
					text = "C";
					break;
				case 2:
					text = "B";
					break;
				case 3:
					text = "A";
					break;
				case 4:
				case 5:
				case 6:
					text = "S";
					break;
				default:
					text = "D";
					break;
				}
			}
			else
			{
				switch (this.rankScore)
				{
				case 1:
					text = "<color=#4CFF00>C</color>";
					break;
				case 2:
					text = "<color=#FFD800>B</color>";
					break;
				case 3:
					text = "<color=#FF6A00>A</color>";
					break;
				case 4:
				case 5:
				case 6:
					text = "<color=#FF0000>S</color>";
					break;
				default:
					text = "<color=#0094FF>D</color>";
					break;
				}
			}
		}
		if (this.asscon.cheatsEnabled)
		{
			this.rankScore = -1;
			text = "<color=#FFFFFF>_</color>";
			this.fr.totalRank.transform.parent.GetComponent<Image>().color = new Color(0.25f, 1f, 0.25f);
		}
		this.fr.SetRank(text);
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x000CB0E4 File Offset: 0x000C92E4
	private void SetRankSound(string rank, GameObject target)
	{
		if (rank == "<color=#FFD800>B</color>")
		{
			target.GetComponent<AudioSource>().clip = this.rankSounds[0];
			return;
		}
		if (rank == "<color=#FF6A00>A</color>")
		{
			target.GetComponent<AudioSource>().clip = this.rankSounds[1];
			return;
		}
		if (!(rank == "<color=#FF0000>S</color>"))
		{
			return;
		}
		target.GetComponent<AudioSource>().clip = this.rankSounds[2];
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x000CB154 File Offset: 0x000C9354
	public void MajorUsed()
	{
		if (this.timer && !this.majorUsed && !MonoSingleton<AssistController>.Instance.hidePopup)
		{
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("<color=#4C99E6>MAJOR ASSISTS ARE ENABLED.</color>", "", "", 0, true, false, true);
		}
		if (this.timer)
		{
			this.majorUsed = true;
		}
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x000CB1A9 File Offset: 0x000C93A9
	public void SecretFound(int i)
	{
		if (!this.prevSecrets.Contains(i) && !this.newSecrets.Contains(i))
		{
			GameProgressSaver.SecretFound(i);
			this.newSecrets.Add(i);
			this.secretObjects[i] = null;
			return;
		}
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x000CB1E4 File Offset: 0x000C93E4
	public static string DivideMoney(int money)
	{
		int i = money;
		int num = 0;
		while (i >= 1000)
		{
			num++;
			i -= 1000;
		}
		string text;
		if (num > 0)
		{
			if (i < 10)
			{
				text = num.ToString() + ",00" + i.ToString();
			}
			else if (i < 100)
			{
				text = num.ToString() + ",0" + i.ToString();
			}
			else
			{
				text = num.ToString() + "," + i.ToString();
			}
		}
		else
		{
			text = i.ToString() ?? "";
		}
		return text;
	}

	// Token: 0x040022D6 RID: 8918
	[HideInInspector]
	public GameObject[] checkPoints;

	// Token: 0x040022D7 RID: 8919
	private GameObject player;

	// Token: 0x040022D8 RID: 8920
	private NewMovement nm;

	// Token: 0x040022D9 RID: 8921
	[HideInInspector]
	public Vector3 spawnPos;

	// Token: 0x040022DA RID: 8922
	[HideInInspector]
	public CheckPoint currentCheckPoint;

	// Token: 0x040022DB RID: 8923
	public int levelNumber;

	// Token: 0x040022DC RID: 8924
	[HideInInspector]
	public int kills;

	// Token: 0x040022DD RID: 8925
	[HideInInspector]
	public int stylePoints;

	// Token: 0x040022DE RID: 8926
	[HideInInspector]
	public int restarts;

	// Token: 0x040022DF RID: 8927
	[HideInInspector]
	public int secrets;

	// Token: 0x040022E0 RID: 8928
	[HideInInspector]
	public float seconds;

	// Token: 0x040022E1 RID: 8929
	public bool timer;

	// Token: 0x040022E2 RID: 8930
	public bool firstPlayThrough;

	// Token: 0x040022E3 RID: 8931
	private bool timerOnOnce;

	// Token: 0x040022E4 RID: 8932
	[HideInInspector]
	public bool levelStarted;

	// Token: 0x040022E5 RID: 8933
	[HideInInspector]
	public FinalRank fr;

	// Token: 0x040022E6 RID: 8934
	private StyleHUD shud;

	// Token: 0x040022E7 RID: 8935
	private GunControl gunc;

	// Token: 0x040022E8 RID: 8936
	[HideInInspector]
	public bool infoSent;

	// Token: 0x040022E9 RID: 8937
	private bool casualFR;

	// Token: 0x040022EA RID: 8938
	public int[] timeRanks;

	// Token: 0x040022EB RID: 8939
	public int[] killRanks;

	// Token: 0x040022EC RID: 8940
	public int[] styleRanks;

	// Token: 0x040022ED RID: 8941
	[HideInInspector]
	public int rankScore;

	// Token: 0x040022EE RID: 8942
	public GameObject[] secretObjects;

	// Token: 0x040022EF RID: 8943
	[HideInInspector]
	public List<int> prevSecrets;

	// Token: 0x040022F0 RID: 8944
	[HideInInspector]
	public List<int> newSecrets = new List<int>();

	// Token: 0x040022F1 RID: 8945
	[HideInInspector]
	public bool challengeComplete;

	// Token: 0x040022F2 RID: 8946
	public AudioClip[] rankSounds;

	// Token: 0x040022F3 RID: 8947
	[HideInInspector]
	public int maxGlassKills;

	// Token: 0x040022F4 RID: 8948
	[HideInInspector]
	public GameObject crosshair;

	// Token: 0x040022F5 RID: 8949
	[HideInInspector]
	public bool tookDamage;

	// Token: 0x040022F6 RID: 8950
	public GameObject bonusGhost;

	// Token: 0x040022F7 RID: 8951
	public GameObject bonusGhostSupercharge;

	// Token: 0x040022F8 RID: 8952
	[HideInInspector]
	public bool majorUsed;

	// Token: 0x040022F9 RID: 8953
	[HideInInspector]
	public bool endlessMode;

	// Token: 0x040022FA RID: 8954
	private AssistController asscon;
}
