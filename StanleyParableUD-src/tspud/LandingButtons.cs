﻿using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class LandingButtons : MonoBehaviour
{
	// Token: 0x060000BD RID: 189 RVA: 0x00007564 File Offset: 0x00005764
	public void OnGUI()
	{
		GUI.Label(new Rect(20f, 20f, 125f, 18f), "Landing Spots: " + this._landingSpotController.transform.childCount);
		if (GUI.Button(new Rect(20f, 40f, 125f, 18f), "Scare All"))
		{
			this._landingSpotController.ScareAll();
		}
		if (GUI.Button(new Rect(20f, 60f, 125f, 18f), "Land In Reach"))
		{
			this._landingSpotController.LandAll();
		}
		if (GUI.Button(new Rect(20f, 80f, 125f, 18f), "Land Instant"))
		{
			base.StartCoroutine(this._landingSpotController.InstantLand(0.01f));
		}
		if (GUI.Button(new Rect(20f, 100f, 125f, 18f), "Destroy"))
		{
			this._flockController.destroyBirds();
		}
		GUI.Label(new Rect(20f, 120f, 125f, 18f), "Bird Amount: " + this._flockController._childAmount);
		this._flockController._childAmount = (int)GUI.HorizontalSlider(new Rect(20f, 140f, 125f, 18f), (float)this._flockController._childAmount, 0f, 250f);
	}

	// Token: 0x0400011F RID: 287
	public LandingSpotController _landingSpotController;

	// Token: 0x04000120 RID: 288
	public FlockController _flockController;

	// Token: 0x04000121 RID: 289
	public float hSliderValue = 250f;
}
