using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
[DisallowMultipleComponent]
public sealed class DestroyMessage : MessageDispatcher
{
	// Token: 0x06000885 RID: 2181 RVA: 0x0003A598 File Offset: 0x00038798
	private void OnDestroy()
	{
		base.Handler.Invoke();
	}
}
