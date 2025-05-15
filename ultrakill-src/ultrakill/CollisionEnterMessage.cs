using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
[DisallowMultipleComponent]
public sealed class CollisionEnterMessage : MessageDispatcher<Collision>.Callback<UnityEventCollision>
{
	// Token: 0x0600087D RID: 2173 RVA: 0x0003A56C File Offset: 0x0003876C
	private void OnCollisionEnter(Collision collision)
	{
		this.Handler.Invoke(collision);
	}
}
