using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
[CreateAssetMenu(fileName = "New Event", menuName = "SimpleEvent/New Event")]
public class SimpleEvent : ScriptableObject
{
	// Token: 0x06000ADB RID: 2779 RVA: 0x000335D2 File Offset: 0x000317D2
	[ContextMenu("Call")]
	public void Call()
	{
		if (this.OnCall != null)
		{
			this.OnCall();
		}
	}

	// Token: 0x04000AD5 RID: 2773
	public Action OnCall;
}
