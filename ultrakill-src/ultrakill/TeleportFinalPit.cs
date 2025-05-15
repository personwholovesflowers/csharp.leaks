using System;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class TeleportFinalPit : MonoBehaviour
{
	// Token: 0x06001A24 RID: 6692 RVA: 0x000D7D54 File Offset: 0x000D5F54
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			other.transform.position = other.transform.position + base.transform.forward * 20f + Vector3.up * 20f;
		}
	}
}
