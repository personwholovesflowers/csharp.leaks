using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class VerticalClippingBlocker : MonoBehaviour
{
	// Token: 0x06001B21 RID: 6945 RVA: 0x000E1E98 File Offset: 0x000E0098
	private void Awake()
	{
		this.col = base.GetComponent<CapsuleCollider>();
		this.rb = base.GetComponent<Rigidbody>();
		this.nm = base.GetComponent<NewMovement>();
		this.gc = this.nm.gc;
		this.lmask = LayerMaskDefaults.Get(LMD.Environment);
		this.lmask |= 262144;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x000E1F04 File Offset: 0x000E0104
	private void FixedUpdate()
	{
		if (!this.nm.enabled)
		{
			return;
		}
		this.lastVelocity = this.rb.velocity;
		if (this.gc && this.gc.heavyFall)
		{
			this.computedHeavyFallOffset = this.CalculateHeavyFallOffset();
			return;
		}
		this.computedHeavyFallOffset = 0f;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x000E1F68 File Offset: 0x000E0168
	private void LateUpdate()
	{
		if (Time.timeScale <= 0f)
		{
			return;
		}
		bool flag = this.gc && this.gc.heavyFall;
		if (flag)
		{
			this.ceilingDetected = this.PerformCeilingCheck();
		}
		this.PerformClippingCheck(flag);
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x000E1FB4 File Offset: 0x000E01B4
	private bool PerformCeilingCheck()
	{
		Vector3 vector = base.transform.TransformPoint(this.col.center);
		RaycastHit raycastHit;
		return Physics.Raycast(new Ray(vector, Vector3.up), out raycastHit, this.ceilingCheckDistance, this.lmask, QueryTriggerInteraction.Ignore);
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x000E1FFC File Offset: 0x000E01FC
	private void PerformClippingCheck(bool heavyFall)
	{
		Vector3 vector = base.transform.TransformPoint(this.col.center + Vector3.up * (this.col.height * 0.5f - this.col.radius));
		Vector3 vector2 = base.transform.TransformPoint(this.col.center - Vector3.up * (this.col.height * 0.5f - this.col.radius));
		if (heavyFall && !this.ceilingDetected && this.computedHeavyFallOffset > 0f)
		{
			vector += Vector3.up * this.computedHeavyFallOffset;
		}
		Vector3 vector3 = vector2 - vector;
		float magnitude = vector3.magnitude;
		vector3.Normalize();
		RaycastHit raycastHit;
		if (!Physics.Raycast(new Ray(vector, vector3), out raycastHit, magnitude, this.lmask, QueryTriggerInteraction.Ignore))
		{
			return;
		}
		float num = magnitude - raycastHit.distance;
		if (num <= 0f)
		{
			return;
		}
		Vector3 position = this.rb.position;
		float num2 = Mathf.Abs(Vector3.Dot(raycastHit.normal, Vector3.up));
		float num3 = num * num2;
		Vector3 vector4 = new Vector3(position.x, position.y + num3, position.z);
		this.rb.position = vector4;
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x000E2160 File Offset: 0x000E0360
	private float CalculateHeavyFallOffset()
	{
		float num = -this.lastVelocity.y;
		if (num <= 0f)
		{
			return 0f;
		}
		return Mathf.Min(num * Time.fixedDeltaTime, this.heavyFallMaxExtraHeight);
	}

	// Token: 0x0400264F RID: 9807
	private CapsuleCollider col;

	// Token: 0x04002650 RID: 9808
	private LayerMask lmask;

	// Token: 0x04002651 RID: 9809
	private Rigidbody rb;

	// Token: 0x04002652 RID: 9810
	private NewMovement nm;

	// Token: 0x04002653 RID: 9811
	private GroundCheck gc;

	// Token: 0x04002654 RID: 9812
	[SerializeField]
	private float ceilingCheckDistance = 3f;

	// Token: 0x04002655 RID: 9813
	[SerializeField]
	private float heavyFallMaxExtraHeight = 5f;

	// Token: 0x04002656 RID: 9814
	private Vector3 lastVelocity;

	// Token: 0x04002657 RID: 9815
	private float computedHeavyFallOffset;

	// Token: 0x04002658 RID: 9816
	private bool ceilingDetected;
}
