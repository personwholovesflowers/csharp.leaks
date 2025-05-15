using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200014A RID: 330
public class PickIntConfigurableEvent : MonoBehaviour
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x00026CDB File Offset: 0x00024EDB
	public void InvokePickEvent()
	{
		PickIntConfigurableEvent.IntPickEvent intPickEvent = this.events.Find((PickIntConfigurableEvent.IntPickEvent x) => x.intConfigurable.GetIntValue() == this.pickInteger);
		if (intPickEvent == null)
		{
			return;
		}
		UnityEvent evt = intPickEvent.evt;
		if (evt == null)
		{
			return;
		}
		evt.Invoke();
	}

	// Token: 0x040007C5 RID: 1989
	[SerializeField]
	private int pickInteger;

	// Token: 0x040007C6 RID: 1990
	[SerializeField]
	private List<PickIntConfigurableEvent.IntPickEvent> events;

	// Token: 0x020003DF RID: 991
	[Serializable]
	private class IntPickEvent
	{
		// Token: 0x0400144D RID: 5197
		public IntConfigurable intConfigurable;

		// Token: 0x0400144E RID: 5198
		public UnityEvent evt;
	}
}
