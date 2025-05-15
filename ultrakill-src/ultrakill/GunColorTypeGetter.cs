using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023E RID: 574
public class GunColorTypeGetter : MonoBehaviour
{
	// Token: 0x06000C57 RID: 3159 RVA: 0x000582C8 File Offset: 0x000564C8
	private void Awake()
	{
		this.templateButtonsImages = new Image[this.templateButtons.Count];
		for (int i = 0; i < this.templateButtons.Count; i++)
		{
			this.templateButtonsImages[i] = this.templateButtons[i].GetComponent<Image>();
		}
		this.templateTexts = new TMP_Text[this.templateButtons.Count];
		for (int j = 0; j < this.templateButtons.Count; j++)
		{
			this.templateTexts[j] = this.templateButtons[j].GetComponentInChildren<TMP_Text>();
		}
		for (int k = 0; k < this.templateButtons.Count; k++)
		{
			int index = k;
			this.templateButtons[k].GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetPreset(index);
			};
		}
		this.originalTemplateTexts = new string[this.templateTexts.Length];
		for (int l = 0; l < this.templateTexts.Length; l++)
		{
			this.originalTemplateTexts[l] = this.templateTexts[l].text;
		}
		this.presetsButton.GetComponent<ShopButton>().PointerClickSuccess += delegate
		{
			this.SetType(false);
		};
		this.customButton.GetComponent<ShopButton>().PointerClickSuccess += delegate
		{
			this.SetType(true);
		};
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00058424 File Offset: 0x00056624
	private void OnEnable()
	{
		this.SetType(MonoSingleton<PrefsManager>.Instance.GetBool("gunColorType." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), false) && GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)(this.weaponNumber - 1)));
		if (this.altButton)
		{
			string text = "";
			switch (this.weaponNumber)
			{
			case 1:
				text = "revalt";
				break;
			case 2:
				text = "shoalt";
				break;
			case 3:
				text = "naialt";
				break;
			}
			if (GameProgressSaver.CheckGear(text) >= 1)
			{
				this.altButton.gameObject.SetActive(true);
			}
			else
			{
				this.altButton.gameObject.SetActive(false);
			}
		}
		this.SetPreset(MonoSingleton<PrefsManager>.Instance.GetInt("gunColorPreset." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), 0));
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0005852C File Offset: 0x0005672C
	public void SetType(bool isCustom)
	{
		bool flag = GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)(this.weaponNumber - 1));
		MonoSingleton<PrefsManager>.Instance.SetBool("gunColorType." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), isCustom && flag);
		MonoSingleton<GunColorController>.Instance.UpdateGunColors();
		this.template.SetActive(!isCustom);
		this.presetsButton.interactable = isCustom;
		this.presetsButton.GetComponent<ShopButton>().deactivated = !isCustom;
		this.custom.SetActive(isCustom);
		this.customButton.interactable = !isCustom;
		this.customButton.GetComponent<ShopButton>().deactivated = isCustom;
		this.UpdatePreview();
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x000585EC File Offset: 0x000567EC
	public void SetPreset(int index)
	{
		int totalSecretsFound = GameProgressSaver.GetTotalSecretsFound();
		MonoSingleton<PrefsManager>.Instance.SetInt("gunColorPreset." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), index);
		MonoSingleton<GunColorController>.Instance.UpdateGunColors();
		for (int i = 0; i < this.templateButtons.Count; i++)
		{
			int num = GunColorController.requiredSecrets[i];
			bool flag = totalSecretsFound >= num;
			ShopButton component = this.templateButtons[i].GetComponent<ShopButton>();
			if (flag)
			{
				this.templateTexts[i].SetText(this.originalTemplateTexts[i], true);
				this.templateTexts[i].color = Color.white;
				this.templateButtonsImages[i].color = Color.white;
				component.failure = false;
				if (i == index)
				{
					this.templateButtons[i].interactable = false;
					component.deactivated = true;
				}
				else
				{
					this.templateButtons[i].interactable = true;
					component.deactivated = false;
				}
			}
			else
			{
				this.templateTexts[i].SetText("SOUL ORBS: " + totalSecretsFound.ToString() + " / " + num.ToString(), true);
				this.templateTexts[i].color = Color.red;
				this.templateButtonsImages[i].color = Color.red;
				this.templateButtons[i].interactable = false;
				component.failure = true;
				component.deactivated = false;
			}
		}
		this.UpdatePreview();
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00058770 File Offset: 0x00056970
	public void UpdatePreview()
	{
		GunColorGetter[] array;
		if (!this.altVersion)
		{
			array = this.previewColorGetterStandard;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UpdateColor();
			}
			return;
		}
		array = this.previewColorGetterAlt;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateColor();
		}
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x000587C0 File Offset: 0x000569C0
	public void ToggleAlternate()
	{
		this.altVersion = !this.altVersion;
		this.altButton.GetComponentInChildren<TMP_Text>().SetText(this.altVersion ? "Standard" : "Alternate", true);
		this.previewModelStandard.SetActive(!this.altVersion);
		this.previewModelAlt.SetActive(this.altVersion);
		GunColorSetter[] array = this.gunColorSetters;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateSliders();
		}
		this.SetType(MonoSingleton<PrefsManager>.Instance.GetBool("gunColorType." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), false) && GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)(this.weaponNumber - 1)));
		this.SetPreset(MonoSingleton<PrefsManager>.Instance.GetInt("gunColorPreset." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), 0));
	}

	// Token: 0x0400103B RID: 4155
	public int weaponNumber;

	// Token: 0x0400103C RID: 4156
	public bool altVersion;

	// Token: 0x0400103D RID: 4157
	public GameObject template;

	// Token: 0x0400103E RID: 4158
	public GameObject custom;

	// Token: 0x0400103F RID: 4159
	public Button altButton;

	// Token: 0x04001040 RID: 4160
	public Button presetsButton;

	// Token: 0x04001041 RID: 4161
	public Button customButton;

	// Token: 0x04001042 RID: 4162
	public GameObject previewModelStandard;

	// Token: 0x04001043 RID: 4163
	public GunColorGetter[] previewColorGetterStandard;

	// Token: 0x04001044 RID: 4164
	public GameObject previewModelAlt;

	// Token: 0x04001045 RID: 4165
	public GunColorGetter[] previewColorGetterAlt;

	// Token: 0x04001046 RID: 4166
	public List<Button> templateButtons;

	// Token: 0x04001047 RID: 4167
	private Image[] templateButtonsImages;

	// Token: 0x04001048 RID: 4168
	private TMP_Text[] templateTexts;

	// Token: 0x04001049 RID: 4169
	private string[] originalTemplateTexts;

	// Token: 0x0400104A RID: 4170
	public GunColorSetter[] gunColorSetters;
}
