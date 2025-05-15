using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000409 RID: 1033
public class SisyphusPrime : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06001759 RID: 5977 RVA: 0x000BEF89 File Offset: 0x000BD189
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x000BEF98 File Offset: 0x000BD198
	private void Awake()
	{
		this.nma = base.GetComponent<NavMeshAgent>();
		this.mach = base.GetComponent<Machine>();
		this.gce = base.GetComponentInChildren<GroundCheckEnemy>();
		this.rb = base.GetComponent<Rigidbody>();
		this.sc = base.GetComponentInChildren<SwingCheck2>();
		this.col = base.GetComponent<Collider>();
		this.aud = base.GetComponent<AudioSource>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.sc.OverrideEnemyIdentifier(this.eid);
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x000BF018 File Offset: 0x000BD218
	private void Start()
	{
		this.defaultMoveSpeed = this.nma.speed;
		this.SetSpeed();
		this.head = this.eid.weakPoint.transform;
		this.originalHp = this.mach.health;
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		this.spawnPoint = base.transform.position;
		BossHealthBar bossHealthBar;
		this.bossVersion = base.TryGetComponent<BossHealthBar>(out bossHealthBar);
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x000BF093 File Offset: 0x000BD293
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x000BF09C File Offset: 0x000BD29C
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

	// Token: 0x0600175E RID: 5982 RVA: 0x000BF1B4 File Offset: 0x000BD3B4
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

	// Token: 0x0600175F RID: 5983 RVA: 0x000BF20B File Offset: 0x000BD40B
	private void OnEnable()
	{
		if (!this.activated)
		{
			this.OutroEnd();
		}
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x000BF21C File Offset: 0x000BD41C
	private void Update()
	{
		if (this.activated && this.target != null)
		{
			this.heightAdjustedTargetPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
			if (!this.inAction || this.taunting)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (!this.enraged && this.mach.health < this.originalHp / 2f)
			{
				this.enraged = true;
				MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("YES! That's it!", null, false);
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
				UltrakillEvent ultrakillEvent = this.onPhaseChange;
				if (ultrakillEvent == null)
				{
					return;
				}
				ultrakillEvent.Invoke("");
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
				float num = 0.1f;
				if (this.activated)
				{
					num = 0.25f;
				}
				base.transform.position = new Vector3(this.origPos.x + Random.Range(-num, num), this.origPos.y + Random.Range(-num, num), this.origPos.z + Random.Range(-num, num));
			}
		}
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x000BF480 File Offset: 0x000BD680
	private void FixedUpdate()
	{
		if (this.activated)
		{
			this.CustomPhysics();
			if (this.eid.target == null)
			{
				return;
			}
			if (!this.inAction && this.gce.onGround && this.nma && this.nma.enabled && this.nma.isOnNavMesh)
			{
				if (Vector3.Distance(base.transform.position, this.heightAdjustedTargetPos) > 10f)
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
				else if (this.cooldown > 0f)
				{
					if (!this.tauntCheck)
					{
						this.tauntCheck = true;
						if (this.attacksSinceTaunt >= (this.enraged ? 15 : 10) && this.mach.health > 20f)
						{
							this.Taunt();
						}
						else
						{
							this.LookAtTarget();
						}
					}
					else
					{
						this.LookAtTarget();
					}
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
					base.transform.LookAt(this.heightAdjustedTargetPos);
				}
				else
				{
					base.transform.rotation = Quaternion.LookRotation(this.target.position - new Vector3(base.transform.position.x, this.aimingBone.position.y, base.transform.position.z));
				}
			}
			if (this.nma && this.nma.enabled && this.nma.isOnNavMesh && !this.inAction)
			{
				bool flag = this.cooldown > 0f && Vector3.Distance(base.transform.position, this.heightAdjustedTargetPos) < 20f;
				this.nma.speed = (flag ? (this.defaultMoveSpeed / 2f) : this.defaultMoveSpeed);
				this.anim.SetBool("Cooldown", flag);
				this.anim.SetBool("Walking", this.nma.velocity.magnitude > 2f);
			}
		}
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x000BF798 File Offset: 0x000BD998
	private void LateUpdate()
	{
		if (this.aiming && this.inAction && this.activated)
		{
			this.aimingBone.LookAt(this.target.position);
			this.aimingBone.Rotate(Vector3.up * -90f, Space.Self);
		}
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000BF7F0 File Offset: 0x000BD9F0
	private void CustomPhysics()
	{
		if ((this.difficulty == 3 && ((!this.enraged && this.attackAmount >= 8) || this.attackAmount >= 16)) || (this.difficulty <= 2 && (((this.difficulty <= 1 || !this.enraged) && this.attackAmount >= 6) || this.attackAmount >= 10)))
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
			this.tauntCheck = false;
		}
		if (!this.inAction)
		{
			this.gravityInAction = false;
			if (this.gce.onGround && !this.jumping)
			{
				this.nma.enabled = true;
				this.rb.isKinematic = true;
				this.hasProjectiled = false;
				if (this.cooldown <= 0f && !this.anim.IsInTransition(0) && this.target != null && this.activated)
				{
					if (!Physics.Raycast(this.target.position, Vector3.down, (float)((MonoSingleton<NewMovement>.Instance.rb.velocity.y > 0f) ? 11 : 15), LayerMaskDefaults.Get(LMD.Environment)))
					{
						this.attacksSinceTaunt++;
						this.secondariesSinceLastPrimary++;
						this.PickSecondaryAttack(-1);
					}
					else
					{
						this.PickAnyAttack();
					}
				}
			}
			else
			{
				this.nma.enabled = false;
				this.rb.isKinematic = false;
				if (this.cooldown <= 0f && !this.anim.IsInTransition(0) && this.eid.target != null && this.activated)
				{
					if (this.secondariesSinceLastPrimary <= (this.enraged ? 3 : 2))
					{
						this.attacksSinceTaunt++;
						this.secondariesSinceLastPrimary++;
						this.PickSecondaryAttack(-1);
					}
					else if (this.enraged)
					{
						this.TeleportOnGround(0);
					}
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
					if (this.hitSuccessful && this.target.position.y > base.transform.position.y && this.activated && this.target != null)
					{
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

	// Token: 0x06001764 RID: 5988 RVA: 0x000BFC3C File Offset: 0x000BDE3C
	private void PickAnyAttack()
	{
		if (this.secondariesSinceLastPrimary != 0 && (this.secondariesSinceLastPrimary > (this.enraged ? 2 : 1) || Random.Range(0f, 1f) > 0.5f))
		{
			this.attacksSinceTaunt++;
			this.secondariesSinceLastPrimary = 0;
			this.PickPrimaryAttack(-1);
			return;
		}
		this.attacksSinceTaunt++;
		this.secondariesSinceLastPrimary++;
		this.PickSecondaryAttack(-1);
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000BFCBC File Offset: 0x000BDEBC
	private void PickPrimaryAttack(int type = -1)
	{
		if (type == -1)
		{
			type = Random.Range(0, 3);
		}
		switch (type)
		{
		case 0:
			if (this.lastPrimaryAttack != SPAttack.UppercutCombo)
			{
				this.attacksSinceLastExplosion++;
				this.UppercutCombo();
				this.lastPrimaryAttack = SPAttack.UppercutCombo;
				return;
			}
			this.PickPrimaryAttack(type + 1);
			return;
		case 1:
			if (this.lastPrimaryAttack != SPAttack.StompCombo)
			{
				this.attacksSinceLastExplosion++;
				this.StompCombo();
				this.lastPrimaryAttack = SPAttack.StompCombo;
				return;
			}
			this.PickPrimaryAttack(type + 1);
			return;
		case 2:
			if (this.attacksSinceLastExplosion >= 2)
			{
				this.lastSecondaryAttack = SPAttack.Explosion;
				this.attacksSinceLastExplosion = 0;
				this.TeleportAnywhere();
				this.ExplodeAttack();
				return;
			}
			this.PickPrimaryAttack(Random.Range(0, 2));
			return;
		default:
			return;
		}
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x000BFD78 File Offset: 0x000BDF78
	private void PickSecondaryAttack(int type = -1)
	{
		if (type == -1)
		{
			type = Random.Range(0, 4);
		}
		switch (type)
		{
		case 0:
			if (this.lastSecondaryAttack != SPAttack.Chop)
			{
				this.lastSecondaryAttack = SPAttack.Chop;
				this.TeleportSide(Random.Range(0, 2), true, false);
				this.Chop();
				return;
			}
			this.PickSecondaryAttack(type + 1);
			return;
		case 1:
			if (this.lastSecondaryAttack != SPAttack.Clap)
			{
				this.lastSecondaryAttack = SPAttack.Clap;
				this.TeleportAnywhere();
				this.Clap();
				return;
			}
			this.PickSecondaryAttack(type + 1);
			return;
		case 2:
			if (this.lastSecondaryAttack != SPAttack.AirStomp)
			{
				this.lastSecondaryAttack = SPAttack.AirStomp;
				this.TeleportAbove();
				this.AirStomp();
				return;
			}
			this.PickSecondaryAttack(type + 1);
			return;
		case 3:
			if (this.lastSecondaryAttack != SPAttack.AirKick)
			{
				this.lastSecondaryAttack = SPAttack.AirKick;
				this.TeleportAnywhere(true);
				this.AirKick();
				return;
			}
			this.PickSecondaryAttack(0);
			return;
		default:
			return;
		}
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x000BFE4C File Offset: 0x000BE04C
	public void CancelIntoSecondary()
	{
		if (this.enraged)
		{
			this.secondariesSinceLastPrimary++;
			int num = Random.Range(0, 3);
			this.PickSecondaryAttack(num);
		}
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x000BFE80 File Offset: 0x000BE080
	public void Taunt()
	{
		this.attacksSinceTaunt = 0;
		this.inAction = true;
		base.transform.LookAt(this.heightAdjustedTargetPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("Taunt", 0, 0f);
		this.aiming = false;
		this.taunting = true;
		this.attackAmount += 2;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("Nice try!", null, false);
		this.PlayVoice(this.tauntVoice);
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x000BFF10 File Offset: 0x000BE110
	public void UppercutCombo()
	{
		this.previousCombo = SPAttack.UppercutCombo;
		this.inAction = true;
		base.transform.LookAt(this.heightAdjustedTargetPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("UppercutCombo", 0, 0f);
		this.sc.knockBackForce = 50f;
		this.aiming = false;
		this.attackAmount += 3;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("DESTROY!", null, false);
		this.PlayVoice(this.uppercutComboVoice);
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000BFFAC File Offset: 0x000BE1AC
	public void StompCombo()
	{
		this.previousCombo = SPAttack.StompCombo;
		this.inAction = true;
		base.transform.LookAt(this.heightAdjustedTargetPos);
		this.tracking = true;
		this.fullTracking = false;
		this.gravityInAction = false;
		this.anim.Play("StompCombo", 0, 0f);
		this.sc.knockBackForce = 50f;
		this.aiming = false;
		this.attackAmount += 3;
		this.teleportToGroundFailsafe = base.transform.position;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("You can't escape!", null, false);
		this.PlayVoice(this.stompComboVoice);
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000C0058 File Offset: 0x000BE258
	private void Chop()
	{
		this.tracking = true;
		this.fullTracking = true;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.gravityInAction = false;
		this.anim.SetTrigger("Chop");
		this.Unparryable();
		this.sc.knockBackForce = 50f;
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = Vector3.down;
		this.aiming = false;
		this.attackAmount++;
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x000C00F0 File Offset: 0x000BE2F0
	private void Clap()
	{
		this.tracking = true;
		this.fullTracking = true;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.gravityInAction = false;
		this.anim.SetTrigger("Clap");
		this.Parryable();
		this.sc.knockBackForce = 100f;
		this.aiming = false;
		this.attackAmount++;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("BE GONE!", null, false);
		this.PlayVoice(this.clapVoice);
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000C0188 File Offset: 0x000BE388
	private void AirStomp()
	{
		this.tracking = true;
		this.fullTracking = false;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.gravityInAction = false;
		this.anim.SetTrigger("AirStomp");
		this.Unparryable();
		this.aiming = false;
		this.attackAmount++;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x000C01F4 File Offset: 0x000BE3F4
	private void AirKick()
	{
		this.tracking = false;
		this.fullTracking = false;
		this.inAction = true;
		this.gravityInAction = false;
		this.anim.SetTrigger("AirKick");
		this.Parryable();
		this.sc.knockBackForce = 100f;
		this.sc.ignoreSlidingPlayer = true;
		this.aiming = false;
		this.attackAmount++;
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x000C0264 File Offset: 0x000BE464
	private void ExplodeAttack()
	{
		this.tracking = true;
		this.fullTracking = true;
		this.inAction = true;
		base.transform.LookAt(this.target.position);
		this.gravityInAction = false;
		this.anim.SetTrigger("Explosion");
		this.aiming = false;
		this.attackAmount++;
		MonoSingleton<SubtitleController>.Instance.DisplaySubtitle("This will hurt.", null, false);
		this.PlayVoice(this.explosionVoice);
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x000C02E5 File Offset: 0x000BE4E5
	public void ClapStart()
	{
		this.SnakeSwingStart(0);
		this.SnakeSwingStart(1);
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x000C02F8 File Offset: 0x000BE4F8
	public void ClapShockwave()
	{
		this.DamageStop();
		if (this.gotParried && this.difficulty <= 2 && !this.enraged)
		{
			this.gotParried = false;
			return;
		}
		if (this.difficulty >= 2)
		{
			PhysicalShockwave physicalShockwave = this.CreateShockwave(Vector3.Lerp(this.swingLimbs[0].position, this.swingLimbs[1].position, 0.5f));
			physicalShockwave.target = this.target;
			physicalShockwave.transform.rotation = base.transform.rotation;
			physicalShockwave.transform.Rotate(Vector3.forward * 90f, Space.Self);
			physicalShockwave.speed *= 2f;
			if (this.difficulty >= 4 && this.enraged)
			{
				this.DelayedExplosion(Vector3.Lerp(this.swingLimbs[0].position, this.swingLimbs[1].position, 0.5f));
			}
		}
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x000C03EC File Offset: 0x000BE5EC
	public void StompShockwave()
	{
		this.DamageStop();
		if (this.gotParried && this.difficulty <= 2 && !this.enraged)
		{
			this.gotParried = false;
			return;
		}
		if (this.difficulty >= 2)
		{
			this.CreateShockwave(new Vector3(this.swingLimbs[2].position.x, base.transform.position.y, this.swingLimbs[2].position.z));
		}
		if (this.difficulty >= 4 && this.enraged)
		{
			this.DelayedExplosion(new Vector3(this.swingLimbs[2].position.x, base.transform.position.y, this.swingLimbs[2].position.z));
		}
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x000C04BC File Offset: 0x000BE6BC
	private PhysicalShockwave CreateShockwave(Vector3 position)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.groundWave, position, Quaternion.identity);
		gameObject.transform.SetParent(this.gz.transform);
		PhysicalShockwave physicalShockwave;
		if (gameObject.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
		{
			physicalShockwave.target = this.target;
			physicalShockwave.enemyType = EnemyType.SisyphusPrime;
			physicalShockwave.damage = Mathf.RoundToInt((float)physicalShockwave.damage * this.eid.totalDamageModifier);
			return physicalShockwave;
		}
		return null;
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000C0530 File Offset: 0x000BE730
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
			this.anim.Play("Falling", 0, 0f);
		}
		else
		{
			base.transform.position = raycastHit.point - Vector3.up * 6.5f;
			this.ResetRotation();
			this.inAction = false;
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

	// Token: 0x06001775 RID: 6005 RVA: 0x000C08CC File Offset: 0x000BEACC
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
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(30f * this.eid.totalDamageModifier), true, 1f, false, false, 0.35f, false);
				MonoSingleton<NewMovement>.Instance.LaunchFromPoint(MonoSingleton<NewMovement>.Instance.transform.position + (MonoSingleton<NewMovement>.Instance.transform.position - new Vector3(raycastHit2.point.x, MonoSingleton<NewMovement>.Instance.transform.position.y, raycastHit2.point.z)).normalized, 100f, 100f);
			}
		}
		base.transform.position = raycastHit.point;
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
				physicalShockwave.enemyType = EnemyType.SisyphusPrime;
				physicalShockwave.damage = Mathf.RoundToInt((float)physicalShockwave.damage * this.eid.totalDamageModifier);
			}
		}
	}

	// Token: 0x06001776 RID: 6006 RVA: 0x000C0BB4 File Offset: 0x000BEDB4
	public void SnakeSwingStart(int limb)
	{
		if (this.eid.dead)
		{
			return;
		}
		Transform child = Object.Instantiate<GameObject>(this.swingSnake, this.aimingBone.position + base.transform.forward * 4f, Quaternion.identity).transform.GetChild(0);
		child.SetParent(base.transform, true);
		child.LookAt(this.heightAdjustedTargetPos);
		this.currentSwingSnakes.Add(child.gameObject);
		if (!this.boxing)
		{
			this.swinging = true;
		}
		SwingCheck2 componentInChildren = child.GetComponentInChildren<SwingCheck2>();
		if (componentInChildren)
		{
			componentInChildren.OverrideEnemyIdentifier(this.eid);
			componentInChildren.knockBackDirectionOverride = true;
			if (this.sc.knockBackDirectionOverride)
			{
				componentInChildren.knockBackDirection = this.sc.knockBackDirection;
			}
			else
			{
				componentInChildren.knockBackDirection = base.transform.forward;
			}
			componentInChildren.knockBackForce = this.sc.knockBackForce;
			componentInChildren.ignoreSlidingPlayer = this.sc.ignoreSlidingPlayer;
		}
		AttackTrail componentInChildren2 = child.GetComponentInChildren<AttackTrail>();
		if (componentInChildren2)
		{
			componentInChildren2.target = this.swingLimbs[limb];
			componentInChildren2.pivot = this.aimingBone;
		}
		this.DamageStart();
	}

	// Token: 0x06001777 RID: 6007 RVA: 0x000C0CED File Offset: 0x000BEEED
	public void DamageStart()
	{
		this.sc.DamageStart();
	}

	// Token: 0x06001778 RID: 6008 RVA: 0x000C0CFC File Offset: 0x000BEEFC
	public void DamageStop()
	{
		this.swinging = false;
		this.sc.DamageStop();
		this.sc.knockBackDirectionOverride = false;
		this.sc.ignoreSlidingPlayer = false;
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

	// Token: 0x06001779 RID: 6009 RVA: 0x000C0DFC File Offset: 0x000BEFFC
	public void Explosion()
	{
		this.vibrating = false;
		if (this.currentExplosionChargeEffect)
		{
			Object.Destroy(this.currentExplosionChargeEffect);
		}
		if (this.gotParried)
		{
			this.gotParried = false;
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, this.aimingBone.position, Quaternion.identity);
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

	// Token: 0x0600177A RID: 6010 RVA: 0x000C0F48 File Offset: 0x000BF148
	public void ProjectileCharge()
	{
		if (this.currentProjectileCharge)
		{
			Object.Destroy(this.currentProjectileCharge);
		}
		this.currentProjectileCharge = Object.Instantiate<GameObject>(this.projectileCharge, this.swingLimbs[1].position, this.swingLimbs[1].rotation);
		this.currentProjectileCharge.transform.SetParent(this.swingLimbs[1]);
	}

	// Token: 0x0600177B RID: 6011 RVA: 0x000C0FB0 File Offset: 0x000BF1B0
	public void ProjectileShoot()
	{
		if (this.currentProjectileCharge)
		{
			Object.Destroy(this.currentProjectileCharge);
		}
		if (this.target == null)
		{
			return;
		}
		this.mach.parryable = false;
		Vector3 vector = this.target.PredictTargetPosition(0.5f, false);
		base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
		this.DelayedExplosion(vector);
		this.aiming = false;
		this.tracking = false;
		this.fullTracking = false;
	}

	// Token: 0x0600177C RID: 6012 RVA: 0x000C1044 File Offset: 0x000BF244
	public void DelayedExplosion(Vector3 target)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.sparkleExplosion, target, Quaternion.identity);
		gameObject.transform.SetParent(this.gz.transform);
		ObjectActivator component = gameObject.GetComponent<ObjectActivator>();
		if (component)
		{
			component.delay /= this.eid.totalSpeedModifier;
		}
		LineRenderer componentInChildren = gameObject.GetComponentInChildren<LineRenderer>();
		if (componentInChildren)
		{
			componentInChildren.SetPosition(0, target);
			componentInChildren.SetPosition(1, this.swingLimbs[1].position);
		}
		foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
		{
			explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.eid.totalDamageModifier);
			explosion.maxSize *= this.eid.totalDamageModifier;
		}
	}

	// Token: 0x0600177D RID: 6013 RVA: 0x000C1118 File Offset: 0x000BF318
	public void TeleportOnGround(int forceNoPrediction = 0)
	{
		if (this.target == null)
		{
			return;
		}
		this.ResetRotation();
		Vector3 point = this.teleportToGroundFailsafe;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
		{
			point = raycastHit.point;
		}
		base.transform.position = point;
		this.heightAdjustedTargetPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
		this.Teleport(this.heightAdjustedTargetPos, base.transform.position);
		if (this.difficulty < 2 || forceNoPrediction == 1)
		{
			base.transform.LookAt(this.heightAdjustedTargetPos);
			return;
		}
		Vector3 vector = this.target.PredictTargetPosition(0.5f, false);
		base.transform.LookAt(new Vector3(vector.x, base.transform.position.y, vector.z));
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000C1231 File Offset: 0x000BF431
	public void TeleportAnywhere()
	{
		this.TeleportAnywhere(false);
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x000C123C File Offset: 0x000BF43C
	public void TeleportAnywhere(bool predictive = false)
	{
		if (this.target == null)
		{
			return;
		}
		this.Teleport(predictive ? this.target.PredictTargetPosition(0.5f, false) : this.target.position, base.transform.position);
		if (this.difficulty < 2)
		{
			base.transform.LookAt(this.target.position);
			return;
		}
		base.transform.LookAt(this.target.PredictTargetPosition(0.5f, false));
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x000C12C0 File Offset: 0x000BF4C0
	public void TeleportAbove()
	{
		this.TeleportAbove(true);
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x000C12CC File Offset: 0x000BF4CC
	public void TeleportAbove(bool predictive = true)
	{
		Vector3 vector = (predictive ? this.target.PredictTargetPosition(0.5f, false) : this.target.position);
		if (vector.y < this.target.position.y)
		{
			vector.y = this.target.position.y;
		}
		this.Teleport(vector + Vector3.up * 25f, vector);
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x000C1346 File Offset: 0x000BF546
	public void TeleportSideRandom(int predictive)
	{
		this.TeleportSide(Random.Range(0, 2), false, predictive == 1);
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000C135E File Offset: 0x000BF55E
	public void TeleportSideRandomAir(int predictive)
	{
		this.TeleportSide(Random.Range(0, 2), true, predictive == 1);
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x000C1378 File Offset: 0x000BF578
	public void TeleportSide(int side, bool inAir = false, bool predictive = false)
	{
		int num = 1;
		Vector3 vector = (predictive ? this.target.PredictTargetPosition(0.5f, false) : this.target.position);
		if (!inAir)
		{
			vector = new Vector3(vector.x, base.transform.position.y, vector.z);
		}
		if (side == 0)
		{
			num = -1;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(vector + Vector3.up, this.target.right * (float)num + this.target.forward, out raycastHit, 4f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
		{
			if (raycastHit.distance >= 2f)
			{
				this.Teleport(vector, vector + Vector3.ClampMagnitude(this.target.right * (float)num + this.target.forward, 1f) * (raycastHit.distance - 1f));
			}
			else
			{
				this.Teleport(vector, base.transform.position);
			}
			base.transform.LookAt(vector);
			return;
		}
		this.Teleport(vector, vector + (this.target.right * (float)num + this.target.forward) * 10f);
		base.transform.LookAt(vector);
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x000C14DC File Offset: 0x000BF6DC
	public void Teleport(Vector3 teleportTarget, Vector3 startPos)
	{
		float num = Vector3.Distance(teleportTarget, startPos);
		if (this.boxing && num > 4.5f)
		{
			num = 4.5f;
		}
		else if (num > 6f)
		{
			num = 6f;
		}
		LineRenderer component = Object.Instantiate<GameObject>(this.attackTrail, this.aimingBone.position, base.transform.rotation).GetComponent<LineRenderer>();
		component.SetPosition(0, this.aimingBone.position);
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
		component.SetPosition(1, this.aimingBone.position);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x000C179C File Offset: 0x000BF99C
	public void LookAtTarget()
	{
		if (this.target == null)
		{
			return;
		}
		this.heightAdjustedTargetPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
		base.transform.LookAt(this.heightAdjustedTargetPos);
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000C1800 File Offset: 0x000BFA00
	public void Death()
	{
		if (this.currentProjectileCharge)
		{
			Object.Destroy(this.currentProjectileCharge);
		}
		this.DamageStop();
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
		Object.Destroy(this.nma);
		this.DisableGravity();
		this.rb.useGravity = false;
		this.rb.isKinematic = true;
		MonoSingleton<TimeController>.Instance.SlowDown(0.0001f);
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000C18D4 File Offset: 0x000BFAD4
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

	// Token: 0x06001789 RID: 6025 RVA: 0x000C1A18 File Offset: 0x000BFC18
	private void LightShaft()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.lightShaft, this.mach.chest.transform.position, Random.rotation).transform.SetParent(base.transform, true);
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000C1A78 File Offset: 0x000BFC78
	public void OutroEnd()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.onOutroEnd.Invoke("");
		Object.Instantiate<GameObject>(this.outroExplosion, this.mach.chest.transform.position, Quaternion.identity);
		base.gameObject.SetActive(false);
		MonoSingleton<TimeController>.Instance.SlowDown(0.001f);
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x000C1AE4 File Offset: 0x000BFCE4
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

	// Token: 0x0600178C RID: 6028 RVA: 0x000C1B1C File Offset: 0x000BFD1C
	public void Parryable()
	{
		this.gotParried = false;
		Object.Instantiate<GameObject>(this.parryableFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 30f;
		this.mach.ParryableCheck(false);
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000C1B90 File Offset: 0x000BFD90
	public void Unparryable()
	{
		Object.Instantiate<GameObject>(this.warningFlash, this.head.position, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.head.position)).transform.localScale *= 15f;
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000C1BF1 File Offset: 0x000BFDF1
	public void GotParried()
	{
		this.PlayVoice(this.hurtVoice);
		this.attackAmount -= 5;
		this.gotParried = true;
		if (this.currentExplosionChargeEffect)
		{
			Object.Destroy(this.currentExplosionChargeEffect);
		}
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000C1C2C File Offset: 0x000BFE2C
	public void Rubble()
	{
		Object.Instantiate<GameObject>(this.bigRubble, base.transform.position + base.transform.forward, base.transform.rotation);
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x000C1C60 File Offset: 0x000BFE60
	public void ResetRotation()
	{
		base.transform.LookAt(new Vector3(base.transform.position.x + base.transform.forward.x, base.transform.position.y, base.transform.position.z + base.transform.forward.z));
		this.ResolveStuckness();
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x000C1CD5 File Offset: 0x000BFED5
	public void DisableGravity()
	{
		this.gravityInAction = false;
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000C1CDE File Offset: 0x000BFEDE
	public void StartTracking()
	{
		this.tracking = true;
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x000C1CE7 File Offset: 0x000BFEE7
	public void StopTracking()
	{
		this.tracking = false;
		this.fullTracking = false;
	}

	// Token: 0x06001794 RID: 6036 RVA: 0x000C1CF8 File Offset: 0x000BFEF8
	public void StopAction()
	{
		this.fullTracking = false;
		this.ResetRotation();
		this.gotParried = false;
		this.inAction = false;
		this.boxing = false;
		this.taunting = false;
		this.sc.knockBackDirectionOverride = false;
		if (this.mach)
		{
			this.mach.parryable = false;
		}
	}

	// Token: 0x06001795 RID: 6037 RVA: 0x000C1D54 File Offset: 0x000BFF54
	public void TargetBeenHit()
	{
		this.sc.DamageStop();
		this.hitSuccessful = true;
		this.mach.parryable = false;
		foreach (GameObject gameObject in this.currentSwingSnakes)
		{
			SwingCheck2 swingCheck;
			if (gameObject && gameObject.TryGetComponent<SwingCheck2>(out swingCheck))
			{
				swingCheck.OverrideEnemyIdentifier(this.eid);
				swingCheck.DamageStop();
			}
		}
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x000C1DE4 File Offset: 0x000BFFE4
	public void OutOfBounds()
	{
		base.transform.position = this.spawnPoint;
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000C1DF8 File Offset: 0x000BFFF8
	public void Vibrate()
	{
		if (this.currentExplosionChargeEffect)
		{
			Object.Destroy(this.currentExplosionChargeEffect);
		}
		if (this.activated)
		{
			this.currentExplosionChargeEffect = Object.Instantiate<GameObject>(this.explosionChargeEffect, this.aimingBone.position, Quaternion.identity);
		}
		this.origPos = base.transform.position;
		this.vibrating = true;
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x000C1E60 File Offset: 0x000C0060
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

	// Token: 0x06001799 RID: 6041 RVA: 0x000C1ED2 File Offset: 0x000C00D2
	public void ForceKnockbackDown()
	{
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = Vector3.down;
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x000C1EF0 File Offset: 0x000C00F0
	public void SwingIgnoreSliding()
	{
		this.sc.ignoreSlidingPlayer = true;
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000C1F00 File Offset: 0x000C0100
	public void ResolveStuckness()
	{
		Collider[] array = Physics.OverlapCapsule(base.transform.position + base.transform.up * 0.76f, base.transform.position + base.transform.up * 5.24f, 0.74f, LayerMaskDefaults.Get(LMD.Environment));
		if (array != null && array.Length != 0)
		{
			if (this.gce.onGround)
			{
				this.gce.onGround = false;
				if (this.nma != null)
				{
					this.nma.enabled = false;
				}
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

	// Token: 0x040020AA RID: 8362
	private NavMeshAgent nma;

	// Token: 0x040020AB RID: 8363
	private Animator anim;

	// Token: 0x040020AC RID: 8364
	private Machine mach;

	// Token: 0x040020AD RID: 8365
	private EnemyIdentifier eid;

	// Token: 0x040020AE RID: 8366
	private GroundCheckEnemy gce;

	// Token: 0x040020AF RID: 8367
	private Rigidbody rb;

	// Token: 0x040020B0 RID: 8368
	private Collider col;

	// Token: 0x040020B1 RID: 8369
	private AudioSource aud;

	// Token: 0x040020B2 RID: 8370
	private float originalHp;

	// Token: 0x040020B3 RID: 8371
	private bool inAction;

	// Token: 0x040020B4 RID: 8372
	private float cooldown = 2f;

	// Token: 0x040020B5 RID: 8373
	private SPAttack lastPrimaryAttack;

	// Token: 0x040020B6 RID: 8374
	private SPAttack lastSecondaryAttack;

	// Token: 0x040020B7 RID: 8375
	private int secondariesSinceLastPrimary;

	// Token: 0x040020B8 RID: 8376
	private int attacksSinceLastExplosion;

	// Token: 0x040020B9 RID: 8377
	private Vector3 heightAdjustedTargetPos;

	// Token: 0x040020BA RID: 8378
	private bool tracking;

	// Token: 0x040020BB RID: 8379
	private bool fullTracking;

	// Token: 0x040020BC RID: 8380
	private bool aiming;

	// Token: 0x040020BD RID: 8381
	private bool jumping;

	// Token: 0x040020BE RID: 8382
	public GameObject explosion;

	// Token: 0x040020BF RID: 8383
	public GameObject explosionChargeEffect;

	// Token: 0x040020C0 RID: 8384
	private GameObject currentExplosionChargeEffect;

	// Token: 0x040020C1 RID: 8385
	public GameObject rubble;

	// Token: 0x040020C2 RID: 8386
	public GameObject bigRubble;

	// Token: 0x040020C3 RID: 8387
	public GameObject groundWave;

	// Token: 0x040020C4 RID: 8388
	public GameObject swoosh;

	// Token: 0x040020C5 RID: 8389
	public Transform aimingBone;

	// Token: 0x040020C6 RID: 8390
	private Transform head;

	// Token: 0x040020C7 RID: 8391
	public GameObject projectileCharge;

	// Token: 0x040020C8 RID: 8392
	private GameObject currentProjectileCharge;

	// Token: 0x040020C9 RID: 8393
	public GameObject sparkleExplosion;

	// Token: 0x040020CA RID: 8394
	private bool hasProjectiled;

	// Token: 0x040020CB RID: 8395
	public GameObject warningFlash;

	// Token: 0x040020CC RID: 8396
	public GameObject parryableFlash;

	// Token: 0x040020CD RID: 8397
	private bool gravityInAction;

	// Token: 0x040020CE RID: 8398
	public GameObject attackTrail;

	// Token: 0x040020CF RID: 8399
	public GameObject swingSnake;

	// Token: 0x040020D0 RID: 8400
	private List<GameObject> currentSwingSnakes = new List<GameObject>();

	// Token: 0x040020D1 RID: 8401
	private bool uppercutting;

	// Token: 0x040020D2 RID: 8402
	private bool hitSuccessful;

	// Token: 0x040020D3 RID: 8403
	private bool gotParried;

	// Token: 0x040020D4 RID: 8404
	private Vector3 teleportToGroundFailsafe;

	// Token: 0x040020D5 RID: 8405
	public Transform[] swingLimbs;

	// Token: 0x040020D6 RID: 8406
	private bool swinging;

	// Token: 0x040020D7 RID: 8407
	private bool boxing;

	// Token: 0x040020D8 RID: 8408
	private SwingCheck2 sc;

	// Token: 0x040020D9 RID: 8409
	private GoreZone gz;

	// Token: 0x040020DA RID: 8410
	private int attackAmount;

	// Token: 0x040020DB RID: 8411
	private bool enraged;

	// Token: 0x040020DC RID: 8412
	public GameObject passiveEffect;

	// Token: 0x040020DD RID: 8413
	private GameObject currentPassiveEffect;

	// Token: 0x040020DE RID: 8414
	public GameObject flameEffect;

	// Token: 0x040020DF RID: 8415
	public GameObject phaseChangeEffect;

	// Token: 0x040020E0 RID: 8416
	private int difficulty = -1;

	// Token: 0x040020E1 RID: 8417
	private SPAttack previousCombo = SPAttack.Explosion;

	// Token: 0x040020E2 RID: 8418
	private bool activated = true;

	// Token: 0x040020E3 RID: 8419
	private bool ascending;

	// Token: 0x040020E4 RID: 8420
	private bool vibrating;

	// Token: 0x040020E5 RID: 8421
	private Vector3 origPos;

	// Token: 0x040020E6 RID: 8422
	public GameObject lightShaft;

	// Token: 0x040020E7 RID: 8423
	public GameObject outroExplosion;

	// Token: 0x040020E8 RID: 8424
	public UltrakillEvent onPhaseChange;

	// Token: 0x040020E9 RID: 8425
	public UltrakillEvent onOutroEnd;

	// Token: 0x040020EA RID: 8426
	private Vector3 spawnPoint;

	// Token: 0x040020EB RID: 8427
	[Header("Voice clips")]
	public AudioClip[] uppercutComboVoice;

	// Token: 0x040020EC RID: 8428
	public AudioClip[] stompComboVoice;

	// Token: 0x040020ED RID: 8429
	public AudioClip phaseChangeVoice;

	// Token: 0x040020EE RID: 8430
	public AudioClip[] hurtVoice;

	// Token: 0x040020EF RID: 8431
	public AudioClip[] explosionVoice;

	// Token: 0x040020F0 RID: 8432
	public AudioClip[] tauntVoice;

	// Token: 0x040020F1 RID: 8433
	public AudioClip[] clapVoice;

	// Token: 0x040020F2 RID: 8434
	private bool bossVersion;

	// Token: 0x040020F3 RID: 8435
	private bool taunting;

	// Token: 0x040020F4 RID: 8436
	private bool tauntCheck;

	// Token: 0x040020F5 RID: 8437
	private int attacksSinceTaunt;

	// Token: 0x040020F6 RID: 8438
	private float defaultMoveSpeed;
}
