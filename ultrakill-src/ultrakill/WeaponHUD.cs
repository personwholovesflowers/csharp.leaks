using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004BF RID: 1215
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class WeaponHUD : MonoSingleton<WeaponHUD>
{
	// Token: 0x06001BE0 RID: 7136 RVA: 0x000E7C38 File Offset: 0x000E5E38
	protected override void Awake()
	{
		base.Awake();
		WeaponIcon weaponIcon = Object.FindObjectOfType<WeaponIcon>();
		if (weaponIcon)
		{
			weaponIcon.UpdateIcon();
		}
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000E7C60 File Offset: 0x000E5E60
	public void UpdateImage(Sprite icon, Sprite glowIcon, int variation)
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		if (this.glowImg == null)
		{
			this.glowImg = base.transform.GetChild(0).GetComponent<Image>();
		}
		this.img.sprite = icon;
		this.img.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[variation];
		this.glowImg.sprite = glowIcon;
		this.glowImg.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[variation];
	}

	// Token: 0x04002756 RID: 10070
	private Image img;

	// Token: 0x04002757 RID: 10071
	private Image glowImg;
}
