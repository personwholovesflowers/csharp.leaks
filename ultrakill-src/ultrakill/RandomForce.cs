using System;
using UnityEngine;

// Token: 0x02000375 RID: 885
public class RandomForce : MonoBehaviour
{
	// Token: 0x06001490 RID: 5264 RVA: 0x000A6AD2 File Offset: 0x000A4CD2
	private void OnEnable()
	{
		if (this.onEnable && (!this.oneTime || !this.applied))
		{
			this.ApplyForce(this.force);
		}
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000A6AF8 File Offset: 0x000A4CF8
	public void ApplyForce()
	{
		this.ApplyForce(this.force);
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x000A6B08 File Offset: 0x000A4D08
	public void ApplyForce(float tempForce)
	{
		this.applied = true;
		base.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * tempForce, ForceMode.VelocityChange);
	}

	// Token: 0x04001C52 RID: 7250
	public float force;

	// Token: 0x04001C53 RID: 7251
	public bool onEnable = true;

	// Token: 0x04001C54 RID: 7252
	public bool oneTime = true;

	// Token: 0x04001C55 RID: 7253
	private bool applied;
}
