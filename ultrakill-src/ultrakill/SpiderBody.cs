using System;
using System.Collections.Generic;
using Sandbox;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

// Token: 0x0200042D RID: 1069
public class SpiderBody : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x170001AB RID: 427
	// (get) Token: 0x0600180C RID: 6156 RVA: 0x000C3B82 File Offset: 0x000C1D82
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000C3B8F File Offset: 0x000C1D8F
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.eid = base.GetComponent<EnemyIdentifier>();
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000C3BB8 File Offset: 0x000C1DB8
	private void Start()
	{
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.maxHealth = this.health;
		if (this.difficulty >= 3)
		{
			this.coolDownMultiplier = 1.25f;
		}
		else if (this.difficulty == 1)
		{
			this.coolDownMultiplier = 0.75f;
		}
		else if (this.difficulty == 0)
		{
			this.coolDownMultiplier = 0.5f;
		}
		if (this.difficulty >= 4)
		{
			this.maxBurst = 10;
		}
		else if (this.difficulty >= 2)
		{
			this.maxBurst = 5;
		}
		else
		{
			this.maxBurst = 2;
		}
		if (!this.mainMesh)
		{
			this.mainMesh = base.GetComponentInChildren<SkinnedMeshRenderer>();
		}
		this.origMaterial = this.mainMesh.material;
		this.gz = GoreZone.ResolveGoreZone(base.transform.parent ? base.transform.parent : base.transform);
		if (this.nma)
		{
			this.nma.updateRotation = false;
			if (this.stationary)
			{
				this.nma.speed = 0f;
			}
		}
		if (this.currentCE)
		{
			Object.Destroy(this.currentCE);
		}
		this.defaultHeight = this.targetHeight;
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x000C3D24 File Offset: 0x000C1F24
	private void OnDisable()
	{
		if (!this.eid.dead)
		{
			this.requestedMusic = false;
			if (this.muman == null)
			{
				this.muman = MonoSingleton<MusicManager>.Instance;
			}
			if (this.muman)
			{
				this.muman.PlayCleanMusic();
			}
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x000C3D78 File Offset: 0x000C1F78
	private void Update()
	{
		if (this.target == null)
		{
			return;
		}
		this.followPlayerRot = Quaternion.LookRotation((this.target.headPosition - base.transform.position).normalized);
		if (!this.eid.dead)
		{
			if (this.beamCharge < 1f)
			{
				this.headModel.transform.rotation = Quaternion.RotateTowards(this.headModel.transform.rotation, this.followPlayerRot, (Quaternion.Angle(this.headModel.transform.rotation, this.followPlayerRot) + 10f) * Time.deltaTime * 15f * this.eid.totalSpeedModifier);
			}
			else if (this.rotating && this.beamCharge == 1f)
			{
				this.headModel.transform.rotation = Quaternion.RotateTowards(this.headModel.transform.rotation, this.predictedRot, Quaternion.Angle(this.headModel.transform.rotation, this.predictedRot) * Time.deltaTime * 20f * this.eid.totalSpeedModifier);
			}
			else if (!this.rotating && this.beamCharge == 1f)
			{
				this.predictedRot = Quaternion.LookRotation(this.target.position - base.transform.position);
				this.headModel.transform.rotation = Quaternion.RotateTowards(this.headModel.transform.rotation, this.predictedRot, (Quaternion.Angle(this.headModel.transform.rotation, this.predictedRot) + 10f) * Time.deltaTime * 10f * this.eid.totalSpeedModifier);
			}
			if (this.difficulty > 2 && this.currentEnrageEffect == null && this.health < this.maxHealth / 2f)
			{
				this.Enrage();
			}
			if (!this.requestedMusic)
			{
				this.requestedMusic = true;
				this.muman = MonoSingleton<MusicManager>.Instance;
				this.muman.PlayBattleMusic();
			}
			if (!this.charging && this.beamCharge == 0f)
			{
				if (this.nma != null && !this.nma.enabled && !this.stationary)
				{
					this.nma.enabled = true;
					if (this.nma.isOnNavMesh)
					{
						this.nma.isStopped = false;
					}
					this.nma.speed = 3.5f * this.eid.totalSpeedModifier;
				}
				if (this.nma != null && this.nma.isOnNavMesh && !this.stationary)
				{
					if (this.eid.buffTargeter)
					{
						this.nma.SetDestination(this.eid.buffTargeter.transform.position);
						if (Vector3.Distance(base.transform.position, this.eid.buffTargeter.transform.position) < 15f)
						{
							this.targetHeight = 0.35f;
						}
						else
						{
							this.targetHeight = this.defaultHeight;
						}
					}
					else
					{
						this.nma.SetDestination(this.target.position);
						this.targetHeight = this.defaultHeight;
					}
					this.nma.baseOffset = Mathf.MoveTowards(this.nma.baseOffset, this.targetHeight, Time.deltaTime * this.defaultHeight / 2f * this.eid.totalSpeedModifier);
				}
				if (this.currentBurst > this.maxBurst && this.burstCharge == 0f)
				{
					this.currentBurst = 0;
					if (this.difficulty > 0)
					{
						this.burstCharge = 5f;
					}
					else
					{
						this.burstCharge = 10f;
					}
				}
				if (this.burstCharge > 0f)
				{
					this.burstCharge = Mathf.MoveTowards(this.burstCharge, 0f, Time.deltaTime * this.coolDownMultiplier * 5f * this.eid.totalSpeedModifier);
				}
				if (this.burstCharge < 0f)
				{
					this.burstCharge = 0f;
				}
				RaycastHit raycastHit;
				if (this.readyToShoot && this.burstCharge == 0f && (Quaternion.Angle(this.headModel.rotation, this.followPlayerRot) < 1f || Vector3.Distance(base.transform.position, this.target.position) < 10f) && !Physics.Raycast(base.transform.position, this.target.position - base.transform.position, out raycastHit, Vector3.Distance(base.transform.position, this.target.position), LayerMaskDefaults.Get(LMD.Environment)))
				{
					if (this.currentBurst != 0)
					{
						this.ShootProj();
						return;
					}
					if ((Random.Range(0f, this.health * 0.4f) >= this.beamProbability && this.beamProbability <= 5f) || (Vector3.Distance(base.transform.position, this.target.position) > 50f && !MonoSingleton<NewMovement>.Instance.ridingRocket))
					{
						this.ShootProj();
						this.beamProbability += 1f;
						return;
					}
					if (!this.eid.buffTargeter || Vector3.Distance(base.transform.position, this.eid.buffTargeter.transform.position) > 15f)
					{
						this.ChargeBeam();
						if (this.difficulty > 2 && this.health < this.maxHealth / 2f)
						{
							this.beamsAmount = 2;
						}
						if (this.health > 10f)
						{
							this.beamProbability = 0f;
							return;
						}
						this.beamProbability = 1f;
						return;
					}
				}
			}
			else if (this.charging)
			{
				if (this.beamCharge + 0.5f * this.coolDownMultiplier * Time.deltaTime * this.eid.totalSpeedModifier < 1f)
				{
					this.nma.speed = 0f;
					if (this.nma.isOnNavMesh)
					{
						this.nma.SetDestination(base.transform.position);
						this.nma.isStopped = true;
					}
					float num = 1f;
					if (this.difficulty >= 4)
					{
						num = 1.5f;
					}
					this.beamCharge += 0.5f * this.coolDownMultiplier * num * Time.deltaTime * this.eid.totalSpeedModifier;
					this.currentCE.transform.localScale = Vector3.one * this.beamCharge * 2.5f;
					this.ceAud.pitch = this.beamCharge * 2f;
					this.ceLight.intensity = this.beamCharge * 30f;
					return;
				}
				this.beamCharge = 1f;
				this.charging = false;
				this.BeamChargeEnd();
			}
		}
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x000C44D0 File Offset: 0x000C26D0
	private void FixedUpdate()
	{
		if (this.parryFramesLeft > 0)
		{
			this.parryFramesLeft--;
		}
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x000C44EC File Offset: 0x000C26EC
	public void GetHurt(GameObject target, Vector3 force, Vector3 hitPoint, float multiplier, GameObject sourceWeapon = null)
	{
		bool flag = false;
		float num = this.health;
		if (hitPoint == Vector3.zero)
		{
			hitPoint = target.transform.position;
		}
		bool goreOn = MonoSingleton<BloodsplatterManager>.Instance.goreOn;
		if (this.eid == null)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (this.eid.hitter != "fire")
		{
			if (!this.eid.sandified && !this.eid.blessed)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Small, this.eid, false), hitPoint, Quaternion.identity);
				if (gameObject)
				{
					Bloodsplatter component = gameObject.GetComponent<Bloodsplatter>();
					gameObject.transform.SetParent(this.gz.goreZone, true);
					if (this.eid.hitter == "drill")
					{
						gameObject.transform.localScale *= 2f;
					}
					if (this.health > 0f)
					{
						component.GetReady();
					}
					if (this.eid.hitter == "nail")
					{
						component.hpAmount = 3;
						component.GetComponent<AudioSource>().volume *= 0.8f;
					}
					else if (multiplier >= 1f)
					{
						component.hpAmount = 30;
					}
					if (goreOn)
					{
						gameObject.GetComponent<ParticleSystem>().Play();
					}
				}
				if (this.eid.hitter != "shotgun" && this.eid.hitter != "drill" && base.gameObject.activeInHierarchy)
				{
					if (this.dripBlood != null)
					{
						this.currentDrip = Object.Instantiate<GameObject>(this.dripBlood, hitPoint, Quaternion.identity);
					}
					if (this.currentDrip)
					{
						this.currentDrip.transform.parent = base.transform;
						this.currentDrip.transform.LookAt(base.transform);
						this.currentDrip.transform.Rotate(180f, 180f, 180f);
						if (goreOn)
						{
							this.currentDrip.GetComponent<ParticleSystem>().Play();
						}
					}
				}
			}
			else
			{
				Object.Instantiate<GameObject>(MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Small, this.eid, false), hitPoint, Quaternion.identity);
			}
		}
		if (!this.eid.dead)
		{
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= 1f * multiplier;
			}
			if (this.scalc == null)
			{
				this.scalc = MonoSingleton<StyleCalculator>.Instance;
			}
			if (this.health <= 0f)
			{
				flag = true;
			}
			if (((this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone") && this.parryable) || this.eid.hitter == "punch")
			{
				if (this.parryable)
				{
					this.parryable = false;
					MonoSingleton<FistControl>.Instance.currentPunch.Parry(false, this.eid, "");
					this.currentExplosion = Object.Instantiate<GameObject>(this.beamExplosion.ToAsset(), base.transform.position, Quaternion.identity);
					if (!InvincibleEnemies.Enabled && !this.eid.blessed)
					{
						this.health -= (float)((this.parryFramesLeft > 0) ? 4 : 5) / this.eid.totalHealthModifier;
					}
					foreach (Explosion explosion in this.currentExplosion.GetComponentsInChildren<Explosion>())
					{
						explosion.speed *= this.eid.totalDamageModifier;
						explosion.maxSize *= 1.75f * this.eid.totalDamageModifier;
						explosion.damage = Mathf.RoundToInt(50f * this.eid.totalDamageModifier);
						explosion.canHit = AffectedSubjects.EnemiesOnly;
						explosion.friendlyFire = true;
					}
					if (this.currentEnrageEffect == null)
					{
						base.CancelInvoke("BeamFire");
						base.Invoke("StopWaiting", 1f);
						Object.Destroy(this.currentCE);
					}
					this.parryFramesLeft = 0;
				}
				else
				{
					this.parryFramesLeft = MonoSingleton<FistControl>.Instance.currentPunch.activeFrames;
				}
			}
			if (multiplier != 0f)
			{
				this.scalc.HitCalculator(this.eid.hitter, "spider", "", flag, this.eid, sourceWeapon);
			}
			if (num >= this.maxHealth / 2f && this.health < this.maxHealth / 2f)
			{
				if (this.ensims == null || this.ensims.Length == 0)
				{
					this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
				}
				Object.Instantiate<GameObject>(this.woundedParticle, base.transform.position, Quaternion.identity);
				if (!this.eid.puppet)
				{
					foreach (EnemySimplifier enemySimplifier in this.ensims)
					{
						if (!enemySimplifier.ignoreCustomColor)
						{
							enemySimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.woundedMaterial);
							enemySimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.enraged, this.woundedEnrageMaterial);
						}
					}
				}
			}
			if (this.hurtSound && num > 0f)
			{
				this.hurtSound.PlayClipAtPoint(MonoSingleton<AudioMixerController>.Instance.goreGroup, base.transform.position, 12, 1f, 0.75f, Random.Range(0.85f, 1.35f), AudioRolloffMode.Linear, 1f, 100f);
			}
			if (this.health <= 0f && !this.eid.dead)
			{
				this.Die();
				return;
			}
		}
		else if (this.eid.hitter == "ground slam")
		{
			this.BreakCorpse();
		}
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x000C4AF0 File Offset: 0x000C2CF0
	public void Die()
	{
		this.rb = base.GetComponentInChildren<Rigidbody>();
		DoubleRender[] componentsInChildren = base.GetComponentsInChildren<DoubleRender>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].RemoveEffect();
		}
		this.falling = true;
		this.parryable = false;
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		if (this.health > 0f)
		{
			this.health = 0f;
		}
		base.gameObject.layer = 11;
		this.ResolveStuckness();
		for (int j = 1; j < base.transform.parent.childCount - 1; j++)
		{
			Object.Destroy(base.transform.parent.GetChild(j).gameObject);
		}
		if (this.currentCE != null)
		{
			Object.Destroy(this.currentCE);
		}
		Object.Destroy(this.nma);
		if (!this.eid.dontCountAsKills)
		{
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
		}
		if (this.muman == null)
		{
			this.muman = MonoSingleton<MusicManager>.Instance;
		}
		this.muman.PlayCleanMusic();
		EnemySimplifier[] array;
		if (this.currentEnrageEffect != null)
		{
			this.mainMesh.material = this.origMaterial;
			MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].material = this.origMaterial;
			}
			array = this.ensims;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enraged = false;
			}
			Object.Destroy(this.currentEnrageEffect);
		}
		if (this.ensims == null)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Begone();
		}
		if (this.eid.hitter == "ground slam" || this.eid.hitter == "breaker")
		{
			this.BreakCorpse();
		}
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x000C4D50 File Offset: 0x000C2F50
	private void ShootProj()
	{
		if (this.target == null)
		{
			return;
		}
		this.currentProj = Object.Instantiate<GameObject>(this.proj, this.mouth.position, this.headModel.transform.rotation);
		this.currentProj.transform.rotation = Quaternion.LookRotation(this.target.headPosition - this.mouth.position);
		if (this.difficulty >= 4)
		{
			switch (this.currentBurst % 5)
			{
			case 1:
				this.currentProj.transform.LookAt(this.target.headPosition + base.transform.right * (float)(1 + this.currentBurst / 5 * 2));
				break;
			case 2:
				this.currentProj.transform.LookAt(this.target.headPosition + base.transform.up * (float)(1 + this.currentBurst / 5 * 2));
				break;
			case 3:
				this.currentProj.transform.LookAt(this.target.headPosition - base.transform.right * (float)(1 + this.currentBurst / 5 * 2));
				break;
			case 4:
				this.currentProj.transform.LookAt(this.target.headPosition - base.transform.up * (float)(1 + this.currentBurst / 5 * 2));
				break;
			}
		}
		this.currentBurst++;
		Projectile component = this.currentProj.GetComponent<Projectile>();
		component.safeEnemyType = EnemyType.MaliciousFace;
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
		this.readyToShoot = false;
		if (this.difficulty >= 4)
		{
			base.Invoke("ReadyToShoot", 0.05f / this.eid.totalSpeedModifier);
			return;
		}
		if (this.difficulty > 0)
		{
			base.Invoke("ReadyToShoot", 0.1f / this.eid.totalSpeedModifier);
			return;
		}
		base.Invoke("ReadyToShoot", 0.2f / this.eid.totalSpeedModifier);
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x000C5000 File Offset: 0x000C3200
	private void ChargeBeam()
	{
		this.charging = true;
		this.currentCE = Object.Instantiate<GameObject>(this.chargeEffect, this.mouth);
		this.currentCE.transform.localScale = Vector3.zero;
		this.ceAud = this.currentCE.GetComponent<AudioSource>();
		this.ceLight = this.currentCE.GetComponent<Light>();
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x000C5064 File Offset: 0x000C3264
	private void BeamChargeEnd()
	{
		if (this.beamsAmount <= 1 && this.ceAud)
		{
			this.ceAud.Stop();
		}
		if (this.target == null)
		{
			return;
		}
		Vector3 velocity = this.target.GetVelocity();
		Vector3 vector = new Vector3(velocity.x, velocity.y / (float)((this.eid.target.isPlayer && MonoSingleton<NewMovement>.Instance.ridingRocket) ? 1 : 2), velocity.z);
		this.predictedPlayerPos = ((this.eid.target.isPlayer && MonoSingleton<NewMovement>.Instance.ridingRocket) ? MonoSingleton<NewMovement>.Instance.ridingRocket.transform.position : this.target.position) + vector / 2f / this.eid.totalSpeedModifier;
		RaycastHit raycastHit;
		if (velocity.magnitude > 1f && this.headCollider.Raycast(new Ray(this.target.position, velocity.normalized), out raycastHit, velocity.magnitude * 0.5f / this.eid.totalSpeedModifier))
		{
			this.predictedPlayerPos = this.target.position;
		}
		else if (Physics.Raycast(this.target.position, this.predictedPlayerPos - this.target.position, out raycastHit, Vector3.Distance(this.predictedPlayerPos, this.target.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Collide))
		{
			this.predictedPlayerPos = raycastHit.point;
		}
		if (this.nma)
		{
			this.nma.enabled = false;
		}
		this.predictedRot = Quaternion.LookRotation(this.predictedPlayerPos - base.transform.position);
		this.rotating = true;
		Object.Instantiate<GameObject>(this.spark, this.mouth.position, this.mouth.rotation).transform.LookAt(this.predictedPlayerPos);
		if (this.difficulty > 1)
		{
			base.Invoke("BeamFire", 0.5f / this.eid.totalSpeedModifier);
		}
		else if (this.difficulty == 1)
		{
			base.Invoke("BeamFire", 0.75f / this.eid.totalSpeedModifier);
		}
		else
		{
			base.Invoke("BeamFire", 1f / this.eid.totalSpeedModifier);
		}
		this.parryable = true;
		if (this.parryFramesLeft > 0)
		{
			this.eid.hitter = "punch";
			this.eid.DeliverDamage(base.gameObject, MonoSingleton<CameraController>.Instance.transform.forward * 25000f, base.transform.position, 1f, false, 0f, null, false, false);
		}
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x000C534C File Offset: 0x000C354C
	private void BeamFire()
	{
		this.parryable = false;
		if (!this.eid.dead)
		{
			this.currentBeam = Object.Instantiate<GameObject>(this.spiderBeam, this.mouth.position, this.mouth.rotation);
			this.rotating = false;
			RevolverBeam revolverBeam;
			if (this.eid.totalDamageModifier != 1f && this.currentBeam.TryGetComponent<RevolverBeam>(out revolverBeam))
			{
				revolverBeam.damage *= this.eid.totalDamageModifier;
			}
			if (this.beamsAmount > 1)
			{
				this.beamsAmount--;
				this.ceAud.pitch = 4f;
				this.ceAud.volume = 1f;
				base.Invoke("BeamChargeEnd", 0.5f / this.eid.totalSpeedModifier);
				return;
			}
			Object.Destroy(this.currentCE);
			base.Invoke("StopWaiting", 1f / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x000C5452 File Offset: 0x000C3652
	private void StopWaiting()
	{
		if (!this.eid.dead)
		{
			this.beamCharge = 0f;
		}
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x000C546C File Offset: 0x000C366C
	private void ReadyToShoot()
	{
		this.readyToShoot = true;
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x000C5478 File Offset: 0x000C3678
	public void TriggerHit(Collider other)
	{
		if (this.falling)
		{
			EnemyIdentifier enemyIdentifier = other.gameObject.GetComponent<EnemyIdentifier>();
			if (enemyIdentifier == null)
			{
				EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
				if (component != null && component.eid != null)
				{
					enemyIdentifier = component.eid;
				}
			}
			IdolMauricer idolMauricer;
			if (enemyIdentifier == null && other.gameObject.TryGetComponent<IdolMauricer>(out idolMauricer))
			{
				enemyIdentifier = other.gameObject.GetComponentInParent<EnemyIdentifier>();
			}
			if (enemyIdentifier && enemyIdentifier != this.eid && !this.fallEnemiesHit.Contains(enemyIdentifier))
			{
				this.FallKillEnemy(enemyIdentifier);
			}
		}
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x000C5520 File Offset: 0x000C3720
	private void FallKillEnemy(EnemyIdentifier targetEid)
	{
		if (MonoSingleton<StyleHUD>.Instance && !targetEid.dead)
		{
			MonoSingleton<StyleHUD>.Instance.AddPoints(80, "ultrakill.mauriced", null, this.eid, -1, "", "");
		}
		targetEid.hitter = "maurice";
		this.fallEnemiesHit.Add(targetEid);
		Collider collider;
		if (targetEid.TryGetComponent<Collider>(out collider))
		{
			Physics.IgnoreCollision(this.headCollider, collider, true);
		}
		EnemyIdentifier.FallOnEnemy(targetEid);
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x000C5598 File Offset: 0x000C3798
	private void OnCollisionEnter(Collision other)
	{
		if (!this.falling)
		{
			return;
		}
		if (other.gameObject.CompareTag("Moving"))
		{
			this.BreakCorpse();
			MonoSingleton<CameraController>.Instance.CameraShake(2f);
			return;
		}
		if (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
		{
			Breakable breakable;
			if (other.gameObject.CompareTag("Floor"))
			{
				this.rb.isKinematic = true;
				this.rb.useGravity = false;
				Transform transform = base.transform;
				Object.Instantiate<GameObject>(this.impactParticle, transform.position, transform.rotation);
				this.spriteRot.eulerAngles = new Vector3(other.contacts[0].normal.x + 90f, other.contacts[0].normal.y, other.contacts[0].normal.z);
				this.spritePos = new Vector3(other.contacts[0].point.x, other.contacts[0].point.y + 0.1f, other.contacts[0].point.z);
				AudioSource componentInChildren = Object.Instantiate<GameObject>(this.shockwave.ToAsset(), this.spritePos, Quaternion.identity).GetComponentInChildren<AudioSource>();
				if (componentInChildren)
				{
					Object.Destroy(componentInChildren);
				}
				Transform transform2 = base.transform;
				transform2.position -= transform2.up * 1.5f;
				this.falling = false;
				MaliciousFaceCatcher maliciousFaceCatcher;
				if (!other.gameObject.TryGetComponent<MaliciousFaceCatcher>(out maliciousFaceCatcher))
				{
					Object.Instantiate<GameObject>(this.impactSprite, this.spritePos, this.spriteRot).transform.SetParent(this.gz.goreZone, true);
				}
				SphereCollider sphereCollider;
				if (base.TryGetComponent<SphereCollider>(out sphereCollider))
				{
					Object.Destroy(sphereCollider);
				}
				SpiderBodyTrigger componentInChildren2 = base.transform.parent.GetComponentInChildren<SpiderBodyTrigger>(true);
				if (componentInChildren2)
				{
					Object.Destroy(componentInChildren2.gameObject);
				}
				this.rb.GetComponent<NavMeshObstacle>().enabled = true;
				MonoSingleton<CameraController>.Instance.CameraShake(2f);
				if (this.fallEnemiesHit.Count > 0)
				{
					foreach (EnemyIdentifier enemyIdentifier in this.fallEnemiesHit)
					{
						Collider collider;
						if (enemyIdentifier != null && !enemyIdentifier.dead && enemyIdentifier.TryGetComponent<Collider>(out collider))
						{
							Physics.IgnoreCollision(this.headCollider, collider, false);
						}
					}
					this.fallEnemiesHit.Clear();
					return;
				}
			}
			else if (other.gameObject.TryGetComponent<Breakable>(out breakable) && !breakable.playerOnly && !breakable.specialCaseOnly)
			{
				breakable.Break();
			}
		}
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x000C5888 File Offset: 0x000C3A88
	public void BreakCorpse()
	{
		if (!this.corpseBroken)
		{
			this.corpseBroken = true;
			if (this.breakParticle != null)
			{
				Transform transform = base.transform;
				Object.Instantiate<GameObject>(this.breakParticle, transform.position, transform.rotation).transform.SetParent(this.gz.gibZone);
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000C58F0 File Offset: 0x000C3AF0
	private void ResolveStuckness()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, 2f, LayerMaskDefaults.Get(LMD.Environment));
		if (array != null && array.Length != 0)
		{
			SphereCollider component = base.GetComponent<SphereCollider>();
			foreach (Collider collider in array)
			{
				Vector3 vector;
				float num;
				Physics.ComputePenetration(component, base.transform.position, base.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector, out num);
				base.transform.position = base.transform.position + vector * (num + 0.5f);
			}
		}
		array = Physics.OverlapSphere(base.transform.position, 2f, LayerMaskDefaults.Get(LMD.Environment));
		if (array != null && array.Length != 0)
		{
			this.BreakCorpse();
		}
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x000C59DC File Offset: 0x000C3BDC
	public void Enrage()
	{
		if (this.eid.dead)
		{
			return;
		}
		if (this.isEnraged)
		{
			return;
		}
		this.isEnraged = true;
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = true;
		}
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, base.transform);
		this.currentEnrageEffect.transform.localScale = Vector3.one * 0.2f;
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x000C5A78 File Offset: 0x000C3C78
	public void UnEnrage()
	{
		if (this.eid.dead)
		{
			return;
		}
		if (!this.isEnraged)
		{
			return;
		}
		this.isEnraged = false;
		if (this.ensims == null || this.ensims.Length == 0)
		{
			this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = false;
		}
		if (this.currentEnrageEffect != null)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06001821 RID: 6177 RVA: 0x000C5AF7 File Offset: 0x000C3CF7
	public string alterKey
	{
		get
		{
			return "spider";
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06001822 RID: 6178 RVA: 0x000C5AFE File Offset: 0x000C3CFE
	public string alterCategoryName
	{
		get
		{
			return "malicious face";
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06001823 RID: 6179 RVA: 0x000C5B08 File Offset: 0x000C3D08
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

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06001824 RID: 6180 RVA: 0x000C5B57 File Offset: 0x000C3D57
	// (set) Token: 0x06001825 RID: 6181 RVA: 0x000C5B5F File Offset: 0x000C3D5F
	public bool isEnraged { get; private set; }

	// Token: 0x040021B1 RID: 8625
	private NavMeshAgent nma;

	// Token: 0x040021B2 RID: 8626
	private Quaternion followPlayerRot;

	// Token: 0x040021B3 RID: 8627
	public GameObject proj;

	// Token: 0x040021B4 RID: 8628
	private RaycastHit hit2;

	// Token: 0x040021B5 RID: 8629
	private bool readyToShoot = true;

	// Token: 0x040021B6 RID: 8630
	private float burstCharge = 5f;

	// Token: 0x040021B7 RID: 8631
	private int maxBurst;

	// Token: 0x040021B8 RID: 8632
	private int currentBurst;

	// Token: 0x040021B9 RID: 8633
	public float health;

	// Token: 0x040021BA RID: 8634
	public bool stationary;

	// Token: 0x040021BB RID: 8635
	private Rigidbody rb;

	// Token: 0x040021BC RID: 8636
	private bool falling;

	// Token: 0x040021BD RID: 8637
	private Enemy enemy;

	// Token: 0x040021BE RID: 8638
	private Transform firstChild;

	// Token: 0x040021BF RID: 8639
	private CharacterJoint[] cjs;

	// Token: 0x040021C0 RID: 8640
	private CharacterJoint cj;

	// Token: 0x040021C1 RID: 8641
	public GameObject impactParticle;

	// Token: 0x040021C2 RID: 8642
	public GameObject impactSprite;

	// Token: 0x040021C3 RID: 8643
	private Quaternion spriteRot;

	// Token: 0x040021C4 RID: 8644
	private Vector3 spritePos;

	// Token: 0x040021C5 RID: 8645
	public Transform mouth;

	// Token: 0x040021C6 RID: 8646
	private GameObject currentProj;

	// Token: 0x040021C7 RID: 8647
	private bool charging;

	// Token: 0x040021C8 RID: 8648
	public GameObject chargeEffect;

	// Token: 0x040021C9 RID: 8649
	[HideInInspector]
	public GameObject currentCE;

	// Token: 0x040021CA RID: 8650
	private float beamCharge;

	// Token: 0x040021CB RID: 8651
	private AudioSource ceAud;

	// Token: 0x040021CC RID: 8652
	private Light ceLight;

	// Token: 0x040021CD RID: 8653
	private Vector3 predictedPlayerPos;

	// Token: 0x040021CE RID: 8654
	public GameObject spiderBeam;

	// Token: 0x040021CF RID: 8655
	private GameObject currentBeam;

	// Token: 0x040021D0 RID: 8656
	public AssetReference beamExplosion;

	// Token: 0x040021D1 RID: 8657
	private GameObject currentExplosion;

	// Token: 0x040021D2 RID: 8658
	private float beamProbability;

	// Token: 0x040021D3 RID: 8659
	private Quaternion predictedRot;

	// Token: 0x040021D4 RID: 8660
	private bool rotating;

	// Token: 0x040021D5 RID: 8661
	public GameObject dripBlood;

	// Token: 0x040021D6 RID: 8662
	private GameObject currentDrip;

	// Token: 0x040021D7 RID: 8663
	public AudioClip hurtSound;

	// Token: 0x040021D8 RID: 8664
	private StyleCalculator scalc;

	// Token: 0x040021D9 RID: 8665
	private EnemyIdentifier eid;

	// Token: 0x040021DA RID: 8666
	public GameObject spark;

	// Token: 0x040021DB RID: 8667
	private int difficulty;

	// Token: 0x040021DC RID: 8668
	private float coolDownMultiplier = 1f;

	// Token: 0x040021DD RID: 8669
	private int beamsAmount = 1;

	// Token: 0x040021DE RID: 8670
	private float maxHealth;

	// Token: 0x040021DF RID: 8671
	public GameObject enrageEffect;

	// Token: 0x040021E0 RID: 8672
	[HideInInspector]
	public GameObject currentEnrageEffect;

	// Token: 0x040021E1 RID: 8673
	private Material origMaterial;

	// Token: 0x040021E2 RID: 8674
	public Material woundedMaterial;

	// Token: 0x040021E3 RID: 8675
	public Material woundedEnrageMaterial;

	// Token: 0x040021E4 RID: 8676
	public GameObject woundedParticle;

	// Token: 0x040021E5 RID: 8677
	private bool parryable;

	// Token: 0x040021E6 RID: 8678
	private MusicManager muman;

	// Token: 0x040021E7 RID: 8679
	private bool requestedMusic;

	// Token: 0x040021E8 RID: 8680
	private GoreZone gz;

	// Token: 0x040021E9 RID: 8681
	[SerializeField]
	private Transform headModel;

	// Token: 0x040021EA RID: 8682
	public GameObject breakParticle;

	// Token: 0x040021EB RID: 8683
	private bool corpseBroken;

	// Token: 0x040021EC RID: 8684
	public AssetReference shockwave;

	// Token: 0x040021ED RID: 8685
	private EnemySimplifier[] ensims;

	// Token: 0x040021EE RID: 8686
	public Renderer mainMesh;

	// Token: 0x040021EF RID: 8687
	public float targetHeight = 1f;

	// Token: 0x040021F0 RID: 8688
	private float defaultHeight;

	// Token: 0x040021F1 RID: 8689
	[SerializeField]
	private Collider headCollider;

	// Token: 0x040021F2 RID: 8690
	private List<EnemyIdentifier> fallEnemiesHit = new List<EnemyIdentifier>();

	// Token: 0x040021F3 RID: 8691
	private int parryFramesLeft;
}
