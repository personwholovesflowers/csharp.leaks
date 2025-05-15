using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B5 RID: 437
public abstract class MessageDispatcherBase : MonoBehaviour
{
	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0003A6A3 File Offset: 0x000388A3
	public UnityEventBase Handler
	{
		get
		{
			return this.GetHandler();
		}
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00004AB6 File Offset: 0x00002CB6
	private protected MessageDispatcherBase()
	{
	}

	// Token: 0x060008A7 RID: 2215
	protected abstract UnityEventBase GetHandler();

	// Token: 0x060008A8 RID: 2216 RVA: 0x0003A6AB File Offset: 0x000388AB
	public void RemoveAllListeners()
	{
		this.GetHandler().RemoveAllListeners();
	}
}
