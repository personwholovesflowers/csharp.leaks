using System;
using System.Collections.Generic;
using SettingsMenu.Components.Pages;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// Token: 0x020002DB RID: 731
public class Machine : MonoBehaviour
{
	// Token: 0x06000FD5 RID: 4053 RVA: 0x00075D0C File Offset: 0x00073F0C
	private void Awake()
	{
		this.nma = base.GetComponent<NavMeshAgent>();
		this.bsm = MonoSingleton<BloodsplatterManager>.Instance;
		this.rbs = base.GetComponentsInChildren<Rigidbody>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x00075D6C File Offset: 0x00073F6C
	private void Start()
	{
		if (this.smr != null)
		{
			this.originalMaterial = this.smr.material;
		}
		EnemyType enemyType = this.eid.enemyType;
		if (enemyType <= EnemyType.Sisyphus)
		{
			switch (enemyType)
			{
			case EnemyType.Mindflayer:
				this.mf = base.GetComponent<Mindflayer>();
				break;
			case EnemyType.Streetcleaner:
				this.sc = base.GetComponent<Streetcleaner>();
				break;
			case EnemyType.Swordsmachine:
				this.sm = base.GetComponent<SwordsMachine>();
				break;
			case EnemyType.V2:
				this.v2 = base.GetComponent<V2>();
				break;
			default:
				if (enemyType == EnemyType.Sisyphus)
				{
					this.sisy = base.GetComponent<Sisyphus>();
				}
				break;
			}
		}
		else if (enemyType != EnemyType.Turret)
		{
			if (enemyType != EnemyType.Ferryman)
			{
				switch (enemyType)
				{
				case EnemyType.Mannequin:
					this.man = base.GetComponent<Mannequin>();
					break;
				case EnemyType.Minotaur:
					this.min = base.GetComponent<Minotaur>();
					break;
				case EnemyType.Gutterman:
					this.gm = base.GetComponent<Gutterman>();
					break;
				}
			}
			else
			{
				this.fm = base.GetComponent<Ferryman>();
			}
		}
		else
		{
			this.tur = base.GetComponent<Turret>();
		}
		if (this.symbiote != null)
		{
			this.symbiotic = true;
		}
		if (!this.gz)
		{
			this.gz = GoreZone.ResolveGoreZone(base.transform);
		}
		if (this.hitJiggleRoot)
		{
			this.jiggleRootPosition = this.hitJiggleRoot.localPosition;
		}
		if (!this.musicRequested && !this.eid.dead && (this.sm == null || !this.eid.IgnorePlayer))
		{
			this.musicRequested = true;
			MonoSingleton<MusicManager>.Instance.PlayBattleMusic();
		}
		if (this.limp && !this.mf)
		{
			this.noheal = true;
		}
		this.lmask = LayerMaskDefaults.Get(LMD.Environment);
		this.lmaskWater = this.lmask;
		this.lmaskWater |= 16;
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x00075F58 File Offset: 0x00074158
	private void OnEnable()
	{
		this.parryable = false;
		this.partiallyParryable = false;
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x00075F68 File Offset: 0x00074168
	private void Update()
	{
		if (this.knockBackCharge > 0f)
		{
			this.knockBackCharge = Mathf.MoveTowards(this.knockBackCharge, 0f, Time.deltaTime);
		}
		if (this.healing && !this.limp && this.symbiote)
		{
			this.health = Mathf.MoveTowards(this.health, this.symbiote.health, Time.deltaTime * 10f);
			this.eid.health = this.health;
			if (this.health >= this.symbiote.health)
			{
				this.healing = false;
				if (this.sm)
				{
					this.sm.downed = false;
				}
				if (this.sisy)
				{
					this.sisy.downed = false;
				}
			}
		}
		if (this.falling && this.rb != null && !this.overrideFalling && (!this.nma || !this.nma.isOnOffMeshLink))
		{
			this.fallTime += Time.deltaTime;
			if (this.man)
			{
				this.noFallDamage = this.man.inControl;
				if (this.fallTime > 0.2f && !this.man.inControl)
				{
					this.parryable = true;
				}
			}
			if (this.gc.onGround && this.falling && this.nma != null)
			{
				if (this.fallSpeed <= -60f && !this.noFallDamage && !InvincibleEnemies.Enabled && !this.eid.blessed && (!this.gc.fallSuppressed || this.eid.unbounceable))
				{
					if (this.eid == null)
					{
						this.eid = base.GetComponent<EnemyIdentifier>();
					}
					this.eid.Splatter(true);
					return;
				}
				this.fallSpeed = 0f;
				this.nma.updatePosition = true;
				this.nma.updateRotation = true;
				if (!this.sm || !this.sm.moveAtTarget)
				{
					this.rb.isKinematic = true;
				}
				if (this.aud == null)
				{
					this.aud = base.GetComponent<AudioSource>();
				}
				if (this.aud && this.aud.clip == this.scream && this.aud.isPlaying)
				{
					this.aud.Stop();
				}
				this.rb.useGravity = false;
				this.nma.enabled = true;
				this.nma.Warp(base.transform.position);
				this.falling = false;
				this.anim.SetBool("Falling", false);
				if (this.man)
				{
					if (this.fallTime > 0.2f)
					{
						this.man.Landing();
					}
					else
					{
						this.man.inControl = true;
					}
					this.man.ResetMovementTarget();
					return;
				}
			}
			else
			{
				if (this.eid.underwater && this.aud && this.aud.clip == this.scream && this.aud.isPlaying)
				{
					this.aud.Stop();
					return;
				}
				if (this.fallTime > 0.05f && this.rb.velocity.y < this.fallSpeed)
				{
					this.fallSpeed = this.rb.velocity.y;
					this.reduceFallTime = 0.5f;
					if (this.aud == null)
					{
						this.aud = base.GetComponent<AudioSource>();
					}
					RaycastHit raycastHit;
					if (this.aud && !this.aud.isPlaying && !this.limp && !this.noFallDamage && !this.eid.underwater && (!Physics.Raycast(base.transform.position, Vector3.down, out raycastHit, float.PositiveInfinity, this.lmaskWater, QueryTriggerInteraction.Collide) || ((raycastHit.distance > 42f || this.rb.velocity.y < -60f) && raycastHit.transform.gameObject.layer != 4)))
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

	// Token: 0x06000FD9 RID: 4057 RVA: 0x000764F0 File Offset: 0x000746F0
	private void FixedUpdate()
	{
		if (this.parryFramesLeft > 0)
		{
			this.parryFramesLeft--;
		}
		if (!this.limp && this.gc != null && !this.overrideFalling)
		{
			if (this.knockedBack && this.knockBackCharge <= 0f && (this.rb.velocity.magnitude < 1f || this.v2 != null) && this.gc.onGround)
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
				if (this.nma != null)
				{
					this.nma.updatePosition = false;
					this.nma.updateRotation = false;
					this.nma.enabled = false;
					this.rb.isKinematic = false;
					this.rb.useGravity = true;
				}
			}
			if (!this.grounded && this.gc.onGround)
			{
				this.grounded = true;
			}
			else if (this.grounded && !this.gc.onGround)
			{
				this.grounded = false;
			}
			if (!this.gc.onGround && !this.falling && this.nma != null && (!this.nma.enabled || !this.nma.isOnOffMeshLink))
			{
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
				this.nma.enabled = false;
				this.falling = true;
				this.anim.SetBool("Falling", true);
				if (this.sc != null)
				{
					this.sc.StopFire();
				}
				if (this.tur != null)
				{
					this.tur.CancelAim(true);
				}
				if (this.man && this.man.inAction && !this.man.jumping && !this.man.inControl)
				{
					this.man.CancelActions(true);
				}
			}
		}
		if (this.hitJiggleRoot != null && this.hitJiggleRoot.localPosition != this.jiggleRootPosition)
		{
			this.hitJiggleRoot.localPosition = Vector3.MoveTowards(this.hitJiggleRoot.localPosition, this.jiggleRootPosition, (Vector3.Distance(this.hitJiggleRoot.localPosition, this.jiggleRootPosition) + 1f) * 100f * Time.fixedDeltaTime);
		}
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0007685C File Offset: 0x00074A5C
	public void KnockBack(Vector3 force)
	{
		if ((this.sc == null || !this.sc.dodging) && (this.sm == null || !this.sm.inAction) && (this.tur == null || !this.tur.lodged) && !this.eid.poise)
		{
			if (this.nma != null)
			{
				this.nma.enabled = false;
				this.rb.isKinematic = false;
				this.rb.useGravity = true;
			}
			if (this.man)
			{
				this.man.inControl = false;
				if (this.man.clinging)
				{
					this.man.Uncling();
				}
			}
			if (this.gc && !this.overrideFalling)
			{
				if (!this.knockedBack || (!this.gc.onGround && this.rb.velocity.y < 0f))
				{
					this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
				}
				if (!this.gc.onGround)
				{
					this.rb.AddForce(Vector3.up, ForceMode.VelocityChange);
				}
			}
			if (this.hitJiggleRoot != null)
			{
				Vector3 vector = new Vector3(force.x, 0f, force.z);
				this.hitJiggleRoot.localPosition = this.jiggleRootPosition + vector.normalized * -0.01f;
				if (Vector3.Distance(this.hitJiggleRoot.localPosition, this.jiggleRootPosition) > 0.1f)
				{
					this.hitJiggleRoot.localPosition = this.jiggleRootPosition + (this.hitJiggleRoot.localPosition - this.jiggleRootPosition).normalized * 0.1f;
				}
			}
			this.rb.AddForce(force / 10f, ForceMode.VelocityChange);
			this.knockedBack = true;
			this.knockBackCharge = Mathf.Min(this.knockBackCharge + force.magnitude / 1500f, 0.35f);
			this.brakes = 1f;
		}
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x00076AC8 File Offset: 0x00074CC8
	public void StopKnockBack()
	{
		this.knockBackCharge = 0f;
		if (!(this.nma != null))
		{
			if (this.v2 != null)
			{
				this.knockedBack = false;
				this.juggleWeight = 0f;
			}
			return;
		}
		RaycastHit raycastHit;
		if (!this.gc.onGround || !Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, float.PositiveInfinity, this.lmask))
		{
			this.knockBackCharge = 0.5f;
			return;
		}
		Vector3 zero = Vector3.zero;
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 4f, this.nma.areaMask))
		{
			this.knockedBack = false;
			this.nma.updatePosition = true;
			this.nma.updateRotation = true;
			this.nma.enabled = true;
			if ((!this.sm || !this.sm.moveAtTarget) && (!this.man || !this.man.jumping))
			{
				this.rb.isKinematic = true;
			}
			if (this.man)
			{
				this.man.inControl = true;
			}
			this.juggleWeight = 0f;
			this.nma.Warp(navMeshHit.position);
			return;
		}
		this.knockBackCharge = 0.5f;
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x00076C48 File Offset: 0x00074E48
	public void GetHurt(GameObject target, Vector3 force, float multiplier, float critMultiplier, GameObject sourceWeapon = null, bool fromExplosion = false)
	{
		string text = "";
		bool flag = false;
		bool flag2 = false;
		float num = multiplier;
		GameObject gameObject = null;
		if (this.eid == null)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (force != Vector3.zero && !this.limp && this.sm == null && (this.v2 == null || !this.v2.inIntro) && (this.tur == null || !this.tur.lodged || this.eid.hitter == "heavypunch" || this.eid.hitter == "railcannon" || this.eid.hitter == "cannonball" || this.eid.hitter == "hammer"))
		{
			if (this.tur && this.tur.lodged)
			{
				this.tur.CancelAim(true);
				this.tur.Unlodge();
			}
			this.KnockBack(force / 100f);
			if (this.eid.hitter == "heavypunch" || (this.gc && !this.gc.onGround && this.eid.hitter == "cannonball"))
			{
				this.eid.useBrakes = false;
			}
			else
			{
				this.eid.useBrakes = true;
			}
		}
		if (this.v2 != null && this.v2.secondEncounter && this.eid.hitter == "heavypunch")
		{
			this.v2.InstaEnrage();
		}
		if (this.sc != null && target.gameObject == this.sc.canister && !this.sc.canisterHit && this.eid.hitter == "revolver")
		{
			if (!InvincibleEnemies.Enabled && !this.eid.blessed)
			{
				this.sc.canisterHit = true;
			}
			if (!this.eid.dead && !InvincibleEnemies.Enabled && !this.eid.blessed)
			{
				MonoSingleton<StyleHUD>.Instance.AddPoints(200, "ultrakill.instakill", sourceWeapon, this.eid, -1, "", "");
			}
			MonoSingleton<TimeController>.Instance.ParryFlash();
			base.Invoke("CanisterExplosion", 0.1f);
			return;
		}
		if (this.tur != null && this.tur.aiming && (this.eid.hitter == "revolver" || this.eid.hitter == "coin") && this.tur.interruptables.Contains(target.transform))
		{
			this.tur.Interrupt();
		}
		if (this.gm)
		{
			if (this.gm.hasShield && !this.eid.dead && (this.eid.hitter == "heavypunch" || this.eid.hitter == "hammer"))
			{
				this.gm.ShieldBreak(true, true);
			}
			if (this.gm.hasShield)
			{
				multiplier /= 1.5f;
			}
			if (this.gm.fallen && !this.gm.exploded && this.eid.hitter == "ground slam")
			{
				this.gm.Explode();
				MonoSingleton<NewMovement>.Instance.Launch(Vector3.up * 750f, 8f, false);
			}
		}
		if (this.mf && this.mf.dying && (this.eid.hitter == "heavypunch" || this.eid.hitter == "hammer"))
		{
			this.mf.DeadLaunch(force);
		}
		if (this.eid.hitter == "punch")
		{
			bool flag3 = this.parryables != null && this.parryables.Count > 0 && this.parryables.Contains(target.transform);
			if (this.parryable || (this.partiallyParryable && (flag3 || (this.parryFramesLeft > 0 && this.parryFramesOnPartial))))
			{
				this.parryable = false;
				this.partiallyParryable = false;
				this.parryables.Clear();
				if (!InvincibleEnemies.Enabled && !this.eid.blessed)
				{
					this.health -= (float)((this.parryFramesLeft > 0) ? 4 : 5);
				}
				MonoSingleton<FistControl>.Instance.currentPunch.Parry(false, this.eid, "");
				if (this.sm != null && this.health > 0f)
				{
					if (!this.sm.enraged)
					{
						this.sm.Knockdown(true, fromExplosion);
					}
					else
					{
						this.sm.Enrage();
					}
				}
				else
				{
					base.SendMessage("GotParried", SendMessageOptions.DontRequireReceiver);
				}
			}
			else
			{
				this.parryFramesOnPartial = flag3;
				this.parryFramesLeft = MonoSingleton<FistControl>.Instance.currentPunch.activeFrames;
			}
		}
		else if (this.min && this.min.ramTimer > 0f && this.eid.hitter == "ground slam")
		{
			this.min.GotSlammed();
		}
		if (this.sisy && num > 0f)
		{
			if (this.eid.burners.Count > 0)
			{
				if (this.eid.hitter != "fire")
				{
					if (num <= 0.5f)
					{
						gameObject = this.bsm.GetGore(GoreType.Limb, this.eid, fromExplosion);
						this.sisy.PlayHurtSound(1);
					}
					else
					{
						gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
						this.sisy.PlayHurtSound(2);
					}
				}
				else
				{
					this.sisy.PlayHurtSound(0);
				}
			}
			else if (this.eid.hitter != "fire")
			{
				gameObject = this.bsm.GetGore(GoreType.Smallest, this.eid, fromExplosion);
			}
		}
		float num2 = 0f;
		if (target.gameObject.CompareTag("Head"))
		{
			num2 = 1f;
		}
		else if (target.gameObject.CompareTag("Limb") || target.gameObject.CompareTag("EndLimb"))
		{
			num2 = 0.5f;
		}
		num = multiplier + num2 * multiplier * critMultiplier;
		if (num2 == 0f && (this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone"))
		{
			if (!this.parryable && (target.gameObject != this.chest || this.health - num > 0f))
			{
				num = 0f;
			}
			else if ((this.parryable && (target.gameObject == this.chest || MonoSingleton<PlayerTracker>.Instance.GetPlayerVelocity(false).magnitude > 18f)) || (this.partiallyParryable && this.parryables != null && this.parryables.Contains(target.transform)))
			{
				num *= 1.5f;
				this.parryable = false;
				this.partiallyParryable = false;
				this.parryables.Clear();
				MonoSingleton<NewMovement>.Instance.Parry(this.eid, "");
				if (this.sm != null && this.health - num > 0f)
				{
					if (!this.sm.enraged)
					{
						this.sm.Knockdown(true, fromExplosion);
					}
					else
					{
						this.sm.Enrage();
					}
				}
				else
				{
					base.SendMessage("GotParried", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
		if (this.sisy && !this.limp && this.eid.hitter == "fire" && this.health > 0f && this.health - num < 0.01f && !this.eid.isGasolined)
		{
			num = this.health - 0.01f;
		}
		if (!this.eid.blessed && !InvincibleEnemies.Enabled)
		{
			this.health -= num;
		}
		if (!gameObject && this.eid.hitter != "fire" && num > 0f)
		{
			if ((num2 == 1f && (num >= 1f || this.health <= 0f)) || this.eid.hitter == "hammer")
			{
				gameObject = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
			}
			else if (((num >= 1f || this.health <= 0f) && this.eid.hitter != "explosion") || (this.eid.hitter == "explosion" && target.gameObject.CompareTag("EndLimb")))
			{
				if (target.gameObject.CompareTag("Body"))
				{
					gameObject = this.bsm.GetGore(GoreType.Body, this.eid, fromExplosion);
				}
				else
				{
					gameObject = this.bsm.GetGore(GoreType.Limb, this.eid, fromExplosion);
				}
			}
			else if (this.eid.hitter != "explosion")
			{
				gameObject = this.bsm.GetGore(GoreType.Small, this.eid, fromExplosion);
			}
		}
		if (!this.limp)
		{
			flag2 = true;
			string text2 = target.gameObject.tag.ToLower();
			if (text2 == "endlimb")
			{
				text2 = "limb";
			}
			text = text2;
		}
		if (this.health <= 0f)
		{
			if (this.symbiotic)
			{
				if (this.sm != null && !this.sm.downed && this.symbiote.health > 0f)
				{
					this.sm.downed = true;
					this.sm.Down(fromExplosion);
					base.Invoke("StartHealing", 3f);
				}
				else if (this.sisy != null && !this.sisy.downed && this.symbiote.health > 0f)
				{
					this.sisy.downed = true;
					this.sisy.Knockdown(base.transform.position + base.transform.forward);
					base.Invoke("StartHealing", 3f);
				}
				else if (this.symbiote.health <= 0f)
				{
					this.symbiotic = false;
					if (!this.limp)
					{
						this.GoLimp(fromExplosion);
					}
				}
			}
			else
			{
				if (!this.limp)
				{
					this.GoLimp(fromExplosion);
				}
				if (MonoSingleton<BloodsplatterManager>.Instance.goreOn && !target.gameObject.CompareTag("EndLimb"))
				{
					float num3 = 1f;
					if (this.eid.hitter == "shotgun" || this.eid.hitter == "shotgunzone" || this.eid.hitter == "explosion")
					{
						num3 = 0.5f;
					}
					string tag = target.gameObject.tag;
					if (!(tag == "Head"))
					{
						if (tag == "Limb")
						{
							int num4 = 0;
							while ((float)num4 < 4f * num3)
							{
								GameObject gameObject2 = this.bsm.GetGib(BSType.gib);
								if (gameObject2 && this.gz && this.gz.gibZone)
								{
									this.ReadyGib(gameObject2, target);
								}
								num4++;
							}
							if (target.transform.childCount > 0 && this.dismemberment)
							{
								Transform child = target.transform.GetChild(0);
								CharacterJoint[] componentsInChildren = target.GetComponentsInChildren<CharacterJoint>();
								if (componentsInChildren.Length != 0)
								{
									foreach (CharacterJoint characterJoint in componentsInChildren)
									{
										EnemyIdentifierIdentifier enemyIdentifierIdentifier;
										if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
										{
											enemyIdentifierIdentifier.SetupForHellBath();
										}
										Object.Destroy(characterJoint);
									}
								}
								CharacterJoint component = target.GetComponent<CharacterJoint>();
								if (component != null)
								{
									component.connectedBody = null;
									Object.Destroy(component);
								}
								target.transform.position = child.position;
								target.transform.SetParent(child);
								child.SetParent(this.gz.gibZone);
								Object.Destroy(target.GetComponent<Rigidbody>());
							}
						}
					}
					else
					{
						int num5 = 0;
						while ((float)num5 < 6f * num3)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.skullChunk);
							if (gameObject2 && this.gz && this.gz.gibZone)
							{
								this.ReadyGib(gameObject2, target);
							}
							num5++;
						}
						int num6 = 0;
						while ((float)num6 < 4f * num3)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.brainChunk);
							if (gameObject2 && this.gz && this.gz.gibZone)
							{
								this.ReadyGib(gameObject2, target);
							}
							num6++;
						}
						int num7 = 0;
						while ((float)num7 < 2f * num3)
						{
							GameObject gameObject2 = this.bsm.GetGib(BSType.eyeball);
							if (gameObject2 && this.gz && this.gz.gibZone)
							{
								this.ReadyGib(gameObject2, target);
							}
							gameObject2 = this.bsm.GetGib(BSType.jawChunk);
							if (gameObject2 && this.gz && this.gz.gibZone)
							{
								this.ReadyGib(gameObject2, target);
							}
							num7++;
						}
					}
				}
				if (this.dismemberment)
				{
					if (!target.gameObject.CompareTag("Body"))
					{
						Collider collider;
						if (target.TryGetComponent<Collider>(out collider))
						{
							Object.Destroy(collider);
						}
						target.transform.localScale = Vector3.zero;
					}
					else if (target.gameObject == this.chest && this.v2 == null && this.sc == null)
					{
						this.chestHP -= num;
						if (this.chestHP <= 0f || this.eid.hitter == "shotgunzone" || this.eid.hitter == "hammerzone")
						{
							CharacterJoint[] componentsInChildren2 = target.GetComponentsInChildren<CharacterJoint>();
							if (componentsInChildren2.Length != 0)
							{
								foreach (CharacterJoint characterJoint2 in componentsInChildren2)
								{
									if (characterJoint2.transform.parent.parent == this.chest.transform)
									{
										EnemyIdentifierIdentifier enemyIdentifierIdentifier2;
										if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint2.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier2))
										{
											enemyIdentifierIdentifier2.SetupForHellBath();
										}
										Object.Destroy(characterJoint2);
										characterJoint2.transform.parent = null;
									}
								}
							}
							if (MonoSingleton<BloodsplatterManager>.Instance.goreOn)
							{
								for (int j = 0; j < 2; j++)
								{
									GameObject gib = this.bsm.GetGib(BSType.gib);
									if (gib && this.gz && this.gz.gibZone)
									{
										this.ReadyGib(gib, target);
									}
								}
							}
							GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, fromExplosion);
							gore.transform.position = target.transform.position;
							gore.transform.SetParent(this.gz.goreZone, true);
							target.transform.localScale = Vector3.zero;
						}
					}
				}
			}
			if (this.limp)
			{
				Rigidbody componentInParent = target.GetComponentInParent<Rigidbody>();
				if (componentInParent != null)
				{
					componentInParent.AddForce(force);
				}
			}
		}
		if (gameObject != null)
		{
			if (!this.gz)
			{
				this.gz = GoreZone.ResolveGoreZone(base.transform);
			}
			Collider collider2;
			if (this.thickLimbs && target.TryGetComponent<Collider>(out collider2))
			{
				gameObject.transform.position = collider2.ClosestPoint(MonoSingleton<NewMovement>.Instance.transform.position);
			}
			else
			{
				gameObject.transform.position = target.transform.position;
			}
			if (this.eid.hitter == "drill")
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
		if ((this.health > 0f || this.symbiotic) && this.hurtSounds.Length != 0 && !this.eid.blessed)
		{
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.clip = this.hurtSounds[Random.Range(0, this.hurtSounds.Length)];
			if (this.tur)
			{
				this.aud.volume = 0.85f;
			}
			else if (this.min)
			{
				this.aud.volume = 1f;
			}
			else
			{
				this.aud.volume = 0.5f;
			}
			if (this.sm != null)
			{
				this.aud.pitch = Random.Range(0.85f, 1.35f);
			}
			else
			{
				this.aud.pitch = Random.Range(0.9f, 1.1f);
			}
			this.aud.priority = 12;
			this.aud.Play();
		}
		if (num == 0f || this.eid.puppet)
		{
			flag2 = false;
		}
		if (flag2 && this.eid.hitter != "enemy")
		{
			if (this.scalc == null)
			{
				this.scalc = MonoSingleton<StyleCalculator>.Instance;
			}
			if (this.health <= 0f && !this.symbiotic && (this.v2 == null || !this.v2.dontDie) && (!this.eid.flying || this.mf))
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
			else if (this.health > 0f && this.gc && !this.gc.onGround && (this.eid.hitter == "explosion" || this.eid.hitter == "ffexplosion" || this.eid.hitter == "railcannon"))
			{
				this.scalc.shud.AddPoints(20, "ultrakill.fireworksweak", sourceWeapon, this.eid, -1, "", "");
			}
			if (this.eid.hitter != "secret")
			{
				if (this.bigKill)
				{
					this.scalc.HitCalculator(this.eid.hitter, "spider", text, flag, this.eid, sourceWeapon);
					return;
				}
				this.scalc.HitCalculator(this.eid.hitter, "machine", text, flag, this.eid, sourceWeapon);
			}
		}
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x000782AD File Offset: 0x000764AD
	public void GoLimp()
	{
		this.GoLimp(false);
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x000782B8 File Offset: 0x000764B8
	public void GoLimp(bool fromExplosion = false)
	{
		if (this.limp)
		{
			return;
		}
		if (!this.gz)
		{
			this.gz = GoreZone.ResolveGoreZone(base.transform);
		}
		UnityEvent unityEvent = this.onDeath;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.smr != null)
		{
			this.smr.updateWhenOffscreen = true;
		}
		if (this.health > 0f)
		{
			this.health = 0f;
		}
		if (!this.mf)
		{
			base.Invoke("StopHealing", 1f);
		}
		if (this.v2)
		{
			this.v2.active = false;
			this.v2.Die();
		}
		if (this.mf)
		{
			this.mf.active = false;
		}
		if (this.tur)
		{
			this.tur.OnDeath();
		}
		if (this.fm)
		{
			this.fm.OnDeath();
		}
		if (this.man)
		{
			this.man.OnDeath();
		}
		SwingCheck2[] componentsInChildren = base.GetComponentsInChildren<SwingCheck2>();
		if (this.sm != null)
		{
			this.anim.StopPlayback();
			SwingCheck2[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i]);
			}
			this.sm.CoolSword();
			if (this.sm.currentEnrageEffect != null)
			{
				Object.Destroy(this.sm.currentEnrageEffect);
			}
			Object.Destroy(this.sm);
		}
		if (this.sc != null)
		{
			if (this.anim != null)
			{
				this.anim.StopPlayback();
			}
			BulletCheck componentInChildren = base.GetComponentInChildren<BulletCheck>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren.gameObject);
			}
			this.sc.hose.SetParent(this.sc.hoseTarget, true);
			this.sc.hose.transform.localPosition = Vector3.zero;
			this.sc.hose.transform.localScale = Vector3.zero;
			this.sc.StopFire();
			this.sc.dead = true;
			this.sc.damaging = false;
			FireZone componentInChildren2 = base.GetComponentInChildren<FireZone>();
			if (componentInChildren2)
			{
				Object.Destroy(componentInChildren2.gameObject);
			}
			if (this.sc.canister != null)
			{
				this.sc.canister.GetComponentInChildren<ParticleSystem>().Stop();
				AudioSource componentInChildren3 = this.sc.canister.GetComponentInChildren<AudioSource>();
				if (componentInChildren3 != null)
				{
					AudioLowPassFilter audioLowPassFilter;
					if (componentInChildren3.TryGetComponent<AudioLowPassFilter>(out audioLowPassFilter))
					{
						Object.Destroy(audioLowPassFilter);
					}
					Object.Destroy(componentInChildren3);
				}
			}
		}
		if (this.destroyOnDeath.Length != 0)
		{
			foreach (GameObject gameObject in this.destroyOnDeath)
			{
				if (gameObject.activeInHierarchy)
				{
					Transform transform = gameObject.GetComponentInParent<Rigidbody>().transform;
					if (transform)
					{
						gameObject.transform.SetParent(transform);
						gameObject.transform.position = transform.position;
						gameObject.transform.localScale = Vector3.zero;
					}
				}
			}
		}
		if (!this.dontDie && !this.eid.dontCountAsKills && !this.limp)
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
		EnemySimplifier[] componentsInChildren2 = base.GetComponentsInChildren<EnemySimplifier>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].Begone();
		}
		if (this.deadMaterial != null)
		{
			this.smr.sharedMaterial = this.deadMaterial;
		}
		else if (this.smr != null && !this.mf)
		{
			this.smr.sharedMaterial = this.originalMaterial;
		}
		if (this.nma != null)
		{
			Object.Destroy(this.nma);
			this.nma = null;
		}
		if (!this.v2 && !this.specialDeath)
		{
			Object.Destroy(this.anim);
			Object.Destroy(base.gameObject.GetComponent<Collider>());
			if (this.rb == null)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			Object.Destroy(this.rb);
		}
		if (this.aud == null)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.deathSound != null)
		{
			this.aud.clip = this.deathSound;
			this.aud.pitch = Random.Range(0.85f, 1.35f);
			this.aud.priority = 11;
			this.aud.Play();
			if (this.tur)
			{
				this.aud.volume = 1f;
			}
		}
		if (!this.limp)
		{
			base.SendMessage("Death", SendMessageOptions.DontRequireReceiver);
			if (this.eid.hitter != "spin")
			{
				if (this.simpleDeath)
				{
					Explosion[] componentsInChildren3 = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.explosion, base.transform.position, base.transform.rotation).GetComponentsInChildren<Explosion>();
					for (int i = 0; i < componentsInChildren3.Length; i++)
					{
						componentsInChildren3[i].canHit = AffectedSubjects.EnemiesOnly;
					}
					Object.Destroy(base.gameObject);
				}
				else if (!this.specialDeath && !this.v2 && !this.mf)
				{
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
							if (this.man)
							{
								rigidbody.AddForce((rigidbody.position - this.eid.overrideCenter.transform.position).normalized * Random.Range(20f, 30f), ForceMode.VelocityChange);
								rigidbody.AddTorque(Random.onUnitSphere * 360f, ForceMode.VelocityChange);
								Object.Instantiate<GameObject>(this.man.bloodSpray, rigidbody.transform.position, Quaternion.LookRotation(rigidbody.transform.parent.position - rigidbody.transform.position)).transform.SetParent(rigidbody.transform, true);
								rigidbody.transform.SetParent(this.gz.goreZone, true);
							}
						}
					}
				}
			}
			if (this.man)
			{
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, this.eid, fromExplosion);
				gore.transform.position = this.chest.transform.position;
				gore.transform.SetParent(this.gz.goreZone, true);
				gore.SetActive(true);
			}
			if (this.musicRequested)
			{
				MonoSingleton<MusicManager>.Instance.PlayCleanMusic();
			}
		}
		this.parryable = false;
		this.partiallyParryable = false;
		this.limp = true;
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00078A67 File Offset: 0x00076C67
	private void StartHealing()
	{
		if (this.symbiotic && this.symbiote != null)
		{
			this.healing = true;
		}
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x00078A86 File Offset: 0x00076C86
	private void StopHealing()
	{
		this.noheal = true;
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00078A90 File Offset: 0x00076C90
	public void CanisterExplosion()
	{
		if (InvincibleEnemies.Enabled || this.eid.blessed)
		{
			if (this.sc && this.sc.canisterHit)
			{
				this.sc.canisterHit = false;
			}
			return;
		}
		this.eid.Explode(true);
		foreach (Explosion explosion in Object.Instantiate<GameObject>(this.sc.explosion.ToAsset(), this.sc.canister.transform.position, Quaternion.identity).GetComponentsInChildren<Explosion>())
		{
			explosion.maxSize *= 1.75f;
			explosion.damage = 50;
			explosion.friendlyFire = true;
		}
		CharacterJoint[] componentsInChildren2 = this.chest.GetComponentsInChildren<CharacterJoint>();
		if (componentsInChildren2.Length != 0)
		{
			foreach (CharacterJoint characterJoint in componentsInChildren2)
			{
				if (characterJoint.transform.parent.parent == this.chest.transform)
				{
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					if (StockMapInfo.Instance.removeGibsWithoutAbsorbers && characterJoint.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
					{
						enemyIdentifierIdentifier.SetupForHellBath();
					}
					Object.Destroy(characterJoint);
					characterJoint.transform.parent = null;
				}
			}
		}
		if (MonoSingleton<BloodsplatterManager>.Instance.goreOn)
		{
			for (int j = 0; j < 2; j++)
			{
				GameObject gib = this.bsm.GetGib(BSType.gib);
				if (gib && this.gz && this.gz.gibZone)
				{
					this.ReadyGib(gib, this.sc.canister);
				}
			}
		}
		GameObject gore = this.bsm.GetGore(GoreType.Head, this.eid, true);
		gore.transform.position = this.sc.canister.transform.position;
		gore.transform.SetParent(this.gz.goreZone, true);
		this.chest.transform.localScale = Vector3.zero;
		Collider collider;
		if (this.sc.canister.TryGetComponent<Collider>(out collider))
		{
			Object.Destroy(collider);
		}
		this.sc.canister.transform.localScale = Vector3.zero;
		this.sc.canister.transform.parent = this.gz.transform;
		this.sc.canister.transform.position = Vector3.zero;
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x00078CF9 File Offset: 0x00076EF9
	public void ReadyGib(GameObject tempGib, GameObject target)
	{
		tempGib.transform.SetPositionAndRotation(target.transform.position, Random.rotation);
		this.gz.SetGoreZone(tempGib);
		if (!GraphicsSettings.bloodEnabled)
		{
			tempGib.SetActive(false);
		}
	}

	// Token: 0x06000FE3 RID: 4067 RVA: 0x00078D30 File Offset: 0x00076F30
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

	// Token: 0x04001576 RID: 5494
	public float health;

	// Token: 0x04001577 RID: 5495
	private BloodsplatterManager bsm;

	// Token: 0x04001578 RID: 5496
	public bool limp;

	// Token: 0x04001579 RID: 5497
	private EnemyIdentifier eid;

	// Token: 0x0400157A RID: 5498
	public GameObject chest;

	// Token: 0x0400157B RID: 5499
	private float chestHP = 3f;

	// Token: 0x0400157C RID: 5500
	private AudioSource aud;

	// Token: 0x0400157D RID: 5501
	public AudioClip[] hurtSounds;

	// Token: 0x0400157E RID: 5502
	[HideInInspector]
	public StyleCalculator scalc;

	// Token: 0x0400157F RID: 5503
	private GoreZone gz;

	// Token: 0x04001580 RID: 5504
	public Material deadMaterial;

	// Token: 0x04001581 RID: 5505
	private Material originalMaterial;

	// Token: 0x04001582 RID: 5506
	public SkinnedMeshRenderer smr;

	// Token: 0x04001583 RID: 5507
	private NavMeshAgent nma;

	// Token: 0x04001584 RID: 5508
	private Rigidbody rb;

	// Token: 0x04001585 RID: 5509
	private Rigidbody[] rbs;

	// Token: 0x04001586 RID: 5510
	private Animator anim;

	// Token: 0x04001587 RID: 5511
	public AudioClip deathSound;

	// Token: 0x04001588 RID: 5512
	public AudioClip scream;

	// Token: 0x04001589 RID: 5513
	private bool noheal;

	// Token: 0x0400158A RID: 5514
	public bool bigKill;

	// Token: 0x0400158B RID: 5515
	public bool thickLimbs;

	// Token: 0x0400158C RID: 5516
	public bool parryable;

	// Token: 0x0400158D RID: 5517
	public bool partiallyParryable;

	// Token: 0x0400158E RID: 5518
	[HideInInspector]
	public List<Transform> parryables = new List<Transform>();

	// Token: 0x0400158F RID: 5519
	private SwordsMachine sm;

	// Token: 0x04001590 RID: 5520
	private Streetcleaner sc;

	// Token: 0x04001591 RID: 5521
	private V2 v2;

	// Token: 0x04001592 RID: 5522
	private Mindflayer mf;

	// Token: 0x04001593 RID: 5523
	private Sisyphus sisy;

	// Token: 0x04001594 RID: 5524
	private Turret tur;

	// Token: 0x04001595 RID: 5525
	private Ferryman fm;

	// Token: 0x04001596 RID: 5526
	private Mannequin man;

	// Token: 0x04001597 RID: 5527
	private Minotaur min;

	// Token: 0x04001598 RID: 5528
	private Gutterman gm;

	// Token: 0x04001599 RID: 5529
	public GameObject[] destroyOnDeath;

	// Token: 0x0400159A RID: 5530
	public Machine symbiote;

	// Token: 0x0400159B RID: 5531
	private bool symbiotic;

	// Token: 0x0400159C RID: 5532
	private bool healing;

	// Token: 0x0400159D RID: 5533
	public bool grounded;

	// Token: 0x0400159E RID: 5534
	[HideInInspector]
	public GroundCheckEnemy gc;

	// Token: 0x0400159F RID: 5535
	public bool knockedBack;

	// Token: 0x040015A0 RID: 5536
	public bool overrideFalling;

	// Token: 0x040015A1 RID: 5537
	private float knockBackCharge;

	// Token: 0x040015A2 RID: 5538
	public float brakes;

	// Token: 0x040015A3 RID: 5539
	public float juggleWeight;

	// Token: 0x040015A4 RID: 5540
	public bool falling;

	// Token: 0x040015A5 RID: 5541
	private LayerMask lmask;

	// Token: 0x040015A6 RID: 5542
	private LayerMask lmaskWater;

	// Token: 0x040015A7 RID: 5543
	private float fallSpeed;

	// Token: 0x040015A8 RID: 5544
	private float fallTime;

	// Token: 0x040015A9 RID: 5545
	private float reduceFallTime;

	// Token: 0x040015AA RID: 5546
	public bool noFallDamage;

	// Token: 0x040015AB RID: 5547
	public bool dontDie;

	// Token: 0x040015AC RID: 5548
	public bool dismemberment;

	// Token: 0x040015AD RID: 5549
	public bool specialDeath;

	// Token: 0x040015AE RID: 5550
	public bool simpleDeath;

	// Token: 0x040015AF RID: 5551
	[HideInInspector]
	public bool musicRequested;

	// Token: 0x040015B0 RID: 5552
	public UnityEvent onDeath;

	// Token: 0x040015B1 RID: 5553
	private int parryFramesLeft;

	// Token: 0x040015B2 RID: 5554
	private bool parryFramesOnPartial;

	// Token: 0x040015B3 RID: 5555
	public Transform hitJiggleRoot;

	// Token: 0x040015B4 RID: 5556
	private Vector3 jiggleRootPosition;
}
