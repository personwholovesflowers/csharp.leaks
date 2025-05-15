using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
[DisallowMultipleComponent]
public sealed class ControllerColliderHitMessage : MessageDispatcher<ControllerColliderHit>.Callback<UnityEventControllerColliderHit>
{
	// Token: 0x06000883 RID: 2179 RVA: 0x0003A582 File Offset: 0x00038782
	private void OnControllerColliderHit(ControllerColliderHit collision)
	{
		this.Handler.Invoke(collision);
	}
}
