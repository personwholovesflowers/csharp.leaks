using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class DisableSelfOnParentDisable : MonoBehaviour
{
	// Token: 0x06000528 RID: 1320 RVA: 0x000225E7 File Offset: 0x000207E7
	private void OnDisable()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(false);
		}
	}
}
