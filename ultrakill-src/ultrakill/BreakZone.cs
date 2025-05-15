using System;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class BreakZone : MonoBehaviour
{
	// Token: 0x060002F0 RID: 752 RVA: 0x000114F4 File Offset: 0x0000F6F4
	private void OnTriggerEnter(Collider other)
	{
		Breakable breakable;
		if (other.gameObject.CompareTag("Breakable") && other.TryGetComponent<Breakable>(out breakable) && (!this.weakOnly || breakable.weak) && (this.countsAsPrecise || !breakable.precisionOnly))
		{
			breakable.Break();
		}
	}

	// Token: 0x0400038F RID: 911
	public bool weakOnly;

	// Token: 0x04000390 RID: 912
	public bool countsAsPrecise;
}
