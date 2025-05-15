using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x020004C0 RID: 1216
public class WeaponIcon : MonoBehaviour
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x000E7D02 File Offset: 0x000E5F02
	private int variationColor
	{
		get
		{
			if (!(this.weaponDescriptor == null))
			{
				return (int)this.weaponDescriptor.variationColor;
			}
			return -1;
		}
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x000E7D1F File Offset: 0x000E5F1F
	private void Start()
	{
		this.UpdateIcon();
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000E7D27 File Offset: 0x000E5F27
	private void OnEnable()
	{
		MonoSingleton<GunControl>.Instance.currentWeaponIcons.Add(this);
		this.UpdateIcon();
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000E7D40 File Offset: 0x000E5F40
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<GunControl>.Instance.currentWeaponIcons.Remove(this);
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x000E7D74 File Offset: 0x000E5F74
	private void OnDestroy()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<GunControl>.Instance.currentWeaponIcons.Remove(this);
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x000E7DA8 File Offset: 0x000E5FA8
	public void UpdateIcon()
	{
		if (MonoSingleton<WeaponHUD>.Instance)
		{
			MonoSingleton<WeaponHUD>.Instance.UpdateImage(this.weaponDescriptor.icon, this.weaponDescriptor.glowIcon, this.variationColor);
		}
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		foreach (Renderer renderer in this.variationColoredRenderers)
		{
			renderer.GetPropertyBlock(materialPropertyBlock);
			if (renderer.sharedMaterial.HasProperty("_EmissiveColor"))
			{
				materialPropertyBlock.SetColor("_EmissiveColor", MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationColor]);
			}
			else
			{
				materialPropertyBlock.SetColor("_Color", MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationColor]);
			}
			renderer.SetPropertyBlock(materialPropertyBlock);
		}
		foreach (Image image in this.variationColoredImages)
		{
			image.color = new Color(MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationColor].r, MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationColor].g, MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.variationColor].b, image.color.a);
		}
	}

	// Token: 0x04002758 RID: 10072
	[FormerlySerializedAs("descriptor")]
	public WeaponDescriptor weaponDescriptor;

	// Token: 0x04002759 RID: 10073
	[SerializeField]
	private Renderer[] variationColoredRenderers;

	// Token: 0x0400275A RID: 10074
	[SerializeField]
	private Image[] variationColoredImages;
}
