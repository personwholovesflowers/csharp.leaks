using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000332 RID: 818
public class ParticleCollisionSpawn : MonoBehaviour
{
	// Token: 0x060012E5 RID: 4837 RVA: 0x00096548 File Offset: 0x00094748
	private void OnParticleCollision(GameObject other)
	{
		if (this.part == null)
		{
			this.part = base.GetComponent<ParticleSystem>();
		}
		this.part.GetCollisionEvents(other, this.collisionEvents);
		if (this.collisionEvents.Count > 0)
		{
			Object.Instantiate<GameObject>(this.toSpawn, this.collisionEvents[0].intersection, Quaternion.LookRotation(this.collisionEvents[0].normal)).SetActive(true);
		}
	}

	// Token: 0x040019D9 RID: 6617
	private ParticleSystem part;

	// Token: 0x040019DA RID: 6618
	private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

	// Token: 0x040019DB RID: 6619
	public GameObject toSpawn;
}
