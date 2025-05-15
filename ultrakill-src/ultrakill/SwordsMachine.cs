using System;
using System.Collections.Generic;
using plog;
using Sandbox;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

// Token: 0x0200046D RID: 1133
public class SwordsMachine : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>, IEnemyRelationshipLogic
{
	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x060019E9 RID: 6633 RVA: 0x000D4916 File Offset: 0x000D2B16
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000D4924 File Offset: 0x000D2B24
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.mach = base.GetComponent<Machine>();
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		this.swordAud = this.swordTrail.GetComponent<AudioSource>();
		this.shotgun = base.GetComponentInChildren<EnemyShotgun>();
		this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
		this.origMat = this.swordMR.sharedMaterial;
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x000D499C File Offset: 0x000D2B9C
	private void Start()
	{
		this.swordTrail.emitting = false;
		this.slapTrail.emitting = false;
		this.SetSpeed();
		this.gunDelay = true;
		BossHealthBar component = base.GetComponent<BossHealthBar>();
		if (component == null || !component.enabled)
		{
			this.bossVersion = false;
		}
		else
		{
			this.bossVersion = true;
		}
		base.Invoke("SlowUpdate", 0.5f);
		base.Invoke("NavigationUpdate", 0.1f);
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000D4A16 File Offset: 0x000D2C16
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000D4A20 File Offset: 0x000D2C20
	private void SetSpeed()
	{
		if (!this.nma)
		{
			this.nma = base.GetComponent<NavMeshAgent>();
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (!this.anim)
		{
			this.anim = base.GetComponentInChildren<Animator>();
		}
		if (!this.ensim)
		{
			this.ensim = base.GetComponentInChildren<EnemySimplifier>();
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
		if (this.difficulty != 2)
		{
			if (this.difficulty >= 3)
			{
				this.nma.speed = (float)(this.firstPhase ? 19 : 23);
				this.anim.speed = 1.2f;
				this.anim.SetFloat("ThrowSpeedMultiplier", 1.35f);
				this.anim.SetFloat("AttackSpeedMultiplier", 1f);
				this.moveSpeedMultiplier = ((this.difficulty == 3) ? 1.2f : 1.35f);
			}
			else if (this.difficulty <= 1)
			{
				this.nma.speed = (float)(this.firstPhase ? 14 : 18);
				this.anim.speed = 0.85f;
				if (this.difficulty == 1)
				{
					this.anim.SetFloat("ThrowSpeedMultiplier", 0.825f);
					this.anim.SetFloat("AttackSpeedMultiplier", 0.825f);
					this.moveSpeedMultiplier = 0.8f;
				}
				else
				{
					this.anim.SetFloat("ThrowSpeedMultiplier", 0.75f);
					this.anim.SetFloat("AttackSpeedMultiplier", 0.75f);
					this.moveSpeedMultiplier = 0.65f;
				}
			}
		}
		else
		{
			this.nma.speed = (float)(this.firstPhase ? 16 : 20);
			this.anim.speed = 1f;
			this.anim.SetFloat("ThrowSpeedMultiplier", 1f);
			this.anim.SetFloat("AttackSpeedMultiplier", 1f);
			this.moveSpeedMultiplier = 1f;
		}
		this.anim.SetFloat("RecoverySpeedMultiplier", (float)((this.difficulty >= 4) ? 2 : 1));
		this.nma.speed *= this.eid.totalSpeedModifier;
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.moveSpeedMultiplier *= this.eid.totalSpeedModifier;
		this.normalAnimSpeed = this.anim.speed;
		this.normalMovSpeed = this.nma.speed;
		if (this.enraged)
		{
			this.anim.speed = this.normalAnimSpeed * 1.15f;
			this.nma.speed = this.normalMovSpeed * 1.25f;
			this.ensim.enraged = true;
			if (!this.eid.puppet)
			{
				this.swordMR.sharedMaterial = this.enragedSword;
			}
		}
		if (this.shotgun)
		{
			this.shotgun.UpdateBuffs(this.eid);
		}
	}

	// Token: 0x060019EE RID: 6638 RVA: 0x000D4D79 File Offset: 0x000D2F79
	private void OnDisable()
	{
		if (base.GetComponent<BossHealthBar>() != null)
		{
			base.GetComponent<BossHealthBar>().DisappearBar();
		}
		if (this.currentThrownSword != null)
		{
			Object.Destroy(this.currentThrownSword);
		}
	}

	// Token: 0x060019EF RID: 6639 RVA: 0x000D4DAD File Offset: 0x000D2FAD
	private void OnEnable()
	{
		if (this.mach)
		{
			this.StopAction();
			this.CoolSword();
			this.StopMoving();
			this.DamageStop();
		}
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x000D4DD4 File Offset: 0x000D2FD4
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.5f);
		this.targetingStalker = false;
		if (!BlindEnemies.Blind && this.nma.isOnNavMesh && !this.eid.sandified)
		{
			List<EnemyIdentifier> enemiesOfType = MonoSingleton<EnemyTracker>.Instance.GetEnemiesOfType(EnemyType.Stalker);
			if (enemiesOfType.Count > 0)
			{
				float num = 100f;
				foreach (EnemyIdentifier enemyIdentifier in enemiesOfType)
				{
					if (!enemyIdentifier.blessed)
					{
						NavMeshPath navMeshPath = new NavMeshPath();
						this.nma.CalculatePath(enemyIdentifier.transform.position, navMeshPath);
						if (navMeshPath != null && navMeshPath.status == NavMeshPathStatus.PathComplete)
						{
							float num2 = 0f;
							for (int i = 1; i < navMeshPath.corners.Length; i++)
							{
								num2 += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
							}
							if (num2 < num)
							{
								this.eid.target = new EnemyTarget(enemyIdentifier.transform);
								this.targetingStalker = true;
								num = num2;
								if (this.shotgunning)
								{
									this.anim.SetLayerWeight(1, 0f);
									this.shotgunning = false;
									if (!this.gunDelay)
									{
										this.gunDelay = true;
										base.Invoke("ShootDelay", (float)Random.Range(5, 10) / this.eid.totalSpeedModifier);
									}
								}
							}
						}
					}
				}
			}
		}
		if (this.target == null)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, this.target.position - base.transform.position, out raycastHit, Vector3.Distance(base.transform.position + Vector3.up * 0.1f, this.target.position), LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.targetViewBlocked = true;
			Breakable breakable;
			if (raycastHit.distance < 5f && raycastHit.transform.TryGetComponent<Breakable>(out breakable) && !breakable.playerOnly)
			{
				this.breakableInWay = true;
				return;
			}
		}
		else
		{
			this.targetViewBlocked = false;
			Breakable breakable2;
			if (this.target.position.y > base.transform.position.y + 2.5f && Vector2.Distance(new Vector2(base.transform.position.x, base.transform.position.z), new Vector3(this.target.position.x, this.target.position.z)) < 5f && Physics.Raycast(this.target.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, Mathf.Clamp(this.target.position.y - base.transform.position.y, 0f, 5f), LayerMaskDefaults.Get(LMD.Environment)) && raycastHit.transform.TryGetComponent<Breakable>(out breakable2) && !breakable2.playerOnly)
			{
				this.breakableInWay = true;
			}
		}
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x000D5168 File Offset: 0x000D3368
	private void NavigationUpdate()
	{
		base.Invoke("NavigationUpdate", 0.1f);
		if (this.target == null)
		{
			return;
		}
		if (!this.inAction && this.nma.enabled && this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(this.target.position);
		}
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000D51C8 File Offset: 0x000D33C8
	private void Update()
	{
		if (this.active && this.nma != null)
		{
			if (this.spawnAttackDelay > 0f)
			{
				this.spawnAttackDelay = Mathf.MoveTowards(this.spawnAttackDelay, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (this.breakableInWay && !this.inAction)
			{
				this.breakableInWay = false;
				this.inAction = true;
				this.RunningSwing();
			}
			else if (this.target != null && this.spawnAttackDelay <= 0f)
			{
				if ((this.firstPhase || this.bothPhases) && (!this.enraged || this.difficulty >= 4) && !this.inAction && this.shotgun.gunReady && !this.gunDelay && !this.shotgunning && Vector3.Distance(this.target.position, base.transform.position) > 5f && !this.targetingStalker)
				{
					this.shotgunning = true;
					this.anim.SetLayerWeight(1, 1f);
					this.anim.SetTrigger("Shoot");
					this.aimLerp = 0f;
				}
				else if (!this.firstPhase && (!this.enraged || this.difficulty >= 4) && !this.inAction && !this.inSemiAction && !this.targetViewBlocked && ((this.swordThrowCharge == 0f && Vector3.Distance(this.target.position, base.transform.position) > 5f) || Vector3.Distance(this.target.position, base.transform.position) > 20f) && !this.targetingStalker)
				{
					this.swordThrowCharge = 3f;
					if ((float)Random.Range(0, 100) <= this.swordThrowChance || this.target.position.y > base.transform.position.y + 3f || Vector3.Distance(this.target.position, base.transform.position) > 16f)
					{
						this.inAction = true;
						this.throwType = 2;
						this.SwordThrow();
						if (this.swordThrowChance > 50f)
						{
							this.swordThrowChance = 25f;
						}
						else
						{
							this.swordThrowChance -= 25f;
						}
					}
					else if (this.swordThrowChance < 50f)
					{
						this.swordThrowChance = 75f;
					}
					else
					{
						this.swordThrowChance += 25f;
					}
				}
				if (this.runningAttack && !this.inAction && (!this.inSemiAction || this.difficulty >= 4) && Vector3.Distance(this.target.position, base.transform.position) <= 8f && Vector3.Distance(this.target.position, base.transform.position) >= 5f)
				{
					this.runningAttackCharge = 3f;
					if ((float)Random.Range(0, 100) <= this.runningAttackChance)
					{
						if (this.runningAttackChance > 50f)
						{
							this.runningAttackChance = 50f;
						}
						this.runningAttackChance -= 25f;
						this.inAction = true;
						this.RunningSwing();
						if (this.shotgunning)
						{
							this.anim.SetLayerWeight(1, 0f);
							this.shotgunning = false;
							if (!this.gunDelay)
							{
								this.gunDelay = true;
								base.Invoke("ShootDelay", (float)Random.Range(5, 10));
							}
						}
					}
					else
					{
						if (this.runningAttackChance < 50f)
						{
							this.runningAttackChance = 50f;
						}
						this.runningAttackChance += 25f;
						this.runningAttack = false;
					}
				}
				else if (!this.inAction && (!this.inSemiAction || this.difficulty >= 4) && Vector3.Distance(this.target.position, base.transform.position) <= 5f)
				{
					this.inAction = true;
					if (this.shotgunning)
					{
						this.anim.SetLayerWeight(1, 0f);
						this.shotgunning = false;
						if (!this.gunDelay)
						{
							this.gunDelay = true;
							base.Invoke("ShootDelay", (float)Random.Range(5, 10) / this.eid.totalSpeedModifier);
						}
					}
					if (!this.firstPhase && !this.enraged && !this.targetingStalker)
					{
						if ((float)Random.Range(0, 100) <= this.spiralSwordChance && !this.inSemiAction)
						{
							this.SwordSpiral();
							if (this.spiralSwordChance > 50f)
							{
								this.spiralSwordChance = 25f;
							}
							else
							{
								this.spiralSwordChance -= 25f;
							}
						}
						else
						{
							this.Combo();
							if (this.spiralSwordChance < 50f)
							{
								this.spiralSwordChance = 50f;
							}
							this.spiralSwordChance += 25f;
						}
					}
					else
					{
						this.Combo();
					}
				}
				if (!this.runningAttack && this.runningAttackCharge > 0f)
				{
					this.runningAttackCharge -= Time.deltaTime * this.eid.totalSpeedModifier;
					if (this.runningAttackCharge <= 0f)
					{
						this.runningAttackCharge = 0f;
						this.runningAttack = true;
					}
				}
				if (!this.firstPhase)
				{
					if (this.swordThrowCharge > 0f && Vector3.Distance(this.target.position, base.transform.position) > 5f)
					{
						this.swordThrowCharge = Mathf.MoveTowards(this.swordThrowCharge, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
					else
					{
						this.swordThrowCharge = 0f;
					}
					if (this.chaseThrowCharge > 0f)
					{
						this.chaseThrowCharge = Mathf.MoveTowards(this.chaseThrowCharge, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
				}
			}
		}
		if (!this.inAction && this.nma && this.nma.enabled && this.nma.isOnNavMesh)
		{
			if (this.nma.velocity.magnitude > 0.1f)
			{
				this.anim.SetBool("Running", true);
			}
			else
			{
				this.anim.SetBool("Running", false);
			}
		}
		if (!this.eternalRage && this.rageLeft > 0f)
		{
			this.rageLeft = Mathf.MoveTowards(this.rageLeft, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.enrageAud != null && this.rageLeft < 3f)
			{
				this.enrageAud.pitch = this.rageLeft / 3f;
			}
			if (this.rageLeft <= 0f)
			{
				this.enraged = false;
				this.ensim.enraged = false;
				if (!this.eid.puppet)
				{
					this.swordMR.sharedMaterial = this.origMat;
				}
				this.nma.speed = this.normalMovSpeed;
				this.anim.speed = this.normalAnimSpeed;
				if (this.currentEnrageEffect != null)
				{
					Object.Destroy(this.currentEnrageEffect);
				}
			}
		}
		if (this.firstPhase && this.mach.health <= this.phaseChangeHealth)
		{
			this.firstPhase = false;
			this.phaseChangeHealth = 0f;
			if (this.bossVersion)
			{
				MonoSingleton<NewMovement>.Instance.ResetHardDamage();
				MonoSingleton<NewMovement>.Instance.GetHealth(999, true, false, true);
			}
			this.EndFirstPhase();
		}
		if (((this.firstPhase && this.mach.health < 110f) || this.bothPhases) && !this.usingShotgun)
		{
			this.usingShotgun = true;
			this.gunDelay = false;
		}
		if (this.mach.health < 95f)
		{
			this.gunDelay = false;
		}
		if (this.idleFailsafe > 0f && this.anim && (this.inAction || !this.active || this.knockedDown || this.downed) && this.anim.GetCurrentAnimatorClipInfo(0).Length != 0 && this.anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
		{
			this.idleFailsafe = Mathf.MoveTowards(this.idleFailsafe, 0f, Time.deltaTime);
			if (this.idleFailsafe == 0f)
			{
				this.StopAction();
				if (this.knockedDown || this.downed)
				{
					this.Disappear();
					return;
				}
			}
		}
		else
		{
			this.idleFailsafe = 1f;
		}
	}

	// Token: 0x060019F3 RID: 6643 RVA: 0x000D5AD4 File Offset: 0x000D3CD4
	private void FixedUpdate()
	{
		if (this.rb.isKinematic)
		{
			return;
		}
		if (this.moveAtTarget)
		{
			float num = Mathf.Min(0f, this.rb.velocity.y);
			RaycastHit raycastHit;
			if (this.enraged || Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward, Vector3.down, out raycastHit, Mathf.Max(22f, base.transform.position.y - MonoSingleton<PlayerTracker>.Instance.GetPlayer().position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.rb.velocity = this.moveTarget * this.moveSpeed;
			}
			else
			{
				this.rb.velocity = Vector3.zero;
			}
			this.rb.velocity = new Vector3(this.rb.velocity.x, num, this.rb.velocity.z);
			return;
		}
		this.rb.velocity = new Vector3(0f, Mathf.Min(0f, this.rb.velocity.y), 0f);
	}

	// Token: 0x060019F4 RID: 6644 RVA: 0x000D5C28 File Offset: 0x000D3E28
	private void LateUpdate()
	{
		if (!this.firstPhase && !this.eternalRage && !this.bothPhases)
		{
			this.rightArm.localScale = Vector3.zero;
		}
		if (this.difficulty >= 4 && this.usingShotgun && this.eid.target != null)
		{
			if (this.shotgunning)
			{
				this.aimLerp = Mathf.MoveTowards(this.aimLerp, 1f, Time.deltaTime * 2f);
			}
			else
			{
				this.aimLerp = Mathf.MoveTowards(this.aimLerp, 0f, Time.deltaTime * 8f);
			}
			if (this.aimLerp > 0f)
			{
				Quaternion[] array = new Quaternion[this.aimBones.Length];
				for (int i = 0; i < this.aimBones.Length; i++)
				{
					array[i] = this.aimBones[i].localRotation;
					this.aimBones[i].LookAt(this.eid.target.position);
					if (i == 1)
					{
						this.aimBones[i].transform.Rotate(Vector3.right * 90f, Space.Self);
					}
					this.aimBones[i].localRotation = Quaternion.Lerp(array[i], this.aimBones[i].localRotation, this.aimLerp);
				}
			}
		}
	}

	// Token: 0x060019F5 RID: 6645 RVA: 0x000D5D8C File Offset: 0x000D3F8C
	public void RunningSwing()
	{
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.SetTrigger("RunningSwing");
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.moveSpeed = 30f * this.moveSpeedMultiplier;
		this.damage = 40;
	}

	// Token: 0x060019F6 RID: 6646 RVA: 0x000D5E44 File Offset: 0x000D4044
	private void Combo()
	{
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.SetTrigger("Combo");
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.moveSpeed = 60f * this.moveSpeedMultiplier;
		this.damage = 25;
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x000D5EFC File Offset: 0x000D40FC
	private void SwordThrow()
	{
		this.anim.SetBool("Running", false);
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.SetTrigger("SwordThrow");
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.damage = 0;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x000D5FB4 File Offset: 0x000D41B4
	private void SwordSpiral()
	{
		this.throwType = 1;
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.SetTrigger("SwordSpiral");
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.waitingForSword = true;
		this.damage = 0;
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x000D6068 File Offset: 0x000D4268
	public void StartMoving()
	{
		if (this.knockedDown || this.downed || this.target == null)
		{
			return;
		}
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.rb.isKinematic = false;
		this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		this.moveTarget = base.transform.forward;
		this.moveAtTarget = true;
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x000D611C File Offset: 0x000D431C
	public void StopMoving()
	{
		this.moveAtTarget = false;
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x000D6180 File Offset: 0x000D4380
	public void LookAt()
	{
		if (this.target == null)
		{
			return;
		}
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x000D61D8 File Offset: 0x000D43D8
	public void StopAction()
	{
		this.mach.parryable = false;
		this.waitingForSword = false;
		if (this.gc.onGround && this.nma)
		{
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
		}
		this.StopMoving();
		this.inAction = false;
		this.runningAttack = true;
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x000D624C File Offset: 0x000D444C
	public void SemiStopAction()
	{
		this.mach.parryable = false;
		if (this.gc.onGround && this.nma)
		{
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
		}
		this.inSemiAction = true;
		this.inAction = false;
		this.anim.SetTrigger("AnimationCancel");
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000D62C4 File Offset: 0x000D44C4
	public void HeatSword()
	{
		if (!this.inSemiAction)
		{
			this.swordTrail.emitting = true;
		}
		else
		{
			this.slapTrail.emitting = true;
		}
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.heatMat;
		}
		this.swordAud.pitch = 1.5f;
		Object.Instantiate<GameObject>(this.flash.ToAsset(), this.head.transform.position + Vector3.up + this.head.transform.forward, this.head.transform.rotation, this.head.transform);
		this.mach.ParryableCheck(false);
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000D6388 File Offset: 0x000D4588
	public void HeatSwordThrow()
	{
		if (this.swordTrail)
		{
			this.swordTrail.emitting = true;
		}
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.heatMat;
		}
		this.swordAud.pitch = 1.5f;
		Object.Instantiate<GameObject>(this.gunFlash.ToAsset(), this.head.transform);
		if (this.throwType == 2 && this.target != null)
		{
			if (this.target.isPlayer)
			{
				this.targetFuturePos = this.target.position + this.target.GetVelocity() * (Vector3.Distance(base.transform.position, this.target.position) / 80f) * Vector3.Distance(base.transform.position, this.target.position) * 0.08f / this.anim.speed;
			}
			else
			{
				this.targetFuturePos = this.target.position + this.target.GetVelocity();
			}
			base.transform.LookAt(new Vector3(this.targetFuturePos.x, base.transform.position.y, this.targetFuturePos.z));
		}
		this.mach.ParryableCheck(false);
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x000D6508 File Offset: 0x000D4708
	public void CoolSword()
	{
		if (this.swordTrail)
		{
			this.swordTrail.emitting = false;
		}
		if (this.slapTrail)
		{
			this.slapTrail.emitting = false;
		}
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = (this.enraged ? this.enragedSword : this.origMat);
		}
		this.swordAud.pitch = 1f;
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x000D6588 File Offset: 0x000D4788
	public void DamageStart()
	{
		this.damaging = true;
		if (!this.inSemiAction)
		{
			if (this.swordTrail)
			{
				Object.Instantiate<GameObject>(this.swingSound, this.swordTrail.transform);
			}
			foreach (SwingCheck2 swingCheck in this.swordSwingCheck)
			{
				swingCheck.OverrideEnemyIdentifier(this.eid);
				swingCheck.damage = this.damage;
				swingCheck.DamageStart();
			}
			return;
		}
		this.slapSwingCheck.OverrideEnemyIdentifier(this.eid);
		this.slapSwingCheck.DamageStart();
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x000D661C File Offset: 0x000D481C
	public void DamageStop()
	{
		this.damaging = false;
		this.mach.parryable = false;
		SwingCheck2[] array = this.swordSwingCheck;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
		this.slapSwingCheck.DamageStop();
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x000D6664 File Offset: 0x000D4864
	public void ShootGun()
	{
		if (!this.inAction)
		{
			this.shotgun.UpdateTarget(this.target);
			this.shotgun.Fire();
		}
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x000D668C File Offset: 0x000D488C
	public void StopShootAnimation()
	{
		this.mach.parryable = false;
		this.anim.SetLayerWeight(1, 0f);
		this.gunDelay = true;
		this.shotgunning = false;
		base.Invoke("ShootDelay", (float)Random.Range(5, 20) / this.eid.totalSpeedModifier);
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x000D66E4 File Offset: 0x000D48E4
	private void ShootDelay()
	{
		this.gunDelay = false;
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x000D66F0 File Offset: 0x000D48F0
	public void FlashGun()
	{
		Object.Instantiate<GameObject>(this.gunFlash.ToAsset(), this.head.transform.position + Vector3.up + this.head.transform.forward, this.head.transform.rotation, this.head.transform);
	}

	// Token: 0x06001A07 RID: 6663 RVA: 0x000D6758 File Offset: 0x000D4958
	public void SwordSpawn()
	{
		this.mach.parryable = false;
		if (this.target == null)
		{
			return;
		}
		RaycastHit raycastHit;
		if (this.throwType != 2)
		{
			this.targetFuturePos = this.target.position;
		}
		else if (this.target.isPlayer)
		{
			this.targetFuturePos = new Vector3(this.targetFuturePos.x, this.target.position.y + MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).y * Vector3.Distance(base.transform.position, this.target.position) * 0.01f, this.targetFuturePos.z);
			if (Physics.Raycast(this.target.position, this.targetFuturePos - this.target.position, out raycastHit, Vector3.Distance(this.target.position, this.targetFuturePos), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.targetFuturePos = raycastHit.point;
			}
		}
		else
		{
			this.targetFuturePos = this.target.position + this.target.GetVelocity() / this.eid.totalSpeedModifier;
		}
		base.transform.LookAt(new Vector3(this.targetFuturePos.x, base.transform.position.y, this.targetFuturePos.z));
		this.currentThrownSword = Object.Instantiate<GameObject>(this.thrownSword[this.throwType], new Vector3(base.transform.position.x, this.handTransform.position.y, base.transform.position.z), Quaternion.identity);
		ThrownSword componentInChildren = this.currentThrownSword.GetComponentInChildren<ThrownSword>();
		componentInChildren.thrownBy = this.eid;
		if (this.throwType != 1)
		{
			this.currentThrownSword.transform.rotation = base.transform.rotation;
		}
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.origMat;
		}
		this.swordMR.enabled = false;
		this.swordTrail.emitting = false;
		this.slapTrail.emitting = false;
		this.swordAud.pitch = 0f;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, (this.targetFuturePos - base.transform.position).normalized, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
		{
			componentInChildren.SetPoints(raycastHit.point, this.handTransform);
		}
		else
		{
			componentInChildren.thrownAtVoid = true;
			componentInChildren.SetPoints((this.targetFuturePos - base.transform.position) * 9999f, this.handTransform);
		}
		if (this.throwType == 2)
		{
			this.SemiStopAction();
		}
		base.Invoke("SwordCatch", 5f);
	}

	// Token: 0x06001A08 RID: 6664 RVA: 0x000D6A6C File Offset: 0x000D4C6C
	public void SwordCatch()
	{
		this.mach.parryable = false;
		if (this.currentThrownSword)
		{
			Object.Destroy(this.currentThrownSword);
		}
		if (!this.knockedDown && !this.downed && (this.difficulty < 4 || !this.inAction || this.waitingForSword))
		{
			this.inAction = true;
			this.anim.SetTrigger("SwordCatch");
		}
		this.inSemiAction = false;
		this.waitingForSword = false;
		this.swordMR.enabled = true;
		this.swordAud.pitch = 1f;
		this.swordThrowCharge = 3f;
		base.CancelInvoke("SwordCatch");
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x000D6B20 File Offset: 0x000D4D20
	private void EndFirstPhase()
	{
		this.DamageStop();
		this.knockedDown = true;
		this.inAction = true;
		this.inSemiAction = false;
		this.waitingForSword = false;
		this.anim.SetLayerWeight(1, 0f);
		this.gunDelay = true;
		this.shotgunning = false;
		this.swordTrail.emitting = false;
		this.slapTrail.emitting = false;
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.origMat;
		}
		this.swordAud.pitch = 1f;
		this.nma.enabled = true;
		this.inPhaseChange = true;
		this.active = false;
		this.moveAtTarget = false;
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		if (this.target != null)
		{
			base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		}
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.isKinematic = false;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		if (!this.bsm)
		{
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		}
		GameObject gore = this.bsm.GetGore(GoreType.Limb, this.eid, false);
		if (gore)
		{
			gore.transform.position = this.rightArm.position;
		}
		if (this.bossVersion && this.shotgunPickUp != null)
		{
			this.shotgunPickUp.transform.SetPositionAndRotation(this.shotgun.transform.position, this.shotgun.transform.rotation);
			this.shotgunPickUp.SetActive(true);
		}
		CharacterJoint[] componentsInChildren = this.rightArm.GetComponentsInChildren<CharacterJoint>();
		base.GetComponentInParent<GoreZone>();
		if (componentsInChildren.Length != 0)
		{
			foreach (CharacterJoint characterJoint in componentsInChildren)
			{
				Object.Destroy(characterJoint);
				characterJoint.transform.localScale = Vector3.zero;
				characterJoint.gameObject.SetActive(false);
			}
		}
		this.anim.Rebind();
		this.SetSpeed();
		this.anim.SetTrigger("Knockdown");
		if (this.bossVersion)
		{
			MonoSingleton<TimeController>.Instance.SlowDown(0.15f);
		}
		Object.Instantiate<GameObject>(this.bigPainSound, base.transform);
		if (this.secondPhasePosTarget != null)
		{
			MonoSingleton<MusicManager>.Instance.ArenaMusicEnd();
			MonoSingleton<MusicManager>.Instance.PlayCleanMusic();
		}
		this.normalMovSpeed = this.nma.speed;
		this.rageLeft = 0.01f;
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x000D6DF4 File Offset: 0x000D4FF4
	public void Knockdown(bool light = false, bool fromExplosion = false)
	{
		this.DamageStop();
		this.knockedDown = true;
		this.inAction = true;
		this.waitingForSword = false;
		this.inSemiAction = false;
		this.anim.SetLayerWeight(1, 0f);
		this.gunDelay = true;
		this.shotgunning = false;
		this.swordMR.enabled = true;
		this.swordTrail.emitting = false;
		this.slapTrail.emitting = false;
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.origMat;
		}
		this.swordAud.pitch = 1f;
		this.nma.enabled = true;
		this.SetSpeed();
		this.moveAtTarget = false;
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.isKinematic = false;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.moveAtTarget = false;
		if (light)
		{
			this.anim.Play("LightKnockdown");
		}
		else
		{
			this.anim.Play("Knockdown");
		}
		if (this.mach == null)
		{
			this.mach = base.GetComponent<Machine>();
		}
		if (!light)
		{
			base.GetComponent<EnemyIdentifier>().hitter = "projectile";
			if (this.mach.health > 20f)
			{
				this.mach.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, 20f, 0f, null, false);
			}
			else
			{
				this.mach.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, this.mach.health - 0.1f, 0f, null, false);
			}
		}
		if (!this.bsm)
		{
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		}
		GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
		gore.transform.position = base.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position;
		Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
		if (component != null)
		{
			component.GetReady();
		}
		ParticleSystem component2 = gore.GetComponent<ParticleSystem>();
		if (component2 != null)
		{
			component2.Play();
		}
		if (!light)
		{
			Object.Instantiate<GameObject>(this.bigPainSound, base.transform);
		}
		this.Enrage();
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x000D70A0 File Offset: 0x000D52A0
	public void Down(bool fromExplosion = false)
	{
		this.downed = true;
		this.DamageStop();
		this.inAction = true;
		this.waitingForSword = false;
		this.inSemiAction = false;
		this.anim.SetLayerWeight(1, 0f);
		this.gunDelay = true;
		this.shotgunning = false;
		this.swordMR.enabled = true;
		this.swordTrail.emitting = false;
		this.slapTrail.emitting = false;
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.origMat;
		}
		this.swordAud.pitch = 1f;
		this.nma.enabled = true;
		this.SetSpeed();
		this.moveAtTarget = false;
		this.nma.updatePosition = false;
		this.nma.updateRotation = false;
		this.nma.enabled = false;
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.isKinematic = false;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.velocity = Vector3.zero;
		}
		this.moveAtTarget = false;
		this.anim.Play("Knockdown");
		base.Invoke("CheckLoop", 0.5f);
		if (this.mach == null)
		{
			this.mach = base.GetComponent<Machine>();
		}
		if (!this.bsm)
		{
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		}
		GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
		gore.transform.position = base.GetComponentInChildren<EnemyIdentifierIdentifier>().transform.position;
		Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
		if (component != null)
		{
			component.GetReady();
		}
		ParticleSystem component2 = gore.GetComponent<ParticleSystem>();
		if (component2 != null)
		{
			component2.Play();
		}
		Object.Instantiate<GameObject>(this.bigPainSound, base.transform);
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000D72BC File Offset: 0x000D54BC
	public void Disappear()
	{
		if (this.secondPhasePosTarget != null && !this.firstPhase)
		{
			BossHealthBar component = base.GetComponent<BossHealthBar>();
			component.DisappearBar();
			new Vector3(base.transform.position.x, base.transform.position.y + 1.5f, base.transform.position.z);
			this.teleportEffect.SetActive(true);
			this.teleportEffect.transform.SetParent(null, true);
			base.gameObject.SetActive(false);
			SwordsMachine[] componentsInChildren = this.secondPhasePosTarget.GetComponentsInChildren<SwordsMachine>();
			if (componentsInChildren.Length != 0)
			{
				foreach (SwordsMachine swordsMachine in componentsInChildren)
				{
					swordsMachine.gameObject.SetActive(false);
					Object.Destroy(swordsMachine.gameObject);
				}
			}
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
			base.transform.position = this.secondPhasePosTarget.position;
			base.transform.parent = this.secondPhasePosTarget;
			this.eid.spawnIn = true;
			base.gameObject.SetActive(true);
			component.enabled = true;
		}
		this.knockedDown = false;
		this.moveAtTarget = false;
		if (this.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		this.inPhaseChange = false;
		this.active = true;
		if (this.gc.onGround)
		{
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
		}
		this.inAction = false;
		this.inSemiAction = false;
		if (this.activateOnPhaseChange != null && !this.firstPhase)
		{
			this.activateOnPhaseChange.SetActive(true);
		}
		base.GetComponent<AudioSource>().volume = 0f;
		if (this.secondPhasePosTarget != null && !this.firstPhase)
		{
			this.secondPhasePosTarget = null;
			this.cpToReset.UpdateRooms();
		}
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000D74CC File Offset: 0x000D56CC
	public void Enrage()
	{
		if (this.enraged)
		{
			return;
		}
		if (!this.bothPhases)
		{
			this.enraged = true;
			this.rageLeft = 10f;
			this.anim.speed = this.normalAnimSpeed * 1.15f;
			this.nma.speed = this.normalMovSpeed * 1.25f;
			this.ensim.enraged = true;
			if (!this.eid.puppet)
			{
				this.swordMR.sharedMaterial = this.enragedSword;
			}
			Object.Instantiate<GameObject>(this.bigPainSound, base.transform).GetComponent<AudioSource>().pitch = 2f;
			if (this.currentEnrageEffect == null)
			{
				this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, this.mach.chest.transform);
				this.enrageAud = this.currentEnrageEffect.GetComponent<AudioSource>();
			}
			this.enrageAud.pitch = 1f;
		}
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x000D75C8 File Offset: 0x000D57C8
	public void UnEnrage()
	{
		if (!this.enraged)
		{
			return;
		}
		this.rageLeft = 0f;
		this.anim.speed = this.normalAnimSpeed;
		this.nma.speed = this.normalMovSpeed;
		this.ensim.enraged = false;
		if (!this.eid.puppet)
		{
			this.swordMR.sharedMaterial = this.origMat;
		}
		this.enraged = false;
		Object.Destroy(this.currentEnrageEffect);
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001A0F RID: 6671 RVA: 0x000D7647 File Offset: 0x000D5847
	public bool isEnraged
	{
		get
		{
			return this.enraged;
		}
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x000D764F File Offset: 0x000D584F
	public void CheckLoop()
	{
		if (this.downed)
		{
			this.anim.Play("Knockdown", 0, 0.25f);
			base.Invoke("CheckLoop", 0.25f);
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06001A11 RID: 6673 RVA: 0x000D767F File Offset: 0x000D587F
	public string alterKey
	{
		get
		{
			return "swordsmachine";
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001A12 RID: 6674 RVA: 0x000D767F File Offset: 0x000D587F
	public string alterCategoryName
	{
		get
		{
			return "swordsmachine";
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06001A13 RID: 6675 RVA: 0x000D7688 File Offset: 0x000D5888
	AlterOption<bool>[] IAlterOptions<bool>.options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					value = this.isEnraged,
					callback = delegate(bool value)
					{
						if (value)
						{
							this.Enrage();
							return;
						}
						this.UnEnrage();
					},
					key = "enraged",
					name = "Enraged"
				},
				new AlterOption<bool>
				{
					value = this.eternalRage,
					callback = delegate(bool value)
					{
						this.eternalRage = value;
					},
					key = "eternal-rage",
					name = "Eternal Rage"
				}
			};
		}
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000C9FC2 File Offset: 0x000C81C2
	public bool ShouldAttackEnemies()
	{
		return false;
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x000D7713 File Offset: 0x000D5913
	public bool ShouldIgnorePlayer()
	{
		return this.target != null && this.target.isEnemy && this.target.enemyIdentifier.enemyType == EnemyType.Stalker;
	}

	// Token: 0x04002443 RID: 9283
	private static readonly global::plog.Logger Log = new global::plog.Logger("SwordsMachine");

	// Token: 0x04002444 RID: 9284
	public Transform targetZone;

	// Token: 0x04002445 RID: 9285
	private NavMeshAgent nma;

	// Token: 0x04002446 RID: 9286
	private Animator anim;

	// Token: 0x04002447 RID: 9287
	private Rigidbody rb;

	// Token: 0x04002448 RID: 9288
	private Machine mach;

	// Token: 0x04002449 RID: 9289
	public float phaseChangeHealth;

	// Token: 0x0400244A RID: 9290
	public bool firstPhase;

	// Token: 0x0400244B RID: 9291
	public bool active = true;

	// Token: 0x0400244C RID: 9292
	public Transform rightArm;

	// Token: 0x0400244D RID: 9293
	[SerializeField]
	private Transform[] aimBones;

	// Token: 0x0400244E RID: 9294
	private float aimLerp;

	// Token: 0x0400244F RID: 9295
	public bool inAction;

	// Token: 0x04002450 RID: 9296
	public bool inSemiAction;

	// Token: 0x04002451 RID: 9297
	[HideInInspector]
	public bool moveAtTarget;

	// Token: 0x04002452 RID: 9298
	private Vector3 moveTarget;

	// Token: 0x04002453 RID: 9299
	private float moveSpeed;

	// Token: 0x04002454 RID: 9300
	public TrailRenderer swordTrail;

	// Token: 0x04002455 RID: 9301
	public TrailRenderer slapTrail;

	// Token: 0x04002456 RID: 9302
	public SkinnedMeshRenderer swordMR;

	// Token: 0x04002457 RID: 9303
	public Material enragedSword;

	// Token: 0x04002458 RID: 9304
	public Material heatMat;

	// Token: 0x04002459 RID: 9305
	private Material origMat;

	// Token: 0x0400245A RID: 9306
	private AudioSource swordAud;

	// Token: 0x0400245B RID: 9307
	public GameObject swingSound;

	// Token: 0x0400245C RID: 9308
	public GameObject head;

	// Token: 0x0400245D RID: 9309
	public AssetReference flash;

	// Token: 0x0400245E RID: 9310
	public AssetReference gunFlash;

	// Token: 0x0400245F RID: 9311
	private bool runningAttack = true;

	// Token: 0x04002460 RID: 9312
	public float runningAttackCharge;

	// Token: 0x04002461 RID: 9313
	public bool damaging;

	// Token: 0x04002462 RID: 9314
	public int damage;

	// Token: 0x04002463 RID: 9315
	public float runningAttackChance = 50f;

	// Token: 0x04002464 RID: 9316
	private EnemyShotgun shotgun;

	// Token: 0x04002465 RID: 9317
	private bool shotgunning;

	// Token: 0x04002466 RID: 9318
	private bool gunDelay;

	// Token: 0x04002467 RID: 9319
	public GameObject shotgunPickUp;

	// Token: 0x04002468 RID: 9320
	public GameObject activateOnPhaseChange;

	// Token: 0x04002469 RID: 9321
	private bool usingShotgun;

	// Token: 0x0400246A RID: 9322
	public Transform secondPhasePosTarget;

	// Token: 0x0400246B RID: 9323
	[SerializeField]
	private GameObject teleportEffect;

	// Token: 0x0400246C RID: 9324
	public CheckPoint cpToReset;

	// Token: 0x0400246D RID: 9325
	public float swordThrowCharge = 3f;

	// Token: 0x0400246E RID: 9326
	public int throwType;

	// Token: 0x0400246F RID: 9327
	public GameObject[] thrownSword;

	// Token: 0x04002470 RID: 9328
	private GameObject currentThrownSword;

	// Token: 0x04002471 RID: 9329
	public Transform handTransform;

	// Token: 0x04002472 RID: 9330
	private float swordThrowChance = 50f;

	// Token: 0x04002473 RID: 9331
	private float spiralSwordChance = 50f;

	// Token: 0x04002474 RID: 9332
	public float chaseThrowCharge;

	// Token: 0x04002475 RID: 9333
	private bool waitingForSword;

	// Token: 0x04002476 RID: 9334
	public GameObject bigPainSound;

	// Token: 0x04002477 RID: 9335
	private Vector3 targetFuturePos;

	// Token: 0x04002478 RID: 9336
	private int difficulty = -1;

	// Token: 0x04002479 RID: 9337
	public bool enraged;

	// Token: 0x0400247A RID: 9338
	private float rageLeft;

	// Token: 0x0400247B RID: 9339
	public EnemySimplifier ensim;

	// Token: 0x0400247C RID: 9340
	private float normalAnimSpeed;

	// Token: 0x0400247D RID: 9341
	private float normalMovSpeed;

	// Token: 0x0400247E RID: 9342
	public GameObject enrageEffect;

	// Token: 0x0400247F RID: 9343
	public GameObject currentEnrageEffect;

	// Token: 0x04002480 RID: 9344
	private AudioSource enrageAud;

	// Token: 0x04002481 RID: 9345
	public Door[] doorsInPath;

	// Token: 0x04002482 RID: 9346
	public bool eternalRage;

	// Token: 0x04002483 RID: 9347
	public bool bothPhases;

	// Token: 0x04002484 RID: 9348
	private bool knockedDown;

	// Token: 0x04002485 RID: 9349
	public bool downed;

	// Token: 0x04002486 RID: 9350
	[SerializeField]
	private SwingCheck2[] swordSwingCheck;

	// Token: 0x04002487 RID: 9351
	[SerializeField]
	private SwingCheck2 slapSwingCheck;

	// Token: 0x04002488 RID: 9352
	private GroundCheckEnemy gc;

	// Token: 0x04002489 RID: 9353
	private bool bossVersion;

	// Token: 0x0400248A RID: 9354
	private EnemyIdentifier eid;

	// Token: 0x0400248B RID: 9355
	private BloodsplatterManager bsm;

	// Token: 0x0400248C RID: 9356
	private float idleFailsafe = 1f;

	// Token: 0x0400248D RID: 9357
	private bool idling;

	// Token: 0x0400248E RID: 9358
	private bool inPhaseChange;

	// Token: 0x0400248F RID: 9359
	private float moveSpeedMultiplier = 1f;

	// Token: 0x04002490 RID: 9360
	private bool breakableInWay;

	// Token: 0x04002491 RID: 9361
	private bool targetViewBlocked;

	// Token: 0x04002492 RID: 9362
	private bool targetingStalker;

	// Token: 0x04002493 RID: 9363
	public float spawnAttackDelay = 0.5f;
}
