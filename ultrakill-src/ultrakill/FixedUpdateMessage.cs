using System;
using UnityEngine;

// Token: 0x020001AE RID: 430
[DisallowMultipleComponent]
public sealed class FixedUpdateMessage : MessageDispatcher
{
	// Token: 0x0600088B RID: 2187 RVA: 0x0003A598 File Offset: 0x00038798
	private void FixedUpdate()
	{
		base.Handler.Invoke();
	}
}
