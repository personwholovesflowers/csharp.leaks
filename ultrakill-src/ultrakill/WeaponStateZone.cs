using System;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class WeaponStateZone : MonoBehaviour
{
	// Token: 0x06001C00 RID: 7168 RVA: 0x000E87D8 File Offset: 0x000E69D8
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.gc = MonoSingleton<GunControl>.Instance;
			if (this.gc == null)
			{
				this.gc = other.GetComponentInChildren<GunControl>();
			}
			if (!this.gc)
			{
				return;
			}
			if (this.allowWeaponsOnEnter)
			{
				this.gc.YesWeapon();
			}
			else
			{
				this.gc.NoWeapon();
			}
			FistControl instance = MonoSingleton<FistControl>.Instance;
			if (this.allowArmOnEnter)
			{
				instance.YesFist();
				return;
			}
			instance.NoFist();
		}
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x000E8868 File Offset: 0x000E6A68
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (this.gc == null)
			{
				this.gc = other.GetComponentInChildren<GunControl>();
			}
			if (!this.gc)
			{
				return;
			}
			if (this.allowWeaponsOnExit)
			{
				this.gc.YesWeapon();
			}
			else
			{
				this.gc.NoWeapon();
			}
			FistControl instance = MonoSingleton<FistControl>.Instance;
			if (this.allowArmOnExit)
			{
				instance.YesFist();
				return;
			}
			instance.NoFist();
		}
	}

	// Token: 0x04002787 RID: 10119
	public bool allowWeaponsOnEnter;

	// Token: 0x04002788 RID: 10120
	public bool allowWeaponsOnExit = true;

	// Token: 0x04002789 RID: 10121
	public bool allowArmOnEnter = true;

	// Token: 0x0400278A RID: 10122
	public bool allowArmOnExit = true;

	// Token: 0x0400278B RID: 10123
	private GunControl gc;
}
