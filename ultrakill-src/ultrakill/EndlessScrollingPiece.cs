using System;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class EndlessScrollingPiece : MonoBehaviour
{
	// Token: 0x0600079A RID: 1946 RVA: 0x00032AC2 File Offset: 0x00030CC2
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00032AD0 File Offset: 0x00030CD0
	private void FixedUpdate()
	{
		if (Vector3.Distance(base.transform.position, base.transform.parent.position) > this.maxDistance)
		{
			base.transform.position += this.velocity.normalized * -2f * this.maxDistance;
		}
		this.rb.MovePosition(base.transform.position + this.velocity * Time.fixedDeltaTime);
	}

	// Token: 0x040009D7 RID: 2519
	private Rigidbody rb;

	// Token: 0x040009D8 RID: 2520
	public Vector3 velocity;

	// Token: 0x040009D9 RID: 2521
	public float maxDistance;
}
