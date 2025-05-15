using System;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class RotateToTarget : MonoBehaviour
{
	// Token: 0x06001518 RID: 5400 RVA: 0x000ACE20 File Offset: 0x000AB020
	private void OnEnable()
	{
		if (this.onEnable && (!this.oneTime || !this.beenActivated))
		{
			this.Rotate();
		}
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000ACE40 File Offset: 0x000AB040
	public void Rotate()
	{
		if (this.beenActivated && this.oneTime)
		{
			return;
		}
		this.beenActivated = true;
		this.rotating = true;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000ACE64 File Offset: 0x000AB064
	private void Update()
	{
		if (this.rotating)
		{
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, Quaternion.Euler(this.target), this.speed * Time.deltaTime * (this.easeTowards ? (Mathf.Min(Quaternion.Angle(base.transform.localRotation, Quaternion.Euler(this.target)) + this.speed / 10f, this.speed) / this.speed) : 1f));
			if (Quaternion.Angle(base.transform.localRotation, Quaternion.Euler(this.target)) < 0.1f)
			{
				base.transform.localRotation = Quaternion.Euler(this.target);
				this.rotating = false;
				UltrakillEvent ultrakillEvent = this.onComplete;
				if (ultrakillEvent == null)
				{
					return;
				}
				ultrakillEvent.Invoke("");
			}
		}
	}

	// Token: 0x04001D59 RID: 7513
	public Vector3 target;

	// Token: 0x04001D5A RID: 7514
	public bool onEnable;

	// Token: 0x04001D5B RID: 7515
	public bool oneTime;

	// Token: 0x04001D5C RID: 7516
	private bool beenActivated;

	// Token: 0x04001D5D RID: 7517
	private bool rotating;

	// Token: 0x04001D5E RID: 7518
	public float speed;

	// Token: 0x04001D5F RID: 7519
	public bool easeTowards;

	// Token: 0x04001D60 RID: 7520
	public UltrakillEvent onComplete;
}
