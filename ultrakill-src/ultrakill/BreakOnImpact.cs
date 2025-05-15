using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class BreakOnImpact : MonoBehaviour
{
	// Token: 0x060002EC RID: 748 RVA: 0x0001146C File Offset: 0x0000F66C
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			return;
		}
		if (collision.relativeVelocity.magnitude < this.minImpactForce)
		{
			return;
		}
		base.GetComponent<Breakable>().Break();
	}

	// Token: 0x0400038D RID: 909
	[SerializeField]
	private float minImpactForce = 1f;
}
