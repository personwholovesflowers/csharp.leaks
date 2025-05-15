using System;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class RemoveOnParentScale : MonoBehaviour
{
	// Token: 0x060014C8 RID: 5320 RVA: 0x000A7927 File Offset: 0x000A5B27
	private void Update()
	{
		if (base.transform.parent.localScale == Vector3.zero)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
