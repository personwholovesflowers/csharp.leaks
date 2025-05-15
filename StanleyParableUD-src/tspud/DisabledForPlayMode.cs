using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class DisabledForPlayMode : MonoBehaviour
{
	// Token: 0x06000418 RID: 1048 RVA: 0x00019283 File Offset: 0x00017483
	private void Start()
	{
		base.gameObject.SetActive(false);
	}
}
