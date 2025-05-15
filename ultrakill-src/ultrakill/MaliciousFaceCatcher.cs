using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002E2 RID: 738
public class MaliciousFaceCatcher : MonoBehaviour
{
	// Token: 0x06001008 RID: 4104 RVA: 0x00079FF8 File Offset: 0x000781F8
	public void RemoveAll()
	{
		for (int i = this.targets.Count - 1; i >= 0; i--)
		{
			if (this.targets[i] != null)
			{
				Object.Destroy(this.targets[i].gameObject);
			}
		}
		this.targets.Clear();
	}

	// Token: 0x040015D2 RID: 5586
	[HideInInspector]
	public List<Transform> targets = new List<Transform>();
}
