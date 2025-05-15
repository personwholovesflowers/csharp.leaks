using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class Projectile : MonoBehaviour
{
	// Token: 0x06001405 RID: 5125 RVA: 0x000A0060 File Offset: 0x0009E260
	private void Start()
	{
		if (this.aud)
		{
			this.aud.pitch = Random.Range(1.8f, 2f);
			if (this.aud.enabled)
			{
				this.aud.Play();
			}
		}
		if (this.decorative)
		{
			this.origScale = base.transform.localScale;
			base.transform.localScale = Vector3.zero;
		}
		if (this.speed != 0f)
		{
			this.speed += Random.Range(-this.speedRandomizer, this.speedRandomizer);
		}
		if (this.col != null && !this.decorative)
		{
			this.col.enabled = false;
			this.col.enabled = true;
		}
		this.maxSpeed = this.speed;
		if (this.target == null)
		{
			this.target = EnemyTarget.TrackPlayerIfAllowed();
		}
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x000A0150 File Offset: 0x0009E350
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.aud = base.GetComponent<AudioSource>();
		if (this.col == null)
		{
			this.col = base.GetComponentInChildren<Collider>();
		}
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x000A01A5 File Offset: 0x0009E3A5
	public static float GetProjectileSpeedMulti(int difficulty)
	{
		if (difficulty > 2)
		{
			return 1.35f;
		}
		if (difficulty == 1)
		{
			return 0.75f;
		}
		if (difficulty == 0)
		{
			return 0.5f;
		}
		return 1f;
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x000A01CC File Offset: 0x0009E3CC
	private void Update()
	{
		if (this.homingType != HomingType.None && this.target != null && !this.hittingPlayer)
		{
			float num = this.predictiveHomingMultiplier;
			Vector3 vector = (this.target.isPlayer ? this.target.headPosition : this.target.position);
			if (Vector3.Distance(base.transform.position, vector) < 15f)
			{
				num = 0f;
			}
			switch (this.homingType)
			{
			case HomingType.Gradual:
			{
				if (this.difficulty == 1)
				{
					this.maxSpeed += Time.deltaTime * 17.5f;
				}
				else if (this.difficulty == 0)
				{
					this.maxSpeed += Time.deltaTime * 10f;
				}
				else
				{
					this.maxSpeed += Time.deltaTime * 25f;
				}
				Quaternion quaternion = Quaternion.LookRotation(vector + this.target.GetVelocity() * num - base.transform.position);
				if (this.difficulty == 0)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 100f * this.turningSpeedMultiplier);
				}
				else if (this.difficulty == 1)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 135f * this.turningSpeedMultiplier);
				}
				else if (this.difficulty == 2)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 185f * this.turningSpeedMultiplier);
				}
				else
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 200f * this.turningSpeedMultiplier);
				}
				this.rb.velocity = base.transform.forward * this.maxSpeed;
				return;
			}
			case HomingType.Loose:
			{
				this.maxSpeed += Time.deltaTime * 10f;
				base.transform.LookAt(base.transform.position + this.rb.velocity);
				Vector3 normalized = (vector + this.target.GetVelocity() * num - base.transform.position).normalized;
				this.rb.AddForce(normalized * this.speed * Time.deltaTime * 200f, ForceMode.Acceleration);
				this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, this.maxSpeed);
				return;
			}
			case HomingType.HorizontalOnly:
			{
				base.transform.LookAt(this.target.position + this.rb.velocity);
				Vector3 vector2 = vector + this.target.GetVelocity() * num;
				vector2.y = base.transform.position.y;
				float num2 = Mathf.Clamp(vector2.x - base.transform.position.x, -this.turnSpeed, this.turnSpeed);
				float num3 = Mathf.Clamp(vector2.z - base.transform.position.z, -this.turnSpeed, this.turnSpeed);
				if (Vector3.Distance(base.transform.position, vector2) < this.turnSpeed / 20f)
				{
					num2 = (vector2 - base.transform.position).x;
					num3 = (vector2 - base.transform.position).z;
				}
				float num4 = 15f;
				if (this.difficulty == 1)
				{
					num4 = 10f;
				}
				else if (this.difficulty == 0)
				{
					num4 = 5f;
				}
				else if (this.difficulty >= 3)
				{
					num4 = 25f;
				}
				float num5 = Mathf.MoveTowards(this.rb.velocity.x, num2, Time.deltaTime * num4 * this.turningSpeedMultiplier);
				float num6 = Mathf.MoveTowards(this.rb.velocity.z, num3, Time.deltaTime * num4 * this.turningSpeedMultiplier);
				this.rb.velocity = new Vector3(num5, this.rb.velocity.y, num6);
				return;
			}
			case HomingType.Instant:
			{
				Quaternion quaternion = Quaternion.LookRotation(vector + this.target.GetVelocity() * num - base.transform.position);
				if (this.difficulty == 0)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 100f * this.turningSpeedMultiplier);
				}
				else if (this.difficulty == 1)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 135f * this.turningSpeedMultiplier);
				}
				else if (this.difficulty == 2)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 185f * this.turningSpeedMultiplier);
				}
				else
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 200f * this.turningSpeedMultiplier);
				}
				this.rb.velocity = base.transform.forward * this.speed;
				return;
			}
			default:
				this.maxSpeed += Time.deltaTime * 10f;
				this.targetRotation = Quaternion.LookRotation(vector + this.target.GetVelocity() * num - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRotation, Time.deltaTime * this.turnSpeed);
				this.rb.velocity = base.transform.forward * this.maxSpeed;
				break;
			}
		}
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000A0820 File Offset: 0x0009EA20
	private void FixedUpdate()
	{
		if (!this.hittingPlayer && !this.undeflectable && !this.decorative && this.speed != 0f && this.homingType == HomingType.None)
		{
			this.rb.velocity = base.transform.forward * this.speed;
		}
		if (this.decorative && base.transform.localScale.x < this.origScale.x)
		{
			this.aud.pitch = base.transform.localScale.x / this.origScale.x * 2.8f;
			base.transform.localScale = Vector3.Slerp(base.transform.localScale, this.origScale, Time.deltaTime * this.speed);
		}
		if (this.precheckForCollisions)
		{
			LayerMask layerMask = LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment);
			layerMask |= 4;
			RaycastHit raycastHit;
			if (Physics.SphereCast(base.transform.position, this.radius, this.rb.velocity.normalized, out raycastHit, this.rb.velocity.magnitude * Time.fixedDeltaTime, layerMask))
			{
				base.transform.position = base.transform.position + this.rb.velocity.normalized * raycastHit.distance;
				this.Collided(raycastHit.collider);
			}
		}
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000A09AD File Offset: 0x0009EBAD
	private void OnTriggerEnter(Collider other)
	{
		this.Collided(other);
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000A09B8 File Offset: 0x0009EBB8
	private void Collided(Collider other)
	{
		if (!this.active)
		{
			return;
		}
		if (!this.friendly && !this.hittingPlayer && other.gameObject.CompareTag("Player"))
		{
			if (!this.target.isPlayer || MonoSingleton<PlayerTracker>.Instance.playerType != PlayerType.Platformer)
			{
				if (this.spreaded)
				{
					ProjectileSpread componentInParent = base.GetComponentInParent<ProjectileSpread>();
					if (componentInParent != null && componentInParent.parried)
					{
						return;
					}
				}
				this.hittingPlayer = true;
				this.rb.velocity = Vector3.zero;
				if (this.keepTrail)
				{
					this.KeepTrail();
				}
				base.transform.position = new Vector3(other.transform.position.x, base.transform.position.y, other.transform.position.z);
				this.nmov = other.gameObject.GetComponentInParent<NewMovement>();
				base.Invoke("RecheckPlayerHit", 0.05f);
				return;
			}
			MonoSingleton<PlatformerMovement>.Instance.Explode(false);
			if (this.explosive)
			{
				this.Explode();
				return;
			}
			if (this.keepTrail)
			{
				this.KeepTrail();
			}
			this.CreateExplosionEffect();
			Object.Destroy(base.gameObject);
			return;
		}
		else
		{
			if (!this.canHitCoin || !other.gameObject.CompareTag("Coin"))
			{
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
				if ((other.gameObject.CompareTag("Armor") && (this.friendly || !other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid || enemyIdentifierIdentifier.eid.enemyType != this.safeEnemyType)) || (this.boosted && other.gameObject.layer == 11 && other.gameObject.CompareTag("Body") && other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2) && enemyIdentifierIdentifier2.eid && enemyIdentifierIdentifier2.eid.enemyType == EnemyType.MaliciousFace && !enemyIdentifierIdentifier2.eid.isGasolined))
				{
					RaycastHit raycastHit;
					if (!this.alreadyDeflectedBy.Contains(other) && Physics.Raycast(base.transform.position - base.transform.forward, base.transform.forward, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment)))
					{
						base.transform.forward = Vector3.Reflect(base.transform.forward, raycastHit.normal).normalized;
						base.transform.position = raycastHit.point + base.transform.forward;
						Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.ineffectiveSound, base.transform.position, Quaternion.identity);
						this.alreadyDeflectedBy.Add(other);
						return;
					}
				}
				else if (this.active && (other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Body") || other.gameObject.CompareTag("Limb") || other.gameObject.CompareTag("EndLimb")) && !other.gameObject.CompareTag("Armor"))
				{
					EnemyIdentifierIdentifier componentInParent2 = other.gameObject.GetComponentInParent<EnemyIdentifierIdentifier>();
					EnemyIdentifier enemyIdentifier = null;
					if (componentInParent2 != null && componentInParent2.eid != null)
					{
						enemyIdentifier = componentInParent2.eid;
					}
					if (enemyIdentifier != null && (this.alreadyHitEnemies.Count == 0 || !this.alreadyHitEnemies.Contains(enemyIdentifier)) && ((enemyIdentifier.enemyType != this.safeEnemyType && !EnemyIdentifier.CheckHurtException(this.safeEnemyType, enemyIdentifier.enemyType, this.target)) || (this.friendly && !enemyIdentifier.immuneToFriendlyFire) || this.playerBullet || this.parried))
					{
						if (this.explosive)
						{
							this.Explode();
						}
						this.active = false;
						bool flag = false;
						bool dead = enemyIdentifier.dead;
						if (this.playerBullet)
						{
							enemyIdentifier.hitter = this.bulletType;
							if (!enemyIdentifier.hitterWeapons.Contains(this.weaponType))
							{
								enemyIdentifier.hitterWeapons.Add(this.weaponType);
							}
						}
						else if (!this.friendly)
						{
							enemyIdentifier.hitter = "enemy";
						}
						else
						{
							enemyIdentifier.hitter = "projectile";
							flag = true;
						}
						if (this.boosted && !enemyIdentifier.blessed && !enemyIdentifier.dead)
						{
							MonoSingleton<StyleHUD>.Instance.AddPoints(90, "ultrakill.projectileboost", this.sourceWeapon, enemyIdentifier, -1, "", "");
						}
						bool flag2 = true;
						if (this.spreaded)
						{
							ProjectileSpread componentInParent3 = base.GetComponentInParent<ProjectileSpread>();
							if (componentInParent3 != null)
							{
								if (componentInParent3.hitEnemies.Contains(enemyIdentifier))
								{
									flag2 = false;
								}
								else
								{
									componentInParent3.hitEnemies.Add(enemyIdentifier);
								}
							}
						}
						if (!this.explosive)
						{
							if (flag2)
							{
								if (this.playerBullet)
								{
									enemyIdentifier.DeliverDamage(other.gameObject, this.rb.velocity.normalized * 2500f, base.transform.position, this.damage / 4f * this.enemyDamageMultiplier, flag, 0f, this.sourceWeapon, false, false);
								}
								else if (this.friendly)
								{
									enemyIdentifier.DeliverDamage(other.gameObject, this.rb.velocity.normalized * 10000f, base.transform.position, this.damage / 4f * this.enemyDamageMultiplier, flag, 0f, this.sourceWeapon, false, false);
								}
								else
								{
									enemyIdentifier.DeliverDamage(other.gameObject, this.rb.velocity.normalized * 100f, base.transform.position, this.damage / 10f * this.enemyDamageMultiplier, flag, 0f, this.sourceWeapon, false, false);
								}
							}
							this.CreateExplosionEffect();
						}
						if (this.keepTrail)
						{
							this.KeepTrail();
						}
						if (!dead)
						{
							MonoSingleton<TimeController>.Instance.HitStop(0.005f);
						}
						if (!dead || other.gameObject.layer == 11 || this.boosted)
						{
							Object.Destroy(base.gameObject);
							return;
						}
						this.alreadyHitEnemies.Add(enemyIdentifier);
						this.active = true;
						return;
					}
				}
				else
				{
					if (!this.hittingPlayer && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) && !this.ignoreEnvironment && this.active)
					{
						Breakable component = other.gameObject.GetComponent<Breakable>();
						if (component != null && !component.precisionOnly && !component.specialCaseOnly && (component.weak || this.strong))
						{
							component.Break();
						}
						Bleeder bleeder;
						if (other.gameObject.TryGetComponent<Bleeder>(out bleeder))
						{
							bool flag3 = false;
							if (!this.friendly && !this.playerBullet && bleeder.ignoreTypes.Length != 0)
							{
								EnemyType[] ignoreTypes = bleeder.ignoreTypes;
								for (int i = 0; i < ignoreTypes.Length; i++)
								{
									if (ignoreTypes[i] == this.safeEnemyType)
									{
										flag3 = true;
										break;
									}
								}
							}
							if (!flag3)
							{
								if (this.damage <= 10f)
								{
									bleeder.GetHit(base.transform.position, GoreType.Body, false);
								}
								else if (this.damage <= 30f)
								{
									bleeder.GetHit(base.transform.position, GoreType.Limb, false);
								}
								else
								{
									bleeder.GetHit(base.transform.position, GoreType.Head, false);
								}
							}
						}
						if (SceneHelper.IsStaticEnvironment(other))
						{
							MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(base.transform.position - base.transform.forward, base.transform.forward, 5f, Mathf.Max(2, Mathf.RoundToInt(this.damage / (float)((this.playerBullet || this.friendly) ? 4 : 10))), Mathf.Min(1f, Mathf.Max(0.5f, this.damage / (float)((this.playerBullet || this.friendly) ? 4 : 10))));
						}
						if (this.explosive)
						{
							this.Explode();
						}
						else
						{
							if (this.keepTrail)
							{
								this.KeepTrail();
							}
							this.CreateExplosionEffect();
							Object.Destroy(base.gameObject);
						}
						this.active = false;
						return;
					}
					if (other.gameObject.layer == 0)
					{
						Rigidbody componentInParent4 = other.GetComponentInParent<Rigidbody>();
						if (componentInParent4 != null)
						{
							componentInParent4.AddForce(base.transform.forward * 1000f);
						}
					}
				}
				return;
			}
			Coin component2 = other.gameObject.GetComponent<Coin>();
			if (component2 && !component2.shot)
			{
				if (!this.friendly)
				{
					if (this.target != null)
					{
						component2.customTarget = this.target;
					}
					component2.DelayedEnemyReflect();
				}
				else
				{
					component2.DelayedReflectRevolver(component2.transform.position, null);
				}
			}
			if (this.explosive)
			{
				this.Explode();
				return;
			}
			if (this.keepTrail)
			{
				this.KeepTrail();
			}
			this.active = false;
			this.CreateExplosionEffect();
			Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000A130C File Offset: 0x0009F50C
	private void CreateExplosionEffect()
	{
		if (this.explosionEffect == null)
		{
			return;
		}
		foreach (Explosion explosion in Object.Instantiate<GameObject>(this.explosionEffect, base.transform.position, base.transform.rotation).GetComponentsInChildren<Explosion>())
		{
			explosion.sourceWeapon = this.sourceWeapon ?? explosion.sourceWeapon;
			if (explosion.damage != 0 && ((!this.friendly && !this.playerBullet) || (float)explosion.damage < this.damage))
			{
				explosion.damage = (int)this.damage;
			}
		}
		if (this.boosted || this.parried)
		{
			MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, 3);
		}
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000A13D4 File Offset: 0x0009F5D4
	public void Explode()
	{
		if (this.active)
		{
			this.active = false;
			if (this.keepTrail)
			{
				this.KeepTrail();
			}
			foreach (Explosion explosion in Object.Instantiate<GameObject>(this.explosionEffect, base.transform.position - this.rb.velocity * 0.02f, base.transform.rotation).GetComponentsInChildren<Explosion>())
			{
				explosion.sourceWeapon = this.sourceWeapon ?? explosion.sourceWeapon;
				if (this.bigExplosion)
				{
					explosion.maxSize *= 1.5f;
				}
				if (explosion.damage != 0)
				{
					explosion.damage = Mathf.RoundToInt(this.damage);
				}
				explosion.enemy = true;
			}
			MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, 3);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x000A14C8 File Offset: 0x0009F6C8
	private void RecheckPlayerHit()
	{
		if (this.hittingPlayer)
		{
			this.hittingPlayer = false;
			if (this.col)
			{
				this.col.enabled = false;
			}
			this.undeflectable = true;
			base.Invoke("TimeToDie", 0.01f);
		}
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x000A1514 File Offset: 0x0009F714
	private void TimeToDie()
	{
		bool flag = false;
		if (this.spreaded)
		{
			ProjectileSpread componentInParent = base.GetComponentInParent<ProjectileSpread>();
			if (componentInParent != null && componentInParent.parried)
			{
				flag = true;
			}
		}
		this.CreateExplosionEffect();
		if (!flag)
		{
			if (this.explosive)
			{
				base.transform.position = base.transform.position - base.transform.forward;
				this.Explode();
			}
			else
			{
				this.nmov.GetHurt(Mathf.RoundToInt(this.damage), true, 1f, false, false, 0.35f, false);
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x000A15B4 File Offset: 0x0009F7B4
	private void KeepTrail()
	{
		TrailRenderer componentInChildren = base.GetComponentInChildren<TrailRenderer>();
		if (componentInChildren != null)
		{
			componentInChildren.transform.parent = null;
			componentInChildren.gameObject.AddComponent<RemoveOnTime>().time = 3f;
		}
	}

	// Token: 0x04001B62 RID: 7010
	public GameObject sourceWeapon;

	// Token: 0x04001B63 RID: 7011
	private Rigidbody rb;

	// Token: 0x04001B64 RID: 7012
	public float speed;

	// Token: 0x04001B65 RID: 7013
	public float turnSpeed;

	// Token: 0x04001B66 RID: 7014
	public float speedRandomizer;

	// Token: 0x04001B67 RID: 7015
	private AudioSource aud;

	// Token: 0x04001B68 RID: 7016
	public GameObject explosionEffect;

	// Token: 0x04001B69 RID: 7017
	public float damage;

	// Token: 0x04001B6A RID: 7018
	public float enemyDamageMultiplier = 1f;

	// Token: 0x04001B6B RID: 7019
	public bool friendly;

	// Token: 0x04001B6C RID: 7020
	public bool playerBullet;

	// Token: 0x04001B6D RID: 7021
	public string bulletType;

	// Token: 0x04001B6E RID: 7022
	public string weaponType;

	// Token: 0x04001B6F RID: 7023
	public bool decorative;

	// Token: 0x04001B70 RID: 7024
	private Vector3 origScale;

	// Token: 0x04001B71 RID: 7025
	private bool active = true;

	// Token: 0x04001B72 RID: 7026
	public EnemyType safeEnemyType;

	// Token: 0x04001B73 RID: 7027
	public bool explosive;

	// Token: 0x04001B74 RID: 7028
	public bool bigExplosion;

	// Token: 0x04001B75 RID: 7029
	public List<EnemyIdentifier> alreadyHitEnemies = new List<EnemyIdentifier>();

	// Token: 0x04001B76 RID: 7030
	public HomingType homingType;

	// Token: 0x04001B77 RID: 7031
	public float turningSpeedMultiplier = 1f;

	// Token: 0x04001B78 RID: 7032
	public EnemyTarget target;

	// Token: 0x04001B79 RID: 7033
	private float maxSpeed;

	// Token: 0x04001B7A RID: 7034
	private Quaternion targetRotation;

	// Token: 0x04001B7B RID: 7035
	public float predictiveHomingMultiplier;

	// Token: 0x04001B7C RID: 7036
	public bool hittingPlayer;

	// Token: 0x04001B7D RID: 7037
	private NewMovement nmov;

	// Token: 0x04001B7E RID: 7038
	public bool boosted;

	// Token: 0x04001B7F RID: 7039
	[HideInInspector]
	public bool parried;

	// Token: 0x04001B80 RID: 7040
	private Collider col;

	// Token: 0x04001B81 RID: 7041
	private float radius;

	// Token: 0x04001B82 RID: 7042
	public bool undeflectable;

	// Token: 0x04001B83 RID: 7043
	public bool unparryable;

	// Token: 0x04001B84 RID: 7044
	public bool keepTrail;

	// Token: 0x04001B85 RID: 7045
	public bool strong;

	// Token: 0x04001B86 RID: 7046
	public bool spreaded;

	// Token: 0x04001B87 RID: 7047
	private int difficulty;

	// Token: 0x04001B88 RID: 7048
	public bool precheckForCollisions;

	// Token: 0x04001B89 RID: 7049
	public bool canHitCoin;

	// Token: 0x04001B8A RID: 7050
	public bool ignoreExplosions;

	// Token: 0x04001B8B RID: 7051
	public bool ignoreEnvironment;

	// Token: 0x04001B8C RID: 7052
	private List<Collider> alreadyDeflectedBy = new List<Collider>();

	// Token: 0x04001B8D RID: 7053
	public List<ContinuousBeam> connectedBeams = new List<ContinuousBeam>();
}
