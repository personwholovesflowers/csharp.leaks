using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000241 RID: 577
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class GunSetter : MonoSingleton<GunSetter>
{
	// Token: 0x06000C77 RID: 3191 RVA: 0x0005A530 File Offset: 0x00058730
	private void Prewarm(AssetReference[] prefabs)
	{
		for (int i = 0; i < prefabs.Length; i++)
		{
			AssetHelper.LoadPrefab(prefabs[i]);
		}
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private new void Awake()
	{
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0005A556 File Offset: 0x00058756
	private void Start()
	{
		this.gunc = base.GetComponent<GunControl>();
		if (base.enabled)
		{
			this.ResetWeapons(true);
		}
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0005A574 File Offset: 0x00058774
	public void ResetWeapons(bool firstTime = false)
	{
		if (this.gunc == null)
		{
			this.gunc = base.GetComponent<GunControl>();
		}
		int num = 5;
		for (int i = 0; i < Mathf.Min(this.gunc.slots.Count, num); i++)
		{
			List<GameObject> list = this.gunc.slots[i];
			foreach (GameObject gameObject in list)
			{
				Object.Destroy(gameObject);
			}
			list.Clear();
		}
		List<int> list2 = this.CheckWeaponOrder("rev");
		for (int j = 1; j < 5; j++)
		{
			switch (list2.IndexOf(j))
			{
			case 0:
				this.CheckWeapon("rev0", this.gunc.slot1, this.revolverPierce.ToAssets());
				break;
			case 1:
				this.CheckWeapon("rev1", this.gunc.slot1, this.revolverTwirl.ToAssets());
				break;
			case 2:
				this.CheckWeapon("rev2", this.gunc.slot1, this.revolverRicochet.ToAssets());
				break;
			}
		}
		list2 = this.CheckWeaponOrder("sho");
		for (int k = 1; k < 5; k++)
		{
			switch (list2.IndexOf(k))
			{
			case 0:
				this.CheckWeapon("sho0", this.gunc.slot2, this.shotgunGrenade.ToAssets());
				break;
			case 1:
				this.CheckWeapon("sho1", this.gunc.slot2, this.shotgunPump.ToAssets());
				break;
			case 2:
				this.CheckWeapon("sho2", this.gunc.slot2, this.shotgunRed.ToAssets());
				break;
			}
		}
		list2 = this.CheckWeaponOrder("nai");
		for (int l = 1; l < 5; l++)
		{
			switch (list2.IndexOf(l))
			{
			case 0:
				this.CheckWeapon("nai0", this.gunc.slot3, this.nailMagnet.ToAssets());
				break;
			case 1:
				this.CheckWeapon("nai1", this.gunc.slot3, this.nailOverheat.ToAssets());
				break;
			case 2:
				this.CheckWeapon("nai2", this.gunc.slot3, this.nailRed.ToAssets());
				break;
			}
		}
		list2 = this.CheckWeaponOrder("rai");
		for (int m = 1; m < 5; m++)
		{
			switch (list2.IndexOf(m))
			{
			case 0:
				this.CheckWeapon("rai0", this.gunc.slot4, this.railCannon.ToAssets());
				break;
			case 1:
				this.CheckWeapon("rai1", this.gunc.slot4, this.railHarpoon.ToAssets());
				break;
			case 2:
				this.CheckWeapon("rai2", this.gunc.slot4, this.railMalicious.ToAssets());
				break;
			}
		}
		list2 = this.CheckWeaponOrder("rock");
		for (int n = 1; n < 5; n++)
		{
			switch (list2.IndexOf(n))
			{
			case 0:
				this.CheckWeapon("rock0", this.gunc.slot5, this.rocketBlue.ToAssets());
				break;
			case 1:
				this.CheckWeapon("rock1", this.gunc.slot5, this.rocketGreen.ToAssets());
				break;
			case 2:
				this.CheckWeapon("rock2", this.gunc.slot5, this.rocketRed.ToAssets());
				break;
			}
		}
		this.gunc.UpdateWeaponList(firstTime);
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0005A96C File Offset: 0x00058B6C
	private List<int> CheckWeaponOrder(string weaponType)
	{
		string text = "1234";
		if (weaponType == "rev")
		{
			text = "1324";
		}
		string text2 = MonoSingleton<PrefsManager>.Instance.GetString("weapon." + weaponType + ".order", text);
		if (text2.Length != 4)
		{
			Debug.LogError("Faulty WeaponOrder: " + weaponType);
			text2 = text;
			MonoSingleton<PrefsManager>.Instance.SetString("weapon." + weaponType + ".order", text);
		}
		List<int> list = new List<int>();
		for (int i = 0; i < text2.Length; i++)
		{
			list.Add((int)(text2[i] - '0'));
		}
		return list;
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0005AA0C File Offset: 0x00058C0C
	private void CheckWeapon(string name, List<GameObject> slot, GameObject[] prefabs)
	{
		if (prefabs == null || prefabs.Length == 0)
		{
			return;
		}
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + name, 1);
		VariantOption variantOption = VariantOption.IfEquipped;
		VariantOption variantOption2 = VariantOption.IfEquipped;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(name);
		if (num <= 1732737622U)
		{
			if (num <= 432292697U)
			{
				if (num != 381959840U)
				{
					if (num != 415515078U)
					{
						if (num == 432292697U)
						{
							if (name == "rev1")
							{
								ForcedLoadout forcedLoadout = this.forcedLoadout;
								variantOption = ((forcedLoadout != null) ? forcedLoadout.revolver.redVariant : VariantOption.IfEquipped);
								ForcedLoadout forcedLoadout2 = this.forcedLoadout;
								variantOption2 = ((forcedLoadout2 != null) ? forcedLoadout2.altRevolver.redVariant : VariantOption.IfEquipped);
							}
						}
					}
					else if (name == "rev0")
					{
						ForcedLoadout forcedLoadout3 = this.forcedLoadout;
						variantOption = ((forcedLoadout3 != null) ? forcedLoadout3.revolver.blueVariant : VariantOption.IfEquipped);
						ForcedLoadout forcedLoadout4 = this.forcedLoadout;
						variantOption2 = ((forcedLoadout4 != null) ? forcedLoadout4.altRevolver.blueVariant : VariantOption.IfEquipped);
					}
				}
				else if (name == "rev2")
				{
					ForcedLoadout forcedLoadout5 = this.forcedLoadout;
					variantOption = ((forcedLoadout5 != null) ? forcedLoadout5.revolver.greenVariant : VariantOption.IfEquipped);
					ForcedLoadout forcedLoadout6 = this.forcedLoadout;
					variantOption2 = ((forcedLoadout6 != null) ? forcedLoadout6.altRevolver.greenVariant : VariantOption.IfEquipped);
				}
			}
			else if (num <= 1715960003U)
			{
				if (num != 1705213223U)
				{
					if (num == 1715960003U)
					{
						if (name == "sho2")
						{
							ForcedLoadout forcedLoadout7 = this.forcedLoadout;
							variantOption = ((forcedLoadout7 != null) ? forcedLoadout7.shotgun.redVariant : VariantOption.IfEquipped);
							ForcedLoadout forcedLoadout8 = this.forcedLoadout;
							variantOption2 = ((forcedLoadout8 != null) ? forcedLoadout8.altShotgun.redVariant : VariantOption.IfEquipped);
						}
					}
				}
				else if (name == "rai2")
				{
					ForcedLoadout forcedLoadout9 = this.forcedLoadout;
					variantOption = ((forcedLoadout9 != null) ? forcedLoadout9.railcannon.redVariant : VariantOption.IfEquipped);
				}
			}
			else if (num != 1721990842U)
			{
				if (num == 1732737622U)
				{
					if (name == "sho1")
					{
						ForcedLoadout forcedLoadout10 = this.forcedLoadout;
						variantOption = ((forcedLoadout10 != null) ? forcedLoadout10.shotgun.greenVariant : VariantOption.IfEquipped);
						ForcedLoadout forcedLoadout11 = this.forcedLoadout;
						variantOption2 = ((forcedLoadout11 != null) ? forcedLoadout11.altShotgun.greenVariant : VariantOption.IfEquipped);
					}
				}
			}
			else if (name == "rai1")
			{
				ForcedLoadout forcedLoadout12 = this.forcedLoadout;
				variantOption = ((forcedLoadout12 != null) ? forcedLoadout12.railcannon.greenVariant : VariantOption.IfEquipped);
			}
		}
		else if (num <= 3912221022U)
		{
			if (num <= 1749515241U)
			{
				if (num != 1738768461U)
				{
					if (num == 1749515241U)
					{
						if (name == "sho0")
						{
							ForcedLoadout forcedLoadout13 = this.forcedLoadout;
							variantOption = ((forcedLoadout13 != null) ? forcedLoadout13.shotgun.blueVariant : VariantOption.IfEquipped);
							ForcedLoadout forcedLoadout14 = this.forcedLoadout;
							variantOption2 = ((forcedLoadout14 != null) ? forcedLoadout14.altShotgun.blueVariant : VariantOption.IfEquipped);
						}
					}
				}
				else if (name == "rai0")
				{
					ForcedLoadout forcedLoadout15 = this.forcedLoadout;
					variantOption = ((forcedLoadout15 != null) ? forcedLoadout15.railcannon.blueVariant : VariantOption.IfEquipped);
				}
			}
			else if (num != 3895443403U)
			{
				if (num == 3912221022U)
				{
					if (name == "nai1")
					{
						ForcedLoadout forcedLoadout16 = this.forcedLoadout;
						variantOption = ((forcedLoadout16 != null) ? forcedLoadout16.nailgun.greenVariant : VariantOption.IfEquipped);
						ForcedLoadout forcedLoadout17 = this.forcedLoadout;
						variantOption2 = ((forcedLoadout17 != null) ? forcedLoadout17.altNailgun.greenVariant : VariantOption.IfEquipped);
					}
				}
			}
			else if (name == "nai2")
			{
				ForcedLoadout forcedLoadout18 = this.forcedLoadout;
				variantOption = ((forcedLoadout18 != null) ? forcedLoadout18.nailgun.redVariant : VariantOption.IfEquipped);
				ForcedLoadout forcedLoadout19 = this.forcedLoadout;
				variantOption2 = ((forcedLoadout19 != null) ? forcedLoadout19.altNailgun.redVariant : VariantOption.IfEquipped);
			}
		}
		else if (num <= 4244000204U)
		{
			if (num != 3928998641U)
			{
				if (num == 4244000204U)
				{
					if (name == "rock0")
					{
						ForcedLoadout forcedLoadout20 = this.forcedLoadout;
						variantOption = ((forcedLoadout20 != null) ? forcedLoadout20.rocketLauncher.blueVariant : VariantOption.IfEquipped);
					}
				}
			}
			else if (name == "nai0")
			{
				ForcedLoadout forcedLoadout21 = this.forcedLoadout;
				variantOption = ((forcedLoadout21 != null) ? forcedLoadout21.nailgun.blueVariant : VariantOption.IfEquipped);
				ForcedLoadout forcedLoadout22 = this.forcedLoadout;
				variantOption2 = ((forcedLoadout22 != null) ? forcedLoadout22.altNailgun.blueVariant : VariantOption.IfEquipped);
			}
		}
		else if (num != 4260777823U)
		{
			if (num == 4277555442U)
			{
				if (name == "rock2")
				{
					ForcedLoadout forcedLoadout23 = this.forcedLoadout;
					variantOption = ((forcedLoadout23 != null) ? forcedLoadout23.rocketLauncher.redVariant : VariantOption.IfEquipped);
				}
			}
		}
		else if (name == "rock1")
		{
			ForcedLoadout forcedLoadout24 = this.forcedLoadout;
			variantOption = ((forcedLoadout24 != null) ? forcedLoadout24.rocketLauncher.greenVariant : VariantOption.IfEquipped);
		}
		if (variantOption != VariantOption.IfEquipped)
		{
			if (variantOption == VariantOption.ForceOn && prefabs[0] != null)
			{
				slot.Add(Object.Instantiate<GameObject>(prefabs[0], base.transform));
			}
		}
		else if (@int > 0 && GameProgressSaver.CheckGear(name) > 0 && (@int <= 1 || prefabs.Length < @int || GameProgressSaver.CheckGear(name.Substring(0, name.Length - 1) + "alt") <= 0) && prefabs[0] != null)
		{
			slot.Add(Object.Instantiate<GameObject>(prefabs[0], base.transform));
		}
		if (variantOption2 != VariantOption.IfEquipped)
		{
			if (variantOption2 == VariantOption.ForceOn && prefabs.Length >= 2)
			{
				slot.Add(Object.Instantiate<GameObject>(prefabs[1], base.transform));
				return;
			}
		}
		else if (@int > 0 && GameProgressSaver.CheckGear(name) > 0 && @int > 1 && prefabs.Length >= @int && GameProgressSaver.CheckGear(name.Substring(0, name.Length - 1) + "alt") > 0)
		{
			slot.Add(Object.Instantiate<GameObject>(prefabs[@int - 1], base.transform));
		}
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0005AFE0 File Offset: 0x000591E0
	public void ForceWeapon(string weaponName)
	{
		if (this.gunc == null)
		{
			this.gunc = base.GetComponent<GunControl>();
		}
		int num = 0;
		if (MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + weaponName, 0) == 2)
		{
			num = 1;
		}
		uint num2 = <PrivateImplementationDetails>.ComputeStringHash(weaponName);
		if (num2 <= 1732737622U)
		{
			if (num2 <= 415515078U)
			{
				if (num2 != 381959840U)
				{
					if (num2 != 415515078U)
					{
						return;
					}
					if (!(weaponName == "rev0"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.revolverPierce[num].ToAsset(), true);
					return;
				}
				else
				{
					if (!(weaponName == "rev2"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.revolverRicochet[num].ToAsset(), true);
					return;
				}
			}
			else if (num2 != 1705213223U)
			{
				if (num2 != 1721990842U)
				{
					if (num2 != 1732737622U)
					{
						return;
					}
					if (!(weaponName == "sho1"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.shotgunPump[num].ToAsset(), true);
					return;
				}
				else
				{
					if (!(weaponName == "rai1"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.railHarpoon[num].ToAsset(), true);
					return;
				}
			}
			else
			{
				if (!(weaponName == "rai2"))
				{
					return;
				}
				this.gunc.ForceWeapon(this.railMalicious[num].ToAsset(), true);
				return;
			}
		}
		else if (num2 <= 3912221022U)
		{
			if (num2 != 1738768461U)
			{
				if (num2 != 1749515241U)
				{
					if (num2 != 3912221022U)
					{
						return;
					}
					if (!(weaponName == "nai1"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.nailOverheat[num].ToAsset(), true);
					return;
				}
				else
				{
					if (!(weaponName == "sho0"))
					{
						return;
					}
					this.gunc.ForceWeapon(this.shotgunGrenade[num].ToAsset(), true);
					return;
				}
			}
			else
			{
				if (!(weaponName == "rai0"))
				{
					return;
				}
				this.gunc.ForceWeapon(this.railCannon[num].ToAsset(), true);
				return;
			}
		}
		else if (num2 != 3928998641U)
		{
			if (num2 != 4244000204U)
			{
				if (num2 != 4260777823U)
				{
					return;
				}
				if (!(weaponName == "rock1"))
				{
					return;
				}
				this.gunc.ForceWeapon(this.rocketGreen[num].ToAsset(), true);
				return;
			}
			else
			{
				if (!(weaponName == "rock0"))
				{
					return;
				}
				this.gunc.ForceWeapon(this.rocketBlue[num].ToAsset(), true);
				return;
			}
		}
		else
		{
			if (!(weaponName == "nai0"))
			{
				return;
			}
			this.gunc.ForceWeapon(this.nailMagnet[num].ToAsset(), true);
			return;
		}
	}

	// Token: 0x0400106F RID: 4207
	[HideInInspector]
	public GunControl gunc;

	// Token: 0x04001070 RID: 4208
	[HideInInspector]
	public ForcedLoadout forcedLoadout;

	// Token: 0x04001071 RID: 4209
	[Header("Revolver")]
	public AssetReference[] revolverPierce;

	// Token: 0x04001072 RID: 4210
	public AssetReference[] revolverRicochet;

	// Token: 0x04001073 RID: 4211
	public AssetReference[] revolverTwirl;

	// Token: 0x04001074 RID: 4212
	[Header("Shotgun")]
	public AssetReference[] shotgunGrenade;

	// Token: 0x04001075 RID: 4213
	public AssetReference[] shotgunPump;

	// Token: 0x04001076 RID: 4214
	public AssetReference[] shotgunRed;

	// Token: 0x04001077 RID: 4215
	[Header("Nailgun")]
	public AssetReference[] nailMagnet;

	// Token: 0x04001078 RID: 4216
	public AssetReference[] nailOverheat;

	// Token: 0x04001079 RID: 4217
	public AssetReference[] nailRed;

	// Token: 0x0400107A RID: 4218
	[Header("Railcannon")]
	public AssetReference[] railCannon;

	// Token: 0x0400107B RID: 4219
	public AssetReference[] railHarpoon;

	// Token: 0x0400107C RID: 4220
	public AssetReference[] railMalicious;

	// Token: 0x0400107D RID: 4221
	[Header("Rocket Launcher")]
	public AssetReference[] rocketBlue;

	// Token: 0x0400107E RID: 4222
	public AssetReference[] rocketGreen;

	// Token: 0x0400107F RID: 4223
	public AssetReference[] rocketRed;
}
