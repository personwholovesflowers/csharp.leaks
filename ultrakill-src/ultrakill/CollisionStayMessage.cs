using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[DisallowMultipleComponent]
public sealed class CollisionStayMessage : MessageDispatcher<Collision>.Callback<UnityEventCollision>
{
	// Token: 0x06000881 RID: 2177 RVA: 0x0003A56C File Offset: 0x0003876C
	private void OnCollisionStay(Collision collision)
	{
		this.Handler.Invoke(collision);
	}
}
