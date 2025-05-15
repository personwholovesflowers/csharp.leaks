using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class DragBehind : MonoBehaviour
{
	// Token: 0x06000569 RID: 1385 RVA: 0x000244F9 File Offset: 0x000226F9
	private void Awake()
	{
		this.previousPosition = base.transform.position;
		this.previousRotation = base.transform.rotation;
		this.defaultRotation = base.transform.localRotation;
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x00024530 File Offset: 0x00022730
	private void LateUpdate()
	{
		if (this.active)
		{
			this.currentRotation = base.transform.rotation;
			Quaternion quaternion = Quaternion.LookRotation(this.previousPosition - base.transform.position, base.transform.right);
			base.transform.rotation = quaternion;
			base.transform.up = base.transform.forward;
			this.nextRotation = Quaternion.Lerp(this.currentRotation, base.transform.rotation, Vector3.Distance(base.transform.position, this.previousPosition) / 5f);
			if (this.notAnimated)
			{
				base.transform.rotation = Quaternion.Lerp(Quaternion.RotateTowards(this.previousRotation, this.nextRotation, Time.deltaTime * 1000f), base.transform.parent.rotation * this.defaultRotation, this.dragAmount);
			}
			else
			{
				base.transform.rotation = Quaternion.Lerp(Quaternion.RotateTowards(this.previousRotation, this.nextRotation, Time.deltaTime * 1000f), this.currentRotation, this.dragAmount);
			}
		}
		this.previousPosition = Vector3.MoveTowards(this.previousPosition, base.transform.position, Time.deltaTime * (Vector3.Distance(this.previousPosition, base.transform.position) * 10f));
		this.previousRotation = base.transform.rotation;
	}

	// Token: 0x04000774 RID: 1908
	private Vector3 previousPosition;

	// Token: 0x04000775 RID: 1909
	private Quaternion currentRotation;

	// Token: 0x04000776 RID: 1910
	private Quaternion nextRotation;

	// Token: 0x04000777 RID: 1911
	private Quaternion previousRotation;

	// Token: 0x04000778 RID: 1912
	public bool active;

	// Token: 0x04000779 RID: 1913
	public bool notAnimated;

	// Token: 0x0400077A RID: 1914
	public float dragAmount;

	// Token: 0x0400077B RID: 1915
	private Quaternion defaultRotation;
}
