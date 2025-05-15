using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class Grenade : MonoBehaviour
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x0005465D File Offset: 0x0005285D
	public bool frozen
	{
		get
		{
			return MonoSingleton<WeaponCharges>.Instance && MonoSingleton<WeaponCharges>.Instance.rocketFrozen;
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00054678 File Offset: 0x00052878
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.col = base.GetComponent<CapsuleCollider>();
		if (!this.enemy)
		{
			this.CanCollideWithPlayer(false);
		}
		MonoSingleton<ObjectTracker>.Instance.AddGrenade(this);
		this.rocketRideMask = LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies);
		this.rocketRideMask |= 262144;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x000546DF File Offset: 0x000528DF
	private void Start()
	{
		if (this.rocket)
		{
			MonoSingleton<WeaponCharges>.Instance.rocketCount++;
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x000546FC File Offset: 0x000528FC
	private void OnDestroy()
	{
		if (base.gameObject.scene.isLoaded)
		{
			MonoSingleton<ObjectTracker>.Instance.RemoveGrenade(this);
			if (this.playerRiding)
			{
				this.PlayerRideEnd();
			}
		}
		if (this.rocket && MonoSingleton<WeaponCharges>.Instance)
		{
			MonoSingleton<WeaponCharges>.Instance.rocketCount--;
			MonoSingleton<WeaponCharges>.Instance.timeSinceIdleFrozen = 0f;
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00054774 File Offset: 0x00052974
	private void Update()
	{
		if (!this.rocket || this.rocketSpeed == 0f || !this.rb)
		{
			return;
		}
		if (MonoSingleton<OptionsManager>.Instance.paused)
		{
			return;
		}
		if (this.playerRiding)
		{
			if (MonoSingleton<InputManager>.Instance.InputSource.Jump.WasPerformedThisFrame)
			{
				this.PlayerRideEnd();
				MonoSingleton<NewMovement>.Instance.Jump();
				return;
			}
			if (MonoSingleton<InputManager>.Instance.InputSource.Slide.WasPerformedThisFrame)
			{
				this.PlayerRideEnd();
			}
		}
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000547FC File Offset: 0x000529FC
	private void FixedUpdate()
	{
		if (!this.rocket || this.rocketSpeed == 0f || !this.rb)
		{
			return;
		}
		if (this.frozen)
		{
			if (this.magnets.Count > 0)
			{
				this.ignoreEnemyType.Clear();
			}
			this.rideable = true;
			if (!this.rb.isKinematic)
			{
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
			}
			this.timeFrozen += Time.fixedDeltaTime;
			if (this.timeFrozen >= 1f && (!this.enemy || this.hasBeenRidden) && !this.levelledUp)
			{
				this.levelledUp = true;
				if (this.levelUpEffect)
				{
					this.levelUpEffect.SetActive(true);
				}
			}
		}
		else if (this.playerRiding)
		{
			if (NoWeaponCooldown.NoCooldown || MonoSingleton<UnderwaterController>.Instance.inWater || MonoSingleton<WeaponCharges>.Instance.infiniteRocketRide)
			{
				if (MonoSingleton<UnderwaterController>.Instance.inWater && this.downpull > 0f)
				{
					this.downpull = 0f;
				}
				this.rb.velocity = base.transform.forward * this.rocketSpeed * 0.65f;
			}
			else
			{
				this.rb.velocity = Vector3.Lerp(base.transform.forward * (this.rocketSpeed * 0.65f), Vector3.down * 100f, Mathf.Max(0f, this.downpull));
				this.downpull += Time.fixedDeltaTime / 4.5f * Mathf.Max(1f, 1f + this.rb.velocity.normalized.y);
			}
		}
		else if (!this.rb.isKinematic)
		{
			this.rb.velocity = base.transform.forward * this.rocketSpeed;
		}
		if (this.playerRiding)
		{
			MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.zero;
			Vector3 vector = MonoSingleton<NewMovement>.Instance.transform.position + MonoSingleton<NewMovement>.Instance.playerCollider.center;
			bool flag = false;
			Vector3 vector2 = Vector3.positiveInfinity;
			Collider collider = null;
			if (!Physics.CheckCapsule(vector + Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), vector - Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), 0.5f, this.rocketRideMask, QueryTriggerInteraction.Ignore))
			{
				RaycastHit[] array = Physics.CapsuleCastAll(vector + Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), vector - Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), 0.499f, this.rb.velocity.normalized, this.rb.velocity.magnitude * Time.fixedDeltaTime, this.rocketRideMask, QueryTriggerInteraction.Ignore);
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i].collider.isTrigger && array[i].collider.gameObject.layer != 12 && array[i].collider.gameObject.layer != 14 && (!array[i].collider.attachedRigidbody || array[i].collider.attachedRigidbody != this.rb))
					{
						Vector3 vector3 = MonoSingleton<NewMovement>.Instance.playerCollider.ClosestPoint(array[i].point);
						Vector3 vector4 = array[i].point - (array[i].point - vector3).normalized * Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, vector3);
						if (Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, vector4) < Vector3.Distance(MonoSingleton<NewMovement>.Instance.transform.position, vector2))
						{
							new GameObject().transform.position = base.transform.position;
							vector2 = vector4;
							collider = array[i].collider;
						}
						flag = true;
					}
					else
					{
						bool isTrigger = array[i].collider.isTrigger;
						int layer = array[i].collider.gameObject.layer;
						int layer2 = array[i].collider.gameObject.layer;
						if (array[i].collider.attachedRigidbody)
						{
							array[i].collider.attachedRigidbody == this.rb;
						}
					}
				}
			}
			else
			{
				vector2 = MonoSingleton<NewMovement>.Instance.transform.position;
				collider = Physics.OverlapCapsule(vector + Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), vector - Vector3.up * (MonoSingleton<NewMovement>.Instance.playerCollider.height / 2f), 0.5f, this.rocketRideMask, QueryTriggerInteraction.Ignore)[0];
				flag = true;
			}
			if (flag)
			{
				this.PlayerRideEnd();
				MonoSingleton<NewMovement>.Instance.transform.position = vector2;
				base.transform.position = MonoSingleton<NewMovement>.Instance.transform.position;
				this.Collision(collider);
			}
		}
		else
		{
			float num = Vector3.Distance(MonoSingleton<NewMovement>.Instance.gc.transform.position, base.transform.position + base.transform.forward);
			if (num < 2.25f && (MonoSingleton<NewMovement>.Instance.rb.velocity.y < 0f || this.hooked) && !MonoSingleton<NewMovement>.Instance.gc.onGround && !MonoSingleton<NewMovement>.Instance.dead && this.rideable && (!this.enemy || MonoSingleton<NewMovement>.Instance.gc.heavyFall))
			{
				if (!MonoSingleton<NewMovement>.Instance.ridingRocket && !this.playerInRidingRange)
				{
					this.PlayerRideStart();
				}
				this.playerInRidingRange = true;
			}
			else if (this.playerInRidingRange && (num > 3f || MonoSingleton<NewMovement>.Instance.gc.onGround || (MonoSingleton<NewMovement>.Instance.rb.velocity.y > 0f && !this.hooked)))
			{
				this.playerInRidingRange = false;
			}
		}
		if (this.freezeEffect.activeSelf != this.frozen)
		{
			this.freezeEffect.SetActive(this.frozen);
		}
		if (this.magnets.Count > 0)
		{
			int j = this.magnets.Count - 1;
			while (j >= 0)
			{
				if (this.magnets[j] == null)
				{
					this.magnets.RemoveAt(j);
					j--;
				}
				else
				{
					if (!this.frozen)
					{
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(this.magnets[j].transform.position - base.transform.position), Time.fixedDeltaTime * 180f);
						break;
					}
					if (this.latestEnemyMagnet && this.latestEnemyMagnet.gameObject.activeInHierarchy && !Physics.Raycast(base.transform.position, this.latestEnemyMagnet.transform.position - base.transform.position, Vector3.Distance(this.latestEnemyMagnet.transform.position, base.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
					{
						base.transform.LookAt(this.latestEnemyMagnet.transform.position);
						break;
					}
					base.transform.LookAt(this.magnets[j].transform.position);
					break;
				}
			}
		}
		else if (this.latestEnemyMagnet && this.latestEnemyMagnet.gameObject.activeInHierarchy && !Physics.Raycast(base.transform.position, this.latestEnemyMagnet.transform.position - base.transform.position, Vector3.Distance(this.latestEnemyMagnet.transform.position, base.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
		{
			base.transform.LookAt(this.latestEnemyMagnet.transform.position);
		}
		if (this.proximityTarget != null && this.magnets.Count == 0 && !this.frozen && !this.playerRiding && !this.selfExploding && Vector3.Distance(this.proximityTarget.position, base.transform.position) < Vector3.Distance(this.proximityTarget.PredictTargetPosition(Time.fixedDeltaTime, false), base.transform.position + this.rb.velocity * Time.fixedDeltaTime))
		{
			this.selfExploding = true;
			this.rideable = true;
			Object.Instantiate<GameObject>(this.proximityWindup, this.col.bounds.center, Quaternion.identity);
			this.rb.isKinematic = true;
			base.Invoke("ProximityExplosion", 0.5f);
		}
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00055260 File Offset: 0x00053460
	private void LateUpdate()
	{
		if (this.playerRiding)
		{
			if (Vector3.Distance(base.transform.position, MonoSingleton<NewMovement>.Instance.transform.position) > 5f + this.rb.velocity.magnitude * Time.deltaTime)
			{
				this.PlayerRideEnd();
				return;
			}
			Vector2 vector = MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>();
			base.transform.Rotate(vector.y * Time.deltaTime * 165f, vector.x * Time.deltaTime * 165f, 0f, Space.Self);
			Vector3 vector2;
			if (Physics.Raycast(base.transform.position + base.transform.forward, base.transform.up, 4f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position + base.transform.forward, Vector3.up, out raycastHit, 2f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					vector2 = base.transform.position + base.transform.forward - Vector3.up * raycastHit.distance;
				}
				else
				{
					vector2 = base.transform.position + base.transform.forward;
				}
			}
			else
			{
				vector2 = base.transform.position + base.transform.up * 2f + base.transform.forward;
			}
			MonoSingleton<NewMovement>.Instance.transform.position = vector2;
			MonoSingleton<NewMovement>.Instance.rb.position = vector2;
			MonoSingleton<CameraController>.Instance.CameraShake(0.1f);
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00055439 File Offset: 0x00053639
	private void OnCollisionEnter(Collision collision)
	{
		this.Collision(collision.collider);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00055447 File Offset: 0x00053647
	private void OnTriggerEnter(Collider other)
	{
		if (this.rocket && this.frozen && (other.gameObject.layer == 10 || other.gameObject.layer == 11) && !other.isTrigger)
		{
			this.Collision(other);
		}
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00055488 File Offset: 0x00053688
	public void Collision(Collider other)
	{
		if (!this.exploded && (this.enemy || !other.CompareTag("Player")) && other.gameObject.layer != 14 && other.gameObject.layer != 20)
		{
			bool flag = false;
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if ((other.gameObject.layer == 11 || other.gameObject.layer == 10) && (other.attachedRigidbody ? other.attachedRigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) : other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier)) && enemyIdentifierIdentifier.eid)
			{
				if (enemyIdentifierIdentifier.eid.enemyType == EnemyType.MaliciousFace && !enemyIdentifierIdentifier.eid.isGasolined)
				{
					flag = true;
				}
				else
				{
					if (this.ignoreEnemyType.Count > 0 && this.ignoreEnemyType.Contains(enemyIdentifierIdentifier.eid.enemyType))
					{
						return;
					}
					if (enemyIdentifierIdentifier.eid.dead)
					{
						Physics.IgnoreCollision(this.col, other, true);
						return;
					}
				}
			}
			if (!flag && other.gameObject.CompareTag("Armor"))
			{
				flag = true;
			}
			if (flag)
			{
				this.rb.constraints = RigidbodyConstraints.None;
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position - base.transform.forward, base.transform.forward, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment)))
				{
					Vector3 velocity = this.rb.velocity;
					this.rb.velocity = Vector3.zero;
					this.rb.AddForce(Vector3.Reflect(velocity.normalized, raycastHit.normal).normalized * velocity.magnitude * 2f, ForceMode.VelocityChange);
					base.transform.forward = Vector3.Reflect(velocity.normalized, raycastHit.normal).normalized;
					this.rb.AddTorque(Random.insideUnitCircle.normalized * (float)Random.Range(0, 250));
				}
				Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.ineffectiveSound, base.transform.position, Quaternion.identity).GetComponent<AudioSource>().volume = 0.75f;
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (this.rocket)
			{
				if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
				{
					EnemyIdentifierIdentifier component = other.GetComponent<EnemyIdentifierIdentifier>();
					if (component && component.eid)
					{
						if (this.levelledUp)
						{
							flag4 = true;
						}
						else if (!component.eid.dead && !component.eid.flying && ((component.eid.gce && !component.eid.gce.onGround) || component.eid.timeSinceSpawned <= 0.15f))
						{
							flag4 = true;
						}
						if (component.eid.stuckMagnets.Count > 0)
						{
							foreach (Magnet magnet in component.eid.stuckMagnets)
							{
								if (!(magnet == null))
								{
									magnet.DamageMagnet((float)(flag4 ? 2 : 1));
								}
							}
						}
						if (component.eid == this.originEnemy && !component.eid.blessed)
						{
							if (this.hasBeenRidden && !this.frozen && this.originEnemy.enemyType == EnemyType.Guttertank)
							{
								this.originEnemy.Explode(true);
								MonoSingleton<StyleHUD>.Instance.AddPoints(300, "ultrakill.roundtrip", null, component.eid, -1, "", "");
							}
							else
							{
								MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.rocketreturn", null, component.eid, -1, "", "");
							}
						}
					}
					MonoSingleton<TimeController>.Instance.HitStop(0.05f);
				}
				else if (!this.enemy || !other.gameObject.CompareTag("Player"))
				{
					flag2 = true;
				}
			}
			else if (!LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
			{
				MonoSingleton<TimeController>.Instance.HitStop(0.05f);
			}
			this.Explode(flag3, flag2, flag4, 1f, false, null, false);
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00055918 File Offset: 0x00053B18
	private void ProximityExplosion()
	{
		this.Explode(true, false, false, 1f, false, null, false);
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0005592C File Offset: 0x00053B2C
	public void Explode(bool big = false, bool harmless = false, bool super = false, float sizeMultiplier = 1f, bool ultrabooster = false, GameObject exploderWeapon = null, bool fup = false)
	{
		if (!this.exploded)
		{
			this.exploded = true;
			int num = (super ? 7 : 3);
			if (MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(base.transform.position, num))
			{
				harmless = false;
			}
			GameObject gameObject;
			if (harmless)
			{
				gameObject = Object.Instantiate<GameObject>(this.harmlessExplosion, base.transform.position, Quaternion.identity);
			}
			else if (super)
			{
				gameObject = Object.Instantiate<GameObject>(this.superExplosion, base.transform.position, Quaternion.identity);
			}
			else
			{
				gameObject = Object.Instantiate<GameObject>(this.explosion, base.transform.position, Quaternion.identity);
			}
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				explosion.sourceWeapon = exploderWeapon ?? this.sourceWeapon;
				explosion.hitterWeapon = this.hitterWeapon;
				explosion.isFup = fup;
				if (this.enemy)
				{
					explosion.enemy = true;
				}
				if (this.ignoreEnemyType.Count > 0)
				{
					explosion.toIgnore = this.ignoreEnemyType;
				}
				if (this.rocket && super && big)
				{
					explosion.maxSize *= 2.5f;
					explosion.speed *= 2.5f;
				}
				else if (big || (this.rocket && this.frozen))
				{
					explosion.maxSize *= 1.5f;
					explosion.speed *= 1.5f;
				}
				if (this.totalDamageMultiplier != 1f)
				{
					explosion.damage = (int)((float)explosion.damage * this.totalDamageMultiplier);
				}
				explosion.maxSize *= sizeMultiplier;
				explosion.speed *= sizeMultiplier;
				if (this.originEnemy)
				{
					explosion.originEnemy = this.originEnemy;
				}
				if (ultrabooster)
				{
					explosion.ultrabooster = true;
				}
				if (this.rocket && explosion.damage != 0)
				{
					explosion.rocketExplosion = true;
				}
			}
			gameObject.transform.localScale *= sizeMultiplier;
			Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00055B54 File Offset: 0x00053D54
	public void PlayerRideStart()
	{
		this.CanCollideWithPlayer(false);
		if (this.enemy && this.proximityTarget != null)
		{
			base.CancelInvoke("ProximityExplosion");
			this.proximityTarget = null;
			this.rb.isKinematic = false;
		}
		this.ignoreEnemyType.Clear();
		this.playerRiding = true;
		MonoSingleton<NewMovement>.Instance.ridingRocket = this;
		MonoSingleton<NewMovement>.Instance.gc.heavyFall = false;
		MonoSingleton<NewMovement>.Instance.gc.ForceOff();
		MonoSingleton<NewMovement>.Instance.slopeCheck.ForceOff();
		Object.Instantiate<GameObject>(this.playerRideSound);
		if (!this.hasBeenRidden && !this.enemy)
		{
			MonoSingleton<NewMovement>.Instance.rocketRides++;
			this.hasBeenRidden = true;
			if (MonoSingleton<NewMovement>.Instance.rocketRides > 3)
			{
				this.downpull += 0.25f * (float)(MonoSingleton<NewMovement>.Instance.rocketRides - 3);
				return;
			}
		}
		else if (!this.hasBeenRidden)
		{
			this.hasBeenRidden = true;
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00055C53 File Offset: 0x00053E53
	public void PlayerRideEnd()
	{
		this.playerRiding = false;
		MonoSingleton<NewMovement>.Instance.ridingRocket = null;
		MonoSingleton<NewMovement>.Instance.gc.StopForceOff();
		MonoSingleton<NewMovement>.Instance.slopeCheck.StopForceOff();
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00055C85 File Offset: 0x00053E85
	public void CanCollideWithPlayer(bool can = true)
	{
		Physics.IgnoreCollision(this.col, MonoSingleton<NewMovement>.Instance.playerCollider, !can);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00055CA0 File Offset: 0x00053EA0
	public void GrenadeBeam(Vector3 targetPoint, GameObject newSourceWeapon = null)
	{
		if (this.exploded)
		{
			return;
		}
		RevolverBeam revolverBeam = Object.Instantiate<RevolverBeam>(this.grenadeBeam, base.transform.position, Quaternion.LookRotation(targetPoint - base.transform.position));
		revolverBeam.sourceWeapon = ((newSourceWeapon != null) ? newSourceWeapon : this.sourceWeapon);
		if (this.levelledUp)
		{
			revolverBeam.hitParticle = this.superExplosion;
		}
		this.exploded = true;
		MonoSingleton<StainVoxelManager>.Instance.TryIgniteAt(targetPoint, 3);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000FD0 RID: 4048
	public string hitterWeapon;

	// Token: 0x04000FD1 RID: 4049
	public GameObject sourceWeapon;

	// Token: 0x04000FD2 RID: 4050
	public GameObject explosion;

	// Token: 0x04000FD3 RID: 4051
	public GameObject harmlessExplosion;

	// Token: 0x04000FD4 RID: 4052
	public GameObject superExplosion;

	// Token: 0x04000FD5 RID: 4053
	[SerializeField]
	private RevolverBeam grenadeBeam;

	// Token: 0x04000FD6 RID: 4054
	private bool exploded;

	// Token: 0x04000FD7 RID: 4055
	public bool enemy;

	// Token: 0x04000FD8 RID: 4056
	[HideInInspector]
	public EnemyIdentifier originEnemy;

	// Token: 0x04000FD9 RID: 4057
	public float totalDamageMultiplier = 1f;

	// Token: 0x04000FDA RID: 4058
	public bool rocket;

	// Token: 0x04000FDB RID: 4059
	[HideInInspector]
	public Rigidbody rb;

	// Token: 0x04000FDC RID: 4060
	[HideInInspector]
	public List<Magnet> magnets = new List<Magnet>();

	// Token: 0x04000FDD RID: 4061
	[HideInInspector]
	public Magnet latestEnemyMagnet;

	// Token: 0x04000FDE RID: 4062
	public float rocketSpeed;

	// Token: 0x04000FDF RID: 4063
	[SerializeField]
	private GameObject freezeEffect;

	// Token: 0x04000FE0 RID: 4064
	private CapsuleCollider col;

	// Token: 0x04000FE1 RID: 4065
	public bool playerRiding;

	// Token: 0x04000FE2 RID: 4066
	private bool playerInRidingRange = true;

	// Token: 0x04000FE3 RID: 4067
	private float downpull = -0.5f;

	// Token: 0x04000FE4 RID: 4068
	public GameObject playerRideSound;

	// Token: 0x04000FE5 RID: 4069
	[HideInInspector]
	public bool rideable;

	// Token: 0x04000FE6 RID: 4070
	[HideInInspector]
	public bool hooked;

	// Token: 0x04000FE7 RID: 4071
	private bool hasBeenRidden;

	// Token: 0x04000FE8 RID: 4072
	private LayerMask rocketRideMask;

	// Token: 0x04000FE9 RID: 4073
	public EnemyTarget proximityTarget;

	// Token: 0x04000FEA RID: 4074
	public GameObject proximityWindup;

	// Token: 0x04000FEB RID: 4075
	private bool selfExploding;

	// Token: 0x04000FEC RID: 4076
	[HideInInspector]
	public bool levelledUp;

	// Token: 0x04000FED RID: 4077
	[HideInInspector]
	public float timeFrozen;

	// Token: 0x04000FEE RID: 4078
	[SerializeField]
	private GameObject levelUpEffect;

	// Token: 0x04000FEF RID: 4079
	public List<EnemyType> ignoreEnemyType = new List<EnemyType>();
}
