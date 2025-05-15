using System;
using System.Collections.Generic;
using Sandbox;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000126 RID: 294
public class Drone : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x0600056C RID: 1388 RVA: 0x000246B8 File Offset: 0x000228B8
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x000246C8 File Offset: 0x000228C8
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.rb = base.GetComponent<Rigidbody>();
		this.kib = base.GetComponent<KeepInBounds>();
		this.type = this.eid.enemyType;
		if (this.type == EnemyType.Virtue)
		{
			this.vc = MonoSingleton<EnemyCooldowns>.Instance;
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00024720 File Offset: 0x00022920
	private void Start()
	{
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		if (!this.chargeParticle)
		{
			this.chargeParticle = base.GetComponentInChildren<ParticleSystem>();
		}
		if (this.type == EnemyType.Virtue)
		{
			this.anim = base.GetComponent<Animator>();
		}
		this.dodgeCooldown = Random.Range(0.5f, 3f);
		if (this.type == EnemyType.Drone)
		{
			this.attackCooldown = Random.Range(1f, 3f);
		}
		else
		{
			this.attackCooldown = 1.5f;
		}
		if (!this.dontStartAware)
		{
			this.targetSpotted = true;
		}
		if (this.type == EnemyType.Drone)
		{
			this.modelTransform = base.transform.Find("drone");
			if (this.modelTransform)
			{
				this.ensims = this.modelTransform.GetComponentsInChildren<EnemySimplifier>();
				this.origMaterial = this.ensims[0].GetComponent<Renderer>().material;
			}
			this.rb.solverIterations *= 3;
			this.rb.solverVelocityIterations *= 3;
		}
		this.SlowUpdate();
		if (!this.musicRequested && !this.eid.dead)
		{
			this.musicRequested = true;
			MonoSingleton<MusicManager>.Instance.PlayBattleMusic();
		}
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		if (this.enraged)
		{
			this.Enrage();
		}
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
			return;
		}
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x000248B2 File Offset: 0x00022AB2
	private void UpdateBuff()
	{
		if (this.anim)
		{
			this.anim.speed = this.eid.totalSpeedModifier;
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x000248D8 File Offset: 0x00022AD8
	private void OnDisable()
	{
		if (this.type == EnemyType.Virtue && this.vc)
		{
			this.vc.RemoveVirtue(this);
		}
		if (this.musicRequested)
		{
			this.musicRequested = false;
			MusicManager instance = MonoSingleton<MusicManager>.Instance;
			if (instance)
			{
				instance.PlayCleanMusic();
			}
		}
		if (!MonoSingleton<EnemyTracker>.Instance)
		{
			return;
		}
		if (this.type == EnemyType.Drone && MonoSingleton<EnemyTracker>.Instance.drones.Contains(this))
		{
			MonoSingleton<EnemyTracker>.Instance.drones.Remove(this);
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00024964 File Offset: 0x00022B64
	private void OnEnable()
	{
		if (this.type == EnemyType.Virtue && this.vc)
		{
			this.vc.AddVirtue(this);
		}
		if (!this.musicRequested && !this.eid.dead)
		{
			this.musicRequested = true;
			MonoSingleton<MusicManager>.Instance.PlayBattleMusic();
		}
		if (this.type == EnemyType.Drone && !MonoSingleton<EnemyTracker>.Instance.drones.Contains(this))
		{
			MonoSingleton<EnemyTracker>.Instance.drones.Add(this);
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x000249E8 File Offset: 0x00022BE8
	private void UpdateRigidbodySettings()
	{
		if (this.target == null && !this.crashing)
		{
			this.rb.drag = 3f;
			this.rb.angularDrag = 3f;
			return;
		}
		this.rb.drag = 0f;
		this.rb.angularDrag = 0f;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00024A48 File Offset: 0x00022C48
	private void Update()
	{
		if (!this.crashing)
		{
			this.UpdateRigidbodySettings();
			if (this.target == null)
			{
				return;
			}
			if (this.targetSpotted)
			{
				this.viewTarget = this.target.position;
				float num = (float)(this.difficulty / 2);
				if (num == 0f)
				{
					num = 0.25f;
				}
				num *= this.eid.totalSpeedModifier;
				if (this.dodgeCooldown > 0f)
				{
					this.dodgeCooldown = Mathf.MoveTowards(this.dodgeCooldown, 0f, Time.deltaTime * num);
				}
				else if (!this.stationary && !this.lockPosition)
				{
					this.dodgeCooldown = Random.Range(1f, 3f);
					this.RandomDodge();
				}
			}
			if ((this.type == EnemyType.Virtue && (!this.target.isPlayer || !MonoSingleton<NewMovement>.Instance.levelOver) && (Vector3.Distance(base.transform.position, this.target.position) < 150f || this.stationary)) || this.targetSpotted)
			{
				float num2 = (float)(this.difficulty / 2);
				if (this.type == EnemyType.Virtue && this.difficulty >= 4)
				{
					num2 = 1.2f;
				}
				else if (this.difficulty == 1)
				{
					num2 = 0.75f;
				}
				else if (this.difficulty == 0)
				{
					num2 = 0.5f;
				}
				num2 *= this.eid.totalSpeedModifier;
				if (this.attackCooldown > 0f)
				{
					this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * num2);
				}
				else if (this.projectile != null && (!this.vc || this.vc.virtueCooldown == 0f))
				{
					if (this.vc)
					{
						this.vc.virtueCooldown = 1f / this.eid.totalSpeedModifier;
					}
					this.parryable = true;
					this.PlaySound(this.windUpSound);
					if (this.chargeParticle != null)
					{
						this.chargeParticle.Play();
					}
					if (this.shootMaterial != null && this.ensims != null && this.ensims.Length != 0)
					{
						EnemySimplifier[] array = this.ensims;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.shootMaterial);
						}
					}
					if (this.type == EnemyType.Drone)
					{
						this.attackCooldown = Random.Range(2f, 4f);
						base.Invoke("Shoot", 0.75f / this.eid.totalSpeedModifier);
					}
					else
					{
						this.attackCooldown = Random.Range(4f, 6f);
						if (this.anim != null)
						{
							this.anim.SetTrigger("Attack");
						}
					}
					if (this.parryFramesLeft > 0)
					{
						this.eid.hitter = "punch";
						this.eid.DeliverDamage(base.gameObject, MonoSingleton<CameraController>.Instance.transform.forward * 25000f, base.transform.position, 1f, false, 0f, null, false, false);
						this.parryFramesLeft = 0;
					}
				}
			}
		}
		if (this.eid && this.eid.hooked && !this.hooked)
		{
			this.Hooked();
			return;
		}
		if (this.eid && !this.eid.hooked && this.hooked)
		{
			this.Unhooked();
		}
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00024DCC File Offset: 0x00022FCC
	private void SlowUpdate()
	{
		if (!this.crashing && this.target != null)
		{
			if (this.targetSpotted)
			{
				if (Physics.Raycast(base.transform.position, this.target.position - base.transform.position, Vector3.Distance(base.transform.position, this.target.position) - 1f, this.relevantSightBlockMask))
				{
					this.targetSpotted = false;
					this.PlaySound(this.loseSound);
					this.lastKnownPos = this.target.position;
					this.blockCooldown = 0f;
					this.checkCooldown = 0f;
					this.toLastKnownPos = true;
				}
			}
			else if (!Physics.Raycast(base.transform.position, this.target.position - base.transform.position, Vector3.Distance(base.transform.position, this.target.position) - 1f, this.relevantSightBlockMask))
			{
				this.PlaySound(this.spotSound);
				this.targetSpotted = true;
			}
			if (this.difficulty >= 4 && MonoSingleton<EnemyTracker>.Instance.drones.Count > 1)
			{
				Vector3 vector = Vector3.zero;
				foreach (Drone drone in MonoSingleton<EnemyTracker>.Instance.drones)
				{
					if (!(drone == this) && Vector3.Distance(drone.transform.position, base.transform.position) < 10f)
					{
						vector += base.transform.position - drone.transform.position;
					}
				}
				if (vector.magnitude > 0f)
				{
					this.Dodge(vector);
				}
			}
		}
		base.Invoke("SlowUpdate", 0.25f);
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00024FD8 File Offset: 0x000231D8
	private void FixedUpdate()
	{
		if (this.parryFramesLeft > 0)
		{
			this.parryFramesLeft--;
		}
		if (this.rb.velocity.magnitude < 1f && this.rb.collisionDetectionMode != CollisionDetectionMode.Discrete)
		{
			this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}
		if (this.crashing)
		{
			if (this.type == EnemyType.Virtue)
			{
				if (this.parried)
				{
					this.rb.useGravity = false;
					if (!this.rb.isKinematic)
					{
						this.rb.velocity = base.transform.forward * 120f * this.eid.totalSpeedModifier;
					}
				}
			}
			else if (!this.parried)
			{
				float num = 50f;
				if (this.difficulty == 1)
				{
					num = 40f;
				}
				else if (this.difficulty == 0)
				{
					num = 25f;
				}
				num *= this.eid.totalSpeedModifier;
				this.rb.AddForce(base.transform.forward * num, ForceMode.Acceleration);
				Transform transform = this.modelTransform;
				if (transform != null)
				{
					transform.Rotate(0f, 0f, 10f, Space.Self);
				}
			}
			else
			{
				if (!this.rb.isKinematic)
				{
					this.rb.velocity = base.transform.forward * 50f;
				}
				Transform transform2 = this.modelTransform;
				if (transform2 != null)
				{
					transform2.Rotate(0f, 0f, 50f, Space.Self);
				}
			}
		}
		else if (this.targetSpotted && this.target != null)
		{
			if (this.type == EnemyType.Drone)
			{
				if (!this.rb.isKinematic)
				{
					this.rb.velocity *= 0.95f;
				}
				if (!this.stationary && !this.lockPosition)
				{
					float num2 = 50f;
					if (this.difficulty >= 4)
					{
						num2 = 250f;
					}
					if (Vector3.Distance(base.transform.position, this.target.position) > this.preferredDistanceToTarget)
					{
						this.rb.AddForce(base.transform.forward * num2 * this.eid.totalSpeedModifier, ForceMode.Acceleration);
					}
					else if (Vector3.Distance(base.transform.position, this.target.position) < 5f)
					{
						if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
						{
							this.rb.AddForce(base.transform.forward * -0.1f * this.eid.totalSpeedModifier, ForceMode.Impulse);
						}
						else
						{
							this.rb.AddForce(base.transform.forward * -num2 * this.eid.totalSpeedModifier, ForceMode.Impulse);
						}
					}
				}
			}
			else
			{
				if (!this.rb.isKinematic)
				{
					this.rb.velocity *= 0.975f;
				}
				if (!this.stationary && Vector3.Distance(base.transform.position, this.target.position) > 15f)
				{
					this.rb.AddForce(base.transform.forward * 10f * this.eid.totalSpeedModifier, ForceMode.Acceleration);
				}
			}
		}
		else if (this.toLastKnownPos && !this.stationary && !this.lockPosition && this.target != null)
		{
			if (this.blockCooldown == 0f)
			{
				this.viewTarget = this.lastKnownPos;
			}
			else
			{
				this.blockCooldown = Mathf.MoveTowards(this.blockCooldown, 0f, 0.01f);
			}
			this.rb.AddForce(base.transform.forward * 10f * this.eid.totalSpeedModifier, ForceMode.Acceleration);
			if (this.checkCooldown == 0f && Vector3.Distance(base.transform.position, this.lastKnownPos) > 5f)
			{
				this.checkCooldown = 0.1f;
				if (Physics.BoxCast(base.transform.position - (this.viewTarget - base.transform.position).normalized, Vector3.one, this.viewTarget - base.transform.position, base.transform.rotation, 4f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
				{
					this.blockCooldown = Random.Range(1.5f, 3f);
					Vector3 vector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
					this.viewTarget = base.transform.position + vector * 100f;
				}
			}
			else if (Vector3.Distance(base.transform.position, this.lastKnownPos) <= 3f)
			{
				RaycastHit raycastHit;
				Physics.Raycast(base.transform.position, Random.onUnitSphere, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies));
				this.lastKnownPos = raycastHit.point;
			}
			if (this.checkCooldown != 0f)
			{
				this.checkCooldown = Mathf.MoveTowards(this.checkCooldown, 0f, 0.01f);
			}
		}
		if (!this.crashing)
		{
			if (!this.lockRotation && this.target != null)
			{
				Quaternion quaternion = Quaternion.LookRotation(this.viewTarget - base.transform.position);
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, quaternion, 0.075f + 0.00025f * Quaternion.Angle(base.transform.rotation, quaternion) * this.eid.totalSpeedModifier);
			}
			if (!this.rb.isKinematic)
			{
				this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, 50f * this.eid.totalSpeedModifier);
			}
			if (this.kib)
			{
				this.kib.ValidateMove();
			}
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002565C File Offset: 0x0002385C
	public void RandomDodge()
	{
		if ((this.difficulty == 1 && Random.Range(0f, 1f) > 0.75f) || this.difficulty == 0)
		{
			return;
		}
		this.Dodge(base.transform.up * Random.Range(-5f, 5f) + base.transform.right * Random.Range(-5f, 5f));
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000256DC File Offset: 0x000238DC
	public void Dodge(Vector3 direction)
	{
		float num = 50f;
		if (this.type == EnemyType.Virtue)
		{
			num = 150f;
		}
		num *= this.eid.totalSpeedModifier;
		this.rb.AddForce(direction.normalized * num, ForceMode.Impulse);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00025728 File Offset: 0x00023928
	public void GetHurt(Vector3 force, float multiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
	{
		bool flag = false;
		if (!this.crashing)
		{
			if ((this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone") && !this.parryable && this.health - multiplier > 0f)
			{
				return;
			}
			if (((this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone") && this.parryable) || this.eid.hitter == "punch")
			{
				if (this.parryable)
				{
					if (!InvincibleEnemies.Enabled && !this.eid.blessed)
					{
						multiplier = (float)((this.parryFramesLeft > 0) ? 3 : 4);
					}
					MonoSingleton<FistControl>.Instance.currentPunch.Parry(false, this.eid, "");
					this.parryable = false;
				}
				else
				{
					this.parryFramesLeft = MonoSingleton<FistControl>.Instance.currentPunch.activeFrames;
				}
			}
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= 1f * multiplier;
			}
			this.health = (float)Math.Round((double)this.health, 4);
			if ((double)this.health <= 0.001)
			{
				this.health = 0f;
			}
			if (this.eid == null)
			{
				this.eid = base.GetComponent<EnemyIdentifier>();
			}
			if (this.health <= 0f)
			{
				flag = true;
			}
			if (this.homeRunnable && !this.fleshDrone && !this.eid.puppet && flag && (this.eid.hitter == "punch" || this.eid.hitter == "heavypunch" || this.eid.hitter == "hammer"))
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(100, "ultrakill.homerun", sourceWeapon, this.eid, -1, "", "");
				MonoSingleton<StyleCalculator>.Instance.AddToMultiKill(null);
			}
			else if (this.eid.hitter != "enemy" && !this.eid.puppet && multiplier != 0f)
			{
				if (this.scalc == null)
				{
					this.scalc = MonoSingleton<StyleCalculator>.Instance;
				}
				if (this.scalc)
				{
					this.scalc.HitCalculator(this.eid.hitter, "drone", "", flag, this.eid, sourceWeapon);
				}
			}
			if (this.health <= 0f && !this.crashing)
			{
				this.parryable = false;
				this.Death(fromExplosion);
				if (this.eid.hitter != "punch" && this.eid.hitter != "heavypunch" && this.eid.hitter != "hammer")
				{
					if (this.target != null)
					{
						this.crashTarget = this.target.position;
					}
				}
				else
				{
					this.canHurtOtherDrones = true;
					base.transform.position += force.normalized;
					this.crashTarget = base.transform.position + force;
					if (!this.rb.isKinematic)
					{
						this.rb.velocity = force.normalized * 40f;
					}
				}
				base.transform.LookAt(this.crashTarget);
				if (this.aud == null)
				{
					this.aud = base.GetComponent<AudioSource>();
				}
				if (this.type == EnemyType.Drone)
				{
					this.aud.clip = this.deathSound;
					this.aud.volume = 0.75f;
					this.aud.pitch = Random.Range(0.85f, 1.35f);
					this.aud.priority = 11;
					this.aud.Play();
				}
				else
				{
					this.PlaySound(this.deathSound);
				}
				base.Invoke("CanInterruptCrash", 0.5f);
				base.Invoke("Explode", 5f);
				return;
			}
			if (!(this.eid.hitter != "fire"))
			{
				this.PlaySound(this.hurtSound);
				return;
			}
			GameObject gameObject = null;
			Bloodsplatter bloodsplatter = null;
			if (multiplier != 0f)
			{
				if (!this.eid.blessed)
				{
					this.PlaySound(this.hurtSound);
				}
				gameObject = this.bsm.GetGore(GoreType.Body, this.eid, fromExplosion);
				gameObject.transform.position = base.transform.position;
				gameObject.SetActive(true);
				gameObject.transform.SetParent(this.gz.goreZone, true);
				if (this.eid.hitter == "drill")
				{
					gameObject.transform.localScale *= 2f;
				}
				bloodsplatter = gameObject.GetComponent<Bloodsplatter>();
			}
			if (this.health > 0f)
			{
				if (this.eid.hitter == "nail")
				{
					bloodsplatter.hpAmount = ((this.type == EnemyType.Virtue) ? 3 : 1);
					bloodsplatter.GetComponent<AudioSource>().volume *= 0.8f;
				}
				if (bloodsplatter)
				{
					bloodsplatter.GetReady();
				}
				if (!this.eid.blessed && !this.rb.isKinematic)
				{
					this.rb.velocity = this.rb.velocity / 10f;
					this.rb.AddForce(force.normalized * (force.magnitude / 100f), ForceMode.Impulse);
					this.rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
					if (this.rb.velocity.magnitude > 50f)
					{
						this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, 50f);
					}
				}
			}
			if (multiplier >= 1f)
			{
				if (bloodsplatter)
				{
					bloodsplatter.hpAmount = 30;
				}
				if (this.gib != null)
				{
					int num = 0;
					while ((float)num <= multiplier)
					{
						Object.Instantiate<GameObject>(this.gib.ToAsset(), base.transform.position, Random.rotation).transform.SetParent(this.gz.gibZone, true);
						num++;
					}
				}
			}
			ParticleSystem particleSystem;
			if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && gameObject && gameObject.TryGetComponent<ParticleSystem>(out particleSystem))
			{
				particleSystem.Play();
				return;
			}
		}
		else if ((this.eid.hitter == "punch" || this.eid.hitter == "hammer") && !this.parried)
		{
			this.parried = true;
			if (!this.rb.isKinematic)
			{
				this.rb.velocity = Vector3.zero;
			}
			base.transform.rotation = MonoSingleton<CameraController>.Instance.transform.rotation;
			Punch currentPunch = MonoSingleton<FistControl>.Instance.currentPunch;
			if (this.eid.hitter == "punch")
			{
				currentPunch.GetComponent<Animator>().Play("Hook", -1, 0.065f);
				currentPunch.Parry(false, this.eid, "");
			}
			Collider collider;
			if (this.type == EnemyType.Virtue && base.TryGetComponent<Collider>(out collider))
			{
				collider.isTrigger = true;
				return;
			}
		}
		else if (multiplier >= 1f || this.canInterruptCrash)
		{
			this.Explode();
		}
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00025EE0 File Offset: 0x000240E0
	public void PlaySound(AudioClip clippe)
	{
		if (!clippe)
		{
			return;
		}
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		this.aud.clip = clippe;
		if (this.type == EnemyType.Drone)
		{
			this.aud.volume = 0.5f;
			this.aud.pitch = Random.Range(0.85f, 1.35f);
		}
		this.aud.priority = 12;
		this.aud.Play();
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00025F68 File Offset: 0x00024168
	public void Explode()
	{
		if (!this.exploded && base.gameObject.activeInHierarchy && (!this.cantInstaExplode || this.canInterruptCrash))
		{
			this.exploded = true;
			GameObject gameObject = Object.Instantiate<GameObject>(this.explosion.ToAsset(), base.transform.position, Quaternion.identity);
			gameObject.transform.SetParent(this.gz.transform, true);
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				if (this.eid.totalDamageModifier != 1f)
				{
					explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.eid.totalDamageModifier);
					explosion.maxSize *= this.eid.totalDamageModifier;
					explosion.speed *= this.eid.totalDamageModifier;
				}
				if (this.difficulty >= 4 && this.type == EnemyType.Drone && !this.parried && !this.canHurtOtherDrones)
				{
					explosion.toIgnore.Add(EnemyType.Drone);
				}
				if (this.killedByPlayer)
				{
					explosion.friendlyFire = true;
				}
			}
			DoubleRender componentInChildren = base.GetComponentInChildren<DoubleRender>();
			if (componentInChildren)
			{
				componentInChildren.RemoveEffect();
			}
			if (!this.crashing)
			{
				this.Death(true);
			}
			else if (this.eid.drillers.Count > 0)
			{
				for (int j = this.eid.drillers.Count - 1; j >= 0; j--)
				{
					Object.Destroy(this.eid.drillers[j].gameObject);
				}
			}
			if (GhostDroneMode.Enabled && this.ghost != null)
			{
				Object.Instantiate<GameObject>(this.ghost, base.transform.position, base.transform.rotation);
			}
			Object.Destroy(base.gameObject);
			if (this.musicRequested)
			{
				MusicManager instance = MonoSingleton<MusicManager>.Instance;
				if (instance)
				{
					instance.PlayCleanMusic();
				}
			}
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00026174 File Offset: 0x00024374
	private void Death(bool fromExplosion = false)
	{
		if (this.crashing)
		{
			return;
		}
		this.crashing = true;
		this.UpdateRigidbodySettings();
		if (this.rb.isKinematic)
		{
			this.rb.isKinematic = false;
		}
		if (this.type == EnemyType.Virtue)
		{
			this.rb.velocity = Vector3.zero;
			this.rb.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);
			this.rb.useGravity = true;
			if (this.childVi.Count > 0)
			{
				for (int i = 0; i < this.childVi.Count; i++)
				{
					if (this.childVi[i] != null && this.childVi[i].gameObject)
					{
						Object.Destroy(this.childVi[i].gameObject);
					}
				}
			}
		}
		if (this.eid.hitter != "enemy")
		{
			this.killedByPlayer = true;
		}
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
		}
		if (this.eid.hitter != "fire")
		{
			GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
			if (gore)
			{
				gore.transform.position = base.transform.position;
				ParticleSystem particleSystem;
				if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && gore.TryGetComponent<ParticleSystem>(out particleSystem))
				{
					particleSystem.Play();
				}
				gore.transform.SetParent(this.gz.goreZone, true);
				if (this.eid.hitter == "drill")
				{
					gore.transform.localScale *= 2f;
				}
				Bloodsplatter bloodsplatter;
				if (gore.TryGetComponent<Bloodsplatter>(out bloodsplatter))
				{
					bloodsplatter.GetReady();
				}
			}
		}
		if (!this.eid.dontCountAsKills)
		{
			ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
			if (componentInParent != null)
			{
				componentInParent.AddDeadEnemy();
			}
		}
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x000263CC File Offset: 0x000245CC
	public void Shoot()
	{
		this.parryable = false;
		if (!this.crashing && this.projectile.RuntimeKeyIsValid())
		{
			EnemySimplifier[] array = this.ensims;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.origMaterial);
			}
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.projectile.ToAsset(), base.transform.position + base.transform.forward, base.transform.rotation);
			gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, (float)Random.Range(0, 360));
			gameObject.transform.localScale *= 0.5f;
			this.SetProjectileSettings(gameObject.GetComponent<Projectile>());
			GameObject gameObject2 = Object.Instantiate<GameObject>(this.projectile.ToAsset(), gameObject.transform.position + gameObject.transform.up, gameObject.transform.rotation);
			if (this.difficulty > 2)
			{
				gameObject2.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x + 10f, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
			}
			gameObject2.transform.localScale *= 0.5f;
			this.SetProjectileSettings(gameObject2.GetComponent<Projectile>());
			gameObject2 = Object.Instantiate<GameObject>(this.projectile.ToAsset(), gameObject.transform.position - gameObject.transform.up, gameObject.transform.rotation);
			if (this.difficulty > 2)
			{
				gameObject2.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x - 10f, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z);
			}
			gameObject2.transform.localScale *= 0.5f;
			this.SetProjectileSettings(gameObject2.GetComponent<Projectile>());
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0002666C File Offset: 0x0002486C
	private void SetProjectileSettings(Projectile proj)
	{
		float num = 35f;
		if (this.difficulty >= 3)
		{
			num = 45f;
		}
		else if (this.difficulty == 1)
		{
			num = 25f;
		}
		else if (this.difficulty == 0)
		{
			num = 15f;
		}
		proj.damage *= this.eid.totalDamageModifier;
		proj.target = this.target;
		proj.safeEnemyType = EnemyType.Drone;
		proj.speed = num;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x000266E4 File Offset: 0x000248E4
	public void SpawnInsignia()
	{
		if (this.target == null)
		{
			return;
		}
		if (!this.crashing)
		{
			this.parryable = false;
			GameObject gameObject = Object.Instantiate<GameObject>(this.projectile.ToAsset(), this.target.position, Quaternion.identity);
			VirtueInsignia component = gameObject.GetComponent<VirtueInsignia>();
			component.target = this.target;
			component.parentDrone = this;
			component.hadParent = true;
			this.chargeParticle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
			if (this.enraged)
			{
				component.predictive = true;
			}
			if (this.difficulty == 1)
			{
				component.windUpSpeedMultiplier = 0.875f;
			}
			else if (this.difficulty == 0)
			{
				component.windUpSpeedMultiplier = 0.75f;
			}
			if (this.difficulty >= 4)
			{
				component.explosionLength = ((this.difficulty == 5) ? 5f : 3.5f);
			}
			if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
			{
				gameObject.transform.localScale *= 0.75f;
				component.windUpSpeedMultiplier *= 0.875f;
			}
			component.windUpSpeedMultiplier *= this.eid.totalSpeedModifier;
			component.damage = Mathf.RoundToInt((float)component.damage * this.eid.totalDamageModifier);
			this.usedAttacks++;
			if (((this.difficulty > 2 && this.usedAttacks > 2) || (this.difficulty == 2 && this.usedAttacks > 4 && !this.eid.blessed)) && !this.enraged && this.vc.currentVirtues.Count < 3)
			{
				base.Invoke("Enrage", 3f / this.eid.totalSpeedModifier);
			}
		}
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x000268A0 File Offset: 0x00024AA0
	private void OnCollisionStay(Collision collision)
	{
		if (this.crashing && (collision.gameObject.layer == 0 || LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment) || collision.gameObject.CompareTag("Player") || collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 12 || collision.gameObject.layer == 26))
		{
			this.Explode();
		}
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0002692C File Offset: 0x00024B2C
	private void OnTriggerEnter(Collider other)
	{
		if (this.crashing)
		{
			if ((this.type == EnemyType.Drone && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12)) || (!other.isTrigger && (other.gameObject.layer == 0 || LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) || other.gameObject.layer == 26 || other.gameObject.CompareTag("Player"))))
			{
				this.Explode();
				return;
			}
			if (this.type != EnemyType.Drone && (other.gameObject.layer == 10 || other.gameObject.layer == 11 || other.gameObject.layer == 12) && !this.checkingForCrash)
			{
				this.checkingForCrash = true;
				EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
				EnemyIdentifier component2;
				if (component && component.eid)
				{
					component2 = component.eid;
				}
				else
				{
					component2 = other.gameObject.GetComponent<EnemyIdentifier>();
				}
				if (component2)
				{
					bool flag = true;
					if (!component2.dead)
					{
						flag = false;
					}
					component2.hitter = "cannonball";
					component2.DeliverDamage(other.gameObject, (other.transform.position - base.transform.position).normalized * 100f, base.transform.position, 5f * component2.totalDamageModifier, true, 0f, null, false, false);
					if (!component2 || component2.dead)
					{
						if (!flag)
						{
							MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.cannonballed", null, component2, -1, "", "");
						}
						if (component2)
						{
							component2.Explode(false);
						}
						this.checkingForCrash = false;
						return;
					}
					this.Explode();
					return;
				}
				else
				{
					this.checkingForCrash = false;
				}
			}
		}
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00026B1F File Offset: 0x00024D1F
	private void CanInterruptCrash()
	{
		this.canInterruptCrash = true;
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00026B28 File Offset: 0x00024D28
	public void Hooked()
	{
		this.hooked = true;
		this.lockPosition = true;
		this.homeRunnable = true;
		base.CancelInvoke("DelayedUnhooked");
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00026B4A File Offset: 0x00024D4A
	public void Unhooked()
	{
		this.hooked = false;
		base.Invoke("DelayedUnhooked", 0.25f);
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00026B63 File Offset: 0x00024D63
	private void DelayedUnhooked()
	{
		if (!this.crashing)
		{
			base.Invoke("NoMoreHomeRun", 0.5f);
		}
		this.lockPosition = false;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00026B84 File Offset: 0x00024D84
	private void NoMoreHomeRun()
	{
		if (!this.crashing)
		{
			this.homeRunnable = false;
		}
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00026B98 File Offset: 0x00024D98
	public void Enrage()
	{
		if (this.isEnraged)
		{
			return;
		}
		if (this.type == EnemyType.Drone)
		{
			return;
		}
		this.isEnraged = true;
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, base.transform.position, base.transform.rotation);
		this.currentEnrageEffect.transform.SetParent(base.transform, true);
		this.enraged = true;
		EnemySimplifier[] componentsInChildren = base.GetComponentsInChildren<EnemySimplifier>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enraged = true;
		}
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00026C24 File Offset: 0x00024E24
	public void UnEnrage()
	{
		if (!this.isEnraged)
		{
			return;
		}
		this.isEnraged = false;
		EnemySimplifier[] array = base.GetComponentsInChildren<EnemySimplifier>();
		if (array == null || array.Length == 0)
		{
			array = base.GetComponentsInChildren<EnemySimplifier>();
		}
		Object.Destroy(this.currentEnrageEffect);
		EnemySimplifier[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].enraged = false;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x00026C7A File Offset: 0x00024E7A
	private int relevantSightBlockMask
	{
		get
		{
			return LayerMaskDefaults.Get(this.target.isPlayer ? LMD.EnvironmentAndBigEnemies : LMD.Environment);
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000589 RID: 1417 RVA: 0x00026C97 File Offset: 0x00024E97
	// (set) Token: 0x0600058A RID: 1418 RVA: 0x00026C9F File Offset: 0x00024E9F
	public bool isEnraged { get; private set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00026CA8 File Offset: 0x00024EA8
	public string alterKey
	{
		get
		{
			return "drone";
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x00026CA8 File Offset: 0x00024EA8
	public string alterCategoryName
	{
		get
		{
			return "drone";
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x00026CB0 File Offset: 0x00024EB0
	public AlterOption<bool>[] options
	{
		get
		{
			if (this.type != EnemyType.Drone)
			{
				return new AlterOption<bool>[]
				{
					new AlterOption<bool>
					{
						key = "enraged",
						name = "Enraged",
						value = this.isEnraged,
						callback = delegate(bool value)
						{
							if (value)
							{
								this.Enrage();
								return;
							}
							this.UnEnrage();
						}
					}
				};
			}
			return null;
		}
	}

	// Token: 0x0400077C RID: 1916
	public bool dontStartAware;

	// Token: 0x0400077D RID: 1917
	public bool stationary;

	// Token: 0x0400077E RID: 1918
	public float health;

	// Token: 0x0400077F RID: 1919
	public bool crashing;

	// Token: 0x04000780 RID: 1920
	private Vector3 crashTarget;

	// Token: 0x04000781 RID: 1921
	private Rigidbody rb;

	// Token: 0x04000782 RID: 1922
	private bool canInterruptCrash;

	// Token: 0x04000783 RID: 1923
	private Transform modelTransform;

	// Token: 0x04000784 RID: 1924
	public bool targetSpotted;

	// Token: 0x04000785 RID: 1925
	public bool toLastKnownPos;

	// Token: 0x04000786 RID: 1926
	private Vector3 lastKnownPos;

	// Token: 0x04000787 RID: 1927
	private Vector3 nextRandomPos;

	// Token: 0x04000788 RID: 1928
	public float checkCooldown;

	// Token: 0x04000789 RID: 1929
	public float blockCooldown;

	// Token: 0x0400078A RID: 1930
	public float preferredDistanceToTarget = 15f;

	// Token: 0x0400078B RID: 1931
	private BloodsplatterManager bsm;

	// Token: 0x0400078C RID: 1932
	public AssetReference explosion;

	// Token: 0x0400078D RID: 1933
	public AssetReference gib;

	// Token: 0x0400078E RID: 1934
	private StyleCalculator scalc;

	// Token: 0x0400078F RID: 1935
	private EnemyIdentifier eid;

	// Token: 0x04000790 RID: 1936
	private EnemyType type;

	// Token: 0x04000791 RID: 1937
	private AudioSource aud;

	// Token: 0x04000792 RID: 1938
	public AudioClip hurtSound;

	// Token: 0x04000793 RID: 1939
	public AudioClip deathSound;

	// Token: 0x04000794 RID: 1940
	public AudioClip windUpSound;

	// Token: 0x04000795 RID: 1941
	public AudioClip spotSound;

	// Token: 0x04000796 RID: 1942
	public AudioClip loseSound;

	// Token: 0x04000797 RID: 1943
	private float dodgeCooldown;

	// Token: 0x04000798 RID: 1944
	private float attackCooldown;

	// Token: 0x04000799 RID: 1945
	public AssetReference projectile;

	// Token: 0x0400079A RID: 1946
	private Material origMaterial;

	// Token: 0x0400079B RID: 1947
	public Material shootMaterial;

	// Token: 0x0400079C RID: 1948
	private EnemySimplifier[] ensims;

	// Token: 0x0400079D RID: 1949
	public ParticleSystem chargeParticle;

	// Token: 0x0400079E RID: 1950
	private bool killedByPlayer;

	// Token: 0x0400079F RID: 1951
	private bool parried;

	// Token: 0x040007A0 RID: 1952
	private bool exploded;

	// Token: 0x040007A1 RID: 1953
	private bool parryable;

	// Token: 0x040007A2 RID: 1954
	private Vector3 viewTarget;

	// Token: 0x040007A3 RID: 1955
	[HideInInspector]
	public bool musicRequested;

	// Token: 0x040007A4 RID: 1956
	private GoreZone gz;

	// Token: 0x040007A5 RID: 1957
	private int difficulty;

	// Token: 0x040007A6 RID: 1958
	private Animator anim;

	// Token: 0x040007A7 RID: 1959
	public bool enraged;

	// Token: 0x040007A8 RID: 1960
	public GameObject enrageEffect;

	// Token: 0x040007A9 RID: 1961
	private int usedAttacks;

	// Token: 0x040007AA RID: 1962
	[HideInInspector]
	public List<VirtueInsignia> childVi = new List<VirtueInsignia>();

	// Token: 0x040007AB RID: 1963
	private EnemyCooldowns vc;

	// Token: 0x040007AC RID: 1964
	private KeepInBounds kib;

	// Token: 0x040007AD RID: 1965
	private bool checkingForCrash;

	// Token: 0x040007AE RID: 1966
	private bool canHurtOtherDrones;

	// Token: 0x040007AF RID: 1967
	[HideInInspector]
	public bool lockRotation;

	// Token: 0x040007B0 RID: 1968
	[HideInInspector]
	public bool lockPosition;

	// Token: 0x040007B1 RID: 1969
	private bool hooked;

	// Token: 0x040007B2 RID: 1970
	private bool homeRunnable;

	// Token: 0x040007B3 RID: 1971
	public bool cantInstaExplode;

	// Token: 0x040007B4 RID: 1972
	private GameObject currentEnrageEffect;

	// Token: 0x040007B5 RID: 1973
	[HideInInspector]
	public bool fleshDrone;

	// Token: 0x040007B6 RID: 1974
	private int parryFramesLeft;

	// Token: 0x040007B7 RID: 1975
	public GameObject ghost;
}
