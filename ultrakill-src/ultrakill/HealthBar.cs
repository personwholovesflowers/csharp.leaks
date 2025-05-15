using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000246 RID: 582
public class HealthBar : MonoBehaviour
{
	// Token: 0x06000CCB RID: 3275 RVA: 0x0005F20F File Offset: 0x0005D40F
	private void Start()
	{
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.colorBlindSettings = MonoSingleton<ColorBlindSettings>.Instance;
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0005F240 File Offset: 0x0005D440
	private void Update()
	{
		if (this.hp < (float)this.nmov.hp)
		{
			this.hp = Mathf.MoveTowards(this.hp, (float)this.nmov.hp, Time.deltaTime * (((float)this.nmov.hp - this.hp) * 5f + 5f));
		}
		else if (this.hp > (float)this.nmov.hp)
		{
			this.hp = (float)this.nmov.hp;
		}
		if (this.hpSliders.Length != 0)
		{
			foreach (Slider slider in this.hpSliders)
			{
				if (slider.value != this.hp)
				{
					slider.value = this.hp;
				}
			}
		}
		if (this.afterImageSliders != null)
		{
			foreach (Slider slider2 in this.afterImageSliders)
			{
				if (slider2.value < this.hp)
				{
					slider2.value = this.hp;
				}
				else if (slider2.value > this.hp)
				{
					slider2.value = Mathf.MoveTowards(slider2.value, this.hp, Time.deltaTime * ((slider2.value - this.hp) * 5f + 5f));
				}
			}
		}
		if (this.antiHpSlider != null)
		{
			if (this.antiHpSlider.value != this.nmov.antiHp)
			{
				this.antiHpSlider.value = Mathf.MoveTowards(this.antiHpSlider.value, this.nmov.antiHp, Time.deltaTime * (Mathf.Abs(this.antiHpSlider.value - this.nmov.antiHp) * 5f + 5f));
			}
			if (this.antiHpSliderFill != null)
			{
				this.antiHpSliderFill.enabled = this.antiHpSlider.value > 0f;
			}
		}
		if (this.hpText != null)
		{
			if (!this.antiHpText)
			{
				if (this.lastHP != this.hp)
				{
					this.hpText.text = this.hp.ToString("F0");
					this.lastHP = this.hp;
				}
				if (this.changeTextColor)
				{
					if (this.hp <= 30f)
					{
						this.hpText.color = Color.red;
						return;
					}
					if (this.hp <= 50f && this.yellowColor)
					{
						this.hpText.color = Color.yellow;
						return;
					}
					this.hpText.color = this.normalTextColor;
					return;
				}
				else if (this.normalTextColor == Color.white)
				{
					if (this.hp <= 30f)
					{
						this.hpText.color = Color.red;
						return;
					}
					this.hpText.color = this.colorBlindSettings.GetHudColor(HudColorType.healthText);
					return;
				}
			}
			else
			{
				if (this.difficulty == 0)
				{
					this.hpText.text = this.lowDifHealth;
					return;
				}
				this.antiHp = Mathf.MoveTowards(this.antiHp, this.nmov.antiHp, Time.deltaTime * (Mathf.Abs(this.antiHp - this.nmov.antiHp) * 5f + 5f));
				float num = 100f - this.antiHp;
				if (this.lastAntiHP != num)
				{
					this.hpText.text = "/" + num.ToString("F0");
					this.lastAntiHP = num;
				}
			}
		}
	}

	// Token: 0x040010F9 RID: 4345
	private NewMovement nmov;

	// Token: 0x040010FA RID: 4346
	public Slider[] hpSliders;

	// Token: 0x040010FB RID: 4347
	public Slider[] afterImageSliders;

	// Token: 0x040010FC RID: 4348
	public Slider antiHpSlider;

	// Token: 0x040010FD RID: 4349
	public Image antiHpSliderFill;

	// Token: 0x040010FE RID: 4350
	public TMP_Text hpText;

	// Token: 0x040010FF RID: 4351
	private float hp;

	// Token: 0x04001100 RID: 4352
	private float antiHp;

	// Token: 0x04001101 RID: 4353
	public bool changeTextColor;

	// Token: 0x04001102 RID: 4354
	public Color normalTextColor;

	// Token: 0x04001103 RID: 4355
	public bool yellowColor;

	// Token: 0x04001104 RID: 4356
	public bool antiHpText;

	// Token: 0x04001105 RID: 4357
	private int difficulty;

	// Token: 0x04001106 RID: 4358
	private float lastHP;

	// Token: 0x04001107 RID: 4359
	private float lastAntiHP;

	// Token: 0x04001108 RID: 4360
	private string lowDifHealth = "/200";

	// Token: 0x04001109 RID: 4361
	private ColorBlindSettings colorBlindSettings;
}
