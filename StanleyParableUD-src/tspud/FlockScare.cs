using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class FlockScare : MonoBehaviour
{
	// Token: 0x060000B3 RID: 179 RVA: 0x00007308 File Offset: 0x00005508
	private void CheckProximityToLandingSpots()
	{
		this.IterateLandingSpots();
		if (this.currentController._activeLandingSpots > 0 && this.CheckDistanceToLandingSpot(this.landingSpotControllers[this.lsc]))
		{
			this.landingSpotControllers[this.lsc].ScareAll();
		}
		base.Invoke("CheckProximityToLandingSpots", this.scareInterval);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00007364 File Offset: 0x00005564
	private void IterateLandingSpots()
	{
		this.ls += this.checkEveryNthLandingSpot;
		this.currentController = this.landingSpotControllers[this.lsc];
		int childCount = this.currentController.transform.childCount;
		if (this.ls > childCount - 1)
		{
			this.ls -= childCount;
			if (this.lsc < this.landingSpotControllers.Length - 1)
			{
				this.lsc++;
				return;
			}
			this.lsc = 0;
		}
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000073EC File Offset: 0x000055EC
	private bool CheckDistanceToLandingSpot(LandingSpotController lc)
	{
		Transform child = lc.transform.GetChild(this.ls);
		return child.GetComponent<LandingSpot>().landingChild != null && (child.position - base.transform.position).sqrMagnitude < this.distanceToScare * this.distanceToScare;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007450 File Offset: 0x00005650
	private void Invoker()
	{
		for (int i = 0; i < this.InvokeAmounts; i++)
		{
			float num = this.scareInterval / (float)this.InvokeAmounts * (float)i;
			base.Invoke("CheckProximityToLandingSpots", this.scareInterval + num);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00007493 File Offset: 0x00005693
	private void OnEnable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
		if (this.landingSpotControllers.Length != 0)
		{
			this.Invoker();
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000074AF File Offset: 0x000056AF
	private void OnDisable()
	{
		base.CancelInvoke("CheckProximityToLandingSpots");
	}

	// Token: 0x04000115 RID: 277
	public LandingSpotController[] landingSpotControllers;

	// Token: 0x04000116 RID: 278
	public float scareInterval = 0.1f;

	// Token: 0x04000117 RID: 279
	public float distanceToScare = 2f;

	// Token: 0x04000118 RID: 280
	public int checkEveryNthLandingSpot = 1;

	// Token: 0x04000119 RID: 281
	public int InvokeAmounts = 1;

	// Token: 0x0400011A RID: 282
	private int lsc;

	// Token: 0x0400011B RID: 283
	private int ls;

	// Token: 0x0400011C RID: 284
	private LandingSpotController currentController;
}
