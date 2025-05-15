using System;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class ZombieIgnorizer : MonoBehaviour
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x000ECFE4 File Offset: 0x000EB1E4
	private void Start()
	{
		EnemyIdentifier[] array = this.eids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ignoredByEnemies = true;
		}
	}

	// Token: 0x04002802 RID: 10242
	public EnemyIdentifier[] eids;
}
