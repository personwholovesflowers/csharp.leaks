using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
[DisallowMultipleComponent]
public sealed class TriggerEnterMessage : MessageDispatcher<Collider>.Callback<UnityEventCollider>
{
	// Token: 0x060008EB RID: 2283 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	private void OnTriggerEnter(Collider other)
	{
		this.Handler.Invoke(other);
	}
}
