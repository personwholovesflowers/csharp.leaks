using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class MoveToTargetEventHolder : MonoBehaviour
{
	// Token: 0x06000776 RID: 1910 RVA: 0x0002656D File Offset: 0x0002476D
	public void MoveToTarget(Transform target)
	{
		base.transform.position = target.position;
	}
}
