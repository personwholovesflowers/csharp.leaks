using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000061 RID: 97
public class AnimationEvent : MonoBehaviour
{
	// Token: 0x060001DA RID: 474 RVA: 0x00009BB4 File Offset: 0x00007DB4
	public void TriggerTheEvent()
	{
		this.onEvent.Invoke();
	}

	// Token: 0x04000201 RID: 513
	[SerializeField]
	private UnityEvent onEvent;
}
