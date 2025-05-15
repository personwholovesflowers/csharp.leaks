using System;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000251 RID: 593
public class HudController : MonoBehaviour
{
	// Token: 0x06000D01 RID: 3329 RVA: 0x000632AC File Offset: 0x000614AC
	private void Awake()
	{
		if (!this.altHud && !HudController.Instance)
		{
			HudController.Instance = this;
		}
		if (this.altHud && this.altHudObj == null)
		{
			this.altHudObj = base.transform.GetChild(0).gameObject;
		}
		if (!this.altHud && this.hudpos == null)
		{
			this.hudpos = this.gunCanvas.GetComponent<HUDPos>();
		}
		MonoSingleton<FistControl>.Instance.FistIconUpdated += this.UpdateFistIcon;
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0006333D File Offset: 0x0006153D
	private void OnDestroy()
	{
		if (MonoSingleton<FistControl>.Instance)
		{
			MonoSingleton<FistControl>.Instance.FistIconUpdated -= this.UpdateFistIcon;
		}
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00063364 File Offset: 0x00061564
	private void UpdateFistIcon(int current)
	{
		this.fistFill.sprite = this.fistIcons[current];
		this.fistBackground.sprite = this.fistIcons[current];
		int num;
		if (current != 1)
		{
			if (current != 2)
			{
				num = current;
			}
			else
			{
				num = 1;
			}
		}
		else
		{
			num = 2;
		}
		int num2 = num;
		MonoSingleton<FistControl>.Instance.fistIconColor = MonoSingleton<ColorBlindSettings>.Instance.variationColors[num2];
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x000633C7 File Offset: 0x000615C7
	private void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x000633E9 File Offset: 0x000615E9
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0006340C File Offset: 0x0006160C
	private void OnPrefChanged(string key, object value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
		if (num <= 2505550477U)
		{
			if (num != 970472614U)
			{
				if (num != 2250332297U)
				{
					if (num != 2505550477U)
					{
						return;
					}
					if (!(key == "weaponIcons"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag = (bool)value;
						this.SetWeaponIcons(flag);
						return;
					}
				}
				else
				{
					if (!(key == "armIcons"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag2 = (bool)value;
						this.SetArmIcons(flag2);
						return;
					}
				}
			}
			else
			{
				if (!(key == "styleInfo"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag3 = (bool)value;
					bool? flag4 = new bool?(flag3);
					this.SetStyleVisibleTemp(null, flag4);
				}
			}
		}
		else if (num <= 2920402583U)
		{
			if (num != 2688740182U)
			{
				if (num != 2920402583U)
				{
					return;
				}
				if (!(key == "hudBackgroundOpacity"))
				{
					return;
				}
				if (value is float)
				{
					float num2 = (float)value;
					this.SetOpacity(num2);
					return;
				}
			}
			else
			{
				if (!(key == "hudType"))
				{
					return;
				}
				this.CheckSituation();
				return;
			}
		}
		else if (num != 3192964159U)
		{
			if (num != 3240462473U)
			{
				return;
			}
			if (!(key == "hudAlwaysOnTop"))
			{
				return;
			}
			if (value is bool)
			{
				bool flag5 = (bool)value;
				this.SetAlwaysOnTop(flag5);
				return;
			}
		}
		else
		{
			if (!(key == "styleMeter"))
			{
				return;
			}
			if (value is bool)
			{
				bool flag6 = (bool)value;
				bool? flag7 = new bool?(flag6);
				bool? flag4 = null;
				this.SetStyleVisibleTemp(flag7, flag4);
				return;
			}
		}
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x000635A4 File Offset: 0x000617A4
	private void SetWeaponIcons(bool showIcons)
	{
		if (!this.altHud)
		{
			this.weaponIcon.SetActive(showIcons);
			Vector3 localPosition = this.weaponIcon.transform.localPosition;
			this.weaponIcon.transform.localPosition = new Vector3(localPosition.x, localPosition.y, (float)(showIcons ? 45 : (-9999)));
			Vector2 vector = (showIcons ? new Vector2(-79f, 590f) : new Vector2(-79f, 190f));
			this.speedometer.rect.anchoredPosition = vector;
			return;
		}
		this.weaponIcon.SetActive(showIcons);
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0006364C File Offset: 0x0006184C
	private void SetArmIcons(bool showIcons)
	{
		if (!this.altHud)
		{
			Vector3 localPosition = this.armIcon.transform.localPosition;
			this.armIcon.transform.localPosition = new Vector3(localPosition.x, localPosition.y, (float)(showIcons ? 0 : (-9999)));
			return;
		}
		this.armIcon.SetActive(showIcons);
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x000636AC File Offset: 0x000618AC
	public void SetStyleVisibleTemp(bool? meterVisible = null, bool? infoVisible = null)
	{
		if (this.altHud)
		{
			return;
		}
		if (HideUI.Active)
		{
			meterVisible = new bool?(false);
			infoVisible = new bool?(false);
		}
		else
		{
			bool flag = meterVisible.GetValueOrDefault();
			if (meterVisible == null)
			{
				flag = MonoSingleton<PrefsManager>.Instance.GetBool("styleMeter", false);
				meterVisible = new bool?(flag);
			}
			flag = infoVisible.GetValueOrDefault();
			if (infoVisible == null)
			{
				flag = MonoSingleton<PrefsManager>.Instance.GetBool("styleInfo", false);
				infoVisible = new bool?(flag);
			}
		}
		this.styleMeter.transform.localPosition = new Vector3(this.styleMeter.transform.localPosition.x, this.styleMeter.transform.localPosition.y, (float)(meterVisible.Value ? 0 : (-9999)));
		this.styleInfo.transform.localPosition = new Vector3(this.styleInfo.transform.localPosition.x, this.styleInfo.transform.localPosition.y, (float)(infoVisible.Value ? 0 : (-9999)));
		MonoSingleton<StyleHUD>.Instance.GetComponent<AudioSource>().enabled = infoVisible.Value;
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x000637E8 File Offset: 0x000619E8
	private void Update()
	{
		float punchStamina = MonoSingleton<WeaponCharges>.Instance.punchStamina;
		Color fistIconColor = MonoSingleton<FistControl>.Instance.fistIconColor;
		this.fistFill.fillAmount = punchStamina / 2f;
		this.fistFill.color = ((punchStamina >= 1f) ? fistIconColor : (fistIconColor * new Color(0.6f, 0.6f, 0.6f, 1f)));
		this.fistBackground.color = fistIconColor * new Color(0.2f, 0.2f, 0.2f, 1f);
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0006387C File Offset: 0x00061A7C
	private void Start()
	{
		if (MapInfoBase.Instance.hideStockHUD)
		{
			this.weaponIcon.SetActive(false);
			this.armIcon.SetActive(false);
			return;
		}
		this.CheckSituation();
		if (!MonoSingleton<PrefsManager>.Instance.GetBool("weaponIcons", false))
		{
			if (!this.altHud)
			{
				this.speedometer.rect.anchoredPosition = new Vector2(-79f, 190f);
				this.weaponIcon.transform.localPosition = new Vector3(this.weaponIcon.transform.localPosition.x, this.weaponIcon.transform.localPosition.y, 45f);
			}
			else
			{
				this.weaponIcon.SetActive(false);
			}
		}
		if (!MonoSingleton<PrefsManager>.Instance.GetBool("armIcons", false))
		{
			if (!this.altHud)
			{
				this.armIcon.transform.localPosition = new Vector3(this.armIcon.transform.localPosition.x, this.armIcon.transform.localPosition.y, 0f);
			}
			else
			{
				this.armIcon.SetActive(false);
			}
		}
		if (!this.altHud)
		{
			if (!MonoSingleton<PrefsManager>.Instance.GetBool("styleMeter", false))
			{
				this.styleMeter.transform.localPosition = new Vector3(this.styleMeter.transform.localPosition.x, this.styleMeter.transform.localPosition.y, -9999f);
			}
			if (!MonoSingleton<PrefsManager>.Instance.GetBool("styleInfo", false))
			{
				this.styleInfo.transform.localPosition = new Vector3(this.styleInfo.transform.localPosition.x, this.styleInfo.transform.localPosition.y, -9999f);
				MonoSingleton<StyleHUD>.Instance.GetComponent<AudioSource>().enabled = false;
			}
		}
		float @float = MonoSingleton<PrefsManager>.Instance.GetFloat("hudBackgroundOpacity", 0f);
		if (@float != 50f)
		{
			this.SetOpacity(@float);
		}
		this.SetAlwaysOnTop(MonoSingleton<PrefsManager>.Instance.GetBool("hudAlwaysOnTop", false));
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x00063AAC File Offset: 0x00061CAC
	public void CheckSituation()
	{
		if (HideUI.Active)
		{
			if (this.gunCanvas)
			{
				this.gunCanvas.GetComponent<Canvas>().enabled = false;
			}
			if (this.altHudObj)
			{
				this.altHudObj.SetActive(false);
			}
			return;
		}
		if (this.altHud)
		{
			if (this.altHudObj)
			{
				if (MonoSingleton<PrefsManager>.Instance.GetInt("hudType", 0) == 2 && !this.colorless)
				{
					this.altHudObj.SetActive(true);
				}
				else if (MonoSingleton<PrefsManager>.Instance.GetInt("hudType", 0) == 3 && this.colorless)
				{
					this.altHudObj.SetActive(true);
				}
				else
				{
					this.altHudObj.SetActive(false);
				}
			}
			MonoSingleton<PrefsManager>.Instance.GetBool("speedometer", false);
			return;
		}
		if (MonoSingleton<PrefsManager>.Instance.GetInt("hudType", 0) != 1)
		{
			if (this.gunCanvas == null)
			{
				this.gunCanvas = base.transform.Find("GunCanvas").gameObject;
			}
			if (this.hudpos == null)
			{
				this.hudpos = this.gunCanvas.GetComponent<HUDPos>();
			}
			this.gunCanvas.transform.localPosition = new Vector3(this.gunCanvas.transform.localPosition.x, this.gunCanvas.transform.localPosition.y, -100f);
			this.gunCanvas.GetComponent<Canvas>().enabled = false;
			if (this.hudpos)
			{
				this.hudpos.active = false;
				return;
			}
		}
		else
		{
			if (this.gunCanvas == null)
			{
				this.gunCanvas = base.transform.Find("GunCanvas").gameObject;
			}
			if (this.hudpos == null)
			{
				this.hudpos = this.gunCanvas.GetComponent<HUDPos>();
			}
			this.gunCanvas.GetComponent<Canvas>().enabled = true;
			this.gunCanvas.transform.localPosition = new Vector3(this.gunCanvas.transform.localPosition.x, this.gunCanvas.transform.localPosition.y, 1f);
			if (this.hudpos)
			{
				this.hudpos.active = true;
				this.hudpos.CheckPos();
			}
		}
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x00063D10 File Offset: 0x00061F10
	public void SetOpacity(float amount)
	{
		foreach (Image image in this.hudBackgrounds)
		{
			if (image)
			{
				Color color = image.color;
				color.a = amount / 100f;
				image.color = color;
			}
		}
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00063D5C File Offset: 0x00061F5C
	public void SetAlwaysOnTop(bool onTop)
	{
		if (this.textElements == null)
		{
			return;
		}
		TMP_Text[] array = this.textElements;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].fontSharedMaterial = (onTop ? this.overlayTextMaterial : this.normalTextMaterial);
		}
	}

	// Token: 0x04001182 RID: 4482
	public static HudController Instance;

	// Token: 0x04001183 RID: 4483
	public bool altHud;

	// Token: 0x04001184 RID: 4484
	public bool colorless;

	// Token: 0x04001185 RID: 4485
	private GameObject altHudObj;

	// Token: 0x04001186 RID: 4486
	private HUDPos hudpos;

	// Token: 0x04001187 RID: 4487
	public GameObject gunCanvas;

	// Token: 0x04001188 RID: 4488
	public GameObject weaponIcon;

	// Token: 0x04001189 RID: 4489
	public GameObject armIcon;

	// Token: 0x0400118A RID: 4490
	public Sprite[] fistIcons;

	// Token: 0x0400118B RID: 4491
	public Image fistFill;

	// Token: 0x0400118C RID: 4492
	public Image fistBackground;

	// Token: 0x0400118D RID: 4493
	public GameObject styleMeter;

	// Token: 0x0400118E RID: 4494
	public GameObject styleInfo;

	// Token: 0x0400118F RID: 4495
	public Speedometer speedometer;

	// Token: 0x04001190 RID: 4496
	[Space]
	public Image[] hudBackgrounds;

	// Token: 0x04001191 RID: 4497
	public TMP_Text[] textElements;

	// Token: 0x04001192 RID: 4498
	[Space]
	public Material normalTextMaterial;

	// Token: 0x04001193 RID: 4499
	public Material overlayTextMaterial;
}
