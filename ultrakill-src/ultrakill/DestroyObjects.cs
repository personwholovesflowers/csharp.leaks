using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class DestroyObjects : MonoBehaviour
{
	// Token: 0x06000506 RID: 1286 RVA: 0x0002200B File Offset: 0x0002020B
	private void OnEnable()
	{
		if (this.destroyOnEnable)
		{
			this.Destroy();
		}
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x0002201B File Offset: 0x0002021B
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		if (this.dontDestroyOnTrigger)
		{
			return;
		}
		this.Destroy();
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0002203C File Offset: 0x0002023C
	public void Destroy()
	{
		foreach (GameObject gameObject in this.targets)
		{
			if (gameObject != null)
			{
				Object.Destroy(gameObject);
			}
		}
	}

	// Token: 0x040006F1 RID: 1777
	[SerializeField]
	private bool destroyOnEnable;

	// Token: 0x040006F2 RID: 1778
	[SerializeField]
	private bool dontDestroyOnTrigger;

	// Token: 0x040006F3 RID: 1779
	[SerializeField]
	private GameObject[] targets;
}
