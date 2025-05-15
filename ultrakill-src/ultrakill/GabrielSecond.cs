using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class GabrielSecond : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x06000AC0 RID: 2752 RVA: 0x0004CAB8 File Offset: 0x0004ACB8
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.smr = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.voice = base.GetComponent<GabrielVoice>();
		this.col = base.GetComponent<Collider>();
	}

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0004CB19 File Offset: 0x0004AD19
	private void Start()
	{
		this.SetValues();
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x0004CB24 File Offset: 0x0004AD24
	private void SetValues()
	{
		if (this.valuesSet)
		{
			return;
		}
		this.valuesSet = true;
		this.origBody = this.smr.sharedMaterials[0];
		this.origWing = this.smr.sharedMaterials[1];
		this.origWing.SetFloat("_OpacScale", 1f);
		this.rightHandTrail = this.rightHand.GetComponentInChildren<TrailRenderer>();
		this.rightSwingCheck = this.rightHand.GetComponentInChildren<SwingCheck2>();
		this.rightHandGlow = this.rightHand.GetComponentInChildren<MeshRenderer>(true);
		this.leftHandTrail = this.leftHand.GetComponentInChildren<TrailRenderer>();
		this.leftSwingCheck = this.leftHand.GetComponentInChildren<SwingCheck2>();
		this.leftHandGlow = this.leftHand.GetComponentInChildren<MeshRenderer>(true);
		this.environmentMask = LayerMaskDefaults.Get(LMD.Environment);
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		if (this.difficulty >= 3)
		{
			this.burstLength = 3;
		}
		this.UpdateSpeed();
		if (this.enraged)
		{
			this.EnrageNow();
		}
		this.RandomizeDirection();
		BossHealthBar bossHealthBar;
		this.bossVersion = base.TryGetComponent<BossHealthBar>(out bossHealthBar);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x0004CC5F File Offset: 0x0004AE5F
	private void UpdateBuff()
	{
		this.SetValues();
		this.UpdateSpeed();
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0004CC70 File Offset: 0x0004AE70
	private void UpdateSpeed()
	{
		if (this.difficulty <= 1)
		{
			this.anim.speed = ((this.difficulty == 1) ? 0.85f : 0.75f);
		}
		else
		{
			this.anim.speed = 1f;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.defaultAnimSpeed = this.anim.speed;
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0004CCE8 File Offset: 0x0004AEE8
	private void OnDisable()
	{
		base.CancelInvoke();
		if (this.leftSwingCheck)
		{
			this.DamageStopLeft(0);
		}
		if (this.rightSwingCheck)
		{
			this.DamageStopRight(0);
		}
		this.StopAction();
		this.ResetAnimSpeed();
		this.overrideRotation = false;
		this.dashing = false;
		if (this.currentSwords)
		{
			this.currentSwords.SetActive(false);
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0004CD56 File Offset: 0x0004AF56
	private void OnEnable()
	{
		if (this.juggled)
		{
			this.JuggleStop(false);
		}
		if (this.currentSwords)
		{
			this.currentSwords.SetActive(true);
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0004CD80 File Offset: 0x0004AF80
	private void UpdateRigidbodySettings()
	{
		if (this.eid.target == null)
		{
			this.rb.drag = 3f;
			this.rb.angularDrag = 3f;
			return;
		}
		this.rb.drag = 0f;
		this.rb.angularDrag = 0f;
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0004CDDC File Offset: 0x0004AFDC
	private void Update()
	{
		this.UpdateRigidbodySettings();
		if (this.eid.target == null)
		{
			return;
		}
		if (this.active)
		{
			if (this.startCooldown > 0f)
			{
				this.startCooldown = Mathf.MoveTowards(this.startCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (this.secondPhase && this.difficulty >= 3 && !this.currentSwords)
			{
				this.summonedSwordsCooldown = Mathf.MoveTowards(this.summonedSwordsCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
				if (this.summonedSwordsCooldown == 0f && !this.inAction && this.readyTaunt)
				{
					this.summonedSwordsCooldown = 15f;
					this.SpawnSummonedSwordsWindup();
					base.Invoke("SpawnSummonedSwords", 1f / this.eid.totalSpeedModifier);
				}
			}
			if (!this.inAction && this.startCooldown <= 0f)
			{
				if (this.attackCooldown > 0f)
				{
					this.attackCooldown = Mathf.MoveTowards(this.attackCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
					if (this.readyTaunt && this.voice)
					{
						this.voice.Taunt();
						this.readyTaunt = false;
					}
				}
				else if ((this.secondPhase || !this.currentCombinedSwordsThrown) && Physics.Raycast(base.transform.position, this.eid.target.position - base.transform.position, Vector3.Distance(base.transform.position, this.eid.target.position), LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.Teleport(false, false, true, false, false);
				}
				else if (this.currentCombinedSwordsThrown && !this.secondPhase && (this.combinedSwordsCooldown > 0f || Vector3.Distance(base.transform.position, this.combinedSwordsThrown.transform.position) < Vector3.Distance(base.transform.position, this.eid.target.position)))
				{
					this.combinedSwordsCooldown = Mathf.MoveTowards(this.combinedSwordsCooldown, 0f, Time.deltaTime);
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					if (Vector3.Distance(base.transform.position, this.eid.target.position) > 20f)
					{
						flag2 = true;
					}
					else if (Vector3.Distance(base.transform.position, this.eid.target.position) < 5f)
					{
						flag = true;
					}
					float[] array = new float[4];
					int num = -1;
					if (this.previousMove != 0 && !flag)
					{
						array[0] = Random.Range(0f, 1f) + this.moveChanceBonuses[0];
					}
					if (this.previousMove != 1 && !flag)
					{
						array[1] = Random.Range(0f, 1f) + this.moveChanceBonuses[1];
					}
					if (this.previousMove != 2 && !flag2)
					{
						array[2] = Random.Range(0f, 1f) + this.moveChanceBonuses[2];
					}
					if (this.previousMove != 3 && !flag2)
					{
						array[3] = Random.Range(0f, 1f) + this.moveChanceBonuses[3];
					}
					float num2 = 0f;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] > num2)
						{
							num2 = array[i];
							num = i;
						}
					}
					switch (num)
					{
					case 0:
						this.CombineSwords();
						break;
					case 1:
						this.FastComboDash();
						break;
					case 2:
						this.BasicCombo();
						break;
					case 3:
						this.ThrowCombo();
						break;
					}
					this.ResetAnimSpeed();
					this.previousMove = num;
					for (int j = 0; j < array.Length; j++)
					{
						if (j != num)
						{
							this.moveChanceBonuses[j] += 0.25f;
						}
						else
						{
							this.moveChanceBonuses[j] = 0f;
						}
					}
					if (num != 0)
					{
						if (this.burstLength > 1)
						{
							this.burstLength--;
						}
						else
						{
							if (this.difficulty >= 3)
							{
								this.burstLength = 3;
							}
							else
							{
								this.burstLength = 2;
							}
							if (this.difficulty <= 3)
							{
								this.attackCooldown = 3f;
							}
							else
							{
								this.attackCooldown = (float)(5 - this.difficulty);
							}
							this.readyTaunt = true;
						}
					}
				}
			}
			if ((Vector3.Distance(base.transform.position, this.eid.target.position) > 20f || base.transform.position.y > this.eid.target.position.y + 15f || Physics.Raycast(base.transform.position, this.eid.target.position - base.transform.position, Vector3.Distance(base.transform.position, this.eid.target.position), this.environmentMask)) && this.startCooldown <= 0f)
			{
				this.outOfSightTime = Mathf.MoveTowards(this.outOfSightTime, 3f, Time.deltaTime * this.eid.totalSpeedModifier);
				if (this.outOfSightTime >= 3f && !this.inAction)
				{
					this.Teleport(false, false, true, false, false);
				}
			}
			else
			{
				this.outOfSightTime = Mathf.MoveTowards(this.outOfSightTime, 0f, Time.deltaTime * 2f * this.eid.totalSpeedModifier);
			}
			if (!this.stopRotation)
			{
				if (!this.overrideRotation)
				{
					Quaternion quaternion = Quaternion.LookRotation(this.eid.target.position - base.transform.position, Vector3.up);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * (10f * Quaternion.Angle(quaternion, base.transform.rotation) + 2f) * this.eid.totalSpeedModifier);
				}
				else
				{
					Quaternion quaternion2 = Quaternion.LookRotation(this.overrideTarget - base.transform.position, Vector3.up);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion2, Time.deltaTime * (2500f * Quaternion.Angle(quaternion2, base.transform.rotation) + 10f) * this.eid.totalSpeedModifier);
				}
			}
		}
		if (!this.secondPhase && this.mach.health <= this.phaseChangeHealth)
		{
			if (!this.juggled)
			{
				this.JuggleStart();
			}
			this.secondPhase = true;
			this.voice.secondPhase = true;
		}
		if (this.juggled)
		{
			if (this.mach.health < this.juggleHp)
			{
				if (this.rb.velocity.y < 0f)
				{
					this.rb.velocity = Vector3.zero;
				}
				this.rb.AddForce(Vector3.up * (this.juggleHp - this.mach.health) * 10f, ForceMode.VelocityChange);
				this.anim.Play("Juggle", 0, 0f);
				this.juggleHp = this.mach.health;
				base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
				this.voice.Hurt();
				if (this.mach.health < this.juggleEndHp || this.juggleLength <= 0f)
				{
					this.JuggleStop(true);
				}
			}
			this.juggleLength = Mathf.MoveTowards(this.juggleLength, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x0004D614 File Offset: 0x0004B814
	private void FixedUpdate()
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (!this.juggled)
		{
			if (!this.inAction)
			{
				Vector3 vector = Vector3.zero;
				float num = Vector3.Distance(base.transform.position, this.eid.target.position);
				if (num > 10f)
				{
					vector += base.transform.forward * 7.5f;
				}
				else if (num > 5f)
				{
					vector += base.transform.forward * 7.5f * (num / 10f);
				}
				RaycastHit raycastHit;
				if (this.eid.target == null)
				{
					vector = Vector3.zero;
				}
				else if (this.goingLeft)
				{
					if (!Physics.SphereCast(base.transform.position, 1.25f, base.transform.right * -1f, out raycastHit, 3f, this.environmentMask))
					{
						vector += base.transform.right * -5f;
					}
					else if (!Physics.SphereCast(base.transform.position, 1.25f, base.transform.right, out raycastHit, 3f, this.environmentMask))
					{
						this.goingLeft = false;
					}
					else
					{
						vector += base.transform.forward * 5f;
					}
				}
				else if (!Physics.SphereCast(base.transform.position, 1.25f, base.transform.right, out raycastHit, 3f, this.environmentMask))
				{
					vector += base.transform.right * 5f;
				}
				else if (!Physics.SphereCast(base.transform.position, 1.25f, base.transform.right * -1f, out raycastHit, 3f, this.environmentMask))
				{
					this.goingLeft = true;
				}
				else
				{
					vector += base.transform.forward * 5f;
				}
				this.rb.velocity = vector * this.eid.totalSpeedModifier;
			}
			else if (this.goForward)
			{
				RaycastHit raycastHit2;
				if (!MonoSingleton<NewMovement>.Instance.playerCollider.Raycast(new Ray(base.transform.position, base.transform.forward), out raycastHit2, this.forwardSpeed * Time.fixedDeltaTime))
				{
					this.rb.velocity = base.transform.forward * this.forwardSpeed * ((this.difficulty >= 4) ? 1.25f : 1f);
				}
				else
				{
					if (raycastHit2.distance > 1f)
					{
						base.transform.position += base.transform.forward * (raycastHit2.distance - 1f);
					}
					this.rb.velocity = Vector3.zero;
				}
			}
			else
			{
				this.rb.velocity = Vector3.zero;
			}
		}
		else
		{
			if (this.rb.velocity.y < 35f)
			{
				this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
			}
			else
			{
				this.rb.velocity = new Vector3(0f, 35f, 0f);
			}
			RaycastHit raycastHit3;
			if (this.ceilingHitCooldown > 0f)
			{
				this.ceilingHitCooldown = Mathf.MoveTowards(this.ceilingHitCooldown, 0f, Time.fixedDeltaTime);
			}
			else if (this.rb.velocity.y > 1f && Physics.Raycast(base.transform.position, Vector3.up, out raycastHit3, 3f + this.rb.velocity.y * Time.fixedDeltaTime, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.ceilingHitCooldown = 0.5f;
				base.transform.position = raycastHit3.point - Vector3.up * 3f;
				this.mach.GetHurt(base.gameObject, Vector3.zero, Mathf.Min(this.rb.velocity.y, 5f), 0f, null, false);
				this.rb.velocity = new Vector3(0f, -this.rb.velocity.y, 0f);
				this.anim.Play("Juggle", 0, 0f);
				this.juggleHp = this.mach.health;
				this.voice.Hurt();
				Object.Instantiate<GameObject>(this.ceilingHitEffect, raycastHit3.point - Vector3.up, Quaternion.LookRotation(Vector3.down));
				MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
				if (this.ceilingHitChallenge)
				{
					MonoSingleton<ChallengeManager>.Instance.ChallengeDone();
				}
			}
			if (this.juggleFalling && Physics.SphereCast(base.transform.position, 1.25f, Vector3.down, out raycastHit3, 3.6f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				this.JuggleStop(false);
			}
			if (this.rb.velocity.y < 0f)
			{
				this.juggleFalling = true;
			}
		}
		if (this.dashing)
		{
			this.col.enabled = false;
			if (this.forcedDashTime > 0f)
			{
				this.forcedDashTime = Mathf.MoveTowards(this.forcedDashTime, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (Vector3.Distance(base.transform.position, this.dashTarget) > 5f)
			{
				RaycastHit raycastHit4;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (!Physics.SphereCast(base.transform.position, 0.75f, this.dashTarget - base.transform.position, out raycastHit4, Vector3.Distance(base.transform.position, this.dashTarget) - 0.75f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore) || (raycastHit4.collider.gameObject.layer == 11 && this.eid.target != null && this.eid.target.enemyIdentifier != null && raycastHit4.collider.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && this.eid.target.enemyIdentifier == enemyIdentifierIdentifier.eid))
				{
					this.rb.velocity = base.transform.forward * 100f * this.eid.totalSpeedModifier;
					return;
				}
				this.col.enabled = true;
				this.dashTarget = this.eid.target.position;
				this.Teleport(false, true, true, false, false);
				this.forcedDashTime = 0.35f;
				this.LookAtTarget(0);
				return;
			}
			else if (this.forcedDashTime <= 0f)
			{
				this.dashing = false;
				this.FastCombo();
				return;
			}
		}
		else
		{
			this.col.enabled = true;
		}
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0004DD98 File Offset: 0x0004BF98
	private void BasicCombo()
	{
		if (this.juggled || this.eid.target == null)
		{
			return;
		}
		this.CheckIfSwordsCombined();
		this.forwardSpeedMinimum = 125f;
		this.forwardSpeedMaximum = 175f;
		this.inAction = true;
		this.anim.Play("BasicCombo");
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0004DDF0 File Offset: 0x0004BFF0
	private void FastComboDash()
	{
		if (this.juggled || this.eid.target == null)
		{
			return;
		}
		this.CheckIfSwordsCombined();
		if (this.difficulty >= 2)
		{
			this.forwardSpeed = 100f;
		}
		else
		{
			this.forwardSpeed = 40f;
		}
		this.forwardSpeed *= this.eid.totalSpeedModifier;
		this.anim.Play("FastComboDash");
		this.inAction = true;
	}

	// Token: 0x06000ACC RID: 2764 RVA: 0x0004DE6C File Offset: 0x0004C06C
	private void FastCombo()
	{
		if (this.juggled || this.eid.target == null)
		{
			return;
		}
		this.forwardSpeedMinimum = 75f;
		this.forwardSpeedMaximum = 125f;
		this.inAction = true;
		this.anim.Play("FastCombo");
		this.LookAtTarget(0);
	}

	// Token: 0x06000ACD RID: 2765 RVA: 0x0004DEC4 File Offset: 0x0004C0C4
	private void ThrowCombo()
	{
		if (this.juggled || this.eid.target == null)
		{
			return;
		}
		this.CheckIfSwordsCombined();
		this.forwardSpeedMinimum = 125f;
		this.forwardSpeedMaximum = 175f;
		this.inAction = true;
		this.anim.Play("ThrowCombo");
		this.LookAtTarget(0);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x0004DF21 File Offset: 0x0004C121
	private void CombineSwords()
	{
		if (this.juggled || this.eid.target == null)
		{
			return;
		}
		if (this.swordsCombined)
		{
			this.UnGattai(true);
		}
		this.inAction = true;
		this.anim.Play("SwordsCombine");
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0004DF60 File Offset: 0x0004C160
	private void Gattai()
	{
		if (this.swordsCombined)
		{
			this.UnGattai(true);
		}
		this.swordsCombined = true;
		Renderer[] array = this.swordRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		this.fakeCombinedSwords.SetActive(true);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0004DFAD File Offset: 0x0004C1AD
	private void CombinedSwordAttack()
	{
		if (this.juggled)
		{
			return;
		}
		this.anim.Play("SwordsCombinedThrow");
	}

	// Token: 0x06000AD1 RID: 2769 RVA: 0x0004DFC8 File Offset: 0x0004C1C8
	public void UnGattai(bool destroySwords = true)
	{
		this.swordsCombined = false;
		Renderer[] array = this.swordRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		this.fakeCombinedSwords.SetActive(false);
		if (destroySwords && this.currentCombinedSwordsThrown)
		{
			Object.Destroy(this.currentCombinedSwordsThrown.gameObject);
		}
		if (this.lightSwords)
		{
			this.lightSwords = false;
			if (!this.leftSwingCheck.damaging)
			{
				this.leftHandGlow.enabled = false;
			}
			if (!this.rightSwingCheck.damaging)
			{
				this.rightHandGlow.enabled = false;
			}
		}
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0004E067 File Offset: 0x0004C267
	private void CheckIfSwordsCombined()
	{
		if (this.swordsCombined)
		{
			if (this.secondPhase || this.currentCombinedSwordsThrown.friendly)
			{
				this.CreateLightSwords();
				return;
			}
			this.UnGattai(true);
		}
	}

	// Token: 0x06000AD3 RID: 2771 RVA: 0x0004E094 File Offset: 0x0004C294
	private void CreateLightSwords()
	{
		this.lightSwords = true;
		this.leftHandGlow.enabled = true;
		this.rightHandGlow.enabled = true;
	}

	// Token: 0x06000AD4 RID: 2772 RVA: 0x0004E0B8 File Offset: 0x0004C2B8
	private void ThrowSwords()
	{
		if (this.juggled)
		{
			return;
		}
		Object.Instantiate<AudioSource>(this.kickSwingSound, base.transform);
		this.fakeCombinedSwords.SetActive(false);
		this.currentCombinedSwordsThrown = Object.Instantiate<Projectile>(this.combinedSwordsThrown, this.fakeCombinedSwords.transform.position, base.transform.rotation, base.transform.parent);
		this.currentCombinedSwordsThrown.target = this.eid.target;
		if (this.difficulty >= 4)
		{
			this.currentCombinedSwordsThrown.speed *= 1.75f;
		}
		if (this.difficulty <= 2)
		{
			this.combinedSwordsCooldown = 2f;
		}
		else
		{
			this.combinedSwordsCooldown = 1f;
		}
		this.currentCombinedSwordsThrown.damage *= this.eid.totalDamageModifier;
		GabrielCombinedSwordsThrown gabrielCombinedSwordsThrown;
		if (this.currentCombinedSwordsThrown.TryGetComponent<GabrielCombinedSwordsThrown>(out gabrielCombinedSwordsThrown))
		{
			gabrielCombinedSwordsThrown.gabe = this;
		}
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0004E1B0 File Offset: 0x0004C3B0
	private void OnTriggerEnter(Collider other)
	{
		if (this.juggleFalling && other.gameObject.layer == 0)
		{
			DeathZone deathZone;
			if (other.attachedRigidbody)
			{
				deathZone = other.attachedRigidbody.GetComponent<DeathZone>();
			}
			else
			{
				deathZone = other.GetComponent<DeathZone>();
			}
			if (deathZone)
			{
				if (this.voice)
				{
					this.voice.BigHurt();
				}
				base.transform.position = deathZone.respawnTarget;
				this.eid.DeliverDamage(this.head.gameObject, Vector3.zero, this.head.position, 15f, false, 0f, null, false, false);
				this.juggleFalling = false;
				this.JuggleStop(false);
			}
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0004E270 File Offset: 0x0004C470
	public void Teleport(bool closeRange = false, bool longrange = false, bool firstTime = true, bool horizontal = false, bool vertical = false)
	{
		if (firstTime)
		{
			this.teleportAttempts = 0;
		}
		this.outOfSightTime = 0f;
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
		else if (longrange)
		{
			num = (float)Random.Range(15, 20);
		}
		if (this.eid.target == null)
		{
			return;
		}
		Vector3 vector = this.eid.target.position + Vector3.up;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.eid.target.position + Vector3.up, normalized, out raycastHit, num, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			vector = raycastHit.point - normalized * 3f;
		}
		else
		{
			vector = this.eid.target.position + Vector3.up + normalized * num;
		}
		bool flag = false;
		bool flag2 = false;
		RaycastHit raycastHit2;
		if (Physics.Raycast(vector, Vector3.up, out raycastHit2, 8f, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			flag = true;
		}
		RaycastHit raycastHit3;
		if (Physics.Raycast(vector, Vector3.down, out raycastHit3, 8f, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			flag2 = true;
		}
		bool flag3 = false;
		Vector3 vector2 = base.transform.position;
		if (flag && flag2)
		{
			if (Vector3.Distance(raycastHit2.point, raycastHit3.point) > 7f)
			{
				if (horizontal)
				{
					vector2 = new Vector3(vector.x, raycastHit3.point.y + 3.5f, vector.z);
				}
				else
				{
					vector2 = new Vector3(vector.x, (raycastHit3.point.y + raycastHit2.point.y) / 2f, vector.z);
				}
				flag3 = true;
			}
			else
			{
				this.teleportAttempts++;
				if (this.teleportAttempts <= 10)
				{
					this.Teleport(closeRange, longrange, false, horizontal, vertical);
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
				if (horizontal)
				{
					vector2 = new Vector3(raycastHit3.point.x, raycastHit3.point.y + 3.5f, raycastHit3.point.z);
				}
				else
				{
					vector2 = raycastHit3.point + Vector3.up * (float)Random.Range(5, 10);
				}
			}
			else if (horizontal)
			{
				vector2 = new Vector3(vector.x, this.eid.target.position.y, vector.z);
			}
			else
			{
				vector2 = vector;
			}
		}
		if (flag3)
		{
			Collider[] array = Physics.OverlapCapsule(vector2 + base.transform.up * -2.25f, vector2 + base.transform.up * 1.25f, 1.25f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore);
			if (array != null && array.Length != 0)
			{
				for (int i = array.Length - 1; i >= 0; i--)
				{
					EnemyIdentifierIdentifier enemyIdentifierIdentifier;
					if (array[i].gameObject.layer != 11 || this.eid.target == null || this.eid.target.enemyIdentifier == null || !array[i].TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) || !enemyIdentifierIdentifier.eid || this.eid.target.enemyIdentifier != enemyIdentifierIdentifier.eid)
					{
						this.teleportAttempts++;
						if (this.teleportAttempts <= 10)
						{
							this.Teleport(closeRange, longrange, false, horizontal, vertical);
						}
						return;
					}
				}
			}
			int num2 = Mathf.RoundToInt(Vector3.Distance(base.transform.position, vector2) / 2.5f);
			for (int j = 0; j < num2; j++)
			{
				this.CreateDecoy(Vector3.Lerp(base.transform.position, vector2, (float)j / (float)num2), (float)j / (float)num2 + 0.1f, null);
			}
			base.transform.position = vector2;
			this.teleportAttempts = 0;
			Object.Instantiate<GameObject>(this.teleportSound, base.transform.position, Quaternion.identity);
			if (this.eid.hooked)
			{
				MonoSingleton<HookArm>.Instance.StopThrow(1f, true);
			}
		}
		if (this.goingLeft)
		{
			this.goingLeft = false;
			return;
		}
		this.goingLeft = true;
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0004E728 File Offset: 0x0004C928
	public GameObject CreateDecoy(Vector3 position, float transparencyOverride = 1f, Animator animatorOverride = null)
	{
		if ((!this.anim && !animatorOverride) || this.eid.target == null)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.decoy, position, base.transform.GetChild(0).rotation);
		Animator componentInChildren = gameObject.GetComponentInChildren<Animator>();
		AnimatorStateInfo animatorStateInfo = (animatorOverride ? animatorOverride.GetCurrentAnimatorStateInfo(0) : this.anim.GetCurrentAnimatorStateInfo(0));
		componentInChildren.Play(animatorStateInfo.shortNameHash, 0, animatorStateInfo.normalizedTime);
		componentInChildren.speed = 0f;
		MindflayerDecoy[] componentsInChildren = gameObject.GetComponentsInChildren<MindflayerDecoy>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].fadeOverride = transparencyOverride;
		}
		return gameObject;
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0004E7DC File Offset: 0x0004C9DC
	private void StartDash()
	{
		if (this.eid.target == null)
		{
			return;
		}
		this.inAction = true;
		this.overrideRotation = true;
		this.dashTarget = this.eid.target.position;
		this.overrideTarget = this.dashTarget;
		this.dashing = true;
		Object.Instantiate<GameObject>(this.dashEffect, base.transform.position, base.transform.rotation);
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0004E850 File Offset: 0x0004CA50
	private void Parryable()
	{
		if (this.juggled)
		{
			return;
		}
		this.mach.ParryableCheck(false);
		this.AttackFlash(0);
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x0004E870 File Offset: 0x0004CA70
	private void AttackFlash(int unparryable = 0)
	{
		if (!this.juggled)
		{
			Object.Instantiate<GameObject>((unparryable == 0) ? MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash : MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.head).transform.localScale *= 3f;
		}
	}

	// Token: 0x06000ADB RID: 2779 RVA: 0x0004E8C4 File Offset: 0x0004CAC4
	private void JuggleStart()
	{
		if (this.eid.target == null)
		{
			return;
		}
		this.DamageStopLeft(0);
		this.DamageStopRight(0);
		MonoSingleton<TimeController>.Instance.SlowDown(0.25f);
		this.voice.BigHurt();
		this.inAction = true;
		base.CancelInvoke();
		this.dashing = false;
		this.rb.velocity = Vector3.zero;
		this.rb.AddForce(Vector3.up * 35f, ForceMode.VelocityChange);
		this.rb.useGravity = true;
		this.origWing.SetFloat("_OpacScale", 0f);
		base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		this.overrideRotation = false;
		this.stopRotation = true;
		this.juggled = true;
		this.juggleHp = this.mach.health;
		this.juggleEndHp = this.mach.health - 7.5f;
		this.juggleLength = 5f;
		this.juggleFalling = false;
		Object.Instantiate<GameObject>(this.juggleEffect, base.transform.position, base.transform.rotation);
		this.eid.totalDamageTakenMultiplier = 0.5f;
		if (this.currentEnrageEffect)
		{
			MeshRenderer componentInChildren = this.currentEnrageEffect.GetComponentInChildren<MeshRenderer>();
			if (componentInChildren)
			{
				componentInChildren.material.color = new Color(0.5f, 0f, 0f, 0.5f);
			}
			Light componentInChildren2 = this.currentEnrageEffect.GetComponentInChildren<Light>();
			if (componentInChildren2)
			{
				componentInChildren2.enabled = false;
			}
		}
		if (this.swordsCombined)
		{
			this.UnGattai(true);
		}
		this.particles.SetActive(false);
		this.particlesEnraged.SetActive(false);
		this.ResetAnimSpeed();
		this.anim.Play("Juggle", 0, 0f);
		UltrakillEvent ultrakillEvent = this.onFirstPhaseEnd;
		if (ultrakillEvent == null)
		{
			return;
		}
		ultrakillEvent.Invoke("");
	}

	// Token: 0x06000ADC RID: 2780 RVA: 0x0004EAEC File Offset: 0x0004CCEC
	private void JuggleStop(bool enrage = false)
	{
		this.rb.useGravity = false;
		if (this.difficulty != 0)
		{
			this.burstLength = this.difficulty;
		}
		else
		{
			this.burstLength = 1;
		}
		this.voice.PhaseChange();
		this.origWing.SetFloat("_OpacScale", 1f);
		this.stopRotation = false;
		this.juggled = false;
		if (this.enraged)
		{
			this.particlesEnraged.SetActive(true);
		}
		else
		{
			this.particles.SetActive(true);
		}
		this.anim.Play("Idle");
		if ((enrage || this.mach.health <= this.phaseChangeHealth) && this.currentEnrageEffect)
		{
			this.EnrageAnimation();
			return;
		}
		this.inAction = false;
		this.attackCooldown = 1f;
		this.Teleport(false, false, true, false, false);
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x0004EBCC File Offset: 0x0004CDCC
	private void EnrageAnimation()
	{
		this.anim.Play("Enrage", 0, 0f);
		if (this.difficulty >= 3)
		{
			this.SpawnSummonedSwordsWindup();
		}
		base.Invoke("ForceUnEnrage", 3f * this.anim.speed);
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0004EC1C File Offset: 0x0004CE1C
	public void EnrageNow()
	{
		Material[] materials = this.smr.materials;
		materials[0] = this.enrageBody;
		materials[1] = this.enrageWing;
		this.smr.materials = materials;
		this.eid.UpdateBuffs(true, true);
		if (!this.currentEnrageEffect)
		{
			this.currentEnrageEffect = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.enrageEffect, base.transform);
		}
		FadeOut fadeOut = this.currentEnrageEffect.AddComponent<FadeOut>();
		fadeOut.activateOnEnable = true;
		fadeOut.speed = 0.1f;
		if (this.particles.activeSelf)
		{
			this.particlesEnraged.SetActive(true);
			this.particles.SetActive(false);
		}
		this.attackCooldown = 0f;
		this.readyTaunt = false;
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0004ECDB File Offset: 0x0004CEDB
	private void ForceUnEnrage()
	{
		this.UnEnrage();
		this.anim.Play("Idle");
		this.StopAction();
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0004ECFC File Offset: 0x0004CEFC
	public void UnEnrage()
	{
		base.CancelInvoke("ForceUnEnrage");
		Material[] materials = this.smr.materials;
		materials[0] = this.origBody;
		materials[1] = this.origWing;
		this.smr.materials = materials;
		this.eid.totalDamageTakenMultiplier = 1f;
		this.enraged = false;
		if (this.particlesEnraged.activeSelf)
		{
			this.particlesEnraged.SetActive(false);
			this.particles.SetActive(true);
		}
		if (this.difficulty >= 3)
		{
			this.SpawnSummonedSwords();
		}
		if (this.currentEnrageEffect)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
		this.burstLength = this.difficulty;
		if (this.burstLength == 0)
		{
			this.burstLength = 1;
		}
		UltrakillEvent ultrakillEvent = this.onSecondPhaseStart;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.attackCooldown = 0f;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0004EDDD File Offset: 0x0004CFDD
	private void RandomizeDirection()
	{
		if (Random.Range(0f, 1f) > 0.5f)
		{
			this.goingLeft = true;
			return;
		}
		this.goingLeft = false;
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0004EE04 File Offset: 0x0004D004
	public void DamageStartLeft(int damage)
	{
		if (!this.juggled)
		{
			this.leftHandTrail.emitting = true;
			this.leftHandGlow.gameObject.SetActive(true);
			this.SetDamage(damage);
			this.leftSwingCheck.DamageStart();
			this.generalSwingCheck.DamageStart();
			Object.Instantiate<AudioSource>(this.swingSound, base.transform);
			this.DecideMovementSpeed(this.forwardSpeedMinimum, this.forwardSpeedMaximum);
			this.goForward = true;
		}
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0004EE80 File Offset: 0x0004D080
	public void DamageStopLeft(int keepMoving)
	{
		this.leftHandTrail.emitting = false;
		this.leftSwingCheck.DamageStop();
		if (!this.lightSwords)
		{
			this.leftHandGlow.gameObject.SetActive(false);
		}
		if (keepMoving == 0)
		{
			this.goForward = false;
		}
		if (!this.rightSwingCheck || !this.rightSwingCheck.damaging)
		{
			this.mach.parryable = false;
			this.generalSwingCheck.DamageStop();
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x0004EEF8 File Offset: 0x0004D0F8
	public void DamageStartRight(int damage)
	{
		if (!this.juggled)
		{
			this.rightHandTrail.emitting = true;
			this.rightHandGlow.gameObject.SetActive(true);
			this.SetDamage(damage);
			this.rightSwingCheck.DamageStart();
			this.generalSwingCheck.DamageStart();
			Object.Instantiate<AudioSource>(this.swingSound, base.transform);
			this.DecideMovementSpeed(this.forwardSpeedMinimum, this.forwardSpeedMaximum);
			this.goForward = true;
		}
	}

	// Token: 0x06000AE5 RID: 2789 RVA: 0x0004EF74 File Offset: 0x0004D174
	public void DamageStopRight(int keepMoving)
	{
		this.rightHandTrail.emitting = false;
		this.rightSwingCheck.DamageStop();
		if (!this.lightSwords)
		{
			this.rightHandGlow.gameObject.SetActive(false);
		}
		if (keepMoving == 0)
		{
			this.goForward = false;
		}
		if (!this.leftSwingCheck || !this.leftSwingCheck.damaging)
		{
			this.mach.parryable = false;
			this.generalSwingCheck.DamageStop();
		}
	}

	// Token: 0x06000AE6 RID: 2790 RVA: 0x0004EFEC File Offset: 0x0004D1EC
	public void DamageStartKick(int damage)
	{
		if (!this.juggled)
		{
			this.kickTrail.emitting = true;
			this.SetDamage(damage);
			this.generalSwingCheck.DamageStart();
			Object.Instantiate<AudioSource>(this.kickSwingSound, base.transform);
			this.DecideMovementSpeed(this.forwardSpeedMinimum, this.forwardSpeedMaximum);
			this.goForward = true;
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0004F04C File Offset: 0x0004D24C
	public void DamageStopKick(int keepMoving)
	{
		if (this.kickTrail)
		{
			this.kickTrail.emitting = false;
		}
		if (keepMoving == 0)
		{
			this.goForward = false;
		}
		if ((!this.leftSwingCheck || !this.leftSwingCheck.damaging) && (!this.rightSwingCheck || !this.rightSwingCheck.damaging))
		{
			this.mach.parryable = false;
			this.generalSwingCheck.DamageStop();
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0004F0C7 File Offset: 0x0004D2C7
	public void DamageStartBoth(int damage)
	{
		this.DamageStartLeft(damage);
		this.DamageStartRight(damage);
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0004F0D7 File Offset: 0x0004D2D7
	public void DamageStopBoth(int keepMoving)
	{
		this.DamageStopLeft(keepMoving);
		this.DamageStopRight(keepMoving);
		this.DamageStopKick(keepMoving);
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0004F0EE File Offset: 0x0004D2EE
	public void SetForwardSpeed(int newSpeed)
	{
		this.forwardSpeedMinimum = (float)newSpeed;
		this.forwardSpeedMaximum = (float)(newSpeed + 50);
		this.DecideMovementSpeed(this.forwardSpeedMinimum, this.forwardSpeedMaximum);
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x0004F118 File Offset: 0x0004D318
	public void EnrageTeleport(int teleportType = 0)
	{
		if (this.secondPhase && !this.currentCombinedSwordsThrown)
		{
			if (teleportType >= 10)
			{
				if (this.difficulty < 3)
				{
					return;
				}
				teleportType -= 10;
			}
			if (teleportType <= 0)
			{
				teleportType = 2;
			}
			switch (teleportType)
			{
			case 1:
				this.Teleport(true, false, true, false, false);
				break;
			case 2:
				this.Teleport(false, false, true, false, false);
				break;
			case 3:
				this.Teleport(true, false, true, true, false);
				break;
			case 4:
				this.Teleport(false, false, true, true, false);
				break;
			case 5:
				this.Teleport(false, false, true, false, true);
				break;
			}
			this.anim.speed = 0f;
			base.Invoke("ResetAnimSpeed", 0.25f);
		}
		if (this.eid.target == null)
		{
			return;
		}
		base.transform.LookAt(this.eid.target.position);
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x0004F202 File Offset: 0x0004D402
	private void ResetAnimSpeed()
	{
		if (this.anim)
		{
			this.anim.speed = this.defaultAnimSpeed;
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x0004F224 File Offset: 0x0004D424
	public void LookAtTarget(int instant = 0)
	{
		if (this.eid.target == null)
		{
			return;
		}
		this.overrideRotation = true;
		this.overrideTarget = base.transform.position + (this.eid.target.position - base.transform.position).normalized * 999f;
		base.transform.LookAt(base.transform.position + (this.eid.target.position - base.transform.position).normalized * 999f);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x0004F2DB File Offset: 0x0004D4DB
	public void FollowTarget()
	{
		if (!this.juggled)
		{
			this.overrideRotation = false;
		}
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x0004F2EC File Offset: 0x0004D4EC
	public void StopAction()
	{
		if (!this.juggled)
		{
			this.FollowTarget();
			this.inAction = false;
		}
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0004F303 File Offset: 0x0004D503
	public void ResetWingMat()
	{
		this.origWing.SetFloat("_OpacScale", 1f);
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0004F31C File Offset: 0x0004D51C
	public void Death()
	{
		if (this.currentSwords)
		{
			Object.Destroy(this.currentSwords);
		}
		if (this.currentEnrageEffect)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
		if (!this.bossVersion)
		{
			Object.Instantiate<GameObject>(this.genericOutro, base.transform.position, Quaternion.LookRotation(new Vector3(base.transform.forward.x, 0f, base.transform.forward.z)));
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0004F3B2 File Offset: 0x0004D5B2
	private void SetDamage(int damage)
	{
		this.leftSwingCheck.damage = damage;
		this.rightSwingCheck.damage = damage;
		this.generalSwingCheck.damage = damage;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0004F3D8 File Offset: 0x0004D5D8
	public void TargetBeenHit()
	{
		this.leftSwingCheck.DamageStop();
		this.rightSwingCheck.DamageStop();
		this.generalSwingCheck.DamageStop();
		this.goForward = false;
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0004F404 File Offset: 0x0004D604
	private void DecideMovementSpeed(float normal, float longDistance)
	{
		if (this.eid.target == null)
		{
			return;
		}
		if (this.difficulty <= 1)
		{
			this.forwardSpeed = normal * this.anim.speed;
		}
		this.forwardSpeed = ((Vector3.Distance(this.eid.target.position + this.eid.target.GetVelocity() * 0.25f, base.transform.position) > 20f) ? (longDistance * this.anim.speed * (this.currentSwords ? 0.85f : 1f)) : (normal * this.anim.speed));
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0004F4BD File Offset: 0x0004D6BD
	private void SpawnSummonedSwordsWindup()
	{
		this.currentWindup = Object.Instantiate<GameObject>(this.summonedSwordsWindup, base.transform.position, Quaternion.identity);
		this.currentWindup.transform.SetParent(base.transform, true);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0004F4F8 File Offset: 0x0004D6F8
	private void SpawnSummonedSwords()
	{
		if (this.currentWindup)
		{
			Object.Destroy(this.currentWindup);
		}
		this.currentSwords = Object.Instantiate<GameObject>(this.summonedSwords, base.transform.position, Quaternion.identity);
		this.currentSwords.transform.SetParent(base.transform.parent, true);
		SummonedSwords summonedSwords;
		if (this.currentSwords.TryGetComponent<SummonedSwords>(out summonedSwords))
		{
			summonedSwords.target = new EnemyTarget(base.transform);
			summonedSwords.speed *= this.eid.totalSpeedModifier;
			summonedSwords.targetEnemy = this.eid.target;
		}
		if (this.eid.totalDamageModifier != 1f)
		{
			Projectile[] componentsInChildren = this.currentSwords.GetComponentsInChildren<Projectile>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].damage *= this.eid.totalDamageModifier;
			}
		}
	}

	// Token: 0x04000E4E RID: 3662
	private Animator anim;

	// Token: 0x04000E4F RID: 3663
	private Machine mach;

	// Token: 0x04000E50 RID: 3664
	private Rigidbody rb;

	// Token: 0x04000E51 RID: 3665
	private EnemyIdentifier eid;

	// Token: 0x04000E52 RID: 3666
	private SkinnedMeshRenderer smr;

	// Token: 0x04000E53 RID: 3667
	private GabrielVoice voice;

	// Token: 0x04000E54 RID: 3668
	private Collider col;

	// Token: 0x04000E55 RID: 3669
	public GameObject particles;

	// Token: 0x04000E56 RID: 3670
	public GameObject particlesEnraged;

	// Token: 0x04000E57 RID: 3671
	private Material origBody;

	// Token: 0x04000E58 RID: 3672
	private Material origWing;

	// Token: 0x04000E59 RID: 3673
	public Material enrageBody;

	// Token: 0x04000E5A RID: 3674
	public Material enrageWing;

	// Token: 0x04000E5B RID: 3675
	private int difficulty;

	// Token: 0x04000E5C RID: 3676
	private bool valuesSet;

	// Token: 0x04000E5D RID: 3677
	private bool active = true;

	// Token: 0x04000E5E RID: 3678
	private bool inAction;

	// Token: 0x04000E5F RID: 3679
	private bool goingLeft;

	// Token: 0x04000E60 RID: 3680
	private bool goForward;

	// Token: 0x04000E61 RID: 3681
	private float forwardSpeed;

	// Token: 0x04000E62 RID: 3682
	private float forwardSpeedMinimum;

	// Token: 0x04000E63 RID: 3683
	private float forwardSpeedMaximum;

	// Token: 0x04000E64 RID: 3684
	private float startCooldown = 2f;

	// Token: 0x04000E65 RID: 3685
	private float attackCooldown;

	// Token: 0x04000E66 RID: 3686
	public bool enraged;

	// Token: 0x04000E67 RID: 3687
	private GameObject currentEnrageEffect;

	// Token: 0x04000E68 RID: 3688
	public bool secondPhase;

	// Token: 0x04000E69 RID: 3689
	public float phaseChangeHealth;

	// Token: 0x04000E6A RID: 3690
	private float outOfSightTime;

	// Token: 0x04000E6B RID: 3691
	private int teleportAttempts;

	// Token: 0x04000E6C RID: 3692
	private int teleportInterval = 6;

	// Token: 0x04000E6D RID: 3693
	public GameObject teleportSound;

	// Token: 0x04000E6E RID: 3694
	public GameObject decoy;

	// Token: 0x04000E6F RID: 3695
	private bool overrideRotation;

	// Token: 0x04000E70 RID: 3696
	private bool stopRotation;

	// Token: 0x04000E71 RID: 3697
	private Vector3 overrideTarget;

	// Token: 0x04000E72 RID: 3698
	private LayerMask environmentMask;

	// Token: 0x04000E73 RID: 3699
	[Header("Swords")]
	public Transform rightHand;

	// Token: 0x04000E74 RID: 3700
	public Transform leftHand;

	// Token: 0x04000E75 RID: 3701
	private TrailRenderer rightHandTrail;

	// Token: 0x04000E76 RID: 3702
	private TrailRenderer leftHandTrail;

	// Token: 0x04000E77 RID: 3703
	[SerializeField]
	private SwingCheck2 generalSwingCheck;

	// Token: 0x04000E78 RID: 3704
	private SwingCheck2 rightSwingCheck;

	// Token: 0x04000E79 RID: 3705
	private SwingCheck2 leftSwingCheck;

	// Token: 0x04000E7A RID: 3706
	private MeshRenderer rightHandGlow;

	// Token: 0x04000E7B RID: 3707
	private MeshRenderer leftHandGlow;

	// Token: 0x04000E7C RID: 3708
	[SerializeField]
	private AudioSource swingSound;

	// Token: 0x04000E7D RID: 3709
	[SerializeField]
	private AudioSource kickSwingSound;

	// Token: 0x04000E7E RID: 3710
	[SerializeField]
	private Renderer[] swordRenderers;

	// Token: 0x04000E7F RID: 3711
	[SerializeField]
	private GameObject fakeCombinedSwords;

	// Token: 0x04000E80 RID: 3712
	[SerializeField]
	private Projectile combinedSwordsThrown;

	// Token: 0x04000E81 RID: 3713
	private Projectile currentCombinedSwordsThrown;

	// Token: 0x04000E82 RID: 3714
	[HideInInspector]
	public bool swordsCombined;

	// Token: 0x04000E83 RID: 3715
	private float combinedSwordsCooldown;

	// Token: 0x04000E84 RID: 3716
	[HideInInspector]
	public bool lightSwords;

	// Token: 0x04000E85 RID: 3717
	[Space(20f)]
	public TrailRenderer kickTrail;

	// Token: 0x04000E86 RID: 3718
	public GameObject dashEffect;

	// Token: 0x04000E87 RID: 3719
	private bool dashing;

	// Token: 0x04000E88 RID: 3720
	private float forcedDashTime;

	// Token: 0x04000E89 RID: 3721
	private Vector3 dashTarget;

	// Token: 0x04000E8A RID: 3722
	private float[] moveChanceBonuses = new float[4];

	// Token: 0x04000E8B RID: 3723
	private int previousMove = -1;

	// Token: 0x04000E8C RID: 3724
	private int burstLength = 2;

	// Token: 0x04000E8D RID: 3725
	private bool juggled;

	// Token: 0x04000E8E RID: 3726
	private float juggleHp;

	// Token: 0x04000E8F RID: 3727
	private float juggleEndHp;

	// Token: 0x04000E90 RID: 3728
	private float juggleLength;

	// Token: 0x04000E91 RID: 3729
	public GameObject juggleEffect;

	// Token: 0x04000E92 RID: 3730
	private bool juggleFalling;

	// Token: 0x04000E93 RID: 3731
	public GameObject summonedSwords;

	// Token: 0x04000E94 RID: 3732
	private GameObject currentSwords;

	// Token: 0x04000E95 RID: 3733
	public GameObject summonedSwordsWindup;

	// Token: 0x04000E96 RID: 3734
	private GameObject currentWindup;

	// Token: 0x04000E97 RID: 3735
	private float summonedSwordsCooldown = 15f;

	// Token: 0x04000E98 RID: 3736
	public Transform head;

	// Token: 0x04000E99 RID: 3737
	private bool readyTaunt;

	// Token: 0x04000E9A RID: 3738
	private float defaultAnimSpeed = 1f;

	// Token: 0x04000E9B RID: 3739
	private bool bossVersion;

	// Token: 0x04000E9C RID: 3740
	[SerializeField]
	private GameObject genericOutro;

	// Token: 0x04000E9D RID: 3741
	public bool ceilingHitChallenge;

	// Token: 0x04000E9E RID: 3742
	[SerializeField]
	private GameObject ceilingHitEffect;

	// Token: 0x04000E9F RID: 3743
	private float ceilingHitCooldown;

	// Token: 0x04000EA0 RID: 3744
	[Header("Events")]
	public UltrakillEvent onFirstPhaseEnd;

	// Token: 0x04000EA1 RID: 3745
	public UltrakillEvent onSecondPhaseStart;
}
