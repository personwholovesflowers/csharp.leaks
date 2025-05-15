using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001D0 RID: 464
public class WalkingBackwards : MonoBehaviour
{
	// Token: 0x06000AA6 RID: 2726 RVA: 0x00031A0C File Offset: 0x0002FC0C
	private void Start()
	{
		if (this.m_MyEvent == null)
		{
			this.m_MyEvent = new UnityEvent();
		}
		if (this.m_MyEventB == null)
		{
			this.m_MyEventB = new UnityEvent();
		}
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00031A34 File Offset: 0x0002FC34
	private void Update()
	{
		if (Input.GetKeyDown("w"))
		{
			this.m_MyEvent.Invoke();
		}
		if (Input.GetKeyUp("w"))
		{
			this.m_MyEventB.Invoke();
		}
	}

	// Token: 0x04000A8A RID: 2698
	public UnityEvent m_MyEvent;

	// Token: 0x04000A8B RID: 2699
	public UnityEvent m_MyEventB;
}
