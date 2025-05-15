using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000F3 RID: 243
public class IntChoiceEvent : MonoBehaviour
{
	// Token: 0x060005D9 RID: 1497 RVA: 0x00020438 File Offset: 0x0001E638
	private void Awake()
	{
		if (this.intConfigurable != null)
		{
			IntConfigurable intConfigurable = this.intConfigurable;
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
			this.intConfigurable.Init();
		}
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00020485 File Offset: 0x0001E685
	private void Start()
	{
		if (this.intConfigurable != null)
		{
			this.intConfigurable.Init();
		}
		if (this.selfInvokeOnValueChange)
		{
			this.InvokeChoiceEvent();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x000204AE File Offset: 0x0001E6AE
	private void OnDestroy()
	{
		if (this.intConfigurable != null)
		{
			IntConfigurable intConfigurable = this.intConfigurable;
			intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000204E5 File Offset: 0x0001E6E5
	private void OnValueChanged(LiveData data)
	{
		if (this.selfInvokeOnValueChange)
		{
			this.InvokeChoiceEvent();
		}
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x000204F5 File Offset: 0x0001E6F5
	public void InvokeChoiceEvent()
	{
		IntChoiceEvent.IntBasedEvent intBasedEvent = this.events.Find((IntChoiceEvent.IntBasedEvent x) => x.i == this.intConfigurable.GetIntValue());
		if (intBasedEvent == null)
		{
			return;
		}
		UnityEvent evt = intBasedEvent.evt;
		if (evt == null)
		{
			return;
		}
		evt.Invoke();
	}

	// Token: 0x04000617 RID: 1559
	[SerializeField]
	private IntConfigurable intConfigurable;

	// Token: 0x04000618 RID: 1560
	[SerializeField]
	private bool selfInvokeOnValueChange;

	// Token: 0x04000619 RID: 1561
	[SerializeField]
	private List<IntChoiceEvent.IntBasedEvent> events;

	// Token: 0x020003C3 RID: 963
	[Serializable]
	private class IntBasedEvent
	{
		// Token: 0x040013EA RID: 5098
		public int i;

		// Token: 0x040013EB RID: 5099
		public UnityEvent evt;
	}
}
