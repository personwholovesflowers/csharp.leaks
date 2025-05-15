using System;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class AttackTrail : MonoBehaviour
{
	// Token: 0x060001F7 RID: 503 RVA: 0x0000A674 File Offset: 0x00008874
	private void Update()
	{
		if (this.target && this.pivot)
		{
			Vector3 position = this.target.position;
			Vector3 vector = this.target.position + (this.target.position - this.pivot.position).normalized * (float)this.distance;
			Quaternion quaternion = Quaternion.LookRotation(base.transform.position - position);
			base.transform.SetPositionAndRotation(vector, quaternion);
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000A70B File Offset: 0x0000890B
	public void DelayedDestroy(float time)
	{
		base.Invoke("DestroyNow", time);
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000A719 File Offset: 0x00008919
	private void DestroyNow()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000219 RID: 537
	public Transform target;

	// Token: 0x0400021A RID: 538
	public Transform pivot;

	// Token: 0x0400021B RID: 539
	public int distance;
}
