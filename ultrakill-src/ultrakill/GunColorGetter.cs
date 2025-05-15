using System;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class GunColorGetter : MonoBehaviour
{
	// Token: 0x06000C46 RID: 3142 RVA: 0x00057704 File Offset: 0x00055904
	private void Awake()
	{
		for (int i = 0; i < this.defaultMaterials.Length; i++)
		{
			this.defaultMaterials[i] = new Material(this.defaultMaterials[i]);
		}
		for (int j = 0; j < this.coloredMaterials.Length; j++)
		{
			this.coloredMaterials[j] = new Material(this.coloredMaterials[j]);
		}
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00057761 File Offset: 0x00055961
	private void OnEnable()
	{
		this.UpdateColor();
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0005776C File Offset: 0x0005596C
	public void UpdateColor()
	{
		if (this.rend == null)
		{
			this.rend = base.GetComponent<Renderer>();
		}
		if (this.customColors == null)
		{
			this.customColors = new MaterialPropertyBlock();
		}
		this.hasCustomColors = MonoSingleton<PrefsManager>.Instance.GetBool("gunColorType." + this.weaponNumber.ToString() + (this.altVersion ? ".a" : ""), false) && MonoSingleton<GunColorController>.Instance.hasUnlockedColors[this.weaponNumber - 1];
		GunColorPreset colors = this.GetColors();
		if (this.currentColors != colors)
		{
			if (this.GetPreset() != 0 || this.hasCustomColors)
			{
				this.rend.materials = this.coloredMaterials;
				this.rend.GetPropertyBlock(this.customColors);
				this.customColors.SetColor("_CustomColor1", colors.color1);
				this.customColors.SetColor("_CustomColor2", colors.color2);
				this.customColors.SetColor("_CustomColor3", colors.color3);
				this.rend.SetPropertyBlock(this.customColors);
			}
			else
			{
				this.rend.materials = this.defaultMaterials;
			}
		}
		this.currentColors = colors;
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000578AC File Offset: 0x00055AAC
	private int GetPreset()
	{
		if (this.weaponNumber <= 0 || this.weaponNumber > 5)
		{
			return 0;
		}
		if (this.altVersion)
		{
			return MonoSingleton<GunColorController>.Instance.altPresets[this.weaponNumber - 1];
		}
		return MonoSingleton<GunColorController>.Instance.presets[this.weaponNumber - 1];
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000578FC File Offset: 0x00055AFC
	private GunColorPreset GetColors()
	{
		if (this.weaponNumber <= 0 || this.weaponNumber > 5)
		{
			return new GunColorPreset(Color.white, Color.white, Color.white);
		}
		if (this.altVersion)
		{
			return MonoSingleton<GunColorController>.Instance.currentAltColors[this.weaponNumber - 1];
		}
		return MonoSingleton<GunColorController>.Instance.currentColors[this.weaponNumber - 1];
	}

	// Token: 0x0400101F RID: 4127
	private Renderer rend;

	// Token: 0x04001020 RID: 4128
	public Material[] defaultMaterials;

	// Token: 0x04001021 RID: 4129
	public Material[] coloredMaterials;

	// Token: 0x04001022 RID: 4130
	private MaterialPropertyBlock customColors;

	// Token: 0x04001023 RID: 4131
	public int weaponNumber;

	// Token: 0x04001024 RID: 4132
	public bool altVersion;

	// Token: 0x04001025 RID: 4133
	private GunColorPreset currentColors = new GunColorPreset(Color.white, Color.white, Color.white);

	// Token: 0x04001026 RID: 4134
	private bool hasCustomColors;
}
