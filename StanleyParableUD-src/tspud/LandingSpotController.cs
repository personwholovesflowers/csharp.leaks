using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class LandingSpotController : MonoBehaviour
{
	// Token: 0x060000C8 RID: 200 RVA: 0x000081B4 File Offset: 0x000063B4
	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._flock == null)
		{
			this._flock = (FlockController)Object.FindObjectOfType(typeof(FlockController));
			Debug.Log(this + " has no assigned FlockController, a random FlockController has been assigned");
		}
		if (this._landOnStart)
		{
			base.StartCoroutine(this.InstantLandOnStart(0.1f));
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000822D File Offset: 0x0000642D
	public void ScareAll()
	{
		this.ScareAll(0f, 1f);
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00008240 File Offset: 0x00006440
	public void ScareAll(float minDelay, float maxDelay)
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().Invoke("ReleaseFlockChild", Random.Range(minDelay, maxDelay));
			}
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000082A0 File Offset: 0x000064A0
	public void LandAll()
	{
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				LandingSpot component = this._thisT.GetChild(i).GetComponent<LandingSpot>();
				base.StartCoroutine(component.GetFlockChild(0f, 2f));
			}
		}
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00008305 File Offset: 0x00006505
	public IEnumerator InstantLandOnStart(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000831B File Offset: 0x0000651B
	public IEnumerator InstantLand(float delay)
	{
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < this._thisT.childCount; i++)
		{
			if (this._thisT.GetChild(i).GetComponent<LandingSpot>() != null)
			{
				this._thisT.GetChild(i).GetComponent<LandingSpot>().InstantLand();
			}
		}
		yield break;
	}

	// Token: 0x04000129 RID: 297
	public bool _randomRotate = true;

	// Token: 0x0400012A RID: 298
	public Vector2 _autoCatchDelay = new Vector2(10f, 20f);

	// Token: 0x0400012B RID: 299
	public Vector2 _autoDismountDelay = new Vector2(10f, 20f);

	// Token: 0x0400012C RID: 300
	public float _maxBirdDistance = 20f;

	// Token: 0x0400012D RID: 301
	public float _minBirdDistance = 5f;

	// Token: 0x0400012E RID: 302
	public bool _takeClosest;

	// Token: 0x0400012F RID: 303
	public FlockController _flock;

	// Token: 0x04000130 RID: 304
	public bool _landOnStart;

	// Token: 0x04000131 RID: 305
	public bool _soarLand = true;

	// Token: 0x04000132 RID: 306
	public bool _onlyBirdsAbove;

	// Token: 0x04000133 RID: 307
	public float _landingSpeedModifier = 0.5f;

	// Token: 0x04000134 RID: 308
	public float _landingTurnSpeedModifier = 5f;

	// Token: 0x04000135 RID: 309
	public Transform _featherPS;

	// Token: 0x04000136 RID: 310
	public Transform _thisT;

	// Token: 0x04000137 RID: 311
	public int _activeLandingSpots;

	// Token: 0x04000138 RID: 312
	public float _snapLandDistance = 0.1f;

	// Token: 0x04000139 RID: 313
	public float _landedRotateSpeed = 0.01f;

	// Token: 0x0400013A RID: 314
	public float _gizmoSize = 0.2f;
}
