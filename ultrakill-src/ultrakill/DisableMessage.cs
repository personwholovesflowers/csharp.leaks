using System;
using UnityEngine;

// Token: 0x020001AC RID: 428
[DisallowMultipleComponent]
public sealed class DisableMessage : MessageDispatcher
{
	// Token: 0x06000887 RID: 2183 RVA: 0x0003A598 File Offset: 0x00038798
	private void OnDisable()
	{
		base.Handler.Invoke();
	}
}
