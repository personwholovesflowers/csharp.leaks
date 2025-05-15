using System;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class ResetPositionEvent : MonoBehaviour
{
	// Token: 0x06000894 RID: 2196 RVA: 0x0002891A File Offset: 0x00026B1A
	public void UpdateLocalPosition()
	{
		this.target.localPosition = this.localPositionToSetOnEvent;
	}

	// Token: 0x04000863 RID: 2147
	[SerializeField]
	private Transform target;

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private Vector3 localPositionToSetOnEvent;
}
