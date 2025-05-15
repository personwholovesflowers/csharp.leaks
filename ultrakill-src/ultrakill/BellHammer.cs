using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class BellHammer : MonoBehaviour
{
	// Token: 0x06000224 RID: 548 RVA: 0x0000B54D File Offset: 0x0000974D
	private void Start()
	{
		this.GetValues();
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000B555 File Offset: 0x00009755
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.joint = base.GetComponent<HingeJoint>();
		this.rb = base.GetComponent<Rigidbody>();
		this.originalRotation = base.transform.localRotation;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000B590 File Offset: 0x00009790
	private void FixedUpdate()
	{
		this.currentFrameVelocity = this.rb.angularVelocity;
		if (this.currentFrameVelocity.magnitude > 0.001f && Vector3.Dot(this.currentFrameVelocity, this.previousFrameVelocity) <= 0f)
		{
			this.Ring();
		}
		else if (this.currentFrameVelocity.magnitude > 1f && this.previousFrameVelocity.magnitude < 0.01f && Vector3.Dot(this.currentFrameVelocity, this.previousFrameVelocity) <= 0.1f)
		{
			this.Ring();
		}
		this.previousFrameVelocity = this.currentFrameVelocity;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000B630 File Offset: 0x00009830
	private void Ring()
	{
		float num = Mathf.Min(1f, Vector3.Distance(this.currentFrameVelocity, this.previousFrameVelocity) / this.speedForMaxVolume);
		AudioSource audioSource = Object.Instantiate<AudioSource>(this.ringSound, base.transform.position, Quaternion.identity);
		audioSource.volume *= num;
		audioSource.Play();
		if ((!this.rung || !this.oneTimeEvent) && num >= 0.5f)
		{
			UltrakillEvent ultrakillEvent = this.onRing;
			if (ultrakillEvent != null)
			{
				ultrakillEvent.Invoke("");
			}
			this.rung = true;
		}
	}

	// Token: 0x04000268 RID: 616
	[HideInInspector]
	public HingeJoint joint;

	// Token: 0x04000269 RID: 617
	public AudioSource ringSound;

	// Token: 0x0400026A RID: 618
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x0400026B RID: 619
	[HideInInspector]
	public bool gotValues;

	// Token: 0x0400026C RID: 620
	[HideInInspector]
	public Quaternion originalRotation;

	// Token: 0x0400026D RID: 621
	private Vector3 previousFrameVelocity;

	// Token: 0x0400026E RID: 622
	private Vector3 currentFrameVelocity;

	// Token: 0x0400026F RID: 623
	[HideInInspector]
	public int hittingLimit;

	// Token: 0x04000270 RID: 624
	public float speedForMaxVolume;

	// Token: 0x04000271 RID: 625
	public bool oneTimeEvent;

	// Token: 0x04000272 RID: 626
	[HideInInspector]
	public bool rung;

	// Token: 0x04000273 RID: 627
	public UltrakillEvent onRing;
}
