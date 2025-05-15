using System;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class Parasite : MonoBehaviour
{
	// Token: 0x1700016F RID: 367
	// (get) Token: 0x060012D1 RID: 4817 RVA: 0x00096041 File Offset: 0x00094241
	private EnemyTarget target
	{
		get
		{
			EnemyTarget enemyTarget;
			if ((enemyTarget = this.localTarget) == null)
			{
				if (!(this.parent == null))
				{
					return this.parent.target;
				}
				enemyTarget = null;
			}
			return enemyTarget;
		}
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x00096068 File Offset: 0x00094268
	private void Start()
	{
		this.cooldown = (float)Random.Range(0, 3);
		this.anim = base.GetComponent<Animator>();
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		if (this.parent == null)
		{
			this.localTarget = new EnemyTarget(MonoSingleton<CameraController>.Instance.transform);
		}
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x000960DC File Offset: 0x000942DC
	private void Update()
	{
		if (this.target == null)
		{
			return;
		}
		Quaternion quaternion = Quaternion.LookRotation(this.target.position - base.transform.position, Vector3.up);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * (Quaternion.Angle(base.transform.rotation, quaternion) + 1f) * this.speedMultiplier);
		if (!this.inAction)
		{
			float num = 1f;
			if (this.difficulty == 1)
			{
				num = 0.75f;
			}
			else if (this.difficulty == 0)
			{
				num = 0.5f;
			}
			if (this.cooldown > 0f)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * num * this.speedMultiplier);
				return;
			}
			if (!this.anim.IsInTransition(0))
			{
				if (Random.Range(0f, 1f) > 0.5f || this.difficulty < 2)
				{
					this.Shoot1();
					return;
				}
				this.Shoot2();
			}
		}
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x000961F4 File Offset: 0x000943F4
	private void Shoot1()
	{
		Object.Instantiate<GameObject>(this.windUpSound, this.projectileSpawnPos);
		this.inAction = true;
		this.cooldown = (float)Random.Range(2, 4);
		this.shootType = 0;
		this.anim.SetTrigger("Shoot1");
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x00096234 File Offset: 0x00094434
	private void Shoot2()
	{
		Object.Instantiate<GameObject>(this.windUpSound, this.projectileSpawnPos);
		this.inAction = true;
		this.cooldown = (float)Random.Range(2, 4);
		this.shootType = 1;
		this.anim.SetTrigger("Shoot2");
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x00096274 File Offset: 0x00094474
	public void SpawnProjectile()
	{
		if (this.currentDecProjectile)
		{
			Object.Destroy(this.currentDecProjectile);
		}
		this.currentDecProjectile = Object.Instantiate<GameObject>(this.decProjectiles[this.shootType], this.projectileSpawnPos.position, this.projectileSpawnPos.rotation);
		this.currentDecProjectile.transform.localScale *= 5f;
		this.currentDecProjectile.transform.SetParent(this.projectileSpawnPos, true);
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x00096300 File Offset: 0x00094500
	public void ShootProjectile()
	{
		if (this.currentDecProjectile)
		{
			Object.Destroy(this.currentDecProjectile);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.projectiles[this.shootType], this.projectileSpawnPos.position, Quaternion.LookRotation(this.target.position - this.projectileSpawnPos.position));
		gameObject.transform.localScale *= 5f;
		gameObject.transform.SetParent(this.gz.transform, true);
		Projectile componentInChildren = gameObject.GetComponentInChildren<Projectile>();
		componentInChildren.target = this.target;
		componentInChildren.safeEnemyType = EnemyType.Minos;
		float num = Vector3.Distance(this.projectileSpawnPos.position, this.target.position);
		if (num > 65f)
		{
			componentInChildren.speed = num;
		}
		if (this.difficulty == 1)
		{
			componentInChildren.speed *= 0.75f;
		}
		else if (this.difficulty == 0)
		{
			componentInChildren.speed *= 0.5f;
		}
		componentInChildren.damage *= this.damageMultiplier;
		ProjectileSpread componentInChildren2 = gameObject.GetComponentInChildren<ProjectileSpread>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.spreadAmount = 3f;
			componentInChildren2.projectileAmount = 8;
		}
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x00096445 File Offset: 0x00094645
	public void StopAction()
	{
		this.inAction = false;
	}

	// Token: 0x040019C4 RID: 6596
	public EnemyIdentifier parent;

	// Token: 0x040019C5 RID: 6597
	private EnemyTarget localTarget;

	// Token: 0x040019C6 RID: 6598
	public Transform projectileSpawnPos;

	// Token: 0x040019C7 RID: 6599
	private Animator anim;

	// Token: 0x040019C8 RID: 6600
	public GameObject[] decProjectiles;

	// Token: 0x040019C9 RID: 6601
	public GameObject[] projectiles;

	// Token: 0x040019CA RID: 6602
	private GameObject currentDecProjectile;

	// Token: 0x040019CB RID: 6603
	private bool inAction = true;

	// Token: 0x040019CC RID: 6604
	private float cooldown;

	// Token: 0x040019CD RID: 6605
	public GameObject windUpSound;

	// Token: 0x040019CE RID: 6606
	private int shootType;

	// Token: 0x040019CF RID: 6607
	private GoreZone gz;

	// Token: 0x040019D0 RID: 6608
	private int difficulty;

	// Token: 0x040019D1 RID: 6609
	public float speedMultiplier = 1f;

	// Token: 0x040019D2 RID: 6610
	public float damageMultiplier = 1f;
}
