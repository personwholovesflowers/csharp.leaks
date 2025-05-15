using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class LandingSpot : MonoBehaviour
{
	// Token: 0x060000BF RID: 191 RVA: 0x00007708 File Offset: 0x00005908
	public void Start()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._controller == null)
		{
			this._controller = this._thisT.parent.GetComponent<LandingSpotController>();
		}
		if (this._controller._autoCatchDelay.x > 0f)
		{
			base.StartCoroutine(this.GetFlockChild(this._controller._autoCatchDelay.x, this._controller._autoCatchDelay.y));
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00007798 File Offset: 0x00005998
	public void OnDrawGizmos()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._controller == null)
		{
			this._controller = this._thisT.parent.GetComponent<LandingSpotController>();
		}
		Gizmos.color = Color.yellow;
		if (this.landingChild != null && this.landing)
		{
			Gizmos.DrawLine(this._thisT.position, this.landingChild._thisT.position);
		}
		if (this._thisT.rotation.eulerAngles.x != 0f || this._thisT.rotation.eulerAngles.z != 0f)
		{
			this._thisT.eulerAngles = new Vector3(0f, this._thisT.eulerAngles.y, 0f);
		}
		Gizmos.DrawCube(new Vector3(this._thisT.position.x, this._thisT.position.y, this._thisT.position.z), Vector3.one * this._controller._gizmoSize);
		Gizmos.DrawCube(this._thisT.position + this._thisT.forward * this._controller._gizmoSize, Vector3.one * this._controller._gizmoSize * 0.5f);
		Gizmos.color = new Color(1f, 1f, 0f, 0.05f);
		Gizmos.DrawWireSphere(this._thisT.position, this._controller._maxBirdDistance);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00007964 File Offset: 0x00005B64
	public void LateUpdate()
	{
		if (this.landingChild == null)
		{
			this._gotcha = false;
			this._idle = false;
			this.lerpCounter = 0;
			return;
		}
		if (this._gotcha)
		{
			this.landingChild.transform.position = this._thisT.position + this.landingChild._landingPosOffset;
			this.RotateBird();
			return;
		}
		if (this._controller._flock.gameObject.activeInHierarchy && this.landing && this.landingChild != null)
		{
			if (!this.landingChild.gameObject.activeInHierarchy)
			{
				base.Invoke("ReleaseFlockChild", 0f);
			}
			float num = Vector3.Distance(this.landingChild._thisT.position, this._thisT.position + this.landingChild._landingPosOffset);
			if (num < 5f && num > 0.5f)
			{
				if (this._controller._soarLand)
				{
					this.landingChild._model.GetComponent<Animation>().CrossFade(this.landingChild._spawner._soarAnimation, 0.5f);
					if (num < 2f)
					{
						this.landingChild._model.GetComponent<Animation>().CrossFade(this.landingChild._spawner._flapAnimation, 0.5f);
					}
				}
				this.landingChild._targetSpeed = this.landingChild._spawner._maxSpeed * this._controller._landingSpeedModifier;
				this.landingChild._wayPoint = this._thisT.position + this.landingChild._landingPosOffset;
				this.landingChild._damping = this._controller._landingTurnSpeedModifier;
				this.landingChild._avoid = false;
			}
			else if (num <= 0.5f)
			{
				this.landingChild._wayPoint = this._thisT.position + this.landingChild._landingPosOffset;
				if (num < this._controller._snapLandDistance && !this._idle)
				{
					this._idle = true;
					this.landingChild._model.GetComponent<Animation>().CrossFade(this.landingChild._spawner._idleAnimation, 0.55f);
				}
				if (num > this._controller._snapLandDistance)
				{
					this.landingChild._targetSpeed = this.landingChild._spawner._minSpeed * this._controller._landingSpeedModifier;
					this.landingChild._thisT.position += (this._thisT.position + this.landingChild._landingPosOffset - this.landingChild._thisT.position) * Time.deltaTime * this.landingChild._speed * this._controller._landingSpeedModifier * 2f;
				}
				else
				{
					this._gotcha = true;
				}
				this.landingChild._move = false;
				this.RotateBird();
			}
			else
			{
				this.landingChild._wayPoint = this._thisT.position + this.landingChild._landingPosOffset;
			}
			this.landingChild._damping += 0.01f;
		}
		this.StraightenBird();
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00007CDC File Offset: 0x00005EDC
	public void StraightenBird()
	{
		if (this.landingChild._thisT.eulerAngles.x == 0f)
		{
			return;
		}
		Vector3 eulerAngles = this.landingChild._thisT.eulerAngles;
		eulerAngles.z = 0f;
		this.landingChild._thisT.eulerAngles = eulerAngles;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00007D34 File Offset: 0x00005F34
	public void RotateBird()
	{
		if (this._controller._randomRotate && this._idle)
		{
			return;
		}
		this.lerpCounter++;
		Quaternion rotation = this.landingChild._thisT.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.y = Mathf.LerpAngle(this.landingChild._thisT.rotation.eulerAngles.y, this._thisT.rotation.eulerAngles.y, (float)this.lerpCounter * Time.deltaTime * this._controller._landedRotateSpeed);
		rotation.eulerAngles = eulerAngles;
		this.landingChild._thisT.rotation = rotation;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007DF1 File Offset: 0x00005FF1
	public IEnumerator GetFlockChild(float minDelay, float maxDelay)
	{
		yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
		if (this._controller._flock.gameObject.activeInHierarchy && this.landingChild == null)
		{
			FlockChild flockChild = null;
			for (int i = 0; i < this._controller._flock._roamers.Count; i++)
			{
				FlockChild flockChild2 = this._controller._flock._roamers[i];
				if (!flockChild2._landing && !flockChild2._dived)
				{
					if (!this._controller._onlyBirdsAbove)
					{
						if (flockChild == null && this._controller._maxBirdDistance > Vector3.Distance(flockChild2._thisT.position, this._thisT.position) && this._controller._minBirdDistance < Vector3.Distance(flockChild2._thisT.position, this._thisT.position))
						{
							flockChild = flockChild2;
							if (!this._controller._takeClosest)
							{
								break;
							}
						}
						else if (flockChild != null && Vector3.Distance(flockChild._thisT.position, this._thisT.position) > Vector3.Distance(flockChild2._thisT.position, this._thisT.position))
						{
							flockChild = flockChild2;
						}
					}
					else if (flockChild == null && flockChild2._thisT.position.y > this._thisT.position.y && this._controller._maxBirdDistance > Vector3.Distance(flockChild2._thisT.position, this._thisT.position) && this._controller._minBirdDistance < Vector3.Distance(flockChild2._thisT.position, this._thisT.position))
					{
						flockChild = flockChild2;
						if (!this._controller._takeClosest)
						{
							break;
						}
					}
					else if (flockChild != null && flockChild2._thisT.position.y > this._thisT.position.y && Vector3.Distance(flockChild._thisT.position, this._thisT.position) > Vector3.Distance(flockChild2._thisT.position, this._thisT.position))
					{
						flockChild = flockChild2;
					}
				}
			}
			if (flockChild != null)
			{
				this.landingChild = flockChild;
				this.landing = true;
				this.landingChild._landing = true;
				if (this._controller._autoDismountDelay.x > 0f)
				{
					base.Invoke("ReleaseFlockChild", Random.Range(this._controller._autoDismountDelay.x, this._controller._autoDismountDelay.y));
				}
				this._controller._activeLandingSpots++;
			}
			else if (this._controller._autoCatchDelay.x > 0f)
			{
				base.StartCoroutine(this.GetFlockChild(this._controller._autoCatchDelay.x, this._controller._autoCatchDelay.y));
			}
		}
		yield break;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007E10 File Offset: 0x00006010
	public void InstantLand()
	{
		if (this._controller._flock.gameObject.activeInHierarchy && this.landingChild == null)
		{
			FlockChild flockChild = null;
			for (int i = 0; i < this._controller._flock._roamers.Count; i++)
			{
				FlockChild flockChild2 = this._controller._flock._roamers[i];
				if (!flockChild2._landing && !flockChild2._dived)
				{
					flockChild = flockChild2;
				}
			}
			if (flockChild != null)
			{
				this.landingChild = flockChild;
				this.landing = true;
				this._controller._activeLandingSpots++;
				this.landingChild._landing = true;
				this.landingChild._thisT.position = this._thisT.position + this.landingChild._landingPosOffset;
				this.landingChild._model.GetComponent<Animation>().Play(this.landingChild._spawner._idleAnimation);
				this.landingChild._thisT.Rotate(Vector3.up, Random.Range(0f, 360f));
				if (this._controller._autoDismountDelay.x > 0f)
				{
					base.Invoke("ReleaseFlockChild", Random.Range(this._controller._autoDismountDelay.x, this._controller._autoDismountDelay.y));
					return;
				}
			}
			else if (this._controller._autoCatchDelay.x > 0f)
			{
				base.StartCoroutine(this.GetFlockChild(this._controller._autoCatchDelay.x, this._controller._autoCatchDelay.y));
			}
		}
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00007FD0 File Offset: 0x000061D0
	public void ReleaseFlockChild()
	{
		if (this._controller._flock.gameObject.activeInHierarchy && this.landingChild != null)
		{
			this._gotcha = false;
			this.lerpCounter = 0;
			if (this._controller._featherPS != null)
			{
				this._controller._featherPS.position = this.landingChild._thisT.position;
				this._controller._featherPS.GetComponent<ParticleSystem>().Emit(Random.Range(0, 3));
			}
			this.landing = false;
			this._idle = false;
			this.landingChild._avoid = true;
			this.landingChild._damping = this.landingChild._spawner._maxDamping;
			this.landingChild._model.GetComponent<Animation>().CrossFade(this.landingChild._spawner._flapAnimation, 0.2f);
			this.landingChild._dived = true;
			this.landingChild._speed = 0f;
			this.landingChild._move = true;
			this.landingChild._landing = false;
			this.landingChild.Flap();
			this.landingChild._wayPoint = new Vector3(this.landingChild._wayPoint.x, this._thisT.position.y + 10f, this.landingChild._wayPoint.z);
			if (this._controller._autoCatchDelay.x > 0f)
			{
				base.StartCoroutine(this.GetFlockChild(this._controller._autoCatchDelay.x + 0.1f, this._controller._autoCatchDelay.y + 0.1f));
			}
			this.landingChild = null;
			this._controller._activeLandingSpots--;
		}
	}

	// Token: 0x04000122 RID: 290
	[HideInInspector]
	public FlockChild landingChild;

	// Token: 0x04000123 RID: 291
	[HideInInspector]
	public bool landing;

	// Token: 0x04000124 RID: 292
	private int lerpCounter;

	// Token: 0x04000125 RID: 293
	[HideInInspector]
	public LandingSpotController _controller;

	// Token: 0x04000126 RID: 294
	private bool _idle;

	// Token: 0x04000127 RID: 295
	public Transform _thisT;

	// Token: 0x04000128 RID: 296
	public bool _gotcha;
}
