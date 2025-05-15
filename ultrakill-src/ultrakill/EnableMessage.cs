using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
[DisallowMultipleComponent]
public sealed class EnableMessage : MessageDispatcher
{
	// Token: 0x06000889 RID: 2185 RVA: 0x0003A598 File Offset: 0x00038798
	private void OnEnable()
	{
		base.Handler.Invoke();
	}
}
