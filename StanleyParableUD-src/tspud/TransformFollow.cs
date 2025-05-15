using System;
using UnityEngine;

// Token: 0x020001B5 RID: 437
public class TransformFollow : MonoBehaviour
{
	// Token: 0x06000A2C RID: 2604 RVA: 0x00030122 File Offset: 0x0002E322
	private void Update()
	{
		if (base.transform.position != this.follow.position)
		{
			base.transform.position = this.follow.position;
		}
	}

	// Token: 0x04000A28 RID: 2600
	public Transform follow;
}
