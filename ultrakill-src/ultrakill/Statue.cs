using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200044F RID: 1103
public class Statue : MonoBehaviour
{
	// Token: 0x06001905 RID: 6405 RVA: 0x000CB2B0 File Offset: 0x000C94B0
	private void Start()
	{
		if (!this.limp)
		{
			this.nma = base.GetComponent<NavMeshAgent>();
			this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
			this.rbs = base.GetComponentsInChildren<Rigidbody>();
			this.anim = base.GetComponentInChildren<Animator>();
			if (this.smr != null)
			{
				this.originalMaterial = this.smr.material;
			}
			this.mass = base.GetComponent<Mass>();
			this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
			if (this.gc == null)
			{
				this.affectedByGravity = false;
			}
			this.rb = base.GetComponent<Rigidbody>();
			this.eid = base.GetComponent<EnemyIdentifier>();
			if (!this.musicRequested)
			{
				this.musicRequested = true;
				MonoSingleton<MusicManager>.Instance.PlayBattleMusic();
			}
			if (this.originalHealth == 0f)
			{
				this.originalHealth = this.health;
			}
		}
		else
		{
			this.noheal = true;
		}
		if (this.gz == null)
		{
			this.gz = GoreZone.ResolveGoreZone((base.transform.parent == null) ? base.transform : base.transform.parent);
		}
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x000CB3D5 File Offset: 0x000C95D5
	private void OnDestroy()
	{
		if (this.massDying)
		{
			this.DeathEnd();
		}
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x000CB3E8 File Offset: 0x000C95E8
	private void Update()
	{
		if (this.massDying)
		{
			base.transform.position = new Vector3(this.origPos.x + Random.Range(-0.5f, 0.5f), this.origPos.y + Random.Range(-0.5f, 0.5f), this.origPos.z + Random.Range(-0.5f, 0.5f));
			if (Random.Range(0f, 1f) < Time.deltaTime * 5f)
			{
				int num = Random.Range(0, this.transforms.Count);
				if (this.transforms[num] != null)
				{
					GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, false);
					if (gore)
					{
						gore.transform.position = this.transforms[num].position;
						if (this.gz != null && this.gz.goreZone != null)
						{
							gore.transform.SetParent(this.gz.goreZone, true);
						}
						Bloodsplatter bloodsplatter;
						if (gore.TryGetComponent<Bloodsplatter>(out bloodsplatter))
						{
							bloodsplatter.GetReady();
							return;
						}
					}
				}
				else
				{
					this.transforms.RemoveAt(num);
				}
			}
		}
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x000CB538 File Offset: 0x000C9738
	private void FixedUpdate()
	{
		if (this.parryFramesLeft > 0)
		{
			this.parryFramesLeft--;
		}
		if (this.affectedByGravity && !this.limp)
		{
			if (this.knockedBack && this.knockBackCharge <= 0f && this.rb.velocity.magnitude < 1f && this.gc.onGround)
			{
				this.StopKnockBack();
			}
			else if (this.knockedBack)
			{
				if (this.knockBackCharge <= 0f)
				{
					this.brakes = Mathf.MoveTowards(this.brakes, 0f, 0.0005f * this.brakes);
				}
				if (this.rb.velocity.y > 0f)
				{
					this.rb.velocity = new Vector3(this.rb.velocity.x * 0.95f * this.brakes, (this.rb.velocity.y - this.juggleWeight) * this.brakes, this.rb.velocity.z * 0.95f * this.brakes);
				}
				else
				{
					this.rb.velocity = new Vector3(this.rb.velocity.x * 0.95f * this.brakes, this.rb.velocity.y - this.juggleWeight, this.rb.velocity.z * 0.95f * this.brakes);
				}
				this.juggleWeight += 0.00025f;
				this.nma.updatePosition = false;
				this.nma.updateRotation = false;
				this.nma.enabled = false;
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
			}
			else if (!this.grounded && this.gc.onGround)
			{
				this.grounded = true;
			}
			else if (this.grounded && !this.gc.onGround)
			{
				this.grounded = false;
			}
			if (!this.gc.onGround && !this.falling && !this.nma.isOnOffMeshLink)
			{
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
				this.nma.enabled = false;
				this.falling = true;
				this.anim.SetBool("Falling", true);
				return;
			}
			if (this.gc.onGround && this.falling)
			{
				if (this.fallSpeed <= -50f && !InvincibleEnemies.Enabled && !this.eid.blessed)
				{
					this.eid.Splatter(true);
					return;
				}
				this.fallSpeed = 0f;
				this.nma.updatePosition = true;
				this.nma.updateRotation = true;
				this.rb.isKinematic = true;
				this.rb.useGravity = false;
				this.nma.enabled = true;
				this.nma.Warp(base.transform.position);
				this.falling = false;
				this.anim.SetBool("Falling", false);
			}
		}
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x000CB880 File Offset: 0x000C9A80
	public void KnockBack(Vector3 force)
	{
		if (this.affectedByGravity && this.sb != null && !this.sb.inAction)
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

	// Token: 0x0600190A RID: 6410 RVA: 0x000CB9AC File Offset: 0x000C9BAC
	public void StopKnockBack()
	{
		if (this.nma != null)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
			{
				Vector3 zero = Vector3.zero;
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 4f, this.nma.areaMask))
				{
					this.knockedBack = false;
					this.nma.updatePosition = true;
					this.nma.updateRotation = true;
					this.nma.enabled = true;
					this.rb.isKinematic = true;
					this.juggleWeight = 0f;
					this.nma.Warp(navMeshHit.position);
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

	// Token: 0x0600190B RID: 6411 RVA: 0x000CBAA0 File Offset: 0x000C9CA0
	public void GetHurt(GameObject target, Vector3 force, float multiplier, float critMultiplier, Vector3 hurtPos, GameObject sourceWeapon = null, bool fromExplosion = false)
	{
		string text = "";
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		GameObject gameObject = null;
		float num = this.health;
		if (this.massDying)
		{
			return;
		}
		if (this.eid == null)
		{
			return;
		}
		float num2;
		if (target.gameObject.CompareTag("Head"))
		{
			num2 = 1f * multiplier + multiplier * critMultiplier;
			if (this.extraDamageZones.Count > 0 && this.extraDamageZones.Contains(target))
			{
				num2 *= this.extraDamageMultiplier;
				flag3 = true;
			}
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num2;
			}
			if (this.eid.hitter != "fire" && num2 > 0f)
			{
				if (num2 >= 1f || this.health <= 0f)
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
				}
			}
			if (!this.limp)
			{
				flag2 = true;
				text = "head";
			}
			if (this.health <= 0f && !this.limp)
			{
				this.GoLimp();
			}
		}
		else if (target.gameObject.CompareTag("Limb") || target.gameObject.CompareTag("EndLimb"))
		{
			num2 = 1f * multiplier + 0.5f * multiplier * critMultiplier;
			if (this.extraDamageZones.Count > 0 && this.extraDamageZones.Contains(target))
			{
				num2 *= this.extraDamageMultiplier;
				flag3 = true;
			}
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num2;
			}
			if (this.eid.hitter != "fire" && num2 > 0f)
			{
				if (this.eid.hitter == "hammer")
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else if ((num2 >= 1f && this.health > 0f) || (this.health <= 0f && this.eid.hitter != "explosion") || (this.eid.hitter == "explosion" && target.gameObject.CompareTag("EndLimb")))
				{
					gameObject = this.bsm.GetGore(GoreType.Limb, this.eid, fromExplosion);
				}
				else if (this.eid.hitter != "explosion")
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
				}
			}
			if (!this.limp)
			{
				flag2 = true;
				text = "limb";
			}
			if (this.health <= 0f && !this.limp)
			{
				this.GoLimp();
			}
		}
		else
		{
			num2 = 1f * multiplier;
			if (this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone")
			{
				if (!this.parryable && (!this.partiallyParryable || this.parryables == null || !this.parryables.Contains(target.transform)) && (target.gameObject != this.chest || this.health - num2 > 0f))
				{
					num2 = 0f;
				}
				else if ((this.parryable && (target.gameObject == this.chest || MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude > 18f)) || (this.partiallyParryable && this.parryables != null && this.parryables.Contains(target.transform)))
				{
					num2 *= 1.5f;
					this.parryable = false;
					this.partiallyParryable = false;
					this.parryables.Clear();
					MonoSingleton<NewMovement>.Instance.Parry(this.eid, "");
					base.SendMessage("GotParried", SendMessageOptions.DontRequireReceiver);
				}
			}
			if (this.extraDamageZones.Count > 0 && this.extraDamageZones.Contains(target))
			{
				num2 *= this.extraDamageMultiplier;
				flag3 = true;
			}
			if (!this.eid.blessed && !InvincibleEnemies.Enabled)
			{
				this.health -= num2;
			}
			if (this.eid.hitter != "fire" && num2 > 0f)
			{
				if (this.eid.hitter == "hammer")
				{
					gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
				}
				else if ((num2 >= 1f && this.health > 0f) || (this.health <= 0f && this.eid.hitter != "explosion") || (this.eid.hitter == "explosion" && target.gameObject.CompareTag("EndLimb")))
				{
					gameObject = this.bsm.GetGore(GoreType.Body, this.eid, fromExplosion);
				}
				else if (this.eid.hitter != "explosion")
				{
					gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
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
				if (target && target.GetComponentInParent<Rigidbody>() != null)
				{
					target.GetComponentInParent<Rigidbody>().AddForce(force);
				}
			}
		}
		if (this.mass != null)
		{
			if (this.mass.spearShot && this.mass.tempSpear && this.mass.tailHitboxes.Contains(target))
			{
				MassSpear component = this.mass.tempSpear.GetComponent<MassSpear>();
				if (component != null && component.hitPlayer)
				{
					if (num2 >= 1f || component.spearHealth - num2 <= 0f)
					{
						GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
						this.ReadyGib(gore, this.mass.tailEnd.GetChild(0).gameObject);
					}
					component.spearHealth -= num2;
				}
			}
			else if (this.mass.spearShot && !this.mass.tempSpear)
			{
				this.mass.spearShot = false;
			}
		}
		if (gameObject != null)
		{
			if (this.gz == null)
			{
				this.gz = GoreZone.ResolveGoreZone(base.transform);
			}
			if (hurtPos != Vector3.zero)
			{
				gameObject.transform.position = hurtPos;
			}
			else
			{
				gameObject.transform.position = target.transform.position;
			}
			if (this.eid.hitter == "drill")
			{
				gameObject.transform.localScale *= 2f;
			}
			if (this.bigBlood)
			{
				gameObject.transform.localScale *= 2f;
			}
			if (this.gz != null && this.gz.goreZone != null)
			{
				gameObject.transform.SetParent(this.gz.goreZone, true);
			}
			Bloodsplatter component2 = gameObject.GetComponent<Bloodsplatter>();
			if (component2)
			{
				ParticleSystem.CollisionModule collision = component2.GetComponent<ParticleSystem>().collision;
				if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone" || this.eid.hitter == "explosion")
				{
					if (Random.Range(0f, 1f) > 0.5f)
					{
						collision.enabled = false;
					}
					component2.hpAmount = 3;
				}
				else if (this.eid.hitter == "nail")
				{
					component2.hpAmount = 1;
					component2.GetComponent<AudioSource>().volume *= 0.8f;
				}
				if (!this.noheal)
				{
					component2.GetReady();
				}
			}
		}
		if (this.eid && this.eid.hitter == "punch")
		{
			bool flag4 = this.parryables != null && this.parryables.Count > 0 && this.parryables.Contains(target.transform);
			if (this.parryable || (this.partiallyParryable && (flag4 || (this.parryFramesLeft > 0 && this.parryFramesOnPartial))))
			{
				this.parryable = false;
				this.partiallyParryable = false;
				this.parryables.Clear();
				if (!InvincibleEnemies.Enabled && !this.eid.blessed)
				{
					num2 = 5f;
				}
				if (!this.eid.blessed && !InvincibleEnemies.Enabled)
				{
					this.health -= num2;
				}
				MonoSingleton<FistControl>.Instance.currentPunch.Parry(true, this.eid, "");
				base.SendMessage("GotParried", SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				this.parryFramesOnPartial = flag4;
				this.parryFramesLeft = MonoSingleton<FistControl>.Instance.currentPunch.activeFrames;
			}
		}
		if (flag3 && (num2 >= 1f || (this.eid.hitter == "shotgun" && Random.Range(0f, 1f) > 0.5f) || (this.eid.hitter == "nail" && Random.Range(0f, 1f) > 0.85f)))
		{
			if (this.extraDamageMultiplier >= 2f)
			{
				gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
			}
			else
			{
				gameObject = this.bsm.GetGore(GoreType.Limb, this.eid, fromExplosion);
			}
			if (gameObject)
			{
				gameObject.transform.position = target.transform.position;
				if (this.gz != null && this.gz.goreZone != null)
				{
					gameObject.transform.SetParent(this.gz.goreZone, true);
				}
				Bloodsplatter component3 = gameObject.GetComponent<Bloodsplatter>();
				if (component3)
				{
					ParticleSystem.CollisionModule collision2 = component3.GetComponent<ParticleSystem>().collision;
					if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone" || this.eid.hitter == "explosion")
					{
						if (Random.Range(0f, 1f) > 0.5f)
						{
							collision2.enabled = false;
						}
						component3.hpAmount = 3;
					}
					else if (this.eid.hitter == "nail")
					{
						component3.hpAmount = 1;
						component3.GetComponent<AudioSource>().volume *= 0.8f;
					}
					if (!this.noheal)
					{
						component3.GetReady();
					}
				}
			}
		}
		if (this.health > 0f && this.hurtSounds.Length != 0 && !this.eid.blessed)
		{
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.clip = this.hurtSounds[Random.Range(0, this.hurtSounds.Length)];
			this.aud.volume = 0.75f;
			this.aud.pitch = Random.Range(0.85f, 1.35f);
			this.aud.priority = 12;
			this.aud.Play();
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
			MinosArm component4 = base.GetComponent<MinosArm>();
			if (this.health <= 0f && !component4)
			{
				flag = true;
				if (this.gc && !this.gc.onGround && !this.eid.flying)
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
				this.scalc.HitCalculator(this.eid.hitter, "spider", text, flag, this.eid, sourceWeapon);
			}
		}
		if ((this.woundedMaterial || this.woundedModel) && num >= this.originalHealth / 2f && this.health < this.originalHealth / 2f)
		{
			if (this.woundedParticle)
			{
				Object.Instantiate<GameObject>(this.woundedParticle, this.chest.transform.position, Quaternion.identity);
			}
			if (!this.eid.puppet)
			{
				if (this.woundedModel)
				{
					this.woundedModel.SetActive(true);
					this.smr.gameObject.SetActive(false);
					return;
				}
				this.smr.material = this.woundedMaterial;
				EnemySimplifier enemySimplifier;
				if (this.smr.TryGetComponent<EnemySimplifier>(out enemySimplifier))
				{
					enemySimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.woundedMaterial);
					enemySimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.enraged, this.woundedEnrageMaterial);
				}
			}
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000CC9B4 File Offset: 0x000CABB4
	public void GoLimp()
	{
		if (this.limp)
		{
			return;
		}
		if (this.health > 0f)
		{
			this.health = 0f;
		}
		if (this.smr != null)
		{
			this.smr.updateWhenOffscreen = true;
		}
		this.gz = base.GetComponentInParent<GoreZone>();
		base.Invoke("StopHealing", 1f);
		StatueBoss component = base.GetComponent<StatueBoss>();
		SwingCheck2[] componentsInChildren = base.GetComponentsInChildren<SwingCheck2>();
		MinosArm component2 = base.GetComponent<MinosArm>();
		if (component2 != null)
		{
			component2.Retreat();
			this.limp = true;
			return;
		}
		if (component != null)
		{
			this.anim.StopPlayback();
			SwingCheck2[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			StatueBoss[] componentsInChildren2 = GoreZone.ResolveGoreZone(base.transform).GetComponentsInChildren<StatueBoss>();
			if (componentsInChildren2.Length != 0)
			{
				foreach (StatueBoss statueBoss in componentsInChildren2)
				{
					if (!(statueBoss == component))
					{
						statueBoss.EnrageDelayed();
					}
				}
			}
			component.ForceStopDashSound();
			if (component.currentEnrageEffect != null)
			{
				Object.Destroy(component.currentEnrageEffect);
			}
			Object.Destroy(component);
		}
		else if ((this.mass != null || this.massDeath) && !this.massDying)
		{
			if (this.mass != null)
			{
				this.mass.dead = true;
				this.mass.enabled = false;
				this.anim.speed = 0f;
				SwingCheck2[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					Object.Destroy(array[i]);
				}
			}
			this.origPos = base.transform.position;
			this.transforms.AddRange(base.GetComponentsInChildren<Transform>());
			this.massDying = true;
			base.Invoke("BloodExplosion", 3f);
			if (this.mass != null && this.mass.currentEnrageEffect != null)
			{
				Object.Destroy(this.mass.currentEnrageEffect);
			}
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
		EnemySimplifier[] componentsInChildren3 = base.GetComponentsInChildren<EnemySimplifier>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].Begone();
		}
		if (this.smr != null)
		{
			if (this.deadMaterial != null)
			{
				this.smr.sharedMaterial = this.deadMaterial;
			}
			else if (this.woundedMaterial != null)
			{
				this.smr.sharedMaterial = this.woundedMaterial;
			}
			else
			{
				this.smr.sharedMaterial = this.originalMaterial;
			}
		}
		if (this.specialDeath)
		{
			base.SendMessage("SpecialDeath", SendMessageOptions.DontRequireReceiver);
		}
		else if (!this.massDying)
		{
			Object.Destroy(this.nma);
			this.nma = null;
			Object.Destroy(this.anim);
			Object.Destroy(base.gameObject.GetComponent<Collider>());
			if (this.rb == null)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			Object.Destroy(this.rb);
			if (!this.limp && !this.eid.dontCountAsKills)
			{
				ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
				if (componentInParent != null)
				{
					componentInParent.AddDeadEnemy();
				}
			}
			if (!this.limp)
			{
				this.rbs = base.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody rigidbody in this.rbs)
				{
					if (rigidbody != null && rigidbody != this.rb)
					{
						rigidbody.isKinematic = false;
						rigidbody.useGravity = true;
						rigidbody.transform.SetParent(this.gz.transform);
						EnemyIdentifierIdentifier enemyIdentifierIdentifier;
						if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && rigidbody.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
						{
							enemyIdentifierIdentifier.SetupForHellBath();
						}
						rigidbody.AddForce(Random.onUnitSphere * 2.5f, ForceMode.VelocityChange);
					}
				}
			}
			if (this.musicRequested)
			{
				this.musicRequested = false;
				MonoSingleton<MusicManager>.Instance.PlayCleanMusic();
			}
		}
		if (this.deathSound != null)
		{
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.clip = this.deathSound;
			this.aud.volume = 1f;
			this.aud.pitch = Random.Range(0.85f, 1.35f);
			this.aud.priority = 11;
			this.aud.Play();
		}
		this.limp = true;
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x000CCEAF File Offset: 0x000CB0AF
	private void StopHealing()
	{
		this.noheal = true;
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x000CCEB8 File Offset: 0x000CB0B8
	private void BloodExplosion()
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform transform in this.transforms)
		{
			if (transform != null && Random.Range(0f, 1f) < 0.33f)
			{
				GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, false);
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
				this.transforms.Remove(transform2);
			}
			list.Clear();
		}
		if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && base.gameObject.activeInHierarchy)
		{
			for (int i = 0; i < 40; i++)
			{
				if (i < 30)
				{
					GameObject gameObject = this.bsm.GetGib(BSType.gib);
					if (gameObject)
					{
						if (this.gz && this.gz.gibZone)
						{
							this.ReadyGib(gameObject, this.transforms[Random.Range(0, this.transforms.Count)].gameObject);
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
					GameObject gameObject = this.bsm.GetGib(BSType.eyeball);
					if (gameObject)
					{
						if (this.gz && this.gz.gibZone)
						{
							this.ReadyGib(gameObject, this.transforms[Random.Range(0, this.transforms.Count)].gameObject);
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
					GameObject gameObject = this.bsm.GetGib(BSType.brainChunk);
					if (!gameObject)
					{
						break;
					}
					if (this.gz && this.gz.gibZone)
					{
						this.ReadyGib(gameObject, this.transforms[Random.Range(0, this.transforms.Count)].gameObject);
					}
					gameObject.transform.localScale *= Random.Range(3f, 4f);
				}
			}
		}
		this.massDying = false;
		this.DeathEnd();
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000CD214 File Offset: 0x000CB414
	private void DeathEnd()
	{
		if (!this.eid.dontCountAsKills)
		{
			ActivateNextWave componentInParent = base.GetComponentInParent<ActivateNextWave>();
			if (componentInParent != null)
			{
				componentInParent.AddDeadEnemy();
			}
		}
		if (this.musicRequested)
		{
			MonoSingleton<MusicManager>.Instance.PlayCleanMusic();
		}
		if (base.gameObject)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000CD270 File Offset: 0x000CB470
	private void ReadyGib(GameObject tempGib, GameObject target)
	{
		tempGib.transform.SetPositionAndRotation(target.transform.position, Random.rotation);
		if (!this.gz)
		{
			this.gz = base.GetComponentInParent<GoreZone>();
		}
		tempGib.transform.SetParent(this.gz.gibZone);
		if (!MonoSingleton<BloodsplatterManager>.Instance.goreOn)
		{
			tempGib.SetActive(false);
		}
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x000CD2DC File Offset: 0x000CB4DC
	public void ParryableCheck(bool partial = false)
	{
		if (partial)
		{
			this.partiallyParryable = true;
		}
		else
		{
			this.parryable = true;
		}
		if (this.parryFramesLeft > 0 && (!partial || this.parryFramesOnPartial))
		{
			this.eid.hitter = "punch";
			this.eid.DeliverDamage(base.gameObject, MonoSingleton<CameraController>.Instance.transform.forward * 25000f, base.transform.position, 1f, false, 0f, null, false, false);
			this.parryFramesLeft = 0;
		}
	}

	// Token: 0x040022FE RID: 8958
	public float health;

	// Token: 0x040022FF RID: 8959
	[HideInInspector]
	public float originalHealth;

	// Token: 0x04002300 RID: 8960
	private BloodsplatterManager bsm;

	// Token: 0x04002301 RID: 8961
	public bool limp;

	// Token: 0x04002302 RID: 8962
	private EnemyIdentifier eid;

	// Token: 0x04002303 RID: 8963
	public GameObject chest;

	// Token: 0x04002304 RID: 8964
	private float chestHP;

	// Token: 0x04002305 RID: 8965
	private AudioSource aud;

	// Token: 0x04002306 RID: 8966
	public AudioClip[] hurtSounds;

	// Token: 0x04002307 RID: 8967
	private StyleCalculator scalc;

	// Token: 0x04002308 RID: 8968
	private GoreZone gz;

	// Token: 0x04002309 RID: 8969
	public Material deadMaterial;

	// Token: 0x0400230A RID: 8970
	public Material woundedMaterial;

	// Token: 0x0400230B RID: 8971
	public Material woundedEnrageMaterial;

	// Token: 0x0400230C RID: 8972
	public GameObject woundedParticle;

	// Token: 0x0400230D RID: 8973
	public GameObject woundedModel;

	// Token: 0x0400230E RID: 8974
	private Material originalMaterial;

	// Token: 0x0400230F RID: 8975
	public SkinnedMeshRenderer smr;

	// Token: 0x04002310 RID: 8976
	private NavMeshAgent nma;

	// Token: 0x04002311 RID: 8977
	private Rigidbody rb;

	// Token: 0x04002312 RID: 8978
	private Rigidbody[] rbs;

	// Token: 0x04002313 RID: 8979
	private Animator anim;

	// Token: 0x04002314 RID: 8980
	public AudioClip deathSound;

	// Token: 0x04002315 RID: 8981
	private bool noheal;

	// Token: 0x04002316 RID: 8982
	public List<GameObject> extraDamageZones = new List<GameObject>();

	// Token: 0x04002317 RID: 8983
	public float extraDamageMultiplier;

	// Token: 0x04002318 RID: 8984
	private StatueBoss sb;

	// Token: 0x04002319 RID: 8985
	private Mass mass;

	// Token: 0x0400231A RID: 8986
	private Vector3 origPos;

	// Token: 0x0400231B RID: 8987
	private List<Transform> transforms = new List<Transform>();

	// Token: 0x0400231C RID: 8988
	public bool grounded;

	// Token: 0x0400231D RID: 8989
	private GroundCheckEnemy gc;

	// Token: 0x0400231E RID: 8990
	public bool knockedBack;

	// Token: 0x0400231F RID: 8991
	private float knockBackCharge;

	// Token: 0x04002320 RID: 8992
	public float brakes;

	// Token: 0x04002321 RID: 8993
	public float juggleWeight;

	// Token: 0x04002322 RID: 8994
	public bool falling;

	// Token: 0x04002323 RID: 8995
	private float fallSpeed;

	// Token: 0x04002324 RID: 8996
	private float fallTime;

	// Token: 0x04002325 RID: 8997
	private bool affectedByGravity = true;

	// Token: 0x04002326 RID: 8998
	[HideInInspector]
	public bool musicRequested;

	// Token: 0x04002327 RID: 8999
	public bool bigBlood;

	// Token: 0x04002328 RID: 9000
	public bool massDeath;

	// Token: 0x04002329 RID: 9001
	private bool massDying;

	// Token: 0x0400232A RID: 9002
	public bool specialDeath;

	// Token: 0x0400232B RID: 9003
	public bool parryable;

	// Token: 0x0400232C RID: 9004
	public bool partiallyParryable;

	// Token: 0x0400232D RID: 9005
	[HideInInspector]
	public List<Transform> parryables = new List<Transform>();

	// Token: 0x0400232E RID: 9006
	private int parryFramesLeft;

	// Token: 0x0400232F RID: 9007
	private bool parryFramesOnPartial;
}
