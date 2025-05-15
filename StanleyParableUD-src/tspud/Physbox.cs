using System;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class Physbox : HammerEntity
{
	// Token: 0x060007A5 RID: 1957 RVA: 0x00026CA4 File Offset: 0x00024EA4
	private void OnValidate()
	{
		MeshCollider[] componentsInChildren = base.GetComponentsInChildren<MeshCollider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].convex = true;
		}
		base.GetComponent<Rigidbody>().isKinematic = false;
	}
}
