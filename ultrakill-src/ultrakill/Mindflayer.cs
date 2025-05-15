using System;
using Sandbox;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020002F2 RID: 754
public class Mindflayer : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06001096 RID: 4246 RVA: 0x0007F111 File Offset: 0x0007D311
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0007F120 File Offset: 0x0007D320
	private void Awake()
	{
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.anim = base.GetComponent<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		if (Random.Range(0, 50) != 8)
		{
			return;
		}
		if (this.smr != null && this.ensim != null)
		{
			this.smr.sharedMesh = this.maleMesh;
			this.smr.material = this.maleMaterial;
			this.ensim.enragedMaterial = this.maleMaterialEnraged;
		}
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0007F1B8 File Offset: 0x0007D3B8
	private void Start()
	{
		this.cooldown = 2f;
		this.decoyThreshold = this.mach.health - (float)this.teleportInterval;
		this.environmentMask = LayerMaskDefaults.Get(LMD.Environment);
		this.sc = base.GetComponentInChildren<SwingCheck2>();
		this.lr = base.GetComponent<LineRenderer>();
		this.lr.enabled = false;
		if (this.tempBeam != null)
		{
			Object.Destroy(this.tempBeam);
		}
		this.RandomizeDirection();
		this.SetSpeed();
		if (this.dying)
		{
			this.Death();
		}
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0007F24C File Offset: 0x0007D44C
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0007F254 File Offset: 0x0007D454
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
		this.eid.immuneToFriendlyFire = this.difficulty >= 4;
		if (this.difficulty >= 4)
		{
			this.cooldownMultiplier = 2.5f;
			this.anim.speed = 1.5f;
		}
		else if (this.difficulty == 3)
		{
			this.cooldownMultiplier = 1.5f;
			this.anim.speed = 1.35f;
		}
		else if (this.difficulty < 2)
		{
			this.cooldownMultiplier = 0.75f;
			if (this.difficulty == 1)
			{
				this.anim.speed = 0.75f;
			}
			else if (this.difficulty == 0)
			{
				this.anim.speed = 0.5f;
			}
		}
		else
		{
			this.cooldownMultiplier = 1f;
			this.anim.speed = 1f;
		}
		this.cooldownMultiplier *= this.eid.totalSpeedModifier;
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.defaultAnimSpeed = this.anim.speed;
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0007F3E4 File Offset: 0x0007D5E4
	private void OnDisable()
	{
		this.StopAction();
		if (this.sc)
		{
			this.DamageEnd();
		}
		if (this.tempBeam != null)
		{
			Object.Destroy(this.tempBeam);
		}
		this.chargeParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
		this.overrideRotation = false;
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0007F438 File Offset: 0x0007D638
	private void UpdateRigidbodySettings()
	{
		if (this.target == null)
		{
			this.rb.drag = 3f;
			this.rb.angularDrag = 3f;
			return;
		}
		this.rb.drag = 0f;
		this.rb.angularDrag = 0f;
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0007F490 File Offset: 0x0007D690
	private void Update()
	{
		this.UpdateRigidbodySettings();
		if (this.vibrate)
		{
			this.model.localPosition = new Vector3(this.origPos.x + Random.Range(-0.2f, 0.2f), this.origPos.y + Random.Range(-0.2f, 0.2f), this.origPos.z + Random.Range(-0.2f, 0.2f));
		}
		if (this.launched)
		{
			this.model.Rotate(Vector3.right, -1200f * Time.deltaTime, Space.Self);
		}
		if (this.target == null)
		{
			if (this.beaming)
			{
				this.StopBeam();
			}
			if (this.inAction)
			{
				this.StopAction();
			}
			return;
		}
		if (this.active)
		{
			bool flag = Vector3.Distance(base.transform.position, this.target.position) > 25f || base.transform.position.y > this.target.position.y + 15f || Physics.Raycast(base.transform.position, this.target.position - base.transform.position, Vector3.Distance(base.transform.position, this.target.position), this.environmentMask);
			if (this.spawnAttackDelay > 0f)
			{
				this.spawnAttackDelay = Mathf.MoveTowards(this.spawnAttackDelay, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			else if (Vector3.Distance(this.target.position, base.transform.position) < 5f && !this.inAction)
			{
				this.MeleeAttack();
			}
			this.timeSinceMelee += Time.deltaTime * this.eid.totalSpeedModifier;
			if (((this.difficulty > 2 && this.timeSinceMelee > 10f) || (this.difficulty == 2 && this.timeSinceMelee > 15f)) && !this.inAction)
			{
				this.Teleport(true);
				this.timeSinceMelee = 5f;
				if (Vector3.Distance(this.target.position, base.transform.position) < 8f)
				{
					this.MeleeAttack();
				}
			}
			if (this.cooldown > 0f)
			{
				this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.cooldownMultiplier);
			}
			else if (!this.inAction && !flag)
			{
				if (this.beamCooldown || (Random.Range(0f, 1f) < 0.25f && !this.beamNext))
				{
					if (!this.beamCooldown)
					{
						this.beamNext = true;
					}
					this.beamCooldown = false;
					this.HomingAttack();
				}
				else
				{
					this.BeamAttack();
				}
			}
			if (flag)
			{
				this.outOfSightTime = Mathf.MoveTowards(this.outOfSightTime, 3f, Time.deltaTime * this.eid.totalSpeedModifier);
				if (this.outOfSightTime >= 3f && !this.inAction)
				{
					this.Teleport(false);
				}
			}
			else
			{
				this.outOfSightTime = Mathf.MoveTowards(this.outOfSightTime, 0f, Time.deltaTime * 2f * this.eid.totalSpeedModifier);
			}
			if (!this.overrideRotation)
			{
				Quaternion quaternion = Quaternion.LookRotation(this.target.position - base.transform.position, Vector3.up);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * (10f * Quaternion.Angle(quaternion, base.transform.rotation) + 2f) * this.eid.totalSpeedModifier);
			}
			else
			{
				Quaternion quaternion2 = Quaternion.LookRotation(this.overrideTarget - base.transform.position, Vector3.up);
				if (!this.beaming)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion2, Time.deltaTime * (100f * Quaternion.Angle(quaternion2, base.transform.rotation) + 10f) * this.eid.totalSpeedModifier);
				}
				else
				{
					float num = 1f;
					if (this.difficulty == 1)
					{
						num = 0.85f;
					}
					else if (this.difficulty == 0)
					{
						num = 0.65f;
					}
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion2, Time.deltaTime * this.beamDistance * num * this.eid.totalSpeedModifier);
					if (Quaternion.Angle(base.transform.rotation, quaternion2) < 1f)
					{
						this.StopBeam();
					}
				}
			}
			if (this.decoyThreshold > this.mach.health && this.decoyThreshold > 0f && !this.dontTeleport)
			{
				Object.Instantiate<GameObject>(this.bigHurt, base.transform.position, Quaternion.identity);
				while (this.decoyThreshold > this.mach.health)
				{
					this.decoyThreshold -= (float)this.teleportInterval;
				}
				this.Teleport(false);
			}
			if (this.difficulty > 2 && this.mach.health < 15f && !this.enraged)
			{
				this.Enrage();
			}
		}
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0007F9F8 File Offset: 0x0007DBF8
	private void FixedUpdate()
	{
		if (this.launched)
		{
			RaycastHit[] array = Physics.SphereCastAll(base.transform.position, 1f, base.transform.forward * -1f, 50f * Time.fixedDeltaTime, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment));
			bool flag = false;
			bool flag2 = false;
			RaycastHit[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				RaycastHit raycastHit = array2[i];
				if (LayerMaskDefaults.IsMatchingLayer(raycastHit.collider.gameObject.layer, LMD.Environment))
				{
					flag = true;
					break;
				}
				foreach (Collider collider in this.ownColliders)
				{
					if (raycastHit.collider == collider)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					flag = true;
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					if (!raycastHit.collider.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid || enemyIdentifierIdentifier.eid.dead)
					{
						break;
					}
					MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.strike", null, this.eid, -1, "", "");
					Gutterman gutterman;
					if (enemyIdentifierIdentifier.eid.enemyType == EnemyType.Gutterman && enemyIdentifierIdentifier.eid.TryGetComponent<Gutterman>(out gutterman) && gutterman.hasShield)
					{
						gutterman.ShieldBreak(true, false);
						break;
					}
					break;
				}
				else
				{
					flag2 = false;
					i++;
				}
			}
			if (flag)
			{
				base.CancelInvoke("DeathExplosion");
				this.DeathExplosion();
			}
			else
			{
				this.rb.MovePosition(base.transform.position - base.transform.forward * 50f * Time.fixedDeltaTime);
			}
		}
		if (this.target == null)
		{
			return;
		}
		if (this.inAction)
		{
			if (this.goForward && !Physics.Raycast(base.transform.position, base.transform.forward, 1f, this.environmentMask))
			{
				this.rb.MovePosition(base.transform.position + base.transform.forward * 75f * Time.fixedDeltaTime * this.anim.speed);
			}
			return;
		}
		if (this.goingLeft)
		{
			if (!Physics.Raycast(base.transform.position, base.transform.right * -1f, 1f, this.environmentMask))
			{
				this.rb.MovePosition(base.transform.position + base.transform.right * -5f * Time.fixedDeltaTime * this.anim.speed);
				return;
			}
			this.goingLeft = false;
			return;
		}
		else
		{
			if (!Physics.Raycast(base.transform.position, base.transform.right, 1f, this.environmentMask))
			{
				this.rb.MovePosition(base.transform.position + base.transform.right * 5f * Time.fixedDeltaTime * this.anim.speed);
				return;
			}
			this.goingLeft = true;
			return;
		}
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0007FD50 File Offset: 0x0007DF50
	private void RandomizeDirection()
	{
		if (Random.Range(0f, 1f) > 0.5f)
		{
			this.goingLeft = true;
			return;
		}
		this.goingLeft = false;
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0007FD78 File Offset: 0x0007DF78
	public void Teleport(bool closeRange = false)
	{
		this.outOfSightTime = 0f;
		if (this.eid && this.eid.drillers.Count > 0)
		{
			return;
		}
		if (this.teleportAttempts == 0)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.decoy, base.transform.GetChild(0).position, base.transform.GetChild(0).rotation);
			Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
			AnimatorStateInfo currentAnimatorStateInfo = this.anim.GetCurrentAnimatorStateInfo(0);
			componentInChildren.Play(currentAnimatorStateInfo.shortNameHash, 0, currentAnimatorStateInfo.normalizedTime);
			componentInChildren.speed = 0f;
			if (this.enraged)
			{
				gameObject.GetComponent<MindflayerDecoy>().enraged = true;
			}
		}
		Vector3 normalized = Random.onUnitSphere.normalized;
		if (normalized.y < 0f)
		{
			normalized.y *= -1f;
		}
		float num = (float)Random.Range(8, 15);
		if (closeRange)
		{
			num = (float)Random.Range(5, 8);
		}
		Vector3 vector = this.target.position + Vector3.up;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.target.position + Vector3.up, normalized, out raycastHit, num, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			vector = raycastHit.point - normalized * 3f;
		}
		else
		{
			vector = this.target.position + Vector3.up + normalized * num;
		}
		bool flag = false;
		bool flag2 = false;
		RaycastHit raycastHit2;
		if (Physics.Raycast(vector, Vector3.up, out raycastHit2, 5f, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			flag = true;
		}
		RaycastHit raycastHit3;
		if (Physics.Raycast(vector, Vector3.down, out raycastHit3, 5f, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			flag2 = true;
		}
		bool flag3 = false;
		Vector3 vector2 = base.transform.position;
		if (flag && flag2)
		{
			if (Vector3.Distance(raycastHit2.point, raycastHit3.point) > 7f)
			{
				vector2 = new Vector3(vector.x, (raycastHit3.point.y + raycastHit2.point.y) / 2f, vector.z);
				flag3 = true;
			}
			else
			{
				this.teleportAttempts++;
				if (this.teleportAttempts <= 10)
				{
					this.Teleport(false);
				}
			}
		}
		else
		{
			flag3 = true;
			if (flag)
			{
				vector2 = raycastHit2.point + Vector3.down * (float)Random.Range(5, 10);
			}
			else if (flag2)
			{
				vector2 = raycastHit3.point + Vector3.up * (float)Random.Range(5, 10);
			}
			else
			{
				vector2 = vector;
			}
		}
		if (flag3)
		{
			if (Physics.CheckSphere(vector2, 0.1f, this.environmentMask, QueryTriggerInteraction.Ignore) || Physics.CheckSphere(vector2, 0.1f, 2097152, QueryTriggerInteraction.Collide))
			{
				this.Teleport(false);
				return;
			}
			if (this.eid.hooked)
			{
				MonoSingleton<HookArm>.Instance.StopThrow(1f, true);
			}
			base.transform.position = vector2;
			this.teleportAttempts = 0;
			Object.Instantiate<GameObject>(this.teleportSound, base.transform.position, Quaternion.identity);
		}
		if (this.goingLeft)
		{
			this.goingLeft = false;
			return;
		}
		this.goingLeft = true;
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x000800BC File Offset: 0x0007E2BC
	public void Death()
	{
		this.active = false;
		this.inAction = true;
		this.chargeParticle.Play();
		this.anim.SetTrigger("Death");
		base.Invoke("DeathExplosion", 2f / this.eid.totalSpeedModifier);
		this.origPos = this.model.localPosition;
		this.vibrate = true;
		this.dying = true;
		if (this.currentEnrageEffect)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
		for (int i = 0; i < this.tentacles.Length; i++)
		{
			TrailRenderer component = this.tentacles[i].GetComponent<TrailRenderer>();
			if (component)
			{
				component.enabled = false;
			}
		}
		if (this.tempBeam != null)
		{
			Object.Destroy(this.tempBeam);
		}
		if (this.lr.enabled)
		{
			this.lr.enabled = false;
		}
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x000801A8 File Offset: 0x0007E3A8
	private void DeathExplosion()
	{
		Object.Instantiate<GameObject>(this.deathExplosion.ToAsset(), base.transform.position, Quaternion.identity);
		if (this.eid.drillers.Count > 0)
		{
			for (int i = this.eid.drillers.Count - 1; i >= 0; i--)
			{
				Object.Destroy(this.eid.drillers[i].gameObject);
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0008022C File Offset: 0x0007E42C
	public void DeadLaunch(Vector3 direction)
	{
		this.launched = true;
		base.transform.LookAt(base.transform.position - direction);
		this.anim.Play("BeamHold", 0, 0f);
		this.ownColliders = base.GetComponentsInChildren<Collider>();
		base.CancelInvoke("DeathExplosion");
		base.Invoke("DeathExplosion", 2f / this.eid.totalSpeedModifier);
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000802A5 File Offset: 0x0007E4A5
	private void HomingAttack()
	{
		this.inAction = true;
		this.dontTeleport = true;
		this.chargeParticle.Play();
		this.anim.SetTrigger("HomingAttack");
		Object.Instantiate<GameObject>(this.windUp, base.transform);
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x000802E4 File Offset: 0x0007E4E4
	private void BeamAttack()
	{
		this.inAction = true;
		this.chargeParticle.Play();
		this.dontTeleport = true;
		this.beamCooldown = true;
		this.beamNext = false;
		this.anim.SetTrigger("BeamAttack");
		Object.Instantiate<GameObject>(this.windUp, base.transform).GetComponent<AudioSource>().pitch = 1.5f;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x00080348 File Offset: 0x0007E548
	private void MeleeAttack()
	{
		this.timeSinceMelee = 0f;
		this.inAction = true;
		this.anim.SetTrigger("MeleeAttack");
		Object.Instantiate<GameObject>(this.windUpSmall, base.transform);
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x00080380 File Offset: 0x0007E580
	public void SwingStart()
	{
		Object.Instantiate<GameObject>(this.warningFlash, this.eid.weakPoint.transform).transform.localScale *= 8f;
		this.mach.ParryableCheck(false);
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x000803CE File Offset: 0x0007E5CE
	public void DamageStart()
	{
		this.sc.DamageStart();
		this.goForward = true;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x000803E2 File Offset: 0x0007E5E2
	public void DamageEnd()
	{
		this.sc.DamageStop();
		this.mach.parryable = false;
		this.goForward = false;
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x00080404 File Offset: 0x0007E604
	public void LockTarget()
	{
		if (this.target == null)
		{
			return;
		}
		if (this.difficulty > 2 && this.enraged && Random.Range(0f, 1f) > 0.5f)
		{
			this.Teleport(false);
		}
		Rigidbody componentInParent = this.target.targetTransform.GetComponentInParent<Rigidbody>();
		if (componentInParent)
		{
			if (this.difficulty < 4)
			{
				float num = -1.5f;
				if (componentInParent.velocity.y < 0f)
				{
					num = componentInParent.velocity.y * 3f - 1.5f;
				}
				Vector3 vector = new Vector3(componentInParent.velocity.x * 2.5f, num, componentInParent.velocity.z * 2.5f);
				this.overrideTarget = this.target.position + vector;
				if (componentInParent.velocity.y < 0f)
				{
					Vector3 vector2 = new Vector3(this.target.position.x + vector.x, this.target.position.y, this.target.position.z + vector.z);
					RaycastHit raycastHit;
					if (Physics.Raycast(this.overrideTarget, Vector3.down, out raycastHit, Vector3.Distance(vector2, this.overrideTarget), this.environmentMask))
					{
						this.overrideTarget = raycastHit.point + Vector3.up;
					}
				}
			}
			else
			{
				this.overrideTarget = MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(0.5f, false, false);
			}
		}
		else
		{
			this.overrideTarget = this.target.position;
		}
		Object.Instantiate<GameObject>(this.warningFlashUnparriable, this.eid.weakPoint.transform).transform.localScale *= 8f;
		this.lr.SetPosition(0, base.transform.position);
		this.lr.SetPosition(1, this.overrideTarget);
		this.lr.enabled = true;
		this.overrideRotation = true;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0008061C File Offset: 0x0007E81C
	public void StartBeam()
	{
		if (this.target == null)
		{
			return;
		}
		if (!this.beaming)
		{
			this.lr.enabled = false;
			this.beaming = true;
			this.tempBeam = Object.Instantiate<GameObject>(this.beam, this.rightHand.transform.position, base.transform.rotation);
			this.tempBeam.transform.SetParent(this.launched ? this.model : this.rightHand, true);
			ContinuousBeam continuousBeam;
			if (this.tempBeam.TryGetComponent<ContinuousBeam>(out continuousBeam))
			{
				continuousBeam.damage *= this.eid.totalDamageModifier;
				continuousBeam.target = this.target;
				if (this.launched)
				{
					continuousBeam.canHitPlayer = false;
				}
			}
			Vector3 vector = ((this.difficulty >= 4 && this.eid.target.isPlayer) ? MonoSingleton<PlayerTracker>.Instance.PredictPlayerPosition(0.5f, false, false) : this.target.position);
			this.overrideTarget += (vector - this.overrideTarget) * 2f;
			Quaternion quaternion = Quaternion.LookRotation(this.overrideTarget - base.transform.position, Vector3.up);
			this.beamDistance = Quaternion.Angle(base.transform.rotation, quaternion);
		}
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0008077E File Offset: 0x0007E97E
	private void StopBeam()
	{
		if (this.tempBeam != null)
		{
			Object.Destroy(this.tempBeam);
		}
		this.chargeParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
		this.overrideRotation = false;
		this.anim.SetTrigger("StopBeam");
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x000807C0 File Offset: 0x0007E9C0
	public void ShootProjectiles()
	{
		if (this.target == null)
		{
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.transform.position = base.transform.position;
		ProjectileSpread projectileSpread = gameObject.AddComponent<ProjectileSpread>();
		projectileSpread.dontSpawn = true;
		projectileSpread.timeUntilDestroy = 10f;
		for (int i = 0; i < this.tentacles.Length; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.homingProjectile, base.transform.position, Quaternion.LookRotation(this.target.position - base.transform.position));
			if (!Physics.Raycast(base.transform.position, this.tentacles[i].position - base.transform.position, Vector3.Distance(this.tentacles[i].position, base.transform.position), this.environmentMask))
			{
				gameObject2.transform.position = this.tentacles[i].position;
			}
			gameObject2.transform.SetParent(gameObject.transform, true);
			Projectile component = gameObject2.GetComponent<Projectile>();
			component.target = this.target;
			component.speed = 10f;
			component.damage *= this.eid.totalDamageModifier;
		}
		this.chargeParticle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
		this.cooldown = (float)Random.Range(4, 5);
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0008092C File Offset: 0x0007EB2C
	public void HighDifficultyTeleport()
	{
		if (this.enraged && !this.dontTeleport)
		{
			this.Teleport(false);
			this.anim.speed = 0f;
			base.Invoke("ResetAnimSpeed", 0.25f / this.eid.totalSpeedModifier);
			if (Random.Range(0f, 1f) < 0.1f || (this.difficulty > 3 && Random.Range(0f, 1f) < 0.33f))
			{
				base.Invoke("Teleport", 0.2f / this.eid.totalSpeedModifier);
			}
		}
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public void MeleeTeleport()
	{
		if (this.enraged)
		{
			this.Teleport(true);
			this.anim.speed = 0f;
			base.CancelInvoke("ResetAnimSpeed");
			base.Invoke("ResetAnimSpeed", 0.25f / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00080A27 File Offset: 0x0007EC27
	public void ResetAnimSpeed()
	{
		this.anim.speed = this.defaultAnimSpeed;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x00080A3A File Offset: 0x0007EC3A
	public void StopAction()
	{
		this.beaming = false;
		this.inAction = false;
		this.dontTeleport = false;
		this.RandomizeDirection();
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00080A58 File Offset: 0x0007EC58
	public void Enrage()
	{
		if (this.enraged)
		{
			return;
		}
		this.enraged = true;
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = true;
		}
		Gradient gradient = new Gradient();
		GradientColorKey[] array2 = new GradientColorKey[2];
		array2[0].color = Color.red;
		array2[0].time = 0f;
		array2[1].color = Color.red;
		array2[1].time = 1f;
		GradientAlphaKey[] array3 = new GradientAlphaKey[2];
		array3[0].alpha = 1f;
		array3[0].time = 0f;
		array3[1].alpha = 0f;
		array3[1].time = 1f;
		gradient.SetKeys(array2, array3);
		for (int j = 0; j < this.tentacles.Length; j++)
		{
			TrailRenderer component = this.tentacles[j].GetComponent<TrailRenderer>();
			if (component)
			{
				component.colorGradient = gradient;
			}
		}
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, base.transform.position, base.transform.rotation);
		this.currentEnrageEffect.transform.SetParent(base.transform, true);
		this.originalGlow.SetActive(false);
		this.enrageGlow.SetActive(true);
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00080BE8 File Offset: 0x0007EDE8
	public void UnEnrage()
	{
		if (!this.enraged)
		{
			return;
		}
		this.enraged = false;
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = false;
		}
		for (int j = 0; j < this.tentacles.Length; j++)
		{
			TrailRenderer component = this.tentacles[j].GetComponent<TrailRenderer>();
			if (component)
			{
				component.colorGradient = this.originalTentacleGradient;
			}
		}
		Object.Destroy(this.currentEnrageEffect);
		this.originalGlow.SetActive(true);
		this.enrageGlow.SetActive(false);
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00080C98 File Offset: 0x0007EE98
	public bool isEnraged
	{
		get
		{
			return this.enraged;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00080CA0 File Offset: 0x0007EEA0
	public string alterKey
	{
		get
		{
			return "mindflayer";
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00080CA0 File Offset: 0x0007EEA0
	public string alterCategoryName
	{
		get
		{
			return "mindflayer";
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x060010B7 RID: 4279 RVA: 0x00080CA8 File Offset: 0x0007EEA8
	public AlterOption<bool>[] options
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
				}
			};
		}
	}

	// Token: 0x0400168D RID: 5773
	private Animator anim;

	// Token: 0x0400168E RID: 5774
	private float defaultAnimSpeed = 1f;

	// Token: 0x0400168F RID: 5775
	[HideInInspector]
	public bool active = true;

	// Token: 0x04001690 RID: 5776
	public Transform model;

	// Token: 0x04001691 RID: 5777
	public GameObject homingProjectile;

	// Token: 0x04001692 RID: 5778
	public GameObject decorativeProjectile;

	// Token: 0x04001693 RID: 5779
	public GameObject warningFlash;

	// Token: 0x04001694 RID: 5780
	public GameObject warningFlashUnparriable;

	// Token: 0x04001695 RID: 5781
	public GameObject decoy;

	// Token: 0x04001696 RID: 5782
	public Transform[] tentacles;

	// Token: 0x04001697 RID: 5783
	private SwingCheck2 sc;

	// Token: 0x04001698 RID: 5784
	public float cooldown;

	// Token: 0x04001699 RID: 5785
	private bool inAction;

	// Token: 0x0400169A RID: 5786
	private bool overrideRotation;

	// Token: 0x0400169B RID: 5787
	private Vector3 overrideTarget;

	// Token: 0x0400169C RID: 5788
	private bool dontTeleport;

	// Token: 0x0400169D RID: 5789
	private EnemyIdentifier eid;

	// Token: 0x0400169E RID: 5790
	private Machine mach;

	// Token: 0x0400169F RID: 5791
	private LayerMask environmentMask;

	// Token: 0x040016A0 RID: 5792
	private float decoyThreshold;

	// Token: 0x040016A1 RID: 5793
	private int teleportAttempts;

	// Token: 0x040016A2 RID: 5794
	private int teleportInterval = 6;

	// Token: 0x040016A3 RID: 5795
	public GameObject bigHurt;

	// Token: 0x040016A4 RID: 5796
	public GameObject windUp;

	// Token: 0x040016A5 RID: 5797
	public GameObject windUpSmall;

	// Token: 0x040016A6 RID: 5798
	public GameObject teleportSound;

	// Token: 0x040016A7 RID: 5799
	private bool goingLeft;

	// Token: 0x040016A8 RID: 5800
	private bool goForward;

	// Token: 0x040016A9 RID: 5801
	private Rigidbody rb;

	// Token: 0x040016AA RID: 5802
	private bool beaming;

	// Token: 0x040016AB RID: 5803
	private bool beamCooldown = true;

	// Token: 0x040016AC RID: 5804
	private bool beamNext;

	// Token: 0x040016AD RID: 5805
	public GameObject beam;

	// Token: 0x040016AE RID: 5806
	[HideInInspector]
	public GameObject tempBeam;

	// Token: 0x040016AF RID: 5807
	public Transform rightHand;

	// Token: 0x040016B0 RID: 5808
	private float beamDistance;

	// Token: 0x040016B1 RID: 5809
	private LineRenderer lr;

	// Token: 0x040016B2 RID: 5810
	private float outOfSightTime;

	// Token: 0x040016B3 RID: 5811
	public AssetReference deathExplosion;

	// Token: 0x040016B4 RID: 5812
	public ParticleSystem chargeParticle;

	// Token: 0x040016B5 RID: 5813
	private bool vibrate;

	// Token: 0x040016B6 RID: 5814
	private Vector3 origPos;

	// Token: 0x040016B7 RID: 5815
	private float timeSinceMelee;

	// Token: 0x040016B8 RID: 5816
	private float spawnAttackDelay = 1f;

	// Token: 0x040016B9 RID: 5817
	private int difficulty = -1;

	// Token: 0x040016BA RID: 5818
	private float cooldownMultiplier;

	// Token: 0x040016BB RID: 5819
	private bool enraged;

	// Token: 0x040016BC RID: 5820
	public GameObject enrageEffect;

	// Token: 0x040016BD RID: 5821
	private GameObject currentEnrageEffect;

	// Token: 0x040016BE RID: 5822
	private EnemySimplifier[] ensims;

	// Token: 0x040016BF RID: 5823
	public GameObject originalGlow;

	// Token: 0x040016C0 RID: 5824
	public GameObject enrageGlow;

	// Token: 0x040016C1 RID: 5825
	public Gradient originalTentacleGradient;

	// Token: 0x040016C2 RID: 5826
	public SkinnedMeshRenderer smr;

	// Token: 0x040016C3 RID: 5827
	public EnemySimplifier ensim;

	// Token: 0x040016C4 RID: 5828
	public Mesh maleMesh;

	// Token: 0x040016C5 RID: 5829
	public Material maleMaterial;

	// Token: 0x040016C6 RID: 5830
	public Material maleMaterialEnraged;

	// Token: 0x040016C7 RID: 5831
	[HideInInspector]
	public bool dying;

	// Token: 0x040016C8 RID: 5832
	private bool launched;

	// Token: 0x040016C9 RID: 5833
	private Collider[] ownColliders;
}
