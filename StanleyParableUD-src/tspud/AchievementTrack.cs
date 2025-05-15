using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200008A RID: 138
public class AchievementTrack : MonoBehaviour
{
	// Token: 0x06000348 RID: 840 RVA: 0x0001629C File Offset: 0x0001449C
	private void Awake()
	{
		PlatformAchievements.AchievementUnlocked = (Action<AchievementID>)Delegate.Combine(PlatformAchievements.AchievementUnlocked, new Action<AchievementID>(this.OnUnlockAchievement));
		SimpleEvent simpleEvent = this.freedomEndingCompleteEvent;
		simpleEvent.OnCall = (Action)Delegate.Combine(simpleEvent.OnCall, new Action(this.CheckSpeedrun));
		StanleyController.OnInteract = (Action<GameObject>)Delegate.Combine(StanleyController.OnInteract, new Action<GameObject>(this.UpdateYouCantJump));
		if (PlatformPlayerPrefs.HasKey(this.playingMinutesKey))
		{
			this.playingMinutes = PlatformPlayerPrefs.GetInt(this.playingMinutesKey);
		}
		this.FillConfDictionary();
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00016334 File Offset: 0x00014534
	private void FillConfDictionary()
	{
		foreach (FloatConfigurable floatConfigurable in this.floatConfigurables)
		{
			this.trackedFloatConfigurables.Add(floatConfigurable.Key, new AchievementTrack.NumberConfigurableStatus(floatConfigurable));
			FloatConfigurable floatConfigurable2 = floatConfigurable;
			floatConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(floatConfigurable2.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
		foreach (BooleanConfigurable booleanConfigurable in this.booleanConfigurables)
		{
			this.trackedBooleanConfigurables.Add(booleanConfigurable.Key, new AchievementTrack.BooleanConfigurableStatus(booleanConfigurable));
			BooleanConfigurable booleanConfigurable2 = booleanConfigurable;
			booleanConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable2.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
		foreach (IntConfigurable intConfigurable in this.intConfigurables)
		{
			this.trackedIntConfigurables.Add(intConfigurable.Key, new AchievementTrack.NumberConfigurableStatus(intConfigurable));
			IntConfigurable intConfigurable2 = intConfigurable;
			intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable2.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00016444 File Offset: 0x00014644
	private void OnConfigurableValueUpdate(LiveData data)
	{
		switch (data.configureableType)
		{
		case ConfigurableTypes.Int:
		{
			AchievementTrack.NumberConfigurableStatus numberConfigurableStatus;
			if (this.trackedIntConfigurables.TryGetValue(data.key, out numberConfigurableStatus))
			{
				if (data.IntValue == numberConfigurableStatus.Intc.MaxValue)
				{
					numberConfigurableStatus.maxValueReached = true;
				}
				if (data.IntValue == numberConfigurableStatus.Intc.MinValue)
				{
					numberConfigurableStatus.minValueReached = true;
				}
				if (numberConfigurableStatus.minValueReached && numberConfigurableStatus.maxValueReached)
				{
					IntConfigurable intc = numberConfigurableStatus.Intc;
					intc.OnValueChanged = (Action<LiveData>)Delegate.Remove(intc.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
					this.trackedIntConfigurables.Remove(data.key);
				}
			}
			break;
		}
		case ConfigurableTypes.Float:
		{
			AchievementTrack.NumberConfigurableStatus numberConfigurableStatus2;
			if (this.trackedFloatConfigurables.TryGetValue(data.key, out numberConfigurableStatus2))
			{
				if (Mathf.Approximately(data.FloatValue, numberConfigurableStatus2.FloatC.MaxValue))
				{
					numberConfigurableStatus2.maxValueReached = true;
				}
				if (Mathf.Approximately(data.FloatValue, numberConfigurableStatus2.FloatC.MinValue))
				{
					numberConfigurableStatus2.minValueReached = true;
				}
				if (numberConfigurableStatus2.minValueReached && numberConfigurableStatus2.maxValueReached)
				{
					FloatConfigurable floatC = numberConfigurableStatus2.FloatC;
					floatC.OnValueChanged = (Action<LiveData>)Delegate.Remove(floatC.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
					this.trackedFloatConfigurables.Remove(data.key);
				}
			}
			break;
		}
		case ConfigurableTypes.Boolean:
		{
			AchievementTrack.BooleanConfigurableStatus booleanConfigurableStatus = null;
			if (this.trackedBooleanConfigurables.TryGetValue(data.key, out booleanConfigurableStatus) && data.BooleanValue != booleanConfigurableStatus.defaultValue)
			{
				BooleanConfigurable cachedBoolC = booleanConfigurableStatus.cachedBoolC;
				cachedBoolC.OnValueChanged = (Action<LiveData>)Delegate.Remove(cachedBoolC.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
				this.trackedBooleanConfigurables.Remove(data.key);
			}
			break;
		}
		}
		if (this.trackedFloatConfigurables.Count == 0 && this.trackedIntConfigurables.Count == 0 && this.trackedBooleanConfigurables.Count == 0)
		{
			this.settingsWorldChampToEnableBooleanConfigurable.SetValue(true);
			PlatformAchievements.UnlockAchievement(AchievementID.SettingsWorldChampion);
		}
	}

	// Token: 0x0600034B RID: 843 RVA: 0x00016654 File Offset: 0x00014854
	private void OnDestroy()
	{
		PlatformAchievements.AchievementUnlocked = (Action<AchievementID>)Delegate.Remove(PlatformAchievements.AchievementUnlocked, new Action<AchievementID>(this.OnUnlockAchievement));
		SimpleEvent simpleEvent = this.freedomEndingCompleteEvent;
		simpleEvent.OnCall = (Action)Delegate.Remove(simpleEvent.OnCall, new Action(this.CheckSpeedrun));
		StanleyController.OnInteract = (Action<GameObject>)Delegate.Remove(StanleyController.OnInteract, new Action<GameObject>(this.UpdateYouCantJump));
		foreach (FloatConfigurable floatConfigurable in this.floatConfigurables)
		{
			floatConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(floatConfigurable.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
		foreach (BooleanConfigurable booleanConfigurable in this.booleanConfigurables)
		{
			booleanConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
		foreach (IntConfigurable intConfigurable in this.intConfigurables)
		{
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnConfigurableValueUpdate));
		}
	}

	// Token: 0x0600034C RID: 844 RVA: 0x00016773 File Offset: 0x00014973
	private void Start()
	{
		SceneManager.sceneLoaded += this.CheckStartupAchievements;
	}

	// Token: 0x0600034D RID: 845 RVA: 0x00016788 File Offset: 0x00014988
	private void CheckStartupAchievements(Scene scene, LoadSceneMode loadsceneMode)
	{
		if (scene.name == "init" || scene.name == "Loading_UD_MASTER" || scene.name == "Settings_UD_MASTER")
		{
			return;
		}
		if (!this.checkedStartupAchievements)
		{
			this.checkedStartupAchievements = true;
			base.StartCoroutine(this.CheckStartupAchievementsRoutine());
		}
	}

	// Token: 0x0600034E RID: 846 RVA: 0x000167EB File Offset: 0x000149EB
	private IEnumerator CheckStartupAchievementsRoutine()
	{
		float checkTimer = 2f;
		while (checkTimer > 0f)
		{
			if (this.IsInGame)
			{
				checkTimer -= Time.unscaledDeltaTime;
			}
			yield return null;
		}
		this.CheckWelcomeBack();
		this.CheckSuperGoOutside();
		yield break;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x000167FA File Offset: 0x000149FA
	private void Update()
	{
		this.runTime = GameMaster.RunTime;
		this.CheckCommitment();
		this.CheckJumpKeyboardMouse();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x00016818 File Offset: 0x00014A18
	private void OnUnlockAchievement(AchievementID id)
	{
		base.StartCoroutine(this.CheckFirstAchievement());
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00016827 File Offset: 0x00014A27
	private IEnumerator CheckFirstAchievement()
	{
		yield return new WaitForSeconds(1f);
		if (PlatformAchievements.AchievementsUnlockedCount >= 1 && !PlatformAchievements.IsAchievementUnlocked(AchievementID.First))
		{
			PlatformAchievements.UnlockAchievement(AchievementID.First);
		}
		yield break;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00016830 File Offset: 0x00014A30
	private void CheckCommitment()
	{
		if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
		{
			float num = Time.realtimeSinceStartup - this.minuteTimeStamp;
			if (num >= 60f)
			{
				int num2 = Mathf.FloorToInt(num / 60f);
				this.minuteTimeStamp = Time.realtimeSinceStartup;
				this.playingMinutes += num2;
				if (this.playingMinutes >= 1439)
				{
					PlatformAchievements.UnlockAchievement(AchievementID.Tuesday);
				}
				PlatformPlayerPrefs.SetInt(this.playingMinutesKey, this.playingMinutes);
			}
		}
	}

	// Token: 0x06000353 RID: 851 RVA: 0x000168AC File Offset: 0x00014AAC
	private void CheckWelcomeBack()
	{
		string text = "FirstStartup";
		if (PlatformPlayerPrefs.HasKey(text))
		{
			PlatformAchievements.UnlockAchievement(AchievementID.WelcomeBack);
			return;
		}
		PlatformPlayerPrefs.SetInt(text, 1);
	}

	// Token: 0x06000354 RID: 852 RVA: 0x000168D5 File Offset: 0x00014AD5
	private void CheckSpeedrun()
	{
		if (GameMaster.RunTime < 262f)
		{
			PlatformAchievements.UnlockAchievement(AchievementID.SpeedRun);
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x06000355 RID: 853 RVA: 0x000168F3 File Offset: 0x00014AF3
	private int lastJumpCounterMax
	{
		get
		{
			return 9;
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x06000356 RID: 854 RVA: 0x000168F7 File Offset: 0x00014AF7
	private float lastJumpTimeTimeout
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x06000357 RID: 855 RVA: 0x000168FE File Offset: 0x00014AFE
	private bool IsInGame
	{
		get
		{
			return !GameMaster.PAUSEMENUACTIVE && !GameMaster.ONMAINMENUORSETTINGS && !Singleton<GameMaster>.Instance.IsLoading && !Singleton<GameMaster>.Instance.FullScreenMoviePlaying && !StanleyController.Instance.motionFrozen;
		}
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00016935 File Offset: 0x00014B35
	private void CheckJumpKeyboardMouse()
	{
		if (this.IsInGame && Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse && Singleton<GameMaster>.Instance.stanleyActions.JumpAction.WasPressed)
		{
			this.UpdateYouCantJump(null);
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00016968 File Offset: 0x00014B68
	private void UpdateYouCantJump(GameObject interactObject)
	{
		if (interactObject != null || this.jumpCircleConfigurable.GetBooleanValue())
		{
			return;
		}
		float num = GameMaster.RunTime;
		if (this.lastJumpTime == -1f || num - this.lastJumpTime < this.lastJumpTimeTimeout)
		{
			this.lastJumpCounter++;
			this.lastJumpTime = num;
		}
		else
		{
			this.lastJumpCounter = -1;
			this.lastJumpTime = -1f;
		}
		if (this.lastJumpCounter >= this.lastJumpCounterMax)
		{
			PlatformAchievements.UnlockAchievement(AchievementID.YouCantJump);
		}
	}

	// Token: 0x0600035A RID: 858 RVA: 0x000169F4 File Offset: 0x00014BF4
	private void CheckSuperGoOutside()
	{
		string text = "LastStartupTime";
		if (PlatformPlayerPrefs.HasKey(text))
		{
			DateTime dateTime = Convert.ToDateTime(PlatformPlayerPrefs.GetString(text));
			if (this.HasItBeenTenYears(dateTime))
			{
				PlatformAchievements.UnlockAchievement(AchievementID.SuperGoOutside);
			}
		}
		PlatformPlayerPrefs.SetString(text, DateTime.UtcNow.ToString());
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00016A40 File Offset: 0x00014C40
	private bool HasItBeenTenYears(DateTime lastStartupTime)
	{
		bool flag = false;
		DateTime dateTime = lastStartupTime.AddYears(10).AddDays(-1.0);
		if (DateTime.UtcNow >= dateTime)
		{
			flag = true;
		}
		lastStartupTime - DateTime.UtcNow;
		return flag;
	}

	// Token: 0x04000345 RID: 837
	public AchievementID id;

	// Token: 0x04000346 RID: 838
	[SerializeField]
	private Configurable jumpCircleConfigurable;

	// Token: 0x04000347 RID: 839
	[SerializeField]
	private SimpleEvent freedomEndingCompleteEvent;

	// Token: 0x04000348 RID: 840
	[SerializeField]
	private float runTime;

	// Token: 0x04000349 RID: 841
	private int playingMinutes;

	// Token: 0x0400034A RID: 842
	private readonly string playingMinutesKey = "playingMinutes";

	// Token: 0x0400034B RID: 843
	private float minuteTimeStamp;

	// Token: 0x0400034C RID: 844
	[Header("Settings World Champion Track")]
	[SerializeField]
	private FloatConfigurable[] floatConfigurables;

	// Token: 0x0400034D RID: 845
	[SerializeField]
	private BooleanConfigurable[] booleanConfigurables;

	// Token: 0x0400034E RID: 846
	[SerializeField]
	private IntConfigurable[] intConfigurables;

	// Token: 0x0400034F RID: 847
	[SerializeField]
	private BooleanConfigurable settingsWorldChampToEnableBooleanConfigurable;

	// Token: 0x04000350 RID: 848
	private Dictionary<string, AchievementTrack.BooleanConfigurableStatus> trackedBooleanConfigurables = new Dictionary<string, AchievementTrack.BooleanConfigurableStatus>();

	// Token: 0x04000351 RID: 849
	private Dictionary<string, AchievementTrack.NumberConfigurableStatus> trackedIntConfigurables = new Dictionary<string, AchievementTrack.NumberConfigurableStatus>();

	// Token: 0x04000352 RID: 850
	private Dictionary<string, AchievementTrack.NumberConfigurableStatus> trackedFloatConfigurables = new Dictionary<string, AchievementTrack.NumberConfigurableStatus>();

	// Token: 0x04000353 RID: 851
	[SerializeField]
	private BooleanConfigurable sanityBool;

	// Token: 0x04000354 RID: 852
	private bool checkedStartupAchievements;

	// Token: 0x04000355 RID: 853
	private int lastJumpCounter;

	// Token: 0x04000356 RID: 854
	private float lastJumpTime = -1f;

	// Token: 0x02000388 RID: 904
	private class NumberConfigurableStatus
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x00076768 File Offset: 0x00074968
		public NumberConfigurableStatus(Configurable _configurable)
		{
			if (_configurable is FloatConfigurable)
			{
				this.FloatC = _configurable as FloatConfigurable;
			}
			if (_configurable is IntConfigurable)
			{
				this.Intc = _configurable as IntConfigurable;
			}
		}

		// Token: 0x040012E6 RID: 4838
		public FloatConfigurable FloatC;

		// Token: 0x040012E7 RID: 4839
		public IntConfigurable Intc;

		// Token: 0x040012E8 RID: 4840
		public bool minValueReached;

		// Token: 0x040012E9 RID: 4841
		public bool maxValueReached;
	}

	// Token: 0x02000389 RID: 905
	private class BooleanConfigurableStatus
	{
		// Token: 0x0600164B RID: 5707 RVA: 0x00076798 File Offset: 0x00074998
		public BooleanConfigurableStatus(BooleanConfigurable boolC)
		{
			this.cachedBoolC = boolC;
			this.defaultValue = this.cachedBoolC.GetBooleanValue();
		}

		// Token: 0x040012EA RID: 4842
		public BooleanConfigurable cachedBoolC;

		// Token: 0x040012EB RID: 4843
		public bool defaultValue;
	}
}
