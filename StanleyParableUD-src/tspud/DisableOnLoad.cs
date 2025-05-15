using System;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class DisableOnLoad : MonoBehaviour
{
	// Token: 0x06000416 RID: 1046 RVA: 0x00019283 File Offset: 0x00017483
	private void Start()
	{
		base.gameObject.SetActive(false);
	}
}
