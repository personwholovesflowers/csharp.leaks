using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
public class CoinActivated : MonoBehaviour
{
	// Token: 0x060003FE RID: 1022 RVA: 0x0001BE14 File Offset: 0x0001A014
	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Coin"))
		{
			return;
		}
		this.events.Invoke("");
		if (this.disableCoin)
		{
			other.gameObject.SetActive(false);
		}
		base.GetComponent<Collider>().enabled = false;
	}

	// Token: 0x040004F2 RID: 1266
	public bool disableCoin;

	// Token: 0x040004F3 RID: 1267
	public UltrakillEvent events;
}
