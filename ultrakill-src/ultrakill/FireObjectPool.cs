using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D1 RID: 465
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class FireObjectPool : MonoSingleton<FireObjectPool>
{
	// Token: 0x06000987 RID: 2439 RVA: 0x00042440 File Offset: 0x00040640
	private new void Awake()
	{
		this.firePool = new Queue<GameObject>();
		this.simpleFirePool = new Queue<GameObject>();
		for (int i = 0; i < this.poolSize; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.firePrefab, base.transform);
			gameObject.SetActive(false);
			this.firePool.Enqueue(gameObject);
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.simpleFirePrefab, base.transform);
			gameObject2.SetActive(false);
			this.simpleFirePool.Enqueue(gameObject2);
		}
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x000424C0 File Offset: 0x000406C0
	public GameObject GetFire(bool isSimple)
	{
		Queue<GameObject> queue = (isSimple ? this.simpleFirePool : this.firePool);
		if (queue.Count > 0)
		{
			GameObject gameObject = queue.Dequeue();
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				return gameObject;
			}
		}
		return Object.Instantiate<GameObject>(isSimple ? this.simpleFirePrefab : this.firePrefab);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00042517 File Offset: 0x00040717
	public void ReturnFire(GameObject fireObject, bool isSimple)
	{
		fireObject.transform.SetParent(base.transform);
		fireObject.SetActive(false);
		(isSimple ? this.simpleFirePool : this.firePool).Enqueue(fireObject);
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00042548 File Offset: 0x00040748
	public void RemoveAllFiresFromObject(GameObject objectToSearch)
	{
		Flammable[] componentsInChildren = objectToSearch.GetComponentsInChildren<Flammable>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].MarkForDestroy();
		}
	}

	// Token: 0x04000C66 RID: 3174
	public GameObject firePrefab;

	// Token: 0x04000C67 RID: 3175
	public GameObject simpleFirePrefab;

	// Token: 0x04000C68 RID: 3176
	public int poolSize = 100;

	// Token: 0x04000C69 RID: 3177
	private Queue<GameObject> firePool;

	// Token: 0x04000C6A RID: 3178
	private Queue<GameObject> simpleFirePool;
}
