using System;
using UnityEngine;

// Token: 0x0200031E RID: 798
public class ObjectSpawner : MonoBehaviour
{
	// Token: 0x06001266 RID: 4710 RVA: 0x00093CB4 File Offset: 0x00091EB4
	public void SpawnObject(int objectNumber)
	{
		if (this.spawnables != null && this.spawnables.Length > objectNumber && this.spawnables[objectNumber] != null)
		{
			Object.Instantiate<GameObject>(this.spawnables[objectNumber], this.spawnables[objectNumber].transform.position, this.spawnables[objectNumber].transform.rotation, this.spawnables[objectNumber].transform.parent).SetActive(true);
		}
	}

	// Token: 0x04001966 RID: 6502
	public GameObject[] spawnables;
}
