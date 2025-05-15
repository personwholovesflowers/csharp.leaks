using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B3 RID: 435
public abstract class MessageDispatcher<T1, T2> : MessageDispatcherBase
{
	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x0600089D RID: 2205
	public new abstract UnityEvent<T1, T2> Handler { get; }

	// Token: 0x0600089E RID: 2206 RVA: 0x0003A5F9 File Offset: 0x000387F9
	private MessageDispatcher()
	{
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x0003A652 File Offset: 0x00038852
	public void AddListener(UnityAction<T1, T2> action)
	{
		this.Handler.AddListener(action);
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x0003A660 File Offset: 0x00038860
	public void RemoveListener(UnityAction<T1, T2> action)
	{
		this.Handler.RemoveListener(action);
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x0003A66E File Offset: 0x0003886E
	public new void RemoveAllListeners()
	{
		this.Handler.RemoveAllListeners();
	}

	// Token: 0x060008A2 RID: 2210 RVA: 0x0003A67B File Offset: 0x0003887B
	protected sealed override UnityEventBase GetHandler()
	{
		return this.Handler;
	}

	// Token: 0x020001B4 RID: 436
	public abstract class Callback<TEvent> : MessageDispatcher<T1, T2> where TEvent : UnityEvent<T1, T2>, new()
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0003A683 File Offset: 0x00038883
		public sealed override UnityEvent<T1, T2> Handler
		{
			get
			{
				return this._handler;
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0003A690 File Offset: 0x00038890
		public Callback()
		{
			this._handler = new TEvent();
		}

		// Token: 0x04000B48 RID: 2888
		[SerializeField]
		private TEvent _handler;
	}
}
