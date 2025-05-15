using System;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class CoinCollector : MonoBehaviour
{
	// Token: 0x06000401 RID: 1025 RVA: 0x0001BE77 File Offset: 0x0001A077
	private void Start()
	{
		base.Invoke("Removal", 10f);
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001BE89 File Offset: 0x0001A089
	private void Removal()
	{
		Object.Destroy(this.coin);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040004F6 RID: 1270
	public GameObject coin;
}
