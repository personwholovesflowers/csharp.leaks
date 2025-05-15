using System;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class DroneFlesh : MonoBehaviour
{
	// Token: 0x06000590 RID: 1424 RVA: 0x00026D3A File Offset: 0x00024F3A
	private void Awake()
	{
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
		this.drn = base.GetComponent<Drone>();
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00026D54 File Offset: 0x00024F54
	private void Start()
	{
		this.cooldown = Random.Range(2f, 3f);
		if (this.drn)
		{
			this.drn.fleshDrone = true;
		}
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (this.difficulty == 1)
		{
			this.difficultySpeedModifier = 0.8f;
			return;
		}
		if (this.difficulty == 0)
		{
			this.difficultySpeedModifier = 0.6f;
		}
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x00026DF0 File Offset: 0x00024FF0
	private void Update()
	{
		if (this.eid && this.eid.enemyType == EnemyType.Virtue)
		{
			return;
		}
		if (this.drn && this.drn.crashing)
		{
			this.drn.Explode();
			return;
		}
		if (this.eid.target == null)
		{
			return;
		}
		if (this.tracking)
		{
			base.transform.LookAt(this.eid.target.position);
			if (this.rotationOffset != Vector3.zero)
			{
				base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles + this.rotationOffset);
			}
		}
		if ((this.drn && !this.drn.targetSpotted) || this.inAction)
		{
			return;
		}
		if (this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.cooldown <= 1f && this.chargeEffect)
			{
				if (!this.currentChargeEffect)
				{
					this.currentChargeEffect = Object.Instantiate<GameObject>(this.chargeEffect, this.shootPoint ? this.shootPoint.position : (base.transform.position + base.transform.forward * 1.5f), this.shootPoint ? this.shootPoint.rotation : base.transform.rotation);
					this.currentChargeEffect.transform.SetParent(base.transform);
					this.currentChargeEffect.transform.localScale = Vector3.zero;
					this.ceAud = this.currentChargeEffect.GetComponent<AudioSource>();
					this.ceLight = this.currentChargeEffect.GetComponent<Light>();
				}
				this.currentChargeEffect.transform.localScale = Vector3.one * (1f - this.cooldown) * 2.5f;
				if (this.ceAud)
				{
					this.ceAud.pitch = (1f - this.cooldown) * 2f;
				}
				if (this.ceLight)
				{
					this.ceLight.intensity = (1f - this.cooldown) * 30f;
					return;
				}
			}
		}
		else
		{
			this.inAction = true;
			this.cooldown = Random.Range(1f, 3f);
			if (this.difficulty > 2)
			{
				this.cooldown *= 0.75f;
			}
			if (this.difficulty == 1)
			{
				this.cooldown *= 1.5f;
			}
			else if (this.difficulty == 0)
			{
				this.cooldown *= 2f;
			}
			this.PrepareBeam();
		}
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x000270FC File Offset: 0x000252FC
	private void PrepareBeam()
	{
		if (this.drn)
		{
			this.drn.lockPosition = true;
			this.drn.lockRotation = true;
		}
		base.transform.LookAt(this.eid.target.PredictTargetPosition(0.5f / this.eid.totalSpeedModifier * this.predictionAmount, false));
		if (this.rotationOffset != Vector3.zero)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles + this.rotationOffset);
		}
		this.currentWarningBeam = Object.Instantiate<GameObject>(this.warningBeam, this.shootPoint ? this.shootPoint : base.transform);
		if (!this.shootPoint)
		{
			this.currentWarningBeam.transform.position += base.transform.forward * 1.5f;
		}
		float num = 0.5f;
		if (this.difficulty == 1)
		{
			num = 1f;
		}
		if (this.difficulty == 0)
		{
			num = 1.5f;
		}
		base.Invoke("ShootBeam", num / this.eid.totalSpeedModifier);
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00027246 File Offset: 0x00025446
	private void StopTracking()
	{
		this.tracking = false;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00027250 File Offset: 0x00025450
	private void ShootBeam()
	{
		if (this.currentWarningBeam)
		{
			Object.Destroy(this.currentWarningBeam);
		}
		if (this.currentChargeEffect)
		{
			Object.Destroy(this.currentChargeEffect);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.beam, this.shootPoint ? this.shootPoint.position : base.transform.position, this.shootPoint ? this.shootPoint.rotation : base.transform.rotation);
		RevolverBeam revolverBeam;
		Grenade grenade;
		if (this.eid.totalDamageModifier != 1f && gameObject.TryGetComponent<RevolverBeam>(out revolverBeam))
		{
			revolverBeam.damage *= this.eid.totalDamageModifier;
		}
		else if (gameObject.TryGetComponent<Grenade>(out grenade))
		{
			if (this.eid.totalDamageModifier != 1f)
			{
				grenade.totalDamageMultiplier = this.eid.totalDamageModifier;
			}
			grenade.originEnemy = this.eid;
			grenade.rocketSpeed *= this.difficultySpeedModifier;
		}
		if (this.drn)
		{
			this.drn.lockPosition = false;
			this.drn.lockRotation = false;
		}
		this.inAction = false;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x00027391 File Offset: 0x00025591
	public void Explode()
	{
		Drone drone = this.drn;
		if (drone == null)
		{
			return;
		}
		drone.Explode();
	}

	// Token: 0x040007B9 RID: 1977
	public GameObject beam;

	// Token: 0x040007BA RID: 1978
	public GameObject warningBeam;

	// Token: 0x040007BB RID: 1979
	public GameObject chargeEffect;

	// Token: 0x040007BC RID: 1980
	private GameObject currentWarningBeam;

	// Token: 0x040007BD RID: 1981
	private GameObject currentChargeEffect;

	// Token: 0x040007BE RID: 1982
	private AudioSource ceAud;

	// Token: 0x040007BF RID: 1983
	private Light ceLight;

	// Token: 0x040007C0 RID: 1984
	private float cooldown = 3f;

	// Token: 0x040007C1 RID: 1985
	private bool inAction;

	// Token: 0x040007C2 RID: 1986
	private Drone drn;

	// Token: 0x040007C3 RID: 1987
	private EnemyIdentifier eid;

	// Token: 0x040007C4 RID: 1988
	private bool tracking;

	// Token: 0x040007C5 RID: 1989
	public Transform shootPoint;

	// Token: 0x040007C6 RID: 1990
	public float predictionAmount;

	// Token: 0x040007C7 RID: 1991
	public Vector3 rotationOffset;

	// Token: 0x040007C8 RID: 1992
	private int difficulty;

	// Token: 0x040007C9 RID: 1993
	private float difficultySpeedModifier = 1f;
}
