using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
[DisallowMultipleComponent]
public sealed class UpdateMessage : MessageDispatcher
{
	// Token: 0x060008F4 RID: 2292 RVA: 0x0003A598 File Offset: 0x00038798
	private void Update()
	{
		base.Handler.Invoke();
	}
}
