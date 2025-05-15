using System;
using UnityEngine;

// Token: 0x0200050F RID: 1295
public class HarpoonDestroyer : MonoBehaviour
{
	// Token: 0x06001D9A RID: 7578 RVA: 0x000F7778 File Offset: 0x000F5978
	private void OnTriggerEnter(Collider col)
	{
		if (col == null || col.attachedRigidbody == null)
		{
			return;
		}
		Harpoon harpoon;
		if (col.attachedRigidbody.TryGetComponent<Harpoon>(out harpoon))
		{
			Object.Destroy(harpoon.gameObject);
		}
	}
}
