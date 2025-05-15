using System;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class SetFishZone : MonoBehaviour
{
	// Token: 0x06000A0E RID: 2574 RVA: 0x0004591E File Offset: 0x00043B1E
	private void OnTriggerEnter(Collider other)
	{
		if (this.onEnter && other.gameObject.CompareTag("Player"))
		{
			this.Set();
		}
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00045940 File Offset: 0x00043B40
	private void OnTriggerExit(Collider other)
	{
		if (this.restorePreviousOnExit && other.gameObject.CompareTag("Player"))
		{
			this.Restore();
		}
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00045962 File Offset: 0x00043B62
	public void Set()
	{
		this.previousFishingDistance = FishingRodWeapon.suggestedDistanceMulti;
		if (this.customMinDistance)
		{
			this.previousMinDistance = FishingRodWeapon.minDistanceMulti;
		}
		FishingRodWeapon.suggestedDistanceMulti = this.suggestedFishingDistance;
		if (this.customMinDistance)
		{
			FishingRodWeapon.minDistanceMulti = this.minDistance;
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x000459A0 File Offset: 0x00043BA0
	public void Restore()
	{
		FishingRodWeapon.suggestedDistanceMulti = this.previousFishingDistance;
		if (this.customMinDistance)
		{
			FishingRodWeapon.minDistanceMulti = this.previousMinDistance;
		}
	}

	// Token: 0x04000D21 RID: 3361
	[SerializeField]
	private bool onEnter = true;

	// Token: 0x04000D22 RID: 3362
	[SerializeField]
	private bool restorePreviousOnExit = true;

	// Token: 0x04000D23 RID: 3363
	public float suggestedFishingDistance = 1f;

	// Token: 0x04000D24 RID: 3364
	private float previousFishingDistance;

	// Token: 0x04000D25 RID: 3365
	private float previousMinDistance;

	// Token: 0x04000D26 RID: 3366
	[SerializeField]
	private bool customMinDistance;

	// Token: 0x04000D27 RID: 3367
	[SerializeField]
	private float minDistance = 1f;
}
