using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020004D3 RID: 1235
public class Zombie : MonoBehaviour
{
	// Token: 0x06001C3F RID: 7231 RVA: 0x000EA469 File Offset: 0x000E8669
	private void Awake()
	{
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.anim = base.GetComponent<Animator>();
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x000EA4A9 File Offset: 0x000E86A9
	private GoreZone GetGoreZone()
	{
		if (this.gz)
		{
			return this.gz;
		}
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		return this.gz;
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x000EA4D6 File Offset: 0x000E86D6
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x000EA4E0 File Offset: 0x000E86E0
	private void SetSpeed()
	{
		if (this.limp)
		{
			return;
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (!this.nma)
		{
			this.nma = base.GetComponent<NavMeshAgent>();
		}
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
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
		if (this.difficulty >= 4)
		{
			this.speedMultiplier = 1.5f;
		}
		else if (this.difficulty == 3)
		{
			this.speedMultiplier = 1.25f;
		}
		else if (this.difficulty == 2)
		{
			this.speedMultiplier = 1f;
		}
		else if (this.difficulty == 1)
		{
			this.speedMultiplier = 0.75f;
		}
		else if (this.difficulty == 0)
		{
			this.speedMultiplier = 0.5f;
		}
		if (this.zm)
		{
			if (this.difficulty >= 4)
			{
				this.nma.acceleration = 120f;
				this.nma.angularSpeed = 9000f;
				this.nma.speed = 20f;
			}
			else if (this.difficulty == 3)
			{
				this.nma.acceleration = 60f;
				this.nma.angularSpeed = 2600f;
				this.nma.speed = 20f;
			}
			else if (this.difficulty == 2)
			{
				this.nma.acceleration = 30f;
				this.nma.angularSpeed = 800f;
				this.nma.speed = 20f;
			}
			else if (this.difficulty == 1)
			{
				this.nma.acceleration = 30f;
				this.nma.angularSpeed = 400f;
				this.nma.speed = 15f;
			}
			else if (this.difficulty == 0)
			{
				this.nma.acceleration = 15f;
				this.nma.angularSpeed = 400f;
				this.nma.speed = 10f;
			}
		}
		else if (this.eid.enemyType == EnemyType.Soldier)
		{
			float num = 15f;
			if (this.difficulty == 4)
			{
				num = 17.5f;
			}
			else if (this.difficulty == 5)
			{
				num = 20f;
			}
			this.nma.speed = num * this.speedMultiplier;
			float num2 = 1f;
			if (this.difficulty == 5)
			{
				num2 = 1.75f;
			}
			this.anim.SetFloat("RunSpeed", num2 * this.speedMultiplier);
			this.nma.angularSpeed = 480f;
			this.nma.acceleration = 480f;
		}
		else
		{
			this.nma.speed = 10f * this.speedMultiplier;
			this.nma.angularSpeed = 800f;
			this.nma.acceleration = 30f;
		}
		this.nma.acceleration *= this.eid.totalSpeedModifier;
		this.nma.angularSpeed *= this.eid.totalSpeedModifier;
		this.nma.speed *= this.eid.totalSpeedModifier;
		if (this.nma)
		{
			this.defaultSpeed = this.nma.speed;
		}
		if (this.anim)
		{
			if (this.variableSpeed)
			{
				this.anim.speed = 1f * this.speedMultiplier;
				return;
			}
			if (this.difficulty >= 2)
			{
				this.anim.speed = 1f * this.eid.totalSpeedModifier;
				return;
			}
			if (this.difficulty == 1)
			{
				this.anim.speed = 0.875f * this.eid.totalSpeedModifier;
				return;
			}
			if (this.difficulty == 0)
			{
				this.anim.speed = 0.75f * this.eid.totalSpeedModifier;
			}
		}
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x000EA91C File Offset: 0x000E8B1C
	private void Start()
	{
		this.rbs = base.GetComponentsInChildren<Rigidbody>();
		this.rb = base.GetComponent<Rigidbody>();
		this.zm = base.GetComponent<ZombieMelee>();
		this.zp = base.GetComponent<ZombieProjectiles>();
		this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
		NavMeshHit navMeshHit;
		if (this.gc && this.gc.onGround && this.nma && NavMesh.SamplePosition(base.transform.position, out navMeshHit, 5f, this.nma.areaMask))
		{
			this.nma.enabled = true;
		}
		if (!this.smr)
		{
			this.smr = base.GetComponentInChildren<SkinnedMeshRenderer>();
		}
		if (this.smr)
		{
			this.originalMaterial = this.smr.sharedMaterial;
		}
		if (this.limp)
		{
			this.noheal = true;
		}
		this.SetSpeed();
		this.lmaskWater = LayerMaskDefaults.Get(LMD.Environment);
		this.lmaskWater |= 16;
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x000EAA2D File Offset: 0x000E8C2D
	private void OnEnable()
	{
		this.attacking = false;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x000EAA38 File Offset: 0x000E8C38
	private void Update()
	{
		if (this.knockBackCharge > 0f)
		{
			this.knockBackCharge = Mathf.MoveTowards(this.knockBackCharge, 0f, Time.deltaTime);
		}
		if (this.falling && !this.limp)
		{
			this.fallTime += Time.deltaTime;
			if (this.gc.onGround)
			{
				if (this.gc.fallSuppressed && !this.eid.unbounceable)
				{
					return;
				}
				if (this.fallSpeed <= -50f && !InvincibleEnemies.Enabled && !this.noFallDamage && !this.eid.blessed && !this.gc.fallSuppressed)
				{
					if (this.eid == null)
					{
						this.eid = base.GetComponent<EnemyIdentifier>();
					}
					this.eid.Splatter(true);
					return;
				}
				if (!Physics.CheckSphere(base.transform.position + Vector3.up * 1.5f, 0.1f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					this.fallSpeed = 0f;
					if (this.aud.clip == this.scream && this.aud.isPlaying)
					{
						this.aud.Stop();
					}
					this.rb.isKinematic = true;
					this.rb.useGravity = false;
					NavMeshHit navMeshHit;
					if (NavMesh.SamplePosition(base.transform.position, out navMeshHit, 4f, this.nma.areaMask))
					{
						this.nma.updatePosition = true;
						this.nma.updateRotation = true;
						this.nma.enabled = true;
						this.nma.Warp(navMeshHit.position);
					}
					this.falling = false;
					if (this.zm && this.zm.diving)
					{
						this.zm.JumpEnd();
					}
					this.anim.SetBool("Falling", false);
					return;
				}
			}
			else
			{
				if (this.eid.underwater && this.aud.clip == this.scream && this.aud.isPlaying)
				{
					this.aud.Stop();
					return;
				}
				if (this.fallTime > 0.05f && this.rb.velocity.y < this.fallSpeed)
				{
					this.fallSpeed = this.rb.velocity.y;
					this.reduceFallTime = 0.5f;
					RaycastHit raycastHit;
					if (!this.aud.isPlaying && !this.limp && !this.eid.underwater && (!Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, float.PositiveInfinity, this.lmaskWater, QueryTriggerInteraction.Collide) || ((raycastHit.distance > 32f || this.rb.velocity.y < -50f) && raycastHit.transform.gameObject.layer != 4)))
					{
						this.aud.clip = this.scream;
						this.aud.volume = 1f;
						this.aud.priority = 78;
						this.aud.pitch = Random.Range(0.8f, 1.2f);
						this.aud.Play();
						return;
					}
				}
				else if (this.fallTime > 0.05f && this.rb.velocity.y > this.fallSpeed)
				{
					this.reduceFallTime = Mathf.MoveTowards(this.reduceFallTime, 0f, Time.deltaTime);
					if (this.reduceFallTime <= 0f)
					{
						this.fallSpeed = this.rb.velocity.y;
						return;
					}
				}
				else if (this.rb.velocity.y > 0f)
				{
					this.fallSpeed = 0f;
					return;
				}
			}
		}
		else if (this.fallTime > 0f)
		{
			this.fallTime = 0f;
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000EAE54 File Offset: 0x000E9054
	private void FixedUpdate()
	{
		if (this.parryFramesLeft > 0)
		{
			this.parryFramesLeft--;
		}
		if (!this.limp)
		{
			if (this.knockedBack && this.knockBackCharge <= 0f && this.rb.velocity.magnitude < 1f && this.gc.onGround)
			{
				this.StopKnockBack();
			}
			else if (this.knockedBack)
			{
				if (this.eid.useBrakes || this.gc.onGround)
				{
					if (this.knockBackCharge <= 0f && this.gc.onGround)
					{
						this.brakes = Mathf.MoveTowards(this.brakes, 0f, 0.0005f * this.brakes);
					}
					this.rb.velocity = new Vector3(this.rb.velocity.x * 0.95f * this.brakes, this.rb.velocity.y - this.juggleWeight, this.rb.velocity.z * 0.95f * this.brakes);
				}
				else if (!this.eid.useBrakes)
				{
					this.brakes = 1f;
				}
				this.nma.updatePosition = false;
				this.nma.updateRotation = false;
				this.nma.enabled = false;
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
			}
			if (this.grounded && this.nma != null && this.nma.enabled && this.variableSpeed && this.nma.isOnNavMesh)
			{
				if (this.nma.isStopped || this.nma.velocity == Vector3.zero || this.stopped)
				{
					this.anim.SetFloat("RunSpeed", 1f);
				}
				else
				{
					this.anim.SetFloat("RunSpeed", this.nma.velocity.magnitude / this.nma.speed);
				}
			}
			else if (!this.grounded && this.gc.onGround)
			{
				this.grounded = true;
				this.nma.speed = this.defaultSpeed;
			}
			this.isOnOffNavmeshLink = this.nma.isOnOffMeshLink;
			if (!this.gc.onGround && !this.falling && !this.nma.isOnOffMeshLink)
			{
				this.grounded = false;
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
				this.nma.enabled = false;
				this.falling = true;
				this.anim.SetBool("Falling", true);
				this.anim.SetTrigger("StartFalling");
				if (this.zp != null)
				{
					this.zp.CancelAttack();
				}
				if (this.zm != null && !this.zm.diving)
				{
					this.zm.CancelAttack();
				}
			}
		}
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000EB198 File Offset: 0x000E9398
	public void KnockBack(Vector3 force)
	{
		if (this.rb)
		{
			this.nma.enabled = false;
			this.rb.isKinematic = false;
			this.rb.useGravity = true;
			if (!this.knockedBack || (!this.gc.onGround && this.rb.velocity.y < 0f))
			{
				this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
			}
			if (!this.gc.onGround)
			{
				this.rb.AddForce(Vector3.up, ForceMode.VelocityChange);
			}
			this.rb.AddForce(force / 10f, ForceMode.VelocityChange);
			this.knockedBack = true;
			this.knockBackCharge = Mathf.Min(this.knockBackCharge + force.magnitude / 1500f, 0.35f);
			this.brakes = 1f;
		}
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000EB2A8 File Offset: 0x000E94A8
	public void StopKnockBack()
	{
		if (this.nma != null)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Vector3 zero = Vector3.zero;
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 8f, this.nma.areaMask))
				{
					this.knockedBack = false;
					this.nma.updatePosition = true;
					this.nma.updateRotation = true;
					this.nma.enabled = true;
					this.rb.isKinematic = true;
					this.juggleWeight = 0f;
					this.eid.pulledByMagnet = false;
					this.nma.Warp(navMeshHit.position);
					return;
				}
				if (this.gc.onGround)
				{
					this.rb.isKinematic = true;
					this.knockedBack = false;
					this.juggleWeight = 0f;
					this.eid.pulledByMagnet = false;
					return;
				}
				this.knockBackCharge = 0.5f;
				return;
			}
			else
			{
				this.knockBackCharge = 0.5f;
			}
		}
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000EB3E0 File Offset: 0x000E95E0
	public void GetHurt(GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
	{
		string text = "";
		bool flag = false;
		bool flag2 = false;
		if (this.eid == null)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (this.gc && !this.gc.onGround && this.eid.hitter != "fire")
		{
			multiplier *= 1.5f;
		}
		if (force != Vector3.zero && !this.limp)
		{
			this.KnockBack(force / 100f);
			if (this.eid.hitter == "heavypunch" || (this.eid.hitter == "cannonball" && this.gc && !this.gc.onGround))
			{
				this.eid.useBrakes = false;
			}
			else
			{
				this.eid.useBrakes = true;
			}
		}
		if (this.chestExploding && this.health <= 0f && (target.gameObject.CompareTag("Limb") || target.gameObject.CompareTag("EndLimb")) && target.GetComponentInParent<EnemyIdentifier>() != null)
		{
			this.ChestExplodeEnd();
		}
		GameObject gameObject = null;
		if (this.bsm == null)
		{
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		}
		if (this.zm && this.zm.diving)
		{
			this.zm.CancelAttack();
		}
		if (this.eid.hitter == "punch")
		{
			if (this.attacking)
			{
				if (!InvincibleEnemies.Enabled && !this.eid.blessed)
				{
					this.health -= (float)((this.parryFramesLeft > 0) ? 4 : 5);
				}
				this.attacking = false;
				MonoSingleton<FistControl>.Instance.currentPunch.Parry(false, this.eid, "");
			}
			else
			{
				this.parryFramesLeft = MonoSingleton<FistControl>.Instance.currentPunch.activeFrames;
			}
		}
		if (target.gameObject.CompareTag("Head"))
		{
			float num = 1f * multiplier + multiplier * critMultiplier;
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num;
			}
			if (this.eid.hitter != "fire" && num > 0f)
			{
				if (num >= 1f || this.health <= 0f)
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
				}
			}
			Vector3 normalized = (target.transform.position - base.transform.position).normalized;
			if (!this.limp)
			{
				flag2 = true;
				text = "head";
			}
			if (this.health <= 0f)
			{
				if (!this.limp)
				{
					this.GoLimp();
				}
				if (this.eid.hitter != "fire" && this.eid.hitter != "sawblade")
				{
					float num2 = 1f;
					if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone")
					{
						num2 = 0.5f;
					}
					else if (this.eid.hitter == "Explosion")
					{
						num2 = 0.25f;
					}
					if (target.transform.parent != null && target.transform.parent.GetComponentInParent<Rigidbody>() != null)
					{
						target.transform.parent.GetComponentInParent<Rigidbody>().AddForce(force * 10f);
					}
					if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && this.eid.hitter != "harpoon")
					{
						this.GetGoreZone();
						int num3 = 0;
						while ((float)num3 < 6f * num2)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.skullChunk);
							this.ReadyGib(gameObject2, target);
							num3++;
						}
						int num4 = 0;
						while ((float)num4 < 4f * num2)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.brainChunk);
							this.ReadyGib(gameObject2, target);
							num4++;
						}
						int num5 = 0;
						while ((float)num5 < 2f * num2)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.eyeball);
							this.ReadyGib(gameObject2, target);
							gameObject2 = this.bsm.GetGib(BSType.jawChunk);
							this.ReadyGib(gameObject2, target);
							num5++;
						}
					}
				}
			}
		}
		else if (target.gameObject.CompareTag("Limb") || target.gameObject.CompareTag("EndLimb"))
		{
			if (this.eid == null)
			{
				this.eid = base.GetComponent<EnemyIdentifier>();
			}
			float num = 1f * multiplier + 0.5f * multiplier * critMultiplier;
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num;
			}
			if (this.eid.hitter != "fire" && num > 0f)
			{
				if (this.eid.hitter == "hammer")
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else if (((num >= 1f || this.health <= 0f) && this.eid.hitter != "explosion") || (this.eid.hitter == "explosion" && target.gameObject.CompareTag("EndLimb")))
				{
					gameObject = this.bsm.GetGore(GoreType.Limb, this.eid, fromExplosion);
				}
				else if (this.eid.hitter != "explosion")
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
				}
			}
			Vector3 normalized2 = (target.transform.position - base.transform.position).normalized;
			if (!this.limp)
			{
				flag2 = true;
				text = "limb";
			}
			if (this.health <= 0f)
			{
				if (!this.limp)
				{
					this.GoLimp();
				}
				if (this.eid.hitter == "sawblade")
				{
					if (!this.chestExploded && target.transform.position.y > this.chest.transform.position.y - 1f)
					{
						this.ChestExplosion(true, false);
					}
				}
				else if (this.eid.hitter != "fire" && this.eid.hitter != "harpoon")
				{
					if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && this.eid.hitter != "explosion" && target.gameObject.CompareTag("Limb"))
					{
						float num6 = 1f;
						this.GetGoreZone();
						if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone")
						{
							num6 = 0.5f;
						}
						int num7 = 0;
						while ((float)num7 < 4f * num6)
						{
							GameObject gib = this.bsm.GetGib(BSType.gib);
							this.ReadyGib(gib, target);
							num7++;
						}
					}
					else
					{
						target.transform.localScale = Vector3.zero;
						target.SetActive(false);
					}
				}
			}
		}
		else
		{
			float num = multiplier;
			if (this.eid == null)
			{
				this.eid = base.GetComponent<EnemyIdentifier>();
			}
			if (this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone")
			{
				if (!this.attacking && (target.gameObject != this.chest || this.health - num > 0f))
				{
					num = 0f;
				}
				else if (this.attacking && (target.gameObject == this.chest || this.eid.target.GetVelocity().magnitude > 18f))
				{
					if (!InvincibleEnemies.Enabled && !this.eid.blessed)
					{
						num *= 2f;
					}
					MonoSingleton<NewMovement>.Instance.Parry(this.eid, "");
				}
			}
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num;
			}
			if (this.eid.hitter != "fire" && num > 0f)
			{
				if (this.eid.hitter == "hammer")
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else if (num >= 1f || this.health <= 0f)
				{
					gameObject = this.bsm.GetGore(GoreType.Body, this.eid, fromExplosion);
				}
				else
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
				}
			}
			if (this.health <= 0f && target.gameObject == this.chest && this.eid.hitter != "fire")
			{
				if (this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone" || this.eid.hitter == "sawblade")
				{
					this.chestHP = 0f;
				}
				else
				{
					this.chestHP -= num;
				}
				if (this.chestHP <= 0f && this.eid.hitter != "harpoon")
				{
					this.ChestExplosion(this.eid.hitter == "sawblade", fromExplosion);
				}
			}
			if (!this.limp)
			{
				flag2 = true;
				text = "body";
			}
			if (this.health <= 0f)
			{
				if (!this.limp)
				{
					this.GoLimp();
				}
				if (this.eid.hitter != "sawblade" && target.GetComponentInParent<Rigidbody>() != null)
				{
					target.GetComponentInParent<Rigidbody>().AddForce(force * 10f);
				}
			}
		}
		if (gameObject != null)
		{
			this.GetGoreZone();
			gameObject.transform.position = target.transform.position;
			if (this.eid.hitter == "drill")
			{
				gameObject.transform.localScale *= 2f;
			}
			if (this.gz != null && this.gz.goreZone != null)
			{
				gameObject.transform.SetParent(this.gz.goreZone, true);
			}
			Bloodsplatter component = gameObject.GetComponent<Bloodsplatter>();
			if (component)
			{
				ParticleSystem.CollisionModule collision = component.GetComponent<ParticleSystem>().collision;
				if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone" || this.eid.hitter == "explosion")
				{
					if (Random.Range(0f, 1f) > 0.5f)
					{
						collision.enabled = false;
					}
					component.hpAmount = 3;
				}
				else if (this.eid.hitter == "nail")
				{
					component.hpAmount = 1;
					component.GetComponent<AudioSource>().volume *= 0.8f;
				}
				if (!this.noheal)
				{
					component.GetReady();
				}
			}
		}
		if (this.health <= 0f)
		{
			if (this.eid.hitter == "sawblade")
			{
				this.Cut(target);
			}
			else if (this.eid.hitter != "harpoon" && this.eid.hitter != "fire")
			{
				if (target.gameObject.CompareTag("Limb"))
				{
					if (target.transform.childCount > 0)
					{
						Transform child = target.transform.GetChild(0);
						CharacterJoint[] componentsInChildren = target.GetComponentsInChildren<CharacterJoint>();
						this.GetGoreZone();
						if (componentsInChildren.Length != 0)
						{
							foreach (CharacterJoint characterJoint in componentsInChildren)
							{
								EnemyIdentifierIdentifier enemyIdentifierIdentifier;
								if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
								{
									enemyIdentifierIdentifier.SetupForHellBath();
								}
								characterJoint.transform.SetParent(this.gz.transform);
								Object.Destroy(characterJoint);
							}
						}
						CharacterJoint component2 = target.GetComponent<CharacterJoint>();
						if (component2 != null)
						{
							component2.connectedBody = null;
							Object.Destroy(component2);
						}
						target.transform.position = child.position;
						target.transform.SetParent(child);
						child.SetParent(this.gz.transform, true);
						Object.Destroy(target.GetComponent<Rigidbody>());
					}
					Object.Destroy(target.GetComponent<Collider>());
					target.transform.localScale = Vector3.zero;
					target.gameObject.SetActive(false);
				}
				else if (target.gameObject.CompareTag("EndLimb") || target.gameObject.CompareTag("Head"))
				{
					target.transform.localScale = Vector3.zero;
					target.gameObject.SetActive(false);
				}
			}
		}
		if (this.health > 0f && !this.limp && this.hurtSounds.Length != 0 && !this.eid.blessed && this.eid.hitter != "blocked")
		{
			this.aud.clip = this.hurtSounds[Random.Range(0, this.hurtSounds.Length)];
			this.aud.volume = this.hurtSoundVol;
			this.aud.pitch = Random.Range(0.85f, 1.35f);
			this.aud.priority = 12;
			this.aud.Play();
		}
		if (this.eid == null)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (multiplier == 0f || this.eid.puppet)
		{
			flag2 = false;
		}
		if (flag2 && this.eid.hitter != "enemy")
		{
			if (this.scalc == null)
			{
				this.scalc = MonoSingleton<StyleCalculator>.Instance;
			}
			if (this.health <= 0f)
			{
				flag = true;
				if (this.gc && !this.gc.onGround)
				{
					if (this.eid.hitter == "explosion" || this.eid.hitter == "ffexplosion" || this.eid.hitter == "railcannon")
					{
						this.scalc.shud.AddPoints(120, "ultrakill.fireworks", sourceWeapon, this.eid, -1, "", "");
					}
					else if (this.eid.hitter == "ground slam")
					{
						this.scalc.shud.AddPoints(160, "ultrakill.airslam", sourceWeapon, this.eid, -1, "", "");
					}
					else if (this.eid.hitter != "deathzone")
					{
						this.scalc.shud.AddPoints(50, "ultrakill.airshot", sourceWeapon, this.eid, -1, "", "");
					}
				}
			}
			if (this.eid.hitter != "secret" && this.scalc)
			{
				this.scalc.HitCalculator(this.eid.hitter, "zombie", text, flag, this.eid, sourceWeapon);
			}
			if (flag && this.eid.hitter != "fire")
			{
				Flammable componentInChildren = base.GetComponentInChildren<Flammable>();
				if (componentInChildren && componentInChildren.burning && this.scalc)
				{
					this.scalc.shud.AddPoints(50, "ultrakill.finishedoff", sourceWeapon, this.eid, -1, "", "");
				}
			}
		}
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000EC4D4 File Offset: 0x000EA6D4
	public void GoLimp()
	{
		if (this.limp)
		{
			return;
		}
		if (this.smr != null)
		{
			(this.smr as SkinnedMeshRenderer).updateWhenOffscreen = true;
		}
		this.gz = this.GetGoreZone();
		this.attacking = false;
		base.Invoke("StopHealing", 1f);
		this.health = 0f;
		if (this.eid == null)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
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
		EnemySimplifier[] componentsInChildren = base.GetComponentsInChildren<EnemySimplifier>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Begone();
		}
		if (this.deadMaterial != null)
		{
			if (this.smr)
			{
				this.smr.sharedMaterial = this.deadMaterial;
			}
			else if (this.smr)
			{
				this.smr.sharedMaterial = this.originalMaterial;
			}
		}
		if (this.zm != null)
		{
			this.zm.track = false;
			if (!this.chestExploding)
			{
				this.anim.StopPlayback();
			}
			if (this.zm.biteTrail != null)
			{
				this.zm.biteTrail.enabled = false;
			}
			if (this.zm.diveTrail != null)
			{
				this.zm.diveTrail.enabled = false;
			}
			Object.Destroy(this.zm.swingCheck.gameObject);
			Object.Destroy(this.zm.diveSwingCheck.gameObject);
			Object.Destroy(this.zm);
		}
		if (this.zp != null)
		{
			this.zp.DamageEnd();
			if (!this.chestExploding)
			{
				this.anim.StopPlayback();
			}
			if (this.zp.hasMelee)
			{
				this.zp.MeleeDamageEnd();
			}
			Object.Destroy(this.zp);
			Projectile componentInChildren = base.GetComponentInChildren<Projectile>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren.gameObject);
			}
		}
		if (this.nma != null)
		{
			Object.Destroy(this.nma);
		}
		if (!this.chestExploding)
		{
			Object.Destroy(this.anim);
		}
		Object.Destroy(base.gameObject.GetComponent<Collider>());
		if (this.rb == null)
		{
			this.rb = base.GetComponent<Rigidbody>();
		}
		Object.Destroy(this.rb);
		if (this.deathSound != null)
		{
			this.aud.clip = this.deathSound;
			if (this.eid.hitter != "fire")
			{
				this.aud.volume = this.deathSoundVol;
			}
			else
			{
				this.aud.volume = 0.5f;
			}
			this.aud.pitch = Random.Range(0.85f, 1.35f);
			this.aud.priority = 11;
			this.aud.Play();
		}
		if (!this.limp && !this.chestExploding)
		{
			this.rbs = base.GetComponentsInChildren<Rigidbody>();
			foreach (Rigidbody rigidbody in this.rbs)
			{
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && rigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
				{
					enemyIdentifierIdentifier.SetupForHellBath();
				}
				if (MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Count > 0)
				{
					CheckForScroller checkForScroller = rigidbody.gameObject.AddComponent<CheckForScroller>();
					checkForScroller.checkOnStart = false;
					checkForScroller.checkOnCollision = true;
					checkForScroller.asRigidbody = true;
				}
			}
		}
		if (!this.limp)
		{
			if (!this.eid.dontCountAsKills)
			{
				ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
				if (componentInParent != null)
				{
					componentInParent.AddDeadEnemy();
				}
			}
			if (this.gz != null && this.gz.transform != null)
			{
				base.transform.SetParent(this.gz.transform, true);
			}
		}
		if (this.musicRequested)
		{
			this.musicRequested = false;
			MonoSingleton<MusicManager>.Instance.PlayCleanMusic();
		}
		this.limp = true;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000EC958 File Offset: 0x000EAB58
	public void ChestExplodeEnd()
	{
		this.anim.enabled = false;
		this.anim.StopPlayback();
		Object.Destroy(this.anim);
		this.rbs = base.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in this.rbs)
		{
			if (rigidbody != null)
			{
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && rigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
				{
					enemyIdentifierIdentifier.SetupForHellBath();
				}
			}
		}
		this.chestExploding = false;
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000EC9E6 File Offset: 0x000EABE6
	public void StopHealing()
	{
		this.noheal = true;
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x000EC9EF File Offset: 0x000EABEF
	private void ReadyGib(GameObject tempGib, GameObject target)
	{
		tempGib.transform.SetPositionAndRotation(target.transform.position, Random.rotation);
		this.gz.SetGoreZone(tempGib);
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000ECA18 File Offset: 0x000EAC18
	public void ChestExplosion(bool cut = false, bool fromExplosion = false)
	{
		if (this.chestExploded)
		{
			return;
		}
		this.GetGoreZone();
		if (!cut)
		{
			CharacterJoint[] componentsInChildren = this.chest.GetComponentsInChildren<CharacterJoint>();
			if (componentsInChildren.Length != 0)
			{
				foreach (CharacterJoint characterJoint in componentsInChildren)
				{
					if (characterJoint.transform.parent.parent == this.chest.transform)
					{
						foreach (Rigidbody rigidbody in characterJoint.transform.GetComponentsInChildren<Rigidbody>())
						{
							rigidbody.isKinematic = false;
							rigidbody.useGravity = true;
						}
						EnemyIdentifierIdentifier enemyIdentifierIdentifier;
						if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
						{
							enemyIdentifierIdentifier.SetupForHellBath();
						}
						Object.Destroy(characterJoint);
					}
					else if (characterJoint.transform == this.chest.transform)
					{
						Collider collider;
						if (characterJoint.TryGetComponent<Collider>(out collider))
						{
							Object.Destroy(collider);
						}
						Object.Destroy(characterJoint);
					}
				}
			}
			Rigidbody rigidbody2;
			if (this.chest.TryGetComponent<Rigidbody>(out rigidbody2))
			{
				Object.Destroy(rigidbody2);
			}
			if (!this.limp && !this.eid.exploded && !this.eid.dead)
			{
				if (this.gc.onGround)
				{
					this.rb.isKinematic = true;
					this.knockedBack = false;
				}
				this.anim.Rebind();
				this.anim.SetTrigger("ChestExplosion");
				this.chestExploding = true;
			}
		}
		this.GetGoreZone();
		if (MonoSingleton<BloodsplatterManager>.Instance.forceOn || MonoSingleton<BloodsplatterManager>.Instance.forceGibs || MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false))
		{
			this.GetGoreZone();
			for (int k = 0; k < 6; k++)
			{
				GameObject gib = this.bsm.GetGib((k < 2) ? BSType.jawChunk : BSType.gib);
				this.ReadyGib(gib, this.chest);
			}
			if (!this.eid.sandified)
			{
				GameObject fromQueue = this.bsm.GetFromQueue(BSType.chestExplosion);
				this.gz.SetGoreZone(fromQueue);
				fromQueue.transform.SetPositionAndRotation(this.chest.transform.parent.position, this.chest.transform.parent.rotation);
				fromQueue.transform.SetParent(this.chest.transform.parent, true);
			}
		}
		EnemyIdentifierIdentifier[] componentsInChildren3 = this.chest.GetComponentsInChildren<EnemyIdentifierIdentifier>();
		for (int l = 0; l < componentsInChildren3.Length; l++)
		{
			if (componentsInChildren3[l])
			{
				string tag = componentsInChildren3[l].gameObject.tag;
				GoreType goreType;
				if (!(tag == "Head"))
				{
					if (!(tag == "EndLimb") && !(tag == "Limb"))
					{
						goreType = GoreType.Body;
					}
					else
					{
						goreType = GoreType.Limb;
					}
				}
				else
				{
					goreType = GoreType.Head;
				}
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(goreType, this.eid, fromExplosion);
				if (gore)
				{
					gore.transform.position = this.chest.transform.position;
					Bloodsplatter component = gore.GetComponent<Bloodsplatter>();
					if (component)
					{
						component.hpAmount = 10;
					}
					if (this.gz != null && this.gz.goreZone != null)
					{
						gore.transform.SetParent(this.gz.goreZone, true);
					}
					if (!this.noheal && component)
					{
						component.GetReady();
					}
				}
			}
		}
		if (!cut)
		{
			this.chest.transform.localScale = Vector3.zero;
		}
		else
		{
			if (!this.limp)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(50, "ultrakill.halfoff", null, this.eid, -1, "", "");
			}
			this.Cut(this.chest);
		}
		this.chestExploded = true;
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000ECDFC File Offset: 0x000EAFFC
	public void Cut(GameObject target)
	{
		CharacterJoint characterJoint;
		if (target.TryGetComponent<CharacterJoint>(out characterJoint))
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				enemyIdentifierIdentifier.SetupForHellBath();
			}
			Object.Destroy(characterJoint);
			target.transform.SetParent(this.gz.transform, true);
			foreach (Rigidbody rigidbody in target.transform.GetComponentsInChildren<Rigidbody>())
			{
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
				rigidbody.angularDrag = 0.001f;
				rigidbody.maxAngularVelocity = float.PositiveInfinity;
				rigidbody.velocity = Vector3.zero;
				rigidbody.AddForce(Vector3.up * (float)(target.CompareTag("Head") ? 250 : 25), ForceMode.VelocityChange);
				rigidbody.AddTorque(target.transform.right * 1f, ForceMode.VelocityChange);
			}
			return;
		}
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x000ECEE0 File Offset: 0x000EB0E0
	public void ParryableCheck()
	{
		this.attacking = true;
		if (this.parryFramesLeft > 0)
		{
			this.eid.hitter = "punch";
			this.eid.DeliverDamage(base.gameObject, MonoSingleton<CameraController>.Instance.transform.forward * 25000f, base.transform.position, 1f, false, 0f, null, false, false);
			this.parryFramesLeft = 0;
		}
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x000ECF58 File Offset: 0x000EB158
	public void Jump(Vector3 vector)
	{
		this.gc.ForceOff();
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.eid.useBrakes = false;
		this.rb.AddForce(vector, ForceMode.VelocityChange);
		base.Invoke("Jumped", 0.5f);
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x000ECFB1 File Offset: 0x000EB1B1
	public void Jumped()
	{
		this.gc.StopForceOff();
	}

	// Token: 0x040027D2 RID: 10194
	public float health;

	// Token: 0x040027D3 RID: 10195
	private int difficulty = -1;

	// Token: 0x040027D4 RID: 10196
	private Rigidbody[] rbs;

	// Token: 0x040027D5 RID: 10197
	public bool limp;

	// Token: 0x040027D6 RID: 10198
	public NavMeshAgent nma;

	// Token: 0x040027D7 RID: 10199
	public Animator anim;

	// Token: 0x040027D8 RID: 10200
	private float currentSpeed;

	// Token: 0x040027D9 RID: 10201
	private Rigidbody rb;

	// Token: 0x040027DA RID: 10202
	private ZombieMelee zm;

	// Token: 0x040027DB RID: 10203
	[HideInInspector]
	public ZombieProjectiles zp;

	// Token: 0x040027DC RID: 10204
	private AudioSource aud;

	// Token: 0x040027DD RID: 10205
	public AudioClip[] hurtSounds;

	// Token: 0x040027DE RID: 10206
	public float hurtSoundVol;

	// Token: 0x040027DF RID: 10207
	public AudioClip deathSound;

	// Token: 0x040027E0 RID: 10208
	public float deathSoundVol;

	// Token: 0x040027E1 RID: 10209
	public AudioClip scream;

	// Token: 0x040027E2 RID: 10210
	private GroundCheckEnemy gc;

	// Token: 0x040027E3 RID: 10211
	public bool grounded;

	// Token: 0x040027E4 RID: 10212
	private float defaultSpeed;

	// Token: 0x040027E5 RID: 10213
	private StyleCalculator scalc;

	// Token: 0x040027E6 RID: 10214
	private EnemyIdentifier eid;

	// Token: 0x040027E7 RID: 10215
	private GoreZone gz;

	// Token: 0x040027E8 RID: 10216
	public Material deadMaterial;

	// Token: 0x040027E9 RID: 10217
	public Renderer smr;

	// Token: 0x040027EA RID: 10218
	private Material originalMaterial;

	// Token: 0x040027EB RID: 10219
	public GameObject chest;

	// Token: 0x040027EC RID: 10220
	private float chestHP = 3f;

	// Token: 0x040027ED RID: 10221
	public bool chestExploding;

	// Token: 0x040027EE RID: 10222
	public bool attacking;

	// Token: 0x040027EF RID: 10223
	private LayerMask lmaskWater;

	// Token: 0x040027F0 RID: 10224
	private bool noheal;

	// Token: 0x040027F1 RID: 10225
	private float speedMultiplier = 1f;

	// Token: 0x040027F2 RID: 10226
	public bool stopped;

	// Token: 0x040027F3 RID: 10227
	public bool knockedBack;

	// Token: 0x040027F4 RID: 10228
	private float knockBackCharge;

	// Token: 0x040027F5 RID: 10229
	public float brakes;

	// Token: 0x040027F6 RID: 10230
	public float juggleWeight;

	// Token: 0x040027F7 RID: 10231
	public bool falling;

	// Token: 0x040027F8 RID: 10232
	public bool noFallDamage;

	// Token: 0x040027F9 RID: 10233
	public bool musicRequested;

	// Token: 0x040027FA RID: 10234
	private float fallSpeed;

	// Token: 0x040027FB RID: 10235
	private float fallTime;

	// Token: 0x040027FC RID: 10236
	private float reduceFallTime;

	// Token: 0x040027FD RID: 10237
	private BloodsplatterManager bsm;

	// Token: 0x040027FE RID: 10238
	public bool variableSpeed;

	// Token: 0x040027FF RID: 10239
	private bool chestExploded;

	// Token: 0x04002800 RID: 10240
	private int parryFramesLeft;

	// Token: 0x04002801 RID: 10241
	public bool isOnOffNavmeshLink;
}
