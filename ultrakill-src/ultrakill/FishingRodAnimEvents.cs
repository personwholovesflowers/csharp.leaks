using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class FishingRodAnimEvents : MonoBehaviour
{
	// Token: 0x060009E8 RID: 2536 RVA: 0x0004464F File Offset: 0x0004284F
	public void ThrowBaitEvent()
	{
		this.weapon.ThrowBaitEvent();
	}

	// Token: 0x04000CE1 RID: 3297
	[SerializeField]
	private FishingRodWeapon weapon;
}
