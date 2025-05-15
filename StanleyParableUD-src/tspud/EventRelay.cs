using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B9 RID: 185
public class EventRelay : MonoBehaviour
{
	// Token: 0x06000446 RID: 1094 RVA: 0x00019D84 File Offset: 0x00017F84
	private void Start()
	{
		if (this.invokeOnStart)
		{
			this.Invoke();
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x00019D94 File Offset: 0x00017F94
	public void TestInvoke()
	{
		this.Invoke();
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00019D9C File Offset: 0x00017F9C
	[ContextMenu("Invoke")]
	public void Invoke()
	{
		UnityEvent unityEvent = this.invokableEvent;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04000439 RID: 1081
	[SerializeField]
	private UnityEvent invokableEvent;

	// Token: 0x0400043A RID: 1082
	[SerializeField]
	[InspectorButton("TestInvoke", "Test Invoke")]
	private bool invokeOnStart;
}
