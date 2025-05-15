using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002EA RID: 746
public class Mass : MonoBehaviour
{
	// Token: 0x06001048 RID: 4168 RVA: 0x0007CB6D File Offset: 0x0007AD6D
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.swingChecks = base.GetComponentsInChildren<SwingCheck2>();
		this.stat = base.GetComponent<Statue>();
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x0007CB93 File Offset: 0x0007AD93
	private void Start()
	{
		this.transformCooldown = 10f;
		this.SetSpeed();
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0007CBA6 File Offset: 0x0007ADA6
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0007CBB0 File Offset: 0x0007ADB0
	private void SetSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponentInChildren<Animator>();
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (this.difficulty < 0)
		{
			if (this.eid.difficultyOverride >= 0)
			{
				this.difficulty = this.eid.difficultyOverride;
			}
			else
			{
				this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
			}
		}
		if (this.difficulty == 1)
		{
			this.anim.speed = 0.85f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.65f;
		}
		else if (this.difficulty >= 4)
		{
			this.anim.speed = 1.25f;
		}
		else
		{
			this.anim.speed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0007CCAC File Offset: 0x0007AEAC
	private void OnDisable()
	{
		this.StopAction();
		this.inSemiAction = false;
		if (this.spearShot)
		{
			this.SpearReturned();
		}
		if (this.swingChecks != null)
		{
			SwingCheck2[] array = this.swingChecks;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
		}
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0007CCF9 File Offset: 0x0007AEF9
	private void OnEnable()
	{
		if (this.battleMode)
		{
			this.anim.Play("BattlePose");
		}
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0007CD14 File Offset: 0x0007AF14
	private void Update()
	{
		if (!this.dead)
		{
			if (this.eid.target == null)
			{
				return;
			}
			this.targetPos = new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z);
			this.targetRot = Quaternion.LookRotation(this.targetPos - base.transform.position, Vector3.up);
			if (!this.inAction && base.transform.rotation != this.targetRot)
			{
				if (this.battleMode || this.crazyMode)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRot, Time.deltaTime * Quaternion.Angle(base.transform.rotation, this.targetRot) + Time.deltaTime * 50f * this.eid.totalSpeedModifier);
				}
				else
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRot, Time.deltaTime * Quaternion.Angle(base.transform.rotation, this.targetRot) + Time.deltaTime * 120f * this.eid.totalSpeedModifier);
				}
				if (this.stat.health >= 35f)
				{
					this.walking = true;
				}
				else
				{
					this.walking = false;
				}
			}
			else
			{
				this.walking = false;
			}
			if (this.walking && this.walkWeight != 1f)
			{
				this.walkWeight = Mathf.MoveTowards(this.walkWeight, 1f, Time.deltaTime * 4f);
				this.anim.SetLayerWeight(1, this.walkWeight);
			}
			else if (!this.walking && this.walkWeight != 0f)
			{
				this.walkWeight = Mathf.MoveTowards(this.walkWeight, 0f, Time.deltaTime * 2f);
				this.anim.SetLayerWeight(1, this.walkWeight);
			}
			if (this.spearCooldown != 0f && !this.spearShot)
			{
				this.spearCooldown = Mathf.MoveTowards(this.spearCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (this.swingCooldown != 0f)
			{
				float num = 1f;
				if (this.difficulty >= 4)
				{
					num = 1.5f;
				}
				this.swingCooldown = Mathf.MoveTowards(this.swingCooldown, 0f, Time.deltaTime * num * this.eid.totalSpeedModifier);
			}
			else if (!this.inAction && !this.inSemiAction && this.battleMode && this.transformCooldown > 0f)
			{
				base.transform.LookAt(this.targetPos);
				if (this.eid.target.position.y - base.transform.position.y < 15f && this.eid.target.position.y - base.transform.position.y > -5f && this.attackedOnce && ((Vector3.Distance(this.targetPos, base.transform.position) > 7f && Random.Range(0f, 1f) < 0.5f) || Vector3.Distance(this.targetPos, base.transform.position) > 15f))
				{
					this.BattleSlam();
				}
				else
				{
					this.SwingAttack();
				}
			}
			if (Vector3.Distance(this.targetPos, base.transform.position) < 7f)
			{
				this.playerDistanceCooldown = Mathf.MoveTowards(this.playerDistanceCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			else
			{
				this.playerDistanceCooldown = Mathf.MoveTowards(this.playerDistanceCooldown, 3f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (!this.battleMode && !this.crazyMode && this.playerDistanceCooldown == 0f && !this.inAction && !this.inSemiAction && !this.spearShot)
			{
				base.transform.LookAt(this.targetPos);
				this.ToBattle();
			}
			if (this.stat.health < this.crazyModeHealth)
			{
				if (this.battleMode)
				{
					this.ToScout();
				}
				else
				{
					this.anim.SetBool("Crazy", true);
				}
			}
			else if (this.transformCooldown != 0f)
			{
				if (this.battleMode)
				{
					this.transformCooldown = Mathf.MoveTowards(this.transformCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
				}
				else
				{
					this.transformCooldown = Mathf.MoveTowards(this.transformCooldown, 0f, Time.deltaTime * 1.5f * this.eid.totalSpeedModifier);
				}
			}
			else if (!this.inAction && !this.inSemiAction && !this.spearShot)
			{
				if (this.battleMode)
				{
					this.ToScout();
				}
				else
				{
					base.transform.LookAt(this.targetPos);
					this.ToBattle();
				}
			}
			if (this.attackCooldown != 0f)
			{
				float num2 = 1f;
				if (this.difficulty >= 4)
				{
					num2 = 1.5f;
				}
				this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * num2 * this.eid.totalSpeedModifier);
				return;
			}
			if (!this.inAction && this.transformCooldown > 0f && this.stat.health >= this.crazyModeHealth && !this.battleMode)
			{
				this.ExplosiveAttack();
			}
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0007D2F8 File Offset: 0x0007B4F8
	private void LateUpdate()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if ((this.battleMode || (this.crazyMode && this.difficulty >= 4)) && !this.inAction && !this.inSemiAction && !this.dead)
		{
			this.tailEnd.LookAt(this.eid.target.position);
			if (this.spearCooldown == 0f)
			{
				if (!Physics.Raycast(this.tailEnd.position, this.eid.target.position - this.tailEnd.position, Vector3.Distance(this.eid.target.position, this.tailEnd.position), LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.spearCooldown = (float)Random.Range(2, 4);
					this.ReadySpear();
					return;
				}
				this.spearCooldown = 0.1f;
			}
		}
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0007D3F8 File Offset: 0x0007B5F8
	public void HomingAttack()
	{
		this.inAction = true;
		this.anim.SetTrigger("HomingAttack");
		this.attackCooldown = (float)Random.Range(3, 5);
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0007D41F File Offset: 0x0007B61F
	public void ExplosiveAttack()
	{
		this.inAction = true;
		this.anim.SetTrigger("ExplosiveAttack");
		this.attackCooldown = (float)Random.Range(3, 5);
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0007D448 File Offset: 0x0007B648
	public void SwingAttack()
	{
		this.inAction = true;
		this.anim.SetTrigger("Swing");
		this.swingCooldown = (float)Random.Range(3, 5);
		Object.Instantiate<GameObject>(this.windupSound, this.shootPoints[2].position, Quaternion.identity);
		this.attackedOnce = true;
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x0007D4A0 File Offset: 0x0007B6A0
	public void ToScout()
	{
		if (this.battleMode)
		{
			this.transformCooldown = (float)Random.Range(8, 12);
			this.inAction = true;
			this.anim.SetBool("Transform", true);
			this.battleMode = false;
			this.eid.weakPoint = this.stat.extraDamageZones[0];
		}
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0007D500 File Offset: 0x0007B700
	public void ToBattle()
	{
		if (!this.battleMode)
		{
			this.anim.SetBool("Transform", false);
			this.transformCooldown = (float)Random.Range(8, 12);
			this.inAction = true;
			this.anim.SetTrigger("Slam");
			this.battleMode = true;
			this.spearCooldown = 3f;
			AudioSource component = Object.Instantiate<GameObject>(this.windupSound, this.shootPoints[2].position, Quaternion.identity).GetComponent<AudioSource>();
			component.pitch = 1f;
			component.volume = 0.75f;
			this.eid.weakPoint = this.stat.extraDamageZones[1];
			this.attackedOnce = false;
		}
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0007D5BC File Offset: 0x0007B7BC
	public void SlamImpact()
	{
		if (!this.dead)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.slamExplosion, new Vector3(this.shootPoints[2].position.x, base.transform.position.y, this.shootPoints[2].position.z), Quaternion.identity);
			if (this.difficulty >= 2)
			{
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * 2.5f, gameObject.transform.localScale.z);
			}
			else if (this.difficulty == 1)
			{
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * 2f, gameObject.transform.localScale.z);
			}
			else if (this.difficulty == 0)
			{
				gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y * 1.5f, gameObject.transform.localScale.z);
			}
			PhysicalShockwave component = gameObject.GetComponent<PhysicalShockwave>();
			component.damage = Mathf.RoundToInt(30f * this.eid.totalDamageModifier);
			if (this.difficulty == 1)
			{
				component.speed = 20f;
			}
			else if (this.difficulty == 0)
			{
				component.speed = 15f;
			}
			else if (this.difficulty >= 4)
			{
				component.speed = 35f;
			}
			else
			{
				component.speed = 25f;
			}
			component.maxSize = 100f;
			component.enemy = true;
			component.enemyType = EnemyType.HideousMass;
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.highVisShockwave;
			}
			gameObject.transform.SetParent(base.GetComponentInParent<GoreZone>().transform, true);
		}
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0007D7D4 File Offset: 0x0007B9D4
	public void ShootHoming(int arm)
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.dead)
		{
			Transform transform = this.shootPoints[arm];
			Projectile component = Object.Instantiate<GameObject>(this.homingProjectile, transform.position, transform.rotation).GetComponent<Projectile>();
			component.target = this.eid.target;
			component.GetComponent<Rigidbody>().velocity = transform.up * 5f;
			component.damage *= this.eid.totalDamageModifier;
		}
	}

	// Token: 0x06001057 RID: 4183 RVA: 0x0007D860 File Offset: 0x0007BA60
	public void ShootExplosive(int arm)
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.dead)
		{
			Transform transform = this.shootPoints[arm];
			Projectile component = Object.Instantiate<GameObject>(this.explosiveProjectile, transform.position, transform.rotation).GetComponent<Projectile>();
			component.target = this.eid.target;
			component.GetComponent<Rigidbody>().AddForce(transform.up * 50f, ForceMode.VelocityChange);
			component.safeEnemyType = EnemyType.HideousMass;
			component.transform.SetParent(base.GetComponentInParent<GoreZone>().transform, true);
			component.damage *= this.eid.totalDamageModifier;
		}
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x0007D910 File Offset: 0x0007BB10
	private void ReadySpear()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (this.difficulty <= 3 && this.crazyMode)
		{
			return;
		}
		if (!this.dead && this.difficulty != 0)
		{
			if (this.tailSpear == null)
			{
				this.tailSpear = this.tailEnd.GetChild(1).gameObject;
			}
			this.inSemiAction = true;
			GameObject gameObject = Object.Instantiate<GameObject>(this.spearFlash, this.tailSpear.transform.position, Quaternion.identity);
			Object.Instantiate<GameObject>(this.regurgitateSound, this.tailSpear.transform.position, Quaternion.identity);
			gameObject.transform.SetParent(this.tailSpear.transform, true);
			if (this.crazyMode && this.difficulty >= 4)
			{
				base.Invoke("ShootSpear", 1f);
				return;
			}
			this.anim.SetTrigger("ShootSpear");
		}
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x0007DA08 File Offset: 0x0007BC08
	public void ShootSpear()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.dead && this.difficulty != 0)
		{
			this.inSemiAction = false;
			this.tailEnd.LookAt(this.eid.target.position);
			this.tempSpear = Object.Instantiate<GameObject>(this.spear, this.tailSpear.transform.position, this.tailEnd.rotation);
			this.tempSpear.transform.LookAt(this.eid.target.position);
			MassSpear massSpear;
			if (this.tempSpear.TryGetComponent<MassSpear>(out massSpear))
			{
				massSpear.target = this.eid.target;
				massSpear.originPoint = this.tailSpear.transform;
				massSpear.damageMultiplier = this.eid.totalDamageModifier;
				if (this.difficulty >= 4)
				{
					massSpear.spearHealth *= 2f;
				}
			}
			this.tailSpear.SetActive(false);
			this.spearShot = true;
		}
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x0007DB17 File Offset: 0x0007BD17
	public void SpearParried()
	{
		if (!this.dead)
		{
			this.inAction = true;
			this.anim.SetTrigger("SpearParried");
			Object.Instantiate<GameObject>(this.bigPainSound, this.tailSpear.transform);
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0007DB4F File Offset: 0x0007BD4F
	public void SpearReturned()
	{
		this.tailSpear.SetActive(true);
		this.spearShot = false;
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0007DB64 File Offset: 0x0007BD64
	public void StopAction()
	{
		this.inAction = false;
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0007DB70 File Offset: 0x0007BD70
	public void BattleSlam()
	{
		this.inAction = true;
		this.anim.SetTrigger("BattleSlam");
		this.swingCooldown = (float)Random.Range(3, 5);
		AudioSource component = Object.Instantiate<GameObject>(this.windupSound, this.shootPoints[2].position, Quaternion.identity).GetComponent<AudioSource>();
		component.pitch = 1f;
		component.volume = 0.75f;
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0007DBDC File Offset: 0x0007BDDC
	public void SwingStart()
	{
		if (!this.dead)
		{
			SwingCheck2[] array = this.swingChecks;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStart();
			}
		}
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0007DC10 File Offset: 0x0007BE10
	public void SwingEnd()
	{
		if (!this.dead)
		{
			SwingCheck2[] array = this.swingChecks;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].DamageStop();
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.slamExplosion, (this.shootPoints[0].position + this.shootPoints[1].position) / 2f, Quaternion.identity);
			gameObject.transform.up = base.transform.right;
			PhysicalShockwave component = gameObject.GetComponent<PhysicalShockwave>();
			component.damage = Mathf.RoundToInt(20f * this.eid.totalDamageModifier);
			component.speed = 100f;
			if (this.difficulty < 2)
			{
				component.maxSize = 10f;
			}
			else
			{
				component.maxSize = 100f;
			}
			component.enemy = true;
			component.enemyType = EnemyType.HideousMass;
			gameObject.transform.SetParent(base.GetComponentInParent<GoreZone>().transform, true);
			AudioSource component2 = gameObject.GetComponent<AudioSource>();
			component2.pitch = 1.5f;
			component2.volume = 0.5f;
		}
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0007DD24 File Offset: 0x0007BF24
	public void Enrage()
	{
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, this.stat.chest.transform);
		this.currentEnrageEffect.transform.localScale = Vector3.one * 2f;
		this.currentEnrageEffect.transform.localPosition = new Vector3(-0.25f, 0f, 0f);
		this.stat.smr.material = this.enrageMaterial;
		base.GetComponentInChildren<EnemySimplifier>().enraged = true;
		this.eid.UpdateBuffs(true, true);
		if (this.eid.sandified)
		{
			this.stat.smr.material.SetFloat("_HasSandBuff", 1f);
		}
		GameObject[] array = this.activateOnEnrage;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		base.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0007DE1C File Offset: 0x0007C01C
	public void CrazyReady()
	{
		this.inAction = false;
		this.inSemiAction = false;
		this.crazyMode = true;
		base.Invoke("CrazyShoot", 0.5f / this.eid.totalSpeedModifier);
		base.Invoke("CrazyShoot", 1.5f / this.eid.totalSpeedModifier);
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0007DE78 File Offset: 0x0007C078
	public void CrazyShoot()
	{
		if (!this.dead)
		{
			this.ShootExplosive(this.crazyPoint);
			if (this.crazyPoint == 0)
			{
				this.crazyPoint = 1;
			}
			else
			{
				this.crazyPoint = 0;
			}
			float num = 1f;
			if (this.difficulty >= 4)
			{
				num = 2f;
			}
			base.Invoke("CrazyShoot", Random.Range(2f, 3f) / num / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x04001626 RID: 5670
	private Animator anim;

	// Token: 0x04001627 RID: 5671
	private bool battleMode;

	// Token: 0x04001628 RID: 5672
	private Vector3 targetPos;

	// Token: 0x04001629 RID: 5673
	private Quaternion targetRot;

	// Token: 0x0400162A RID: 5674
	private float transformCooldown;

	// Token: 0x0400162B RID: 5675
	private bool walking;

	// Token: 0x0400162C RID: 5676
	private float walkWeight;

	// Token: 0x0400162D RID: 5677
	public bool inAction;

	// Token: 0x0400162E RID: 5678
	private bool inSemiAction;

	// Token: 0x0400162F RID: 5679
	public Transform[] shootPoints;

	// Token: 0x04001630 RID: 5680
	public GameObject homingProjectile;

	// Token: 0x04001631 RID: 5681
	private float homingAttackChance = 50f;

	// Token: 0x04001632 RID: 5682
	public float attackCooldown = 2f;

	// Token: 0x04001633 RID: 5683
	public GameObject explosiveProjectile;

	// Token: 0x04001634 RID: 5684
	public GameObject slamExplosion;

	// Token: 0x04001635 RID: 5685
	private SwingCheck2[] swingChecks;

	// Token: 0x04001636 RID: 5686
	private float swingCooldown = 2f;

	// Token: 0x04001637 RID: 5687
	private bool attackedOnce;

	// Token: 0x04001638 RID: 5688
	private float playerDistanceCooldown = 1.5f;

	// Token: 0x04001639 RID: 5689
	public Transform tailEnd;

	// Token: 0x0400163A RID: 5690
	private GameObject tailSpear;

	// Token: 0x0400163B RID: 5691
	private float spearCooldown = 5f;

	// Token: 0x0400163C RID: 5692
	public GameObject spear;

	// Token: 0x0400163D RID: 5693
	public bool spearShot;

	// Token: 0x0400163E RID: 5694
	public GameObject spearFlash;

	// Token: 0x0400163F RID: 5695
	public GameObject tempSpear;

	// Token: 0x04001640 RID: 5696
	public List<GameObject> tailHitboxes = new List<GameObject>();

	// Token: 0x04001641 RID: 5697
	public GameObject regurgitateSound;

	// Token: 0x04001642 RID: 5698
	public GameObject bigPainSound;

	// Token: 0x04001643 RID: 5699
	public GameObject windupSound;

	// Token: 0x04001644 RID: 5700
	public bool dead;

	// Token: 0x04001645 RID: 5701
	public bool crazyMode;

	// Token: 0x04001646 RID: 5702
	public float crazyModeHealth;

	// Token: 0x04001647 RID: 5703
	private Statue stat;

	// Token: 0x04001648 RID: 5704
	private EnemyIdentifier eid;

	// Token: 0x04001649 RID: 5705
	private int crazyPoint;

	// Token: 0x0400164A RID: 5706
	public GameObject enrageEffect;

	// Token: 0x0400164B RID: 5707
	public GameObject currentEnrageEffect;

	// Token: 0x0400164C RID: 5708
	public Material enrageMaterial;

	// Token: 0x0400164D RID: 5709
	public Material highVisShockwave;

	// Token: 0x0400164E RID: 5710
	public GameObject[] activateOnEnrage;

	// Token: 0x0400164F RID: 5711
	private int difficulty = -1;
}
