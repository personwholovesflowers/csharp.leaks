using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
public class EntererTracker
{
	// Token: 0x06001194 RID: 4500 RVA: 0x0008929A File Offset: 0x0008749A
	public EntererTracker(GameObject newTarget, Vector3 newPosition)
	{
		this.target = newTarget;
		this.position = newPosition;
	}

	// Token: 0x040017F7 RID: 6135
	public GameObject target;

	// Token: 0x040017F8 RID: 6136
	public int amount;

	// Token: 0x040017F9 RID: 6137
	public Vector3 position;
}
