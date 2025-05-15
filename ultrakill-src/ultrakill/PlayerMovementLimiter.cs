using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class PlayerMovementLimiter : MonoBehaviour
{
	// Token: 0x06001348 RID: 4936 RVA: 0x0009B758 File Offset: 0x00099958
	private void Awake()
	{
		this.rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0009B766 File Offset: 0x00099966
	private void LateUpdate()
	{
		this.lastVel = this.rigidbody.velocity;
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0009B77C File Offset: 0x0009997C
	private bool AnimatorCheck(Collision collision)
	{
		Animator animator;
		return collision.gameObject.TryGetComponent<Animator>(out animator) || (collision.transform.parent && collision.transform.parent.TryGetComponent<Animator>(out animator));
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x0009B7C4 File Offset: 0x000999C4
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.AnimatorCheck(collision))
		{
			return;
		}
		if ((this.lastVel - this.rigidbody.velocity).magnitude > this.animatorInteractionSpeedCap)
		{
			base.StartCoroutine(this.CancelOnNext(this.lastVel, this.lastVel - this.rigidbody.velocity));
		}
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x0009B82C File Offset: 0x00099A2C
	private void OnCollisionStay(Collision collision)
	{
		if (!this.AnimatorCheck(collision))
		{
			return;
		}
		if ((this.lastVel - this.rigidbody.velocity).magnitude > this.animatorInteractionSpeedCap)
		{
			base.StartCoroutine(this.CancelOnNext(this.lastVel, this.lastVel - this.rigidbody.velocity));
		}
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x0009B892 File Offset: 0x00099A92
	private IEnumerator CancelOnNext(Vector3 lastVelocity, Vector3 newDelta)
	{
		this.rigidbody.velocity = lastVelocity;
		yield return new WaitForFixedUpdate();
		this.rigidbody.velocity = lastVelocity;
		yield break;
	}

	// Token: 0x04001AA6 RID: 6822
	[SerializeField]
	private float animatorInteractionSpeedCap = 30f;

	// Token: 0x04001AA7 RID: 6823
	private Rigidbody rigidbody;

	// Token: 0x04001AA8 RID: 6824
	private Vector3 lastVel;
}
