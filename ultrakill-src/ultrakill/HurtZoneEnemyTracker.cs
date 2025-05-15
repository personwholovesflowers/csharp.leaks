using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class HurtZoneEnemyTracker
{
	// Token: 0x06000D42 RID: 3394 RVA: 0x00064BCF File Offset: 0x00062DCF
	public HurtZoneEnemyTracker(EnemyIdentifier eid, Collider limb, float hurtCooldown)
	{
		this.target = eid;
		this.limbs.Add(limb);
		this.timer = hurtCooldown;
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x00064BFC File Offset: 0x00062DFC
	public bool HasLimbs(Collider colliderToCheck)
	{
		if (this.limbs.Count == 0)
		{
			return false;
		}
		for (int i = this.limbs.Count - 1; i >= 0; i--)
		{
			Vector3 vector;
			float num;
			if (!(this.limbs[i] == null) && this.limbs[i].enabled && !(this.limbs[i].transform.localScale == Vector3.zero) && this.limbs[i].gameObject.activeInHierarchy && (!colliderToCheck || Physics.ComputePenetration(colliderToCheck, colliderToCheck.transform.position, colliderToCheck.transform.rotation, this.limbs[i], this.limbs[i].transform.position, this.limbs[i].transform.rotation, out vector, out num)))
			{
				return true;
			}
			this.limbs.RemoveAt(i);
		}
		return false;
	}

	// Token: 0x040011D8 RID: 4568
	public EnemyIdentifier target;

	// Token: 0x040011D9 RID: 4569
	public List<Collider> limbs = new List<Collider>();

	// Token: 0x040011DA RID: 4570
	public float timer;
}
