using System;
using UnityEngine;

// Token: 0x020002D7 RID: 727
public class LineToPoint : MonoBehaviour
{
	// Token: 0x06000FC7 RID: 4039 RVA: 0x00075BC0 File Offset: 0x00073DC0
	private void Update()
	{
		if (this.lr == null)
		{
			this.lr = base.GetComponent<LineRenderer>();
			this.lr.useWorldSpace = true;
		}
		for (int i = 0; i < this.targets.Length; i++)
		{
			this.lr.SetPosition(i, this.targets[i].position);
		}
	}

	// Token: 0x0400155C RID: 5468
	private LineRenderer lr;

	// Token: 0x0400155D RID: 5469
	public Transform[] targets;
}
