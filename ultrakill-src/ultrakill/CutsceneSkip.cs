using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class CutsceneSkip : MonoBehaviour
{
	// Token: 0x060004DF RID: 1247 RVA: 0x0002128E File Offset: 0x0001F48E
	private void Start()
	{
		if (CheckLevelRank.CheckLevelStatus() || Debug.isDebugBuild)
		{
			this.waitingForInput = true;
			this.timeLeft = this.addToTimer;
			MonoSingleton<CutsceneSkipText>.Instance.Show();
			this.startTime = MonoSingleton<StatsManager>.Instance.seconds;
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000212CB File Offset: 0x0001F4CB
	private void OnEnable()
	{
		if (this.waitingForInput)
		{
			MonoSingleton<CutsceneSkipText>.Instance.Show();
		}
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x000212E0 File Offset: 0x0001F4E0
	private void OnDisable()
	{
		if (!MonoSingleton<CutsceneSkipText>.Instance)
		{
			return;
		}
		if (this.waitingForInput)
		{
			MonoSingleton<CutsceneSkipText>.Instance.Hide();
			if (this.printLengthOfCutscene)
			{
				Debug.Log("Length of cutscene: " + (MonoSingleton<StatsManager>.Instance.seconds - this.startTime).ToString());
			}
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x0002133C File Offset: 0x0001F53C
	private void Update()
	{
		if (this.waitingForInput && MonoSingleton<StatsManager>.Instance.timer)
		{
			this.timeLeft = Mathf.MoveTowards(this.timeLeft, 0f, Time.deltaTime);
		}
		if (this.waitingForInput && MonoSingleton<NewMovement>.Instance.dead)
		{
			this.waitingForInput = false;
			MonoSingleton<CutsceneSkipText>.Instance.Hide();
		}
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x000213A0 File Offset: 0x0001F5A0
	private void LateUpdate()
	{
		if (this.waitingForInput && MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame)
		{
			this.waitingForInput = false;
			this.printLengthOfCutscene = false;
			if (MonoSingleton<OptionsManager>.Instance.paused)
			{
				MonoSingleton<OptionsManager>.Instance.UnPause();
			}
			this.onSkip.Invoke("");
			MonoSingleton<CutsceneSkipText>.Instance.Hide();
			if (this.timeLeft > 0f)
			{
				MonoSingleton<StatsManager>.Instance.seconds += this.timeLeft;
				MonoSingleton<WeaponCharges>.Instance.Charge(this.timeLeft);
			}
		}
	}

	// Token: 0x040006A4 RID: 1700
	public float addToTimer;

	// Token: 0x040006A5 RID: 1701
	private float timeLeft;

	// Token: 0x040006A6 RID: 1702
	public UltrakillEvent onSkip;

	// Token: 0x040006A7 RID: 1703
	private bool waitingForInput;

	// Token: 0x040006A8 RID: 1704
	private bool printLengthOfCutscene = true;

	// Token: 0x040006A9 RID: 1705
	private float startTime;
}
