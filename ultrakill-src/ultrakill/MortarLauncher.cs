using System;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class MortarLauncher : MonoBehaviour
{
	// Token: 0x0600118B RID: 4491 RVA: 0x000887A4 File Offset: 0x000869A4
	private void Start()
	{
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
		this.anim = base.GetComponent<Animator>();
		this.cooldown = this.firstFireDelay;
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

	// Token: 0x0600118C RID: 4492 RVA: 0x00088834 File Offset: 0x00086A34
	private void Update()
	{
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier * this.difficultySpeedModifier);
		if (this.cooldown == 0f && this.eid.target != null)
		{
			this.cooldown = this.firingDelay;
			this.ShootHoming();
			UltrakillEvent ultrakillEvent = this.onFire;
			if (ultrakillEvent == null)
			{
				return;
			}
			ultrakillEvent.Invoke("");
		}
	}

	// Token: 0x0600118D RID: 4493 RVA: 0x000888B0 File Offset: 0x00086AB0
	public void ShootHoming()
	{
		if (this.eid.target == null)
		{
			return;
		}
		Projectile projectile = Object.Instantiate<Projectile>(this.mortar, this.shootPoint.position, this.shootPoint.rotation);
		projectile.target = this.eid.target;
		projectile.GetComponent<Rigidbody>().velocity = this.shootPoint.forward * this.projectileForce;
		projectile.damage *= this.eid.totalDamageModifier;
		projectile.safeEnemyType = this.eid.enemyType;
		projectile.turningSpeedMultiplier *= this.difficultySpeedModifier;
		projectile.gameObject.SetActive(true);
		if (this.anim)
		{
			this.anim.Play("Shoot", 0, 0f);
		}
	}

	// Token: 0x0600118E RID: 4494 RVA: 0x00088988 File Offset: 0x00086B88
	public void ChangeFiringDelay(float target)
	{
		this.firingDelay = target;
		if (this.cooldown > this.firingDelay)
		{
			this.cooldown = this.firingDelay;
		}
	}

	// Token: 0x040017D2 RID: 6098
	private EnemyIdentifier eid;

	// Token: 0x040017D3 RID: 6099
	public Transform shootPoint;

	// Token: 0x040017D4 RID: 6100
	public Projectile mortar;

	// Token: 0x040017D5 RID: 6101
	private float cooldown = 1f;

	// Token: 0x040017D6 RID: 6102
	public float firingDelay;

	// Token: 0x040017D7 RID: 6103
	public float firstFireDelay = 1f;

	// Token: 0x040017D8 RID: 6104
	public float projectileForce;

	// Token: 0x040017D9 RID: 6105
	public UltrakillEvent onFire;

	// Token: 0x040017DA RID: 6106
	private Animator anim;

	// Token: 0x040017DB RID: 6107
	private int difficulty;

	// Token: 0x040017DC RID: 6108
	private float difficultySpeedModifier = 1f;
}
