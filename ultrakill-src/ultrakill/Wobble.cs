using System;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class Wobble : MonoBehaviour
{
	// Token: 0x06001C20 RID: 7200 RVA: 0x000E97C0 File Offset: 0x000E79C0
	private void Update()
	{
		Quaternion quaternion = Quaternion.Euler(this.rotations[this.rotations.Length - 1]);
		if (this.targetRotation > 0)
		{
			quaternion = Quaternion.Euler(this.rotations[this.targetRotation - 1]);
		}
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(this.rotations[this.targetRotation]), (Mathf.Min(new float[]
		{
			this.speed,
			Quaternion.Angle(base.transform.rotation, Quaternion.Euler(this.rotations[this.targetRotation])),
			Quaternion.Angle(base.transform.rotation, quaternion)
		}) + 0.1f) * Time.deltaTime);
		if (Quaternion.Angle(base.transform.rotation, Quaternion.Euler(this.rotations[this.targetRotation])) < 0.1f)
		{
			if (this.targetRotation + 1 < this.rotations.Length)
			{
				this.targetRotation++;
				return;
			}
			this.targetRotation = 0;
		}
	}

	// Token: 0x040027AA RID: 10154
	public Vector3[] rotations;

	// Token: 0x040027AB RID: 10155
	private int targetRotation;

	// Token: 0x040027AC RID: 10156
	public float speed;
}
