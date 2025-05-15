using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AE RID: 1198
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SimpleMeshCombineManager : MonoSingleton<SimpleMeshCombineManager>
{
	// Token: 0x06001B8D RID: 7053 RVA: 0x000E478C File Offset: 0x000E298C
	private void Start()
	{
		foreach (SimpleMeshCombiner simpleMeshCombiner in Resources.FindObjectsOfTypeAll<SimpleMeshCombiner>())
		{
			if (simpleMeshCombiner.gameObject.isStatic)
			{
				Debug.LogWarning("we can't process static meshes");
			}
			else
			{
				this.combinersQueue.Enqueue(simpleMeshCombiner);
			}
		}
		base.StartCoroutine(this.ProcessCombiners());
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x000E47E3 File Offset: 0x000E29E3
	private IEnumerator ProcessCombiners()
	{
		WaitForSeconds waitTime = new WaitForSeconds(this.waitTimeUntilProcess);
		yield return waitTime;
		while (this.combinersQueue.Count > 0)
		{
			this.combinersQueue.Dequeue().CombineMeshes();
			yield return waitTime;
		}
		yield break;
	}

	// Token: 0x040026CD RID: 9933
	public float waitTimeUntilProcess = 0.2f;

	// Token: 0x040026CE RID: 9934
	private Queue<SimpleMeshCombiner> combinersQueue = new Queue<SimpleMeshCombiner>();
}
