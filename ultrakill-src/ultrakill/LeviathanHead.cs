using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class LeviathanHead : MonoBehaviour
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00072395 File Offset: 0x00070595
	[HideInInspector]
	public EnemyTarget Target
	{
		get
		{
			return this.lcon.eid.target;
		}
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x000723A8 File Offset: 0x000705A8
	private void Start()
	{
		this.SetSpeed();
		this.previousHeadRotation = this.tracker.rotation;
		this.defaultBodyRotation = base.transform.rotation;
		this.defaultPosition = base.transform.position;
		this.tailBones = this.tailBone.GetComponentsInChildren<Transform>();
		this.defaultTailPositions = new Vector3[this.tailBones.Length];
		for (int i = 0; i < this.tailBones.Length; i++)
		{
			this.defaultTailPositions[i] = this.tailBones[i].position;
		}
		this.defaultTailRotations = new Quaternion[this.tailBones.Length];
		for (int j = 0; j < this.tailBones.Length; j++)
		{
			this.defaultTailRotations[j] = this.tailBones[j].rotation;
		}
		if (!BlindEnemies.Blind)
		{
			this.anim.Play("AscendLong");
		}
		this.lookAtPlayer = false;
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0007249C File Offset: 0x0007069C
	public void SetSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
		if (this.lcon.difficulty == 2)
		{
			this.anim.speed = 0.9f;
		}
		else if (this.lcon.difficulty == 1)
		{
			this.anim.speed = 0.8f;
		}
		else if (this.lcon.difficulty == 0)
		{
			this.anim.speed = 0.65f;
		}
		else if (this.lcon.difficulty == 3)
		{
			this.anim.speed = 1f;
		}
		else
		{
			this.anim.speed = 1.25f;
		}
		if (this.lcon.difficulty >= 2)
		{
			this.projectileBurstMaximum = 80;
		}
		else if (this.lcon.difficulty == 1)
		{
			this.projectileBurstMaximum = 60;
		}
		else
		{
			this.projectileBurstMaximum = 40;
		}
		this.anim.speed *= this.lcon.eid.totalSpeedModifier;
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x000725AB File Offset: 0x000707AB
	private void OnEnable()
	{
		this.ResetDefaults();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x000725B3 File Offset: 0x000707B3
	private void ResetDefaults()
	{
		this.defaultBodyRotation = base.transform.rotation;
		this.headRotationSpeedMultiplier = 1f;
		this.defaultPosition = base.transform.position;
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x000725E2 File Offset: 0x000707E2
	private void OnDisable()
	{
		this.trackerOverrideAnimation = false;
		this.trackerIgnoreLimits = false;
		this.projectileBursting = false;
		if (this.anim)
		{
			this.anim.SetBool("ProjectileBurst", false);
		}
		this.bodyRotationOverride = false;
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x00072620 File Offset: 0x00070820
	private void LateUpdate()
	{
		if (this.headExploded)
		{
			this.tracker.transform.localScale = Vector3.zero;
		}
		if (!this.active)
		{
			return;
		}
		if (this.beamTime > 0f)
		{
			this.beamTime = Mathf.MoveTowards(this.beamTime, 0f, Time.deltaTime);
			float num = Mathf.Clamp(Mathf.Pow(10f - this.beamTime, 2f) * 5f, 0f, 180f);
			base.transform.Rotate(Vector3.up * num * Time.deltaTime);
			this.anim.SetFloat("BeamSpeed", num / 180f);
			if (this.beamTime <= 0f)
			{
				this.BeamStop();
			}
		}
		else if ((this.rotateBody || this.lcon.secondPhase) && this.Target != null)
		{
			Vector3 vector = (this.bodyRotationOverride ? this.bodyRotationOverrideTarget : this.Target.position);
			Quaternion quaternion = Quaternion.LookRotation(base.transform.position - ((vector.y < base.transform.position.y) ? new Vector3(vector.x, base.transform.position.y, vector.z) : vector));
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * Mathf.Max(Mathf.Min(270f, Quaternion.Angle(base.transform.rotation, quaternion) * 13.5f), 10f) * this.lcon.eid.totalSpeedModifier);
			Quaternion quaternion2 = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * Mathf.Max(Mathf.Min(270f, Quaternion.Angle(base.transform.rotation, quaternion) * 13.5f), 10f) * this.lcon.eid.totalSpeedModifier);
			Vector3 vector2 = this.defaultPosition + Vector3.up * (Mathf.Max(0f, base.transform.localRotation.eulerAngles.x) * 0.85f);
			base.transform.SetPositionAndRotation(vector2, quaternion2);
		}
		else
		{
			Quaternion quaternion3 = Quaternion.RotateTowards(base.transform.rotation, this.defaultBodyRotation, Time.deltaTime * Mathf.Max(Mathf.Min(270f, Quaternion.Angle(base.transform.rotation, this.defaultBodyRotation) * 13.5f), 10f) * this.lcon.eid.totalSpeedModifier);
			Vector3 vector3 = Vector3.MoveTowards(base.transform.position, this.defaultPosition, Time.deltaTime * Mathf.Max(10f, Vector3.Distance(base.transform.position, this.defaultPosition) * 5f) * this.lcon.eid.totalSpeedModifier);
			base.transform.SetPositionAndRotation(vector3, quaternion3);
		}
		if (this.lookAtPlayer && this.Target != null)
		{
			Quaternion quaternion4 = Quaternion.LookRotation(this.Target.position - this.tracker.position);
			if (this.predictPlayer)
			{
				quaternion4 = Quaternion.LookRotation(MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(1.5f, true, true) - this.tracker.position);
			}
			quaternion4 *= Quaternion.Euler(Vector3.right * 90f);
			if (!this.trackerOverrideAnimation)
			{
				Quaternion quaternion5 = Quaternion.Inverse(this.tracker.parent.rotation * this.defaultHeadRotation) * this.tracker.rotation;
				quaternion4 *= quaternion5;
				if (!this.trackerIgnoreLimits)
				{
					float num2 = Quaternion.Angle(quaternion4, this.tracker.rotation);
					if (num2 > 50f)
					{
						quaternion4 = Quaternion.Lerp(this.tracker.rotation, quaternion4, 50f / num2);
						this.cantTurnToPlayer = Mathf.MoveTowards(this.cantTurnToPlayer, 5f, Time.deltaTime);
					}
					else
					{
						this.cantTurnToPlayer = Mathf.MoveTowards(this.cantTurnToPlayer, 0f, Time.deltaTime);
					}
				}
				quaternion4 = Quaternion.RotateTowards(this.previousHeadRotation, quaternion4, Time.deltaTime * Mathf.Max(Mathf.Min(270f, Quaternion.Angle(this.previousHeadRotation, quaternion4) * 13.5f), 10f) * this.headRotationSpeedMultiplier * this.lcon.eid.totalSpeedModifier);
			}
			this.tracker.rotation = quaternion4;
			this.previousHeadRotation = this.tracker.rotation;
			this.notAtDefaultHeadRotation = true;
		}
		else if (this.notAtDefaultHeadRotation)
		{
			if (Quaternion.Angle(this.previousHeadRotation, this.tracker.rotation) > 1f)
			{
				this.tracker.rotation = Quaternion.RotateTowards(this.previousHeadRotation, this.tracker.rotation, Time.deltaTime * Mathf.Max(Mathf.Min(270f, Quaternion.Angle(this.previousHeadRotation, this.tracker.rotation) * 13.5f), 10f) * this.headRotationSpeedMultiplier * this.lcon.eid.totalSpeedModifier);
				this.previousHeadRotation = this.tracker.rotation;
			}
			else
			{
				this.previousHeadRotation = this.tracker.rotation;
				this.notAtDefaultHeadRotation = false;
			}
		}
		else
		{
			this.previousHeadRotation = this.tracker.rotation;
		}
		if (this.freezeTail)
		{
			for (int i = 0; i < this.tailBones.Length; i++)
			{
				this.tailBones[i].SetPositionAndRotation(this.defaultTailPositions[i], this.defaultTailRotations[i]);
			}
		}
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x00072C38 File Offset: 0x00070E38
	private void Update()
	{
		if (!this.active)
		{
			return;
		}
		if (this.lcon.secondPhase)
		{
			this.defaultBodyRotation = Quaternion.LookRotation(base.transform.position - new Vector3(this.Target.position.x, base.transform.position.y, this.Target.position.z));
		}
		if (!this.inAction && this.Target != null)
		{
			this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * this.lcon.eid.totalSpeedModifier);
			if (this.lcon.readyForSecondPhase)
			{
				this.forceBeam = true;
				this.Descend();
				return;
			}
			if (this.attackCooldown <= 0f)
			{
				if (this.recentAttacks >= 3)
				{
					if (this.lcon.secondPhase)
					{
						this.recentAttacks = 0;
						this.forceBeam = true;
						return;
					}
					this.Descend();
					return;
				}
				else
				{
					if (Vector3.Distance(this.Target.position, this.tracker.position) < 50f)
					{
						this.Bite();
						this.previousAttack = 1;
						this.recentAttacks++;
						return;
					}
					int num = Random.Range(0, 2);
					if (num == this.previousAttack)
					{
						num++;
					}
					if (num > 1)
					{
						num = 0;
					}
					if (this.forceBeam)
					{
						num = 2;
						this.forceBeam = false;
					}
					switch (num)
					{
					case 0:
						this.ProjectileBurst();
						break;
					case 1:
						this.Bite();
						break;
					case 2:
						this.BeamAttack();
						break;
					}
					this.previousAttack = num;
					this.recentAttacks++;
				}
			}
		}
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x00072DF0 File Offset: 0x00070FF0
	private void FixedUpdate()
	{
		if (!this.active)
		{
			return;
		}
		if (this.projectileBursting)
		{
			if (this.projectileBurstCooldown > 0f)
			{
				this.projectileBurstCooldown = Mathf.MoveTowards(this.projectileBurstCooldown, 0f, Time.deltaTime * this.lcon.eid.totalSpeedModifier);
			}
			else
			{
				if (this.lcon.difficulty >= 2)
				{
					this.projectileBurstCooldown = 0.025f;
				}
				else
				{
					this.projectileBurstCooldown = ((this.lcon.difficulty == 1) ? 0.0375f : 0.05f);
				}
				this.projectilesLeftInBurst--;
				GameObject gameObject = Object.Instantiate<GameObject>((this.lcon.difficulty >= 2 && this.projectilesLeftInBurst % 20 == 0) ? MonoSingleton<DefaultReferenceManager>.Instance.projectileExplosive : MonoSingleton<DefaultReferenceManager>.Instance.projectile, this.shootPoint.position, this.shootPoint.rotation);
				Projectile projectile;
				if (gameObject.TryGetComponent<Projectile>(out projectile))
				{
					projectile.safeEnemyType = EnemyType.Leviathan;
					if (this.lcon.difficulty == 0)
					{
						projectile.speed *= 0.75f;
					}
					projectile.enemyDamageMultiplier = 0.5f;
					projectile.damage *= this.lcon.eid.totalDamageModifier;
				}
				if (this.projectilesLeftInBurst % 10 != 0)
				{
					gameObject.transform.Rotate(Vector3.forward * (float)(this.projectilesLeftInBurst % 10) * 36f);
					if (this.projectilesLeftInBurst > this.projectileBurstMaximum / 2)
					{
						gameObject.transform.Rotate(Vector3.up * this.projectileSpreadAmount * (float)((this.lcon.difficulty >= 2) ? 2 : 1) * 1.5f * (1f - (float)this.projectilesLeftInBurst / (float)this.projectileBurstMaximum));
					}
					else
					{
						gameObject.transform.Rotate(Vector3.up * this.projectileSpreadAmount * (float)((this.lcon.difficulty >= 2) ? 2 : 1) * 1.5f * ((float)this.projectilesLeftInBurst / (float)this.projectileBurstMaximum));
					}
				}
				else if (this.Target != null)
				{
					gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.LookRotation(this.Target.headPosition - gameObject.transform.position), 5f);
				}
				gameObject.transform.localScale *= 2f;
			}
		}
		if (this.projectileBursting && (this.projectilesLeftInBurst <= 0 || this.Target == null))
		{
			this.projectileBursting = false;
			this.predictPlayer = false;
			this.trackerIgnoreLimits = false;
			this.anim.SetBool("ProjectileBurst", false);
		}
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x000730D4 File Offset: 0x000712D4
	private void Descend()
	{
		if (!this.active)
		{
			return;
		}
		this.inAction = true;
		this.headRotationSpeedMultiplier = 0.5f;
		this.lookAtPlayer = false;
		this.rotateBody = false;
		this.anim.SetBool("Sunken", true);
		Object.Instantiate<AudioSource>(this.biteWindupSound, this.tracker.position, Quaternion.identity, this.tracker).pitch = 0.5f;
		this.recentAttacks = 0;
		this.previousAttack = -1;
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00073154 File Offset: 0x00071354
	private void DescendEnd()
	{
		if (!this.active)
		{
			return;
		}
		base.gameObject.SetActive(false);
		this.lcon.MainPhaseOver();
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00073178 File Offset: 0x00071378
	public void ChangePosition()
	{
		if (!this.active)
		{
			return;
		}
		int num = Random.Range(0, this.spawnPositions.Length);
		if (this.spawnPositions.Length > 1 && num == this.previousSpawnPosition)
		{
			num++;
		}
		if (num >= this.spawnPositions.Length)
		{
			num = 0;
		}
		if (this.lcon.tail && this.lcon.tail.gameObject.activeInHierarchy && Vector3.Distance(this.spawnPositions[num], new Vector3(this.lcon.tail.transform.localPosition.x, this.spawnPositions[num].y, this.lcon.tail.transform.localPosition.z)) < 10f)
		{
			num++;
		}
		if (num >= this.spawnPositions.Length)
		{
			num = 0;
		}
		base.transform.localPosition = this.spawnPositions[num];
		this.previousSpawnPosition = num;
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - new Vector3(base.transform.parent.position.x, base.transform.position.y, base.transform.parent.position.z));
		base.gameObject.SetActive(true);
		this.ResetDefaults();
		this.Ascend();
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x000732F4 File Offset: 0x000714F4
	public void CenterPosition()
	{
		if (!this.active)
		{
			return;
		}
		base.transform.localPosition = this.centerSpawnPosition;
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - new Vector3(this.Target.position.x, base.transform.position.y, this.Target.position.z));
		base.gameObject.SetActive(true);
		this.ResetDefaults();
		this.Ascend();
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x00073388 File Offset: 0x00071588
	private void Ascend()
	{
		if (!this.active)
		{
			return;
		}
		this.inAction = true;
		this.headRotationSpeedMultiplier = 0.5f;
		this.lookAtPlayer = false;
		this.rotateBody = false;
		this.anim.SetBool("Sunken", false);
		this.BigSplash();
		if (this.lcon.secondPhase)
		{
			this.attackCooldown = 3f;
			return;
		}
		if (this.lcon.difficulty <= 2)
		{
			this.attackCooldown = (float)(1 + (2 - this.lcon.difficulty));
		}
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x00073412 File Offset: 0x00071612
	private void StartHeadTracking()
	{
		this.lookAtPlayer = true;
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0007341B File Offset: 0x0007161B
	private void StartBodyTracking()
	{
		this.rotateBody = true;
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x00073424 File Offset: 0x00071624
	private void Bite()
	{
		if (!this.active)
		{
			return;
		}
		this.rotateBody = true;
		this.anim.SetTrigger("Bite");
		this.trackerOverrideAnimation = true;
		this.inAction = true;
		Object.Instantiate<AudioSource>(this.biteWindupSound, this.tracker.position, Quaternion.identity, this.tracker);
		if (this.lcon.difficulty <= 2)
		{
			this.attackCooldown = 0.2f + (float)(2 - this.lcon.difficulty);
		}
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x000734AC File Offset: 0x000716AC
	private void BiteStopTracking()
	{
		if (!this.active)
		{
			return;
		}
		this.lookAtPlayer = false;
		this.trackerOverrideAnimation = false;
		this.bodyRotationOverride = true;
		if (this.Target == null)
		{
			return;
		}
		if (this.lcon.difficulty == 0)
		{
			this.bodyRotationOverrideTarget = this.Target.position;
		}
		else
		{
			this.bodyRotationOverrideTarget = this.Target.position + this.Target.GetVelocity() * ((this.lcon.difficulty >= 2) ? 0.85f : 0.4f);
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.warningFlash, this.lcon.eid.weakPoint.transform.position + this.lcon.eid.weakPoint.transform.up, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.transform.position - this.tracker.position), this.tracker);
		gameObject.transform.localScale *= 0.05f;
		gameObject.transform.position += gameObject.transform.forward * 10f;
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x000735F8 File Offset: 0x000717F8
	private void BiteDamageStart()
	{
		if (!this.active)
		{
			return;
		}
		this.biteSwingCheck.DamageStart();
		this.parryHelper.SetActive(true);
		Object.Instantiate<AudioSource>(this.swingSound, base.transform.position, Quaternion.identity);
		if (this.trackerBones == null || this.trackerBones.Count == 0)
		{
			this.trackerBones.AddRange(this.tracker.GetComponentsInChildren<Transform>());
		}
		this.lcon.stat.parryables = this.trackerBones;
		this.lcon.stat.ParryableCheck(true);
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x00073693 File Offset: 0x00071893
	public void BiteDamageStop()
	{
		this.biteSwingCheck.DamageStop();
		this.parryHelper.SetActive(false);
		this.lcon.stat.partiallyParryable = false;
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x000736BD File Offset: 0x000718BD
	private void BiteResetRotation()
	{
		this.rotateBody = false;
		this.bodyRotationOverride = false;
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x000736CD File Offset: 0x000718CD
	private void BiteEnd()
	{
		this.headRotationSpeedMultiplier = 1f;
		this.lookAtPlayer = true;
		this.StopAction();
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000736E8 File Offset: 0x000718E8
	private void ProjectileBurst()
	{
		if (!this.active)
		{
			return;
		}
		this.anim.SetBool("ProjectileBurst", true);
		this.projectilesLeftInBurst = this.projectileBurstMaximum;
		this.inAction = true;
		this.lookAtPlayer = true;
		if (this.lcon.secondPhase)
		{
			this.predictPlayer = true;
		}
		if (this.lcon.difficulty <= 2)
		{
			this.attackCooldown = 0.5f + (float)(2 - this.lcon.difficulty);
		}
		Object.Instantiate<AudioSource>(this.projectileWindupSound, this.tracker.position, Quaternion.identity, this.tracker);
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x00073787 File Offset: 0x00071987
	private void ProjectileBurstStart()
	{
		this.projectileBursting = true;
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x00073790 File Offset: 0x00071990
	private void BeamAttack()
	{
		if (!this.active)
		{
			return;
		}
		this.inAction = true;
		this.lcon.stopTail = true;
		this.anim.SetBool("BeamAttack", true);
		Object.Instantiate<AudioSource>(this.beamWindupSound, this.tracker.position, Quaternion.identity, this.tracker);
		this.bodyRotationOverride = true;
		this.bodyRotationOverrideTarget = new Vector3(this.Target.position.x, this.defaultPosition.y, this.Target.position.z);
		this.BeamCharge();
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0007382F File Offset: 0x00071A2F
	private void BeamCharge()
	{
		this.beamCharge.SetActive(true);
		this.lookAtPlayer = false;
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x00073844 File Offset: 0x00071A44
	private void BeamTurn()
	{
		this.bodyRotationOverride = true;
		this.lookAtPlayer = false;
		this.trackerOverrideAnimation = false;
		this.bodyRotationOverrideTarget = base.transform.position + Quaternion.AngleAxis(-90f, Vector3.up) * (new Vector3(this.Target.position.x, this.defaultPosition.y, this.Target.position.z) - base.transform.position);
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x000738D0 File Offset: 0x00071AD0
	private void BeamStart()
	{
		this.beamCharge.SetActive(false);
		this.beam.SetActive(true);
		this.beamTime = 10f;
		this.lookAtPlayer = false;
		this.trackerOverrideAnimation = false;
		this.notAtDefaultHeadRotation = false;
		this.previousHeadRotation = this.tracker.rotation;
		base.transform.position = this.defaultPosition;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00073938 File Offset: 0x00071B38
	private void BeamStop()
	{
		this.beam.SetActive(false);
		this.anim.SetBool("BeamAttack", false);
		this.anim.SetFloat("BeamSpeed", 0f);
		this.lookAtPlayer = true;
		this.bodyRotationOverride = false;
		this.bodyRotationOverrideTarget = this.Target.position;
		this.lcon.stopTail = false;
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000739A2 File Offset: 0x00071BA2
	private void StopAction()
	{
		this.inAction = false;
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x000739AB File Offset: 0x00071BAB
	private void Roar()
	{
		UltrakillEvent ultrakillEvent = this.onRoar;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x000739C4 File Offset: 0x00071BC4
	private void BigSplash()
	{
		Object.Instantiate<GameObject>(this.lcon.bigSplash, new Vector3(this.tracker.position.x, base.transform.position.y, this.tracker.position.z), Quaternion.LookRotation(Vector3.up));
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00073A24 File Offset: 0x00071C24
	public void GotParried()
	{
		this.BiteDamageStop();
		this.BiteResetRotation();
		this.anim.Play("BiteParried", -1, 0f);
		Object.Instantiate<AudioSource>(this.hurtSound, this.tracker.position, Quaternion.identity, this.tracker);
		MonoSingleton<StyleHUD>.Instance.AddPoints(500, "ultrakill.downtosize", null, this.lcon.eid, -1, "", "");
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00073AA0 File Offset: 0x00071CA0
	public void Death()
	{
		this.BiteDamageStop();
		this.anim.Play("Death", -1, 0f);
		this.anim.SetBool("Death", true);
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00073ACF File Offset: 0x00071CCF
	public void DeathEnd()
	{
		this.lcon.DeathEnd();
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00073ADC File Offset: 0x00071CDC
	public void HeadExplode()
	{
		this.headExploded = true;
		UltrakillEvent ultrakillEvent = this.onHeadExplode;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.beam.SetActive(false);
		this.beamCharge.SetActive(false);
		this.lcon.FinalExplosion();
	}

	// Token: 0x040014C0 RID: 5312
	[HideInInspector]
	public bool active = true;

	// Token: 0x040014C1 RID: 5313
	private Animator anim;

	// Token: 0x040014C2 RID: 5314
	[SerializeField]
	private Transform shootPoint;

	// Token: 0x040014C3 RID: 5315
	private bool projectileBursting;

	// Token: 0x040014C4 RID: 5316
	private int projectilesLeftInBurst;

	// Token: 0x040014C5 RID: 5317
	private int projectileBurstMaximum;

	// Token: 0x040014C6 RID: 5318
	private float projectileBurstCooldown;

	// Token: 0x040014C7 RID: 5319
	public float projectileSpreadAmount;

	// Token: 0x040014C8 RID: 5320
	[SerializeField]
	private GameObject beam;

	// Token: 0x040014C9 RID: 5321
	[SerializeField]
	private GameObject beamCharge;

	// Token: 0x040014CA RID: 5322
	private float beamTime;

	// Token: 0x040014CB RID: 5323
	private bool forceBeam;

	// Token: 0x040014CC RID: 5324
	public Transform tracker;

	// Token: 0x040014CD RID: 5325
	private List<Transform> trackerBones = new List<Transform>();

	// Token: 0x040014CE RID: 5326
	[SerializeField]
	private Transform tailBone;

	// Token: 0x040014CF RID: 5327
	private Transform[] tailBones;

	// Token: 0x040014D0 RID: 5328
	private bool inAction = true;

	// Token: 0x040014D1 RID: 5329
	private float attackCooldown;

	// Token: 0x040014D2 RID: 5330
	public bool lookAtPlayer;

	// Token: 0x040014D3 RID: 5331
	private bool predictPlayer;

	// Token: 0x040014D4 RID: 5332
	private Quaternion defaultHeadRotation = new Quaternion(-0.645012f, 0.2603323f, 0.6614516f, 0.2804788f);

	// Token: 0x040014D5 RID: 5333
	private Quaternion previousHeadRotation;

	// Token: 0x040014D6 RID: 5334
	private bool notAtDefaultHeadRotation;

	// Token: 0x040014D7 RID: 5335
	private bool trackerOverrideAnimation;

	// Token: 0x040014D8 RID: 5336
	private bool trackerIgnoreLimits;

	// Token: 0x040014D9 RID: 5337
	private float cantTurnToPlayer;

	// Token: 0x040014DA RID: 5338
	private float headRotationSpeedMultiplier = 1f;

	// Token: 0x040014DB RID: 5339
	private bool freezeTail;

	// Token: 0x040014DC RID: 5340
	private Vector3[] defaultTailPositions;

	// Token: 0x040014DD RID: 5341
	private Quaternion[] defaultTailRotations;

	// Token: 0x040014DE RID: 5342
	private bool rotateBody;

	// Token: 0x040014DF RID: 5343
	private Quaternion defaultBodyRotation;

	// Token: 0x040014E0 RID: 5344
	private Vector3 defaultPosition;

	// Token: 0x040014E1 RID: 5345
	private bool bodyRotationOverride;

	// Token: 0x040014E2 RID: 5346
	private Vector3 bodyRotationOverrideTarget;

	// Token: 0x040014E3 RID: 5347
	[SerializeField]
	private SwingCheck2 biteSwingCheck;

	// Token: 0x040014E4 RID: 5348
	[SerializeField]
	private GameObject parryHelper;

	// Token: 0x040014E5 RID: 5349
	public Vector3[] spawnPositions;

	// Token: 0x040014E6 RID: 5350
	public Vector3 centerSpawnPosition;

	// Token: 0x040014E7 RID: 5351
	private int previousSpawnPosition;

	// Token: 0x040014E8 RID: 5352
	private int previousAttack = -1;

	// Token: 0x040014E9 RID: 5353
	private int recentAttacks;

	// Token: 0x040014EA RID: 5354
	[HideInInspector]
	public LeviathanController lcon;

	// Token: 0x040014EB RID: 5355
	[SerializeField]
	private UltrakillEvent onRoar;

	// Token: 0x040014EC RID: 5356
	[SerializeField]
	private AudioSource projectileWindupSound;

	// Token: 0x040014ED RID: 5357
	[SerializeField]
	private AudioSource biteWindupSound;

	// Token: 0x040014EE RID: 5358
	[SerializeField]
	private AudioSource beamWindupSound;

	// Token: 0x040014EF RID: 5359
	[SerializeField]
	private AudioSource swingSound;

	// Token: 0x040014F0 RID: 5360
	[SerializeField]
	private AudioSource hurtSound;

	// Token: 0x040014F1 RID: 5361
	[SerializeField]
	private GameObject warningFlash;

	// Token: 0x040014F2 RID: 5362
	private bool headExploded;

	// Token: 0x040014F3 RID: 5363
	public UltrakillEvent onHeadExplode;
}
