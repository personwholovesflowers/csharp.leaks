using System;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class AnimationEventToUltrakillEvent : MonoBehaviour
{
	// Token: 0x060001DC RID: 476 RVA: 0x00009BC1 File Offset: 0x00007DC1
	public void ToUltrakillEvent(int num)
	{
		if (num == this.eventNumber)
		{
			this.onEvent.Invoke("");
		}
	}

	// Token: 0x04000202 RID: 514
	public int eventNumber;

	// Token: 0x04000203 RID: 515
	public UltrakillEvent onEvent;
}
