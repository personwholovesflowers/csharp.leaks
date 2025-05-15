using System;
using UnityEngine;

// Token: 0x020001BA RID: 442
[DisallowMultipleComponent]
public sealed class TriggerStayMessage : MessageDispatcher<Collider>.Callback<UnityEventCollider>
{
	// Token: 0x060008EF RID: 2287 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	private void OnTriggerStay(Collider other)
	{
		this.Handler.Invoke(other);
	}
}
