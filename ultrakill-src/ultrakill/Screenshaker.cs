using System;
using UnityEngine;

// Token: 0x020003CD RID: 973
public class Screenshaker : MonoBehaviour
{
	// Token: 0x06001615 RID: 5653 RVA: 0x000B291D File Offset: 0x000B0B1D
	private void Awake()
	{
		this.colliderless = base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null;
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x000B2942 File Offset: 0x000B0B42
	private void OnEnable()
	{
		if (this.colliderless)
		{
			this.Shake();
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x000B2952 File Offset: 0x000B0B52
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.Shake();
		}
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x000B2971 File Offset: 0x000B0B71
	private void Update()
	{
		if (this.continuous && base.gameObject.activeInHierarchy)
		{
			MonoSingleton<CameraController>.Instance.CameraShake((this.maxDistance == 0f) ? this.amount : this.GetDistanceValue());
		}
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x000B29B0 File Offset: 0x000B0BB0
	public void Shake()
	{
		if (this.oneTime && this.alreadyShaken)
		{
			return;
		}
		float distanceValue = this.amount;
		if (this.maxDistance != 0f)
		{
			distanceValue = this.GetDistanceValue();
		}
		this.alreadyShaken = true;
		MonoSingleton<CameraController>.Instance.CameraShake(distanceValue);
		if (this.oneTime && !this.continuous)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x000B2A14 File Offset: 0x000B0C14
	private float GetDistanceValue()
	{
		return Mathf.Lerp(this.amount, 0f, (Vector3.Distance(MonoSingleton<CameraController>.Instance.transform.position, base.transform.position) - this.minDistance) / (this.maxDistance - this.minDistance));
	}

	// Token: 0x04001E64 RID: 7780
	public float amount;

	// Token: 0x04001E65 RID: 7781
	public bool oneTime;

	// Token: 0x04001E66 RID: 7782
	public bool continuous;

	// Token: 0x04001E67 RID: 7783
	private bool alreadyShaken;

	// Token: 0x04001E68 RID: 7784
	private bool colliderless;

	// Token: 0x04001E69 RID: 7785
	public float minDistance;

	// Token: 0x04001E6A RID: 7786
	public float maxDistance;
}
