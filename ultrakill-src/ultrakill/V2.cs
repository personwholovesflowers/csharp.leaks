using System;
using System.Collections.Generic;
using Sandbox;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000496 RID: 1174
public class V2 : MonoBehaviour, IEnrage, IAlter, IAlterOptions<bool>
{
	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06001AEA RID: 6890 RVA: 0x000DD1B8 File Offset: 0x000DB3B8
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06001AEB RID: 6891 RVA: 0x000DD1C5 File Offset: 0x000DB3C5
	private void Awake()
	{
		this.anim = base.GetComponentInChildren<Animator>();
		this.bhb = base.GetComponent<BossHealthBar>();
		this.mac = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
	}

	// Token: 0x06001AEC RID: 6892 RVA: 0x000DD204 File Offset: 0x000DB404
	private void Start()
	{
		if (this.alwaysAimAtGround)
		{
			this.aimAtGround = true;
		}
		this.ensims = base.GetComponentsInChildren<EnemySimplifier>();
		if (MonoSingleton<StatueIntroChecker>.Instance && MonoSingleton<StatueIntroChecker>.Instance.beenSeen)
		{
			this.longIntro = false;
		}
		if (!this.intro)
		{
			this.active = true;
			if (this.bhb)
			{
				this.bhb.enabled = true;
			}
		}
		else
		{
			this.inIntro = true;
			this.rb.AddForce(base.transform.forward * 20f, ForceMode.VelocityChange);
			this.anim.SetBool("InAir", true);
			if (this.anim.layerCount > 1)
			{
				this.anim.SetLayerWeight(1, 1f);
				this.anim.SetLayerWeight(2, 0f);
			}
			if (this.longIntro)
			{
				this.eidids = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
				EnemyIdentifierIdentifier[] array = this.eidids;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].GetComponent<Collider>().enabled = false;
				}
				if (this.bhb)
				{
					this.bhb.enabled = false;
				}
			}
			else if (this.bhb)
			{
				this.bhb.enabled = true;
			}
		}
		this.SetSpeed();
		this.running = true;
		this.aiming = true;
		this.inPattern = true;
		this.wingTrails = base.GetComponentsInChildren<TrailRenderer>();
		this.drags = base.GetComponentsInChildren<DragBehind>();
		this.ChangeDirection(Random.Range(-90f, 90f));
		this.SwitchPattern(0);
		this.shootCooldown = 1f;
		this.altShootCooldown = 5f;
		if (!this.weapons[this.currentWeapon].activeInHierarchy)
		{
			GameObject[] array2 = this.weapons;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActive(false);
			}
			this.weapons[this.currentWeapon].SetActive(true);
		}
		if (!this.bhb)
		{
			this.bossVersion = false;
		}
		if (this.secondEncounter)
		{
			this.SlowUpdate();
		}
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000DD41B File Offset: 0x000DB61B
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000DD424 File Offset: 0x000DB624
	private void SetSpeed()
	{
		if (!this.nma)
		{
			this.nma = base.GetComponent<NavMeshAgent>();
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
		if (this.originalMovementSpeed != 0f)
		{
			this.movementSpeed = this.originalMovementSpeed;
		}
		if (this.difficulty >= 4)
		{
			this.movementSpeed *= 1.5f;
		}
		else if (this.difficulty == 2)
		{
			this.movementSpeed *= 0.85f;
		}
		else if (this.difficulty == 1)
		{
			this.movementSpeed *= 0.75f;
		}
		else if (this.difficulty == 0)
		{
			this.movementSpeed *= 0.65f;
		}
		this.movementSpeed *= this.eid.totalSpeedModifier;
		this.originalMovementSpeed = this.movementSpeed;
		if (this.enraged)
		{
			this.movementSpeed *= 2f;
		}
		if (this.nma)
		{
			this.nma.speed = this.originalMovementSpeed;
		}
		GameObject[] array = this.weapons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.GetChild(0).SendMessage("UpdateBuffs", this.eid, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000DD5C0 File Offset: 0x000DB7C0
	private void Update()
	{
		if (this.target == null)
		{
			if (this.gc.onGround)
			{
				this.rb.velocity = Vector3.zero;
			}
			if (this.anim.layerCount > 1)
			{
				this.anim.SetLayerWeight(1, 0f);
				this.anim.SetLayerWeight(2, 0f);
			}
			if (this.sliding)
			{
				this.StopSlide();
			}
		}
		if (this.active && !this.escaping && this.target != null)
		{
			if (!this.sliding && this.slideOnly && this.gc.onGround && !this.dodging)
			{
				if (this.eid.enemyType != EnemyType.BigJohnator)
				{
					this.anim.Play("Slide");
				}
				base.transform.LookAt(new Vector3(base.transform.position.x + this.forceSlideDirection.x, base.transform.position.y + this.forceSlideDirection.y, base.transform.position.z + this.forceSlideDirection.z));
				this.Slide();
			}
			this.targetPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
			if (!this.slideOnly)
			{
				if (this.dodging)
				{
					this.anim.SetBool("InAir", true);
					if (this.anim.layerCount > 1)
					{
						this.anim.SetLayerWeight(1, 0f);
						this.anim.SetLayerWeight(2, 0f);
					}
					if (this.drags.Length != 0 && !this.drags[0].active)
					{
						DragBehind[] array = this.drags;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].active = true;
						}
					}
				}
				else if (!this.gc.onGround)
				{
					this.anim.SetBool("InAir", true);
					if (this.anim.layerCount > 1)
					{
						this.anim.SetLayerWeight(1, 0f);
						this.anim.SetLayerWeight(2, 0f);
					}
					if (this.drags.Length != 0 && !this.drags[0].active)
					{
						DragBehind[] array = this.drags;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].active = true;
						}
					}
				}
				else if (this.running && !this.sliding)
				{
					this.anim.SetBool("InAir", false);
					if (this.anim.layerCount > 1)
					{
						this.anim.SetLayerWeight(1, 1f);
					}
					if (this.anim.transform.rotation.eulerAngles.y > base.transform.rotation.eulerAngles.y)
					{
						this.anim.SetBool("RunningLeft", true);
					}
					else
					{
						this.anim.SetBool("RunningLeft", false);
					}
					float num = Quaternion.Angle(this.anim.transform.rotation, base.transform.rotation);
					if (num > 90f)
					{
						this.anim.SetBool("RunningBack", true);
					}
					else
					{
						this.anim.SetBool("RunningBack", false);
					}
					if (this.anim.layerCount > 2)
					{
						if (num <= 90f)
						{
							this.anim.SetLayerWeight(2, num / 90f);
						}
						else
						{
							this.anim.SetLayerWeight(2, Mathf.Abs(-180f + num) / 90f);
						}
					}
					if (this.drags.Length != 0 && this.drags[0].active)
					{
						DragBehind[] array = this.drags;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].active = false;
						}
					}
				}
				else
				{
					this.anim.SetBool("InAir", false);
					if (this.anim.layerCount > 1)
					{
						this.anim.SetLayerWeight(1, 0f);
						this.anim.SetLayerWeight(2, 0f);
					}
					if (this.sliding && this.drags.Length != 0 && !this.drags[0].active)
					{
						DragBehind[] array = this.drags;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].active = true;
						}
					}
					else if (!this.sliding && this.drags[0].active)
					{
						DragBehind[] array = this.drags;
						for (int i = 0; i < array.Length; i++)
						{
							array[i].active = false;
						}
					}
				}
			}
			if (this.eid.target == null)
			{
				this.running = false;
				if (this.mac.health <= this.knockOutHealth && this.knockOutHealth != 0f && this.firstPhase)
				{
					this.firstPhase = false;
					this.KnockedOut("KnockedDown");
					this.eid.totalDamageTakenMultiplier = 0f;
				}
				return;
			}
			if (!this.sliding)
			{
				this.targetRot = Quaternion.LookRotation(this.targetPos - base.transform.position, Vector3.up);
				if (this.inPattern && this.currentPattern != 0)
				{
					if (this.cowardPattern)
					{
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(base.transform.position - this.targetPos, Vector3.up), Time.deltaTime * 350f * this.eid.totalSpeedModifier);
					}
					else if (this.currentPattern == 1 || Vector3.Distance(base.transform.position, this.targetPos) < 10f)
					{
						float num2 = 90f;
						if (Vector3.Distance(base.transform.position, this.targetPos) > 10f)
						{
							num2 = 80f;
						}
						else if (Vector3.Distance(base.transform.position, this.targetPos) < 5f)
						{
							num2 = 100f;
						}
						Quaternion quaternion = this.targetRot;
						quaternion.eulerAngles = new Vector3(quaternion.eulerAngles.x, quaternion.eulerAngles.y + num2 * (float)this.pattern1direction, quaternion.eulerAngles.z);
						base.transform.rotation = quaternion;
					}
					else
					{
						base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.targetRot, Time.deltaTime * 350f * this.eid.totalSpeedModifier);
						if (base.transform.rotation == this.targetRot && this.playerInSight && this.gc.onGround && !this.jumping && ((this.difficulty <= 2 && MonoSingleton<NewMovement>.Instance.hp > 50 && Vector3.Distance(base.transform.position, this.targetPos) > 10f) || Vector3.Distance(base.transform.position, this.targetPos) > 20f))
						{
							this.Slide();
						}
					}
				}
				if (this.inPattern && this.currentPattern != 1 && !this.sliding && this.gc.onGround && this.difficulty >= 4 && this.randomSlideCheck > 0.5f)
				{
					this.randomSlideCheck = 0f;
					if (Random.Range(0f, 1f) > 0.75f)
					{
						this.Slide();
					}
				}
				this.anim.transform.rotation = Quaternion.RotateTowards(this.anim.transform.rotation, this.targetRot, Time.deltaTime * 10f * Quaternion.Angle(this.anim.transform.rotation, this.targetRot) * this.eid.totalSpeedModifier);
			}
			else if (!this.slideOnly)
			{
				Quaternion quaternion2 = Quaternion.LookRotation(base.transform.forward, Vector3.up);
				Quaternion quaternion3 = Quaternion.LookRotation(this.targetPos - base.transform.position, Vector3.up);
				if (this.nma && !this.playerInSight)
				{
					this.StopSlide();
				}
				else if (Quaternion.Angle(quaternion2, quaternion3) > 90f || (this.distancePatience >= 5f && Quaternion.Angle(quaternion2, quaternion3) > 45f))
				{
					this.slideStopTimer = Mathf.MoveTowards(this.slideStopTimer, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					if (this.slideStopTimer <= 0f || this.enraged || (this.difficulty <= 2 && MonoSingleton<NewMovement>.Instance.hp < 50))
					{
						this.StopSlide();
					}
				}
			}
			else
			{
				this.anim.transform.localRotation = Quaternion.identity;
			}
			if (this.dodgeCooldown < 6f)
			{
				if (this.difficulty >= 4)
				{
					this.dodgeCooldown = Mathf.MoveTowards(this.dodgeCooldown, 6f, Time.deltaTime * this.eid.totalSpeedModifier);
				}
				else if (this.difficulty == 3)
				{
					this.dodgeCooldown = Mathf.MoveTowards(this.dodgeCooldown, 6f, Time.deltaTime * 0.5f * this.eid.totalSpeedModifier);
				}
				else
				{
					this.dodgeCooldown = Mathf.MoveTowards(this.dodgeCooldown, 6f, Time.deltaTime * 0.1f * this.eid.totalSpeedModifier);
				}
			}
			if (this.dodgeLeft > 0f)
			{
				this.dodgeLeft = Mathf.MoveTowards(this.dodgeLeft, 0f, Time.deltaTime * 3f * this.eid.totalSpeedModifier);
				if (this.dodgeLeft <= 0f)
				{
					this.dodging = false;
					this.eid.hookIgnore = false;
					this.inPattern = true;
					this.CheckPattern();
					if (this.currentPattern == 2 && !this.cowardPattern)
					{
						Quaternion rotation = this.anim.transform.rotation;
						base.transform.LookAt(this.targetPos);
						this.anim.transform.rotation = rotation;
					}
				}
			}
			if (this.patternCooldown > 0f)
			{
				this.patternCooldown = Mathf.MoveTowards(this.patternCooldown, 0f, Time.deltaTime);
			}
			if (this.inPattern)
			{
				if (this.playerInSight)
				{
					int num3 = this.currentPattern;
					if (this.gc.onGround && !this.jumping)
					{
						if (Physics.Raycast(base.transform.position + Vector3.up, base.transform.forward, 4f, LayerMaskDefaults.Get(LMD.Environment)) && !this.slideOnly)
						{
							this.Jump();
						}
					}
					else if (this.wc.onGround)
					{
						if (!this.gc.onGround && !this.jumping)
						{
							this.WallJump();
						}
						else if (this.gc.onGround)
						{
							this.ChangeDirection((float)Random.Range(100, 260));
						}
					}
				}
				if (Vector3.Distance(base.transform.position, this.target.position) > 15f || !this.playerInSight)
				{
					this.distancePatience = Mathf.MoveTowards(this.distancePatience, 12f, Time.deltaTime * this.eid.totalSpeedModifier);
					if ((this.distancePatience >= 4f || Vector3.Distance(base.transform.position, this.target.position) > 30f) && this.currentPattern != 2)
					{
						this.currentPattern = 2;
						this.SwitchPattern(2);
					}
					if (this.distancePatience == 12f && !this.enraged && !this.dontEnrage)
					{
						this.Enrage();
					}
				}
				else if (this.distancePatience > 0f)
				{
					if (!this.enraged)
					{
						this.distancePatience = Mathf.MoveTowards(this.distancePatience, 0f, Time.deltaTime * 2f * this.eid.totalSpeedModifier);
					}
					else
					{
						this.distancePatience = Mathf.MoveTowards(this.distancePatience, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
					if (this.enraged && this.distancePatience < 10f)
					{
						this.UnEnrage();
					}
				}
			}
			if (!this.slideOnly)
			{
				if ((this.currentPattern == 1 && !this.cowardPattern) || Vector3.Distance(base.transform.position, this.target.position) < 10f)
				{
					if (this.currentPattern == 1)
					{
						this.circleTimer = Mathf.MoveTowards(this.circleTimer, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
					else
					{
						this.circleTimer = Mathf.MoveTowards(this.circleTimer, 0f, Time.deltaTime * 1.5f * this.eid.totalSpeedModifier);
					}
					if (this.circleTimer <= 0f && !this.dodging && this.dodgeLeft <= 0f && !this.enraged && (MonoSingleton<NewMovement>.Instance.hp > 33 || this.difficulty >= 4))
					{
						this.circleTimer = 1f;
						this.ForceDodge(base.transform.position - this.targetPos);
						if (!this.cowardPattern && this.currentPattern != 1)
						{
							this.cowardPattern = true;
							this.SwitchPattern(3);
						}
					}
				}
				else
				{
					this.circleTimer = Mathf.MoveTowards(this.circleTimer, 5f, Time.deltaTime * this.eid.totalSpeedModifier);
					if (this.cowardPattern && this.circleTimer > 2f)
					{
						this.cowardPattern = false;
						this.CheckPattern();
						this.SwitchPattern(this.currentPattern);
					}
				}
			}
			float num4 = 1f;
			if (this.difficulty == 1)
			{
				num4 = 0.85f;
			}
			if (this.difficulty == 0)
			{
				num4 = 0.75f;
			}
			if (this.altShootCooldown > 0f)
			{
				this.altShootCooldown = Mathf.MoveTowards(this.altShootCooldown, 0f, Time.deltaTime * num4 * this.eid.totalSpeedModifier);
			}
			if (this.secondEncounter && !this.enraged && this.coinsToThrow <= 0)
			{
				if (this.coins.Count > 0)
				{
					Coin coin = null;
					float num5 = 60f;
					foreach (Coin coin2 in this.coins)
					{
						float num6 = Vector3.Distance(coin2.transform.position, this.aimAtTarget[1].position);
						if (!coin2.shot && Vector3.Distance(coin2.transform.position, base.transform.position) < num5 && !Physics.Raycast(this.aimAtTarget[1].position, coin2.transform.position - this.aimAtTarget[1].position, num6, LayerMaskDefaults.Get(LMD.Environment)))
						{
							num5 = num6;
							coin = coin2;
						}
						if (this.eid.difficultyOverride >= 0)
						{
							coin2.difficulty = this.eid.difficultyOverride;
						}
					}
					if (coin != null)
					{
						if (this.coinsInSightCooldown > 0f)
						{
							this.coinsInSightCooldown = Mathf.MoveTowards(this.coinsInSightCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
						}
						else
						{
							this.overrideTarget = coin.transform;
							this.overrideTargetRb = coin.GetComponent<Rigidbody>();
							if (this.currentWeapon != 0 || !this.aboutToShoot || !this.shootingForCoin)
							{
								if (this.currentWeapon != 0 || !this.shootingForCoin)
								{
									base.CancelInvoke("ShootWeapon");
									base.CancelInvoke("AltShootWeapon");
									this.weapons[this.currentWeapon].transform.GetChild(0).SendMessage("CancelAltCharge", SendMessageOptions.DontRequireReceiver);
									if (this.currentWeapon != 0)
									{
										this.SwitchWeapon(0);
									}
								}
								this.shootCooldown = 1f;
								this.shootingForCoin = true;
								this.aboutToShoot = true;
								Object.Instantiate<GameObject>(this.gunFlash, this.aimAtTarget[1].transform.position, Quaternion.LookRotation(this.target.position - this.aimAtTarget[1].transform.position)).transform.localScale *= 20f;
								base.Invoke("ShootWeapon", 0.4f / this.eid.totalSpeedModifier);
							}
						}
					}
					else
					{
						if (this.shootingForCoin && this.aboutToShoot)
						{
							base.CancelInvoke("ShootWeapon");
						}
						this.shootingForCoin = false;
						this.overrideTarget = null;
					}
				}
				else if (this.overrideTarget)
				{
					if (this.shootingForCoin && this.aboutToShoot)
					{
						base.CancelInvoke("ShootWeapon");
					}
					this.shootingForCoin = false;
					this.overrideTarget = null;
				}
				else
				{
					this.shootingForCoin = false;
				}
			}
			else if (this.overrideTarget && this.enraged)
			{
				if (this.shootingForCoin && this.aboutToShoot)
				{
					base.CancelInvoke("ShootWeapon");
				}
				this.overrideTarget = null;
				this.shootingForCoin = false;
			}
			if (this.secondEncounter && (this.coins.Count == 0 || (this.aboutToShoot && this.shootingForCoin)))
			{
				if (this.difficulty > 3)
				{
					this.coinsInSightCooldown = 0f;
				}
				else
				{
					switch (this.difficulty)
					{
					case 0:
						this.coinsInSightCooldown = 0.8f;
						break;
					case 1:
						this.coinsInSightCooldown = 0.6f;
						break;
					case 2:
						this.coinsInSightCooldown = 0.4f;
						break;
					case 3:
						this.coinsInSightCooldown = 0.2f;
						break;
					}
				}
			}
			if (this.shootCooldown > 0f)
			{
				if (this.cowardPattern)
				{
					this.shootCooldown = Mathf.MoveTowards(this.shootCooldown, 0f, Time.deltaTime * num4 * 0.5f * this.eid.totalSpeedModifier);
				}
				else
				{
					this.shootCooldown = Mathf.MoveTowards(this.shootCooldown, 0f, Time.deltaTime * num4 * this.eid.totalSpeedModifier);
				}
			}
			else if (this.aiming && (!this.nma || this.playerInSight))
			{
				if (!this.aboutToShoot)
				{
					if ((this.weapons.Length < 2 && Vector3.Distance(this.target.position, base.transform.position) > 15f) || Vector3.Distance(this.target.position, base.transform.position) > 25f)
					{
						this.SwitchWeapon(0);
					}
					else if (this.weapons.Length > 2 && Vector3.Distance(this.target.position, base.transform.position) > 15f)
					{
						if (this.eid.stuckMagnets.Count <= 0)
						{
							this.SwitchWeapon(2);
						}
						else
						{
							this.SwitchWeapon(0);
						}
					}
					else
					{
						this.SwitchWeapon(1);
					}
				}
				if (!Physics.Raycast(base.transform.position + Vector3.up * 2f, this.target.position - base.transform.position, out this.rhit, Vector3.Distance(base.transform.position, this.target.position), LayerMaskDefaults.Get(LMD.Environment)))
				{
					if (this.altShootCooldown <= 0f || (this.distancePatience >= 8f && this.currentWeapon == 0 && !this.dontEnrage))
					{
						if (this.currentWeapon == 0)
						{
							if (this.weapons.Length == 1)
							{
								this.aimAtGround = true;
							}
							this.predictAmount = 0.15f / this.eid.totalSpeedModifier;
						}
						else
						{
							this.aimAtGround = true;
							if (this.currentWeapon == 1 || this.difficulty > 2)
							{
								this.predictAmount = 0.25f / this.eid.totalSpeedModifier;
							}
							else
							{
								this.predictAmount = -0.25f / this.eid.totalSpeedModifier;
							}
						}
						if (this.difficulty > 2)
						{
							this.shootCooldown = Random.Range(1f, 2f);
						}
						else
						{
							this.shootCooldown = 2f;
						}
						this.altShootCooldown = 5f;
						this.aboutToShoot = true;
						if (!this.secondEncounter || Vector3.Distance(this.target.position, base.transform.position) < 8f || Random.Range(0f, 1f) < 0.5f || this.enraged)
						{
							this.chargingAlt = true;
							this.weapons[this.currentWeapon].transform.GetChild(0).SendMessage("PrepareAltFire");
							if (this.difficulty >= 2)
							{
								base.Invoke("AltShootWeapon", 1f / this.eid.totalSpeedModifier);
							}
							else if (this.difficulty == 1)
							{
								base.Invoke("AltShootWeapon", 1.25f / this.eid.totalSpeedModifier);
							}
							else
							{
								base.Invoke("AltShootWeapon", 1.5f / this.eid.totalSpeedModifier);
							}
						}
						else
						{
							this.SwitchWeapon(0);
							if (this.difficulty >= 2)
							{
								this.coinsToThrow = 3;
							}
							else
							{
								this.coinsToThrow = 1;
							}
							this.ThrowCoins();
						}
					}
					else
					{
						if (this.currentWeapon == 0)
						{
							if (this.distancePatience >= 4f)
							{
								this.shootCooldown = 1f;
							}
							if (this.difficulty > 2)
							{
								this.shootCooldown = Random.Range(1.5f, 2f);
							}
							else
							{
								this.shootCooldown = 2f;
							}
						}
						else
						{
							if (this.currentWeapon == 1 || this.difficulty > 2)
							{
								this.predictAmount = 0.15f / this.eid.totalSpeedModifier;
							}
							else
							{
								this.predictAmount = -0.25f / this.eid.totalSpeedModifier;
							}
							if (this.difficulty > 2)
							{
								this.shootCooldown = Random.Range(1.5f, 2f);
							}
							else
							{
								this.shootCooldown = 2f;
							}
						}
						this.weapons[this.currentWeapon].transform.GetChild(0).SendMessage("PrepareFire");
						this.aboutToShoot = true;
						if (this.currentWeapon == 0)
						{
							Object.Instantiate<GameObject>(this.gunFlash, this.aimAtTarget[this.aimAtTarget.Length - 1].transform.position, Quaternion.LookRotation(this.target.position - this.aimAtTarget[this.aimAtTarget.Length - 1].transform.position)).transform.localScale *= 20f;
							this.shootingForCoin = false;
							if (this.difficulty >= 2)
							{
								base.Invoke("ShootWeapon", 0.75f / this.eid.totalSpeedModifier);
							}
							if (this.difficulty >= 1)
							{
								base.Invoke("ShootWeapon", 0.95f / this.eid.totalSpeedModifier);
							}
							base.Invoke("ShootWeapon", 1.15f / this.eid.totalSpeedModifier);
						}
						else if (this.difficulty >= 2)
						{
							base.Invoke("ShootWeapon", 0.75f / this.eid.totalSpeedModifier);
						}
						else if (this.difficulty == 1)
						{
							base.Invoke("ShootWeapon", 1f / this.eid.totalSpeedModifier);
						}
						else
						{
							base.Invoke("ShootWeapon", 1.25f / this.eid.totalSpeedModifier);
						}
					}
				}
				else if (this.altShootCooldown <= 0f && this.rhit.transform != null && this.rhit.transform.gameObject.CompareTag("Breakable"))
				{
					this.predictAmount = 0f;
					if (!this.alwaysAimAtGround)
					{
						this.aimAtGround = false;
					}
					if (this.distancePatience >= 4f)
					{
						this.shootCooldown = 1f;
					}
					else if (this.difficulty > 2)
					{
						this.shootCooldown = Random.Range(1f, 2f);
					}
					else
					{
						this.shootCooldown = 2f;
					}
					this.altShootCooldown = 5f;
					this.weapons[this.currentWeapon].transform.GetChild(0).SendMessage("PrepareAltFire");
					this.aboutToShoot = true;
					this.chargingAlt = true;
					base.Invoke("AltShootWeapon", 1f / this.eid.totalSpeedModifier);
				}
			}
			if (this.eid)
			{
				if (this.eid.drillers.Count > 0)
				{
					this.slowMode = true;
					this.drilled = true;
				}
				else if (this.drilled)
				{
					this.slowMode = false;
					this.drilled = false;
				}
			}
		}
		else if (this.inIntro)
		{
			if (this.gc.onGround)
			{
				GameObject gameObject = null;
				if (this.longIntro)
				{
					this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
					if (!this.introHitGround)
					{
						if (this.eid.enemyType != EnemyType.BigJohnator)
						{
							this.anim.SetTrigger("Intro");
						}
						this.introHitGround = true;
						if (this.anim.layerCount > 1)
						{
							this.anim.SetLayerWeight(1, 0f);
							this.anim.SetLayerWeight(2, 0f);
						}
						gameObject = Object.Instantiate<GameObject>(this.shockwave, base.transform.position, Quaternion.identity);
					}
				}
				else
				{
					this.inIntro = false;
					this.active = true;
					if (this.bhb)
					{
						this.bhb.enabled = true;
					}
					gameObject = Object.Instantiate<GameObject>(this.shockwave, base.transform.position, Quaternion.identity);
				}
				PhysicalShockwave physicalShockwave;
				if (gameObject && gameObject.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
				{
					physicalShockwave.enemyType = EnemyType.V2;
				}
			}
			if (this.staringAtPlayer)
			{
				this.targetPos = new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z);
				this.targetRot = Quaternion.LookRotation(this.targetPos - base.transform.position, Vector3.up);
				this.anim.transform.rotation = Quaternion.RotateTowards(this.anim.transform.rotation, this.targetRot, Time.deltaTime * 10f * Quaternion.Angle(this.anim.transform.rotation, this.targetRot));
			}
		}
		if (this.mac.health <= this.knockOutHealth && this.knockOutHealth != 0f && this.firstPhase)
		{
			this.firstPhase = false;
			this.KnockedOut("KnockedDown");
			this.eid.totalDamageTakenMultiplier = 0f;
		}
		if (this.bhb)
		{
			if (!this.enraged)
			{
				this.bhb.UpdateSecondaryBar(this.distancePatience / 12f);
			}
			else
			{
				this.bhb.UpdateSecondaryBar((this.distancePatience - 10f) / 2f);
			}
			if (this.enraged)
			{
				this.flashTimer = Mathf.MoveTowards(this.flashTimer, 1f, Time.deltaTime * 5f);
				if (this.flashTimer < 0.5f)
				{
					this.bhb.SetSecondaryBarColor(Color.red);
				}
				else
				{
					this.bhb.SetSecondaryBarColor(Color.black);
				}
				if (this.flashTimer >= 1f)
				{
					this.flashTimer = 0f;
					return;
				}
			}
			else
			{
				if (this.distancePatience < 4f)
				{
					this.bhb.SetSecondaryBarColor(Color.green);
					return;
				}
				if (this.distancePatience < 8f)
				{
					this.bhb.SetSecondaryBarColor(Color.yellow);
					return;
				}
				this.bhb.SetSecondaryBarColor(new Color(1f, 0.35f, 0f));
			}
		}
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000DF30C File Offset: 0x000DD50C
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.1f);
		this.coins = MonoSingleton<CoinList>.Instance.revolverCoinsList;
	}

	// Token: 0x06001AF1 RID: 6897 RVA: 0x000DF330 File Offset: 0x000DD530
	private void ThrowCoins()
	{
		if (this.coinsToThrow == 0)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.coin, base.transform.position, base.transform.rotation);
		Rigidbody rigidbody;
		if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody.AddForce((this.target.position - this.anim.transform.position).normalized * 20f + Vector3.up * 30f, ForceMode.VelocityChange);
		}
		Coin coin;
		if (gameObject.TryGetComponent<Coin>(out coin))
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(coin.flash, coin.transform.position, MonoSingleton<CameraController>.Instance.transform.rotation);
			gameObject2.transform.localScale *= 2f;
			gameObject2.transform.SetParent(gameObject.transform, true);
		}
		this.coinsToThrow--;
		if (this.coinsToThrow > 0)
		{
			base.Invoke("ThrowCoins", 0.2f / this.eid.totalSpeedModifier);
			return;
		}
		this.aboutToShoot = false;
	}

	// Token: 0x06001AF2 RID: 6898 RVA: 0x000DF458 File Offset: 0x000DD658
	private void FixedUpdate()
	{
		if (this.escaping && this.nma && this.nma.isOnNavMesh)
		{
			this.rb.isKinematic = true;
			this.nma.SetDestination(this.escapeTarget.position);
			return;
		}
		if (this.escaping)
		{
			this.rb.isKinematic = false;
			this.targetPos = new Vector3(this.escapeTarget.position.x, base.transform.position.y, this.escapeTarget.position.z);
			if (Vector3.Distance(base.transform.position, this.targetPos) > 8f || (this.escapeTarget.position.y < base.transform.position.y + 5f && Vector3.Distance(base.transform.position, this.escapeTarget.position) > 1f))
			{
				this.aiming = false;
				this.inPattern = false;
				base.transform.LookAt(this.targetPos);
				this.anim.transform.LookAt(this.targetPos);
				this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed);
			}
			else
			{
				if (!this.jumping && this.gc.onGround && !this.slideOnly)
				{
					this.Jump();
				}
				this.rb.velocity = this.targetPos - base.transform.position + Vector3.up * 50f;
				base.GetComponent<Collider>().enabled = false;
			}
			if (base.transform.position.y > this.escapeTarget.position.y - 20f && this.spawnOnDeath != null)
			{
				this.spawnOnDeath.SetActive(true);
				this.spawnOnDeath.transform.position = base.transform.position;
				this.spawnOnDeath = null;
				return;
			}
		}
		else if (this.active && (this.inPattern || this.dodging))
		{
			if (this.target == null)
			{
				return;
			}
			if (this.nma && Physics.Raycast(base.transform.position + Vector3.up, this.target.position - (base.transform.position + Vector3.up), Vector3.Distance(this.target.position, base.transform.position + Vector3.up), LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.playerInSight = false;
			}
			else
			{
				this.playerInSight = true;
			}
			if (this.running)
			{
				this.Move();
			}
		}
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000DF784 File Offset: 0x000DD984
	private void ShootWeapon()
	{
		if (!this.aiming)
		{
			return;
		}
		this.shootingForCoin = false;
		IEnemyWeapon component = this.weapons[this.currentWeapon].transform.GetChild(0).GetComponent<IEnemyWeapon>();
		if (component != null)
		{
			component.UpdateTarget(this.target);
		}
		if (component != null)
		{
			component.Fire();
		}
		this.aboutToShoot = false;
		this.predictAmount = 0f;
		if (!this.alwaysAimAtGround)
		{
			this.aimAtGround = false;
		}
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000DF7FC File Offset: 0x000DD9FC
	private void AltShootWeapon()
	{
		if (!this.aiming)
		{
			return;
		}
		IEnemyWeapon component = this.weapons[this.currentWeapon].transform.GetChild(0).GetComponent<IEnemyWeapon>();
		if (component != null)
		{
			component.UpdateTarget(this.target);
		}
		if (component != null)
		{
			component.AltFire();
		}
		this.aboutToShoot = false;
		if (!this.enraged)
		{
			this.predictAmount = 0f;
		}
		if (!this.alwaysAimAtGround)
		{
			this.aimAtGround = false;
		}
		this.chargingAlt = false;
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x000DF87C File Offset: 0x000DDA7C
	private void Move()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (this.nma)
		{
			if (!this.nma.isOnOffMeshLink && (this.dodging || this.sliding || !this.gc.onGround || this.playerInSight))
			{
				this.nma.enabled = false;
			}
			else if (this.nma.isOnNavMesh)
			{
				this.nma.enabled = true;
				this.nma.SetDestination(this.target.position);
				if (this.distancePatience > 4f && !this.enraged)
				{
					this.nma.speed = this.movementSpeed * 1.5f;
					return;
				}
				this.nma.speed = this.movementSpeed;
				return;
			}
		}
		this.rb.isKinematic = false;
		if (this.dodging)
		{
			this.rb.velocity = new Vector3(base.transform.forward.x * (this.movementSpeed * 5f * this.dodgeLeft), 0f, base.transform.forward.z * (this.movementSpeed * 5f * this.dodgeLeft));
			return;
		}
		if (this.sliding)
		{
			if (this.slideOnly)
			{
				Vector3 vector = this.target.position + (base.transform.position - this.target.position).normalized * 10f;
				Vector3 normalized = new Vector3(vector.x - base.transform.position.x, 0f, vector.z - base.transform.position.z).normalized;
				float num = (float)this.difficulty;
				this.rb.velocity = Vector3.MoveTowards(this.rb.velocity, normalized * this.movementSpeed * Mathf.Max(1f, num / 1.75f), Time.fixedDeltaTime * 75f);
				Quaternion quaternion = Quaternion.LookRotation(this.forceSlideDirection, Vector3.up);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * 360f);
				if (this.difficulty >= 2)
				{
					if (Vector3.Distance(this.target.position, base.transform.position) < 8f)
					{
						this.circleTimer = Mathf.MoveTowards(this.circleTimer, 1f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
					else
					{
						this.circleTimer = Mathf.MoveTowards(this.circleTimer, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					}
					if (this.circleTimer >= 1f)
					{
						this.circleTimer = 0.65f;
						this.ForceDodge((base.transform.position - this.targetPos).normalized + base.transform.right * Random.Range(-1f, 1f));
					}
				}
				return;
			}
			if (this.distancePatience > 4f && !this.enraged)
			{
				this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed * 2f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * 3f);
				return;
			}
			this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed * 2f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * 2f);
			return;
		}
		else
		{
			if (!this.gc.onGround)
			{
				bool flag = Vector3.Distance(base.transform.position, this.targetPos) < 10f && this.difficulty <= 2;
				Vector3 vector2;
				if (this.slowMode || (flag && MonoSingleton<NewMovement>.Instance.hp <= 33 && !this.enraged))
				{
					if (this.distancePatience < 4f)
					{
						vector2 = new Vector3(base.transform.forward.x * this.movementSpeed * Time.deltaTime * 1.25f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * Time.deltaTime * 1.25f);
					}
					else
					{
						vector2 = new Vector3(base.transform.forward.x * this.movementSpeed * Time.deltaTime * 1.25f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * Time.deltaTime * 2f);
					}
				}
				else if (flag && this.distancePatience < 4f)
				{
					vector2 = new Vector3(base.transform.forward.x * this.movementSpeed * Time.deltaTime * 1.25f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * Time.deltaTime * 2f);
				}
				else if (this.distancePatience > 4f && !this.enraged)
				{
					vector2 = new Vector3(base.transform.forward.x * this.movementSpeed * Time.deltaTime * 3f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * Time.deltaTime * 3f);
				}
				else
				{
					vector2 = new Vector3(base.transform.forward.x * this.movementSpeed * Time.deltaTime * 2.5f, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * Time.deltaTime * 2.5f);
				}
				Vector3 zero = Vector3.zero;
				if ((vector2.x > 0f && this.rb.velocity.x < vector2.x) || (vector2.x < 0f && this.rb.velocity.x > vector2.x))
				{
					zero.x = vector2.x;
				}
				else
				{
					zero.x = 0f;
				}
				if ((vector2.z > 0f && this.rb.velocity.z < vector2.z) || (vector2.z < 0f && this.rb.velocity.z > vector2.z))
				{
					zero.z = vector2.z;
				}
				else
				{
					zero.z = 0f;
				}
				this.rb.AddForce(zero.normalized * this.airAcceleration);
				return;
			}
			if (this.distancePatience > 4f && !this.enraged)
			{
				this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * 1.5f);
				return;
			}
			float num2 = 1f;
			if (MonoSingleton<NewMovement>.Instance.hp <= 33 && this.difficulty <= 3)
			{
				num2 -= 0.1f;
			}
			if (Vector3.Distance(base.transform.position, this.targetPos) < 10f && this.difficulty <= 2 && this.distancePatience < 4f)
			{
				num2 -= 0.1f;
			}
			if (this.slowMode)
			{
				this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * num2 * 0.75f);
				return;
			}
			this.rb.velocity = new Vector3(base.transform.forward.x * this.movementSpeed, this.rb.velocity.y, base.transform.forward.z * this.movementSpeed * num2);
			return;
		}
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000E018C File Offset: 0x000DE38C
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		if ((this.active && this.aiming) || this.escaping)
		{
			if (this.difficulty <= 1)
			{
				this.predictAmount = 0f;
			}
			Vector3 vector = this.target.position;
			Rigidbody rigidbody = this.eid.target.rigidbody;
			if (this.escaping)
			{
				this.predictAmount = 0f;
				vector = this.escapeTarget.position;
			}
			else if (this.overrideTarget)
			{
				this.predictAmount = 0.05f * (Vector3.Distance(this.overrideTarget.position, base.transform.position) / 20f);
				vector = this.overrideTarget.position;
				rigidbody = this.overrideTargetRb;
			}
			else if (Vector3.Distance(base.transform.position, this.targetPos) < 8f)
			{
				this.predictAmount *= 0.2f;
			}
			try
			{
				if (this.aimAtTarget.Length == 1 && this.aimAtGround)
				{
					this.aimAtTarget[0].LookAt(vector + Vector3.down * 2.5f + rigidbody.velocity * (Vector3.Distance(vector, this.aimAtTarget[0].position) * (this.predictAmount / 10f)));
				}
				else
				{
					this.aimAtTarget[0].LookAt(vector + rigidbody.velocity * (Vector3.Distance(vector, this.aimAtTarget[0].position) * (this.predictAmount / 10f)));
				}
			}
			catch
			{
				throw;
			}
			this.aimAtTarget[0].Rotate(Vector3.right, 10f, Space.Self);
			if (this.aimAtTarget.Length > 1)
			{
				Quaternion quaternion;
				if (this.aimAtGround)
				{
					quaternion = Quaternion.LookRotation(rigidbody.transform.position + rigidbody.velocity * this.predictAmount - this.aimAtTarget[1].position, Vector3.up);
				}
				else
				{
					quaternion = Quaternion.LookRotation(vector + rigidbody.velocity * this.predictAmount - this.aimAtTarget[1].position, Vector3.up);
				}
				quaternion = Quaternion.Euler(quaternion.eulerAngles.x + 90f, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
				this.aimAtTarget[1].rotation = quaternion;
				this.aimAtTarget[1].Rotate(Vector3.up, 180f, Space.Self);
			}
		}
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000E0440 File Offset: 0x000DE640
	private void Jump()
	{
		this.jumping = true;
		if (this.anim.layerCount > 1)
		{
			this.anim.SetLayerWeight(1, 1f);
			this.anim.SetLayerWeight(2, 0f);
		}
		if (this.eid.enemyType != EnemyType.BigJohnator)
		{
			this.anim.SetTrigger("Jump");
		}
		base.Invoke("NotJumping", 0.25f);
		bool flag = this.slowMode || (Vector3.Distance(base.transform.position, this.targetPos) < 10f && this.difficulty <= 2 && MonoSingleton<NewMovement>.Instance.hp <= 33 && !this.enraged);
		if (this.sliding)
		{
			Object.Instantiate<GameObject>(this.jumpSound, base.transform.position, Quaternion.identity);
			if (flag)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * 1500f);
			}
			else
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * 1500f * 2f);
			}
			this.StopSlide();
			return;
		}
		if (this.dodging)
		{
			Object.Instantiate<GameObject>(this.dashJumpSound);
			if (flag)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * 1500f * 0.75f);
				return;
			}
			this.rb.AddForce(Vector3.up * this.jumpPower * 1500f * 1.5f);
			return;
		}
		else
		{
			Object.Instantiate<GameObject>(this.jumpSound, base.transform.position, Quaternion.identity);
			if (flag)
			{
				this.rb.AddForce(Vector3.up * this.jumpPower * 1500f * 1.25f);
				return;
			}
			this.rb.AddForce(Vector3.up * this.jumpPower * 1500f * 2.5f);
			return;
		}
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000E0678 File Offset: 0x000DE878
	private void WallJump()
	{
		if (this.sliding)
		{
			this.StopSlide();
		}
		this.jumping = true;
		base.Invoke("NotJumping", 0.25f);
		Object.Instantiate<GameObject>(this.jumpSound, base.transform.position, Quaternion.identity).GetComponent<AudioSource>().pitch = 2f;
		Vector3 vector = base.transform.position - this.wc.ClosestPoint();
		this.rb.velocity = Vector3.zero;
		Vector3 vector2 = new Vector3(vector.normalized.x * 3f, 0.75f, vector.normalized.z * 3f);
		this.CheckPattern();
		if (this.currentPattern == 0)
		{
			Quaternion rotation = this.anim.transform.rotation;
			Vector3 vector3 = new Vector3(vector.normalized.x, 0f, vector.normalized.z);
			base.transform.rotation = Quaternion.LookRotation(vector3, Vector3.up);
			this.anim.transform.rotation = rotation;
			this.ChangeDirection((float)Random.Range(-90, 90));
		}
		else if (this.currentPattern == 1)
		{
			if (this.pattern1direction < 0)
			{
				this.pattern1direction = 1;
			}
			else
			{
				this.pattern1direction = -1;
			}
		}
		else
		{
			Quaternion rotation2 = this.anim.transform.rotation;
			base.transform.LookAt(this.targetPos);
			this.anim.transform.rotation = rotation2;
		}
		float num = 2000f;
		bool flag = this.slowMode || (Vector3.Distance(base.transform.position, this.targetPos) < 10f && this.difficulty <= 2 && MonoSingleton<NewMovement>.Instance.hp <= 33 && !this.enraged);
		if (this.difficulty == 1 || flag)
		{
			num = 1000f;
		}
		else if (this.difficulty == 0)
		{
			num = 500f;
		}
		this.rb.AddForce(vector2 * this.wallJumpPower * num);
	}

	// Token: 0x06001AF9 RID: 6905 RVA: 0x000E089C File Offset: 0x000DEA9C
	private void CheckPattern()
	{
		if (this.patternCooldown <= 0f && this.distancePatience < 4f && !this.cowardPattern)
		{
			int num = this.currentPattern;
			this.currentPattern = Random.Range(0, 3);
			if (num == this.currentPattern)
			{
				this.patternCooldown = Random.Range(0.5f, 1f);
				if (this.currentPattern == 1)
				{
					this.circleTimer += 1f;
				}
			}
			else
			{
				this.patternCooldown = (float)Random.Range(2, 5);
				this.SwitchPattern(this.currentPattern);
			}
			if (this.currentPattern == 1 && Random.Range(0f, 1f) > 0.5f)
			{
				this.pattern1direction = -1;
				return;
			}
			this.pattern1direction = 1;
		}
	}

	// Token: 0x06001AFA RID: 6906 RVA: 0x000E096C File Offset: 0x000DEB6C
	private void ChangeDirection(float degrees)
	{
		Quaternion rotation = this.anim.transform.rotation;
		base.transform.Rotate(base.transform.up, degrees, Space.World);
		this.anim.transform.rotation = rotation;
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x000E09B4 File Offset: 0x000DEBB4
	public void Dodge(Transform projectile)
	{
		if (this.target == null)
		{
			return;
		}
		if (this.active && this.dodgeLeft <= 0f && !this.chargingAlt && Vector3.Distance(base.transform.position, this.target.position) > 15f)
		{
			if (this.sliding && !this.slideOnly)
			{
				this.StopSlide();
			}
			if (this.dodgeCooldown >= (float)(6 - this.difficulty))
			{
				this.dodgeCooldown -= (float)(6 - this.difficulty);
				this.dodgeLeft = 1f;
				this.dodging = true;
				this.eid.hookIgnore = true;
				this.inPattern = false;
				Object.Instantiate<GameObject>(this.dodgeEffect, base.transform.position + Vector3.up * 2f, base.transform.rotation);
				Vector3 vector = new Vector3(base.transform.position.x - projectile.position.x, 0f, base.transform.position.z - projectile.position.z);
				if (this.currentPattern == 2)
				{
					vector = vector.normalized + (this.targetPos - base.transform.position).normalized;
				}
				base.transform.LookAt(base.transform.position + vector);
				if (Random.Range(0f, 1f) > 0.5f)
				{
					this.ChangeDirection(90f);
				}
				else
				{
					this.ChangeDirection(-90f);
				}
				if (!this.slideOnly && this.eid.enemyType != EnemyType.BigJohnator)
				{
					this.anim.SetTrigger("Jump");
					return;
				}
			}
			else if (this.gc.onGround && !this.jumping && !this.slideOnly)
			{
				float num;
				if (this.difficulty > 2)
				{
					num = Random.Range(0f, 2f);
				}
				else
				{
					num = Random.Range(0f, 3f);
				}
				if (num < 1f)
				{
					if (num > 0.75f || this.cowardPattern)
					{
						this.Jump();
						return;
					}
					this.Slide();
				}
			}
		}
	}

	// Token: 0x06001AFC RID: 6908 RVA: 0x000E0C0C File Offset: 0x000DEE0C
	public void ForceDodge(Vector3 direction)
	{
		if (this.sliding && !this.slideOnly)
		{
			this.StopSlide();
		}
		this.dodgeLeft = 1f;
		this.dodging = true;
		this.eid.hookIgnore = true;
		this.inPattern = false;
		Object.Instantiate<GameObject>(this.dodgeEffect, base.transform.position + Vector3.up * 2f, base.transform.rotation);
		direction = new Vector3(direction.x, 0f, direction.z);
		base.transform.LookAt(base.transform.position + direction);
		if (!this.slideOnly && this.eid.enemyType != EnemyType.BigJohnator)
		{
			this.anim.SetTrigger("Jump");
		}
	}

	// Token: 0x06001AFD RID: 6909 RVA: 0x000E0CE5 File Offset: 0x000DEEE5
	private void NotJumping()
	{
		this.jumping = false;
	}

	// Token: 0x06001AFE RID: 6910 RVA: 0x000E0CEE File Offset: 0x000DEEEE
	private void Slide()
	{
		this.anim.SetBool("Sliding", true);
		this.sliding = true;
		this.slideEffect.SetActive(true);
		this.slideStopTimer = 0.2f;
	}

	// Token: 0x06001AFF RID: 6911 RVA: 0x000E0D1F File Offset: 0x000DEF1F
	private void StopSlide()
	{
		this.sliding = false;
		this.anim.SetBool("Sliding", false);
		this.slideEffect.SetActive(false);
		this.CheckPattern();
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000E0D4C File Offset: 0x000DEF4C
	private void SwitchWeapon(int weapon)
	{
		if (this.currentWeapon != weapon && this.weapons.Length > weapon)
		{
			this.currentWeapon = weapon;
			GameObject[] array = this.weapons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			this.weapons[weapon].SetActive(true);
		}
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000E0DA0 File Offset: 0x000DEFA0
	public void SwitchPattern(int pattern)
	{
		if (this.currentWingChangeEffect != null)
		{
			Object.Destroy(this.currentWingChangeEffect);
		}
		foreach (EnemySimplifier enemySimplifier in this.ensims)
		{
			if (enemySimplifier.matList != null && enemySimplifier.matList.Length > 1)
			{
				enemySimplifier.matList[1].mainTexture = this.wingTextures[pattern];
			}
		}
		this.currentWingChangeEffect = Object.Instantiate<GameObject>(this.wingChangeEffect, base.transform.position + Vector3.up * 2f, Quaternion.identity);
		this.currentWingChangeEffect.GetComponent<Light>().color = this.wingColors[pattern];
		foreach (TrailRenderer trailRenderer in this.wingTrails)
		{
			if (trailRenderer)
			{
				trailRenderer.startColor = new Color(this.wingColors[pattern].r, this.wingColors[pattern].g, this.wingColors[pattern].b, 0.5f);
			}
		}
		if (pattern == 0)
		{
			this.currentWingChangeEffect.GetComponent<AudioSource>().pitch = 1.5f;
			return;
		}
		if (pattern != 1)
		{
			return;
		}
		this.currentWingChangeEffect.GetComponent<AudioSource>().pitch = 1.25f;
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000E0EF4 File Offset: 0x000DF0F4
	public void Die()
	{
		if (this.dontDie && !this.dead)
		{
			this.dead = true;
			if (!this.bossVersion)
			{
				foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in base.GetComponentsInChildren<EnemyIdentifierIdentifier>())
				{
					this.eid.DeliverDamage(enemyIdentifierIdentifier.gameObject, Vector3.zero, enemyIdentifierIdentifier.transform.position, 10f, false, 0f, null, false, false);
				}
				base.gameObject.SetActive(false);
				Object.Destroy(base.gameObject);
				return;
			}
			MonoSingleton<MusicManager>.Instance.off = true;
			if (this.secondEncounter)
			{
				this.KnockedOut("Flailing");
				return;
			}
			this.KnockedOut("KnockedDown");
		}
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000E0FB0 File Offset: 0x000DF1B0
	public void KnockedOut(string triggerName = "KnockedDown")
	{
		this.active = false;
		this.inPattern = false;
		this.aiming = false;
		this.inIntro = false;
		this.anim.transform.LookAt(new Vector3(this.target.position.x, this.anim.transform.position.y, this.target.position.z));
		if (this.eid.enemyType != EnemyType.BigJohnator)
		{
			this.anim.SetTrigger(triggerName);
			this.anim.SetLayerWeight(1, 0f);
			this.anim.SetLayerWeight(2, 0f);
		}
		if (!this.secondEncounter || !this.dead)
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
		else
		{
			this.rb.constraints = RigidbodyConstraints.None;
			this.rb.velocity = new Vector3(0f, 15f, 0f);
			this.rb.AddTorque(-180f, (float)Random.Range(-35, 35), (float)Random.Range(-35, 35), ForceMode.VelocityChange);
			this.rb.useGravity = false;
		}
		if (this.KoScream)
		{
			Object.Instantiate<GameObject>(this.KoScream, base.transform.position, Quaternion.identity);
		}
		this.weapons[this.currentWeapon].transform.GetChild(0).SendMessage("CancelAltCharge", SendMessageOptions.DontRequireReceiver);
		this.eidids = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
		EnemyIdentifierIdentifier[] array = this.eidids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Collider>().enabled = false;
		}
		this.onKnockout.Invoke("");
		this.UnEnrage();
		if (this.nma)
		{
			this.mac.StopKnockBack();
			this.nma.speed = 25f;
		}
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x000E11B4 File Offset: 0x000DF3B4
	public void Undie()
	{
		this.active = true;
		this.inPattern = true;
		this.aiming = true;
		this.eid.totalDamageTakenMultiplier = 1f;
		EnemyIdentifierIdentifier[] array = this.eidids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Collider>().enabled = true;
		}
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x000E120C File Offset: 0x000DF40C
	public void IntroEnd()
	{
		this.inIntro = false;
		this.active = true;
		this.staringAtPlayer = false;
		EnemyIdentifierIdentifier[] array = this.eidids;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Collider>().enabled = true;
		}
		if (this.bhb)
		{
			this.bhb.enabled = true;
		}
		this.longIntro = false;
		MonoSingleton<StatueIntroChecker>.Instance.beenSeen = true;
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x000E127C File Offset: 0x000DF47C
	public void StareAtPlayer()
	{
		this.staringAtPlayer = true;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x000E1288 File Offset: 0x000DF488
	public void BeginEscape()
	{
		this.escaping = true;
		this.anim.SetLayerWeight(1, 1f);
		this.anim.SetLayerWeight(2, 0f);
		this.anim.SetBool("RunningBack", false);
		this.anim.SetBool("InAir", false);
		base.transform.LookAt(new Vector3(this.target.position.x, base.transform.position.y, this.target.position.z));
		this.anim.transform.LookAt(new Vector3(this.target.position.x, this.anim.transform.position.y, this.target.position.z));
		if (this.gc.onGround && this.nma && !this.mac.knockedBack)
		{
			this.nma.enabled = true;
		}
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x000E139D File Offset: 0x000DF59D
	public void InstaEnrage()
	{
		this.distancePatience = 12f;
		this.Enrage("STOP HITTING YOURSELF");
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000E13B5 File Offset: 0x000DF5B5
	public void Enrage()
	{
		this.Enrage("");
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000E13C4 File Offset: 0x000DF5C4
	public void Enrage(string enrageName)
	{
		if (this.enraged)
		{
			return;
		}
		this.enraged = true;
		this.currentEnrageEffect = Object.Instantiate<GameObject>(this.enrageEffect, this.mac.chest.transform.position, base.transform.rotation);
		this.currentEnrageEffect.transform.SetParent(this.mac.chest.transform, true);
		EnrageEffect enrageEffect;
		if (!string.IsNullOrEmpty(enrageName) && this.currentEnrageEffect.TryGetComponent<EnrageEffect>(out enrageEffect))
		{
			enrageEffect.styleNameOverride = enrageName;
		}
		if (!this.currentEnrageEffect.activeSelf)
		{
			this.currentEnrageEffect.SetActive(true);
		}
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = true;
		}
		this.movementSpeed = this.originalMovementSpeed * 2f;
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000E149C File Offset: 0x000DF69C
	public void UnEnrage()
	{
		if (!this.enraged)
		{
			return;
		}
		if (this.currentEnrageEffect != null)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
		this.enraged = false;
		EnemySimplifier[] array = this.ensims;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enraged = false;
		}
		this.movementSpeed = this.originalMovementSpeed;
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06001B0C RID: 6924 RVA: 0x000E14FC File Offset: 0x000DF6FC
	public bool isEnraged
	{
		get
		{
			return this.enraged;
		}
	}

	// Token: 0x06001B0D RID: 6925 RVA: 0x000E1504 File Offset: 0x000DF704
	public void SlideOnly(bool value)
	{
		this.slideOnly = value;
		if (value)
		{
			this.rb.constraints = (RigidbodyConstraints)116;
			this.anim.Play("Slide", 0, 0f);
			return;
		}
		this.rb.constraints = RigidbodyConstraints.FreezeRotation;
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06001B0E RID: 6926 RVA: 0x000E1541 File Offset: 0x000DF741
	public string alterKey
	{
		get
		{
			return "v2";
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06001B0F RID: 6927 RVA: 0x000E1548 File Offset: 0x000DF748
	public string alterCategoryName
	{
		get
		{
			return "V2";
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06001B10 RID: 6928 RVA: 0x000E1550 File Offset: 0x000DF750
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

	// Token: 0x040025DE RID: 9694
	private Animator anim;

	// Token: 0x040025DF RID: 9695
	private Transform overrideTarget;

	// Token: 0x040025E0 RID: 9696
	private Rigidbody overrideTargetRb;

	// Token: 0x040025E1 RID: 9697
	private Vector3 targetPos;

	// Token: 0x040025E2 RID: 9698
	private Quaternion targetRot;

	// Token: 0x040025E3 RID: 9699
	public Transform[] aimAtTarget;

	// Token: 0x040025E4 RID: 9700
	private Rigidbody rb;

	// Token: 0x040025E5 RID: 9701
	private NavMeshAgent nma;

	// Token: 0x040025E6 RID: 9702
	private int currentWeapon;

	// Token: 0x040025E7 RID: 9703
	public SkinnedMeshRenderer smr;

	// Token: 0x040025E8 RID: 9704
	private EnemySimplifier[] ensims;

	// Token: 0x040025E9 RID: 9705
	public Texture[] wingTextures;

	// Token: 0x040025EA RID: 9706
	public GameObject wingChangeEffect;

	// Token: 0x040025EB RID: 9707
	public Color[] wingColors;

	// Token: 0x040025EC RID: 9708
	public GameObject[] weapons;

	// Token: 0x040025ED RID: 9709
	private GameObject currentWingChangeEffect;

	// Token: 0x040025EE RID: 9710
	private TrailRenderer[] wingTrails;

	// Token: 0x040025EF RID: 9711
	private DragBehind[] drags;

	// Token: 0x040025F0 RID: 9712
	private int currentPattern;

	// Token: 0x040025F1 RID: 9713
	private bool inPattern;

	// Token: 0x040025F2 RID: 9714
	public GroundCheckEnemy gc;

	// Token: 0x040025F3 RID: 9715
	public GroundCheckEnemy wc;

	// Token: 0x040025F4 RID: 9716
	private int pattern1direction = 1;

	// Token: 0x040025F5 RID: 9717
	public GameObject jumpSound;

	// Token: 0x040025F6 RID: 9718
	public GameObject dashJumpSound;

	// Token: 0x040025F7 RID: 9719
	public bool secondEncounter;

	// Token: 0x040025F8 RID: 9720
	public bool slowMode;

	// Token: 0x040025F9 RID: 9721
	public float movementSpeed;

	// Token: 0x040025FA RID: 9722
	private float originalMovementSpeed;

	// Token: 0x040025FB RID: 9723
	public float jumpPower;

	// Token: 0x040025FC RID: 9724
	public float wallJumpPower;

	// Token: 0x040025FD RID: 9725
	public float airAcceleration;

	// Token: 0x040025FE RID: 9726
	public bool intro;

	// Token: 0x040025FF RID: 9727
	[HideInInspector]
	public bool inIntro;

	// Token: 0x04002600 RID: 9728
	public bool active;

	// Token: 0x04002601 RID: 9729
	private bool running;

	// Token: 0x04002602 RID: 9730
	private bool aiming;

	// Token: 0x04002603 RID: 9731
	private bool sliding;

	// Token: 0x04002604 RID: 9732
	private bool dodging;

	// Token: 0x04002605 RID: 9733
	private bool jumping;

	// Token: 0x04002606 RID: 9734
	private float patternCooldown;

	// Token: 0x04002607 RID: 9735
	private float dodgeCooldown = 3f;

	// Token: 0x04002608 RID: 9736
	private float dodgeLeft;

	// Token: 0x04002609 RID: 9737
	public GameObject dodgeEffect;

	// Token: 0x0400260A RID: 9738
	public GameObject slideEffect;

	// Token: 0x0400260B RID: 9739
	private int difficulty = -1;

	// Token: 0x0400260C RID: 9740
	private float slideStopTimer;

	// Token: 0x0400260D RID: 9741
	private TimeSince randomSlideCheck;

	// Token: 0x0400260E RID: 9742
	private float shootCooldown;

	// Token: 0x0400260F RID: 9743
	private float altShootCooldown;

	// Token: 0x04002610 RID: 9744
	public GameObject gunFlash;

	// Token: 0x04002611 RID: 9745
	public GameObject altFlash;

	// Token: 0x04002612 RID: 9746
	private bool aboutToShoot;

	// Token: 0x04002613 RID: 9747
	private bool chargingAlt;

	// Token: 0x04002614 RID: 9748
	private float predictAmount;

	// Token: 0x04002615 RID: 9749
	private bool aimAtGround;

	// Token: 0x04002616 RID: 9750
	public bool dontDie;

	// Token: 0x04002617 RID: 9751
	public Transform escapeTarget;

	// Token: 0x04002618 RID: 9752
	private bool escaping;

	// Token: 0x04002619 RID: 9753
	private bool dead;

	// Token: 0x0400261A RID: 9754
	public bool longIntro;

	// Token: 0x0400261B RID: 9755
	private bool staringAtPlayer;

	// Token: 0x0400261C RID: 9756
	private bool introHitGround;

	// Token: 0x0400261D RID: 9757
	private EnemyIdentifierIdentifier[] eidids;

	// Token: 0x0400261E RID: 9758
	private BossHealthBar bhb;

	// Token: 0x0400261F RID: 9759
	public GameObject shockwave;

	// Token: 0x04002620 RID: 9760
	public GameObject KoScream;

	// Token: 0x04002621 RID: 9761
	private RaycastHit rhit;

	// Token: 0x04002622 RID: 9762
	private float distancePatience;

	// Token: 0x04002623 RID: 9763
	private bool enraged;

	// Token: 0x04002624 RID: 9764
	public GameObject enrageEffect;

	// Token: 0x04002625 RID: 9765
	private GameObject currentEnrageEffect;

	// Token: 0x04002626 RID: 9766
	private Machine mac;

	// Token: 0x04002627 RID: 9767
	private EnemyIdentifier eid;

	// Token: 0x04002628 RID: 9768
	private bool drilled;

	// Token: 0x04002629 RID: 9769
	private float circleTimer = 5f;

	// Token: 0x0400262A RID: 9770
	public GameObject spawnOnDeath;

	// Token: 0x0400262B RID: 9771
	private bool playerInSight;

	// Token: 0x0400262C RID: 9772
	private int coinsToThrow;

	// Token: 0x0400262D RID: 9773
	private bool shootingForCoin;

	// Token: 0x0400262E RID: 9774
	public GameObject coin;

	// Token: 0x0400262F RID: 9775
	[HideInInspector]
	public bool firstPhase = true;

	// Token: 0x04002630 RID: 9776
	public float knockOutHealth;

	// Token: 0x04002631 RID: 9777
	public bool slideOnly;

	// Token: 0x04002632 RID: 9778
	public bool dontEnrage;

	// Token: 0x04002633 RID: 9779
	public bool alwaysAimAtGround;

	// Token: 0x04002634 RID: 9780
	public Vector3 forceSlideDirection;

	// Token: 0x04002635 RID: 9781
	private bool cowardPattern;

	// Token: 0x04002636 RID: 9782
	public UltrakillEvent onKnockout;

	// Token: 0x04002637 RID: 9783
	private float flashTimer;

	// Token: 0x04002638 RID: 9784
	private List<Coin> coins = new List<Coin>();

	// Token: 0x04002639 RID: 9785
	private bool bossVersion = true;

	// Token: 0x0400263A RID: 9786
	private float coinsInSightCooldown;
}
