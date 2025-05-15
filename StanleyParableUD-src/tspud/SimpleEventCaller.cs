using System;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class SimpleEventCaller : MonoBehaviour
{
	// Token: 0x06000933 RID: 2355 RVA: 0x0002B640 File Offset: 0x00029840
	public void CallSimpleEvent(SimpleEvent simpleEvent)
	{
		simpleEvent.Call();
	}
}
