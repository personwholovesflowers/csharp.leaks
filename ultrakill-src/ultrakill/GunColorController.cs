using System;
using UnityEngine;

// Token: 0x0200023A RID: 570
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class GunColorController : MonoSingleton<GunColorController>
{
	// Token: 0x06000C3D RID: 3133 RVA: 0x000572FC File Offset: 0x000554FC
	private void Start()
	{
		this.weaponCount = Enum.GetNames(typeof(GameProgressSaver.WeaponCustomizationType)).Length;
		this.currentColors = new GunColorPreset[this.weaponCount];
		this.currentAltColors = new GunColorPreset[this.weaponCount];
		this.presets = new int[this.weaponCount];
		this.altPresets = new int[this.weaponCount];
		this.hasUnlockedColors = new bool[this.weaponCount];
		this.currentPropBlocks = new MaterialPropertyBlock[this.weaponCount];
		this.currentAltPropBlocks = new MaterialPropertyBlock[this.weaponCount];
		this.UpdateGunColors();
	}

	// Token: 0x06000C3E RID: 3134 RVA: 0x000573A0 File Offset: 0x000555A0
	public void UpdateGunColors()
	{
		for (int i = 0; i < this.weaponCount; i++)
		{
			this.UpdateColor(i, false);
			this.UpdateColor(i, true);
			this.hasUnlockedColors[i] = GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)i);
		}
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x000573DC File Offset: 0x000555DC
	private void UpdateColor(int gunNumber, bool altVersion)
	{
		this.GetColorPresets((GameProgressSaver.WeaponCustomizationType)gunNumber);
		int[] array = (altVersion ? this.altPresets : this.presets);
		string text = (altVersion ? ".a" : "");
		if (MonoSingleton<PrefsManager>.Instance.GetBool("gunColorType." + (gunNumber + 1).ToString() + text, false) && GameProgressSaver.HasWeaponCustomization((GameProgressSaver.WeaponCustomizationType)gunNumber))
		{
			this.SetCustomColors(gunNumber, altVersion, null);
		}
		else
		{
			string text2 = "gunColorPreset." + (gunNumber + 1).ToString() + text;
			array[gunNumber] = MonoSingleton<PrefsManager>.Instance.GetInt(text2, 0);
			if (GameProgressSaver.GetTotalSecretsFound() < GunColorController.requiredSecrets[array[gunNumber]])
			{
				array[gunNumber] = 0;
				MonoSingleton<PrefsManager>.Instance.SetInt(text2, 0);
			}
			this.SetCustomColors(gunNumber, altVersion, array);
		}
		if (altVersion)
		{
			this.altPresets = array;
			return;
		}
		this.presets = array;
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x000574AC File Offset: 0x000556AC
	private void SetCustomColors(int gunNumber, bool altVersion, int[] presetArray = null)
	{
		GunColorPreset gunColorPreset;
		if (presetArray == null)
		{
			gunColorPreset = this.CustomGunColorPreset(gunNumber + 1, altVersion);
		}
		else
		{
			gunColorPreset = this.GetColorPresets((GameProgressSaver.WeaponCustomizationType)gunNumber)[presetArray[gunNumber]];
		}
		if (altVersion)
		{
			this.currentAltColors[gunNumber] = gunColorPreset;
			return;
		}
		this.currentColors[gunNumber] = gunColorPreset;
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x000574EC File Offset: 0x000556EC
	private GunColorPreset[] GetColorPresets(GameProgressSaver.WeaponCustomizationType weaponType)
	{
		switch (weaponType)
		{
		case GameProgressSaver.WeaponCustomizationType.Revolver:
			return this.revolverColors;
		case GameProgressSaver.WeaponCustomizationType.Shotgun:
			return this.shotgunColors;
		case GameProgressSaver.WeaponCustomizationType.Nailgun:
			return this.nailgunColors;
		case GameProgressSaver.WeaponCustomizationType.Railcannon:
			return this.railcannonColors;
		case GameProgressSaver.WeaponCustomizationType.RocketLauncher:
			return this.rocketLauncherColors;
		default:
			Debug.LogError(string.Format("Invalid WeaponCustomizationType: {0}", weaponType));
			return null;
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x0005754E File Offset: 0x0005574E
	private GunColorPreset CustomGunColorPreset(int gunNumber, bool altVersion)
	{
		return new GunColorPreset(this.GetGunColor(1, gunNumber, altVersion), this.GetGunColor(2, gunNumber, altVersion), this.GetGunColor(3, gunNumber, altVersion));
	}

	// Token: 0x06000C43 RID: 3139 RVA: 0x00057570 File Offset: 0x00055770
	private Color GetGunColor(int number, int gunNumber, bool altVersion)
	{
		return new Color(MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			gunNumber.ToString(),
			".",
			number.ToString(),
			altVersion ? ".a" : ".",
			"r"
		}), 1f), MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			gunNumber.ToString(),
			".",
			number.ToString(),
			altVersion ? ".a" : ".",
			"g"
		}), 1f), MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			gunNumber.ToString(),
			".",
			number.ToString(),
			altVersion ? ".a" : ".",
			"b"
		}), 1f), MonoSingleton<PrefsManager>.Instance.GetFloat(string.Concat(new string[]
		{
			"gunColor.",
			gunNumber.ToString(),
			".",
			number.ToString(),
			altVersion ? ".a" : ".",
			"a"
		}), 0f));
	}

	// Token: 0x04001011 RID: 4113
	public static int[] requiredSecrets = new int[] { 0, 10, 25, 50, 100 };

	// Token: 0x04001012 RID: 4114
	public GunColorPreset[] revolverColors;

	// Token: 0x04001013 RID: 4115
	public GunColorPreset[] shotgunColors;

	// Token: 0x04001014 RID: 4116
	public GunColorPreset[] nailgunColors;

	// Token: 0x04001015 RID: 4117
	public GunColorPreset[] railcannonColors;

	// Token: 0x04001016 RID: 4118
	public GunColorPreset[] rocketLauncherColors;

	// Token: 0x04001017 RID: 4119
	[HideInInspector]
	public GunColorPreset[] currentColors;

	// Token: 0x04001018 RID: 4120
	[HideInInspector]
	public GunColorPreset[] currentAltColors;

	// Token: 0x04001019 RID: 4121
	[HideInInspector]
	public int[] presets;

	// Token: 0x0400101A RID: 4122
	[HideInInspector]
	public int[] altPresets;

	// Token: 0x0400101B RID: 4123
	[HideInInspector]
	public bool[] hasUnlockedColors;

	// Token: 0x0400101C RID: 4124
	[HideInInspector]
	public MaterialPropertyBlock[] currentPropBlocks;

	// Token: 0x0400101D RID: 4125
	[HideInInspector]
	public MaterialPropertyBlock[] currentAltPropBlocks;

	// Token: 0x0400101E RID: 4126
	[HideInInspector]
	public int weaponCount;
}
