using System;
using SettingsMenu.Components.Pages;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000354 RID: 852
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class PowerUpMeter : MonoSingleton<PowerUpMeter>
{
	// Token: 0x060013CD RID: 5069 RVA: 0x0009E577 File Offset: 0x0009C777
	private void Start()
	{
		this.vignette.enabled = false;
		this.meter = base.GetComponent<Image>();
		this.meter.fillAmount = 0f;
		this.pp = MonoSingleton<PostProcessV2_Handler>.Instance;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x0009E5AC File Offset: 0x0009C7AC
	private void Update()
	{
		this.UpdateMeter();
		this.UpdateCheats();
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x0009E5BC File Offset: 0x0009C7BC
	public void UpdateMeter()
	{
		if (this.juice > 0f)
		{
			this.hasPowerUp = true;
			if (!InfinitePowerUps.Enabled)
			{
				this.juice -= Time.deltaTime;
			}
			if (HUDSettings.powerUpMeterEnabled && !HideUI.Active)
			{
				this.meter.fillAmount = this.juice / this.latestMaxJuice;
			}
			else
			{
				this.meter.fillAmount = 0f;
			}
			if (this.currentColor != this.powerUpColor)
			{
				this.currentColor = this.powerUpColor;
				this.currentColor.a = this.juice / this.latestMaxJuice;
				this.vignette.color = this.currentColor;
				Shader.SetGlobalColor("_VignetteColor", this.currentColor);
				this.pp.Vignette(true);
				return;
			}
		}
		else if (this.hasPowerUp)
		{
			this.EndPowerUp();
		}
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x0009E6A4 File Offset: 0x0009C8A4
	public void EndPowerUp()
	{
		this.hasPowerUp = false;
		this.juice = 0f;
		this.latestMaxJuice = 0f;
		this.meter.fillAmount = 0f;
		if (this.vignette.color.a != 0f)
		{
			this.currentColor.a = 0f;
			Shader.SetGlobalColor("_VignetteColor", this.currentColor);
			this.vignette.color = this.currentColor;
			this.pp.Vignette(false);
		}
		if (this.endEffect)
		{
			Object.Instantiate<GameObject>(this.endEffect, MonoSingleton<NewMovement>.Instance.transform.position, Quaternion.identity);
		}
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x0009E75F File Offset: 0x0009C95F
	private void UpdateCheats()
	{
		if (!HideUI.Active)
		{
			return;
		}
		Shader.SetGlobalColor("_VignetteColor", Color.clear);
		this.pp.Vignette(false);
		this.vignette.color = Color.clear;
	}

	// Token: 0x04001B31 RID: 6961
	public float juice;

	// Token: 0x04001B32 RID: 6962
	public float latestMaxJuice;

	// Token: 0x04001B33 RID: 6963
	private Image meter;

	// Token: 0x04001B34 RID: 6964
	public Image vignette;

	// Token: 0x04001B35 RID: 6965
	public Color powerUpColor;

	// Token: 0x04001B36 RID: 6966
	private Color currentColor;

	// Token: 0x04001B37 RID: 6967
	public GameObject endEffect;

	// Token: 0x04001B38 RID: 6968
	private bool hasPowerUp;

	// Token: 0x04001B39 RID: 6969
	private PostProcessV2_Handler pp;
}
