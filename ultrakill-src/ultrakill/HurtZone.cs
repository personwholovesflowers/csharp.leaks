using System;
using System.Collections.Generic;
using Sandbox;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200025A RID: 602
public class HurtZone : MonoBehaviour, IAlter, IAlterOptions<float>
{
	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000D44 RID: 3396 RVA: 0x00064D10 File Offset: 0x00062F10
	private float damage
	{
		get
		{
			return this.setDamage * this.damageMultiplier;
		}
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x00064D20 File Offset: 0x00062F20
	private void Start()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (this.difficulty < 2 && this.damage < 100f)
		{
			if (this.difficulty == 1)
			{
				this.damageMultiplier = 0.5f;
			}
			else if (this.difficulty == 0)
			{
				this.damageMultiplier = 0.25f;
			}
		}
		this.col = base.GetComponent<Collider>();
		this.rb = base.GetComponent<Rigidbody>();
		if (this.rb)
		{
			Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].attachedRigidbody == this.rb)
				{
					this.childColliders.Add(componentsInChildren[i]);
				}
			}
		}
		this.newMovement = MonoSingleton<NewMovement>.Instance;
		this.playerTracker = MonoSingleton<PlayerTracker>.Instance;
		this.platformerMovement = MonoSingleton<PlatformerMovement>.Instance;
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00064E00 File Offset: 0x00063000
	private void OnDisable()
	{
		this.hurtingPlayer = 0;
		this.enemies.Clear();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00064E14 File Offset: 0x00063014
	private void FixedUpdate()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.trigger && this.ignoreDashingPlayer && this.ignoringDash != (MonoSingleton<NewMovement>.Instance.gameObject.layer == 15))
		{
			this.ignoringDash = MonoSingleton<NewMovement>.Instance.gameObject.layer == 15;
			Physics.IgnoreCollision(this.col, MonoSingleton<NewMovement>.Instance.playerCollider, this.ignoringDash);
		}
		if (this.hurtingPlayer > 0 && this.playerHurtCooldown <= 0f && (this.ignoreInvincibility || MonoSingleton<NewMovement>.Instance.gameObject.layer != 15))
		{
			if (this.playerTracker.playerType == PlayerType.FPS)
			{
				if (this.newMovement == null)
				{
					this.newMovement = MonoSingleton<NewMovement>.Instance;
				}
				if (!this.newMovement.dead && this.newMovement.gameObject.activeInHierarchy)
				{
					if (this.damage > 0f)
					{
						if (this.hardDamagePercentage > 0f)
						{
							this.newMovement.GetHurt((int)this.damage, true, 1f, false, false, this.hardDamagePercentage, this.ignoreInvincibility);
						}
						else
						{
							this.newMovement.GetHurt((int)this.damage, false, 1f, false, false, 0.35f, this.ignoreInvincibility);
						}
					}
					if (this.hurtParticle)
					{
						Object.Instantiate<GameObject>(this.hurtParticle, this.newMovement.transform.position, Quaternion.identity);
					}
					if (this.bounceForce != 0f)
					{
						Vector3 vector = this.newMovement.transform.position + Vector3.down;
						if (this.col && !this.rb)
						{
							vector = this.col.ClosestPoint(this.newMovement.transform.position);
						}
						else if (this.rb && this.childColliders.Count > 0)
						{
							vector = this.childColliders[0].ClosestPoint(this.newMovement.transform.position);
							if (this.childColliders.Count > 1)
							{
								float num = Vector3.Distance(this.newMovement.playerCollider.ClosestPoint(vector), vector);
								for (int i = 1; i < this.childColliders.Count; i++)
								{
									Vector3 vector2 = this.childColliders[i].ClosestPoint(this.newMovement.transform.position);
									float num2 = Vector3.Distance(this.newMovement.playerCollider.ClosestPoint(vector), vector2);
									if (num2 < num)
									{
										num = num2;
										vector = vector2;
									}
								}
							}
						}
						this.newMovement.Launch((this.newMovement.transform.position - vector).normalized * this.bounceForce, 8f, false);
					}
				}
				else
				{
					this.hurtingPlayer = 0;
				}
			}
			else if (this.damage > 0f)
			{
				if (this.platformerMovement == null)
				{
					this.platformerMovement = MonoSingleton<PlatformerMovement>.Instance;
				}
				if (!this.platformerMovement.dead && this.platformerMovement.gameObject.activeInHierarchy)
				{
					if (this.damageType == EnviroDamageType.WeakBurn || this.damageType == EnviroDamageType.Burn || this.damageType == EnviroDamageType.Acid)
					{
						this.platformerMovement.Burn(false);
					}
					else
					{
						this.platformerMovement.Explode(false);
						if (this.hurtParticle)
						{
							Object.Instantiate<GameObject>(this.hurtParticle, this.platformerMovement.transform.position, Quaternion.identity);
						}
					}
				}
				else
				{
					this.hurtingPlayer = 0;
				}
			}
			this.playerHurtCooldown = this.hurtCooldown;
		}
		else if (this.playerHurtCooldown > 0f)
		{
			this.playerHurtCooldown -= Time.deltaTime;
		}
		if (this.enemies.Count > 0)
		{
			for (int j = this.enemies.Count - 1; j >= 0; j--)
			{
				if (this.enemies[j] == null || this.enemies[j].target == null || !this.enemies[j].HasLimbs(this.col))
				{
					this.enemies.RemoveAt(j);
				}
				else
				{
					EnemyIdentifier target = this.enemies[j].target;
					float num3 = this.enemies[j].timer;
					num3 -= Time.deltaTime;
					if (num3 <= 0f)
					{
						if (!this.DamageEnemy(target, j))
						{
							goto IL_04D7;
						}
						if (target.dead && this.damageType == EnviroDamageType.Acid)
						{
							num3 = 0.1f;
						}
						else
						{
							num3 = this.hurtCooldown;
						}
					}
					this.enemies[j].timer = num3;
				}
				IL_04D7:;
			}
		}
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x00065308 File Offset: 0x00063508
	private bool DamageEnemy(EnemyIdentifier eid, int i)
	{
		V2 v;
		if (eid.enemyType == EnemyType.V2 && eid.TryGetComponent<V2>(out v) && v.inIntro)
		{
			return false;
		}
		if (this.damageType == EnviroDamageType.Burn || this.damageType == EnviroDamageType.WeakBurn)
		{
			eid.hitter = "fire";
		}
		else if (this.damageType == EnviroDamageType.Acid)
		{
			eid.hitter = "acid";
		}
		else if (this.damageType == EnviroDamageType.Chainsaw)
		{
			eid.hitter = "chainsawzone";
		}
		else
		{
			eid.hitter = "environment";
		}
		GameObject gameObject = eid.gameObject;
		RaycastHit raycastHit;
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (this.damageType == EnviroDamageType.Chainsaw && Physics.Raycast(MonoSingleton<CameraController>.Instance.GetDefaultPos(), MonoSingleton<CameraController>.Instance.transform.forward, out raycastHit, 15f, LayerMaskDefaults.Get(LMD.Enemies)) && raycastHit.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && enemyIdentifierIdentifier.eid == eid)
		{
			gameObject = raycastHit.transform.gameObject;
		}
		else
		{
			gameObject = this.enemies[i].limbs[this.enemies[i].limbs.Count - 1].gameObject;
		}
		if (eid.dead && this.damageType != EnviroDamageType.Chainsaw)
		{
			if (eid.enemyClass == EnemyClass.Demon || (eid.enemyClass == EnemyClass.Machine && eid.machine && !eid.machine.dismemberment))
			{
				this.enemies.RemoveAt(i);
				return false;
			}
			if (gameObject == eid.gameObject || gameObject.layer == 12 || gameObject.layer == 20 || gameObject.CompareTag("Body"))
			{
				this.enemies[i].limbs.RemoveAt(this.enemies[i].limbs.Count - 1);
			}
		}
		Vector3 vector = eid.transform.position;
		if (this.damageType == EnviroDamageType.Chainsaw)
		{
			Collider collider;
			if (eid.dead)
			{
				vector = gameObject.transform.position;
			}
			else if (eid.TryGetComponent<Collider>(out collider) && collider.Raycast(new Ray(MonoSingleton<CameraController>.Instance.GetDefaultPos(), MonoSingleton<CameraController>.Instance.transform.forward), out raycastHit, 15f))
			{
				vector = raycastHit.point;
			}
			else if (collider)
			{
				vector = collider.ClosestPoint(MonoSingleton<CameraController>.Instance.GetDefaultPos());
			}
			else
			{
				vector = new Vector3(eid.transform.position.x, MonoSingleton<CameraController>.Instance.GetDefaultPos().y, eid.transform.position.z);
			}
			MonoSingleton<CameraController>.Instance.CameraShake(0.2f);
		}
		if (this.hurtParticle && !eid.dead)
		{
			Object.Instantiate<GameObject>(this.hurtParticle, vector, Quaternion.identity);
		}
		eid.DeliverDamage(gameObject, Vector3.zero, vector, (this.enemyDamageOverride == 0f) ? (this.damage / 2f) : this.enemyDamageOverride, false, 0f, this.sourceWeapon, false, false);
		if ((this.damageType == EnviroDamageType.Burn || this.damageType == EnviroDamageType.WeakBurn) && !eid.dead)
		{
			Flammable componentInChildren = eid.GetComponentInChildren<Flammable>();
			if (componentInChildren != null)
			{
				componentInChildren.Burn(4f, false);
			}
		}
		return true;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00065660 File Offset: 0x00063860
	private HurtZoneEnemyTracker EnemiesContains(EnemyIdentifier eid)
	{
		if (this.enemies.Count == 0)
		{
			return null;
		}
		for (int i = this.enemies.Count - 1; i >= 0; i--)
		{
			if (this.enemies[i] == null || this.enemies[i].target == null)
			{
				this.enemies.RemoveAt(i);
			}
			else if (this.enemies[i].target == eid)
			{
				return this.enemies[i];
			}
		}
		return null;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x000656F0 File Offset: 0x000638F0
	private void Enter(Collider other)
	{
		if (this.affected != AffectedSubjects.EnemiesOnly && other.gameObject.CompareTag("Player"))
		{
			this.hurtingPlayer++;
			return;
		}
		if (this.affected != AffectedSubjects.PlayerOnly && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12 || other.gameObject.layer == 20))
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (other.gameObject.layer == 12)
			{
				enemyIdentifierIdentifier = other.gameObject.GetComponentInChildren<EnemyIdentifierIdentifier>();
			}
			else if (other.gameObject.layer == 20 && other.transform.parent)
			{
				enemyIdentifierIdentifier = other.transform.parent.GetComponentInChildren<EnemyIdentifierIdentifier>();
			}
			else
			{
				enemyIdentifierIdentifier = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
			}
			if (enemyIdentifierIdentifier != null && enemyIdentifierIdentifier.eid != null && (this.ignoredEnemyTypes.Count == 0 || !this.ignoredEnemyTypes.Contains(enemyIdentifierIdentifier.eid.enemyType)) && (!enemyIdentifierIdentifier.eid.dead || ((this.damageType == EnviroDamageType.Chainsaw || enemyIdentifierIdentifier.eid.enemyClass != EnemyClass.Demon) && other.gameObject.layer != 12 && other.gameObject.layer != 20)) && enemyIdentifierIdentifier.transform.localScale != Vector3.zero)
			{
				if (this.damageType == EnviroDamageType.WeakBurn && (enemyIdentifierIdentifier.eid.enemyType == EnemyType.Streetcleaner || enemyIdentifierIdentifier.eid.enemyType == EnemyType.Sisyphus || enemyIdentifierIdentifier.eid.enemyType == EnemyType.Stalker))
				{
					return;
				}
				HurtZoneEnemyTracker hurtZoneEnemyTracker = this.EnemiesContains(enemyIdentifierIdentifier.eid);
				if (hurtZoneEnemyTracker == null)
				{
					this.enemies.Add(new HurtZoneEnemyTracker(enemyIdentifierIdentifier.eid, other, this.hurtCooldown));
					if (!base.enabled)
					{
						return;
					}
					this.DamageEnemy(enemyIdentifierIdentifier.eid, this.enemies.Count - 1);
					return;
				}
				else
				{
					hurtZoneEnemyTracker.limbs.Add(other);
				}
			}
		}
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0006590C File Offset: 0x00063B0C
	private void Exit(Collider other)
	{
		if (this.affected != AffectedSubjects.EnemiesOnly && other.gameObject.CompareTag("Player") && this.hurtingPlayer > 0)
		{
			this.hurtingPlayer--;
			return;
		}
		if (this.affected != AffectedSubjects.PlayerOnly && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12 || other.gameObject.layer == 20))
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (other.gameObject.layer == 12)
			{
				enemyIdentifierIdentifier = other.gameObject.GetComponentInChildren<EnemyIdentifierIdentifier>();
			}
			else if (other.gameObject.layer == 20 && other.transform.parent)
			{
				enemyIdentifierIdentifier = other.transform.parent.GetComponentInChildren<EnemyIdentifierIdentifier>();
			}
			else
			{
				enemyIdentifierIdentifier = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
			}
			if (enemyIdentifierIdentifier != null && enemyIdentifierIdentifier.eid != null)
			{
				HurtZoneEnemyTracker hurtZoneEnemyTracker = this.EnemiesContains(enemyIdentifierIdentifier.eid);
				if (hurtZoneEnemyTracker == null || !hurtZoneEnemyTracker.limbs.Contains(other))
				{
					return;
				}
				hurtZoneEnemyTracker.limbs.Remove(other);
				if (!hurtZoneEnemyTracker.HasLimbs(this.col))
				{
					this.enemies.Remove(hurtZoneEnemyTracker);
				}
			}
		}
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x00065A52 File Offset: 0x00063C52
	private void OnTriggerEnter(Collider other)
	{
		if (this.trigger || other.gameObject.layer == 20)
		{
			this.Enter(other);
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x00065A72 File Offset: 0x00063C72
	private void OnCollisionEnter(Collision other)
	{
		if (!this.trigger)
		{
			this.Enter(other.collider);
		}
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00065A88 File Offset: 0x00063C88
	private void OnTriggerExit(Collider other)
	{
		if (this.trigger || other.gameObject.layer == 20)
		{
			this.Exit(other);
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00065AA8 File Offset: 0x00063CA8
	private void OnCollisionExit(Collision other)
	{
		if (!this.trigger)
		{
			this.Exit(other.collider);
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00065ABE File Offset: 0x00063CBE
	public string alterKey
	{
		get
		{
			return "hurt_zone";
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00065AC5 File Offset: 0x00063CC5
	public string alterCategoryName
	{
		get
		{
			return "Hurt Zone";
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00065ACC File Offset: 0x00063CCC
	public AlterOption<float>[] options
	{
		get
		{
			return new AlterOption<float>[]
			{
				new AlterOption<float>
				{
					key = "damage",
					name = "Damage",
					value = this.setDamage,
					callback = delegate(float f)
					{
						this.setDamage = f;
					},
					constraints = new SliderConstraints
					{
						min = 0f,
						max = 200f
					}
				},
				new AlterOption<float>
				{
					key = "hurt_cooldown",
					name = "Hurt Cooldown",
					value = this.hurtCooldown,
					callback = delegate(float f)
					{
						this.hurtCooldown = f;
					},
					constraints = new SliderConstraints
					{
						min = 0f,
						max = 10f,
						step = 0.1f
					}
				}
			};
		}
	}

	// Token: 0x040011DB RID: 4571
	public EnviroDamageType damageType;

	// Token: 0x040011DC RID: 4572
	public bool trigger;

	// Token: 0x040011DD RID: 4573
	public AffectedSubjects affected;

	// Token: 0x040011DE RID: 4574
	public bool ignoreDashingPlayer;

	// Token: 0x040011DF RID: 4575
	public bool ignoreInvincibility;

	// Token: 0x040011E0 RID: 4576
	private bool ignoringDash;

	// Token: 0x040011E1 RID: 4577
	public float bounceForce;

	// Token: 0x040011E2 RID: 4578
	private Collider col;

	// Token: 0x040011E3 RID: 4579
	public float hurtCooldown = 1f;

	// Token: 0x040011E4 RID: 4580
	[FormerlySerializedAs("damage")]
	public float setDamage;

	// Token: 0x040011E5 RID: 4581
	public float hardDamagePercentage = 0.35f;

	// Token: 0x040011E6 RID: 4582
	public float enemyDamageOverride;

	// Token: 0x040011E7 RID: 4583
	private int hurtingPlayer;

	// Token: 0x040011E8 RID: 4584
	private float playerHurtCooldown;

	// Token: 0x040011E9 RID: 4585
	private Rigidbody rb;

	// Token: 0x040011EA RID: 4586
	private List<Collider> childColliders = new List<Collider>();

	// Token: 0x040011EB RID: 4587
	private List<HurtZoneEnemyTracker> enemies = new List<HurtZoneEnemyTracker>();

	// Token: 0x040011EC RID: 4588
	public GameObject hurtParticle;

	// Token: 0x040011ED RID: 4589
	private int difficulty;

	// Token: 0x040011EE RID: 4590
	private float damageMultiplier = 1f;

	// Token: 0x040011EF RID: 4591
	private NewMovement newMovement;

	// Token: 0x040011F0 RID: 4592
	private PlayerTracker playerTracker;

	// Token: 0x040011F1 RID: 4593
	private PlatformerMovement platformerMovement;

	// Token: 0x040011F2 RID: 4594
	public List<EnemyType> ignoredEnemyTypes = new List<EnemyType>();

	// Token: 0x040011F3 RID: 4595
	public GameObject sourceWeapon;
}
