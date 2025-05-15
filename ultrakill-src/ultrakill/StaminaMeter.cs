using System;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200044C RID: 1100
public class StaminaMeter : MonoBehaviour
{
	// Token: 0x060018E8 RID: 6376 RVA: 0x000CA048 File Offset: 0x000C8248
	private void Start()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		this.stm = base.GetComponent<Slider>();
		if (this.stm != null)
		{
			this.staminaBar = base.transform.GetChild(1).GetChild(0).GetComponent<Image>();
			this.staminaFlash = this.staminaBar.transform.GetChild(0).GetComponent<Image>();
			this.flashColor = this.staminaFlash.color;
			this.origColor = this.staminaBar.color;
		}
		this.stmText = base.GetComponent<TMP_Text>();
		this.nmov = MonoSingleton<NewMovement>.Instance;
		this.lastStamina = this.stamina;
		this.UpdateColors();
		this.parentCanvas = base.GetComponentInParent<Canvas>();
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x000CA113 File Offset: 0x000C8313
	private void OnEnable()
	{
		this.UpdateColors();
	}

	// Token: 0x060018EA RID: 6378 RVA: 0x000CA11C File Offset: 0x000C831C
	private void Update()
	{
		if (this.intro)
		{
			this.stamina = Mathf.MoveTowards(this.stamina, this.nmov.boostCharge, Time.deltaTime * ((this.nmov.boostCharge - this.stamina) * 5f + 10f));
			if (this.stamina >= this.nmov.boostCharge)
			{
				this.intro = false;
			}
		}
		else if (this.stamina < this.nmov.boostCharge)
		{
			this.stamina = Mathf.MoveTowards(this.stamina, this.nmov.boostCharge, Time.deltaTime * ((this.nmov.boostCharge - this.stamina) * 25f + 25f));
		}
		else if (this.stamina > this.nmov.boostCharge)
		{
			this.stamina = Mathf.MoveTowards(this.stamina, this.nmov.boostCharge, Time.deltaTime * ((this.stamina - this.nmov.boostCharge) * 25f + 25f));
		}
		if (this.alwaysUpdate || (this.parentCanvas && this.parentCanvas.enabled))
		{
			if (this.stm != null)
			{
				this.stm.value = this.stamina;
				if (this.stm.value >= this.stm.maxValue && !this.full)
				{
					this.full = true;
					this.staminaBar.color = this.origColor;
					this.Flash(false);
				}
				if (this.flashColor.a > 0f)
				{
					if (this.flashColor.a - Time.deltaTime > 0f)
					{
						this.flashColor.a = this.flashColor.a - Time.deltaTime;
					}
					else
					{
						this.flashColor.a = 0f;
					}
					this.staminaFlash.color = this.flashColor;
				}
				if (this.stm.value < this.stm.maxValue)
				{
					this.full = false;
					this.staminaBar.color = this.emptyColor;
				}
			}
			if (this.stmText != null)
			{
				if (this.lastStamina != this.stamina)
				{
					this.stmText.text = (this.stamina / 100f).ToString("0.00");
				}
				this.lastStamina = this.stamina;
				if (this.changeTextColor)
				{
					if (this.stamina < 100f)
					{
						this.stmText.color = Color.red;
						return;
					}
					this.stmText.color = MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(HudColorType.stamina);
					return;
				}
				else if (this.normalTextColor == Color.white)
				{
					if (this.stamina < 100f)
					{
						this.stmText.color = Color.red;
						return;
					}
					this.stmText.color = MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(HudColorType.healthText);
				}
			}
		}
	}

	// Token: 0x060018EB RID: 6379 RVA: 0x000CA428 File Offset: 0x000C8628
	public void Flash(bool red = false)
	{
		if (this.stm != null)
		{
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.Play();
			if (red)
			{
				this.flashColor = Color.red;
			}
			else
			{
				this.flashColor = Color.white;
			}
			this.staminaFlash.color = this.flashColor;
		}
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000CA494 File Offset: 0x000C8694
	public void UpdateColors()
	{
		this.origColor = MonoSingleton<ColorBlindSettings>.Instance.staminaColor;
		if (this.redEmpty)
		{
			this.emptyColor = MonoSingleton<ColorBlindSettings>.Instance.staminaEmptyColor;
		}
		else
		{
			this.emptyColor = MonoSingleton<ColorBlindSettings>.Instance.staminaChargingColor;
		}
		if (this.staminaBar)
		{
			if (this.full)
			{
				this.staminaBar.color = this.origColor;
				return;
			}
			this.staminaBar.color = this.emptyColor;
		}
	}

	// Token: 0x040022C3 RID: 8899
	private NewMovement nmov;

	// Token: 0x040022C4 RID: 8900
	private float stamina;

	// Token: 0x040022C5 RID: 8901
	private Slider stm;

	// Token: 0x040022C6 RID: 8902
	private TMP_Text stmText;

	// Token: 0x040022C7 RID: 8903
	public bool changeTextColor;

	// Token: 0x040022C8 RID: 8904
	public Color normalTextColor;

	// Token: 0x040022C9 RID: 8905
	private Image staminaFlash;

	// Token: 0x040022CA RID: 8906
	private Color flashColor;

	// Token: 0x040022CB RID: 8907
	private Image staminaBar;

	// Token: 0x040022CC RID: 8908
	private bool full = true;

	// Token: 0x040022CD RID: 8909
	private AudioSource aud;

	// Token: 0x040022CE RID: 8910
	private Color emptyColor;

	// Token: 0x040022CF RID: 8911
	private Color origColor;

	// Token: 0x040022D0 RID: 8912
	public bool redEmpty;

	// Token: 0x040022D1 RID: 8913
	private bool intro = true;

	// Token: 0x040022D2 RID: 8914
	private float lastStamina;

	// Token: 0x040022D3 RID: 8915
	private Canvas parentCanvas;

	// Token: 0x040022D4 RID: 8916
	public bool alwaysUpdate;
}
