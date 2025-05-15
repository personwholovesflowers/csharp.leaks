using System;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class Shockwave : MonoBehaviour
{
	// Token: 0x060016AB RID: 5803 RVA: 0x000B6088 File Offset: 0x000B4288
	private void Start()
	{
		base.Invoke("TimeToDie", this.lifeTime);
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000B609C File Offset: 0x000B429C
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Breakable"))
		{
			Breakable component = other.gameObject.GetComponent<Breakable>();
			if (component != null && ((component.weak && !component.precisionOnly && !component.specialCaseOnly) || (this.groundSlam && component.forceGroundSlammable)))
			{
				component.Break();
			}
		}
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000B60FE File Offset: 0x000B42FE
	private void TimeToDie()
	{
		Object.Destroy(this);
	}

	// Token: 0x04001F70 RID: 8048
	public bool groundSlam;

	// Token: 0x04001F71 RID: 8049
	public float lifeTime;
}
