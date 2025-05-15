using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
[DisallowMultipleComponent]
public sealed class CollisionExitMessage : MessageDispatcher<Collision>.Callback<UnityEventCollision>
{
	// Token: 0x0600087F RID: 2175 RVA: 0x0003A56C File Offset: 0x0003876C
	private void OnCollisionExit(Collision collision)
	{
		this.Handler.Invoke(collision);
	}
}
