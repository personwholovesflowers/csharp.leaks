using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class Explosion : MonoBehaviour
{
	// Token: 0x060008FD RID: 2301 RVA: 0x0003AB6C File Offset: 0x00038D6C
	private void Start()
	{
		this.explosionTime = 0f;
		this.mr = base.GetComponent<MeshRenderer>();
		this.materialColor = this.mr.material.GetColor("_Color");
		this.originalMaterial = this.mr.sharedMaterial;
		this.mr.material = new Material(MonoSingleton<DefaultReferenceManager>.Instance.blankMaterial);
		this.whiteExplosion = true;
		this.cc = MonoSingleton<CameraController>.Instance;
		float num = Vector3.Distance(base.transform.position, this.cc.transform.position);
		float num2 = ((this.damage == 0) ? 0.25f : 1f);
		if (num < 3f * this.maxSize)
		{
			this.cc.CameraShake(1.5f * num2);
		}
		else if (num < 85f)
		{
			this.cc.CameraShake((1.5f - (num - 20f) / 65f * 1.5f) / 6f * this.maxSize * num2);
		}
		this.scol = base.GetComponent<SphereCollider>();
		if (this.scol)
		{
			this.scol.enabled = true;
		}
		if (this.speed == 0f)
		{
			this.speed = 1f;
		}
		if (!this.lowQuality)
		{
			this.lowQuality = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("simpleExplosions", false);
		}
		if (MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Count > 0)
		{
			foreach (Collider collider in Physics.OverlapSphere(base.transform.position, 1f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				ScrollingTexture scrollingTexture;
				if (MonoSingleton<ComponentsDatabase>.Instance.scrollers.Contains(collider.transform) && collider.transform.TryGetComponent<ScrollingTexture>(out scrollingTexture))
				{
					scrollingTexture.attachedObjects.Add(base.transform);
				}
			}
		}
		if (!this.lowQuality)
		{
			this.light = base.GetComponentInChildren<Light>();
			this.light.enabled = true;
			if (this.explosionChunk != null)
			{
				for (int j = 0; j < Random.Range(24, 30); j++)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.explosionChunk, base.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.rotation);
					Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 2f), Random.Range(-1f, 1f));
					gameObject.GetComponent<Rigidbody>().AddForce(vector * 250f, ForceMode.VelocityChange);
					Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), this.scol);
				}
			}
		}
		this.lmask = LayerMaskDefaults.Get(LMD.Environment);
		this.lmask |= 67108864;
		this.speed *= Explosion.globalSizeMulti;
		this.maxSize *= Explosion.globalSizeMulti;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0003AEB8 File Offset: 0x000390B8
	private void Update()
	{
		if (this.light != null)
		{
			this.light.range += 5f * Time.deltaTime * this.speed;
		}
		if (this.whiteExplosion && this.explosionTime > 0.1f)
		{
			this.whiteExplosion = false;
			this.mr.material = new Material(this.originalMaterial);
		}
		if (this.fading)
		{
			this.materialColor.a = this.materialColor.a - 2f * Time.deltaTime;
			if (this.light != null)
			{
				this.light.intensity -= 65f * Time.deltaTime;
			}
			this.mr.material.SetColor("_Color", this.materialColor);
			if (this.materialColor.a <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0003AFB4 File Offset: 0x000391B4
	private void FixedUpdate()
	{
		base.transform.localScale += Vector3.one * 0.05f * this.speed;
		float num = base.transform.lossyScale.x * this.scol.radius;
		if (!this.fading && num > this.maxSize)
		{
			this.harmless = true;
			this.scol.enabled = false;
			this.fading = true;
			this.speed /= 4f;
		}
		if (!this.halved && num > this.maxSize / 2f)
		{
			this.halved = true;
			this.damage = Mathf.RoundToInt((float)this.damage / 1.5f);
		}
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0003B081 File Offset: 0x00039281
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 9)
		{
			return;
		}
		if (!this.harmless)
		{
			this.Collide(other);
		}
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0003B0A4 File Offset: 0x000392A4
	private void Collide(Collider other)
	{
		Vector3 position = other.transform.position;
		Vector3 normalized = (position - base.transform.position).normalized;
		float num = Vector3.Distance(position, base.transform.position);
		Vector3 vector = base.transform.position - normalized * 0.01f;
		float num2 = Vector3.Distance(vector, position);
		int instanceID = other.GetInstanceID();
		if (!this.hitColliders.Contains(instanceID))
		{
			if (!this.hasHitPlayer && other.gameObject.CompareTag("Player"))
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(vector, normalized, out raycastHit, num2, 2048, QueryTriggerInteraction.Ignore))
				{
					return;
				}
				if (this.enemy && Physics.Raycast(position, -normalized, num - 0.1f, this.lmask, QueryTriggerInteraction.Ignore))
				{
					return;
				}
				this.hasHitPlayer = true;
				this.hitColliders.Add(instanceID);
				if (this.canHit != AffectedSubjects.EnemiesOnly)
				{
					if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && this.damage > 0)
					{
						MonoSingleton<PlatformerMovement>.Instance.Burn(false);
						return;
					}
					if (!MonoSingleton<NewMovement>.Instance.exploded && (MonoSingleton<NewMovement>.Instance.safeExplosionLaunchCooldown <= 0f || this.damage > 0))
					{
						int num3 = 200;
						if (this.rocketExplosion && this.damage == 0)
						{
							num3 = Mathf.RoundToInt(100f / ((float)(MonoSingleton<NewMovement>.Instance.rocketJumps + 3) / 3f));
							MonoSingleton<NewMovement>.Instance.rocketJumps++;
						}
						Vector3 vector2 = base.transform.position - position;
						vector2 = new Vector3(vector2.x, 0f, vector2.z);
						if (vector2.sqrMagnitude < 0.0625f)
						{
							if (this.isFup)
							{
								MonoSingleton<NewMovement>.Instance.LaunchFromPointAtSpeed(position, 60f);
							}
							else
							{
								MonoSingleton<NewMovement>.Instance.LaunchFromPoint(position, (float)num3, this.maxSize);
								if (this.ultrabooster && num < 12f)
								{
									MonoSingleton<NewMovement>.Instance.LaunchFromPoint(position, (float)num3, this.maxSize);
								}
							}
						}
						else if (this.isFup)
						{
							MonoSingleton<NewMovement>.Instance.LaunchFromPointAtSpeed(base.transform.position, 60f);
						}
						else
						{
							MonoSingleton<NewMovement>.Instance.LaunchFromPoint(base.transform.position, (float)num3, this.maxSize);
							if (this.ultrabooster && num < 12f)
							{
								MonoSingleton<NewMovement>.Instance.LaunchFromPoint(base.transform.position, (float)num3, this.maxSize);
							}
						}
						if (this.damage <= 0)
						{
							MonoSingleton<NewMovement>.Instance.safeExplosionLaunchCooldown = 0.5f;
						}
					}
					if (this.damage > 0)
					{
						int num4 = this.damage;
						if (this.ultrabooster)
						{
							num4 = ((num < 3f) ? 35 : 50);
						}
						num4 = ((this.playerDamageOverride >= 0) ? this.playerDamageOverride : num4);
						MonoSingleton<NewMovement>.Instance.GetHurt(num4, true, (float)(this.enemy ? 1 : 0), true, false, 0.35f, false);
					}
				}
			}
			else if ((other.gameObject.layer == 10 || other.gameObject.layer == 11) && this.canHit != AffectedSubjects.PlayerOnly)
			{
				EnemyIdentifierIdentifier componentInParent = other.GetComponentInParent<EnemyIdentifierIdentifier>();
				if (componentInParent != null && componentInParent.eid != null)
				{
					Collider collider;
					if (!componentInParent.eid.dead && componentInParent.eid.TryGetComponent<Collider>(out collider))
					{
						int instanceID2 = collider.GetInstanceID();
						if (this.hitColliders.Add(instanceID2) && (this.HurtCooldownCollection == null || this.HurtCooldownCollection.TryHurtCheckEnemy(componentInParent.eid, true)))
						{
							if (componentInParent.eid.enemyType == EnemyType.Idol)
							{
								if (!Physics.Linecast(base.transform.position, collider.bounds.center, LayerMaskDefaults.Get(LMD.Environment)))
								{
									componentInParent.eid.hitter = this.hitterWeapon;
									componentInParent.eid.DeliverDamage(other.gameObject, Vector3.zero, position, 1f, false, 0f, this.sourceWeapon, false, true);
								}
							}
							else if (componentInParent.eid.enemyType == EnemyType.MaliciousFace && !componentInParent.eid.isGasolined)
							{
								Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.ineffectiveSound, position, Quaternion.identity);
							}
							else if ((!this.enemy || (componentInParent.eid.enemyType != EnemyType.HideousMass && componentInParent.eid.enemyType != EnemyType.Sisyphus)) && !this.toIgnore.Contains(componentInParent.eid.enemyType))
							{
								if (componentInParent.eid.enemyType == EnemyType.Gutterman && this.hitterWeapon == "heavypunch")
								{
									componentInParent.eid.hitter = "heavypunch";
								}
								else if (this.hitterWeapon == "lightningbolt")
								{
									componentInParent.eid.hitter = "lightningbolt";
								}
								else
								{
									componentInParent.eid.hitter = (this.friendlyFire ? "ffexplosion" : (this.enemy ? "enemy" : "explosion"));
								}
								if (!componentInParent.eid.hitterWeapons.Contains(this.hitterWeapon))
								{
									componentInParent.eid.hitterWeapons.Add(this.hitterWeapon);
								}
								Vector3 vector3 = normalized;
								if (componentInParent.eid.enemyType == EnemyType.Drone && this.damage == 0)
								{
									vector3 = Vector3.zero;
								}
								else if (vector3.y <= 0.5f)
								{
									vector3 = new Vector3(vector3.x, vector3.y + 0.5f, vector3.z);
								}
								else if (vector3.y < 1f)
								{
									vector3 = new Vector3(vector3.x, 1f, vector3.z);
								}
								float num5 = (float)this.damage / 10f * this.enemyDamageMultiplier;
								if (this.rocketExplosion && componentInParent.eid.enemyType == EnemyType.Cerberus)
								{
									num5 *= 1.5f;
								}
								Zombie zombie;
								if (componentInParent.eid.enemyType != EnemyType.Soldier || componentInParent.eid.isGasolined || this.unblockable || BlindEnemies.Blind || !componentInParent.eid.TryGetComponent<Zombie>(out zombie) || !zombie.grounded || !zombie.zp || zombie.zp.difficulty < 2)
								{
									if (this.electric)
									{
										componentInParent.eid.hitterAttributes.Add(HitterAttribute.Electricity);
									}
									componentInParent.eid.DeliverDamage(componentInParent.gameObject, vector3 * 50000f, position, num5, false, 0f, this.sourceWeapon, false, true);
									if (this.ignite)
									{
										if (componentInParent.eid.flammables != null && componentInParent.eid.flammables.Count > 0)
										{
											componentInParent.eid.StartBurning((float)(this.damage / 10));
										}
										else
										{
											Flammable componentInChildren = componentInParent.eid.GetComponentInChildren<Flammable>();
											if (componentInChildren != null)
											{
												componentInChildren.Burn((float)(this.damage / 10), false);
											}
										}
									}
								}
								else
								{
									componentInParent.eid.hitter = "blocked";
									if (zombie.zp.difficulty <= 3 || this.electric)
									{
										if (this.electric)
										{
											componentInParent.eid.hitterAttributes.Add(HitterAttribute.Electricity);
										}
										componentInParent.eid.DeliverDamage(other.gameObject, Vector3.zero, position, num5 * 0.25f, false, 0f, this.sourceWeapon, false, true);
									}
									zombie.zp.Block(base.transform.position);
								}
							}
						}
					}
					else if (componentInParent.eid.dead)
					{
						this.hitColliders.Add(instanceID);
						componentInParent.eid.hitter = (this.enemy ? "enemy" : "explosion");
						componentInParent.eid.DeliverDamage(other.gameObject, normalized * 5000f, position, (float)this.damage / 10f * this.enemyDamageMultiplier, false, 0f, this.sourceWeapon, false, true);
						Flammable flammable;
						if (this.ignite && componentInParent.TryGetComponent<Flammable>(out flammable))
						{
							Flammable componentInChildren2 = componentInParent.eid.GetComponentInChildren<Flammable>();
							if (componentInChildren2 != null)
							{
								componentInChildren2.Burn((float)(this.damage / 10), false);
							}
						}
					}
				}
			}
			else
			{
				if (SceneHelper.IsStaticEnvironment(other))
				{
					this.enviroGibs++;
					Vector3 vector4 = other.ClosestPoint(base.transform.position);
					Vector3 normalized2 = (vector4 - base.transform.position).normalized;
					float num6 = Vector3.Distance(base.transform.position, vector4);
					float num7 = 1f;
					if (this.enemyDamageMultiplier > 1f)
					{
						num7 = Mathf.Min(this.enemyDamageMultiplier, 2f);
					}
					MonoSingleton<SceneHelper>.Instance.CreateEnviroGibs(vector4 - normalized2, normalized2, 5f, Mathf.RoundToInt(Mathf.Lerp(10f, 2f, num6 / this.maxSize) * num7) / this.enviroGibs, Mathf.Lerp(2f, 0.5f, num6 / this.maxSize) * num7 / (float)this.enviroGibs);
				}
				Breakable breakable;
				Bleeder bleeder;
				Glass glass;
				Flammable flammable2;
				if (other.TryGetComponent<Breakable>(out breakable) && !breakable.unbreakable && !breakable.precisionOnly && (!breakable.playerOnly || !this.enemy) && !breakable.specialCaseOnly)
				{
					if (!breakable.accurateExplosionsOnly)
					{
						breakable.Break();
					}
					else
					{
						Vector3 vector5 = other.ClosestPoint(base.transform.position);
						if (!Physics.Raycast(vector5 + (vector5 - base.transform.position).normalized * 0.001f, base.transform.position - vector5, Vector3.Distance(base.transform.position, vector5), this.lmask, QueryTriggerInteraction.Ignore))
						{
							breakable.Break();
						}
					}
				}
				else if (other.TryGetComponent<Bleeder>(out bleeder))
				{
					bool flag = false;
					if (this.toIgnore.Count > 0 && bleeder.ignoreTypes.Length != 0)
					{
						foreach (EnemyType enemyType in bleeder.ignoreTypes)
						{
							for (int j = 0; j < this.toIgnore.Count; j++)
							{
								if (enemyType == this.toIgnore[j])
								{
									flag = true;
									break;
								}
							}
							if (flag)
							{
								break;
							}
						}
					}
					if (!flag)
					{
						bleeder.GetHit(position, GoreType.Head, true);
					}
				}
				else if (other.TryGetComponent<Glass>(out glass))
				{
					glass.Shatter();
				}
				else if (this.ignite && other.TryGetComponent<Flammable>(out flammable2) && (!this.enemy || !flammable2.playerOnly) && (this.enemy || !flammable2.enemyOnly))
				{
					flammable2.Burn(4f, false);
				}
			}
		}
		if (!other.gameObject.CompareTag("Player") || MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
		{
			Rigidbody component = other.GetComponent<Rigidbody>();
			bool flag2 = other.gameObject.layer == 14;
			Nail nail;
			if ((!component || !flag2 || !other.gameObject.CompareTag("Metal") || !other.TryGetComponent<Nail>(out nail) || nail.magnets.Count == 0) && component && (!flag2 || component.useGravity) && !other.gameObject.CompareTag("IgnorePushes"))
			{
				this.hitColliders.Add(instanceID);
				Vector3 vector6 = normalized * Mathf.Max(5f - num, 0f);
				vector6 = Vector3.Scale(vector6, new Vector3(7500f, 1f, 7500f));
				if (component.useGravity)
				{
					vector6 = new Vector3(vector6.x, 18750f, vector6.z);
				}
				if (other.gameObject.layer == 27 || other.gameObject.layer == 9)
				{
					vector6 = Vector3.ClampMagnitude(vector6, 5000f);
				}
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && other.gameObject == MonoSingleton<PlatformerMovement>.Instance.gameObject)
				{
					vector6 *= 30f;
				}
				component.AddForce(vector6);
			}
			if (flag2)
			{
				ThrownSword component2 = other.GetComponent<ThrownSword>();
				Projectile component3 = other.GetComponent<Projectile>();
				if (component2 != null)
				{
					component2.deflected = true;
				}
				if (component3 != null && !component3.ignoreExplosions)
				{
					component3.homingType = HomingType.None;
					other.transform.LookAt(position + normalized);
					component3.friendly = true;
					component3.target = null;
					component3.turnSpeed = 0f;
					component3.speed = Mathf.Max(component3.speed, 65f);
					if (component3.connectedBeams.Count > 0)
					{
						foreach (ContinuousBeam continuousBeam in component3.connectedBeams)
						{
							if (continuousBeam)
							{
								continuousBeam.enemy = false;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x04000B4F RID: 2895
	public static float globalSizeMulti = 1f;

	// Token: 0x04000B50 RID: 2896
	public HurtCooldownCollection HurtCooldownCollection;

	// Token: 0x04000B51 RID: 2897
	public GameObject sourceWeapon;

	// Token: 0x04000B52 RID: 2898
	public bool enemy;

	// Token: 0x04000B53 RID: 2899
	public bool harmless;

	// Token: 0x04000B54 RID: 2900
	public bool lowQuality;

	// Token: 0x04000B55 RID: 2901
	private CameraController cc;

	// Token: 0x04000B56 RID: 2902
	private Light light;

	// Token: 0x04000B57 RID: 2903
	private MeshRenderer mr;

	// Token: 0x04000B58 RID: 2904
	private Color materialColor;

	// Token: 0x04000B59 RID: 2905
	private Material originalMaterial;

	// Token: 0x04000B5A RID: 2906
	private TimeSince explosionTime;

	// Token: 0x04000B5B RID: 2907
	private bool whiteExplosion;

	// Token: 0x04000B5C RID: 2908
	private bool fading;

	// Token: 0x04000B5D RID: 2909
	public float speed;

	// Token: 0x04000B5E RID: 2910
	public float maxSize;

	// Token: 0x04000B5F RID: 2911
	private LayerMask lmask;

	// Token: 0x04000B60 RID: 2912
	public int damage;

	// Token: 0x04000B61 RID: 2913
	public float enemyDamageMultiplier;

	// Token: 0x04000B62 RID: 2914
	[HideInInspector]
	public int playerDamageOverride = -1;

	// Token: 0x04000B63 RID: 2915
	public GameObject explosionChunk;

	// Token: 0x04000B64 RID: 2916
	public bool ignite;

	// Token: 0x04000B65 RID: 2917
	public bool friendlyFire;

	// Token: 0x04000B66 RID: 2918
	public bool isFup;

	// Token: 0x04000B67 RID: 2919
	private HashSet<int> hitColliders = new HashSet<int>();

	// Token: 0x04000B68 RID: 2920
	public string hitterWeapon;

	// Token: 0x04000B69 RID: 2921
	public bool halved;

	// Token: 0x04000B6A RID: 2922
	private SphereCollider scol;

	// Token: 0x04000B6B RID: 2923
	public AffectedSubjects canHit;

	// Token: 0x04000B6C RID: 2924
	private bool hasHitPlayer;

	// Token: 0x04000B6D RID: 2925
	[HideInInspector]
	public EnemyIdentifier originEnemy;

	// Token: 0x04000B6E RID: 2926
	public bool rocketExplosion;

	// Token: 0x04000B6F RID: 2927
	public List<EnemyType> toIgnore;

	// Token: 0x04000B70 RID: 2928
	[HideInInspector]
	public EnemyIdentifier interruptedEnemy;

	// Token: 0x04000B71 RID: 2929
	[HideInInspector]
	public bool ultrabooster;

	// Token: 0x04000B72 RID: 2930
	public bool unblockable;

	// Token: 0x04000B73 RID: 2931
	public bool electric;

	// Token: 0x04000B74 RID: 2932
	private int enviroGibs;
}
