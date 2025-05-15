using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000312 RID: 786
public class NailBurstController : MonoBehaviour
{
	// Token: 0x060011EE RID: 4590 RVA: 0x0008C3E0 File Offset: 0x0008A5E0
	private void Update()
	{
		for (int i = this.nails.Count - 1; i >= 0; i--)
		{
			if (this.nails[i] == null || this.nails[i].hit)
			{
				this.nails.RemoveAt(i);
			}
		}
		if (this.nails.Count == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400186E RID: 6254
	public List<EnemyIdentifier> damagedEnemies = new List<EnemyIdentifier>();

	// Token: 0x0400186F RID: 6255
	public List<Nail> nails;
}
