using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000CE RID: 206
public class ColorBlindSetter : MonoBehaviour
{
	// Token: 0x06000418 RID: 1048 RVA: 0x0001C500 File Offset: 0x0001A700
	private void OnEnable()
	{
		if (this.nameText && this.enemyColor)
		{
			if (MonoSingleton<BestiaryData>.Instance.GetEnemy(this.ect) < 1)
			{
				this.nameText.text = "???";
				return;
			}
			this.nameText.text = this.originalName;
		}
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x0001C558 File Offset: 0x0001A758
	public void Prepare()
	{
		if (this.variationColor)
		{
			this.originalColor = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationNumber];
		}
		else if (!this.enemyColor)
		{
			this.originalColor = MonoSingleton<ColorBlindSettings>.Instance.GetHudColor(this.hct);
		}
		else
		{
			this.originalColor = MonoSingleton<ColorBlindSettings>.Instance.GetEnemyColor(this.ect);
		}
		this.redAmount = MonoSingleton<PrefsManager>.Instance.GetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".r", this.originalColor.r);
		this.greenAmount = MonoSingleton<PrefsManager>.Instance.GetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".g", this.originalColor.g);
		this.blueAmount = MonoSingleton<PrefsManager>.Instance.GetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".b", this.originalColor.b);
		this.newColor = new Color(this.redAmount, this.greenAmount, this.blueAmount, this.originalColor.a);
		this.colorExample.color = this.newColor;
		if (this.newColor != this.originalColor)
		{
			if (this.variationColor)
			{
				MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationNumber] = this.newColor;
				MonoSingleton<ColorBlindSettings>.Instance.UpdateWeaponColors();
			}
			else if (!this.enemyColor)
			{
				MonoSingleton<ColorBlindSettings>.Instance.SetHudColor(this.hct, this.newColor);
			}
			else
			{
				MonoSingleton<ColorBlindSettings>.Instance.SetEnemyColor(this.ect, this.newColor);
			}
		}
		this.redSlider.value = this.redAmount;
		this.greenSlider.value = this.greenAmount;
		this.blueSlider.value = this.blueAmount;
		this.nameText = base.transform.GetChild(0).GetComponent<TMP_Text>();
		this.originalName = this.nameText.text;
		if (MonoSingleton<BestiaryData>.Instance.GetEnemy(this.ect) < 1 && this.enemyColor)
		{
			this.nameText.text = "???";
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x0001C7B4 File Offset: 0x0001A9B4
	private void UpdateColor()
	{
		Color color = this.newColor;
		if (this.newColor.a == 0f)
		{
			return;
		}
		bool flag = false;
		if (this.newColor.r != this.redAmount)
		{
			this.newColor.r = this.redAmount;
			MonoSingleton<PrefsManager>.Instance.SetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".r", this.redAmount);
			flag = true;
		}
		if (this.newColor.g != this.greenAmount)
		{
			this.newColor.g = this.greenAmount;
			MonoSingleton<PrefsManager>.Instance.SetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".g", this.greenAmount);
			flag = true;
		}
		if (this.newColor.b != this.blueAmount)
		{
			this.newColor.b = this.blueAmount;
			MonoSingleton<PrefsManager>.Instance.SetFloat((this.enemyColor ? "enemyColor." : "hudColor.") + this.name + ".b", this.blueAmount);
			flag = true;
		}
		this.colorExample.color = this.newColor;
		if (flag)
		{
			if (this.variationColor)
			{
				MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationNumber] = this.newColor;
				MonoSingleton<ColorBlindSettings>.Instance.UpdateWeaponColors();
				return;
			}
			if (!this.enemyColor)
			{
				MonoSingleton<ColorBlindSettings>.Instance.SetHudColor(this.hct, this.newColor);
				return;
			}
			MonoSingleton<ColorBlindSettings>.Instance.SetEnemyColor(this.ect, this.newColor);
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001C962 File Offset: 0x0001AB62
	public void ChangeRed(float amount)
	{
		this.redAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001C971 File Offset: 0x0001AB71
	public void ChangeGreen(float amount)
	{
		this.greenAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x0001C980 File Offset: 0x0001AB80
	public void ChangeBlue(float amount)
	{
		this.blueAmount = amount;
		this.UpdateColor();
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x0001C990 File Offset: 0x0001AB90
	public void ResetToDefault()
	{
		this.redAmount = this.originalColor.r;
		this.greenAmount = this.originalColor.g;
		this.blueAmount = this.originalColor.b;
		this.redSlider.value = this.redAmount;
		this.greenSlider.value = this.greenAmount;
		this.blueSlider.value = this.blueAmount;
		this.UpdateColor();
	}

	// Token: 0x04000509 RID: 1289
	private TMP_Text nameText;

	// Token: 0x0400050A RID: 1290
	private string originalName;

	// Token: 0x0400050B RID: 1291
	public new string name;

	// Token: 0x0400050C RID: 1292
	public bool enemyColor;

	// Token: 0x0400050D RID: 1293
	public bool variationColor;

	// Token: 0x0400050E RID: 1294
	public HudColorType hct;

	// Token: 0x0400050F RID: 1295
	public EnemyType ect;

	// Token: 0x04000510 RID: 1296
	public int variationNumber;

	// Token: 0x04000511 RID: 1297
	private Color originalColor;

	// Token: 0x04000512 RID: 1298
	private Color newColor;

	// Token: 0x04000513 RID: 1299
	public Image colorExample;

	// Token: 0x04000514 RID: 1300
	private float redAmount;

	// Token: 0x04000515 RID: 1301
	private float greenAmount;

	// Token: 0x04000516 RID: 1302
	private float blueAmount;

	// Token: 0x04000517 RID: 1303
	public Slider redSlider;

	// Token: 0x04000518 RID: 1304
	public Slider greenSlider;

	// Token: 0x04000519 RID: 1305
	public Slider blueSlider;
}
