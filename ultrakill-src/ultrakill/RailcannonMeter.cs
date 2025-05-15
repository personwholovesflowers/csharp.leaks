using System;
using SettingsMenu.Components.Pages;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000372 RID: 882
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class RailcannonMeter : MonoSingleton<RailcannonMeter>
{
	// Token: 0x06001481 RID: 5249 RVA: 0x000A6354 File Offset: 0x000A4554
	private void Start()
	{
		this.CheckStatus();
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000A635C File Offset: 0x000A455C
	protected override void OnEnable()
	{
		base.OnEnable();
		this.CheckStatus();
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000A636C File Offset: 0x000A456C
	private void Update()
	{
		if (!this.self.enabled && !this.miniVersion.activeSelf)
		{
			this.flashAmount = 0f;
			this.meterBackground.enabled = false;
			this.hasFlashed = false;
			Image[] array = this.trueMeters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			return;
		}
		for (int j = 0; j < this.trueMeters.Length; j++)
		{
			if (this.self.enabled || j != 0)
			{
				this.trueMeters[j].enabled = true;
			}
			else
			{
				this.trueMeters[j].enabled = false;
			}
		}
		if (MonoSingleton<WeaponCharges>.Instance.raicharge > 4f)
		{
			if (!this.hasFlashed && Time.timeScale > 0f)
			{
				this.flashAmount = 1f;
			}
			this.hasFlashed = true;
			if (!MonoSingleton<ColorBlindSettings>.Instance)
			{
				return;
			}
			Color color = MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(HudColorType.railcannonFull);
			if (this.flashAmount > 0f)
			{
				color = Color.Lerp(color, Color.white, this.flashAmount);
				this.flashAmount = Mathf.MoveTowards(this.flashAmount, 0f, Time.deltaTime);
			}
			foreach (Image image in this.trueMeters)
			{
				image.fillAmount = 1f;
				if (image != this.colorlessMeter)
				{
					image.color = color;
				}
				else
				{
					image.color = Color.white;
				}
			}
		}
		else
		{
			this.flashAmount = 0f;
			this.hasFlashed = false;
			foreach (Image image2 in this.trueMeters)
			{
				image2.color = MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(HudColorType.railcannonCharging);
				image2.fillAmount = MonoSingleton<WeaponCharges>.Instance.raicharge / 4f;
			}
		}
		if (MonoSingleton<WeaponCharges>.Instance.raicharge > 4f || !this.self.enabled)
		{
			this.meterBackground.enabled = false;
			return;
		}
		this.meterBackground.enabled = true;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000A6578 File Offset: 0x000A4778
	public void CheckStatus()
	{
		if (this.trueMeters == null || this.trueMeters.Length == 0)
		{
			this.trueMeters = new Image[this.meters.Length + 1];
			for (int i = 0; i < this.trueMeters.Length; i++)
			{
				if (i < this.meters.Length)
				{
					this.trueMeters[i] = this.meters[i];
				}
				else
				{
					this.trueMeters[i] = this.colorlessMeter;
				}
			}
		}
		if (!this.self)
		{
			this.self = base.GetComponent<Image>();
		}
		GameObject[] array;
		if (HUDSettings.railcannonMeterEnabled && this.RailcannonStatus())
		{
			if (HUDSettings.weaponIconEnabled)
			{
				this.self.enabled = true;
				this.miniVersion.SetActive(false);
			}
			else
			{
				this.self.enabled = false;
				this.miniVersion.SetActive(true);
			}
			array = this.altHudPanels;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(true);
			}
			return;
		}
		this.self.enabled = false;
		this.miniVersion.SetActive(false);
		array = this.altHudPanels;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(false);
		}
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000A66A0 File Offset: 0x000A48A0
	private bool RailcannonStatus()
	{
		for (int i = 0; i < 4; i++)
		{
			string text = "rai" + i.ToString();
			if (GameProgressSaver.CheckGear(text) == 1 && MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + text, 1) == 1 && !MonoSingleton<GunControl>.Instance.noWeapons)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04001C3A RID: 7226
	public Image meterBackground;

	// Token: 0x04001C3B RID: 7227
	public Image[] meters;

	// Token: 0x04001C3C RID: 7228
	private Image[] trueMeters;

	// Token: 0x04001C3D RID: 7229
	public Image colorlessMeter;

	// Token: 0x04001C3E RID: 7230
	private Image self;

	// Token: 0x04001C3F RID: 7231
	public GameObject[] altHudPanels;

	// Token: 0x04001C40 RID: 7232
	private float flashAmount;

	// Token: 0x04001C41 RID: 7233
	public GameObject miniVersion;

	// Token: 0x04001C42 RID: 7234
	private bool hasFlashed;
}
