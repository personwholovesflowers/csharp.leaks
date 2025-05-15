using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class DestroyComponents : MonoBehaviour
{
	// Token: 0x06000504 RID: 1284 RVA: 0x00021FCC File Offset: 0x000201CC
	public void Activate()
	{
		for (int i = this.targets.Length - 1; i >= 0; i--)
		{
			if (this.targets[i] != null)
			{
				Object.Destroy(this.targets[i]);
			}
		}
	}

	// Token: 0x040006F0 RID: 1776
	public Component[] targets;
}
