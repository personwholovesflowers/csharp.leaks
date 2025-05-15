using System;
using Sandbox;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000242 RID: 578
public class Gutterman : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x06000C7F RID: 3199 RVA: 0x0005B28E File Offset: 0x0005948E
	private void Start()
	{
		this.GetValues();
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0005B298 File Offset: 0x00059498
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.anim = base.GetComponent<Animator>();
		this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		if (this.dead)
		{
			this.CheckIfInstaCorpse();
			return;
		}
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (!this.hasShield)
		{
			GameObject[] array = this.shield;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
		this.anim.SetBool("Shield", this.hasShield);
		this.torsoDefaultRotation = Quaternion.Inverse(base.transform.rotation) * this.torsoAimBone.rotation;
		this.lastParried = 5f;
		this.barrelRotation = this.windupBarrel.localRotation;
		if (this.windupAud.pitch != 0f)
		{
			this.windupAud.Play();
		}
		this.SetSpeed();
		this.SlowUpdate();
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0005B3EF File Offset: 0x000595EF
	private void OnEnable()
	{
		this.GetValues();
		this.CheckIfInstaCorpse();
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0005B3FD File Offset: 0x000595FD
	private void OnDisable()
	{
		this.inAction = false;
		base.CancelInvoke("DoneDying");
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0005B414 File Offset: 0x00059614
	private void CheckIfInstaCorpse()
	{
		if (this.dead)
		{
			if (this.anim)
			{
				this.anim.Play("Death", 0, 1f);
			}
			this.fallen = true;
			base.Invoke("DoneDying", 0.5f);
		}
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0005B463 File Offset: 0x00059663
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0005B46C File Offset: 0x0005966C
	private void SetSpeed()
	{
		this.GetValues();
		if (this.difficulty == 0)
		{
			this.anim.speed = 0.8f;
			this.defaultMovementSpeed = 8f;
			this.windupSpeed = 0.5f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.9f;
			this.defaultMovementSpeed = 9f;
			this.windupSpeed = 0.75f;
		}
		else
		{
			this.anim.speed = 1f;
			this.defaultMovementSpeed = 10f;
			this.windupSpeed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.defaultMovementSpeed *= this.eid.totalSpeedModifier;
		this.nma.speed = (this.slowMode ? (this.defaultMovementSpeed / 2f) : this.defaultMovementSpeed);
		this.windupSpeed *= this.eid.totalSpeedModifier;
		if (this.difficulty > 2)
		{
			this.trackingSpeedMultiplier = 1f;
		}
		else if (this.difficulty == 2)
		{
			this.trackingSpeedMultiplier = 0.8f;
		}
		else if (this.difficulty == 1)
		{
			this.trackingSpeedMultiplier = 0.5f;
		}
		else
		{
			this.trackingSpeedMultiplier = 0.35f;
		}
		this.defaultTrackingSpeed = 1f;
		if (this.trackingSpeed < this.defaultTrackingSpeed)
		{
			this.trackingSpeed = this.defaultTrackingSpeed;
		}
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0005B5EC File Offset: 0x000597EC
	private void Update()
	{
		if (this.dead)
		{
			return;
		}
		if (this.lineOfSightTimer >= 0.9f || (this.slowMode && this.lineOfSightTimer > 0f))
		{
			this.windup = Mathf.MoveTowards(this.windup, 1f, Time.deltaTime * this.windupSpeed);
		}
		else
		{
			this.windup = Mathf.MoveTowards(this.windup, 0f, Time.deltaTime * this.windupSpeed);
		}
		this.windupAud.pitch = this.windup * 3f;
		if (this.windupAud.pitch == 0f)
		{
			this.windupAud.Stop();
		}
		else if (!this.windupAud.isPlaying)
		{
			this.windupAud.Play();
		}
		if (this.inAction)
		{
			this.firing = false;
			if (this.nma.enabled)
			{
				this.nma.updateRotation = false;
			}
			if (this.difficulty <= 1)
			{
				this.windup = 0f;
			}
			if (this.eid.target != null)
			{
				this.trackingPosition = base.transform.position + base.transform.forward * Mathf.Max(5f, Vector3.Distance(base.transform.position, new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z))) + Vector3.up * (this.eid.target.position.y - base.transform.position.y);
				if (this.trackInAction || this.moveForward)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z) - base.transform.position), (float)(this.trackInAction ? 360 : 90) * Time.deltaTime);
				}
			}
		}
		else
		{
			RaycastHit raycastHit;
			bool flag = this.eid.target != null && !Physics.Raycast(base.transform.position + Vector3.up * 3.5f, this.eid.target.headPosition - (base.transform.position + Vector3.up), out raycastHit, Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment));
			this.lineOfSightTimer = Mathf.MoveTowards(this.lineOfSightTimer, (float)(flag ? 1 : 0), Time.deltaTime * 2f);
			if (this.windup >= 0.5f)
			{
				if (!this.slowMode)
				{
					this.trackingPosition = base.transform.position + base.transform.forward * Mathf.Max(30f, Vector3.Distance(base.transform.position, this.eid.target.headPosition));
				}
				this.slowMode = true;
			}
			else if (this.slowMode && this.windup <= 0f)
			{
				this.slowMode = false;
				Object.Instantiate<AudioSource>(this.releaseSound, base.transform.position, Quaternion.identity);
			}
			if (this.firing && !this.mach.gc.onGround)
			{
				this.firing = false;
			}
			if (this.slowMode && this.firing)
			{
				this.trackingSpeed += Time.deltaTime * (float)(this.hasShield ? 2 : 5) * this.trackingSpeedMultiplier * this.eid.totalSpeedModifier;
			}
			else if (!this.slowMode)
			{
				this.trackingSpeed = this.defaultTrackingSpeed;
			}
			if (this.slowMode)
			{
				if (this.nma.enabled)
				{
					this.nma.updateRotation = false;
				}
				if (this.eid.target != null)
				{
					if (this.lineOfSightTimer > 0f)
					{
						this.lastKnownPosition = this.eid.target.headPosition;
					}
					this.trackingPosition = Vector3.MoveTowards(this.trackingPosition, this.lastKnownPosition, (Vector3.Distance(this.trackingPosition, this.eid.target.headPosition) + this.trackingSpeed) * Time.deltaTime);
				}
				base.transform.rotation = Quaternion.LookRotation(new Vector3(this.trackingPosition.x, base.transform.position.y, this.trackingPosition.z) - base.transform.position);
			}
			else if (this.nma.enabled)
			{
				this.nma.updateRotation = true;
			}
			this.nma.speed = (this.slowMode ? (this.defaultMovementSpeed / 2f) : this.defaultMovementSpeed);
			if (this.eid.target != null && this.lineOfSightTimer >= 0.5f && this.lastParried > 5f && this.mach.gc.onGround && Vector3.Distance(base.transform.position, this.eid.target.position) < 12f)
			{
				this.ShieldBash();
			}
		}
		this.slowModeLerp = Mathf.MoveTowards(this.slowModeLerp, (float)(this.slowMode ? 1 : 0), Time.deltaTime * 2.5f);
		this.anim.SetFloat("WalkSpeed", this.slowMode ? 0.5f : 1f);
		this.anim.SetBool("Walking", this.nma.velocity.magnitude > 2.5f);
		this.anim.SetLayerWeight(1, (float)(this.firing ? 1 : 0));
		if (!this.eternalRage && this.rageLeft > 0f)
		{
			this.rageLeft = Mathf.MoveTowards(this.rageLeft, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.currentEnrageEffect != null && this.rageLeft < 3f)
			{
				this.currentEnrageEffect.pitch = this.rageLeft / 3f;
			}
			if (this.rageLeft <= 0f)
			{
				this.enraged = false;
				foreach (EnemySimplifier enemySimplifier in this.ensims)
				{
					if (enemySimplifier)
					{
						enemySimplifier.enraged = false;
					}
				}
				if (this.currentEnrageEffect != null)
				{
					Object.Destroy(this.currentEnrageEffect.gameObject);
				}
			}
		}
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0005BD2C File Offset: 0x00059F2C
	private void LateUpdate()
	{
		if (this.dead)
		{
			return;
		}
		if (this.inAction)
		{
			Quaternion quaternion = Quaternion.RotateTowards(this.torsoAimBone.rotation, Quaternion.LookRotation(this.torsoAimBone.position - this.trackingPosition, Vector3.up), 60f);
			Quaternion quaternion2 = Quaternion.Inverse(base.transform.rotation * this.torsoDefaultRotation) * this.torsoAimBone.rotation;
			this.torsoAimBone.rotation = quaternion * quaternion2;
			this.sc.knockBackDirection = this.trackingPosition - this.torsoAimBone.position;
		}
		else if (this.slowModeLerp > 0f)
		{
			this.torsoAimBone.rotation = Quaternion.Lerp(this.torsoAimBone.rotation, Quaternion.LookRotation(this.torsoAimBone.position - this.trackingPosition), this.slowModeLerp);
			Quaternion rotation = this.gunAimBone.rotation;
			this.gunAimBone.rotation = Quaternion.LookRotation(this.gunAimBone.position - this.trackingPosition);
			this.gunAimBone.Rotate(Vector3.left, 90f, Space.Self);
			this.gunAimBone.Rotate(Vector3.up, 180f, Space.Self);
			this.gunAimBone.rotation = Quaternion.Lerp(rotation, this.gunAimBone.rotation, this.slowModeLerp);
		}
		this.windupBarrel.localRotation = this.barrelRotation;
		if (this.windup > 0f)
		{
			this.windupBarrel.Rotate(Vector3.up * -3600f * this.windup * Time.deltaTime);
			this.barrelRotation = this.windupBarrel.localRotation;
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0005BF14 File Offset: 0x0005A114
	private void FixedUpdate()
	{
		if (this.dead)
		{
			return;
		}
		if (this.inAction && !this.stationary)
		{
			this.rb.isKinematic = !this.moveForward;
			if (this.moveForward)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward, Vector3.down, out raycastHit, (this.eid.target == null) ? 22f : Mathf.Max(22f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					this.rb.velocity = base.transform.forward * (float)(this.hasShield ? 25 : 45) * this.anim.speed * this.eid.totalSpeedModifier;
				}
				else
				{
					this.rb.velocity = Vector3.zero;
				}
			}
		}
		if (this.firing)
		{
			if (this.bulletCooldown == 0f)
			{
				Vector3 vector = this.shootPoint.position + this.shootPoint.right * Random.Range(-0.2f, 0.2f) + this.shootPoint.up * Random.Range(-0.2f, 0.2f);
				if (Physics.Raycast(this.shootPoint.position - this.shootPoint.forward * 4f, this.shootPoint.forward, 4f, LayerMaskDefaults.Get(LMD.EnvironmentAndPlayer)))
				{
					vector = this.shootPoint.position - this.shootPoint.forward * 4f;
				}
				Object.Instantiate<GameObject>(this.beam, vector, this.shootPoint.rotation).transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
				this.bulletCooldown = 0.05f / this.windup;
				return;
			}
			this.bulletCooldown = Mathf.MoveTowards(this.bulletCooldown, 0f, Time.fixedDeltaTime);
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x0005C1A4 File Offset: 0x0005A3A4
	private void SlowUpdate()
	{
		if (this.dead)
		{
			return;
		}
		base.Invoke("SlowUpdate", 0.1f);
		if (!this.inAction && this.eid.target != null && !this.stationary && this.nma.enabled && this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(this.eid.target.position);
		}
		if (!this.inAction)
		{
			if (this.slowMode && this.windup >= 0.5f && this.eid.target != null && (this.firing || this.windup >= 1f) && this.mach.gc.onGround)
			{
				this.firing = true;
				foreach (RaycastHit raycastHit in Physics.RaycastAll(base.transform.position + Vector3.up + base.transform.forward * 3f, this.eid.target.headPosition - (base.transform.position + Vector3.up), Vector3.Distance(this.eid.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Enemies)))
				{
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					if (!(raycastHit.transform == this.eid.target.targetTransform) && !(raycastHit.transform == this.shield[0].transform) && (!raycastHit.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid || (!(enemyIdentifierIdentifier.eid == this.eid) && !enemyIdentifierIdentifier.eid.dead && !(enemyIdentifierIdentifier.eid == this.eid.target.enemyIdentifier))))
					{
						this.firing = false;
						return;
					}
				}
				return;
			}
			this.firing = false;
		}
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x0005C3DC File Offset: 0x0005A5DC
	private void Death()
	{
		Object.Instantiate<AudioSource>(this.deathSound, base.transform);
		this.ShieldBashStop();
		this.dead = true;
		this.windupAud.Stop();
		this.anim.SetBool("Dead", true);
		this.anim.SetLayerWeight(1, 0f);
		this.anim.Play("Death", 0, 0f);
		Collider collider;
		if (base.TryGetComponent<Collider>(out collider))
		{
			collider.enabled = false;
		}
		if (this.mach.gc.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.constraints = (RigidbodyConstraints)122;
		}
		if (this.currentEnrageEffect != null)
		{
			Object.Destroy(this.currentEnrageEffect.gameObject);
		}
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0005C4A8 File Offset: 0x0005A6A8
	public void ShieldBreak(bool player = true, bool flash = true)
	{
		this.anim.Play("ShieldBreak", 0, 0f);
		this.anim.SetBool("Shield", false);
		if (player)
		{
			if (flash)
			{
				MonoSingleton<NewMovement>.Instance.Parry(null, "GUARD BREAK");
			}
			else
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(100, "<color=green>GUARD BREAK</color>", null, null, -1, "", "");
			}
			if (this.difficulty >= 4)
			{
				this.Enrage();
			}
		}
		GameObject[] array = this.shield;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.hasShield = false;
		Object.Instantiate<AudioSource>(this.bonkSound, base.transform.position, Quaternion.identity);
		Object.Instantiate<GameObject>(this.shieldBreakEffect, this.shield[0].transform.position, Quaternion.identity);
		if (this.inAction)
		{
			this.ShieldBashStop();
			this.StopAction();
		}
		this.sc = this.shieldlessSwingcheck;
		this.inAction = true;
		this.attacking = false;
		this.trackInAction = false;
		this.nma.enabled = false;
		this.moveForward = false;
		this.firing = false;
		this.slowMode = false;
		this.windup = 0f;
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0005C5E8 File Offset: 0x0005A7E8
	private void ShieldBash()
	{
		if (this.difficulty <= 2 && this.hasShield)
		{
			this.lastParried = 3f;
		}
		this.anim.Play(this.hasShield ? "ShieldBash" : "Smack", 0, 0f);
		Object.Instantiate<GameObject>((this.hasShield || this.enraged) ? MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash : MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, this.shield[0].transform.position + base.transform.forward, base.transform.rotation).transform.localScale *= 15f;
		this.inAction = true;
		this.nma.enabled = false;
		this.firing = false;
		this.attacking = true;
		this.trackInAction = true;
		if (!this.hasShield && !this.enraged)
		{
			this.mach.parryable = true;
		}
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x0005C6F4 File Offset: 0x0005A8F4
	private void ShieldBashActive()
	{
		if (!this.attacking)
		{
			return;
		}
		this.sc.DamageStart();
		this.sc.knockBackDirectionOverride = true;
		this.sc.knockBackDirection = base.transform.forward;
		this.moveForward = true;
		this.trackInAction = false;
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0005C745 File Offset: 0x0005A945
	private void ShieldBashStop()
	{
		this.sc.DamageStop();
		this.moveForward = false;
		this.mach.parryable = false;
		this.attacking = false;
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0005C76C File Offset: 0x0005A96C
	private void StopAction()
	{
		if (this.dead)
		{
			return;
		}
		this.inAction = false;
		if (this.mach.gc.onGround)
		{
			this.rb.isKinematic = true;
			this.nma.enabled = true;
			return;
		}
		this.rb.isKinematic = false;
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x0005C7C0 File Offset: 0x0005A9C0
	public void GotParried()
	{
		this.anim.Play("ShieldBreak", 0, 0f);
		this.ShieldBashStop();
		this.StopAction();
		this.inAction = true;
		this.trackInAction = false;
		this.attacking = false;
		this.nma.enabled = false;
		this.moveForward = false;
		if (this.difficulty >= 4)
		{
			this.Enrage();
		}
		else
		{
			this.lastParried = 0f;
		}
		this.windup = 0f;
		this.trackingSpeed = this.defaultTrackingSpeed;
		Object.Instantiate<AudioSource>(this.bonkSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x0005C86B File Offset: 0x0005AA6B
	private void FallStart()
	{
		this.fallingKillTrigger.SetActive(true);
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x0005C87C File Offset: 0x0005AA7C
	private void FallOver()
	{
		if (!this.fallEffect)
		{
			return;
		}
		if (MonoSingleton<EndlessGrid>.Instance)
		{
			this.Explode();
			return;
		}
		if (this.mach.gc.onGround)
		{
			for (int i = 0; i < this.mach.gc.cols.Count; i++)
			{
				if (this.mach.gc.cols[i].gameObject.CompareTag("Moving"))
				{
					this.Explode();
					return;
				}
			}
		}
		this.fallEffect.transform.position = new Vector3(this.mach.chest.transform.position.x, base.transform.position.y, this.mach.chest.transform.position.z);
		this.fallEffect.SetActive(true);
		this.fallingKillTrigger.SetActive(false);
		this.playerUnstucker.SetActive(true);
		this.fallen = true;
		base.Invoke("DoneDying", 1f);
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x0005C9A0 File Offset: 0x0005ABA0
	public void Explode()
	{
		if (this.exploded)
		{
			return;
		}
		this.exploded = true;
		Object.Instantiate<GameObject>(this.corpseExplosion, this.torsoAimBone.position, Quaternion.identity);
		if (this.mach)
		{
			foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
			{
				if (!(enemyIdentifierIdentifier == null))
				{
					this.mach.GetHurt(enemyIdentifierIdentifier.gameObject, (base.transform.position - enemyIdentifierIdentifier.transform.position).normalized * 1000f, 999f, 1f, null, false);
				}
			}
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x0005CA5D File Offset: 0x0005AC5D
	private void DoneDying()
	{
		this.playerUnstucker.SetActive(false);
		this.anim.enabled = false;
		base.enabled = false;
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x0005CA7E File Offset: 0x0005AC7E
	public void SetStationary(bool status)
	{
		this.stationary = status;
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x0005CA88 File Offset: 0x0005AC88
	public void Enrage()
	{
		if (this.enraged)
		{
			return;
		}
		this.enraged = true;
		this.rageLeft = 10f;
		foreach (EnemySimplifier enemySimplifier in this.ensims)
		{
			if (enemySimplifier)
			{
				enemySimplifier.enraged = true;
			}
		}
		if (this.currentEnrageEffect == null)
		{
			this.currentEnrageEffect = Object.Instantiate<AudioSource>(this.enrageEffect, this.mach.chest.transform);
			this.currentEnrageEffect.pitch = 1f;
			this.currentEnrageEffect.transform.localScale *= 0.01f;
		}
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x0005CB38 File Offset: 0x0005AD38
	public void UnEnrage()
	{
		this.enraged = false;
		this.rageLeft = 0f;
		foreach (EnemySimplifier enemySimplifier in this.ensims)
		{
			if (enemySimplifier)
			{
				enemySimplifier.enraged = false;
			}
		}
		if (this.currentEnrageEffect != null)
		{
			Object.Destroy(this.currentEnrageEffect.gameObject);
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0005CB9D File Offset: 0x0005AD9D
	public bool isEnraged
	{
		get
		{
			return this.enraged;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0005CBA5 File Offset: 0x0005ADA5
	public string alterKey
	{
		get
		{
			return "Gutterman";
		}
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0005CBA5 File Offset: 0x0005ADA5
	public string alterCategoryName
	{
		get
		{
			return "Gutterman";
		}
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0005CBAC File Offset: 0x0005ADAC
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

	// Token: 0x04001080 RID: 4224
	private bool gotValues;

	// Token: 0x04001081 RID: 4225
	private EnemyIdentifier eid;

	// Token: 0x04001082 RID: 4226
	private NavMeshAgent nma;

	// Token: 0x04001083 RID: 4227
	private Machine mach;

	// Token: 0x04001084 RID: 4228
	private Rigidbody rb;

	// Token: 0x04001085 RID: 4229
	private Animator anim;

	// Token: 0x04001086 RID: 4230
	private int difficulty;

	// Token: 0x04001087 RID: 4231
	private float defaultMovementSpeed;

	// Token: 0x04001088 RID: 4232
	[HideInInspector]
	public bool dead;

	// Token: 0x04001089 RID: 4233
	[HideInInspector]
	public bool fallen;

	// Token: 0x0400108A RID: 4234
	[HideInInspector]
	public bool exploded;

	// Token: 0x0400108B RID: 4235
	public bool hasShield = true;

	// Token: 0x0400108C RID: 4236
	public bool stationary;

	// Token: 0x0400108D RID: 4237
	[SerializeField]
	private GameObject[] shield;

	// Token: 0x0400108E RID: 4238
	public Transform torsoAimBone;

	// Token: 0x0400108F RID: 4239
	public Transform gunAimBone;

	// Token: 0x04001090 RID: 4240
	private Quaternion torsoDefaultRotation;

	// Token: 0x04001091 RID: 4241
	[SerializeField]
	private SwingCheck2 sc;

	// Token: 0x04001092 RID: 4242
	[SerializeField]
	private SwingCheck2 shieldlessSwingcheck;

	// Token: 0x04001093 RID: 4243
	private bool inAction;

	// Token: 0x04001094 RID: 4244
	private bool attacking;

	// Token: 0x04001095 RID: 4245
	private bool moveForward;

	// Token: 0x04001096 RID: 4246
	private bool trackInAction;

	// Token: 0x04001097 RID: 4247
	public Transform shootPoint;

	// Token: 0x04001098 RID: 4248
	public GameObject beam;

	// Token: 0x04001099 RID: 4249
	private float windup;

	// Token: 0x0400109A RID: 4250
	private float windupSpeed;

	// Token: 0x0400109B RID: 4251
	[SerializeField]
	private AudioSource windupAud;

	// Token: 0x0400109C RID: 4252
	[SerializeField]
	private Transform windupBarrel;

	// Token: 0x0400109D RID: 4253
	private Quaternion barrelRotation;

	// Token: 0x0400109E RID: 4254
	private bool slowMode;

	// Token: 0x0400109F RID: 4255
	private float slowModeLerp;

	// Token: 0x040010A0 RID: 4256
	private bool firing;

	// Token: 0x040010A1 RID: 4257
	private float bulletCooldown;

	// Token: 0x040010A2 RID: 4258
	private float lineOfSightTimer;

	// Token: 0x040010A3 RID: 4259
	private float trackingSpeed;

	// Token: 0x040010A4 RID: 4260
	private float trackingSpeedMultiplier;

	// Token: 0x040010A5 RID: 4261
	private float defaultTrackingSpeed = 1f;

	// Token: 0x040010A6 RID: 4262
	private Vector3 trackingPosition;

	// Token: 0x040010A7 RID: 4263
	private Vector3 lastKnownPosition;

	// Token: 0x040010A8 RID: 4264
	private TimeSince lastParried;

	// Token: 0x040010A9 RID: 4265
	[SerializeField]
	private GameObject playerUnstucker;

	// Token: 0x040010AA RID: 4266
	[SerializeField]
	private GameObject fallingKillTrigger;

	// Token: 0x040010AB RID: 4267
	[SerializeField]
	private GameObject fallEffect;

	// Token: 0x040010AC RID: 4268
	[SerializeField]
	private GameObject corpseExplosion;

	// Token: 0x040010AD RID: 4269
	[SerializeField]
	private GameObject shieldBreakEffect;

	// Token: 0x040010AE RID: 4270
	[SerializeField]
	private AudioSource bonkSound;

	// Token: 0x040010AF RID: 4271
	[SerializeField]
	private AudioSource releaseSound;

	// Token: 0x040010B0 RID: 4272
	[SerializeField]
	private AudioSource deathSound;

	// Token: 0x040010B1 RID: 4273
	private bool enraged;

	// Token: 0x040010B2 RID: 4274
	public bool eternalRage;

	// Token: 0x040010B3 RID: 4275
	[SerializeField]
	private AudioSource enrageEffect;

	// Token: 0x040010B4 RID: 4276
	private AudioSource currentEnrageEffect;

	// Token: 0x040010B5 RID: 4277
	private float rageLeft;

	// Token: 0x040010B6 RID: 4278
	private EnemySimplifier[] ensims;
}
