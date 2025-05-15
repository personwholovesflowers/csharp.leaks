using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class EnemyNailgun : MonoBehaviour, IEnemyWeapon
{
	// Token: 0x06000809 RID: 2057 RVA: 0x00037876 File Offset: 0x00035A76
	private void Awake()
	{
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00037884 File Offset: 0x00035A84
	private void Start()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x0003789C File Offset: 0x00035A9C
	private void FixedUpdate()
	{
		if (this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.fixedDeltaTime * this.speedMultiplier);
		}
		if (this.burstAmount > 0 && this.cooldown <= 0f)
		{
			Vector3 position = this.shootPoint.position;
			if (Vector3.Distance(base.transform.position, this.eid.transform.position) > Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.eid.transform.position))
			{
				Vector3 vector = this.eid.transform.position + base.transform.forward * Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, this.eid.transform.position);
				position = new Vector3(vector.x, base.transform.position.y, vector.z);
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.currentNail, position, this.shootPoint.rotation);
			gameObject.transform.Rotate(Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f), Random.Range(-this.currentSpread / 3f, this.currentSpread / 3f));
			gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 200f * this.speedMultiplier, ForceMode.VelocityChange);
			Nail nail;
			if (this.damageMultiplier != 1f && gameObject.TryGetComponent<Nail>(out nail))
			{
				nail.damage *= this.damageMultiplier;
			}
			Collider component = gameObject.GetComponent<CapsuleCollider>();
			foreach (Collider collider in this.toIgnore)
			{
				Physics.IgnoreCollision(component, collider, true);
			}
			Object.Instantiate<GameObject>(this.muzzleFlash, this.shootPoint);
			this.cooldown = this.fireRate;
			this.burstAmount--;
		}
		if (this.charging)
		{
			this.chargeAmount = Mathf.MoveTowards(this.chargeAmount, 1f, Time.deltaTime);
			this.chargeSound.pitch = this.chargeAmount * 2f;
		}
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x00037B21 File Offset: 0x00035D21
	public void UpdateTarget(EnemyTarget target)
	{
		this.target = target;
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00037B2C File Offset: 0x00035D2C
	public void Fire()
	{
		this.burstAmount = 30;
		if (this.difficulty > 2)
		{
			this.currentSpread = 5f;
		}
		else if (this.difficulty == 2)
		{
			this.currentSpread = 3f;
		}
		else
		{
			this.currentSpread = 1.5f;
		}
		this.fireRate = 0.033f;
		this.currentNail = this.nail;
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x00037B90 File Offset: 0x00035D90
	public void AltFire()
	{
		this.burstAmount = 100;
		if (this.difficulty > 2)
		{
			this.currentSpread = 25f;
		}
		else if (this.difficulty == 2)
		{
			this.currentSpread = 15f;
		}
		else
		{
			this.currentSpread = 7.5f;
		}
		this.fireRate = 0.01f;
		this.currentNail = this.altNail;
		this.chargeSound.Stop();
		this.charging = false;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x00037C08 File Offset: 0x00035E08
	public void PrepareFire()
	{
		this.burstAmount = 0;
		Object.Instantiate<GameObject>(this.flash, this.shootPoint.position, this.shootPoint.rotation).transform.localScale *= 4f;
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00037C57 File Offset: 0x00035E57
	public void PrepareAltFire()
	{
		this.burstAmount = 0;
		this.charging = true;
		this.chargeAmount = 0f;
		this.chargeSound.pitch = 0f;
		this.chargeSound.Play();
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00037C8D File Offset: 0x00035E8D
	public void CancelAltCharge()
	{
		this.charging = false;
		this.chargeAmount = 0f;
		this.chargeSound.pitch = 0f;
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00037CB1 File Offset: 0x00035EB1
	private void UpdateBuffs(EnemyIdentifier eid)
	{
		this.speedMultiplier = eid.totalSpeedModifier;
		this.damageMultiplier = eid.totalDamageModifier;
	}

	// Token: 0x04000AA9 RID: 2729
	private EnemyTarget target;

	// Token: 0x04000AAA RID: 2730
	public GameObject nail;

	// Token: 0x04000AAB RID: 2731
	public GameObject altNail;

	// Token: 0x04000AAC RID: 2732
	public Transform shootPoint;

	// Token: 0x04000AAD RID: 2733
	public GameObject flash;

	// Token: 0x04000AAE RID: 2734
	public GameObject muzzleFlash;

	// Token: 0x04000AAF RID: 2735
	[SerializeField]
	private AudioSource chargeSound;

	// Token: 0x04000AB0 RID: 2736
	private bool charging;

	// Token: 0x04000AB1 RID: 2737
	private float chargeAmount;

	// Token: 0x04000AB2 RID: 2738
	private int burstAmount;

	// Token: 0x04000AB3 RID: 2739
	private float cooldown;

	// Token: 0x04000AB4 RID: 2740
	private GameObject currentNail;

	// Token: 0x04000AB5 RID: 2741
	private float currentSpread = 5f;

	// Token: 0x04000AB6 RID: 2742
	private float fireRate = 0.033f;

	// Token: 0x04000AB7 RID: 2743
	public Collider[] toIgnore;

	// Token: 0x04000AB8 RID: 2744
	private int difficulty;

	// Token: 0x04000AB9 RID: 2745
	private EnemyIdentifier eid;

	// Token: 0x04000ABA RID: 2746
	private float speedMultiplier;

	// Token: 0x04000ABB RID: 2747
	private float damageMultiplier;
}
