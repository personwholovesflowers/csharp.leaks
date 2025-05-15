using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
[DisallowMultipleComponent]
public sealed class TriggerExitMessage : MessageDispatcher<Collider>.Callback<UnityEventCollider>
{
	// Token: 0x060008ED RID: 2285 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	private void OnTriggerExit(Collider other)
	{
		this.Handler.Invoke(other);
	}
}
