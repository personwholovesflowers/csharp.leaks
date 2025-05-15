using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class FlockWaypointTrigger : MonoBehaviour
{
	// Token: 0x060000BA RID: 186 RVA: 0x000074E8 File Offset: 0x000056E8
	public void Start()
	{
		if (this._flockChild == null)
		{
			this._flockChild = base.transform.parent.GetComponent<FlockChild>();
		}
		float num = Random.Range(this._timer, this._timer * 3f);
		base.InvokeRepeating("Trigger", num, num);
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000753E File Offset: 0x0000573E
	public void Trigger()
	{
		this._flockChild.Wander(0f);
	}

	// Token: 0x0400011D RID: 285
	public float _timer = 1f;

	// Token: 0x0400011E RID: 286
	public FlockChild _flockChild;
}
