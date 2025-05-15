using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000369 RID: 873
public class Puppet : MonoBehaviour
{
	// Token: 0x0600144F RID: 5199 RVA: 0x000A4B08 File Offset: 0x000A2D08
	private void Start()
	{
		this.nma = base.GetComponent<NavMeshAgent>();
		this.anim = base.GetComponent<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.SlowUpdate();
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x000A4B58 File Offset: 0x000A2D58
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.25f);
		if (this.eid.target == null || this.inAction || !this.nma.enabled || !this.nma.isOnNavMesh)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(this.eid.target.position, Vector3.down, out raycastHit, 120f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.nma.SetDestination(raycastHit.point);
		}
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000A4BE8 File Offset: 0x000A2DE8
	private void Update()
	{
		Vector3 vector = ((this.eid.target == null) ? (base.transform.position + base.transform.forward) : new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		if (!this.inAction && this.eid.target != null)
		{
			if (Vector3.Distance(base.transform.position, vector) < 5f)
			{
				this.Swing();
			}
		}
		else if (this.moving)
		{
			this.rb.MovePosition(base.transform.position + base.transform.forward * Time.deltaTime * 15f);
		}
		this.anim.SetBool("Walking", !this.inAction && this.nma.velocity.magnitude > 1.5f);
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x000A4D0C File Offset: 0x000A2F0C
	private void Swing()
	{
		this.inAction = true;
		this.nma.enabled = false;
		this.anim.Play("Swing", -1, 0f);
		if (this.eid.target != null)
		{
			base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000A4D99 File Offset: 0x000A2F99
	private void DamageStart()
	{
		this.sc.DamageStart();
		this.moving = true;
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x000A4DAD File Offset: 0x000A2FAD
	private void DamageStop()
	{
		this.sc.DamageStop();
		this.moving = false;
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x000A4DC1 File Offset: 0x000A2FC1
	private void StopAction()
	{
		this.inAction = false;
		if (this.mach.gc.onGround)
		{
			this.nma.enabled = true;
		}
	}

	// Token: 0x04001BE1 RID: 7137
	private NavMeshAgent nma;

	// Token: 0x04001BE2 RID: 7138
	[SerializeField]
	private SwingCheck2 sc;

	// Token: 0x04001BE3 RID: 7139
	private Animator anim;

	// Token: 0x04001BE4 RID: 7140
	private EnemyIdentifier eid;

	// Token: 0x04001BE5 RID: 7141
	private Machine mach;

	// Token: 0x04001BE6 RID: 7142
	private Rigidbody rb;

	// Token: 0x04001BE7 RID: 7143
	private bool inAction;

	// Token: 0x04001BE8 RID: 7144
	private bool moving;
}
