using System;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
public class WeaponPickUp : MonoBehaviour
{
	// Token: 0x06001BF1 RID: 7153 RVA: 0x000E8100 File Offset: 0x000E6300
	private void Awake()
	{
		this.tempSlot = this.inventorySlot;
		if (!this.arm)
		{
			this.tempSlot--;
		}
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000E8124 File Offset: 0x000E6324
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && !this.activated)
		{
			this.GotActivated();
		}
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000E8146 File Offset: 0x000E6346
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !this.activated)
		{
			this.GotActivated();
		}
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000E8168 File Offset: 0x000E6368
	private void GotActivated()
	{
		this.activated = true;
		bool flag = false;
		if (this.pPref != "")
		{
			flag = GameProgressSaver.CheckGear(this.pPref) != 0 && MonoSingleton<PrefsManager>.Instance.GetInt("weapon." + this.pPref, 0) != 0;
			if (!flag)
			{
				MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + this.pPref, 1);
				if (!SceneHelper.IsPlayingCustom)
				{
					GameProgressSaver.AddGear(this.pPref);
				}
			}
		}
		if (!this.arm)
		{
			MonoSingleton<GunControl>.Instance.noWeapons = false;
			if (this.gs != null)
			{
				this.gs.enabled = true;
				this.gs.ResetWeapons(false);
			}
			if (!flag)
			{
				for (int i = 0; i < MonoSingleton<GunControl>.Instance.slots[this.tempSlot].Count; i++)
				{
					if (MonoSingleton<GunControl>.Instance.slots[this.tempSlot][i].name == this.weapon.name + "(Clone)")
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.weapon, MonoSingleton<GunControl>.Instance.transform);
				MonoSingleton<GunControl>.Instance.slots[this.tempSlot].Add(gameObject);
				MonoSingleton<GunControl>.Instance.ForceWeapon(this.weapon, true);
				MonoSingleton<GunControl>.Instance.noWeapons = false;
				MonoSingleton<GunControl>.Instance.UpdateWeaponList(false);
			}
			else if (SceneHelper.IsPlayingCustom)
			{
				for (int j = 0; j < MonoSingleton<GunControl>.Instance.slots[this.tempSlot].Count; j++)
				{
					if (MonoSingleton<GunControl>.Instance.slots[this.tempSlot][j].name == this.weapon.name + "(Clone)")
					{
						MonoSingleton<GunControl>.Instance.ForceWeapon(this.weapon, true);
						MonoSingleton<GunControl>.Instance.noWeapons = false;
						MonoSingleton<GunControl>.Instance.UpdateWeaponList(false);
					}
				}
			}
		}
		else
		{
			HookArm instance = MonoSingleton<HookArm>.Instance;
			if (instance != null)
			{
				instance.Cancel();
			}
			MonoSingleton<FistControl>.Instance.ResetFists();
			MonoSingleton<FistControl>.Instance.ForceArm(this.tempSlot, true);
		}
		if (this.activateOnPickup != null)
		{
			this.activateOnPickup.SetActive(true);
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002765 RID: 10085
	public GameObject weapon;

	// Token: 0x04002766 RID: 10086
	public int inventorySlot;

	// Token: 0x04002767 RID: 10087
	private int tempSlot;

	// Token: 0x04002768 RID: 10088
	public GunSetter gs;

	// Token: 0x04002769 RID: 10089
	public string pPref;

	// Token: 0x0400276A RID: 10090
	public bool arm;

	// Token: 0x0400276B RID: 10091
	public GameObject activateOnPickup;

	// Token: 0x0400276C RID: 10092
	private bool activated;
}
