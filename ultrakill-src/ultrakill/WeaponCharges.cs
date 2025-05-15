using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020004BE RID: 1214
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class WeaponCharges : MonoSingleton<WeaponCharges>
{
	// Token: 0x06001BDA RID: 7130 RVA: 0x000E7440 File Offset: 0x000E5640
	private void Update()
	{
		if (NoWeaponCooldown.NoCooldown)
		{
			this.MaxCharges();
		}
		else
		{
			this.Charge(Time.deltaTime);
		}
		if (this.rocketFrozen)
		{
			if (this.rocketCount > 0)
			{
				this.canAutoUnfreeze = true;
			}
			if (this.canAutoUnfreeze && this.rocketCount == 0 && this.timeSinceIdleFrozen > 0.5f)
			{
				this.rocketLauncher.UnfreezeRockets();
			}
		}
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000E74AC File Offset: 0x000E56AC
	public void Charge(float amount)
	{
		if (this.rev0charge < 100f)
		{
			if (this.rev0alt)
			{
				this.rev0charge = Mathf.MoveTowards(this.rev0charge, 100f, 20f * amount);
			}
			else
			{
				this.rev0charge = Mathf.MoveTowards(this.rev0charge, 100f, 40f * amount);
			}
		}
		if (this.rev1charge < 400f)
		{
			this.rev1charge = Mathf.MoveTowards(this.rev1charge, 400f, 25f * amount);
		}
		if (this.rev2charge < 300f)
		{
			if (this.rev2alt)
			{
				this.rev2charge = Mathf.MoveTowards(this.rev2charge, 300f, 35f * amount);
			}
			else
			{
				this.rev2charge = Mathf.MoveTowards(this.rev2charge, 300f, 15f * amount);
			}
		}
		if (this.shoAltNadeCharge < 1f)
		{
			this.shoAltNadeCharge = Mathf.MoveTowards(this.shoAltNadeCharge, 1f, amount * 0.5f);
		}
		if (this.shoSawCharge < 1f)
		{
			this.shoSawCharge = Mathf.MoveTowards(this.shoSawCharge, 1f, amount * ((this.shoSawAmount == 0) ? 0.25f : 0.125f));
		}
		if (this.naiHeatsinks < 2f)
		{
			this.naiHeatsinks = Mathf.MoveTowards(this.naiHeatsinks, 2f, amount * 0.125f);
		}
		if (this.naiSawHeatsinks < 1f)
		{
			this.naiSawHeatsinks = Mathf.MoveTowards(this.naiSawHeatsinks, 1f, amount * 0.125f);
		}
		if (this.naiheatUp > 0f)
		{
			this.naiheatUp = Mathf.MoveTowards(this.naiheatUp, 0f, amount * 0.3f);
		}
		if (this.naiZapperRecharge < 5f)
		{
			this.naiZapperRecharge = Mathf.MoveTowards(this.naiZapperRecharge, 5f, amount);
		}
		if (this.raicharge < 5f)
		{
			this.raicharge = Mathf.MoveTowards(this.raicharge, 5f, amount * 0.25f);
			if (this.raicharge >= 4f && this.railCannonFullChargeSound)
			{
				this.raicharge = 5f;
				this.PlayRailCharge();
			}
		}
		if (this.rocketcharge > 0f)
		{
			this.rocketcharge = Mathf.MoveTowards(this.rocketcharge, 0f, amount);
		}
		if (this.rocketCannonballCharge < 1f)
		{
			this.rocketCannonballCharge = Mathf.MoveTowards(this.rocketCannonballCharge, 1f, amount * 0.125f);
		}
		if (this.rocketNapalmFuel < 1f)
		{
			this.rocketNapalmFuel = Mathf.MoveTowards(this.rocketNapalmFuel, 1f, amount * 0.125f);
		}
		for (int i = 0; i < this.revaltpickupcharges.Length; i++)
		{
			if (this.revaltpickupcharges[i] > 0f)
			{
				this.revaltpickupcharges[i] = Mathf.MoveTowards(this.revaltpickupcharges[i], 0f, amount);
			}
		}
		for (int j = 0; j < this.shoaltcooldowns.Length; j++)
		{
			if (this.shoaltcooldowns[j] > 0f)
			{
				this.shoaltcooldowns[j] = Mathf.MoveTowards(this.shoaltcooldowns[j], 0f, amount);
			}
		}
		if (!this.naiAmmoDontCharge)
		{
			this.naiAmmo = Mathf.MoveTowards(this.naiAmmo, 100f, amount * 5f);
			this.naiSaws = Mathf.MoveTowards(this.naiSaws, 10f, amount * 0.5f);
		}
		if (this.magnets.Count > 0)
		{
			for (int k = this.magnets.Count - 1; k >= 0; k--)
			{
				if (this.magnets[k] == null)
				{
					this.magnets.RemoveAt(k);
				}
			}
			if (this.magnets.Count < 3 && this.naiMagnetCharge < (float)(3 - this.magnets.Count))
			{
				this.naiMagnetCharge = Mathf.MoveTowards(this.naiMagnetCharge, (float)(3 - this.magnets.Count), amount * 3f);
			}
		}
		else if (this.naiMagnetCharge < 3f)
		{
			this.naiMagnetCharge = Mathf.MoveTowards(this.naiMagnetCharge, 3f, amount * 3f);
		}
		this.rocketFreezeTime = Mathf.MoveTowards(this.rocketFreezeTime, (float)(this.rocketFrozen ? 0 : 5), this.rocketFrozen ? Time.deltaTime : (amount / 2f));
		if (this.rocketFrozen && this.rocketLauncher && this.rocketLauncher.currentTimerTickSound)
		{
			this.rocketLauncher.currentTimerTickSound.pitch = Mathf.Lerp(0.75f, 1f, this.rocketFreezeTime / 5f);
		}
		if (this.rocketFrozen && this.rocketFreezeTime <= 0f)
		{
			this.rocketLauncher.UnfreezeRockets();
		}
		if (this.punchStamina < 2f)
		{
			this.punchStamina = Mathf.MoveTowards(this.punchStamina, 2f, amount * 1.25f);
		}
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x000E79A4 File Offset: 0x000E5BA4
	public void MaxCharges()
	{
		this.rev0charge = 100f;
		this.rev1charge = 400f;
		this.rev2charge = 300f;
		this.shoAltNadeCharge = 1f;
		this.shoSawCharge = 1f;
		this.naiHeatsinks = 2f;
		this.naiSawHeatsinks = 1f;
		this.naiheatUp = 0f;
		this.rocketcharge = 0f;
		for (int i = 0; i < this.revaltpickupcharges.Length; i++)
		{
			this.revaltpickupcharges[i] = 0f;
		}
		for (int j = 0; j < this.shoaltcooldowns.Length; j++)
		{
			this.shoaltcooldowns[j] = 0f;
		}
		this.naiAmmo = 100f;
		this.naiSaws = 10f;
		this.magnets.Clear();
		this.naiMagnetCharge = 3f;
		this.naiZapperRecharge = 5f;
		if (this.raicharge < 5f)
		{
			this.raicharge = 5f;
			this.PlayRailCharge();
		}
		this.rocketFreezeTime = 5f;
		this.rocketCannonballCharge = 1f;
		this.rocketNapalmFuel = 1f;
		this.punchStamina = 2f;
		if (!this.gc)
		{
			this.gc = base.GetComponent<GunControl>();
		}
		if (this.gc && this.gc.currentWeapon)
		{
			this.gc.currentWeapon.SendMessage("MaxCharge", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000E7B23 File Offset: 0x000E5D23
	public void PlayRailCharge()
	{
		this.railChargePlayed = true;
		Object.Instantiate<GameObject>(this.railCannonFullChargeSound);
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000E7B38 File Offset: 0x000E5D38
	public void ResetNailgunAmmo()
	{
		this.naiAmmo = 100f;
		this.naiSaws = 10f;
	}

	// Token: 0x04002732 RID: 10034
	private GunControl gc;

	// Token: 0x04002733 RID: 10035
	public float rev0charge = 100f;

	// Token: 0x04002734 RID: 10036
	public bool rev0alt;

	// Token: 0x04002735 RID: 10037
	public float rev1charge = 400f;

	// Token: 0x04002736 RID: 10038
	public float rev2charge = 300f;

	// Token: 0x04002737 RID: 10039
	public bool rev2alt;

	// Token: 0x04002738 RID: 10040
	public float shoAltNadeCharge = 1f;

	// Token: 0x04002739 RID: 10041
	public float shoSawCharge = 1f;

	// Token: 0x0400273A RID: 10042
	public int shoSawAmount;

	// Token: 0x0400273B RID: 10043
	[HideInInspector]
	public bool nai0set;

	// Token: 0x0400273C RID: 10044
	public float naiHeatsinks = 2f;

	// Token: 0x0400273D RID: 10045
	public float naiSawHeatsinks = 1f;

	// Token: 0x0400273E RID: 10046
	public float naiheatUp;

	// Token: 0x0400273F RID: 10047
	public float naiAmmo = 100f;

	// Token: 0x04002740 RID: 10048
	public float naiSaws = 10f;

	// Token: 0x04002741 RID: 10049
	public bool naiAmmoDontCharge;

	// Token: 0x04002742 RID: 10050
	[HideInInspector]
	public List<Magnet> magnets = new List<Magnet>();

	// Token: 0x04002743 RID: 10051
	public float naiMagnetCharge = 3f;

	// Token: 0x04002744 RID: 10052
	public float naiZapperRecharge = 5f;

	// Token: 0x04002745 RID: 10053
	public float raicharge = 5f;

	// Token: 0x04002746 RID: 10054
	public GameObject railCannonFullChargeSound;

	// Token: 0x04002747 RID: 10055
	public bool railChargePlayed;

	// Token: 0x04002748 RID: 10056
	[HideInInspector]
	public bool rocketset;

	// Token: 0x04002749 RID: 10057
	public float rocketcharge;

	// Token: 0x0400274A RID: 10058
	[HideInInspector]
	public bool rocketFrozen;

	// Token: 0x0400274B RID: 10059
	public float rocketFreezeTime = 5f;

	// Token: 0x0400274C RID: 10060
	[HideInInspector]
	public RocketLauncher rocketLauncher;

	// Token: 0x0400274D RID: 10061
	public int rocketCount;

	// Token: 0x0400274E RID: 10062
	[HideInInspector]
	public bool canAutoUnfreeze;

	// Token: 0x0400274F RID: 10063
	public TimeSince timeSinceIdleFrozen;

	// Token: 0x04002750 RID: 10064
	public float rocketCannonballCharge = 1f;

	// Token: 0x04002751 RID: 10065
	public float rocketNapalmFuel = 1f;

	// Token: 0x04002752 RID: 10066
	[HideInInspector]
	public bool infiniteRocketRide;

	// Token: 0x04002753 RID: 10067
	public float[] revaltpickupcharges = new float[3];

	// Token: 0x04002754 RID: 10068
	public float[] shoaltcooldowns = new float[3];

	// Token: 0x04002755 RID: 10069
	public float punchStamina = 2f;
}
