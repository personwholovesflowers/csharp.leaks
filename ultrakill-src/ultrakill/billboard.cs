using System;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class billboard : MonoBehaviour
{
	// Token: 0x0600023B RID: 571 RVA: 0x0000BEC7 File Offset: 0x0000A0C7
	private void Start()
	{
		if (billboard.cam == null)
		{
			billboard.cam = MonoSingleton<CameraController>.Instance.GetComponent<Transform>();
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
	private void LateUpdate()
	{
		base.transform.LookAt(billboard.cam);
		base.transform.Rotate(0f, 180f, 0f);
		this.eangles = base.transform.eulerAngles;
		this.eangles.x = this.eangles.x * this.freeRotation.x;
		this.eangles.y = this.eangles.y * this.freeRotation.y;
		this.eangles.z = this.eangles.z * this.freeRotation.z;
		base.transform.eulerAngles = this.eangles;
	}

	// Token: 0x0400028D RID: 653
	public static Transform cam;

	// Token: 0x0400028E RID: 654
	public Vector3 freeRotation = Vector3.one;

	// Token: 0x0400028F RID: 655
	private Vector3 eangles = Vector3.zero;
}
