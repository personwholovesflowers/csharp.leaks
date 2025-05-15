using System;
using UnityEngine;

// Token: 0x02000249 RID: 585
public class HideBone : MonoBehaviour
{
	// Token: 0x06000CD9 RID: 3289 RVA: 0x0005FB42 File Offset: 0x0005DD42
	private void LateUpdate()
	{
		base.transform.localScale = Vector3.zero;
	}
}
