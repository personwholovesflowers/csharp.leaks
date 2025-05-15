using System;
using UnityEngine;

// Token: 0x0200040A RID: 1034
public class SisyphusPrimeIntro : MonoBehaviour
{
	// Token: 0x0600179D RID: 6045 RVA: 0x000C2138 File Offset: 0x000C0338
	private void Start()
	{
		this.ts = 0f;
		Vector3 position = MonoSingleton<PlayerTracker>.Instance.GetPlayer().position;
		position.y = base.transform.position.y;
		base.transform.rotation = Quaternion.LookRotation(position - base.transform.position);
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000C21A0 File Offset: 0x000C03A0
	private void Update()
	{
		if (this.tracking)
		{
			Vector3 position = MonoSingleton<PlayerTracker>.Instance.GetPlayer().position;
			position.y = base.transform.position.y;
			Quaternion quaternion = Quaternion.LookRotation(position - base.transform.position);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 10f * Quaternion.Angle(base.transform.rotation, quaternion));
		}
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000C222C File Offset: 0x000C042C
	private void FixedUpdate()
	{
		if (!this.hasHitGround)
		{
			if (!this.rb)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			this.rb.velocity -= Vector3.up * Mathf.Lerp(0f, 100f, this.ts) * Time.fixedDeltaTime;
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x000C22A0 File Offset: 0x000C04A0
	private void OnCollisionEnter(Collision col)
	{
		if (this.hasHitGround)
		{
			return;
		}
		if (LayerMaskDefaults.IsMatchingLayer(col.gameObject.layer, LMD.Environment))
		{
			this.hasHitGround = true;
			base.GetComponent<Animator>().Play("Intro");
			Object.Instantiate<GameObject>(this.groundImpactEffect, base.transform.position, Quaternion.identity);
			UltrakillEvent ultrakillEvent = this.onGroundImpact;
			if (ultrakillEvent != null)
			{
				ultrakillEvent.Invoke("");
			}
			this.rb.isKinematic = true;
			base.gameObject.layer = 16;
			this.tracking = true;
		}
	}

	// Token: 0x040020F7 RID: 8439
	public GameObject groundImpactEffect;

	// Token: 0x040020F8 RID: 8440
	public UltrakillEvent onGroundImpact;

	// Token: 0x040020F9 RID: 8441
	private bool hasHitGround;

	// Token: 0x040020FA RID: 8442
	private Rigidbody rb;

	// Token: 0x040020FB RID: 8443
	private TimeSince ts;

	// Token: 0x040020FC RID: 8444
	private bool tracking;
}
