using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002F7 RID: 759
public class MinosPrime : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x060010E8 RID: 4328 RVA: 0x00082506 File Offset: 0x00080706
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x00082514 File Offset: 0x00080714
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.mach = base.GetComponent<Machine>();
		this.gce = base.GetComponentInChildren<GroundCheckEnemy>();
		this.rb = base.GetComponent<Rigidbody>();
		this.sc = base.GetComponentInChildren<SwingCheck2>();
		this.col = base.GetComponent<Collider>();
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x00082584 File Offset: 0x00080784
	private void Start()
	{
		this.SetSpeed();
		this.head = this.eid.weakPoint.transform;
		this.originalHp = this.mach.health;
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		this.spawnPoint = base.transform.position;
		BossHealthBar bossHealthBar;
		this.bossVersion = base.TryGetComponent<BossHealthBar>(out bossHealthBar);
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x000825EE File Offset: 0x000807EE
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x000825F8 File Offset: 0x000807F8
	private void SetSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
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
			this.anim.speed = 0.9f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.8f;
		}
		else if (this.difficulty == 5)
		{
			this.anim.speed = 1.25f;
		}
		else if (this.difficulty == 4)
		{
			this.anim.speed = 1.125f;
		}
		else
		{
			this.anim.speed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00082710 File Offset: 0x00080910
	private void OnDisable()
	{
		if (!this.mach)
		{
			return;
		}
		base.CancelInvoke();
		this.StopAction();
		this.DamageStop();
		this.uppercutting = false;
		this.ascending = false;
		this.tracking = false;
		this.fullTracking = false;
		this.aiming = false;
		this.jumping = false;
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x00082767 File Offset: 0x00080967
	private void OnEnable()
	{
		if (!this.activated)
		{
			this.OutroEnd();
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x00082778 File Offset: 0x00080978
	private void Update()
	{
		if (this.target == null)
		{
			return;
		}
		if (this.activated)
		{
			this.playerPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
			if (!this.inAction)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (!this.enraged && this.mach.health < this.originalHp / 2f)
			{
				this.enraged = true;
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("WEAK", null, false);
				this.aud.clip = this.phaseChangeVoice;
				this.aud.pitch = 1f;
				this.aud.Play();
				this.currentPassiveEffect = Object.Instantiate<GameObject>(this.passiveEffect, base.transform.position + Vector3.up * 3.5f, Quaternion.identity);
				this.currentPassiveEffect.transform.SetParent(base.transform);
				foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
				{
					Object.Instantiate<GameObject>(this.flameEffect, enemyIdentifierIdentifier.transform);
				}
				Object.Instantiate<GameObject>(this.phaseChangeEffect, this.mach.chest.transform.position, Quaternion.identity);
				return;
			}
		}
		else
		{
			if (this.ascending)
			{
				this.rb.velocity = Vector3.MoveTowards(this.rb.velocity, Vector3.up * 3f, Time.deltaTime);
				MonoSingleton<CameraController>.Instance.CameraShake(0.1f);
				return;
			}
			if (this.vibrating)
			{
				base.transform.position = new Vector3(this.origPos.x + Random.Range(-0.1f, 0.1f), this.origPos.y + Random.Range(-0.1f, 0.1f), this.origPos.z + Random.Range(-0.1f, 0.1f));
			}
		}
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x000829BC File Offset: 0x00080BBC
	private void FixedUpdate()
	{
		if (this.activated)
		{
			this.CustomPhysics();
			if (this.target == null)
			{
				this.anim.SetBool("Walking", false);
				if (this.nma.enabled && this.nma.isOnNavMesh)
				{
					this.nma.isStopped = true;
				}
				return;
			}
			if (!this.inAction && this.gce.onGround && this.nma && this.nma.enabled && this.nma.isOnNavMesh && Vector3.Distance(base.transform.position, this.playerPos) > 2.5f)
			{
				this.nma.isStopped = false;
				if (MonoSingleton<NewMovement>.Instance.gc && !MonoSingleton<NewMovement>.Instance.gc.onGround)
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(this.target.position, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.nma.SetDestination(raycastHit.point);
					}
					else
					{
						this.nma.SetDestination(this.target.position);
					}
				}
				else
				{
					this.nma.SetDestination(this.target.position);
				}
			}
			else if (this.inAction)
			{
				if (this.nma)
				{
					this.nma.enabled = false;
				}
				this.anim.SetBool("Walking", false);
			}
			if (this.tracking || this.fullTracking)
			{
				if (!this.fullTracking)
				{
					base.transform.LookAt(this.playerPos);
				}
				else
				{
					base.transform.rotation = Quaternion.LookRotation(this.target.position - new Vector3(base.transform.position.x, this.aimingBone.position.y, base.transform.position.z));
				}
			}
			if (this.nma && this.nma.enabled && this.nma.isOnNavMesh && !this.inAction)
			{
				if (this.nma.velocity.magnitude > 2f)
				{
					this.anim.SetBool("Walking", true);
					return;
				}
				this.anim.SetBool("Walking", false);
			}
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x00082C44 File Offset: 0x00080E44
	private void LateUpdate()
	{
		if (this.aiming && this.inAction && this.activated && this.target != null)
		{
			this.aimingBone.LookAt(this.target.position);
			this.aimingBone.Rotate(Vector3.up * -90f, Space.Self);
		}
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00082CA4 File Offset: 0x00080EA4
	private void CustomPhysics()
	{
		if ((this.difficulty == 3 && !this.enraged && this.attackAmount >= 10) || (this.difficulty <= 2 && (((this.difficulty <= 1 || !this.enraged) && this.attackAmount >= 6) || this.attackAmount >= 12)))
		{
			this.attackAmount = 0;
			if (this.difficulty == 1)
			{
				this.cooldown = 3f;
			}
			else if (this.difficulty == 0)
			{
				this.cooldown = 4f;
			}
			else
			{
				this.cooldown = 2f;
			}
		}
		if (!this.inAction)
		{
			this.gravityInAction = false;
			if (this.gce.onGround && !this.jumping)
			{
				this.nma.enabled = true;
				this.rb.isKinematic = true;
				this.hasRiderKicked = false;
				this.hasProjectiled = false;
				this.downSwingAmount = 0;
				if (this.cooldown <= 0f && !this.anim.IsInTransition(0) && this.target != null)
				{
					float num = Vector3.Distance(base.transform.position, this.playerPos);
					if (!Physics.Raycast(this.target.position, Vector3.down, 6f, LayerMaskDefaults.Get(LMD.Environment)) || MonoSingleton<NewMovement>.Instance.rb.velocity.y > 0f)
					{
						if (num < 25f && this.lastAttack != MPAttack.Jump)
						{
							if (this.activated)
							{
								this.Jump();
							}
							this.lastAttack = MPAttack.Jump;
						}
						else if (num > 25f && this.lastAttack != MPAttack.ProjectilePunch)
						{
							if (this.activated)
							{
								this.ProjectilePunch();
							}
							this.lastAttack = MPAttack.ProjectilePunch;
						}
						else if (this.activated)
						{
							int num2 = Random.Range(0, 4);
							this.PickAttack(num2);
						}
					}
					else if (this.activated)
					{
						int num3 = Random.Range(0, 4);
						this.PickAttack(num3);
					}
				}
			}
			else
			{
				this.nma.enabled = false;
				this.rb.isKinematic = false;
				if (this.rb.velocity.y < 0f && !this.anim.IsInTransition(0) && this.activated && this.target != null)
				{
					if (!this.hasProjectiled && Random.Range(0f, 1f) < 0.25f && this.enraged && Vector3.Distance(this.playerPos, base.transform.position) > 6f && !Physics.Raycast(base.transform.position, Vector3.down, 4f, LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.hasProjectiled = true;
						this.ProjectilePunch();
					}
					else if (Vector3.Distance(this.playerPos, base.transform.position) < 5f)
					{
						if (this.target.position.y < base.transform.position.y)
						{
							this.DropAttack();
						}
						else if (this.target.position.y < base.transform.position.y + 10f && this.downSwingAmount < 2)
						{
							this.DownSwing();
						}
					}
					else if (Vector3.Angle(Vector3.up, this.target.position - base.transform.position) > 90f || Vector3.Distance(base.transform.position, this.target.position) < 10f || this.ignoreRiderkickAngle)
					{
						if (this.previouslyRiderKicked && this.downSwingAmount < 2)
						{
							this.TeleportAnywhere();
							this.DownSwing();
							this.hasRiderKicked = true;
						}
						else if (!this.hasRiderKicked)
						{
							this.RiderKick();
						}
					}
					this.ignoreRiderkickAngle = false;
				}
			}
		}
		else
		{
			this.nma.enabled = false;
			if (this.gravityInAction)
			{
				this.rb.isKinematic = false;
			}
			else
			{
				this.rb.isKinematic = true;
			}
			if (this.swinging && !Physics.Raycast(base.transform.position, base.transform.forward, 1f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				if (MonoSingleton<NewMovement>.Instance.sliding)
				{
					this.rb.MovePosition(base.transform.position + base.transform.forward * 125f * Time.fixedDeltaTime * this.eid.totalSpeedModifier);
				}
				else
				{
					this.rb.MovePosition(base.transform.position + base.transform.forward * 75f * Time.fixedDeltaTime * this.eid.totalSpeedModifier);
				}
			}
		}
		if (!this.rb.isKinematic && this.rb.useGravity)
		{
			this.rb.velocity -= Vector3.up * 100f * Time.fixedDeltaTime;
		}
		if (this.jumping)
		{
			if (!this.inAction && this.rb.velocity.y < 0f)
			{
				this.jumping = false;
				if (this.uppercutting)
				{
					this.uppercutting = false;
					this.DamageStop();
					if (this.hitSuccessful && this.target != null && this.target.position.y > base.transform.position.y && this.activated)
					{
						this.Jump();
						this.hitSuccessful = false;
					}
				}
			}
			return;
		}
		if (!this.rb.isKinematic)
		{
			this.anim.SetBool("Falling", true);
			return;
		}
		this.anim.SetBool("Falling", false);
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x000832B0 File Offset: 0x000814B0
	private void PickAttack(int type)
	{
		if (this.attacksSinceBoxing >= 5)
		{
			type = 0;
		}
		switch (type)
		{
		case 0:
			if (this.lastAttack != MPAttack.Boxing)
			{
				this.Boxing();
				this.lastAttack = MPAttack.Boxing;
				this.attacksSinceBoxing = 0;
				return;
			}
			this.PickAttack(type + 1);
			return;
		case 1:
			if (this.lastAttack != MPAttack.Combo)
			{
				this.Combo();
				this.lastAttack = MPAttack.Combo;
				this.attacksSinceBoxing++;
				return;
			}
			this.PickAttack(type + 1);
			return;
		case 2:
			if (this.lastAttack != MPAttack.Dropkick)
			{
				this.Dropkick();
				this.lastAttack = MPAttack.Dropkick;
				this.attacksSinceBoxing++;
				return;
			}
			this.PickAttack(type + 1);
			return;
		case 3:
			this.Uppercut();
			this.attacksSinceBoxing++;
			return;
		default:
			return;
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0008337C File Offset: 0x0008157C
	private void Dropkick()
	{
		this.inAction = true;
		if (this.nma && this.nma.isOnNavMesh)
		{
			this.nma.isStopped = true;
		}
		this.anim.Play("Dropkick", 0, 0f);
		this.tracking = true;
		this.fullTracking = false;
		this.sc.knockBackForce = 100f;
		this.aiming = false;
		this.attackAmount += 2;
		if (!this.enraged)
		{
			this.cooldown += 1.25f;
		}
		else
		{
			this.cooldown += 0.25f;
		}
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Judgement!", null, false);
		this.PlayVoice(this.dropkickVoice);
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0008344C File Offset: 0x0008164C
	private void ProjectilePunch()
	{
		this.inAction = true;
		if (this.nma && this.nma.isOnNavMesh)
		{
			this.nma.isStopped = true;
		}
		this.tracking = true;
		this.fullTracking = false;
		this.aiming = true;
		this.anim.Play("ProjectilePunch", 0, 0f);
		this.ProjectileCharge();
		this.aiming = false;
		this.attackAmount++;
		this.PlayVoice(this.projectileVoice);
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x000834D8 File Offset: 0x000816D8
	private void Jump()
	{
		this.inAction = true;
		base.transform.LookAt(this.playerPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = true;
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.jumping = true;
		this.anim.SetBool("Falling", false);
		this.anim.Play("Jump", 0, 0f);
		base.Invoke("StopAction", 0.1f);
		this.rb.AddForce(Vector3.up * 100f, ForceMode.VelocityChange);
		Object.Instantiate<GameObject>(this.swoosh, base.transform.position, Quaternion.identity);
		this.aiming = false;
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x000835A8 File Offset: 0x000817A8
	private void Uppercut()
	{
		this.hitSuccessful = false;
		this.inAction = true;
		base.transform.LookAt(this.playerPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("Uppercut", 0, 0f);
		this.anim.SetBool("Falling", false);
		Object.Instantiate<GameObject>(this.warningFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 5f;
		this.sc.knockBackForce = 100f;
		this.aiming = false;
		this.attackAmount++;
		this.PlayVoice(this.uppercutVoice);
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x00083698 File Offset: 0x00081898
	private void RiderKick()
	{
		if (this.target == null)
		{
			return;
		}
		this.downSwingAmount = 0;
		this.previouslyRiderKicked = true;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.tracking = true;
		this.fullTracking = true;
		this.gravityInAction = false;
		this.anim.SetTrigger("RiderKick");
		if (this.difficulty >= 2)
		{
			base.Invoke("StopTracking", 0.5f / this.anim.speed);
		}
		else
		{
			base.Invoke("StopTracking", 0.25f / this.anim.speed);
		}
		base.Invoke("RiderKickActivate", 0.75f / this.anim.speed);
		Object.Instantiate<GameObject>(this.warningFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 5f;
		this.sc.knockBackForce = 50f;
		this.aiming = false;
		this.attackAmount++;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Die!", null, false);
		this.PlayVoice(this.riderKickVoice);
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x000837F4 File Offset: 0x000819F4
	private void DropAttack()
	{
		this.downSwingAmount = 0;
		this.tracking = true;
		this.fullTracking = false;
		this.ResetRotation();
		this.inAction = true;
		base.transform.LookAt(this.playerPos);
		this.gravityInAction = false;
		this.anim.SetTrigger("DropAttack");
		base.Invoke("DropAttackActivate", 0.75f / this.anim.speed);
		Object.Instantiate<GameObject>(this.warningFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 5f;
		this.sc.knockBackForce = 50f;
		this.aiming = false;
		this.attackAmount++;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Crush!", null, false);
		this.PlayVoice(this.dropAttackVoice);
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x00083900 File Offset: 0x00081B00
	private void DownSwing()
	{
		if (this.target == null)
		{
			return;
		}
		this.downSwingAmount++;
		this.previouslyRiderKicked = false;
		this.tracking = true;
		this.fullTracking = true;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.gravityInAction = false;
		this.anim.SetTrigger("DownSwing");
		Object.Instantiate<GameObject>(this.warningFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 5f;
		this.sc.knockBackForce = 100f;
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = Vector3.down;
		this.aiming = false;
		this.attackAmount++;
		this.PlayVoice(this.overheadVoice);
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x00083A10 File Offset: 0x00081C10
	public void UppercutActivate()
	{
		base.transform.LookAt(this.playerPos);
		this.uppercutting = true;
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = true;
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.jumping = true;
		this.anim.SetBool("Falling", false);
		base.Invoke("StopAction", 0.1f);
		this.rb.AddForce(Vector3.up * 100f, ForceMode.VelocityChange);
		Object.Instantiate<GameObject>(this.swoosh, base.transform.position, Quaternion.identity);
		Transform child = Object.Instantiate<GameObject>(this.swingSnake, this.aimingBone.position + base.transform.forward * 4f, Quaternion.identity).transform.GetChild(0);
		child.SetParent(base.transform, true);
		child.rotation = Quaternion.LookRotation(Vector3.up);
		this.currentSwingSnakes.Add(child.gameObject);
		SwingCheck2[] componentsInChildren = child.GetComponentsInChildren<SwingCheck2>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].OverrideEnemyIdentifier(this.eid);
		}
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = Vector3.up;
		this.DamageStart();
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x00083B78 File Offset: 0x00081D78
	public void UppercutCancel(int parryable = 0)
	{
		if (this.target == null)
		{
			return;
		}
		if (this.target.position.y > base.transform.position.y + 5f)
		{
			this.DamageStop();
			this.Uppercut();
			return;
		}
		if (parryable == 1)
		{
			this.Parryable();
		}
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x00083BD0 File Offset: 0x00081DD0
	public void Combo()
	{
		if (this.previousCombo == MPAttack.Combo)
		{
			this.Boxing();
			return;
		}
		this.previousCombo = MPAttack.Combo;
		this.inAction = true;
		base.transform.LookAt(this.playerPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("Combo", 0, 0f);
		this.sc.knockBackForce = 50f;
		this.aiming = false;
		this.attackAmount += 3;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Prepare thyself!", null, false);
		this.PlayVoice(this.comboVoice);
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x00083C7C File Offset: 0x00081E7C
	public void Boxing()
	{
		if (this.previousCombo == MPAttack.Boxing)
		{
			this.Combo();
			return;
		}
		this.previousCombo = MPAttack.Boxing;
		this.inAction = true;
		base.transform.LookAt(this.playerPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("Boxing", 0, 0f);
		this.sc.knockBackForce = 30f;
		this.aiming = false;
		this.attackAmount += 2;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Thy end is now!", null, false);
		this.PlayVoice(this.boxingVoice);
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x00083D24 File Offset: 0x00081F24
	private void RiderKickActivate()
	{
		RaycastHit raycastHit;
		Physics.Raycast(this.aimingBone.position, base.transform.forward, out raycastHit, 250f, LayerMaskDefaults.Get(LMD.Environment));
		LineRenderer component = Object.Instantiate<GameObject>(this.attackTrail, this.aimingBone.position, base.transform.rotation).GetComponent<LineRenderer>();
		component.SetPosition(0, this.aimingBone.position);
		RaycastHit[] array = Physics.SphereCastAll(this.aimingBone.position, 5f, base.transform.forward, Vector3.Distance(this.aimingBone.position, raycastHit.point), LayerMaskDefaults.Get(LMD.EnemiesAndPlayer));
		bool flag = false;
		new List<EnemyIdentifier>();
		foreach (RaycastHit raycastHit2 in array)
		{
			if (!flag && raycastHit2.collider.gameObject.CompareTag("Player"))
			{
				flag = true;
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(30f * this.eid.totalDamageModifier), true, 1f, false, false, 0.35f, false);
				MonoSingleton<NewMovement>.Instance.LaunchFromPoint(MonoSingleton<NewMovement>.Instance.transform.position + base.transform.forward * -1f, 100f, 100f);
			}
		}
		if (Vector3.Angle(Vector3.up, raycastHit.normal) < 35f)
		{
			this.ResetRotation();
			base.transform.position = raycastHit.point;
			this.anim.Play("DropRecovery", 0, 0f);
		}
		else if (Vector3.Angle(Vector3.up, raycastHit.normal) < 145f)
		{
			base.transform.position = raycastHit.point - base.transform.forward;
			this.ResetRotation();
			this.inAction = false;
			this.hasRiderKicked = true;
			this.anim.Play("Falling", 0, 0f);
		}
		else
		{
			base.transform.position = raycastHit.point - Vector3.up * 6.5f;
			this.ResetRotation();
			this.inAction = false;
			this.hasRiderKicked = true;
			this.anim.Play("Falling", 0, 0f);
		}
		this.ResolveStuckness();
		component.SetPosition(1, this.aimingBone.position);
		GameObject gameObject = Object.Instantiate<GameObject>(this.bigRubble, raycastHit.point, Quaternion.identity);
		if (Vector3.Angle(raycastHit.normal, Vector3.up) < 5f)
		{
			gameObject.transform.LookAt(new Vector3(gameObject.transform.position.x + base.transform.forward.x, gameObject.transform.position.y, gameObject.transform.position.z + base.transform.forward.z));
		}
		else
		{
			gameObject.transform.up = raycastHit.normal;
		}
		if (this.difficulty >= 2)
		{
			gameObject = Object.Instantiate<GameObject>(this.groundWave, raycastHit.point, Quaternion.identity);
			gameObject.transform.up = raycastHit.normal;
			gameObject.transform.SetParent(this.gz.transform);
			PhysicalShockwave physicalShockwave;
			if (gameObject.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
			{
				physicalShockwave.enemyType = EnemyType.MinosPrime;
				physicalShockwave.damage = Mathf.RoundToInt((float)physicalShockwave.damage * this.eid.totalDamageModifier);
			}
		}
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x000840D0 File Offset: 0x000822D0
	private void DropAttackActivate()
	{
		RaycastHit raycastHit;
		Physics.Raycast(this.aimingBone.position, Vector3.down, out raycastHit, 250f, LayerMaskDefaults.Get(LMD.Environment));
		LineRenderer component = Object.Instantiate<GameObject>(this.attackTrail, this.aimingBone.position, base.transform.rotation).GetComponent<LineRenderer>();
		component.SetPosition(0, this.aimingBone.position);
		RaycastHit[] array = Physics.SphereCastAll(this.aimingBone.position, 5f, Vector3.down, Vector3.Distance(this.aimingBone.position, raycastHit.point), LayerMaskDefaults.Get(LMD.EnemiesAndPlayer));
		bool flag = false;
		new List<EnemyIdentifier>();
		foreach (RaycastHit raycastHit2 in array)
		{
			if (!flag && raycastHit2.collider.gameObject.CompareTag("Player"))
			{
				flag = true;
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(50f * this.eid.totalDamageModifier), true, 1f, false, false, 0.35f, false);
				MonoSingleton<NewMovement>.Instance.LaunchFromPoint(MonoSingleton<NewMovement>.Instance.transform.position + (MonoSingleton<NewMovement>.Instance.transform.position - new Vector3(raycastHit2.point.x, MonoSingleton<NewMovement>.Instance.transform.position.y, raycastHit2.point.z)).normalized, 100f, 100f);
			}
		}
		base.transform.position = raycastHit.point;
		this.anim.Play("DropRecovery", 0, 0f);
		component.SetPosition(1, this.aimingBone.position);
		GameObject gameObject = Object.Instantiate<GameObject>(this.bigRubble, raycastHit.point, Quaternion.identity);
		if (Vector3.Angle(raycastHit.normal, Vector3.up) < 5f)
		{
			gameObject.transform.LookAt(new Vector3(gameObject.transform.position.x + base.transform.forward.x, gameObject.transform.position.y, gameObject.transform.position.z + base.transform.forward.z));
		}
		else
		{
			gameObject.transform.up = raycastHit.normal;
		}
		if (this.difficulty >= 2)
		{
			gameObject = Object.Instantiate<GameObject>(this.groundWave, raycastHit.point, Quaternion.identity);
			gameObject.transform.up = raycastHit.normal;
			gameObject.transform.SetParent(this.gz.transform);
			PhysicalShockwave physicalShockwave;
			if (gameObject.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
			{
				physicalShockwave.enemyType = EnemyType.MinosPrime;
				physicalShockwave.damage = Mathf.RoundToInt((float)physicalShockwave.damage * this.eid.totalDamageModifier);
			}
		}
		this.Explosion();
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x000843D4 File Offset: 0x000825D4
	public void SnakeSwingStart(int limb)
	{
		Transform child = Object.Instantiate<GameObject>(this.swingSnake, this.aimingBone.position + base.transform.forward * 4f, Quaternion.identity).transform.GetChild(0);
		child.SetParent(base.transform, true);
		child.LookAt(this.playerPos);
		this.currentSwingSnakes.Add(child.gameObject);
		if (!this.boxing)
		{
			this.swinging = true;
		}
		SwingCheck2 swingCheck;
		if (child.TryGetComponent<SwingCheck2>(out swingCheck))
		{
			swingCheck.OverrideEnemyIdentifier(this.eid);
			swingCheck.knockBackDirectionOverride = true;
			if (this.sc.knockBackDirectionOverride)
			{
				swingCheck.knockBackDirection = this.sc.knockBackDirection;
			}
			else
			{
				swingCheck.knockBackDirection = base.transform.forward;
			}
			swingCheck.knockBackForce = this.sc.knockBackForce;
		}
		AttackTrail attackTrail;
		if (child.TryGetComponent<AttackTrail>(out attackTrail))
		{
			attackTrail.target = this.swingLimbs[limb];
			attackTrail.pivot = this.aimingBone;
		}
		this.DamageStart();
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x000844E4 File Offset: 0x000826E4
	public void DamageStart()
	{
		this.sc.DamageStart();
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x000844F4 File Offset: 0x000826F4
	public void DamageStop()
	{
		this.swinging = false;
		this.sc.DamageStop();
		this.sc.knockBackDirectionOverride = false;
		this.mach.parryable = false;
		if (this.currentSwingSnakes.Count > 0)
		{
			for (int i = this.currentSwingSnakes.Count - 1; i >= 0; i--)
			{
				SwingCheck2 swingCheck;
				if (this.currentSwingSnakes[i].TryGetComponent<SwingCheck2>(out swingCheck))
				{
					swingCheck.DamageStop();
				}
				AttackTrail attackTrail;
				if (base.gameObject.activeInHierarchy && this.currentSwingSnakes[i].TryGetComponent<AttackTrail>(out attackTrail))
				{
					attackTrail.DelayedDestroy(0.5f);
					this.currentSwingSnakes[i].transform.parent = null;
					attackTrail.target = null;
					attackTrail.pivot = null;
				}
				else
				{
					Object.Destroy(this.currentSwingSnakes[i]);
				}
			}
			this.currentSwingSnakes.Clear();
		}
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x000845E8 File Offset: 0x000827E8
	public void Explosion()
	{
		if (this.gotParried && this.difficulty <= 2 && !this.enraged)
		{
			this.gotParried = false;
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, base.transform.position, Quaternion.identity);
		this.mach.parryable = false;
		if (this.difficulty <= 1 || this.eid.totalDamageModifier != 1f)
		{
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				if (this.difficulty == 1)
				{
					explosion.speed *= 0.6f;
					explosion.maxSize *= 0.75f;
				}
				else if (this.difficulty == 0)
				{
					explosion.speed *= 0.5f;
					explosion.maxSize *= 0.5f;
				}
				explosion.speed *= this.eid.totalDamageModifier;
				explosion.maxSize *= this.eid.totalDamageModifier;
				explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.eid.totalDamageModifier);
			}
		}
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00084724 File Offset: 0x00082924
	public void ProjectileCharge()
	{
		Object.Instantiate<GameObject>(this.projectileCharge, this.swingLimbs[0].position, this.swingLimbs[0].rotation).transform.SetParent(this.swingLimbs[0]);
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x00084760 File Offset: 0x00082960
	public void ProjectileShoot()
	{
		if (this.target == null)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.snakeProjectile, this.mach.chest.transform.position, Quaternion.LookRotation(this.target.position - (base.transform.position + Vector3.up)));
		gameObject.transform.SetParent(this.gz.transform);
		Projectile componentInChildren = gameObject.GetComponentInChildren<Projectile>();
		if (componentInChildren)
		{
			componentInChildren.target = (this.target.isPlayer ? new EnemyTarget(MonoSingleton<CameraController>.Instance.transform) : this.target);
			componentInChildren.damage *= this.eid.totalDamageModifier;
		}
		this.aiming = false;
		this.tracking = false;
		this.fullTracking = false;
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0008483B File Offset: 0x00082A3B
	public void TeleportOnGround()
	{
		this.Teleport(this.playerPos, base.transform.position);
		base.transform.LookAt(this.playerPos);
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x00084865 File Offset: 0x00082A65
	public void TeleportAnywhere()
	{
		if (this.target == null)
		{
			return;
		}
		this.Teleport(this.target.position, base.transform.position);
		base.transform.LookAt(this.target.position);
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x000848A4 File Offset: 0x00082AA4
	public void TeleportSide(int side)
	{
		if (this.target == null)
		{
			return;
		}
		int num = 1;
		this.boxing = true;
		if (side == 0)
		{
			num = -1;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(this.playerPos + Vector3.up, this.target.right * (float)num + this.target.forward, out raycastHit, 4f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
		{
			if (raycastHit.distance >= 2f)
			{
				this.Teleport(this.playerPos, this.playerPos + Vector3.ClampMagnitude(this.target.right * (float)num + this.target.forward, 1f) * (raycastHit.distance - 1f));
			}
			else
			{
				this.Teleport(this.playerPos, base.transform.position);
			}
			base.transform.LookAt(this.playerPos);
			return;
		}
		this.Teleport(this.playerPos, this.playerPos + (this.target.right * (float)num + this.target.forward) * 10f);
		base.transform.LookAt(this.playerPos);
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000849F8 File Offset: 0x00082BF8
	public void Teleport(Vector3 teleportTarget, Vector3 startPos)
	{
		float num = Vector3.Distance(teleportTarget, startPos);
		if (this.boxing && num > 2.5f)
		{
			num = 2.5f;
		}
		else if (num > 3f)
		{
			num = 3f;
		}
		Vector3 vector = teleportTarget + (startPos - teleportTarget).normalized * num;
		Collider[] array = Physics.OverlapCapsule(vector + base.transform.up * 0.75f, vector + base.transform.up * 5.25f, 0.75f, LayerMaskDefaults.Get(LMD.Environment));
		if (array != null && array.Length != 0)
		{
			for (int i = 0; i < 6; i++)
			{
				Collider collider = array[0];
				Vector3 vector2;
				float num2;
				if (!Physics.ComputePenetration(this.col, vector + base.transform.up * 3f, base.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector2, out num2))
				{
					break;
				}
				vector += vector2 * num2;
				array = Physics.OverlapCapsule(vector + base.transform.up * 0.75f, vector + base.transform.up * 5.25f, 0.75f, LayerMaskDefaults.Get(LMD.Environment));
				if (array == null || array.Length == 0)
				{
					break;
				}
				if (i == 5)
				{
					this.ResolveStuckness();
					break;
				}
			}
		}
		float num3 = Vector3.Distance(base.transform.position, vector);
		int num4 = 0;
		while ((float)num4 < num3)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(Vector3.Lerp(base.transform.position, vector, (num3 - (float)num4) / num3) + Vector3.up, Vector3.down, out raycastHit, 3f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Object.Instantiate<GameObject>(this.rubble, raycastHit.point, Quaternion.Euler(0f, (float)Random.Range(0, 360), 0f));
			}
			num4 += 3;
		}
		MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
		base.transform.position = vector;
		this.tracking = false;
		this.fullTracking = false;
		Object.Instantiate<GameObject>(this.swoosh, base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x00084C68 File Offset: 0x00082E68
	public void Death()
	{
		this.anim.Play("Outro");
		this.anim.SetBool("Dead", true);
		if (this.bossVersion)
		{
			this.anim.speed = 1f;
		}
		else
		{
			this.anim.speed = 5f;
		}
		this.activated = false;
		if (this.currentPassiveEffect)
		{
			Object.Destroy(this.currentPassiveEffect);
		}
		base.CancelInvoke();
		this.DamageStop();
		Object.Destroy(this.nma);
		this.DisableGravity();
		this.rb.useGravity = false;
		this.rb.isKinematic = true;
		MonoSingleton<TimeController>.Instance.SlowDown(0.0001f);
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x00084D24 File Offset: 0x00082F24
	public void Ascend()
	{
		if (!this.bossVersion)
		{
			this.OutroEnd();
			return;
		}
		this.rb.isKinematic = false;
		this.rb.constraints = (RigidbodyConstraints)122;
		this.ascending = true;
		this.LightShaft();
		base.Invoke("LightShaft", 1.5f);
		base.Invoke("LightShaft", 3f);
		base.Invoke("LightShaft", 4f);
		base.Invoke("LightShaft", 5f);
		base.Invoke("LightShaft", 5.5f);
		base.Invoke("LightShaft", 6f);
		base.Invoke("LightShaft", 6.25f);
		base.Invoke("LightShaft", 6.5f);
		base.Invoke("LightShaft", 6.7f);
		base.Invoke("LightShaft", 6.8f);
		base.Invoke("LightShaft", 6.85f);
		base.Invoke("LightShaft", 6.9f);
		base.Invoke("LightShaft", 6.925f);
		base.Invoke("LightShaft", 6.95f);
		base.Invoke("LightShaft", 6.975f);
		base.Invoke("OutroEnd", 7f);
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x00084E68 File Offset: 0x00083068
	private void LightShaft()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.lightShaft, this.mach.chest.transform.position, Random.rotation).transform.SetParent(base.transform, true);
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x00084EC8 File Offset: 0x000830C8
	public void OutroEnd()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.onOutroEnd.Invoke("");
		Object.Instantiate<GameObject>(this.outroExplosion, this.mach.chest.transform.position, Quaternion.identity);
		base.gameObject.SetActive(false);
		MonoSingleton<TimeController>.Instance.SlowDown(0.01f);
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x00084F34 File Offset: 0x00083134
	public void EnableGravity(int earlyCancel)
	{
		if (!this.gce.onGround)
		{
			this.anim.SetBool("Falling", true);
			this.gravityInAction = true;
			if (earlyCancel == 1)
			{
				this.inAction = false;
			}
		}
		this.ResetRotation();
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x00084F6C File Offset: 0x0008316C
	public void Parryable()
	{
		if (this.difficulty <= 2 || !this.enraged)
		{
			this.gotParried = false;
			Object.Instantiate<GameObject>(this.parryableFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 10f;
			this.mach.ParryableCheck(false);
		}
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x00084FF1 File Offset: 0x000831F1
	public void GotParried()
	{
		this.PlayVoice(this.hurtVoice);
		this.attackAmount -= 5;
		this.gotParried = true;
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00085014 File Offset: 0x00083214
	public void Rubble()
	{
		Object.Instantiate<GameObject>(this.bigRubble, base.transform.position + base.transform.forward, base.transform.rotation);
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x00085048 File Offset: 0x00083248
	public void ResetRotation()
	{
		base.transform.LookAt(new Vector3(base.transform.position.x + base.transform.forward.x, base.transform.position.y, base.transform.position.z + base.transform.forward.z));
		this.ResolveStuckness();
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x000850BD File Offset: 0x000832BD
	public void DisableGravity()
	{
		this.gravityInAction = false;
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x000850C6 File Offset: 0x000832C6
	public void StopTracking()
	{
		this.tracking = false;
		this.fullTracking = false;
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x000850D6 File Offset: 0x000832D6
	public void StopAction()
	{
		this.gotParried = false;
		this.inAction = false;
		this.boxing = false;
		if (this.mach)
		{
			this.mach.parryable = false;
		}
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x00085108 File Offset: 0x00083308
	public void TargetBeenHit()
	{
		this.sc.DamageStop();
		this.hitSuccessful = true;
		this.mach.parryable = false;
		if (this.uppercutting)
		{
			this.ignoreRiderkickAngle = true;
		}
		foreach (GameObject gameObject in this.currentSwingSnakes)
		{
			SwingCheck2 swingCheck;
			if (gameObject && gameObject.TryGetComponent<SwingCheck2>(out swingCheck))
			{
				swingCheck.DamageStop();
			}
		}
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x0008519C File Offset: 0x0008339C
	public void OutOfBounds()
	{
		base.transform.position = this.spawnPoint;
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x000851AF File Offset: 0x000833AF
	public void Vibrate()
	{
		this.origPos = base.transform.position;
		this.vibrating = true;
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x000851CC File Offset: 0x000833CC
	public void PlayVoice(AudioClip[] voice)
	{
		if (voice.Length == 0 || (this.aud.clip == this.phaseChangeVoice && this.aud.isPlaying))
		{
			return;
		}
		this.aud.clip = voice[Random.Range(0, voice.Length)];
		this.aud.pitch = Random.Range(0.95f, 1f);
		this.aud.Play();
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x00085240 File Offset: 0x00083440
	public void ResolveStuckness()
	{
		Collider[] array = Physics.OverlapCapsule(base.transform.position + base.transform.up * 0.76f, base.transform.position + base.transform.up * 5.24f, 0.74f, LayerMaskDefaults.Get(LMD.Environment));
		if (array != null && array.Length != 0)
		{
			if (this.gce.onGround)
			{
				this.gce.onGround = false;
				this.nma.enabled = false;
			}
			for (int i = 0; i < 6; i++)
			{
				RaycastHit[] array2 = Physics.CapsuleCastAll(this.spawnPoint + base.transform.up * 0.75f, this.spawnPoint + base.transform.up * 5.25f, 0.75f, base.transform.position - this.spawnPoint, Vector3.Distance(this.spawnPoint, base.transform.position), LayerMaskDefaults.Get(LMD.Environment));
				if (array2 == null || array2.Length == 0)
				{
					break;
				}
				foreach (RaycastHit raycastHit in array2)
				{
					bool flag = false;
					Collider[] array4 = array;
					for (int k = 0; k < array4.Length; k++)
					{
						if (array4[k] == raycastHit.collider)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						base.transform.position = this.spawnPoint + (base.transform.position - this.spawnPoint).normalized * raycastHit.distance + raycastHit.normal * 0.1f;
						break;
					}
				}
			}
		}
	}

	// Token: 0x04001716 RID: 5910
	private NavMeshAgent nma;

	// Token: 0x04001717 RID: 5911
	private Animator anim;

	// Token: 0x04001718 RID: 5912
	private Machine mach;

	// Token: 0x04001719 RID: 5913
	private EnemyIdentifier eid;

	// Token: 0x0400171A RID: 5914
	private GroundCheckEnemy gce;

	// Token: 0x0400171B RID: 5915
	private Rigidbody rb;

	// Token: 0x0400171C RID: 5916
	private Collider col;

	// Token: 0x0400171D RID: 5917
	private AudioSource aud;

	// Token: 0x0400171E RID: 5918
	private float originalHp;

	// Token: 0x0400171F RID: 5919
	private bool inAction;

	// Token: 0x04001720 RID: 5920
	private float cooldown = 2f;

	// Token: 0x04001721 RID: 5921
	private MPAttack lastAttack;

	// Token: 0x04001722 RID: 5922
	private Vector3 playerPos;

	// Token: 0x04001723 RID: 5923
	private bool tracking;

	// Token: 0x04001724 RID: 5924
	private bool fullTracking;

	// Token: 0x04001725 RID: 5925
	private bool aiming;

	// Token: 0x04001726 RID: 5926
	private bool jumping;

	// Token: 0x04001727 RID: 5927
	public GameObject explosion;

	// Token: 0x04001728 RID: 5928
	public GameObject rubble;

	// Token: 0x04001729 RID: 5929
	public GameObject bigRubble;

	// Token: 0x0400172A RID: 5930
	public GameObject groundWave;

	// Token: 0x0400172B RID: 5931
	public GameObject swoosh;

	// Token: 0x0400172C RID: 5932
	public Transform aimingBone;

	// Token: 0x0400172D RID: 5933
	private Transform head;

	// Token: 0x0400172E RID: 5934
	public GameObject projectileCharge;

	// Token: 0x0400172F RID: 5935
	public GameObject snakeProjectile;

	// Token: 0x04001730 RID: 5936
	private bool hasProjectiled;

	// Token: 0x04001731 RID: 5937
	public GameObject warningFlash;

	// Token: 0x04001732 RID: 5938
	public GameObject parryableFlash;

	// Token: 0x04001733 RID: 5939
	private bool gravityInAction;

	// Token: 0x04001734 RID: 5940
	private bool hasRiderKicked;

	// Token: 0x04001735 RID: 5941
	private bool previouslyRiderKicked;

	// Token: 0x04001736 RID: 5942
	private int downSwingAmount;

	// Token: 0x04001737 RID: 5943
	private bool ignoreRiderkickAngle;

	// Token: 0x04001738 RID: 5944
	public GameObject attackTrail;

	// Token: 0x04001739 RID: 5945
	public GameObject swingSnake;

	// Token: 0x0400173A RID: 5946
	private List<GameObject> currentSwingSnakes = new List<GameObject>();

	// Token: 0x0400173B RID: 5947
	private bool uppercutting;

	// Token: 0x0400173C RID: 5948
	private bool hitSuccessful;

	// Token: 0x0400173D RID: 5949
	private bool gotParried;

	// Token: 0x0400173E RID: 5950
	public Transform[] swingLimbs;

	// Token: 0x0400173F RID: 5951
	private bool swinging;

	// Token: 0x04001740 RID: 5952
	private bool boxing;

	// Token: 0x04001741 RID: 5953
	private int attacksSinceBoxing;

	// Token: 0x04001742 RID: 5954
	private SwingCheck2 sc;

	// Token: 0x04001743 RID: 5955
	private GoreZone gz;

	// Token: 0x04001744 RID: 5956
	private int attackAmount;

	// Token: 0x04001745 RID: 5957
	private bool enraged;

	// Token: 0x04001746 RID: 5958
	public GameObject passiveEffect;

	// Token: 0x04001747 RID: 5959
	private GameObject currentPassiveEffect;

	// Token: 0x04001748 RID: 5960
	public GameObject flameEffect;

	// Token: 0x04001749 RID: 5961
	public GameObject phaseChangeEffect;

	// Token: 0x0400174A RID: 5962
	private int difficulty = -1;

	// Token: 0x0400174B RID: 5963
	private MPAttack previousCombo = MPAttack.Jump;

	// Token: 0x0400174C RID: 5964
	private bool activated = true;

	// Token: 0x0400174D RID: 5965
	private bool ascending;

	// Token: 0x0400174E RID: 5966
	private bool vibrating;

	// Token: 0x0400174F RID: 5967
	private Vector3 origPos;

	// Token: 0x04001750 RID: 5968
	public GameObject lightShaft;

	// Token: 0x04001751 RID: 5969
	public GameObject outroExplosion;

	// Token: 0x04001752 RID: 5970
	public UltrakillEvent onOutroEnd;

	// Token: 0x04001753 RID: 5971
	private Vector3 spawnPoint;

	// Token: 0x04001754 RID: 5972
	[Header("Voice clips")]
	public AudioClip[] riderKickVoice;

	// Token: 0x04001755 RID: 5973
	public AudioClip[] dropkickVoice;

	// Token: 0x04001756 RID: 5974
	public AudioClip[] dropAttackVoice;

	// Token: 0x04001757 RID: 5975
	public AudioClip[] boxingVoice;

	// Token: 0x04001758 RID: 5976
	public AudioClip[] comboVoice;

	// Token: 0x04001759 RID: 5977
	public AudioClip[] overheadVoice;

	// Token: 0x0400175A RID: 5978
	public AudioClip[] projectileVoice;

	// Token: 0x0400175B RID: 5979
	public AudioClip[] uppercutVoice;

	// Token: 0x0400175C RID: 5980
	public AudioClip phaseChangeVoice;

	// Token: 0x0400175D RID: 5981
	public AudioClip[] hurtVoice;

	// Token: 0x0400175E RID: 5982
	private bool bossVersion;
}
