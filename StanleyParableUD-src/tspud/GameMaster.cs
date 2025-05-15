using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000DC RID: 220
public class GameMaster : Singleton<GameMaster>
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001CDEC File Offset: 0x0001AFEC
	public AchievementsData AchievementsData
	{
		get
		{
			return this.achievementsData;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001CDF4 File Offset: 0x0001AFF4
	public MainMenu MenuManager
	{
		get
		{
			return this.menuManager;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000508 RID: 1288 RVA: 0x0001CDFC File Offset: 0x0001AFFC
	public bool FullScreenMoviePlaying
	{
		get
		{
			return this.fullscreenPlaybackContext != null && this.fullscreenPlaybackContext.moviePlaying;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001CE13 File Offset: 0x0001B013
	private IEnumerable<GameMaster.MoviePlaybackContext> AllMoviePlaybackContexts
	{
		get
		{
			if (this.fullscreenPlaybackContext != null)
			{
				yield return this.fullscreenPlaybackContext;
			}
			foreach (KeyValuePair<string, GameMaster.MoviePlaybackContext> keyValuePair in this.inGamePlaybackContexts)
			{
				yield return keyValuePair.Value;
			}
			Dictionary<string, GameMaster.MoviePlaybackContext>.Enumerator enumerator = default(Dictionary<string, GameMaster.MoviePlaybackContext>.Enumerator);
			yield break;
			yield break;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600050A RID: 1290 RVA: 0x0001CE23 File Offset: 0x0001B023
	private bool AnyMoviePlaying
	{
		get
		{
			return new List<GameMaster.MoviePlaybackContext>(this.AllMoviePlaybackContexts).FindIndex((GameMaster.MoviePlaybackContext m) => m.moviePlaying) != -1;
		}
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0001CE5A File Offset: 0x0001B05A
	private IMoviePlayer GetNewPlatformMoviePlayer()
	{
		return new PCMoviePlayer();
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600050C RID: 1292 RVA: 0x0001CE61 File Offset: 0x0001B061
	// (set) Token: 0x0600050D RID: 1293 RVA: 0x0001CE69 File Offset: 0x0001B069
	[HideInInspector]
	public fint32 GameTime { get; private set; }

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001CE72 File Offset: 0x0001B072
	// (set) Token: 0x0600050F RID: 1295 RVA: 0x0001CE7A File Offset: 0x0001B07A
	[HideInInspector]
	public float GameDeltaTime { get; private set; }

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001CE83 File Offset: 0x0001B083
	// (set) Token: 0x06000511 RID: 1297 RVA: 0x0001CE8B File Offset: 0x0001B08B
	public float GameSmoothedDeltaTime { get; private set; }

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001CE94 File Offset: 0x0001B094
	// (set) Token: 0x06000513 RID: 1299 RVA: 0x0001CE9B File Offset: 0x0001B09B
	public static fint32 RunTime { get; private set; }

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000514 RID: 1300 RVA: 0x0001CEA4 File Offset: 0x0001B0A4
	// (remove) Token: 0x06000515 RID: 1301 RVA: 0x0001CED8 File Offset: 0x0001B0D8
	public static event GameMaster.Pause OnPause;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000516 RID: 1302 RVA: 0x0001CF0C File Offset: 0x0001B10C
	// (remove) Token: 0x06000517 RID: 1303 RVA: 0x0001CF40 File Offset: 0x0001B140
	public static event GameMaster.Pause OnResume;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000518 RID: 1304 RVA: 0x0001CF74 File Offset: 0x0001B174
	// (remove) Token: 0x06000519 RID: 1305 RVA: 0x0001CFA8 File Offset: 0x0001B1A8
	public static event Action OnPrepareLoadingLevel;

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001CFDB File Offset: 0x0001B1DB
	// (set) Token: 0x0600051B RID: 1307 RVA: 0x0001CFE3 File Offset: 0x0001B1E3
	public bool MouseMoved { get; private set; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001CFEC File Offset: 0x0001B1EC
	// (set) Token: 0x0600051D RID: 1309 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
	public float masterVolume
	{
		get
		{
			return this._masterVolume;
		}
		private set
		{
			if (value != this._masterVolume)
			{
				this._masterVolume = Mathf.Clamp01(value);
			}
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001D00B File Offset: 0x0001B20B
	// (set) Token: 0x0600051F RID: 1311 RVA: 0x0001D013 File Offset: 0x0001B213
	public float musicVolume
	{
		get
		{
			return this._musicVolume;
		}
		private set
		{
			this._musicVolume = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001D021 File Offset: 0x0001B221
	public float modulatedMusicVolume
	{
		get
		{
			return this.musicVolume * this.masterVolume;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001D030 File Offset: 0x0001B230
	// (set) Token: 0x06000522 RID: 1314 RVA: 0x0001D038 File Offset: 0x0001B238
	public bool closedCaptionsOn
	{
		get
		{
			return this._closedCaptionsOn;
		}
		private set
		{
			if (value != this._closedCaptionsOn)
			{
				this._closedCaptionsOn = value;
			}
		}
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001D04C File Offset: 0x0001B24C
	private void ResetTSPOriginalValues()
	{
		this.beginAgainMap1 = false;
		this.beginAgainMap2 = false;
		this.loungePassCounter = 1;
		this.broomPassCounter = 1;
		this.bossPassCounter = 1;
		this.bossSkipCounter = 1;
		this.countPassCounter = 1;
		this.buttonPassCounter = 1;
		this.seriousPassCounter = 1;
		this.boxesNextTime = false;
		this.barking = false;
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001D0A6 File Offset: 0x0001B2A6
	// (set) Token: 0x06000525 RID: 1317 RVA: 0x0001D0AE File Offset: 0x0001B2AE
	public bool barking { get; private set; }

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001D0B7 File Offset: 0x0001B2B7
	public bool IsLoading
	{
		get
		{
			return this.loading;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000527 RID: 1319 RVA: 0x0001D0BF File Offset: 0x0001B2BF
	public float loadProgress
	{
		get
		{
			return this._loadProgress;
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00005444 File Offset: 0x00003644
	private void OnLoadingScreenSetupDone()
	{
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x0001D0C8 File Offset: 0x0001B2C8
	protected override void Awake()
	{
		base.Awake();
		this.UpdateSceneAudioSource();
		if (this != Singleton<GameMaster>.Instance)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		LoadingManager.OnLoadingScreenSetupDone = (Action)Delegate.Combine(LoadingManager.OnLoadingScreenSetupDone, new Action(this.OnLoadingScreenSetupDone));
		SceneManager.sceneLoaded += this.UpdateSceneAudioSource;
		AssetBundleControl.OnSceneReady = (Action)Delegate.Combine(AssetBundleControl.OnSceneReady, new Action(this.OnSceneReady));
		PlatformPlayerPrefsHelper.saveSystemInitialized = (Action)Delegate.Combine(PlatformPlayerPrefsHelper.saveSystemInitialized, new Action(this.ReadAllPrefs));
		StanleyController.OnOutOfBounds = (Action)Delegate.Combine(StanleyController.OnOutOfBounds, new Action(this.OnOutOfBounds));
		PlatformAchievements.OnAchievementUnlockedFirstTime += this.RecordAchievementDate;
		if (this.ReporterPrefab != null)
		{
			Object.Instantiate<GameObject>(this.ReporterPrefab);
		}
		ReportUI.OnOpenReportTool = (Action)Delegate.Combine(ReportUI.OnOpenReportTool, new Action(this.OnReportPause));
		PlatformManager.Instance.Init();
		this.languageProfile.SetNewMaxValue(this.languageProfileData.profiles.Length - 1);
		IntConfigurable intConfigurable = this.languageProfile;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnLanguageChanged));
		this.InitMainMenu();
		this.stanleyActions = StanleyActions.CreateWithDefaultBindings();
		this.stanleyActions.LoadCustomKeyBindings(this.keyBindings);
		this.InputTypeDetectionInit();
		StanleyController stanleyController = Object.FindObjectOfType<StanleyController>();
		if (this.stanleyPrefab)
		{
			bool flag = stanleyController != null;
			Vector3 vector = (flag ? stanleyController.transform.position : this.stanleyPrefab.transform.position);
			Quaternion quaternion = (flag ? stanleyController.transform.rotation : this.stanleyPrefab.transform.rotation);
			Object.DontDestroyOnLoad(Object.Instantiate<GameObject>(this.stanleyPrefab, vector, quaternion));
			GameObject gameObject = Object.Instantiate<GameObject>(this.choreoPrefab);
			this.captionCanvases = gameObject.GetComponentsInChildren<Canvas>();
		}
		else
		{
			Debug.LogWarning("GameMaster has been made via means other than the menu, hopefully you're just running this in the editor?");
		}
		if (this.crosshair == null && this.crosshairPrefab != null)
		{
			this.crosshair = Object.Instantiate<GameObject>(this.crosshairPrefab);
			Object.DontDestroyOnLoad(this.crosshair);
		}
		if (this.eyelidsObj == null && this.eyelidsPrefab != null)
		{
			this.eyelidsObj = Object.Instantiate<GameObject>(this.eyelidsPrefab);
			this.eyelids = this.eyelidsObj.GetComponent<Eyelids>();
			Object.DontDestroyOnLoad(this.eyelidsObj);
			this.eyelidCanvas = this.eyelidsObj.GetComponent<Canvas>();
		}
		if (this.fadeCanvasPrefab != null)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.fadeCanvasPrefab);
			gameObject2.transform.parent = base.transform;
			this.fadeImage = gameObject2.GetComponentInChildren<RawImage>();
			this.fadeImage.enabled = false;
			this.fadeImage.color = Color.clear;
			this.fadeCanvas = gameObject2.GetComponent<Canvas>();
			this.fadeCanvas.enabled = false;
		}
		Object.Instantiate<GameObject>(this.figleyOverlayPrefab).transform.parent = base.transform;
		Object.Instantiate<GameObject>(this.platformSettingsPrefab).transform.parent = base.transform;
		Object.Instantiate<GameObject>(this.cursorControllerPrefab).transform.parent = base.transform;
		GameObject gameObject3 = Object.Instantiate<GameObject>(this.resolutionControllerPrefab);
		gameObject3.transform.parent = base.transform;
		Object.Instantiate<GameObject>(this.motionControllerPrefab);
		gameObject3.transform.parent = base.transform;
		GameMaster.ONMAINMENUORSETTINGS = this.isOnMainMenuOrSettingScene(SceneManager.GetActiveScene().name);
		foreach (Configurable configurable in this.resetableConfigurablesList.allConfigurables)
		{
			configurable.Init();
		}
		if (StanleyController.Instance != null && !GameMaster.ONMAINMENUORSETTINGS)
		{
			StanleyController.Instance.FreezeMotionAndView();
		}
		SceneManager.sceneLoaded += this.SceneLoaded;
		this.GameTime = new fint32(0U);
		if (PlatformManager.UseLowEndConfiguration)
		{
			this.useDefaultPerformanceConfiguration.SetValue(false);
		}
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x0001D514 File Offset: 0x0001B714
	private void RecordAchievementDate(AchievementID achievementID)
	{
		string text = DateTime.Now.ToShortDateString();
		this.achievementsData.FindAchievement(achievementID).dateFoundConfigurable.SetValue(text);
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x0001D548 File Offset: 0x0001B748
	private void OnOutOfBounds()
	{
		string name = SceneManager.GetActiveScene().name;
		this.ChangeLevel(name, true);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001D56C File Offset: 0x0001B76C
	private void InitMainMenu()
	{
		this.gameMenu = Object.Instantiate<GameObject>(this.menuPrefab);
		this.gameMenu.transform.parent = base.transform;
		this.menuManager = this.gameMenu.GetComponent<MainMenu>();
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600052D RID: 1325 RVA: 0x0001D5A8 File Offset: 0x0001B7A8
	// (remove) Token: 0x0600052E RID: 1326 RVA: 0x0001D5DC File Offset: 0x0001B7DC
	public static event Action OnFullDataReset;

	// Token: 0x0600052F RID: 1327 RVA: 0x0001D60F File Offset: 0x0001B80F
	public void ReInit()
	{
		Object.Destroy(this.gameMenu);
		this.gameMenu = null;
		this.InitMainMenu();
		Action onFullDataReset = GameMaster.OnFullDataReset;
		if (onFullDataReset != null)
		{
			onFullDataReset();
		}
		this.ResetTSPOriginalValues();
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001D63F File Offset: 0x0001B83F
	private void OnLanguageChanged(LiveData liveData)
	{
		LocalizationManager.CurrentLanguage = this.languageProfileData.profiles[liveData.IntValue].DescriptionIni2Loc;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x0001D65D File Offset: 0x0001B85D
	private void OnAchievementUnlock(int id)
	{
		this.achievementConfigurables[id].SetValue(true);
		this.achievementConfigurables[id].SaveToDiskAll();
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x0001D67A File Offset: 0x0001B87A
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.SceneLoaded;
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x0001D690 File Offset: 0x0001B890
	private void OnDestroy()
	{
		LoadingManager.OnLoadingScreenSetupDone = (Action)Delegate.Remove(LoadingManager.OnLoadingScreenSetupDone, new Action(this.OnLoadingScreenSetupDone));
		SceneManager.sceneLoaded -= this.UpdateSceneAudioSource;
		AssetBundleControl.OnSceneReady = (Action)Delegate.Remove(AssetBundleControl.OnSceneReady, new Action(this.OnSceneReady));
		PlatformPlayerPrefsHelper.saveSystemInitialized = (Action)Delegate.Remove(PlatformPlayerPrefsHelper.saveSystemInitialized, new Action(this.ReadAllPrefs));
		StanleyController.OnOutOfBounds = (Action)Delegate.Remove(StanleyController.OnOutOfBounds, new Action(this.OnOutOfBounds));
		PlatformAchievements.OnAchievementUnlockedFirstTime -= this.RecordAchievementDate;
		if (this.languageProfile != null)
		{
			IntConfigurable intConfigurable = this.languageProfile;
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnLanguageChanged));
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0001D774 File Offset: 0x0001B974
	private void OnReportPause()
	{
		if (this.menuCoroutine != null)
		{
			base.StopCoroutine(this.menuCoroutine);
		}
		this.menuCoroutine = this.OpenPauseMenu();
		base.StartCoroutine(this.menuCoroutine);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x0001D7A4 File Offset: 0x0001B9A4
	private void PauseActiveAudioSources()
	{
		this.pausedAudioSources.Clear();
		foreach (GameMaster.AudioSourceValues audioSourceValues in this.regularAudioSources)
		{
			if (audioSourceValues.source != null && audioSourceValues.source.isPlaying)
			{
				audioSourceValues.source.Pause();
				this.pausedAudioSources.Add(audioSourceValues.source);
			}
		}
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x0001D834 File Offset: 0x0001BA34
	private void UnpausePausedAudioSources()
	{
		foreach (AudioSource audioSource in this.pausedAudioSources)
		{
			if (audioSource != null)
			{
				audioSource.UnPause();
			}
		}
		this.pausedAudioSources.Clear();
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x0001D89C File Offset: 0x0001BA9C
	public StanleyController RespawnStanley()
	{
		if (!this.stanleyPrefab)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.stanleyPrefab);
		Object.DontDestroyOnLoad(gameObject);
		return gameObject.GetComponent<StanleyController>();
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x0001D8C4 File Offset: 0x0001BAC4
	private void PitchRegularAudioSources(float multiplier, bool divide = false)
	{
		for (int i = 0; i < this.regularAudioSources.Count; i++)
		{
			AudioSource source = this.regularAudioSources[i].source;
			if (source != null)
			{
				source.pitch = this.regularAudioSources[i].originalPitch * multiplier;
			}
		}
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x0001D91C File Offset: 0x0001BB1C
	private void Update()
	{
		if (!GameMaster.PAUSEMENUACTIVE && !this.loading)
		{
			Time.timeScale = GameMaster.timeMultiplier;
			this.GameDeltaTime = Time.deltaTime;
			this.GameSmoothedDeltaTime = Time.smoothDeltaTime;
			ChoreoMaster.GameSpeed = 1f;
			PhaseManager.GameSpeed = 1f;
		}
		else
		{
			Time.timeScale = 0f;
			this.GameDeltaTime = 0f;
			this.GameSmoothedDeltaTime = 0f;
			ChoreoMaster.GameSpeed = 0f;
			PhaseManager.GameSpeed = 0f;
		}
		this.GameTime += this.GameDeltaTime;
		GameMaster.RunTime += this.GameDeltaTime;
		foreach (GameMaster.MoviePlaybackContext moviePlaybackContext in this.AllMoviePlaybackContexts)
		{
			if (moviePlaybackContext.moviePlaying && moviePlaybackContext.canSkipmovie && !GameMaster.PAUSEMENUACTIVE && this.stanleyActions.UseAction.WasPressed)
			{
				if (moviePlaybackContext.moviePlayerSceneReference != null)
				{
					moviePlaybackContext.moviePlayerSceneReference.Skipped();
				}
				moviePlaybackContext.StopMovie();
			}
		}
		Vector2 vector = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
		bool flag = Input.GetAxisRaw("Mouse ScrollWheel") != 0f;
		this.MouseMoved = vector.sqrMagnitude > 0f || flag;
		this.InputTypeChangeUpdate();
		if (this.stanleyActions.MenuOpen.WasPressed && !GameMaster.PAUSEMENUACTIVE && !this.pauseMenuBlocked)
		{
			if (this.menuCoroutine != null)
			{
				base.StopCoroutine(this.menuCoroutine);
			}
			this.menuCoroutine = this.OpenPauseMenu();
			base.StartCoroutine(this.menuCoroutine);
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001DAEC File Offset: 0x0001BCEC
	public bool IsRumbleEnabled
	{
		get
		{
			return this.rumbleConfigurable.GetBooleanValue();
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001DAF9 File Offset: 0x0001BCF9
	public bool UsingAutowalk
	{
		get
		{
			return this.autowalkConfigurable.GetBooleanValue();
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001DB06 File Offset: 0x0001BD06
	public bool UsingSimplifiedControls
	{
		get
		{
			return this.simplifiedControlsConfigurable.GetBooleanValue();
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600053D RID: 1341 RVA: 0x0001DB13 File Offset: 0x0001BD13
	// (set) Token: 0x0600053E RID: 1342 RVA: 0x0001DB1B File Offset: 0x0001BD1B
	public GameMaster.InputDevice InputDeviceType { get; private set; }

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600053F RID: 1343 RVA: 0x0001DB24 File Offset: 0x0001BD24
	// (remove) Token: 0x06000540 RID: 1344 RVA: 0x0001DB5C File Offset: 0x0001BD5C
	public event Action<GameMaster.InputDevice> OnInputDeviceTypeChanged;

	// Token: 0x06000541 RID: 1345 RVA: 0x0001DB91 File Offset: 0x0001BD91
	public void InputTypeDetectionInit()
	{
		if (!GameMaster.IsStandalonePlatform)
		{
			this.RegisterInputDeviceTypeChange(GameMaster.ConsoleSpecificInputDevice);
			return;
		}
		if (InputManager.ActiveDevice != null)
		{
			this.RegisterControllerForPC();
			return;
		}
		this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.KeyboardAndMouse);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001DBBC File Offset: 0x0001BDBC
	private void RegisterControllerForPC()
	{
		string name = InputManager.ActiveDevice.Name;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 1170147956U)
		{
			if (num <= 934826445U)
			{
				if (num != 19423422U)
				{
					if (num != 934826445U)
					{
						goto IL_013D;
					}
					if (!(name == "PlayStation DualShock 2 Controller"))
					{
						goto IL_013D;
					}
					goto IL_012D;
				}
				else
				{
					if (!(name == "PlayStation 5 Controller"))
					{
						goto IL_013D;
					}
					goto IL_0135;
				}
			}
			else if (num != 1169023264U)
			{
				if (num != 1170147956U)
				{
					goto IL_013D;
				}
				if (!(name == "XBox One Controller"))
				{
					goto IL_013D;
				}
			}
			else
			{
				if (!(name == "PlayStation 3 Controller"))
				{
					goto IL_013D;
				}
				goto IL_012D;
			}
		}
		else if (num <= 1297164263U)
		{
			if (num != 1281539097U)
			{
				if (num != 1297164263U)
				{
					goto IL_013D;
				}
				if (!(name == "DualSense® wireless controller"))
				{
					goto IL_013D;
				}
				goto IL_0135;
			}
			else if (!(name == "Xbox 360 Controller"))
			{
				goto IL_013D;
			}
		}
		else if (num != 2418349549U)
		{
			if (num != 3137633415U)
			{
				if (num != 3684424571U)
				{
					goto IL_013D;
				}
				if (!(name == "PlayStation 4 Controller"))
				{
					goto IL_013D;
				}
				goto IL_012D;
			}
			else
			{
				if (!(name == "DUALSENSE® wireless controller"))
				{
					goto IL_013D;
				}
				goto IL_0135;
			}
		}
		else
		{
			if (!(name == "DUALSHOCK®4 wireless controller"))
			{
				goto IL_013D;
			}
			goto IL_012D;
		}
		this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.GamepadXBOXOneOrGeneric);
		return;
		IL_012D:
		this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.GamepadPS4);
		return;
		IL_0135:
		this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.GamepadPS5);
		return;
		IL_013D:
		Debug.LogWarning("Unknown Controller, reverting to default xbox: " + InputManager.ActiveDevice.Name);
		this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.GamepadXBOXOneOrGeneric);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001DD28 File Offset: 0x0001BF28
	public void InputTypeChangeUpdate()
	{
		if (InputManager.AnyKeyIsPressed || Singleton<GameMaster>.Instance.MouseMoved)
		{
			new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
			this.RegisterInputDeviceTypeChange(GameMaster.InputDevice.KeyboardAndMouse);
		}
		if (InputManager.ActiveDevice.AnyButtonWasPressed || Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) > 0.5f || Mathf.Abs(InputManager.ActiveDevice.LeftStickY.Value) > 0.5f || Mathf.Abs(InputManager.ActiveDevice.RightStickX.Value) > 0.5f || Mathf.Abs(InputManager.ActiveDevice.RightStickY.Value) > 0.5f || InputManager.ActiveDevice.LeftBumper.WasPressed || InputManager.ActiveDevice.RightBumper.WasPressed || InputManager.ActiveDevice.LeftTrigger.WasPressed || InputManager.ActiveDevice.RightTrigger.WasPressed)
		{
			if (GameMaster.IsStandalonePlatform)
			{
				this.RegisterControllerForPC();
				return;
			}
			this.RegisterInputDeviceTypeChange(GameMaster.ConsoleSpecificInputDevice);
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001DE44 File Offset: 0x0001C044
	private static bool IsStandalonePlatform
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			if (platform <= RuntimePlatform.WindowsEditor)
			{
				if (platform > RuntimePlatform.WindowsPlayer && platform != RuntimePlatform.WindowsEditor)
				{
					return false;
				}
			}
			else if (platform != RuntimePlatform.LinuxPlayer && platform != RuntimePlatform.LinuxEditor)
			{
				return false;
			}
			return true;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001DE74 File Offset: 0x0001C074
	private static GameMaster.InputDevice ConsoleSpecificInputDevice
	{
		get
		{
			RuntimePlatform platform = Application.platform;
			if (platform != RuntimePlatform.PS4)
			{
				if (platform != RuntimePlatform.XboxOne)
				{
					switch (platform)
					{
					case RuntimePlatform.Switch:
						return GameMaster.InputDevice.GamepadSwitch;
					case RuntimePlatform.PS5:
						return GameMaster.InputDevice.GamepadPS5;
					}
				}
				return GameMaster.InputDevice.GamepadXBOXOneOrGeneric;
			}
			return GameMaster.InputDevice.GamepadPS4;
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
	private void RegisterInputDeviceTypeChange(GameMaster.InputDevice newDeviceType)
	{
		this.usingControllerConfigurable.Init();
		this.usingControllerConfigurable.SetValue(newDeviceType > GameMaster.InputDevice.KeyboardAndMouse);
		this.usingControllerConfigurable.SaveToDiskAll();
		if (this.InputDeviceType != newDeviceType)
		{
			this.InputDeviceType = newDeviceType;
			Action<GameMaster.InputDevice> onInputDeviceTypeChanged = this.OnInputDeviceTypeChanged;
			if (onInputDeviceTypeChanged == null)
			{
				return;
			}
			onInputDeviceTypeChanged(newDeviceType);
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x0001DF13 File Offset: 0x0001C113
	private IEnumerator OpenPauseMenu()
	{
		if (this.loading || GameMaster.ONMAINMENUORSETTINGS)
		{
			yield break;
		}
		GameMaster.PAUSEMENUACTIVE = true;
		if (StanleyController.Instance.gameObject.activeInHierarchy)
		{
			this.activateStanleyOnResume = true;
			StanleyController.Instance.FreezeMotionAndView();
		}
		else
		{
			this.activateStanleyOnResume = false;
		}
		if (GameMaster.OnPause != null)
		{
			GameMaster.OnPause();
		}
		this.PauseActiveAudioSources();
		foreach (GameMaster.MoviePlaybackContext moviePlaybackContext in this.AllMoviePlaybackContexts)
		{
			if (moviePlaybackContext.moviePlaying)
			{
				if (moviePlaybackContext.PlatformMoviePlayer != null)
				{
					moviePlaybackContext.PlatformMoviePlayer.Pause();
				}
				moviePlaybackContext.movieToResume = true;
			}
		}
		this.menuManager.CallPauseMenu();
		Canvas[] array = this.captionCanvases;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		yield break;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0001DF24 File Offset: 0x0001C124
	public void ClosePauseMenu(bool resumeGame = true)
	{
		if (resumeGame && GameMaster.OnResume != null)
		{
			GameMaster.OnResume();
		}
		this.UnpausePausedAudioSources();
		foreach (GameMaster.MoviePlaybackContext moviePlaybackContext in this.AllMoviePlaybackContexts)
		{
			if (resumeGame && moviePlaybackContext.movieToResume)
			{
				if (moviePlaybackContext.PlatformMoviePlayer != null)
				{
					moviePlaybackContext.PlatformMoviePlayer.Unpause();
				}
				moviePlaybackContext.movieToResume = false;
			}
		}
		GameMaster.PAUSEMENUACTIVE = false;
		Canvas[] array = this.captionCanvases;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		if (resumeGame && this.activateStanleyOnResume)
		{
			StanleyController.Instance.UnfreezeMotionAndView();
		}
		GameMaster.CursorLockState = CursorLockMode.Locked;
		GameMaster.CursorVisible = false;
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001DFF0 File Offset: 0x0001C1F0
	// (set) Token: 0x0600054A RID: 1354 RVA: 0x0001DFF7 File Offset: 0x0001C1F7
	public static bool CursorVisible
	{
		get
		{
			return Cursor.visible;
		}
		set
		{
			Cursor.visible = value;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600054B RID: 1355 RVA: 0x0001DFFF File Offset: 0x0001C1FF
	// (set) Token: 0x0600054C RID: 1356 RVA: 0x0001E006 File Offset: 0x0001C206
	public static CursorLockMode CursorLockState
	{
		get
		{
			return Cursor.lockState;
		}
		set
		{
			Cursor.lockState = value;
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001E010 File Offset: 0x0001C210
	public GameMaster.MoviePlaybackContext StartMovie(bool skip, MoviePlayer player, string cameraName, string moviePath, bool isFullscreenMovie)
	{
		GameMaster.MoviePlaybackContext moviePlaybackContext;
		if (isFullscreenMovie)
		{
			if (this.fullscreenPlaybackContext == null)
			{
				this.fullscreenPlaybackContext = new GameMaster.MoviePlaybackContext(new Func<IMoviePlayer>(this.GetNewPlatformMoviePlayer));
			}
			moviePlaybackContext = this.fullscreenPlaybackContext;
		}
		else
		{
			if (!this.inGamePlaybackContexts.TryGetValue(cameraName, out moviePlaybackContext))
			{
				this.inGamePlaybackContexts[cameraName] = new GameMaster.MoviePlaybackContext(new Func<IMoviePlayer>(this.GetNewPlatformMoviePlayer));
			}
			moviePlaybackContext = this.inGamePlaybackContexts[cameraName];
		}
		if (moviePlaybackContext == null)
		{
			return null;
		}
		moviePlaybackContext.StartMovie(skip, player, cameraName, moviePath);
		return moviePlaybackContext;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001E094 File Offset: 0x0001C294
	public void CancelFade()
	{
		this.fadeHold = false;
		this.fading = false;
		this.fadeImage.enabled = false;
		this.fadeCanvas.enabled = false;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0001E0BC File Offset: 0x0001C2BC
	public IEnumerator DelayedCancelFade()
	{
		yield return new WaitForSeconds(0.5f);
		if (this.fadeCoroutine == null)
		{
			this.CancelFade();
		}
		yield break;
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001E0CC File Offset: 0x0001C2CC
	public void BeginFade(Color color, float inDuration, float holdDuration, bool fadeFrom, bool stayOut)
	{
		this.fadeColor = color;
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeImage.enabled = true;
		this.fadeCanvas.enabled = true;
		this.fadeCoroutine = this.Fade(inDuration, holdDuration, fadeFrom, stayOut);
		base.StartCoroutine(this.fadeCoroutine);
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x0001E12B File Offset: 0x0001C32B
	private IEnumerator Fade(float inDuration, float holdDuration, bool fadeFrom, bool stayOut)
	{
		this.fadeHold = false;
		this.fading = true;
		TimePair timer = new TimePair(inDuration);
		float fadeStartAlpha = 0f;
		float fadeEndAlpha = this.fadeColor.a;
		if (fadeFrom)
		{
			fadeStartAlpha = fadeEndAlpha;
			fadeEndAlpha = 0f;
		}
		while (timer.keepWaiting)
		{
			float num = timer.InverseLerp();
			this.fadeColor.a = Mathf.Lerp(fadeStartAlpha, fadeEndAlpha, num);
			this.fadeImage.color = this.fadeColor;
			yield return new WaitForEndOfFrame();
		}
		this.fadeColor.a = fadeEndAlpha;
		this.fadeImage.color = this.fadeColor;
		if (holdDuration > 0f)
		{
			yield return new WaitForGameSeconds(holdDuration);
		}
		if (stayOut)
		{
			this.fadeHold = true;
		}
		else
		{
			this.fadeColor.a = 0f;
		}
		this.fading = false;
		if (!this.fadeHold)
		{
			this.fadeCanvas.enabled = false;
			this.fadeImage.enabled = false;
		}
		this.fadeCoroutine = null;
		yield break;
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0001E157 File Offset: 0x0001C357
	private bool isOnMainMenuOrSettingScene(string sceneName)
	{
		sceneName = sceneName.ToLower();
		return sceneName.Contains("settings") || sceneName.Contains("menu");
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x0001E17C File Offset: 0x0001C37C
	private void SceneLoaded(Scene scene, LoadSceneMode loadMode)
	{
		string text = scene.name.ToLower();
		Resources.UnloadUnusedAssets();
		GameMaster.ONMAINMENUORSETTINGS = this.isOnMainMenuOrSettingScene(text);
		StanleyController.Instance.gameObject.SetActive(!GameMaster.ONMAINMENUORSETTINGS);
		if (!this.isOnMainMenuOrSettingScene(text))
		{
			StanleyController.Instance.NewMapReset();
			PlatformRichPresence.SetPresence((PresenceID)Random.Range(0, Enum.GetValues(typeof(PresenceID)).Length));
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.ClosePauseMenu(false);
		}
		if (!(text == "loadingscene_ud_master"))
		{
			if (!(text == "menu_ud_master"))
			{
				if (!(text == "map1_ud_master"))
				{
					if (!(text == "map2_ud_master"))
					{
						if (!(text == "seriousroom"))
						{
							return;
						}
						GameObject gameObject = GameObject.Find("seriouspass" + this.seriousPassCounter);
						if (gameObject)
						{
							gameObject.GetComponent<HammerEntity>().Input_Enable();
							return;
						}
						return;
					}
					else
					{
						GameObject gameObject2 = GameObject.Find("countpass" + this.countPassCounter);
						if (gameObject2)
						{
							gameObject2.GetComponent<HammerEntity>().Input_Enable();
						}
						if (!this.beginAgainMap2)
						{
							this.beginAgainMap2 = true;
							return;
						}
						GameObject gameObject3 = GameObject.Find("beginagainmap2");
						if (gameObject3)
						{
							gameObject3.GetComponent<HammerEntity>().Input_Enable();
							return;
						}
						return;
					}
				}
				else
				{
					GameMaster.RunTime = 0f;
					StanleyController.Instance.currentCam.transform.localPosition = Vector3.zero;
					StanleyController.Instance.currentCam.transform.localRotation = Quaternion.identity;
					StanleyController.Instance.currentCam.transform.localScale = Vector3.one;
					StanleyController.Instance.Bucket.SetBucket(false, false, true, true);
					StanleyController.Instance.Bucket.SetAnimationSpeedImmediate(1f);
					string text2 = "NumberOfTimesRestarted";
					if (PlatformPlayerPrefs.HasKey(text2))
					{
						int num = PlatformPlayerPrefs.GetInt(text2);
						num = Mathf.Clamp(num + 1, 0, 99);
						PlatformPlayerPrefs.SetInt(text2, num);
						if (num >= 3)
						{
							string text3 = "NewContentAvailable";
							if (!PlatformPlayerPrefs.HasKey(text3))
							{
								PlatformPlayerPrefs.SetInt(text3, 1);
							}
						}
					}
					else
					{
						PlatformPlayerPrefs.SetInt(text2, 1);
					}
					if (this.beginAgainMap1)
					{
						GameObject gameObject4 = GameObject.Find("beginagain");
						if (gameObject4)
						{
							gameObject4.GetComponent<HammerEntity>().Input_Enable();
						}
						GameObject gameObject5 = GameObject.Find("zaxis");
						if (gameObject5)
						{
							gameObject5.GetComponent<HammerEntity>().Input_Enable();
						}
					}
					else
					{
						this.beginAgainMap1 = true;
					}
					GameObject gameObject6 = GameObject.Find("loungeenter" + this.loungePassCounter);
					if (gameObject6)
					{
						gameObject6.GetComponent<HammerEntity>().Input_Enable();
					}
					GameObject gameObject7 = GameObject.Find("broompass" + this.broomPassCounter);
					if (gameObject7)
					{
						gameObject7.GetComponent<HammerEntity>().Input_Enable();
					}
					string text4 = "";
					if (this.bossPassCounter > 1)
					{
						text4 = "b";
					}
					GameObject gameObject8 = GameObject.Find("bossenter" + this.bossPassCounter + text4);
					if (gameObject8)
					{
						gameObject8.GetComponent<HammerEntity>().Input_Enable();
					}
					GameObject gameObject9 = GameObject.Find("bossskip" + this.bossSkipCounter);
					if (gameObject9)
					{
						gameObject9.GetComponent<HammerEntity>().Input_Enable();
					}
					GameObject gameObject10 = GameObject.Find("buthevL" + this.buttonPassCounter + "B");
					if (gameObject10)
					{
						gameObject10.GetComponent<FuncButton>().Input_Unlock();
					}
					GameObject gameObject11 = GameObject.Find("buthevL" + this.buttonPassCounter + "S1");
					if (gameObject11)
					{
						gameObject11.GetComponent<HammerEntity>().Input_Enable();
					}
					if (!this.boxesNextTime)
					{
						return;
					}
					GameObject gameObject12 = GameObject.Find("boxaxis");
					if (gameObject12)
					{
						gameObject12.GetComponent<HammerEntity>().Input_Enable();
						this.boxesNextTime = false;
						return;
					}
					return;
				}
			}
		}
		else
		{
			using (IEnumerator<GameMaster.MoviePlaybackContext> enumerator = this.AllMoviePlaybackContexts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameMaster.MoviePlaybackContext moviePlaybackContext = enumerator.Current;
					moviePlaybackContext.StopMovie();
				}
				return;
			}
		}
		this.beginAgainMap1 = false;
		this.CancelFade();
		this.menuManager.CallMainMenu();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x0001E5F0 File Offset: 0x0001C7F0
	private void PrepareLoadingLevel(string mapName)
	{
		this.loading = true;
		AudioListener.volume = 0f;
		this.StopSound();
		this.Crosshair(false);
		StanleyController.Instance.Deparent(true);
		StanleyController.Instance.ResetClientCommandFreezes();
		StanleyController.Instance.FreezeMotionAndView();
		Object.DontDestroyOnLoad(StanleyController.Instance.gameObject);
		this.sceneComingFrom = SceneManager.GetActiveScene();
		this.SetLoadingStyle(SceneManager.GetActiveScene().name, mapName);
		this.Blackout();
		Action onPrepareLoadingLevel = GameMaster.OnPrepareLoadingLevel;
		if (onPrepareLoadingLevel != null)
		{
			onPrepareLoadingLevel();
		}
		Singleton<ChoreoMaster>.Instance.DropAll();
		this.pauseMenuBlocked = false;
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0001E68F File Offset: 0x0001C88F
	public bool ChangeLevel(string mapName, bool waitMinLoadTime = true)
	{
		if (this.loading)
		{
			return false;
		}
		if (AssetBundleControl.ChangeScene(mapName, "LoadingScene_UD_Master", this))
		{
			this.PrepareLoadingLevel(mapName);
			return true;
		}
		return false;
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0001E6B3 File Offset: 0x0001C8B3
	public void OnSceneReady()
	{
		this.loading = false;
		AudioListener.volume = Singleton<GameMaster>.Instance.masterVolume;
		base.StartCoroutine(this.DelayedCancelFade());
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x0001E6D8 File Offset: 0x0001C8D8
	public void LoungePass()
	{
		if (this.loungePassCounter < 8)
		{
			this.loungePassCounter++;
		}
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x0001E6F1 File Offset: 0x0001C8F1
	public void BroomPass()
	{
		if (this.broomPassCounter < 3)
		{
			this.broomPassCounter++;
		}
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0001E70A File Offset: 0x0001C90A
	public void BossPass()
	{
		if (this.bossPassCounter < 4)
		{
			this.bossPassCounter++;
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001E723 File Offset: 0x0001C923
	public void BossSkip()
	{
		if (this.bossSkipCounter < 3)
		{
			this.bossSkipCounter++;
		}
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001E73C File Offset: 0x0001C93C
	public void CountPass()
	{
		if (this.countPassCounter < 2)
		{
			this.countPassCounter++;
		}
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0001E755 File Offset: 0x0001C955
	public void ButtonPass()
	{
		if (this.buttonPassCounter < 5)
		{
			this.buttonPassCounter++;
		}
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001E76E File Offset: 0x0001C96E
	public void SeriousPass()
	{
		if (this.seriousPassCounter < 4)
		{
			this.seriousPassCounter++;
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001E787 File Offset: 0x0001C987
	[ContextMenu("SpawnPrefab")]
	public void SpawnPrefabOnStanley()
	{
		Object.Instantiate<GameObject>(this.spawnPrefab).transform.position = StanleyController.Instance.transform.position;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001E7AD File Offset: 0x0001C9AD
	public void Boxes()
	{
		this.boxesNextTime = true;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001E7B6 File Offset: 0x0001C9B6
	public void BarkModeOn()
	{
		this.barking = true;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001E7BF File Offset: 0x0001C9BF
	public void BarkModeOff()
	{
		this.barking = false;
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0001E7C8 File Offset: 0x0001C9C8
	public void TSP_Reload(int val)
	{
		this.TSP_Reload();
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0001E7D0 File Offset: 0x0001C9D0
	private void UpdateSceneAudioSource(Scene scene, LoadSceneMode mode)
	{
		this.UpdateSceneAudioSource();
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
	private void UpdateSceneAudioSource()
	{
		this.sceneAudioSources = Object.FindObjectsOfType<AudioSource>();
		this.regularAudioSources.Clear();
		for (int i = 0; i < this.sceneAudioSources.Length; i++)
		{
			AudioSource audioSource = this.sceneAudioSources[i];
			if (this.IsRegularAudioSource(audioSource))
			{
				GameMaster.AudioSourceValues audioSourceValues = new GameMaster.AudioSourceValues(audioSource, audioSource.pitch);
				this.regularAudioSources.Add(audioSourceValues);
			}
		}
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0001E83C File Offset: 0x0001CA3C
	private bool IsRegularAudioSource(AudioSource source)
	{
		Object component = source.gameObject.GetComponent<AmbientGeneric>();
		Soundscape component2 = source.gameObject.GetComponent<Soundscape>();
		ChoreoMaster component3 = source.gameObject.GetComponent<ChoreoMaster>();
		return component == null && component2 == null && component3 == null;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0001E886 File Offset: 0x0001CA86
	public void TSP_Reload()
	{
		this.TSP_Reload(GameMaster.TSP_Reload_Behaviour.Standard);
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001E890 File Offset: 0x0001CA90
	public void TSP_Reload(GameMaster.TSP_Reload_Behaviour behaviour)
	{
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
			this.fadeCoroutine = null;
		}
		this.Blackout();
		this.eyelids.Reset();
		this.ClosePauseMenu(false);
		StanleyController.Instance.Bucket.SetBucket(false, true, true, true);
		if (behaviour == GameMaster.TSP_Reload_Behaviour.Epilogue)
		{
			this.ChangeLevel("MemoryzonePartThree_UD_MASTER", true);
			return;
		}
		if (FigleyOverlayController.Instance.FiglysFound >= 5 && !this.memoryZoneTwoComplete.GetBooleanValue())
		{
			this.ChangeLevel("MemoryzonePartTwo_UD_MASTER", true);
			return;
		}
		if (this.tspSequelNumber.GetIntValue() == 8)
		{
			this.ChangeLevel("eight_UD_MASTER", true);
			return;
		}
		this.ChangeLevel("map1_UD_MASTER", true);
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001E948 File Offset: 0x0001CB48
	private void Blackout()
	{
		switch (GameMaster.LoadingScreenStyle)
		{
		case LoadingManager.LoadScreenStyle.Standard:
		case LoadingManager.LoadScreenStyle.Message:
		case LoadingManager.LoadScreenStyle.Black:
			this.fadeImage.color = Color.black;
			break;
		case LoadingManager.LoadScreenStyle.Blue:
			this.fadeImage.color = new Color(0f, 0.1098039f, 0.4705883f, 1f);
			break;
		case LoadingManager.LoadScreenStyle.DoneyWithTheFunny:
		case LoadingManager.LoadScreenStyle.White:
			this.fadeImage.color = Color.white;
			break;
		}
		this.fadeImage.enabled = true;
		this.fadeCanvas.enabled = true;
		if (this.fadeCoroutine != null)
		{
			base.StopCoroutine(this.fadeCoroutine);
		}
		this.fadeCoroutine = null;
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0001E9F6 File Offset: 0x0001CBF6
	public void TSP_MainMenu()
	{
		this.ChangeLevel("menu_UD_Master", true);
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0001EA08 File Offset: 0x0001CC08
	public void StopSound()
	{
		AudioSource[] array = Object.FindObjectsOfType<AudioSource>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
			Singleton<ChoreoMaster>.Instance.DropAll();
		}
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0001EA3B File Offset: 0x0001CC3B
	public void Crosshair(bool on)
	{
		if (this.crosshair == null)
		{
			return;
		}
		this.crosshair.GetComponentInChildren<Animator>().SetBool("Show", on);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0001EA62 File Offset: 0x0001CC62
	private IEnumerator CrosshairFadeRoutine()
	{
		this.crosshair.GetComponentInChildren<Animator>().SetTrigger("FadeOut");
		yield return new WaitForGameSeconds(1f);
		this.crosshair.SetActive(false);
		this.crosshairFadeCoroutine = null;
		yield break;
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00005444 File Offset: 0x00003644
	public void StartCredits()
	{
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0001EA71 File Offset: 0x0001CC71
	public void EyelidAnimate(EyelidDir dir)
	{
		if (dir == EyelidDir.Close)
		{
			this.eyelids.StartClose();
			return;
		}
		this.eyelids.StartOpen();
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0001EA90 File Offset: 0x0001CC90
	public void StartApartment(string gameObjectName)
	{
		GameObject gameObject = GameObject.Find(gameObjectName);
		if (gameObject)
		{
			InstructorHint component = gameObject.GetComponent<InstructorHint>();
			base.StartCoroutine(this.ApartmentEnding(component));
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001EAC1 File Offset: 0x0001CCC1
	private IEnumerator ApartmentEnding(InstructorHint hint)
	{
		int num;
		for (int i = 0; i < hint.apartmentEndingRelays.Length; i = num + 1)
		{
			LogicRelay relay = hint.apartmentEndingRelays[i];
			while (!relay.isEnabled)
			{
				yield return null;
			}
			if (i == hint.apartmentEndingRelays.Length - 1 && hint.noInputOnLastRelay)
			{
				hint.ShowHint(i, false);
				yield return new WaitForGameSeconds(7f);
				hint.HideHint();
			}
			else
			{
				hint.ShowHint(i, true);
				while (hint.waiting)
				{
					yield return null;
				}
				yield return new WaitForGameSeconds(0.5f);
				relay.Input_Trigger();
			}
			relay = null;
			num = i;
		}
		yield break;
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
	private void SetLoadingStyle(string from, string to)
	{
		to = to.Replace("_UD_MASTER", "").ToLower();
		from = from.Replace("_UD_MASTER", "").ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(to);
		if (num <= 1551104593U)
		{
			if (num <= 847392620U)
			{
				if (num <= 375251406U)
				{
					if (num != 31788274U)
					{
						if (num != 375251406U)
						{
							goto IL_02D6;
						}
						if (!(to == "zending"))
						{
							goto IL_02D6;
						}
					}
					else
					{
						if (!(to == "map_one"))
						{
							goto IL_02D6;
						}
						goto IL_02AC;
					}
				}
				else if (num != 654019781U)
				{
					if (num != 847392620U)
					{
						goto IL_02D6;
					}
					if (!(to == "incorrect"))
					{
						goto IL_02D6;
					}
					if (BucketController.HASBUCKET)
					{
						this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.DoneyWithTheFunny, "");
						goto IL_02E2;
					}
					this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Blue, "");
					goto IL_02E2;
				}
				else if (!(to == "redstair"))
				{
					goto IL_02D6;
				}
			}
			else if (num <= 1182879452U)
			{
				if (num != 1032425025U)
				{
					if (num != 1182879452U)
					{
						goto IL_02D6;
					}
					if (!(to == "thefirstmap"))
					{
						goto IL_02D6;
					}
					goto IL_02AC;
				}
				else
				{
					if (!(to == "apartment_ending"))
					{
						goto IL_02D6;
					}
					this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.White, "");
					goto IL_02E2;
				}
			}
			else if (num != 1427181539U)
			{
				if (num != 1551104593U)
				{
					goto IL_02D6;
				}
				if (!(to == "freedom"))
				{
					goto IL_02D6;
				}
			}
			else
			{
				if (!(to == "babygame"))
				{
					goto IL_02D6;
				}
				goto IL_02D6;
			}
		}
		else if (num <= 2581912890U)
		{
			if (num <= 2368333904U)
			{
				if (num != 1745255176U)
				{
					if (num != 2368333904U)
					{
						goto IL_02D6;
					}
					if (!(to == "theonlymap"))
					{
						goto IL_02D6;
					}
					goto IL_02AC;
				}
				else if (!(to == "settings"))
				{
					goto IL_02D6;
				}
			}
			else if (num != 2424245049U)
			{
				if (num != 2581912890U)
				{
					goto IL_02D6;
				}
				if (!(to == "menu"))
				{
					goto IL_02D6;
				}
			}
			else
			{
				if (!(to == "map2"))
				{
					goto IL_02D6;
				}
				this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Message, "LOADING");
				goto IL_02E2;
			}
		}
		else if (num <= 3365091272U)
		{
			if (num != 3274577880U)
			{
				if (num != 3365091272U)
				{
					goto IL_02D6;
				}
				if (!(to == "map_two"))
				{
					goto IL_02D6;
				}
				goto IL_02AC;
			}
			else
			{
				if (!(to == "map_death"))
				{
					goto IL_02D6;
				}
				goto IL_02D6;
			}
		}
		else if (num != 3751997361U)
		{
			if (num != 3982288071U)
			{
				goto IL_02D6;
			}
			if (!(to == "buttonworld"))
			{
				goto IL_02D6;
			}
		}
		else
		{
			if (!(to == "map"))
			{
				goto IL_02D6;
			}
			goto IL_02AC;
		}
		this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Black, "");
		goto IL_02E2;
		IL_02AC:
		this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Message, "NEVER THE END IS");
		goto IL_02E2;
		IL_02D6:
		this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Message, "NEVER THE END IS");
		IL_02E2:
		if (from == "map")
		{
			this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Message, "NEVER THE END IS");
		}
		if (from == "settings")
		{
			this.UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle.Minimal, "");
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0001EDF1 File Offset: 0x0001CFF1
	private void UpdateLoadingStyleChange(LoadingManager.LoadScreenStyle newStyle, string newLoadingMessage = "")
	{
		GameMaster.LoadingScreenStyle = newStyle;
		if (newLoadingMessage != "")
		{
			GameMaster.LoadingScreenMessage = newLoadingMessage;
		}
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0001EE0C File Offset: 0x0001D00C
	public void ReadAllPrefs()
	{
		this.SetMasterVolume(PlatformPlayerPrefs.GetFloat("PREFSKEY_MASTER_VOLUME", 1f));
		this.SetMusicVolume(PlatformPlayerPrefs.GetFloat("PREFSKEY_MUSIC_VOLUME", 1f));
		this.SetCaptionsActive(PlatformPlayerPrefs.GetInt("PREFSKEY_CAPTIONING", 1) == 1);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001EE4C File Offset: 0x0001D04C
	public void WriteAllPrefs()
	{
		PlatformPlayerPrefs.SetFloat("PREFSKEY_MASTER_VOLUME", this.masterVolume);
		PlatformPlayerPrefs.SetFloat("PREFSKEY_MUSIC_VOLUME", this.musicVolume);
		PlatformPlayerPrefs.SetInt("PREFSKEY_CAPTIONING", this.closedCaptionsOn ? 1 : 0);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001EE84 File Offset: 0x0001D084
	public void SetMasterVolume(float val)
	{
		this.masterVolume = val;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001EE8D File Offset: 0x0001D08D
	public void SetMusicVolume(float val)
	{
		this.musicVolume = val;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001EE96 File Offset: 0x0001D096
	public void SetCaptionsActive(bool state)
	{
		this.closedCaptionsOn = state;
	}

	// Token: 0x040004DA RID: 1242
	public static LoadingManager.LoadScreenStyle LoadingScreenStyle = LoadingManager.LoadScreenStyle.Minimal;

	// Token: 0x040004DB RID: 1243
	public static Action<float> OnUpdateLoadProgress;

	// Token: 0x040004DC RID: 1244
	public static string LoadingScreenMessage = "LOADING";

	// Token: 0x040004DD RID: 1245
	public GameObject stanleyPrefab;

	// Token: 0x040004DE RID: 1246
	public GameObject choreoPrefab;

	// Token: 0x040004DF RID: 1247
	public GameObject crosshairPrefab;

	// Token: 0x040004E0 RID: 1248
	public GameObject eyelidsPrefab;

	// Token: 0x040004E1 RID: 1249
	public GameObject menuPrefab;

	// Token: 0x040004E2 RID: 1250
	public GameObject fadeCanvasPrefab;

	// Token: 0x040004E3 RID: 1251
	public GameObject ReporterPrefab;

	// Token: 0x040004E4 RID: 1252
	public GameObject figleyOverlayPrefab;

	// Token: 0x040004E5 RID: 1253
	public GameObject platformSettingsPrefab;

	// Token: 0x040004E6 RID: 1254
	public GameObject cursorControllerPrefab;

	// Token: 0x040004E7 RID: 1255
	public GameObject resolutionControllerPrefab;

	// Token: 0x040004E8 RID: 1256
	public GameObject motionControllerPrefab;

	// Token: 0x040004E9 RID: 1257
	public StringConfigurable keyBindings;

	// Token: 0x040004EA RID: 1258
	[Header("Global/Scriptable Object -Data")]
	public SparkEffectData sparkEffectData;

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private BooleanConfigurable[] achievementConfigurables;

	// Token: 0x040004EC RID: 1260
	[SerializeField]
	private AchievementsData achievementsData;

	// Token: 0x040004ED RID: 1261
	private GameObject crosshair;

	// Token: 0x040004EE RID: 1262
	private GameObject eyelidsObj;

	// Token: 0x040004EF RID: 1263
	private Eyelids eyelids;

	// Token: 0x040004F0 RID: 1264
	private GameObject gameMenu;

	// Token: 0x040004F1 RID: 1265
	private MainMenu menuManager;

	// Token: 0x040004F2 RID: 1266
	private RawImage fadeImage;

	// Token: 0x040004F3 RID: 1267
	private Camera fadeCamera;

	// Token: 0x040004F4 RID: 1268
	private IEnumerator menuCoroutine;

	// Token: 0x040004F5 RID: 1269
	private AssetBundle levelAssetBundle;

	// Token: 0x040004F6 RID: 1270
	public static bool systemRequestPauseMenu = false;

	// Token: 0x040004F7 RID: 1271
	private GameMaster.MoviePlaybackContext fullscreenPlaybackContext;

	// Token: 0x040004F8 RID: 1272
	private Dictionary<string, GameMaster.MoviePlaybackContext> inGamePlaybackContexts = new Dictionary<string, GameMaster.MoviePlaybackContext>();

	// Token: 0x040004F9 RID: 1273
	private bool fading;

	// Token: 0x040004FA RID: 1274
	private Color fadeColor = Color.black;

	// Token: 0x040004FB RID: 1275
	private bool fadeHold;

	// Token: 0x040004FC RID: 1276
	private IEnumerator fadeCoroutine;

	// Token: 0x040004FD RID: 1277
	private Canvas fadeCanvas;

	// Token: 0x040004FE RID: 1278
	private Canvas[] captionCanvases;

	// Token: 0x040004FF RID: 1279
	private Canvas eyelidCanvas;

	// Token: 0x04000500 RID: 1280
	[Header("Time")]
	public static float timeMultiplier = 1f;

	// Token: 0x04000501 RID: 1281
	private float fastForwardMultiplier = 4f;

	// Token: 0x04000502 RID: 1282
	private float slowDownMultiplier = 0.1f;

	// Token: 0x04000507 RID: 1287
	[HideInInspector]
	public static bool PAUSEMENUACTIVE;

	// Token: 0x04000508 RID: 1288
	[HideInInspector]
	public bool pauseMenuBlocked;

	// Token: 0x04000509 RID: 1289
	private bool activateStanleyOnResume;

	// Token: 0x0400050D RID: 1293
	[HideInInspector]
	public StanleyActions stanleyActions;

	// Token: 0x0400050F RID: 1295
	public BooleanConfigurable DefaultConfigConfigurable;

	// Token: 0x04000510 RID: 1296
	[Header("Prefab to Spawn via Command")]
	public GameObject spawnPrefab;

	// Token: 0x04000511 RID: 1297
	[Header("Language Selection")]
	public IntConfigurable languageProfile;

	// Token: 0x04000512 RID: 1298
	public LanguageProfileData languageProfileData;

	// Token: 0x04000513 RID: 1299
	[Header("Memeory Zone Configurables")]
	public BooleanConfigurable memoryZoneOneComplete;

	// Token: 0x04000514 RID: 1300
	public BooleanConfigurable memoryZoneTwoComplete;

	// Token: 0x04000515 RID: 1301
	public IntConfigurable tspSequelNumber;

	// Token: 0x04000516 RID: 1302
	[Header("Configurables Init")]
	public ResetableConfigurablesList resetableConfigurablesList;

	// Token: 0x04000517 RID: 1303
	private const string PREFSKEY_MASTER_VOLUME = "PREFSKEY_MASTER_VOLUME";

	// Token: 0x04000518 RID: 1304
	private const string PREFSKEY_MUSIC_VOLUME = "PREFSKEY_MUSIC_VOLUME";

	// Token: 0x04000519 RID: 1305
	private const string PREFSKEY_CAPTIONING = "PREFSKEY_CAPTIONING";

	// Token: 0x0400051A RID: 1306
	[SerializeField]
	private AudioSource[] sceneAudioSources = new AudioSource[0];

	// Token: 0x0400051B RID: 1307
	[SerializeField]
	private List<AudioSource> pausedAudioSources = new List<AudioSource>();

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private List<GameMaster.AudioSourceValues> regularAudioSources = new List<GameMaster.AudioSourceValues>();

	// Token: 0x0400051D RID: 1309
	private float _masterVolume = 1f;

	// Token: 0x0400051E RID: 1310
	private float _musicVolume = 1f;

	// Token: 0x0400051F RID: 1311
	private bool _closedCaptionsOn = true;

	// Token: 0x04000520 RID: 1312
	[Header("Default Config Configurable")]
	public BooleanConfigurable useDefaultPerformanceConfiguration;

	// Token: 0x04000521 RID: 1313
	public static bool ONMAINMENUORSETTINGS = true;

	// Token: 0x04000522 RID: 1314
	private bool beginAgainMap1;

	// Token: 0x04000523 RID: 1315
	private bool beginAgainMap2;

	// Token: 0x04000524 RID: 1316
	private int loungePassCounter = 1;

	// Token: 0x04000525 RID: 1317
	private int broomPassCounter = 1;

	// Token: 0x04000526 RID: 1318
	private int bossPassCounter = 1;

	// Token: 0x04000527 RID: 1319
	private int bossSkipCounter = 1;

	// Token: 0x04000528 RID: 1320
	private int countPassCounter = 1;

	// Token: 0x04000529 RID: 1321
	private int buttonPassCounter = 1;

	// Token: 0x0400052A RID: 1322
	private int seriousPassCounter = 1;

	// Token: 0x0400052B RID: 1323
	private bool boxesNextTime;

	// Token: 0x0400052D RID: 1325
	private Scene sceneComingFrom;

	// Token: 0x0400052E RID: 1326
	private bool loading;

	// Token: 0x0400052F RID: 1327
	private float _loadProgress;

	// Token: 0x04000531 RID: 1329
	[Header("Controller Rumble")]
	public BooleanConfigurable rumbleConfigurable;

	// Token: 0x04000532 RID: 1330
	[Header("Input and Keybinding")]
	public BooleanConfigurable autowalkConfigurable;

	// Token: 0x04000533 RID: 1331
	public BooleanConfigurable simplifiedControlsConfigurable;

	// Token: 0x04000534 RID: 1332
	public BooleanConfigurable usingControllerConfigurable;

	// Token: 0x04000537 RID: 1335
	private const string RegistrationMark = "®";

	// Token: 0x04000538 RID: 1336
	private Coroutine crosshairFadeCoroutine;

	// Token: 0x020003AD RID: 941
	public class MoviePlaybackContext
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x00077B7C File Offset: 0x00075D7C
		public bool MoviePlaying
		{
			get
			{
				return this.moviePlaying;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00077B84 File Offset: 0x00075D84
		// (set) Token: 0x060016CA RID: 5834 RVA: 0x00077B8C File Offset: 0x00075D8C
		public GameObject CameraGameObject { get; private set; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060016CB RID: 5835 RVA: 0x00077B95 File Offset: 0x00075D95
		private Camera Camera
		{
			get
			{
				if (!(this.CameraGameObject != null))
				{
					return null;
				}
				return this.CameraGameObject.GetComponent<Camera>();
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060016CC RID: 5836 RVA: 0x00077BB2 File Offset: 0x00075DB2
		// (set) Token: 0x060016CD RID: 5837 RVA: 0x00077BCF File Offset: 0x00075DCF
		public bool CameraEnabled
		{
			get
			{
				return this.Camera != null && this.Camera.enabled;
			}
			set
			{
				if (this.Camera != null)
				{
					this.Camera.enabled = value;
				}
			}
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00077BEB File Offset: 0x00075DEB
		public MoviePlaybackContext(Func<IMoviePlayer> platformMoviePlayerGenerator)
		{
			this.PlatformMoviePlayer = platformMoviePlayerGenerator();
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00077C00 File Offset: 0x00075E00
		public void StartMovie(bool skip, MoviePlayer player, string cameraName, string moviePath)
		{
			this.moviePlaying = true;
			this.moviePlayerSceneReference = player;
			this.canSkipmovie = skip;
			if (this.PlatformMoviePlayer != null)
			{
				this.PlatformMoviePlayer.OnMovieLoopPointReached -= this.OnMovieLoopPointReached;
				this.PlatformMoviePlayer.OnMovieLoopPointReached += this.OnMovieLoopPointReached;
				this.CameraGameObject = this.PlatformMoviePlayer.Play(cameraName, moviePath);
			}
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00077C6C File Offset: 0x00075E6C
		private void OnMovieLoopPointReached()
		{
			if (this.moviePlayerSceneReference != null && !this.moviePlayerSceneReference.loop)
			{
				this.moviePlayerSceneReference.Ended(this);
				this.StopMovie();
			}
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00077C9B File Offset: 0x00075E9B
		public void StopMovie()
		{
			this.moviePlaying = false;
			if (this.PlatformMoviePlayer != null)
			{
				this.PlatformMoviePlayer.Stop();
			}
		}

		// Token: 0x04001390 RID: 5008
		public bool moviePlaying;

		// Token: 0x04001391 RID: 5009
		public MoviePlayer moviePlayerSceneReference;

		// Token: 0x04001392 RID: 5010
		public IMoviePlayer PlatformMoviePlayer;

		// Token: 0x04001393 RID: 5011
		public bool canSkipmovie;

		// Token: 0x04001394 RID: 5012
		public bool movieToResume;
	}

	// Token: 0x020003AE RID: 942
	// (Invoke) Token: 0x060016D3 RID: 5843
	public delegate void Pause();

	// Token: 0x020003AF RID: 943
	[Serializable]
	private struct AudioSourceValues
	{
		// Token: 0x060016D6 RID: 5846 RVA: 0x00077CB7 File Offset: 0x00075EB7
		public AudioSourceValues(AudioSource _source, float pitch)
		{
			this.source = _source;
			this.originalPitch = pitch;
		}

		// Token: 0x04001396 RID: 5014
		public AudioSource source;

		// Token: 0x04001397 RID: 5015
		public float originalPitch;
	}

	// Token: 0x020003B0 RID: 944
	public enum InputDevice
	{
		// Token: 0x04001399 RID: 5017
		KeyboardAndMouse,
		// Token: 0x0400139A RID: 5018
		GamepadXBOXOneOrGeneric,
		// Token: 0x0400139B RID: 5019
		GamepadPS4,
		// Token: 0x0400139C RID: 5020
		GamepadSwitch,
		// Token: 0x0400139D RID: 5021
		GamepadXBOXSeriesX,
		// Token: 0x0400139E RID: 5022
		GamepadPS5
	}

	// Token: 0x020003B1 RID: 945
	public enum TSP_Reload_Behaviour
	{
		// Token: 0x040013A0 RID: 5024
		Standard,
		// Token: 0x040013A1 RID: 5025
		Epilogue
	}
}
