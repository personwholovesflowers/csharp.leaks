using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020004D6 RID: 1238
public class ZombieProjectiles : MonoBehaviour
{
	// Token: 0x06001C6D RID: 7277 RVA: 0x000EDE02 File Offset: 0x000EC002
	private void Awake()
	{
		this.zmb = base.GetComponent<Zombie>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.anim = base.GetComponent<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000EDE40 File Offset: 0x000EC040
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.player = MonoSingleton<PlayerTracker>.Instance.GetPlayer().gameObject;
		this.camObj = MonoSingleton<PlayerTracker>.Instance.GetTarget().gameObject;
		if (this.aimer)
		{
			this.aimerDefaultRotation = Quaternion.Inverse(base.transform.rotation) * this.aimer.rotation;
		}
		this.nmp = new NavMeshPath();
		if (this.hasMelee && (this.swingChecks == null || this.swingChecks.Length == 0))
		{
			this.swingChecks = base.GetComponentsInChildren<SwingCheck2>();
		}
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.origWP = this.eid.weakPoint;
		this.spawnPos = base.transform.position;
		if (this.alwaysStationary)
		{
			this.stationary = true;
		}
		if (this.stationary || this.smallRay)
		{
			this.raySize = 0.25f;
		}
		if (this.difficulty >= 3)
		{
			this.coolDownReduce = 1f;
		}
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x000EDF81 File Offset: 0x000EC181
	private void Start()
	{
		this.SetValues();
		if (!this.stationary && this.wanderer && this.eid.target != null)
		{
			base.Invoke("Wander", 0.5f);
		}
		this.SlowUpdate();
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000EDFBC File Offset: 0x000EC1BC
	private void OnEnable()
	{
		this.SetValues();
		if (!this.musicRequested && this.playerSpotted && this.zmb && !this.eid.IgnorePlayer)
		{
			this.musicRequested = true;
			this.zmb.musicRequested = true;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance != null)
			{
				instance.PlayBattleMusic();
			}
		}
		if (this.hasMelee)
		{
			this.MeleeDamageEnd();
		}
		if (this.tr != null)
		{
			this.tr.emitting = false;
		}
		if (this.currentDecProjectile != null)
		{
			Object.Destroy(this.currentDecProjectile);
			this.eid.weakPoint = this.origWP;
		}
		this.swinging = false;
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x000EE07C File Offset: 0x000EC27C
	private void OnDisable()
	{
		if (this.musicRequested && !this.eid.IgnorePlayer && !this.zmb.limp)
		{
			this.musicRequested = false;
			this.zmb.musicRequested = false;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance != null)
			{
				instance.PlayCleanMusic();
			}
		}
		this.coolDown = Random.Range(1f, 2.5f) - this.coolDownReduce;
	}

	// Token: 0x06001C72 RID: 7282 RVA: 0x000EE0F0 File Offset: 0x000EC2F0
	private void SlowUpdate()
	{
		if (base.gameObject.activeInHierarchy)
		{
			if (this.zmb.grounded && this.nma && !this.zmb.limp && this.eid.target != null && !this.swinging)
			{
				Vector3 vector = this.eid.target.position - base.transform.position;
				Vector3 normalized = (this.eid.target.headPosition - this.head.transform.position).normalized;
				float num = Vector3.Distance(this.eid.target.position, base.transform.position);
				if (this.afraid && !this.swinging && num < 15f && this.nma.enabled)
				{
					this.nma.updateRotation = true;
					this.targetPosition = new Vector3(base.transform.position.x + vector.normalized.x * -10f, base.transform.position.y, base.transform.position.z + vector.normalized.z * -10f);
					if (this.nma.enabled && this.nma.isOnNavMesh)
					{
						if (NavMesh.SamplePosition(this.targetPosition, out this.hit, 1f, this.nma.areaMask))
						{
							this.SetDestination(this.targetPosition);
						}
						else if (NavMesh.FindClosestEdge(this.targetPosition, out this.hit, this.nma.areaMask))
						{
							this.targetPosition = this.hit.position;
							this.SetDestination(this.targetPosition);
						}
					}
					if (this.nma.velocity.magnitude < 1f)
					{
						this.lengthOfStop += 0.5f;
					}
					else
					{
						this.lengthOfStop = 0f;
					}
				}
				if (num > 15f || this.lengthOfStop > 0.75f || !this.afraid)
				{
					this.lengthOfStop = 0f;
					if (this.playerSpotted && (!this.chaser || Vector3.Distance(base.transform.position, this.eid.target.position) < 3f || this.coolDown == 0f) && (Vector3.Distance(base.transform.position, this.eid.target.position) < 30f || (Vector3.Distance(base.transform.position, this.eid.target.position) < 60f && this.coolDown == 0f) || this.stationary || (this.nmp.status != NavMeshPathStatus.PathComplete && (this.nmp.corners.Length == 0 || Vector3.Distance(base.transform.position, this.nmp.corners[this.nmp.corners.Length - 1]) < 3f))) && !Physics.Raycast(this.head.transform.position, normalized, out this.bhit, Vector3.Distance(this.eid.target.headPosition, this.head.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.seekingPlayer = false;
						if (!this.wanderer)
						{
							this.SetDestination(base.transform.position);
						}
						else if (this.wanderer && !this.chaser && this.coolDown <= 0f)
						{
							this.SetDestination(base.transform.position);
						}
						if (this.hasMelee && Vector3.Distance(base.transform.position, this.eid.target.position) <= 3f)
						{
							this.Melee();
						}
						else if (this.coolDown <= 0f && (!this.nma.enabled || this.nma.velocity.magnitude <= 2.5f))
						{
							this.Swing();
						}
						else if (this.wanderer && this.coolDown > 0f && this.nma.velocity.magnitude < 1f)
						{
							this.Wander();
						}
					}
					else if (!this.stationary && this.nma.enabled)
					{
						if (this.chaser)
						{
							if (this.nma == null)
							{
								this.nma = this.zmb.nma;
							}
							if (this.zmb.grounded && this.nma != null && this.nma.enabled && this.nma.isOnNavMesh && this.eid.target != null)
							{
								RaycastHit raycastHit;
								if (Physics.Raycast(this.eid.target.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
								{
									this.SetDestination(raycastHit.point);
								}
								else
								{
									this.SetDestination(this.eid.target.position);
								}
							}
						}
						else if (this.nma && this.nma.enabled && this.nma.isOnNavMesh)
						{
							this.seekingPlayer = true;
							this.nma.updateRotation = true;
							if (Physics.Raycast(this.eid.target.position + Vector3.up * 0.1f, Vector3.down, out this.rhit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
							{
								this.SetDestination(this.rhit.point);
							}
							else
							{
								this.SetDestination(this.eid.target.position);
							}
						}
					}
				}
			}
			if (this.stationary && !this.alwaysStationary && Vector3.Distance(base.transform.position, this.spawnPos) > 5f)
			{
				this.stationary = false;
			}
		}
		if (this.eid.dead)
		{
			return;
		}
		if (this.chaser || this.eid.enemyType == EnemyType.Soldier)
		{
			base.Invoke("SlowUpdate", 0.1f);
			return;
		}
		base.Invoke("SlowUpdate", 0.5f);
	}

	// Token: 0x06001C73 RID: 7283 RVA: 0x000EE7E8 File Offset: 0x000EC9E8
	private void Update()
	{
		if (!this.zmb.grounded || this.zmb.limp)
		{
			return;
		}
		if (this.coolDown > 0f)
		{
			this.coolDown = Mathf.MoveTowards(this.coolDown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (!this.playerSpotted && this.eid.target != null)
		{
			Vector3 normalized = (this.eid.target.headPosition - this.head.transform.position).normalized;
			if (!Physics.Raycast(this.head.transform.position, normalized, out this.rhit, Vector3.Distance(this.eid.target.headPosition, this.head.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.seekingPlayer = false;
				this.playerSpotted = true;
				this.coolDown = (float)Random.Range(1, 2) - this.coolDownReduce / 2f;
				if (this.eid.target.isPlayer && !this.musicRequested)
				{
					this.musicRequested = true;
					this.zmb.musicRequested = true;
					MusicManager instance = MonoSingleton<MusicManager>.Instance;
					if (instance != null)
					{
						instance.PlayBattleMusic();
					}
				}
			}
		}
		if (this.eid.target == null)
		{
			if (!this.nma.enabled || this.nma.velocity.magnitude <= 2.5f)
			{
				this.anim.SetBool("Running", false);
				this.nma.updateRotation = false;
			}
			return;
		}
		if ((!this.nma.enabled || this.nma.velocity.magnitude <= 2.5f) && this.playerSpotted && !this.seekingPlayer && (!this.wanderer || !this.swinging || this.chaser))
		{
			this.anim.SetBool("Running", false);
			this.nma.updateRotation = false;
			base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
			return;
		}
		if (this.nma.enabled && this.nma.velocity.magnitude > 2.5f)
		{
			this.anim.SetBool("Running", true);
			this.nma.updateRotation = true;
			return;
		}
		if ((!this.nma.enabled || this.nma.velocity.magnitude <= 2.5f) && this.playerSpotted && !this.seekingPlayer && this.wanderer && this.swinging)
		{
			this.anim.SetBool("Running", false);
			this.nma.updateRotation = false;
			if (this.difficulty >= 2)
			{
				Vector3 vector = new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z);
				Quaternion quaternion = Quaternion.LookRotation((vector - base.transform.position).normalized);
				if (this.difficulty == 2)
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, Time.deltaTime * 3.5f * this.eid.totalSpeedModifier);
					return;
				}
				if (this.difficulty == 3)
				{
					base.transform.LookAt(vector);
					return;
				}
				if (this.difficulty > 3)
				{
					base.transform.LookAt(vector);
				}
			}
		}
	}

	// Token: 0x06001C74 RID: 7284 RVA: 0x000EEBE8 File Offset: 0x000ECDE8
	private void LateUpdate()
	{
		if (this.aimer != null && this.aiming && this.eid.target != null)
		{
			Vector3 vector = this.eid.target.headPosition;
			if (this.predictionLerping)
			{
				this.predictionLerp = Mathf.MoveTowards(this.predictionLerp, 1f, Time.deltaTime * 0.75f * this.anim.speed * this.eid.totalSpeedModifier);
				vector = Vector3.Lerp(vector, this.predictedPosition, this.predictionLerp);
			}
			Quaternion quaternion = Quaternion.LookRotation((vector - this.aimer.position).normalized);
			Quaternion quaternion2 = Quaternion.Inverse(base.transform.rotation * this.aimerDefaultRotation) * this.aimer.rotation;
			this.aimer.rotation = quaternion * quaternion2;
			if (this.aimEase < 1f)
			{
				this.aimEase = Mathf.MoveTowards(this.aimEase, 1f, Time.deltaTime * (20f - this.aimEase * 20f) * this.eid.totalSpeedModifier);
			}
			this.aimer.rotation = Quaternion.Slerp(this.origRotation, quaternion, this.aimEase);
		}
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x000EED48 File Offset: 0x000ECF48
	private void FixedUpdate()
	{
		if (this.moveForward)
		{
			float num = this.forwardSpeed * this.anim.speed * this.eid.totalSpeedModifier;
			this.forwardSpeed /= 1f + Time.fixedDeltaTime * this.forwardSpeed / 3f;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward * 2.5f, Vector3.down, out raycastHit, (this.eid.target == null) ? 11f : Mathf.Max(11f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore) && Vector3.Dot(base.transform.up, raycastHit.normal) > 0.25f)
			{
				this.rb.velocity = new Vector3(base.transform.forward.x * num, Mathf.Min(0f, this.rb.velocity.y), base.transform.forward.z * num);
				return;
			}
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
	}

	// Token: 0x06001C76 RID: 7286 RVA: 0x000EEECE File Offset: 0x000ED0CE
	public void MoveForward(float speed)
	{
		this.forwardSpeed = speed * 10f;
		if (this.nma)
		{
			this.nma.enabled = false;
		}
		this.moveForward = true;
		this.rb.isKinematic = false;
	}

	// Token: 0x06001C77 RID: 7287 RVA: 0x000EEF09 File Offset: 0x000ED109
	private void StopMoveForward()
	{
		this.moveForward = false;
		if (this.zmb.grounded)
		{
			if (this.nma)
			{
				this.nma.enabled = true;
			}
			this.rb.isKinematic = true;
		}
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x000EEF44 File Offset: 0x000ED144
	private void SetDestination(Vector3 position)
	{
		if (!this.nma || !this.nma.isOnNavMesh)
		{
			return;
		}
		NavMesh.CalculatePath(base.transform.position, position, this.nma.areaMask, this.nmp);
		this.nma.SetPath(this.nmp);
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x000EEFA4 File Offset: 0x000ED1A4
	public void Melee()
	{
		this.swinging = true;
		this.seekingPlayer = false;
		this.nma.updateRotation = false;
		base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		this.nma.enabled = false;
		if (this.tr == null)
		{
			this.tr = base.GetComponentInChildren<TrailRenderer>();
		}
		this.tr.GetComponent<AudioSource>().Play();
		this.anim.SetTrigger("Melee");
	}

	// Token: 0x06001C7A RID: 7290 RVA: 0x000EF05B File Offset: 0x000ED25B
	public void MeleePrep()
	{
		this.zmb.ParryableCheck();
	}

	// Token: 0x06001C7B RID: 7291 RVA: 0x000EF068 File Offset: 0x000ED268
	public void MeleeDamageStart()
	{
		if (this.tr == null)
		{
			this.tr = base.GetComponentInChildren<TrailRenderer>();
		}
		if (this.tr != null)
		{
			this.tr.enabled = true;
			this.tr.emitting = true;
		}
		SwingCheck2[] array = this.swingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStart();
		}
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x000EF0D4 File Offset: 0x000ED2D4
	public void MeleeDamageEnd()
	{
		if (this.tr != null)
		{
			this.tr.emitting = false;
		}
		foreach (SwingCheck2 swingCheck in this.swingChecks)
		{
			if (swingCheck)
			{
				swingCheck.DamageStop();
			}
		}
		this.zmb.attacking = false;
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x000EF130 File Offset: 0x000ED330
	public void Swing()
	{
		this.swinging = true;
		this.seekingPlayer = false;
		this.nma.updateRotation = false;
		base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		this.nma.enabled = false;
		this.currentProjectile = null;
		this.previousProjectile = null;
		if (this.difficulty >= 4 && this.eid.enemyType == EnemyType.Schism)
		{
			this.aiming = true;
			this.predictionLerp = 0f;
			this.predictionLerping = false;
			this.anim.SetFloat("AttackType", 0f);
		}
		else if (this.eid.target.position.y - 12f > base.transform.position.y || this.eid.target.position.y + 5f < base.transform.position.y)
		{
			this.anim.SetFloat("AttackType", 1f);
		}
		else
		{
			this.anim.SetFloat("AttackType", (float)((Random.Range(0f, 1f) > 0.66f) ? 1 : 0));
		}
		if (!this.stationary && this.zmb.grounded && this.eid.enemyType == EnemyType.Soldier && this.difficulty >= 4)
		{
			this.MoveForward(25f);
			this.anim.Play("RollShoot", -1, 0f);
		}
		else
		{
			this.anim.SetTrigger("Swing");
		}
		this.coolDown = 99f;
	}

	// Token: 0x06001C7E RID: 7294 RVA: 0x000EF310 File Offset: 0x000ED510
	public void SwingEnd()
	{
		this.swinging = false;
		this.aiming = false;
		if (this.zmb.grounded)
		{
			this.nma.enabled = true;
		}
		this.coolDown = Random.Range(1f, 2.5f) - this.coolDownReduce;
		if (this.wanderer)
		{
			if (this.difficulty >= 4 && this.eid.enemyType == EnemyType.Soldier && Random.Range(0f, 1f) > 0.66f)
			{
				this.chaser = true;
				this.coolDown = 1f;
			}
			else
			{
				this.chaser = false;
				this.Wander();
				this.coolDown = Mathf.Max(Random.Range(0.5f, 2f) - this.coolDownReduce, 0.5f);
			}
		}
		if (this.blocking)
		{
			this.coolDown = 0f;
		}
		this.blocking = false;
		this.moveForward = false;
		if (this.tr != null)
		{
			this.tr.enabled = false;
		}
	}

	// Token: 0x06001C7F RID: 7295 RVA: 0x000EF418 File Offset: 0x000ED618
	public void SpawnProjectile()
	{
		if (!this.swinging)
		{
			return;
		}
		this.currentDecProjectile = Object.Instantiate<GameObject>(this.decProjectile, this.decProjectileSpawner.transform.position, this.decProjectileSpawner.transform.rotation);
		this.currentDecProjectile.transform.SetParent(this.decProjectileSpawner.transform, true);
		this.currentDecProjectile.GetComponentInChildren<Breakable>().interruptEnemy = this.eid;
		this.eid.weakPoint = this.currentDecProjectile;
	}

	// Token: 0x06001C80 RID: 7296 RVA: 0x000EF4A4 File Offset: 0x000ED6A4
	public void DamageStart()
	{
		if (!this.hasMelee)
		{
			if (this.tr == null)
			{
				this.tr = base.GetComponentInChildren<TrailRenderer>();
			}
			if (this.tr != null)
			{
				this.tr.enabled = true;
			}
		}
		this.zmb.ParryableCheck();
		if (this.aimer != null && (this.eid.enemyType != EnemyType.Schism || this.difficulty >= 4))
		{
			this.origRotation = this.aimer.rotation;
			this.aiming = true;
		}
	}

	// Token: 0x06001C81 RID: 7297 RVA: 0x000EF538 File Offset: 0x000ED738
	public void ThrowProjectile()
	{
		if (this.currentDecProjectile != null)
		{
			Object.Destroy(this.currentDecProjectile);
			this.eid.weakPoint = this.origWP;
		}
		this.currentProjectile = Object.Instantiate<GameObject>(this.projectile, this.shootPos.position, base.transform.rotation);
		Projectile componentInChildren = this.currentProjectile.GetComponentInChildren<Projectile>();
		if (componentInChildren != null)
		{
			componentInChildren.target = this.eid.target;
			componentInChildren.safeEnemyType = EnemyType.Stray;
			if (this.difficulty > 2)
			{
				componentInChildren.speed *= 1.35f;
			}
			else if (this.difficulty == 1)
			{
				componentInChildren.speed *= 0.75f;
			}
			else if (this.difficulty == 0)
			{
				componentInChildren.speed *= 0.5f;
			}
			componentInChildren.damage *= this.eid.totalDamageModifier;
		}
		Vector3 vector = this.currentProjectile.transform.position + base.transform.forward;
		EnemyTarget target = this.eid.target;
		if (target != null && target.isPlayer)
		{
			if (this.difficulty >= 4)
			{
				vector = MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(Vector3.Distance(this.currentProjectile.transform.position, this.camObj.transform.position) / (float)((this.difficulty == 5) ? 90 : Random.Range(110, 180)), true, false);
			}
			else
			{
				vector = this.camObj.transform.position;
			}
		}
		else if (this.eid.target != null)
		{
			EnemyIdentifierIdentifier componentInChildren2 = this.eid.target.targetTransform.GetComponentInChildren<EnemyIdentifierIdentifier>();
			if (componentInChildren2)
			{
				vector = componentInChildren2.transform.position;
			}
			else
			{
				vector = this.eid.target.position;
			}
		}
		this.currentProjectile.transform.LookAt(vector);
		ProjectileSpread componentInChildren3 = this.currentProjectile.GetComponentInChildren<ProjectileSpread>();
		if (componentInChildren3)
		{
			componentInChildren3.target = this.eid.target;
			if (this.difficulty <= 2)
			{
				if (this.difficulty == 2)
				{
					componentInChildren3.spreadAmount = 5f;
				}
				else if (this.difficulty == 1)
				{
					componentInChildren3.spreadAmount = 3f;
				}
				else if (this.difficulty == 0)
				{
					componentInChildren3.spreadAmount = 2f;
				}
				componentInChildren3.projectileAmount = 3;
			}
		}
	}

	// Token: 0x06001C82 RID: 7298 RVA: 0x000EF7A8 File Offset: 0x000ED9A8
	public void ShootProjectile(int skipOnEasy)
	{
		if (skipOnEasy > 0 && this.difficulty < 2)
		{
			return;
		}
		this.swinging = true;
		if (this.difficulty >= 4 && this.eid.enemyType == EnemyType.Schism && !this.predictionLerping && this.eid.target != null)
		{
			this.predictedPosition = (this.eid.target.isPlayer ? MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(4f, true, true) : this.eid.target.headPosition);
			if (this.eid.target.isPlayer)
			{
				this.predictedPosition.y = this.eid.target.headPosition.y;
			}
			this.predictionLerp = 0f;
			this.predictionLerping = true;
		}
		if (this.currentDecProjectile != null)
		{
			Object.Destroy(this.currentDecProjectile);
			this.eid.weakPoint = this.origWP;
		}
		if (this.currentProjectile)
		{
			this.previousProjectile = this.currentProjectile;
		}
		this.currentProjectile = Object.Instantiate<GameObject>(this.projectile, this.decProjectileSpawner.transform.position, this.decProjectileSpawner.transform.rotation);
		Projectile component = this.currentProjectile.GetComponent<Projectile>();
		component.safeEnemyType = EnemyType.Schism;
		component.target = this.eid.target;
		if (this.difficulty > 2)
		{
			component.speed *= 1.25f;
		}
		else if (this.difficulty == 1)
		{
			component.speed *= 0.75f;
		}
		else if (this.difficulty == 0)
		{
			component.speed *= 0.5f;
		}
		component.damage *= this.eid.totalDamageModifier;
		if (this.projectileBeam != null && this.previousProjectile && this.difficulty > 1)
		{
			ContinuousBeam continuousBeam = Object.Instantiate<ContinuousBeam>(this.projectileBeam, this.currentProjectile.transform.position, this.currentProjectile.transform.rotation, this.currentProjectile.transform);
			continuousBeam.endPoint = this.previousProjectile.transform;
			component.connectedBeams.Add(continuousBeam);
			if (this.previousProjectile.TryGetComponent<Projectile>(out component))
			{
				component.connectedBeams.Add(continuousBeam);
			}
		}
	}

	// Token: 0x06001C83 RID: 7299 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void StopTracking()
	{
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x000EFA1C File Offset: 0x000EDC1C
	public void DamageEnd()
	{
		if (!this.hasMelee && this.tr != null)
		{
			this.tr.enabled = false;
		}
		if (this.currentDecProjectile != null)
		{
			Object.Destroy(this.currentDecProjectile);
			this.eid.weakPoint = this.origWP;
		}
		this.zmb.attacking = false;
		this.moveForward = false;
		if (this.aimer != null)
		{
			this.aimEase = 0f;
			this.aiming = false;
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x000EFAA8 File Offset: 0x000EDCA8
	public void CancelAttack()
	{
		this.swinging = false;
		this.blocking = false;
		this.aiming = false;
		this.coolDown = 0f;
		this.moveForward = false;
		if (this.currentDecProjectile != null)
		{
			Object.Destroy(this.currentDecProjectile);
			this.eid.weakPoint = this.origWP;
		}
		if (this.tr != null)
		{
			this.tr.enabled = false;
		}
		this.zmb.attacking = false;
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x000EFB2C File Offset: 0x000EDD2C
	private void Wander()
	{
		if (this.nma.isOnNavMesh)
		{
			Vector3 onUnitSphere = Random.onUnitSphere;
			onUnitSphere.y = 0f;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up, onUnitSphere, out raycastHit, 15f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.wanderTarget = raycastHit.point;
			}
			else if (Physics.Raycast(base.transform.position + Vector3.up + onUnitSphere * 15f, Vector3.down, out raycastHit, 15f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.wanderTarget = raycastHit.point;
			}
			else
			{
				this.wanderTarget = base.transform.position + onUnitSphere * 15f;
			}
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(this.wanderTarget, out navMeshHit, 15f, 1))
			{
				this.wanderTarget = navMeshHit.position;
				this.SetDestination(navMeshHit.position);
			}
		}
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x000EFC3C File Offset: 0x000EDE3C
	public void Block(Vector3 attackPosition)
	{
		if (this.swinging)
		{
			this.CancelAttack();
		}
		this.swinging = true;
		this.blocking = true;
		this.aiming = false;
		this.seekingPlayer = false;
		this.nma.updateRotation = false;
		base.transform.LookAt(new Vector3(attackPosition.x, base.transform.position.y, attackPosition.z));
		this.zmb.KnockBack(base.transform.forward * -1f * 500f);
		Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.ineffectiveSound, base.transform.position, Quaternion.identity);
		this.nma.enabled = false;
		this.anim.Play("Block", -1, 0f);
	}

	// Token: 0x0400281F RID: 10271
	public bool stationary;

	// Token: 0x04002820 RID: 10272
	public bool alwaysStationary;

	// Token: 0x04002821 RID: 10273
	public bool smallRay;

	// Token: 0x04002822 RID: 10274
	public bool wanderer;

	// Token: 0x04002823 RID: 10275
	public bool afraid;

	// Token: 0x04002824 RID: 10276
	public bool chaser;

	// Token: 0x04002825 RID: 10277
	public bool hasMelee;

	// Token: 0x04002826 RID: 10278
	private Zombie zmb;

	// Token: 0x04002827 RID: 10279
	private GameObject player;

	// Token: 0x04002828 RID: 10280
	private GameObject camObj;

	// Token: 0x04002829 RID: 10281
	private NavMeshAgent nma;

	// Token: 0x0400282A RID: 10282
	private NavMeshPath nmp;

	// Token: 0x0400282B RID: 10283
	private NavMeshHit hit;

	// Token: 0x0400282C RID: 10284
	private Animator anim;

	// Token: 0x0400282D RID: 10285
	private Rigidbody rb;

	// Token: 0x0400282E RID: 10286
	public Vector3 targetPosition;

	// Token: 0x0400282F RID: 10287
	private float coolDown = 1f;

	// Token: 0x04002830 RID: 10288
	private AudioSource aud;

	// Token: 0x04002831 RID: 10289
	public TrailRenderer tr;

	// Token: 0x04002832 RID: 10290
	public GameObject projectile;

	// Token: 0x04002833 RID: 10291
	public ContinuousBeam projectileBeam;

	// Token: 0x04002834 RID: 10292
	private GameObject currentProjectile;

	// Token: 0x04002835 RID: 10293
	private GameObject previousProjectile;

	// Token: 0x04002836 RID: 10294
	public Transform shootPos;

	// Token: 0x04002837 RID: 10295
	public GameObject head;

	// Token: 0x04002838 RID: 10296
	public bool playerSpotted;

	// Token: 0x04002839 RID: 10297
	private RaycastHit rhit;

	// Token: 0x0400283A RID: 10298
	private RaycastHit bhit;

	// Token: 0x0400283B RID: 10299
	public bool seekingPlayer = true;

	// Token: 0x0400283C RID: 10300
	private Vector3 wanderTarget;

	// Token: 0x0400283D RID: 10301
	private float raySize = 1f;

	// Token: 0x0400283E RID: 10302
	private bool musicRequested;

	// Token: 0x0400283F RID: 10303
	public GameObject decProjectileSpawner;

	// Token: 0x04002840 RID: 10304
	public GameObject decProjectile;

	// Token: 0x04002841 RID: 10305
	private GameObject currentDecProjectile;

	// Token: 0x04002842 RID: 10306
	public bool swinging;

	// Token: 0x04002843 RID: 10307
	[HideInInspector]
	public bool blocking;

	// Token: 0x04002844 RID: 10308
	[HideInInspector]
	public int difficulty;

	// Token: 0x04002845 RID: 10309
	private float coolDownReduce;

	// Token: 0x04002846 RID: 10310
	private EnemyIdentifier eid;

	// Token: 0x04002847 RID: 10311
	private GameObject origWP;

	// Token: 0x04002848 RID: 10312
	public Transform aimer;

	// Token: 0x04002849 RID: 10313
	private Quaternion aimerDefaultRotation;

	// Token: 0x0400284A RID: 10314
	private bool aiming;

	// Token: 0x0400284B RID: 10315
	private Quaternion origRotation;

	// Token: 0x0400284C RID: 10316
	private float aimEase;

	// Token: 0x0400284D RID: 10317
	private Vector3 predictedPosition;

	// Token: 0x0400284E RID: 10318
	private float predictionLerp;

	// Token: 0x0400284F RID: 10319
	private bool predictionLerping;

	// Token: 0x04002850 RID: 10320
	private bool moveForward;

	// Token: 0x04002851 RID: 10321
	private float forwardSpeed;

	// Token: 0x04002852 RID: 10322
	private SwingCheck2[] swingChecks;

	// Token: 0x04002853 RID: 10323
	private float lengthOfStop;

	// Token: 0x04002854 RID: 10324
	private Vector3 spawnPos;

	// Token: 0x04002855 RID: 10325
	private bool valuesSet;
}
