using System;
using System.Collections.Generic;
using SettingsMenu.Components.Pages;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002F8 RID: 760
public class Minotaur : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x0600111D RID: 4381 RVA: 0x00085469 File Offset: 0x00083669
	private void Start()
	{
		this.GetValues();
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x00085474 File Offset: 0x00083674
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.rb = base.GetComponent<Rigidbody>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.anim = base.GetComponent<Animator>();
		this.mach = base.GetComponent<Machine>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.SetSpeed();
		if (this.tantrumOnSpawn)
		{
			this.currentAttacks++;
			this.previousAttack = 0;
			this.QuickTantrum();
		}
		base.Invoke("SlowUpdate", 0.1f);
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x0008554E File Offset: 0x0008374E
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x00085558 File Offset: 0x00083758
	private void SetSpeed()
	{
		this.GetValues();
		float num = 1f;
		if (this.difficulty >= 4)
		{
			num = 1.2f;
		}
		if (this.difficulty == 2)
		{
			num = 0.9f;
		}
		else if (this.difficulty == 1)
		{
			num = 0.85f;
		}
		else if (this.difficulty == 0)
		{
			num = 0.7f;
		}
		this.anim.speed = num * this.eid.totalSpeedModifier;
		this.nma.speed = 50f * this.anim.speed;
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x000855E4 File Offset: 0x000837E4
	private void Update()
	{
		if (this.dead)
		{
			this.deathTimer = Mathf.MoveTowards(this.deathTimer, 5f, Time.deltaTime);
			base.transform.position = new Vector3(this.deathPosition.x + Random.Range(-this.deathTimer / 10f, this.deathTimer / 10f), this.deathPosition.y + Random.Range(-this.deathTimer / 10f, this.deathTimer / 10f), this.deathPosition.z + Random.Range(-this.deathTimer / 10f, this.deathTimer / 10f));
			if (this.deathTimer < 4f && Random.Range(0f, 1f) < Time.deltaTime * 5f * this.deathTimer)
			{
				int num = Random.Range(0, this.deathTransforms.Count);
				if (this.deathTransforms[num] != null)
				{
					GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, this.eid, false);
					if (gore && this.gz && this.gz.goreZone)
					{
						gore.transform.position = this.deathTransforms[num].position;
						gore.transform.SetParent(this.gz.goreZone, true);
					}
					Bloodsplatter bloodsplatter;
					if (gore && gore.TryGetComponent<Bloodsplatter>(out bloodsplatter))
					{
						bloodsplatter.GetReady();
					}
				}
				else
				{
					this.deathTransforms.RemoveAt(num);
				}
			}
			if (this.deathTimer >= 1f)
			{
				if (!this.roar.isPlaying)
				{
					this.Roar(0.75f);
				}
				this.roar.pitch = 0.75f - (this.deathTimer - 1f) / 10f;
			}
			if (this.deathTimer == 5f)
			{
				this.BloodExplosion();
			}
			return;
		}
		if (this.trackTarget && (this.eid.target != null || this.tempTarget != null))
		{
			Transform transform = (this.tempTarget ? this.tempTarget : this.eid.target.targetTransform);
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(transform.position.x, base.transform.position.y, transform.position.z) - base.transform.position);
			float num2 = 5f;
			if (this.difficulty == 1)
			{
				num2 = 3f;
			}
			if (this.difficulty == 0)
			{
				num2 = 1.5f;
			}
			this.rb.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, (45f + Quaternion.Angle(base.transform.rotation, quaternion)) * num2 * this.trackSpeed * this.anim.speed * Time.deltaTime);
		}
		if (this.mach.gc.onGround && this.nma.enabled && this.nma.velocity.magnitude > 2f)
		{
			this.anim.SetBool("Running", true);
			this.anim.SetFloat("RunningSpeed", this.nma.velocity.magnitude / 25f);
		}
		else
		{
			this.anim.SetBool("Running", false);
			this.anim.SetFloat("RunningSpeed", 0f);
		}
		if (this.cooldown > 0f && !this.inAction)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		}
		if (this.ramCooldown > 0f)
		{
			this.ramCooldown = Mathf.MoveTowards(this.ramCooldown, 0f, Time.deltaTime);
		}
		if (this.eid.target != null && this.eid.target.isPlayer)
		{
			this.playerAirBias = Mathf.MoveTowards(this.playerAirBias, (float)(MonoSingleton<NewMovement>.Instance.gc.onGround ? 0 : 1), Time.deltaTime / 20f);
		}
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x00085A44 File Offset: 0x00083C44
	private void FixedUpdate()
	{
		if (this.dead)
		{
			return;
		}
		this.rb.isKinematic = !this.moveForward && this.mach.gc.onGround;
		if (this.moveForward)
		{
			this.rb.velocity = base.transform.forward * 30f * this.moveSpeed * this.anim.speed;
			if (!this.mach.gc.onGround)
			{
				this.rb.velocity += Vector3.up * this.rb.velocity.y;
			}
		}
		else if (!this.mach.gc.onGround)
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
		if (this.moveBreakSpeed != 0f)
		{
			this.moveSpeed = Mathf.MoveTowards(this.moveSpeed, 0f, Time.fixedDeltaTime * this.moveBreakSpeed);
		}
		if (this.ramTimer <= 0f)
		{
			this.anim.SetBool("Ramming", false);
			return;
		}
		RaycastHit raycastHit;
		if (Physics.SphereCast(base.transform.position + base.transform.up * 5f + base.transform.forward * 5f, 4f, base.transform.forward, out raycastHit, this.rb.velocity.magnitude * Time.fixedDeltaTime, LayerMaskDefaults.Get(LMD.Environment)))
		{
			Breakable breakable;
			if (raycastHit.transform.gameObject.CompareTag("Breakable") && raycastHit.transform.TryGetComponent<Breakable>(out breakable) && !breakable.playerOnly && !breakable.specialCaseOnly)
			{
				breakable.Break();
				return;
			}
			this.RamBonk(raycastHit.point);
			return;
		}
		else
		{
			this.ramTimer = Mathf.MoveTowards(this.ramTimer, 0f, Time.fixedDeltaTime);
			if (this.ramTimer == 0f)
			{
				this.anim.Play("RamSwing");
				this.Roar(this.roarShortClip, 1.5f);
				this.moveBreakSpeed = 5f;
				this.StopRam();
				return;
			}
			this.anim.SetBool("Ramming", true);
			return;
		}
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x00085CCC File Offset: 0x00083ECC
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.1f);
		if (!this.inAction && this.mach.gc.onGround)
		{
			if (this.eid.target == null)
			{
				return;
			}
			float num = Vector3.Distance(base.transform.position, this.eid.target.position);
			RaycastHit raycastHit;
			bool flag = !Physics.Raycast(base.transform.position + Vector3.up, this.eid.target.position - (base.transform.position + Vector3.up), out raycastHit, Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment));
			if (flag && this.cooldown <= 0f)
			{
				if (this.currentAttacks >= 3 || this.ramCooldown <= 0f)
				{
					this.Ram();
					this.ramCooldown = 15f;
					this.currentAttacks = 0;
					this.previousAttack = -1;
				}
				else if (num <= 20f)
				{
					int num2 = Random.Range(0, 3);
					if (num2 == this.previousAttack)
					{
						num2++;
					}
					if (num2 >= 3)
					{
						num2 = 0;
					}
					switch (num2)
					{
					case 0:
						this.HammerTantrum();
						break;
					case 1:
						if (num < 15f || Vector3.Distance(base.transform.position, this.eid.target.position + this.eid.target.rigidbody.velocity.normalized) < num + 0.2f)
						{
							this.HammerSmash();
						}
						break;
					case 2:
						if (Random.Range(0f, 1f) > this.playerAirBias)
						{
							this.playerAirBias = 1f;
							this.MeatPool();
						}
						else
						{
							this.playerAirBias = 0f;
							this.MeatCloud();
						}
						break;
					}
					if (this.inAction)
					{
						this.previousAttack = num2;
						this.currentAttacks++;
					}
				}
				if (this.inAction)
				{
					return;
				}
			}
			if (!this.chaseTarget)
			{
				if (num > 20f)
				{
					this.chaseTarget = true;
				}
				else if (!flag && !raycastHit.transform.gameObject.CompareTag("Breakable"))
				{
					this.chaseTarget = true;
				}
			}
			if (this.chaseTarget)
			{
				if (flag && num <= 15f)
				{
					this.chaseTarget = false;
					if (this.nma && this.nma.enabled && this.nma.isOnNavMesh)
					{
						this.nma.SetDestination(base.transform.position);
						return;
					}
				}
				else if (this.nma && this.nma.enabled && this.nma.isOnNavMesh)
				{
					this.nma.SetDestination(this.eid.target.position);
					return;
				}
			}
		}
		else
		{
			this.chaseTarget = false;
		}
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x00085FF0 File Offset: 0x000841F0
	private void Ram()
	{
		this.anim.Play("RamWindup", -1, 0f);
		this.inAction = true;
		this.trackTarget = true;
		this.Roar(this.exhaleClip, 1f);
		this.nma.enabled = false;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x00086040 File Offset: 0x00084240
	private void RamStart()
	{
		this.anim.Play("RamRun", -1, 0f);
		this.anim.SetBool("Ramming", true);
		this.moveSpeed = 2f;
		this.moveForward = true;
		this.trackSpeed = 0.25f;
		this.ramTimer = 3f;
		this.ramStuff.SetActive(true);
		GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, this.ramStuff.transform.position + this.ramStuff.transform.forward * 1.5f, base.transform.rotation);
		gameObject.transform.localScale *= 15f;
		gameObject.transform.SetParent(this.ramStuff.transform, true);
		this.mach.ParryableCheck(false);
		this.Roar(0.75f);
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x0008613C File Offset: 0x0008433C
	private void RamBonk(Vector3 point)
	{
		if (point != Vector3.zero)
		{
			Object.Instantiate<GameObject>(this.hammerImpact, point, Quaternion.identity);
			base.transform.LookAt(new Vector3(point.x, base.transform.position.y, point.z));
		}
		this.anim.Play("RamBonk", -1, 0f);
		this.anim.SetBool("Ramming", false);
		this.moveForward = false;
		this.ramTimer = 0f;
		this.trackTarget = false;
		this.ramStuff.SetActive(false);
		this.mach.parryable = false;
		this.eid.hitter = "enemy";
		this.mach.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, 6f, 0f, null, false);
		MonoSingleton<CameraController>.Instance.CameraShake(2f);
		this.Roar(this.longGruntClip, 2f);
	}

	// Token: 0x06001127 RID: 4391 RVA: 0x00086243 File Offset: 0x00084443
	private void StopRam()
	{
		this.anim.SetBool("Ramming", false);
		this.ramStuff.SetActive(false);
		this.mach.parryable = false;
		this.ramTimer = 0f;
	}

	// Token: 0x06001128 RID: 4392 RVA: 0x0008627C File Offset: 0x0008447C
	private void MeatCloud()
	{
		this.nma.enabled = false;
		this.anim.Play("MeatHigh", -1, 0f);
		this.inAction = true;
		this.trackTarget = true;
		this.Roar(this.longGruntClip, 1f);
	}

	// Token: 0x06001129 RID: 4393 RVA: 0x000862CC File Offset: 0x000844CC
	private void MeatPool()
	{
		this.nma.enabled = false;
		this.anim.Play("MeatLow", -1, 0f);
		this.inAction = true;
		this.trackTarget = true;
		this.Roar(this.longGruntClip, 1f);
	}

	// Token: 0x0600112A RID: 4394 RVA: 0x0008631A File Offset: 0x0008451A
	private void HandBlood()
	{
		Object.Instantiate<GameObject>(this.handBlood, this.meatInHand.transform.position, Quaternion.identity);
	}

	// Token: 0x0600112B RID: 4395 RVA: 0x0008633D File Offset: 0x0008453D
	private void MeatSpawn()
	{
		this.meatInHand.SetActive(true);
		this.HandBlood();
	}

	// Token: 0x0600112C RID: 4396 RVA: 0x00086354 File Offset: 0x00084554
	private void MeatExplode()
	{
		this.meatInHand.SetActive(false);
		this.HandBlood();
		GameObject gameObject = Object.Instantiate<GameObject>((this.difficulty >= 4) ? this.toxicCloudLong : this.toxicCloud, this.meatInHand.transform.position, Quaternion.identity);
		gameObject.transform.SetParent(this.gz.transform, true);
		if (this.difficulty == 1)
		{
			gameObject.transform.localScale *= 0.85f;
			return;
		}
		if (this.difficulty == 0)
		{
			gameObject.transform.localScale *= 0.75f;
		}
	}

	// Token: 0x0600112D RID: 4397 RVA: 0x00086404 File Offset: 0x00084604
	private void MeatSplash()
	{
		this.meatInHand.SetActive(false);
		this.HandBlood();
		GameObject gameObject = Object.Instantiate<GameObject>((this.difficulty >= 4) ? this.goopLong : this.goop, new Vector3(this.meatInHand.transform.position.x, base.transform.position.y, this.meatInHand.transform.position.z), Quaternion.identity);
		gameObject.transform.SetParent(this.gz.transform, true);
		if (this.difficulty == 1)
		{
			gameObject.transform.localScale *= 0.85f;
			return;
		}
		if (this.difficulty == 0)
		{
			gameObject.transform.localScale *= 0.75f;
		}
	}

	// Token: 0x0600112E RID: 4398 RVA: 0x000864E3 File Offset: 0x000846E3
	private void MeatThrowThrow()
	{
		this.meatInHand.SetActive(false);
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x000864F4 File Offset: 0x000846F4
	private void HammerSmash()
	{
		if (this.dead)
		{
			return;
		}
		this.nma.enabled = false;
		this.anim.Play("HammerSmash", -1, 0f);
		this.inAction = true;
		this.trackTarget = true;
		this.Roar(this.squealClip, 0.75f);
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0008654C File Offset: 0x0008474C
	private void HammerTantrum()
	{
		if (this.dead)
		{
			return;
		}
		this.nma.enabled = false;
		this.anim.Play("HammerTantrum", -1, 0f);
		this.inAction = true;
		this.trackTarget = true;
		this.Roar(1.25f);
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0008659D File Offset: 0x0008479D
	public void QuickTantrum()
	{
		if (this.dead)
		{
			return;
		}
		this.nma.enabled = false;
		this.anim.Play("HammerTantrum", -1, 0.275f);
		this.inAction = true;
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x000865D4 File Offset: 0x000847D4
	private void HammerSwingStart()
	{
		if (this.dead)
		{
			return;
		}
		this.nma.enabled = false;
		SwingCheck2[] array = this.hammerSwingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStart();
		}
		this.hammerTrail.emitting = true;
		this.moveForward = true;
		this.trackTarget = false;
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x00086630 File Offset: 0x00084830
	private void HammerImpact()
	{
		Object.Instantiate<GameObject>(this.hammerImpact, new Vector3(this.hammerPoint.position.x, base.transform.position.y + 0.25f, this.hammerPoint.position.z), Quaternion.identity);
		MonoSingleton<CameraController>.Instance.CameraShake(0.25f);
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x00086698 File Offset: 0x00084898
	private void HammerExplosion(int size = 0)
	{
		foreach (Explosion explosion in Object.Instantiate<GameObject>((size == 0) ? this.hammerExplosion : this.hammerBigExplosion, new Vector3(this.hammerPoint.position.x, base.transform.position.y + 0.25f, this.hammerPoint.position.z), Quaternion.identity).GetComponentsInChildren<Explosion>())
		{
			explosion.toIgnore.Add(EnemyType.Minotaur);
			explosion.maxSize *= ((size == 0) ? 2f : 1.75f);
			explosion.speed *= ((size == 0) ? 2f : 1.75f);
		}
		MonoSingleton<CameraController>.Instance.CameraShake(1.5f);
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x00086768 File Offset: 0x00084968
	private void HammerSwingStop(int startTrackingTarget = 0)
	{
		SwingCheck2[] array = this.hammerSwingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
		this.hammerTrail.emitting = false;
		this.moveForward = false;
		this.trackTarget = startTrackingTarget >= 1;
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x000867B2 File Offset: 0x000849B2
	private void HammerSwingStopImpact(int startTrackingTarget = 0)
	{
		this.HammerImpact();
		this.HammerSwingStop(startTrackingTarget);
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x000867C1 File Offset: 0x000849C1
	private void StartTracking()
	{
		this.trackTarget = true;
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x000867CA File Offset: 0x000849CA
	private void StopMoving()
	{
		this.moveForward = false;
		this.moveSpeed = 1f;
		this.moveBreakSpeed = 0f;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x000867EC File Offset: 0x000849EC
	private void GotParried()
	{
		if (this.ramTimer > 0f)
		{
			this.RamBonk(MonoSingleton<NewMovement>.Instance.transform.position);
		}
		this.anim.Play("RamParried");
		this.nma.enabled = false;
		this.moveForward = true;
		this.moveSpeed = -2.5f;
		this.moveBreakSpeed = 5f;
		MonoSingleton<CameraController>.Instance.CameraShake(3f);
		this.eid.hitter = "";
		this.mach.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, 20f, 0f, null, false);
		this.ramCooldown = 30f;
		this.Roar(2.5f);
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x000868B0 File Offset: 0x00084AB0
	public void GotSlammed()
	{
		if (this.ramTimer > 0f)
		{
			this.RamBonk(base.transform.position + base.transform.forward);
		}
		this.anim.Play("RamParried");
		this.nma.enabled = false;
		this.moveForward = true;
		this.moveSpeed = 2.5f;
		this.moveBreakSpeed = 1.5f;
		MonoSingleton<CameraController>.Instance.CameraShake(3f);
		this.eid.hitter = "";
		this.mach.GetHurt(base.GetComponentInChildren<EnemyIdentifierIdentifier>().gameObject, Vector3.zero, 20f, 0f, null, false);
		MonoSingleton<FistControl>.Instance.currentPunch.Parry(false, this.eid, "");
		this.mach.parryable = false;
		this.ramCooldown = 30f;
		this.Roar(2.5f);
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x000869A8 File Offset: 0x00084BA8
	private void StopAction()
	{
		if (this.mach.gc && this.mach.gc.onGround)
		{
			this.nma.enabled = true;
		}
		this.inAction = false;
		this.moveForward = false;
		this.mach.parryable = false;
		this.trackTarget = true;
		this.moveSpeed = 1f;
		this.moveBreakSpeed = 0f;
		this.trackSpeed = 1f;
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00086A28 File Offset: 0x00084C28
	public void TargetBeenHit()
	{
		if (this.ramTimer > 0f)
		{
			this.anim.Play("RamSwing");
			this.moveBreakSpeed = 5f;
			this.StopRam();
			this.Roar(this.roarShortClip, 1.5f);
		}
		SwingCheck2[] array = this.hammerSwingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00086A94 File Offset: 0x00084C94
	public void Death()
	{
		if (this.dead)
		{
			return;
		}
		this.nma.enabled = false;
		this.dead = true;
		this.deathPosition = base.transform.position;
		this.deathTransforms.AddRange(base.GetComponentsInChildren<Transform>());
		this.HammerSwingStop(0);
		this.StopAction();
		this.meatInHand.SetActive(false);
		this.anim.Play("Death", -1, 0f);
		this.anim.SetBool("Dead", true);
		this.anim.speed = 1f;
		this.inAction = true;
		this.trackTarget = false;
		this.roar.Stop();
		base.Invoke("Roar", 0.9f);
		MonoSingleton<TimeController>.Instance.SlowDown(0.001f);
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x00086B68 File Offset: 0x00084D68
	private void BloodExplosion()
	{
		List<Transform> list = new List<Transform>();
		BloodsplatterManager instance = MonoSingleton<BloodsplatterManager>.Instance;
		foreach (Transform transform in this.deathTransforms)
		{
			if (transform != null && Random.Range(0f, 1f) < 0.33f)
			{
				GameObject gore = instance.GetGore(GoreType.Head, this.eid, false);
				if (gore)
				{
					gore.transform.position = transform.position;
					if (this.gz != null && this.gz.goreZone != null)
					{
						gore.transform.SetParent(this.gz.goreZone, true);
					}
					Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
					if (component != null)
					{
						component.GetReady();
					}
				}
			}
			else if (transform == null)
			{
				list.Add(transform);
			}
		}
		if (list.Count > 0)
		{
			foreach (Transform transform2 in list)
			{
				this.deathTransforms.Remove(transform2);
			}
			list.Clear();
		}
		if (GraphicsSettings.bloodEnabled && base.gameObject.activeInHierarchy)
		{
			for (int i = 0; i < 40; i++)
			{
				if (i < 30)
				{
					GameObject gameObject = instance.GetGib(BSType.gib);
					if (gameObject && this.gz && this.gz.gibZone)
					{
						if (this.gz && this.gz.gibZone)
						{
							this.mach.ReadyGib(gameObject, this.deathTransforms[Random.Range(0, this.deathTransforms.Count)].gameObject);
						}
						gameObject.transform.localScale *= Random.Range(4f, 7f);
					}
					else
					{
						i = 30;
					}
				}
				else if (i < 35)
				{
					GameObject gameObject = instance.GetGib(BSType.eyeball);
					if (gameObject && this.gz && this.gz.gibZone)
					{
						if (this.gz && this.gz.gibZone)
						{
							this.mach.ReadyGib(gameObject, this.deathTransforms[Random.Range(0, this.deathTransforms.Count)].gameObject);
						}
						gameObject.transform.localScale *= Random.Range(3f, 6f);
					}
					else
					{
						i = 35;
					}
				}
				else
				{
					GameObject gameObject = instance.GetGib(BSType.brainChunk);
					if (!gameObject || !this.gz || !this.gz.gibZone)
					{
						break;
					}
					if (this.gz && this.gz.gibZone)
					{
						this.mach.ReadyGib(gameObject, this.deathTransforms[Random.Range(0, this.deathTransforms.Count)].gameObject);
					}
					gameObject.transform.localScale *= Random.Range(3f, 4f);
				}
			}
		}
		if (this.gz != null && this.gz.checkpoint != null)
		{
			this.gz.AddDeath();
			this.gz.checkpoint.sm.kills++;
		}
		else
		{
			MonoSingleton<StatsManager>.Instance.kills++;
		}
		ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
		if (componentInParent != null)
		{
			componentInParent.AddDeadEnemy();
		}
		UltrakillEvent ultrakillEvent = this.onDeath;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		MonoSingleton<TimeController>.Instance.SlowDown(0.0001f);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00086FCC File Offset: 0x000851CC
	private void Roar()
	{
		this.Roar(this.roarClip, 1f);
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x00086FDF File Offset: 0x000851DF
	private void Roar(float pitch = 1f)
	{
		this.Roar(this.roarClip, pitch);
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x00086FEE File Offset: 0x000851EE
	private void Roar(AudioClip clip, float pitch = 1f)
	{
		this.roar.clip = clip;
		this.roar.pitch = Random.Range(pitch - 0.1f, pitch + 0.1f);
		this.roar.Play();
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x00087025 File Offset: 0x00085225
	private void BodyImpact()
	{
		Object.Instantiate<GameObject>(this.fallEffect, base.transform.position, Quaternion.identity);
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
	}

	// Token: 0x0400175F RID: 5983
	private Rigidbody rb;

	// Token: 0x04001760 RID: 5984
	private EnemyIdentifier eid;

	// Token: 0x04001761 RID: 5985
	private Animator anim;

	// Token: 0x04001762 RID: 5986
	private Machine mach;

	// Token: 0x04001763 RID: 5987
	private NavMeshAgent nma;

	// Token: 0x04001764 RID: 5988
	private int difficulty;

	// Token: 0x04001765 RID: 5989
	private bool gotValues;

	// Token: 0x04001766 RID: 5990
	private bool dead;

	// Token: 0x04001767 RID: 5991
	private float cooldown;

	// Token: 0x04001768 RID: 5992
	private int previousAttack = -1;

	// Token: 0x04001769 RID: 5993
	private int currentAttacks;

	// Token: 0x0400176A RID: 5994
	private float ramCooldown = 15f;

	// Token: 0x0400176B RID: 5995
	private bool inAction;

	// Token: 0x0400176C RID: 5996
	private bool moveForward;

	// Token: 0x0400176D RID: 5997
	private float moveSpeed = 1f;

	// Token: 0x0400176E RID: 5998
	private float moveBreakSpeed;

	// Token: 0x0400176F RID: 5999
	private bool trackTarget;

	// Token: 0x04001770 RID: 6000
	private float trackSpeed = 1f;

	// Token: 0x04001771 RID: 6001
	private Transform tempTarget;

	// Token: 0x04001772 RID: 6002
	private bool chaseTarget;

	// Token: 0x04001773 RID: 6003
	[SerializeField]
	private AudioSource roar;

	// Token: 0x04001774 RID: 6004
	[SerializeField]
	private AudioClip roarClip;

	// Token: 0x04001775 RID: 6005
	[SerializeField]
	private AudioClip roarShortClip;

	// Token: 0x04001776 RID: 6006
	[SerializeField]
	private AudioClip squealClip;

	// Token: 0x04001777 RID: 6007
	[SerializeField]
	private AudioClip longGruntClip;

	// Token: 0x04001778 RID: 6008
	[SerializeField]
	private AudioClip exhaleClip;

	// Token: 0x04001779 RID: 6009
	[SerializeField]
	private SwingCheck2[] hammerSwingChecks;

	// Token: 0x0400177A RID: 6010
	[SerializeField]
	private TrailRenderer hammerTrail;

	// Token: 0x0400177B RID: 6011
	[SerializeField]
	private Transform hammerPoint;

	// Token: 0x0400177C RID: 6012
	[SerializeField]
	private GameObject hammerImpact;

	// Token: 0x0400177D RID: 6013
	[SerializeField]
	private GameObject hammerExplosion;

	// Token: 0x0400177E RID: 6014
	[SerializeField]
	private GameObject hammerBigExplosion;

	// Token: 0x0400177F RID: 6015
	public bool tantrumOnSpawn;

	// Token: 0x04001780 RID: 6016
	[SerializeField]
	private GameObject meatInHand;

	// Token: 0x04001781 RID: 6017
	[SerializeField]
	private GameObject handBlood;

	// Token: 0x04001782 RID: 6018
	[SerializeField]
	private GameObject toxicCloud;

	// Token: 0x04001783 RID: 6019
	[SerializeField]
	private GameObject toxicCloudLong;

	// Token: 0x04001784 RID: 6020
	[SerializeField]
	private GameObject goop;

	// Token: 0x04001785 RID: 6021
	[SerializeField]
	private GameObject goopLong;

	// Token: 0x04001786 RID: 6022
	[HideInInspector]
	public float ramTimer;

	// Token: 0x04001787 RID: 6023
	[SerializeField]
	private GameObject ramStuff;

	// Token: 0x04001788 RID: 6024
	[SerializeField]
	private GameObject fallEffect;

	// Token: 0x04001789 RID: 6025
	private Vector3 deathPosition;

	// Token: 0x0400178A RID: 6026
	private List<Transform> deathTransforms = new List<Transform>();

	// Token: 0x0400178B RID: 6027
	private float deathTimer;

	// Token: 0x0400178C RID: 6028
	private GoreZone gz;

	// Token: 0x0400178D RID: 6029
	public UltrakillEvent onDeath;

	// Token: 0x0400178E RID: 6030
	private float playerAirBias = 0.5f;
}
