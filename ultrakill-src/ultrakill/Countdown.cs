using System;
using TMPro;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class Countdown : MonoBehaviour
{
	// Token: 0x060004A5 RID: 1189 RVA: 0x0001FD44 File Offset: 0x0001DF44
	private void Awake()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001FD5C File Offset: 0x0001DF5C
	private void Start()
	{
		if (this.time == 0f && !this.done)
		{
			this.time = this.GetCountdownLength();
		}
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0001FD7F File Offset: 0x0001DF7F
	private void OnEnable()
	{
		this.ResetTime();
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0001FD87 File Offset: 0x0001DF87
	private void OnDisable()
	{
		if (this.bossbar && this.disableBossBarOnDisable)
		{
			this.bossbar.secondaryBarValue = 0f;
			this.bossbar.secondaryBar = false;
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001FDBC File Offset: 0x0001DFBC
	private void Update()
	{
		if (!this.paused)
		{
			this.time = Mathf.MoveTowards(this.time, 0f, Time.deltaTime);
		}
		if (!this.done && this.time <= 0f)
		{
			UltrakillEvent ultrakillEvent = this.onZero;
			if (ultrakillEvent != null)
			{
				ultrakillEvent.Invoke("");
			}
			this.done = true;
		}
		if (this.countdownText)
		{
			if (this.decimalFontSize == 0f)
			{
				this.countdownText.text = this.time.ToString("F2");
			}
			else
			{
				int num = Mathf.FloorToInt(this.time % 1f * 100f);
				this.countdownText.text = string.Concat(new string[]
				{
					Mathf.FloorToInt(this.time).ToString(),
					"<size=",
					this.decimalFontSize.ToString(),
					(num < 10) ? ">.0" : ">.",
					num.ToString()
				});
			}
		}
		if (this.bossbar)
		{
			this.bossbar.secondaryBar = true;
			this.bossbar.secondaryBarValue = (this.invertBossBarAmount ? ((this.countdownLength - this.time) / this.countdownLength) : (this.time / this.countdownLength));
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001FF1F File Offset: 0x0001E11F
	public void PauseState(bool pause)
	{
		this.paused = pause;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001FF28 File Offset: 0x0001E128
	public void ChangeTime(float newTime)
	{
		this.time = newTime;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001FF31 File Offset: 0x0001E131
	public void ResetTime()
	{
		this.time = this.GetCountdownLength();
		this.done = false;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001FF46 File Offset: 0x0001E146
	private float GetCountdownLength()
	{
		if (!this.changePerDifficulty)
		{
			return this.countdownLength;
		}
		return this.countdownLengthPerDifficulty[this.difficulty];
	}

	// Token: 0x0400064B RID: 1611
	public bool changePerDifficulty;

	// Token: 0x0400064C RID: 1612
	public float countdownLength;

	// Token: 0x0400064D RID: 1613
	public float[] countdownLengthPerDifficulty = new float[6];

	// Token: 0x0400064E RID: 1614
	private float time;

	// Token: 0x0400064F RID: 1615
	public TextMeshProUGUI countdownText;

	// Token: 0x04000650 RID: 1616
	public float decimalFontSize;

	// Token: 0x04000651 RID: 1617
	public BossHealthBar bossbar;

	// Token: 0x04000652 RID: 1618
	public bool invertBossBarAmount;

	// Token: 0x04000653 RID: 1619
	public bool disableBossBarOnDisable;

	// Token: 0x04000654 RID: 1620
	public bool paused;

	// Token: 0x04000655 RID: 1621
	public bool resetOnEnable;

	// Token: 0x04000656 RID: 1622
	public UltrakillEvent onZero;

	// Token: 0x04000657 RID: 1623
	private bool done;

	// Token: 0x04000658 RID: 1624
	private int difficulty;
}
