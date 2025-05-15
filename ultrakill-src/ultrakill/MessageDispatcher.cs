using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B0 RID: 432
public abstract class MessageDispatcher : MessageDispatcherBase
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x0003A5AD File Offset: 0x000387AD
	public new UnityEvent Handler
	{
		get
		{
			return this._handler;
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0003A5B5 File Offset: 0x000387B5
	public void AddListener(UnityAction action)
	{
		this.Handler.AddListener(action);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0003A5C3 File Offset: 0x000387C3
	public void RemoveListener(UnityAction action)
	{
		this.Handler.RemoveListener(action);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0003A5D1 File Offset: 0x000387D1
	public new void RemoveAllListeners()
	{
		this.Handler.RemoveAllListeners();
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0003A5DE File Offset: 0x000387DE
	protected sealed override UnityEventBase GetHandler()
	{
		return this.Handler;
	}

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private UnityEvent _handler = new UnityEvent();
}
