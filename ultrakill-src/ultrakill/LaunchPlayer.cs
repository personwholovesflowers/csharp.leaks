using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public class LaunchPlayer : MonoBehaviour
{
	// Token: 0x06000EA0 RID: 3744 RVA: 0x0006C664 File Offset: 0x0006A864
	private void Awake()
	{
		this.colliderless = base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null;
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0006C689 File Offset: 0x0006A889
	private void OnEnable()
	{
		if (this.colliderless && !this.dontLaunchOnEnable && (!this.oneTime || !this.beenLaunched))
		{
			this.Launch();
		}
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0006C6B1 File Offset: 0x0006A8B1
	private void OnTriggerEnter(Collider other)
	{
		if (this.oneTime && this.beenLaunched)
		{
			return;
		}
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.Launch();
		}
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0006C6E4 File Offset: 0x0006A8E4
	public void Launch()
	{
		if (!this.beenLaunched)
		{
			this.beenLaunched = true;
		}
		else if (this.oneTime)
		{
			return;
		}
		if (this.relative)
		{
			MonoSingleton<NewMovement>.Instance.Launch(base.transform.rotation * this.direction, 8f, false);
			return;
		}
		MonoSingleton<NewMovement>.Instance.Launch(this.direction, 8f, false);
	}

	// Token: 0x0400135C RID: 4956
	public Vector3 direction;

	// Token: 0x0400135D RID: 4957
	public bool relative;

	// Token: 0x0400135E RID: 4958
	public bool oneTime;

	// Token: 0x0400135F RID: 4959
	private bool beenLaunched;

	// Token: 0x04001360 RID: 4960
	public bool dontLaunchOnEnable;

	// Token: 0x04001361 RID: 4961
	private bool colliderless;
}
