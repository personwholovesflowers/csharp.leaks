using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001DC RID: 476
public class SimpleEventReceiver : MonoBehaviour
{
	// Token: 0x06000ADD RID: 2781 RVA: 0x000335E7 File Offset: 0x000317E7
	private void Awake()
	{
		if (this.simpleEvent != null)
		{
			SimpleEvent simpleEvent = this.simpleEvent;
			simpleEvent.OnCall = (Action)Delegate.Combine(simpleEvent.OnCall, new Action(this.OnCall));
		}
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0003361E File Offset: 0x0003181E
	private void OnDestroy()
	{
		if (this.simpleEvent != null)
		{
			SimpleEvent simpleEvent = this.simpleEvent;
			simpleEvent.OnCall = (Action)Delegate.Remove(simpleEvent.OnCall, new Action(this.OnCall));
		}
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x00033655 File Offset: 0x00031855
	private void OnCall()
	{
		this.OnCallEvent.Invoke();
	}

	// Token: 0x04000AD6 RID: 2774
	[SerializeField]
	private SimpleEvent simpleEvent;

	// Token: 0x04000AD7 RID: 2775
	[SerializeField]
	private UnityEvent OnCallEvent;
}
