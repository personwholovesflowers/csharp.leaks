using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DE RID: 478
public abstract class SimpleFSMState
{
	// Token: 0x06000AED RID: 2797 RVA: 0x000337D7 File Offset: 0x000319D7
	public SimpleFSMState(SimpleFSMStateController controller)
	{
		this.cntrl = controller;
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x000337E6 File Offset: 0x000319E6
	public virtual IEnumerator Begin()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x000337EE File Offset: 0x000319EE
	public virtual IEnumerator Exit()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void Setup()
	{
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void Reason()
	{
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void DoUpdate()
	{
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void DoFixedUpdate()
	{
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void DoLateUpdate()
	{
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void UpdateAnimator()
	{
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void OnCollisionState(Collision col)
	{
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void OnTriggerEnterState(Collider col)
	{
	}

	// Token: 0x04000ADF RID: 2783
	public SimpleFSMStateController cntrl;
}
