using System;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class CopyLineRenderer : MonoBehaviour
{
	// Token: 0x06000492 RID: 1170 RVA: 0x0001FAF7 File Offset: 0x0001DCF7
	private void Start()
	{
		this.lr = base.GetComponent<LineRenderer>();
		this.toCopy = base.transform.parent.GetComponentInParent<LineRenderer>();
		this.origWidth = this.lr.widthMultiplier;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001FB2C File Offset: 0x0001DD2C
	private void Update()
	{
		for (int i = 0; i < this.toCopy.positionCount; i++)
		{
			this.lr.SetPosition(i, this.toCopy.GetPosition(i));
		}
		this.lr.widthMultiplier = this.toCopy.widthMultiplier * this.origWidth;
	}

	// Token: 0x0400063C RID: 1596
	private LineRenderer toCopy;

	// Token: 0x0400063D RID: 1597
	private LineRenderer lr;

	// Token: 0x0400063E RID: 1598
	private float origWidth;
}
