using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B1 RID: 433
public abstract class MessageDispatcher<T> : MessageDispatcherBase
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000895 RID: 2197
	public new abstract UnityEvent<T> Handler { get; }

	// Token: 0x06000896 RID: 2198 RVA: 0x0003A5F9 File Offset: 0x000387F9
	private MessageDispatcher()
	{
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003A601 File Offset: 0x00038801
	public void AddListener(UnityAction<T> action)
	{
		this.Handler.AddListener(action);
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0003A60F File Offset: 0x0003880F
	public void RemoveListener(UnityAction<T> action)
	{
		this.Handler.RemoveListener(action);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0003A61D File Offset: 0x0003881D
	public new void RemoveAllListeners()
	{
		this.Handler.RemoveAllListeners();
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0003A62A File Offset: 0x0003882A
	protected sealed override UnityEventBase GetHandler()
	{
		return this.Handler;
	}

	// Token: 0x020001B2 RID: 434
	public abstract class Callback<TEvent> : MessageDispatcher<T> where TEvent : UnityEvent<T>, new()
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0003A632 File Offset: 0x00038832
		public sealed override UnityEvent<T> Handler
		{
			get
			{
				return this._handler;
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0003A63F File Offset: 0x0003883F
		public Callback()
		{
			this._handler = new TEvent();
		}

		// Token: 0x04000B47 RID: 2887
		[SerializeField]
		private TEvent _handler;
	}
}
