using System;
using UnityEngine;

// Token: 0x02000197 RID: 407
public class EnemyShotgun : MonoBehaviour, IEnemyWeapon
{
	// Token: 0x06000827 RID: 2087 RVA: 0x00038490 File Offset: 0x00036690
	private void Start()
	{
		this.gunAud = base.GetComponent<AudioSource>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.parts = base.GetComponentsInChildren<ParticleSystem>();
		this.heatSinkAud = this.shootPoint.GetComponent<AudioSource>();
		this.chargeSound = base.transform.GetChild(0).GetComponent<AudioSource>();
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
		if (this.difficulty == 1)
		{
			this.spread *= 0.75f;
			return;
		}
		if (this.difficulty == 0)
		{
			this.spread *= 0.5f;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x00038544 File Offset: 0x00036744
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
			this.chargeSound.pitch = this.chargeAmount * 1.25f;
		}
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000385B7 File Offset: 0x000367B7
	public void UpdateTarget(EnemyTarget target)
	{
		this.target = target;
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x000385C0 File Offset: 0x000367C0
	public void Fire()
	{
		if (this.target == null)
		{
			return;
		}
		this.gunReady = false;
		int num = 12;
		this.anim.SetTrigger("Shoot");
		Vector3 position = this.shootPoint.position;
		if (Vector3.Distance(base.transform.position, this.eid.transform.position) > Vector3.Distance(this.target.position, this.eid.transform.position))
		{
			position = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
		}
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<ProjectileSpread>();
		gameObject.transform.position = base.transform.position;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject2;
			if (i == 0)
			{
				gameObject2 = Object.Instantiate<GameObject>(this.bullet, position, this.shootPoint.rotation, gameObject.transform);
			}
			else
			{
				Quaternion quaternion = this.shootPoint.rotation * Quaternion.Euler(Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread), Random.Range(-this.spread, this.spread));
				gameObject2 = Object.Instantiate<GameObject>(this.bullet, position, quaternion, gameObject.transform);
			}
			Projectile projectile;
			if (gameObject2.TryGetComponent<Projectile>(out projectile))
			{
				projectile.target = this.target;
				projectile.safeEnemyType = this.safeEnemyType;
				if (this.difficulty == 1)
				{
					projectile.speed *= 0.75f;
				}
				else if (this.difficulty == 0)
				{
					projectile.speed *= 0.5f;
				}
				projectile.damage *= this.damageMultiplier;
				projectile.spreaded = true;
			}
		}
		this.gunAud.clip = this.shootSound;
		this.gunAud.volume = 0.35f;
		this.gunAud.panStereo = 0f;
		this.gunAud.pitch = Random.Range(0.95f, 1.05f);
		this.gunAud.Play();
		Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint.position, this.shootPoint.rotation);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x00038830 File Offset: 0x00036A30
	public void AltFire()
	{
		if (this.target == null)
		{
			this.CancelAltCharge();
			return;
		}
		this.gunReady = false;
		float num = 70f;
		if (this.difficulty == 1)
		{
			num = 50f;
		}
		else if (this.difficulty == 0)
		{
			num = 30f;
		}
		if (this.shootPoint == null)
		{
			return;
		}
		Vector3 position = this.shootPoint.position;
		if (Vector3.Distance(base.transform.position, this.eid.transform.position) > Vector3.Distance(this.target.position, this.eid.transform.position))
		{
			position = new Vector3(this.eid.transform.position.x, base.transform.position.y, this.eid.transform.position.z);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.grenade, position, Random.rotation);
		gameObject.GetComponent<Rigidbody>().AddForce(this.shootPoint.forward * num, ForceMode.VelocityChange);
		Grenade componentInChildren = gameObject.GetComponentInChildren<Grenade>();
		if (componentInChildren != null)
		{
			componentInChildren.enemy = true;
		}
		this.anim.SetTrigger("Secondary Fire");
		this.gunAud.clip = this.shootSound;
		this.gunAud.volume = 0.35f;
		this.gunAud.panStereo = 0f;
		this.gunAud.pitch = Random.Range(0.75f, 0.85f);
		this.gunAud.Play();
		Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint.position, this.shootPoint.rotation);
		this.CancelAltCharge();
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000389E8 File Offset: 0x00036BE8
	public void PrepareFire()
	{
		if (this.heatSinkAud == null)
		{
			this.heatSinkAud = this.shootPoint.GetComponent<AudioSource>();
		}
		this.heatSinkAud.Play();
		Object.Instantiate<GameObject>(this.warningFlash, this.shootPoint.position, this.shootPoint.rotation).transform.localScale *= 2f;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00038A5C File Offset: 0x00036C5C
	public void PrepareAltFire()
	{
		if (this.chargeSound == null)
		{
			this.chargeSound = base.transform.GetChild(0).GetComponent<AudioSource>();
		}
		this.charging = true;
		this.chargeAmount = 0f;
		this.chargeSound.pitch = 0f;
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00038AB0 File Offset: 0x00036CB0
	public void CancelAltCharge()
	{
		if (this.chargeSound == null)
		{
			this.chargeSound = base.transform.GetChild(0).GetComponent<AudioSource>();
		}
		this.charging = false;
		this.chargeAmount = 0f;
		this.chargeSound.pitch = 0f;
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x00038B04 File Offset: 0x00036D04
	public void ReleaseHeat()
	{
		ParticleSystem[] array = this.parts;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Play();
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x00038B30 File Offset: 0x00036D30
	public void ClickSound()
	{
		this.gunAud.clip = this.clickSound;
		this.gunAud.volume = 0.5f;
		this.gunAud.pitch = Random.Range(0.95f, 1.05f);
		this.gunAud.Play();
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x00038B83 File Offset: 0x00036D83
	public void ReadyGun()
	{
		this.gunReady = true;
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x00038B8C File Offset: 0x00036D8C
	public void Smack()
	{
		this.gunAud.clip = this.smackSound;
		this.gunAud.volume = 0.75f;
		this.gunAud.pitch = Random.Range(2f, 2.2f);
		this.gunAud.Play();
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00038BDF File Offset: 0x00036DDF
	public void UpdateBuffs(EnemyIdentifier eid)
	{
		this.speedMultiplier = eid.totalSpeedModifier;
		this.damageMultiplier = eid.totalDamageModifier;
	}

	// Token: 0x04000AD5 RID: 2773
	private EnemyTarget target;

	// Token: 0x04000AD6 RID: 2774
	public EnemyType safeEnemyType;

	// Token: 0x04000AD7 RID: 2775
	private AudioSource gunAud;

	// Token: 0x04000AD8 RID: 2776
	public AudioClip shootSound;

	// Token: 0x04000AD9 RID: 2777
	public AudioClip clickSound;

	// Token: 0x04000ADA RID: 2778
	public AudioClip smackSound;

	// Token: 0x04000ADB RID: 2779
	private AudioSource heatSinkAud;

	// Token: 0x04000ADC RID: 2780
	public int variation;

	// Token: 0x04000ADD RID: 2781
	public GameObject bullet;

	// Token: 0x04000ADE RID: 2782
	public GameObject grenade;

	// Token: 0x04000ADF RID: 2783
	public float spread;

	// Token: 0x04000AE0 RID: 2784
	private Animator anim;

	// Token: 0x04000AE1 RID: 2785
	public bool gunReady = true;

	// Token: 0x04000AE2 RID: 2786
	public Transform shootPoint;

	// Token: 0x04000AE3 RID: 2787
	public GameObject muzzleFlash;

	// Token: 0x04000AE4 RID: 2788
	private ParticleSystem[] parts;

	// Token: 0x04000AE5 RID: 2789
	private bool charging;

	// Token: 0x04000AE6 RID: 2790
	private AudioSource chargeSound;

	// Token: 0x04000AE7 RID: 2791
	private float chargeAmount;

	// Token: 0x04000AE8 RID: 2792
	public GameObject warningFlash;

	// Token: 0x04000AE9 RID: 2793
	private int difficulty;

	// Token: 0x04000AEA RID: 2794
	private EnemyIdentifier eid;

	// Token: 0x04000AEB RID: 2795
	private float speedMultiplier = 1f;

	// Token: 0x04000AEC RID: 2796
	private float damageMultiplier = 1f;
}
