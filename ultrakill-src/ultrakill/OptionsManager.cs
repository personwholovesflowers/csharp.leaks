using System;
using System.Linq;
using Logic;
using SettingsMenu.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000325 RID: 805
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class OptionsManager : MonoSingleton<OptionsManager>
{
	// Token: 0x06001285 RID: 4741 RVA: 0x00094452 File Offset: 0x00092652
	protected override void Awake()
	{
		base.Awake();
		if (GameObject.FindWithTag("OptionsManager") == null)
		{
			Object.Instantiate<GameObject>(this.progressChecker);
		}
		base.transform.SetParent(null);
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00094484 File Offset: 0x00092684
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x000944AC File Offset: 0x000926AC
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x000944D0 File Offset: 0x000926D0
	private void OnPrefChanged(string key, object value)
	{
		if (!(key == "mouseSensitivity"))
		{
			if (!(key == "bloodStainChance"))
			{
				if (!(key == "bloodStainMax"))
				{
					if (!(key == "maxGore"))
					{
						if (!(key == "simplifyEnemies"))
						{
							if (!(key == "simplifyEnemiesDistance"))
							{
								return;
							}
							if (value is float)
							{
								float num = (float)value;
								this.simplifiedDistance = num;
							}
						}
						else if (value is int)
						{
							int num2 = (int)value;
							this.SetSimplifyEnemies(num2);
							return;
						}
					}
					else if (value is float)
					{
						float num3 = (float)value;
						this.maxGore = num3;
						return;
					}
				}
				else if (value is float)
				{
					float num4 = (float)value;
					this.maxStains = num4;
					return;
				}
			}
			else if (value is float)
			{
				float num5 = (float)value;
				this.bloodstainChance = num5 * 100f;
				return;
			}
		}
		else if (value is float)
		{
			float num6 = (float)value;
			this.mouseSensitivity = num6;
			return;
		}
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x000945CC File Offset: 0x000927CC
	public void SetSimplifyEnemies(int value)
	{
		bool flag = value != 0;
		this.simplifyEnemies = flag;
		this.outlinesOnly = value == 1;
		Debug.Log(string.Format("val: {0} simplifyEnemies: {1} outlinesOnly: {2}", value, flag, this.outlinesOnly));
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x00094618 File Offset: 0x00092818
	private void Start()
	{
		this.mouseSensitivity = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("mouseSensitivity", 0f);
		this.bloodstainChance = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("bloodStainChance", 0f) * 100f;
		this.maxStains = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("bloodStainMax", 0f);
		this.maxGore = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("maxGore", 0f);
		this.SetSimplifyEnemies(MonoSingleton<PrefsManager>.Instance.GetInt("simplifyEnemies", 0));
		this.simplifiedDistance = MonoSingleton<PrefsManager>.Instance.GetFloat("simplifyEnemiesDistance", 0f);
		if (MonoSingleton<CheatsController>.Instance == null || MonoSingleton<CheatsController>.Instance.cheatsEnabled)
		{
			return;
		}
		if (OptionsManager.forceRadiance)
		{
			OptionsManager.forceRadiance = false;
		}
		if (OptionsManager.forceSand)
		{
			OptionsManager.forceSand = false;
		}
		if (OptionsManager.forcePuppet)
		{
			OptionsManager.forcePuppet = false;
		}
		if (OptionsManager.forceBossBars)
		{
			OptionsManager.forceBossBars = false;
		}
		if (OptionsManager.radianceTier != 1f)
		{
			OptionsManager.radianceTier = 1f;
		}
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00094728 File Offset: 0x00092928
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Return))
		{
			if (!Screen.fullScreen)
			{
				Screen.SetResolution(Screen.resolutions.Last<Resolution>().width, Screen.resolutions.Last<Resolution>().height, true);
			}
			else
			{
				Screen.fullScreen = false;
			}
		}
		if (this.frozen)
		{
			return;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame && !this.inIntro && !this.mainMenu)
		{
			if (!this.paused)
			{
				this.Pause();
			}
			else if (!this.dontUnpause)
			{
				if (SandboxHud.SavesMenuOpen)
				{
					Debug.Log("Closing sandbox saves menu first");
					MonoSingleton<SandboxHud>.Instance.HideSavesMenu();
					return;
				}
				this.CloseOptions();
				this.UnPause();
			}
		}
		if (this.mainMenu && !this.paused)
		{
			this.Pause();
		}
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x00094814 File Offset: 0x00092A14
	private void LateUpdate()
	{
		if (this.paused)
		{
			if (this.mainMenu)
			{
				Time.timeScale = 1f;
			}
			if (!this.mainMenu)
			{
				Time.timeScale = 0f;
			}
		}
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00094844 File Offset: 0x00092A44
	public void Pause()
	{
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
			this.gc = this.nm.GetComponentInChildren<GunControl>();
			this.fc = this.nm.GetComponentInChildren<FistControl>();
		}
		if (!this.mainMenu)
		{
			this.nm.enabled = false;
			MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", 0f);
			MonoSingleton<AudioMixerController>.Instance.doorSound.SetFloat("allPitch", 0f);
			if (MonoSingleton<MusicManager>.Instance)
			{
				MonoSingleton<MusicManager>.Instance.FilterMusic();
			}
		}
		GameStateManager.Instance.RegisterState(new GameState("pause", new GameObject[]
		{
			this.pauseMenu,
			this.optionsMenu.gameObject
		})
		{
			cursorLock = LockMode.Unlock,
			cameraInputLock = LockMode.Lock,
			playerInputLock = LockMode.Lock
		});
		MonoSingleton<CameraController>.Instance.activated = false;
		this.gc.activated = false;
		this.paused = true;
		if (this.pauseMenu)
		{
			this.pauseMenu.SetActive(true);
		}
		foreach (VideoPlayer videoPlayer in Object.FindObjectsOfType<VideoPlayer>())
		{
			if (videoPlayer.isPlaying)
			{
				videoPlayer.Pause();
			}
		}
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x00094990 File Offset: 0x00092B90
	public void UnPause()
	{
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
			this.gc = this.nm.GetComponentInChildren<GunControl>();
			this.fc = this.nm.GetComponentInChildren<FistControl>();
		}
		this.CloseOptions();
		this.paused = false;
		Time.timeScale = MonoSingleton<TimeController>.Instance.timeScale * MonoSingleton<TimeController>.Instance.timeScaleModifier;
		MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", 1f);
		MonoSingleton<AudioMixerController>.Instance.doorSound.SetFloat("allPitch", 1f);
		if (MonoSingleton<MusicManager>.Instance)
		{
			MonoSingleton<MusicManager>.Instance.UnfilterMusic();
		}
		if (!this.nm.dead)
		{
			this.nm.enabled = true;
			MonoSingleton<CameraController>.Instance.activated = true;
			if (!this.fc || !this.fc.shopping)
			{
				if (!this.gc.stayUnarmed)
				{
					this.gc.activated = true;
				}
				if (this.fc != null)
				{
					this.fc.activated = true;
				}
			}
		}
		if (this.pauseMenu)
		{
			this.pauseMenu.SetActive(false);
		}
		foreach (VideoPlayer videoPlayer in Object.FindObjectsOfType<VideoPlayer>())
		{
			if (videoPlayer.isPaused)
			{
				videoPlayer.Play();
			}
		}
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00094AFC File Offset: 0x00092CFC
	public void Freeze()
	{
		this.frozen = true;
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
			this.gc = this.nm.GetComponentInChildren<GunControl>();
			this.fc = this.nm.GetComponentInChildren<FistControl>();
		}
		MonoSingleton<CameraController>.Instance.activated = false;
		this.previousWeaponState = !this.gc.noWeapons;
		this.gc.NoWeapon();
		this.gc.enabled = false;
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00094B84 File Offset: 0x00092D84
	public void UnFreeze()
	{
		this.frozen = false;
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
			this.gc = this.nm.GetComponentInChildren<GunControl>();
			this.fc = this.nm.GetComponentInChildren<FistControl>();
		}
		MonoSingleton<CameraController>.Instance.activated = true;
		if (this.previousWeaponState)
		{
			this.gc.YesWeapon();
		}
		this.gc.enabled = true;
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00094C00 File Offset: 0x00092E00
	public void RestartCheckpoint()
	{
		this.UnPause();
		StatsManager instance = MonoSingleton<StatsManager>.Instance;
		if (!instance.infoSent)
		{
			instance.Restart();
		}
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00094C27 File Offset: 0x00092E27
	public void RestartMission()
	{
		Time.timeScale = 1f;
		SceneHelper.RestartScene();
		if (MonoSingleton<MapVarManager>.Instance)
		{
			MonoSingleton<MapVarManager>.Instance.ResetStores();
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x00094C59 File Offset: 0x00092E59
	public void OpenOptions()
	{
		this.pauseMenu.SetActive(false);
		this.optionsMenu.gameObject.SetActive(true);
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x00094C78 File Offset: 0x00092E78
	public void CloseOptions()
	{
		this.optionsMenu.gameObject.SetActive(false);
		if (MonoSingleton<CheatsManager>.Instance)
		{
			MonoSingleton<CheatsManager>.Instance.HideMenu();
		}
		this.pauseMenu.SetActive(true);
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x00094CAD File Offset: 0x00092EAD
	public void QuitMission()
	{
		Time.timeScale = 1f;
		SceneHelper.LoadScene("Main Menu", false);
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x00094CC4 File Offset: 0x00092EC4
	public void QuitGame()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x00094CD3 File Offset: 0x00092ED3
	public void ChangeLevel(string levelname)
	{
		this.SetChangeLevelPosition(true);
		SceneHelper.LoadScene(levelname, false);
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00094CE3 File Offset: 0x00092EE3
	public void ChangeLevelAbrupt(string scene)
	{
		SceneHelper.LoadScene(scene, false);
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x00094CEC File Offset: 0x00092EEC
	public void ChangeLevelWithPosition(string levelname)
	{
		if (Application.CanStreamedLevelBeLoaded(levelname))
		{
			this.SetChangeLevelPosition(false);
			SceneManager.LoadScene(levelname);
			return;
		}
		SceneHelper.LoadScene("Main Menu", false);
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x00094D10 File Offset: 0x00092F10
	public void SetChangeLevelPosition(bool noPosition)
	{
		if (this.nm == null)
		{
			this.nm = MonoSingleton<NewMovement>.Instance;
		}
		PlayerPosInfo component = Object.Instantiate<GameObject>(this.playerPosInfo).GetComponent<PlayerPosInfo>();
		component.velocity = this.nm.GetComponent<Rigidbody>().velocity;
		component.wooshTime = this.nm.GetComponentInChildren<WallCheck>().GetComponent<AudioSource>().time;
		component.noPosition = noPosition;
	}

	// Token: 0x04001977 RID: 6519
	public bool mainMenu;

	// Token: 0x04001978 RID: 6520
	[HideInInspector]
	public bool paused;

	// Token: 0x04001979 RID: 6521
	public bool inIntro;

	// Token: 0x0400197A RID: 6522
	public bool frozen;

	// Token: 0x0400197B RID: 6523
	[HideInInspector]
	public GameObject pauseMenu;

	// Token: 0x0400197C RID: 6524
	[HideInInspector]
	public SettingsMenu optionsMenu;

	// Token: 0x0400197D RID: 6525
	public GameObject progressChecker;

	// Token: 0x0400197E RID: 6526
	private NewMovement nm;

	// Token: 0x0400197F RID: 6527
	private GunControl gc;

	// Token: 0x04001980 RID: 6528
	private FistControl fc;

	// Token: 0x04001981 RID: 6529
	[HideInInspector]
	public float mouseSensitivity;

	// Token: 0x04001982 RID: 6530
	[HideInInspector]
	public float simplifiedDistance;

	// Token: 0x04001983 RID: 6531
	[HideInInspector]
	public bool simplifyEnemies;

	// Token: 0x04001984 RID: 6532
	[HideInInspector]
	public bool outlinesOnly;

	// Token: 0x04001985 RID: 6533
	private int screenWidth;

	// Token: 0x04001986 RID: 6534
	private int screenHeight;

	// Token: 0x04001987 RID: 6535
	[HideInInspector]
	public Toggle fullScreen;

	// Token: 0x04001988 RID: 6536
	[HideInInspector]
	public float bloodstainChance;

	// Token: 0x04001989 RID: 6537
	[HideInInspector]
	public float maxGore;

	// Token: 0x0400198A RID: 6538
	[HideInInspector]
	public float maxStains;

	// Token: 0x0400198B RID: 6539
	[HideInInspector]
	public GameObject playerPosInfo;

	// Token: 0x0400198C RID: 6540
	[HideInInspector]
	public bool dontUnpause;

	// Token: 0x0400198D RID: 6541
	public bool previousWeaponState;

	// Token: 0x0400198E RID: 6542
	public static bool forceRadiance;

	// Token: 0x0400198F RID: 6543
	public static bool forceSand;

	// Token: 0x04001990 RID: 6544
	public static bool forcePuppet;

	// Token: 0x04001991 RID: 6545
	public static bool forceBossBars;

	// Token: 0x04001992 RID: 6546
	public static float radianceTier = 1f;
}
