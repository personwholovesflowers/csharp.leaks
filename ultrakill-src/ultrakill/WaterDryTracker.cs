using System;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class WaterDryTracker
{
	// Token: 0x0600164A RID: 5706 RVA: 0x000B3600 File Offset: 0x000B1800
	public WaterDryTracker(Transform tf, Vector3 clopo)
	{
		this.transform = tf;
		this.closestPosition = clopo;
	}

	// Token: 0x04001EAA RID: 7850
	public Transform transform;

	// Token: 0x04001EAB RID: 7851
	public Vector3 closestPosition;
}
