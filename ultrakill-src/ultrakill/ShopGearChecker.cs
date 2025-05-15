using System;
using UnityEngine;

// Token: 0x020003F6 RID: 1014
public class ShopGearChecker : MonoBehaviour
{
	// Token: 0x060016C0 RID: 5824 RVA: 0x000B64B0 File Offset: 0x000B46B0
	private void OnEnable()
	{
		if (this.shopcats == null)
		{
			this.shopcats = base.GetComponentsInChildren<ShopCategory>();
		}
		ShopCategory[] array = this.shopcats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CheckGear();
		}
	}

	// Token: 0x04001F82 RID: 8066
	private ShopCategory[] shopcats;
}
