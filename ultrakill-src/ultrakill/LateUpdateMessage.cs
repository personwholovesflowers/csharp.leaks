using System;
using UnityEngine;

// Token: 0x020001AF RID: 431
[DisallowMultipleComponent]
public sealed class LateUpdateMessage : MessageDispatcher
{
	// Token: 0x0600088D RID: 2189 RVA: 0x0003A598 File Offset: 0x00038798
	private void LateUpdate()
	{
		base.Handler.Invoke();
	}
}
