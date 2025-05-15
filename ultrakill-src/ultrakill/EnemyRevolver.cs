using System;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class EnemyRevolver : MonoBehaviour, IEnemyWeapon
{
	// Token: 0x06000814 RID: 2068 RVA: 0x00037CEC File Offset: 0x00035EEC
	private void Start()
	{
		this.altCharge = this.shootPoint.GetChild(0).gameObject;
		this.altChargeAud = this.altCharge.GetComponent<AudioSource>();
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00037D44 File Offset: 0x00035F44
	private void Update()
	{
		if (this.charging)
		{
			float num = 2f;
			if (this.difficulty == 1)
			{
				num = 1.5f;
			}
			if (this.difficulty == 0)
			{
				num = 1f;
			}
			this.chargeAmount = Mathf.MoveTowards(this.chargeAmount, 1f, Time.deltaTime * num * this.speedMultiplier);
			this.altChargeAud.pitch = this.chargeAmount / 1.75f;
			this.altCharge.transform.localScale = Vector3.one * this.chargeAmount * 10f;
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00037DE4 File Offset: 0x00035FE4
	public void UpdateTarget(EnemyTarget target)
	{
		this.target = target;
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00037DF0 File Offset: 0x00035FF0
	public void Fire()
	{
		if (this.currentpp != null)
		{
			Object.Destroy(this.currentpp);
		}
		Vector3 position = this.shootPoint.position;
		if (Vector3.Distance(base.transform.position, this.eid.transform.position) > Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.eid.transform.position))
		{
			position = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.bullet, position, this.shootPoint.rotation);
		Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint.position, this.shootPoint.rotation);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (component)
		{
			component.safeEnemyType = this.safeEnemyType;
			component.target = this.target;
			if (this.difficulty == 1)
			{
				component.speed *= 0.75f;
			}
			if (this.difficulty == 0)
			{
				component.speed *= 0.5f;
			}
			component.damage *= this.damageMultiplier;
		}
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00037F50 File Offset: 0x00036150
	public void AltFire()
	{
		this.CancelAltCharge();
		Vector3 position = this.shootPoint.position;
		if (Vector3.Distance(base.transform.position, this.eid.transform.position) > Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.eid.transform.position))
		{
			position = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.altBullet, position, this.shootPoint.rotation);
		Object.Instantiate<GameObject>(this.muzzleFlashAlt, this.shootPoint.position, this.shootPoint.rotation);
		Projectile component = gameObject.GetComponent<Projectile>();
		if (component)
		{
			component.target = this.target;
			component.safeEnemyType = this.safeEnemyType;
			if (this.difficulty == 1)
			{
				component.speed *= 0.75f;
			}
			if (this.difficulty == 0)
			{
				component.speed *= 0.5f;
			}
			component.damage *= this.damageMultiplier;
		}
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000380A0 File Offset: 0x000362A0
	public void PrepareFire()
	{
		if (this.currentpp != null)
		{
			Object.Destroy(this.currentpp);
		}
		this.currentpp = Object.Instantiate<GameObject>(this.primaryPrepare, this.shootPoint);
		this.currentpp.transform.Rotate(Vector3.up * 90f);
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x000380FC File Offset: 0x000362FC
	public void PrepareAltFire()
	{
		if (this.altCharge)
		{
			this.charging = true;
			this.altCharge.SetActive(true);
		}
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0003811E File Offset: 0x0003631E
	public void CancelAltCharge()
	{
		if (this.altChargeAud)
		{
			this.charging = false;
			this.chargeAmount = 0f;
			this.altChargeAud.pitch = 0f;
			this.altCharge.SetActive(false);
		}
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x0003815B File Offset: 0x0003635B
	private void OnDisable()
	{
		if (this.currentpp != null)
		{
			Object.Destroy(this.currentpp);
		}
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00038176 File Offset: 0x00036376
	private void UpdateBuffs(EnemyIdentifier eid)
	{
		this.speedMultiplier = eid.totalSpeedModifier;
		this.damageMultiplier = eid.totalDamageModifier;
	}

	// Token: 0x04000ABC RID: 2748
	private EnemyTarget target;

	// Token: 0x04000ABD RID: 2749
	public EnemyType safeEnemyType;

	// Token: 0x04000ABE RID: 2750
	public int variation;

	// Token: 0x04000ABF RID: 2751
	public GameObject bullet;

	// Token: 0x04000AC0 RID: 2752
	public GameObject altBullet;

	// Token: 0x04000AC1 RID: 2753
	public GameObject primaryPrepare;

	// Token: 0x04000AC2 RID: 2754
	private GameObject currentpp;

	// Token: 0x04000AC3 RID: 2755
	private GameObject altCharge;

	// Token: 0x04000AC4 RID: 2756
	private AudioSource altChargeAud;

	// Token: 0x04000AC5 RID: 2757
	private float chargeAmount;

	// Token: 0x04000AC6 RID: 2758
	private bool charging;

	// Token: 0x04000AC7 RID: 2759
	public Transform shootPoint;

	// Token: 0x04000AC8 RID: 2760
	public GameObject muzzleFlash;

	// Token: 0x04000AC9 RID: 2761
	public GameObject muzzleFlashAlt;

	// Token: 0x04000ACA RID: 2762
	private int difficulty;

	// Token: 0x04000ACB RID: 2763
	private EnemyIdentifier eid;

	// Token: 0x04000ACC RID: 2764
	private float speedMultiplier = 1f;

	// Token: 0x04000ACD RID: 2765
	private float damageMultiplier = 1f;
}
