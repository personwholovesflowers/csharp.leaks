using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
public class CannonballExtraCollider : MonoBehaviour
{
	// Token: 0x06000334 RID: 820 RVA: 0x00013C39 File Offset: 0x00011E39
	private void OnTriggerEnter(Collider other)
	{
		if (this.source.launched)
		{
			this.source.Collide(other);
		}
	}

	// Token: 0x04000403 RID: 1027
	public Cannonball source;
}
