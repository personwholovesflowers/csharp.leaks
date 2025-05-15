using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public class ShopCategory : MonoBehaviour
{
	// Token: 0x060016BE RID: 5822 RVA: 0x000B6485 File Offset: 0x000B4685
	public void CheckGear()
	{
		if (GameProgressSaver.CheckGear(this.weaponName) == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x04001F81 RID: 8065
	public string weaponName;
}
