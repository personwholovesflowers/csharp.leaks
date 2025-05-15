using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000247 RID: 583
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class HeatResistance : MonoSingleton<HeatResistance>
{
	// Token: 0x06000CCE RID: 3278 RVA: 0x0005F5E0 File Offset: 0x0005D7E0
	protected override void Awake()
	{
		base.Awake();
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (@int >= 2)
		{
			this.difficultySpeedModifier = 1f;
			return;
		}
		if (@int == 1)
		{
			this.difficultySpeedModifier = 0.75f;
			return;
		}
		this.difficultySpeedModifier = 0.5f;
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0005F62F File Offset: 0x0005D82F
	protected override void OnEnable()
	{
		base.OnEnable();
		this.RechargeOnce();
		StatsManager.checkpointRestart += this.RechargeOnce;
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0005F64E File Offset: 0x0005D84E
	private void OnDisable()
	{
		StatsManager.checkpointRestart -= this.RechargeOnce;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0005F664 File Offset: 0x0005D864
	private void Update()
	{
		if (this.recharging)
		{
			this.heatResistance = Mathf.MoveTowards(this.heatResistance, 100f, Time.deltaTime * this.speed * 12f * this.rechargeSpeed);
		}
		else if (this.heatResistance > 0f)
		{
			this.heatResistance = Mathf.MoveTowards(this.heatResistance, 0f, Time.deltaTime * this.speed * this.difficultySpeedModifier);
		}
		else
		{
			if (MonoSingleton<NewMovement>.Instance.hp > 100)
			{
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.Abs(100 - MonoSingleton<NewMovement>.Instance.hp), false, 0f, false, false, 0f, true);
			}
			MonoSingleton<NewMovement>.Instance.ForceAntiHP(this.hurtTimer * 5f * this.difficultySpeedModifier, true, false, true, true);
		}
		this.meter.value = Mathf.MoveTowards(this.meter.value, this.heatResistance, Time.deltaTime * 10f * Mathf.Max(1f, Mathf.Abs(Mathf.Abs(this.meter.value) - Mathf.Abs(this.heatResistance))));
		this.meterPercentage.text = this.meter.value.ToString("00.00") + "%";
		if (this.recharging || this.greenFlash.color.a != 0f)
		{
			this.greenFlash.color = new Color(this.greenFlash.color.r, this.greenFlash.color.g, this.greenFlash.color.b, this.recharging ? 1f : Mathf.MoveTowards(this.greenFlash.color.a, 0f, Time.deltaTime));
		}
		this.meterLabel.color = ((this.heatResistance > 0f) ? Color.white : Color.Lerp(Color.red, Color.white, this.hurtTimer % 0.5f));
		if (this.heatResistance > 0f)
		{
			this.hurtTimer = 0f;
		}
		if (this.heatResistance == 0f)
		{
			this.screenShatter.color = new Color(1f, 1f, 1f, 0.5f);
		}
		if (this.hurtingSound.activeSelf != (this.heatResistance == 0f))
		{
			this.hurtingSound.SetActive(this.heatResistance == 0f);
		}
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0005F914 File Offset: 0x0005DB14
	public void RechargeOnce()
	{
		this.heatResistance = 100f;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0005F921 File Offset: 0x0005DB21
	public void SetRechargeMode(float targetSpeedMultiplier)
	{
		this.recharging = targetSpeedMultiplier != 0f;
		this.rechargeSpeed = targetSpeedMultiplier;
	}

	// Token: 0x0400110A RID: 4362
	[SerializeField]
	private Slider meter;

	// Token: 0x0400110B RID: 4363
	[SerializeField]
	private TMP_Text meterLabel;

	// Token: 0x0400110C RID: 4364
	[SerializeField]
	private TMP_Text meterPercentage;

	// Token: 0x0400110D RID: 4365
	[SerializeField]
	private Image greenFlash;

	// Token: 0x0400110E RID: 4366
	[SerializeField]
	private GameObject hurtingSound;

	// Token: 0x0400110F RID: 4367
	[SerializeField]
	private Image screenShatter;

	// Token: 0x04001110 RID: 4368
	[SerializeField]
	private CanvasGroup heatResistanceGroup;

	// Token: 0x04001111 RID: 4369
	[SerializeField]
	private CanvasGroup heatFixedGroup;

	// Token: 0x04001112 RID: 4370
	public float speed;

	// Token: 0x04001113 RID: 4371
	private float difficultySpeedModifier = 1f;

	// Token: 0x04001114 RID: 4372
	private float heatResistance;

	// Token: 0x04001115 RID: 4373
	private TimeSince hurtTimer;

	// Token: 0x04001116 RID: 4374
	private bool recharging;

	// Token: 0x04001117 RID: 4375
	private float rechargeSpeed;
}
