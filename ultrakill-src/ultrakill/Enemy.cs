using System;
using UnityEngine;

// Token: 0x02000189 RID: 393
public class Enemy : MonoBehaviour
{
	// Token: 0x0600079D RID: 1949 RVA: 0x00032B66 File Offset: 0x00030D66
	private void Start()
	{
		this.rbs = base.GetComponentsInChildren<Rigidbody>();
		this.rb = base.GetComponent<Rigidbody>();
		this.anim = base.GetComponent<Animator>();
		this.gc = base.GetComponentInChildren<GroundCheck>();
	}

	// Token: 0x040009DA RID: 2522
	private Rigidbody[] rbs;

	// Token: 0x040009DB RID: 2523
	public bool limp;

	// Token: 0x040009DC RID: 2524
	private Rigidbody rb;

	// Token: 0x040009DD RID: 2525
	private Animator anim;

	// Token: 0x040009DE RID: 2526
	private float currentSpeed;

	// Token: 0x040009DF RID: 2527
	public float coolDown;

	// Token: 0x040009E0 RID: 2528
	public bool damaging;

	// Token: 0x040009E1 RID: 2529
	private TrailRenderer tr;

	// Token: 0x040009E2 RID: 2530
	private bool track;

	// Token: 0x040009E3 RID: 2531
	private AudioSource aud;

	// Token: 0x040009E4 RID: 2532
	private GroundCheck gc;

	// Token: 0x040009E5 RID: 2533
	public bool grounded;

	// Token: 0x040009E6 RID: 2534
	private float defaultSpeed;

	// Token: 0x040009E7 RID: 2535
	public Vector3 agentVelocity;
}
