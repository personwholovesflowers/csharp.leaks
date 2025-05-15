using System;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class Gabriel : MonoBehaviour
{
	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000A74 RID: 2676 RVA: 0x000496C5 File Offset: 0x000478C5
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x000496D4 File Offset: 0x000478D4
	private void Awake()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.smr = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.voice = base.GetComponent<GabrielVoice>();
		this.col = base.GetComponent<Collider>();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00049742 File Offset: 0x00047942
	private void Start()
	{
		this.SetValues();
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0004974C File Offset: 0x0004794C
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
		if (this.enraged)
		{
			this.EnrageNow();
		}
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
		this.RandomizeDirection();
		BossHealthBar bossHealthBar;
		this.bossVersion = base.TryGetComponent<BossHealthBar>(out bossHealthBar);
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0004981F File Offset: 0x00047A1F
	private void UpdateBuff()
	{
		this.SetValues();
		this.UpdateSpeed();
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00049830 File Offset: 0x00047A30
	private void UpdateSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
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

	// Token: 0x06000A7A RID: 2682 RVA: 0x000498C0 File Offset: 0x00047AC0
	private void OnDisable()
	{
		base.CancelInvoke();
		if (this.rightHandWeapon || this.leftHandWeapon)
		{
			this.DisableWeapon();
		}
		this.DamageStopLeft(0);
		this.DamageStopRight(0);
		this.StopAction();
		this.ResetAnimSpeed();
		this.overrideRotation = false;
		this.spearing = false;
		this.spearAttacks = 0;
		this.dashing = false;
		if (this.currentSwords)
		{
			this.currentSwords.SetActive(false);
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x00049942 File Offset: 0x00047B42
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

	// Token: 0x06000A7C RID: 2684 RVA: 0x0004996C File Offset: 0x00047B6C
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

	// Token: 0x06000A7D RID: 2685 RVA: 0x000499C4 File Offset: 0x00047BC4
	private void Update()
	{
		this.UpdateRigidbodySettings();
		if (this.target == null)
		{
			return;
		}
		if (this.active)
		{
			if (this.startCooldown > 0f)
			{
				this.startCooldown = Mathf.MoveTowards(this.startCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if ((this.secondPhase || this.enraged) && this.difficulty >= 3 && !this.currentSwords)
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
				else if (Physics.Raycast(base.transform.position, this.target.headPosition - base.transform.position, Vector3.Distance(base.transform.position, this.target.headPosition), LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.Teleport(false, false, true, false, false);
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					if (Vector3.Distance(base.transform.position, this.target.headPosition) > 10f)
					{
						flag2 = true;
					}
					else if (Vector3.Distance(base.transform.position, this.target.headPosition) < 5f)
					{
						flag = true;
					}
					float[] array = new float[4];
					int num = -1;
					if (this.previousMove != 0 && !flag && !this.threwAxes)
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
					if (this.previousMove != 3)
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
						this.AxeThrow();
						break;
					case 1:
						this.SpearCombo();
						break;
					case 2:
						this.StingerCombo();
						break;
					case 3:
						this.ZweiDash();
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
							this.threwAxes = false;
							this.readyTaunt = true;
						}
					}
				}
			}
			if ((Vector3.Distance(base.transform.position, this.target.headPosition) > 20f || base.transform.position.y > this.target.headPosition.y + 15f || Physics.Raycast(base.transform.position, this.target.headPosition - base.transform.position, Vector3.Distance(base.transform.position, this.target.headPosition), this.environmentMask)) && this.startCooldown <= 0f)
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
					Quaternion quaternion = Quaternion.LookRotation(this.target.headPosition - base.transform.position, Vector3.up);
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
		}
		if (this.juggled)
		{
			if (this.mach.health < this.juggleHp)
			{
				if (this.rb.velocity.y < 0f)
				{
					this.rb.velocity = Vector3.zero;
				}
				this.rb.AddForce(Vector3.up * (this.juggleHp - this.mach.health) * 5f, ForceMode.VelocityChange);
				this.anim.Play("Juggle", 0, 0f);
				this.juggleHp = this.mach.health;
				base.transform.LookAt(new Vector3(this.target.headPosition.x, base.transform.position.y, this.target.headPosition.z));
				this.voice.Hurt();
				if (this.mach.health < this.juggleEndHp || this.juggleLength <= 0f)
				{
					this.JuggleStop(true);
				}
			}
			this.juggleLength = Mathf.MoveTowards(this.juggleLength, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0004A12C File Offset: 0x0004832C
	private void FixedUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		if (!this.juggled)
		{
			if (!this.inAction)
			{
				Vector3 vector = Vector3.zero;
				float num = Vector3.Distance(base.transform.position, this.target.headPosition);
				if (num > 10f)
				{
					vector += base.transform.forward * 7.5f;
				}
				else if (num > 5f)
				{
					vector += base.transform.forward * 7.5f * (num / 10f);
				}
				RaycastHit raycastHit;
				if (this.target == null)
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
				this.rb.velocity = base.transform.forward * this.forwardSpeed * ((this.difficulty >= 4) ? 1.25f : 1f);
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
			RaycastHit raycastHit2;
			if (this.juggleFalling && Physics.SphereCast(base.transform.position, 1.25f, Vector3.down, out raycastHit2, 3.6f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies)))
			{
				this.JuggleStop(false);
			}
			if (this.rb.velocity.y < 0f)
			{
				this.juggleFalling = true;
			}
		}
		if (this.spearing)
		{
			if (!this.goForward)
			{
				base.transform.position = this.target.headPosition + Vector3.up * 15f;
			}
			else if (Physics.Raycast(base.transform.position, base.transform.forward, 2f, this.environmentMask))
			{
				this.spearing = false;
				this.DamageStopRight(0);
			}
		}
		if (this.dashing)
		{
			this.col.enabled = false;
			if (this.forcedDashTime > 0f)
			{
				this.forcedDashTime = Mathf.MoveTowards(this.forcedDashTime, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			if (Vector3.Distance(base.transform.position, this.dashTarget) > (float)(5 + this.dashAttempts))
			{
				RaycastHit raycastHit3;
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (!Physics.SphereCast(base.transform.position, 0.75f, this.dashTarget - base.transform.position, out raycastHit3, Vector3.Distance(base.transform.position, this.dashTarget) - 0.75f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore) || (raycastHit3.collider.gameObject.layer == 11 && this.eid.target != null && this.eid.target.enemyIdentifier != null && raycastHit3.collider.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && this.eid.target.enemyIdentifier == enemyIdentifierIdentifier.eid))
				{
					this.rb.velocity = base.transform.forward * 100f * this.eid.totalSpeedModifier;
					return;
				}
				this.dashAttempts++;
				this.col.enabled = true;
				this.dashTarget = this.target.headPosition;
				this.Teleport(false, true, true, false, false);
				this.forcedDashTime = 0.35f;
				this.LookAtTarget(0);
				return;
			}
			else if (this.forcedDashTime <= 0f)
			{
				this.dashAttempts = 0;
				this.dashing = false;
				this.ZweiCombo();
				return;
			}
		}
		else
		{
			this.col.enabled = true;
			this.dashAttempts = 0;
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0004A708 File Offset: 0x00048908
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
				MonoSingleton<ChallengeManager>.Instance.ChallengeDone();
			}
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0004A7D0 File Offset: 0x000489D0
	public void Teleport(bool closeRange = false, bool longrange = false, bool firstTime = true, bool horizontal = false, bool vertical = false)
	{
		if (firstTime)
		{
			this.teleportAttempts = 0;
		}
		this.outOfSightTime = 0f;
		this.spearing = false;
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
		if (this.target == null)
		{
			return;
		}
		Vector3 vector = this.target.headPosition + Vector3.up;
		RaycastHit raycastHit;
		if (Physics.Raycast(this.target.headPosition + Vector3.up, normalized, out raycastHit, num, this.environmentMask, QueryTriggerInteraction.Ignore))
		{
			vector = raycastHit.point - normalized * 3f;
		}
		else
		{
			vector = this.target.headPosition + Vector3.up + normalized * num;
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
				vector2 = new Vector3(vector.x, this.target.headPosition.y, vector.z);
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

	// Token: 0x06000A81 RID: 2689 RVA: 0x0004AC78 File Offset: 0x00048E78
	public GameObject CreateDecoy(Vector3 position, float transparencyOverride = 1f, Animator animatorOverride = null)
	{
		if ((!this.anim && !animatorOverride) || this.target == null)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.decoy, position, base.transform.GetChild(0).rotation, base.transform.parent);
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

	// Token: 0x06000A82 RID: 2690 RVA: 0x0004AD2F File Offset: 0x00048F2F
	private void StingerCombo()
	{
		this.forwardSpeed = 100f * this.anim.speed;
		this.SpawnLeftHandWeapon(GabrielWeaponType.Sword);
		this.inAction = true;
		this.anim.Play("StingerCombo");
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0004AD68 File Offset: 0x00048F68
	private void SpearCombo()
	{
		if (this.difficulty >= 2)
		{
			this.forwardSpeed = 150f;
		}
		else if (this.difficulty == 1)
		{
			this.forwardSpeed = 75f;
		}
		else
		{
			this.forwardSpeed = 60f;
		}
		this.forwardSpeed *= this.eid.totalSpeedModifier;
		if (this.enraged && this.secondPhase)
		{
			this.spearAttacks = 3;
		}
		else if (this.enraged || this.secondPhase)
		{
			this.spearAttacks = 2;
		}
		else
		{
			this.spearAttacks = 1;
		}
		this.SpawnRightHandWeapon(GabrielWeaponType.Spear);
		this.inAction = true;
		this.anim.Play("SpearReady");
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0004AE1C File Offset: 0x0004901C
	private void ZweiDash()
	{
		if (this.difficulty >= 2)
		{
			this.forwardSpeed = 100f;
		}
		else
		{
			this.forwardSpeed = 40f;
		}
		this.forwardSpeed *= this.eid.totalSpeedModifier;
		this.anim.Play("ZweiDash");
		this.inAction = true;
		this.SpawnRightHandWeapon(GabrielWeaponType.Zweihander);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0004AE80 File Offset: 0x00049080
	private void StartDash()
	{
		this.inAction = true;
		this.overrideRotation = true;
		this.dashTarget = this.target.headPosition;
		this.overrideTarget = this.dashTarget;
		this.dashing = true;
		Object.Instantiate<GameObject>(this.dashEffect, base.transform.position, base.transform.rotation);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0004AEE4 File Offset: 0x000490E4
	private void ZweiCombo()
	{
		this.forwardSpeed = 65f * this.anim.speed;
		this.inAction = true;
		this.anim.Play("ZweiCombo");
		this.LookAtTarget(0);
		if (this.secondPhase || this.enraged)
		{
			this.throws = 1;
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0004AF3D File Offset: 0x0004913D
	private void AxeThrow()
	{
		this.threwAxes = true;
		this.inAction = true;
		this.SpawnRightHandWeapon(GabrielWeaponType.Axe);
		this.SpawnLeftHandWeapon(GabrielWeaponType.Axe);
		this.anim.Play("AxeThrow");
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0004AF6C File Offset: 0x0004916C
	private void SpearAttack()
	{
		if (!this.juggled)
		{
			if (this.target == null)
			{
				this.spearAttacks = 0;
			}
			if (this.spearAttacks > 0)
			{
				this.spearing = true;
				this.goForward = false;
				this.spearAttacks--;
				float num = Random.Range(0f, 1f);
				if (this.difficulty <= 1)
				{
					base.Invoke("SpearAttack", 2f / this.eid.totalSpeedModifier);
				}
				else if (this.difficulty == 2)
				{
					base.Invoke("SpearAttack", 1.5f / this.eid.totalSpeedModifier);
				}
				else
				{
					base.Invoke("SpearAttack", 0.75f / this.eid.totalSpeedModifier);
				}
				bool flag = false;
				Vector3 vector = this.target.headPosition;
				RaycastHit raycastHit;
				if (!Physics.Raycast(this.target.headPosition, Vector3.up, out raycastHit, 17f, this.environmentMask, QueryTriggerInteraction.Ignore))
				{
					vector = this.target.headPosition + Vector3.up * 15f;
					flag = true;
				}
				else if (!Physics.Raycast(this.target.headPosition, Vector3.down, out raycastHit, 17f, this.environmentMask, QueryTriggerInteraction.Ignore))
				{
					vector = base.transform.position + Vector3.down * 15f;
					flag = true;
				}
				if (!flag || ((this.difficulty > 3 || this.enraged) && num <= 0.5f))
				{
					this.anim.Play("SpearStinger");
					this.Teleport(false, true, true, true, false);
					this.FollowTarget();
					base.Invoke("SpearFlash", 0.25f / this.eid.totalSpeedModifier);
					base.Invoke("SpearGoHorizontal", 0.5f / this.eid.totalSpeedModifier);
					return;
				}
				Animator animator = this.anim;
				if (animator != null)
				{
					animator.Play("SpearDown");
				}
				int num2 = Mathf.RoundToInt(Vector3.Distance(base.transform.position, vector) / 2.5f);
				for (int i = 0; i < num2; i++)
				{
					this.CreateDecoy(Vector3.Lerp(base.transform.position, vector, (float)i / (float)num2), (float)i / (float)num2 + 0.1f, null);
				}
				base.transform.position = vector;
				this.teleportAttempts = 0;
				Object.Instantiate<GameObject>(this.teleportSound, base.transform.position, Quaternion.identity);
				if (this.eid.hooked)
				{
					MonoSingleton<HookArm>.Instance.StopThrow(1f, true);
				}
				this.LookAtTarget(0);
				if (this.difficulty <= 1)
				{
					base.Invoke("SpearFlash", 0.5f / this.eid.totalSpeedModifier);
					base.Invoke("SpearGo", 1f / this.eid.totalSpeedModifier);
					return;
				}
				if (this.difficulty == 2)
				{
					base.Invoke("SpearFlash", 0.375f / this.eid.totalSpeedModifier);
					base.Invoke("SpearGo", 0.75f / this.eid.totalSpeedModifier);
					return;
				}
				base.Invoke("SpearFlash", 0.25f / this.eid.totalSpeedModifier);
				base.Invoke("SpearGo", 0.5f / this.eid.totalSpeedModifier);
				return;
			}
			else
			{
				this.SpearThrow();
			}
		}
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0004B2E1 File Offset: 0x000494E1
	private void SpearFlash()
	{
		if (!this.juggled)
		{
			Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.head);
		}
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0004B301 File Offset: 0x00049501
	private void SpearGoHorizontal()
	{
		if (!this.juggled)
		{
			this.LookAtTarget(0);
			this.SpearGo();
		}
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0004B318 File Offset: 0x00049518
	private void SpearGo()
	{
		if (!this.juggled)
		{
			Object.Instantiate<GameObject>(this.dashEffect, base.transform.position, base.transform.rotation);
			this.DamageStartRight(25);
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0004B34C File Offset: 0x0004954C
	private void JuggleStart()
	{
		if (this.leftHandWeapon)
		{
			this.DamageStopLeft(0);
		}
		if (this.rightHandWeapon)
		{
			this.DamageStopRight(0);
		}
		MonoSingleton<TimeController>.Instance.SlowDown(0.25f);
		this.voice.BigHurt();
		this.inAction = true;
		this.DisableWeapon();
		base.CancelInvoke();
		this.dashing = false;
		this.spearing = false;
		this.rb.velocity = Vector3.zero;
		this.rb.AddForce(Vector3.up * 35f, ForceMode.VelocityChange);
		this.rb.useGravity = true;
		this.origWing.SetFloat("_OpacScale", 0f);
		base.transform.LookAt(new Vector3(this.target.headPosition.x, base.transform.position.y, this.target.headPosition.z));
		this.overrideRotation = false;
		this.stopRotation = true;
		this.juggled = true;
		this.juggleHp = this.mach.health;
		this.juggleEndHp = this.mach.health - 15f;
		this.juggleLength = 5f;
		this.juggleFalling = false;
		Object.Instantiate<GameObject>(this.juggleEffect, base.transform.position, base.transform.rotation);
		this.eid.totalDamageTakenMultiplier = 0.5f;
		this.particles.SetActive(false);
		this.particlesEnraged.SetActive(false);
		this.ResetAnimSpeed();
		this.anim.Play("Juggle");
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0004B4F8 File Offset: 0x000496F8
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
		this.spearing = false;
		this.eid.totalDamageTakenMultiplier = 1f;
		if ((enrage || this.mach.health <= this.phaseChangeHealth) && !this.currentEnrageEffect)
		{
			this.Enrage();
			return;
		}
		this.inAction = false;
		this.attackCooldown = 1f;
		this.Teleport(false, false, true, false, false);
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0004B5EC File Offset: 0x000497EC
	private void Enrage()
	{
		this.anim.Play("Enrage");
		if (this.difficulty >= 3)
		{
			this.SpawnSummonedSwordsWindup();
		}
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0004B610 File Offset: 0x00049810
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
		if (this.difficulty >= 3)
		{
			this.SpawnSummonedSwords();
		}
		if (this.particles.activeSelf)
		{
			this.particlesEnraged.SetActive(true);
			this.particles.SetActive(false);
		}
		this.burstLength = this.difficulty;
		if (this.burstLength == 0)
		{
			this.burstLength = 1;
		}
		this.attackCooldown = 0f;
		this.readyTaunt = false;
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0004B6E0 File Offset: 0x000498E0
	public void UnEnrage()
	{
		Material[] materials = this.smr.materials;
		materials[0] = this.origBody;
		materials[1] = this.origWing;
		this.smr.materials = materials;
		this.enraged = false;
		if (this.particlesEnraged.activeSelf)
		{
			this.particlesEnraged.SetActive(false);
			this.particles.SetActive(true);
		}
		if (this.currentEnrageEffect)
		{
			Object.Destroy(this.currentEnrageEffect);
		}
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0004B75B File Offset: 0x0004995B
	private void SpearThrow()
	{
		if (!this.juggled)
		{
			this.spearing = false;
			this.DamageStopRight(0);
			this.Teleport(false, false, true, false, false);
			this.FollowTarget();
			this.anim.Play("SpearThrow");
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0004B794 File Offset: 0x00049994
	private void ThrowWeapon(GameObject projectile)
	{
		if (!this.juggled)
		{
			if (this.rightHandWeapon != null)
			{
				this.rightHandWeapon.SetActive(false);
				if (this.rightHandTrail)
				{
					this.rightHandTrail.RemoveTrail();
				}
				Object.Destroy(this.rightHandWeapon);
				if (this.rightSwingCheck)
				{
					Object.Destroy(this.rightSwingCheck.gameObject);
				}
			}
			if (this.leftHandWeapon != null)
			{
				this.leftHandWeapon.SetActive(false);
				if (this.leftHandTrail)
				{
					this.leftHandTrail.RemoveTrail();
				}
				Object.Destroy(this.leftHandWeapon);
				if (this.leftSwingCheck)
				{
					Object.Destroy(this.leftSwingCheck.gameObject);
				}
			}
			if (this.throws > 0)
			{
				this.throws--;
				base.Invoke("CheckForThrown", 0.35f / this.eid.totalSpeedModifier);
			}
			this.thrownObject = Object.Instantiate<GameObject>(projectile, base.transform.position + base.transform.forward * 3f, base.transform.rotation);
			if (this.difficulty <= 1 || this.eid.totalSpeedModifier != 1f || this.eid.totalDamageModifier != 1f)
			{
				Projectile componentInChildren = this.thrownObject.GetComponentInChildren<Projectile>();
				componentInChildren.target = this.target;
				if (componentInChildren)
				{
					if (this.difficulty <= 1)
					{
						componentInChildren.speed *= 0.5f;
					}
					componentInChildren.damage *= this.eid.totalDamageModifier;
				}
			}
		}
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0004B950 File Offset: 0x00049B50
	private void CheckForThrown()
	{
		if (!this.juggled)
		{
			if (this.thrownObject != null)
			{
				Vector3 position = this.thrownObject.transform.position;
				Collider[] array = Physics.OverlapCapsule(position + base.transform.up * -2.25f, position + base.transform.up * 1.25f, 1.25f, LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore);
				if (array != null && array.Length != 0)
				{
					this.throws = 0;
					return;
				}
				int num = Mathf.RoundToInt(Vector3.Distance(base.transform.position, position) / 2.5f);
				for (int i = 0; i < num; i++)
				{
					this.CreateDecoy(Vector3.Lerp(base.transform.position, position, (float)i / (float)num), (float)i / (float)num + 0.1f, null);
				}
				base.transform.position = position;
				this.teleportAttempts = 0;
				Object.Instantiate<GameObject>(this.teleportSound, base.transform.position, Quaternion.identity);
				this.thrownObject.gameObject.SetActive(false);
				Object.Destroy(this.thrownObject);
				base.transform.LookAt(this.target.headPosition);
				this.anim.speed = 0f;
				this.SpearFlash();
				base.Invoke("ResetAnimSpeed", 0.25f / this.eid.totalSpeedModifier);
				this.anim.Play("ZweiCombo", -1, 0.5f);
				return;
			}
			else
			{
				this.throws = 0;
			}
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0004BAE9 File Offset: 0x00049CE9
	public void EnableWeapon()
	{
		if (!this.juggled)
		{
			if (this.rightHandWeapon)
			{
				this.rightHandWeapon.SetActive(true);
			}
			if (this.leftHandWeapon)
			{
				this.leftHandWeapon.SetActive(true);
			}
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0004BB28 File Offset: 0x00049D28
	public void DisableWeapon()
	{
		if (!this.juggled)
		{
			if (this.rightHandWeapon)
			{
				if (this.rightHandTrail)
				{
					this.rightHandTrail.RemoveTrail();
				}
				Object.Destroy(this.rightHandWeapon);
				if (this.rightSwingCheck)
				{
					Object.Destroy(this.rightSwingCheck.gameObject);
				}
			}
			if (this.leftHandWeapon)
			{
				if (this.leftHandTrail)
				{
					this.leftHandTrail.RemoveTrail();
				}
				Object.Destroy(this.leftHandWeapon);
				if (this.leftSwingCheck)
				{
					Object.Destroy(this.leftSwingCheck.gameObject);
				}
			}
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0004BBDA File Offset: 0x00049DDA
	private void RandomizeDirection()
	{
		if (Random.Range(0f, 1f) > 0.5f)
		{
			this.goingLeft = true;
			return;
		}
		this.goingLeft = false;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0004BC04 File Offset: 0x00049E04
	private void SpawnLeftHandWeapon(GabrielWeaponType weapon)
	{
		if (!this.juggled)
		{
			GameObject weaponGameObject = this.GetWeaponGameObject(weapon);
			if (weaponGameObject != null)
			{
				this.leftHandWeapon = Object.Instantiate<GameObject>(weaponGameObject, this.leftHand.position, this.leftHand.rotation);
				this.leftHandWeapon.transform.forward = this.leftHand.transform.up;
				this.leftHandWeapon.transform.SetParent(this.leftHand, true);
				this.leftHandTrail = this.leftHandWeapon.GetComponentInChildren<WeaponTrail>();
				this.leftHandWeapon.SetActive(false);
				this.leftSwingCheck = this.WeaponHitBox(weapon);
			}
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0004BCB4 File Offset: 0x00049EB4
	private void SpawnRightHandWeapon(GabrielWeaponType weapon)
	{
		if (!this.juggled)
		{
			GameObject weaponGameObject = this.GetWeaponGameObject(weapon);
			if (weaponGameObject != null)
			{
				this.rightHandWeapon = Object.Instantiate<GameObject>(weaponGameObject, this.rightHand.position, this.rightHand.rotation);
				this.rightHandWeapon.transform.forward = this.rightHand.transform.up;
				this.rightHandWeapon.transform.SetParent(this.rightHand, true);
				this.rightHandTrail = this.rightHandWeapon.GetComponentInChildren<WeaponTrail>();
				this.rightHandWeapon.SetActive(false);
				this.rightSwingCheck = this.WeaponHitBox(weapon);
			}
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0004BD64 File Offset: 0x00049F64
	private GameObject GetWeaponGameObject(GabrielWeaponType weapon)
	{
		switch (weapon)
		{
		case GabrielWeaponType.Sword:
			return this.sword;
		case GabrielWeaponType.Zweihander:
			return this.zweiHander;
		case GabrielWeaponType.Axe:
			return this.axe;
		case GabrielWeaponType.Spear:
			return this.spear;
		case GabrielWeaponType.Glaive:
			return this.glaive;
		default:
			return null;
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0004BDB4 File Offset: 0x00049FB4
	private SwingCheck2 WeaponHitBox(GabrielWeaponType weapon)
	{
		switch (weapon)
		{
		case GabrielWeaponType.Sword:
			return this.CreateHitBox(new Vector3(0f, 0f, 1.5f), new Vector3(4f, 5f, 3f), false);
		case GabrielWeaponType.Zweihander:
			return this.CreateHitBox(new Vector3(0f, 0f, 2.5f), new Vector3(8f, 5f, 5f), false);
		case GabrielWeaponType.Spear:
			return this.CreateHitBox(new Vector3(0f, 0f, 2.5f), new Vector3(3.5f, 3.5f, 5f), true);
		}
		return null;
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0004BE70 File Offset: 0x0004A070
	private SwingCheck2 CreateHitBox(Vector3 position, Vector3 size, bool ignoreSlide = false)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
		gameObject.transform.SetParent(base.transform, true);
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.enabled = false;
		boxCollider.isTrigger = true;
		boxCollider.center = position;
		boxCollider.size = size;
		SwingCheck2 swingCheck = gameObject.AddComponent<SwingCheck2>();
		swingCheck.type = EnemyType.Gabriel;
		swingCheck.ignoreSlidingPlayer = ignoreSlide;
		swingCheck.OverrideEnemyIdentifier(this.eid);
		return swingCheck;
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0004BEF6 File Offset: 0x0004A0F6
	public void DamageStartLeft(int damage)
	{
		if (!this.juggled)
		{
			this.leftHandTrail.AddTrail();
			this.leftSwingCheck.damage = damage;
			this.leftSwingCheck.DamageStart();
			this.goForward = true;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0004BF29 File Offset: 0x0004A129
	public void DamageStopLeft(int keepMoving)
	{
		if (this.leftHandTrail)
		{
			this.leftHandTrail.RemoveTrail();
		}
		if (this.leftSwingCheck)
		{
			this.leftSwingCheck.DamageStop();
		}
		if (keepMoving == 0)
		{
			this.goForward = false;
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x0004BF65 File Offset: 0x0004A165
	public void DamageStartRight(int damage)
	{
		if (!this.juggled)
		{
			this.rightHandTrail.AddTrail();
			this.rightSwingCheck.damage = damage;
			this.rightSwingCheck.DamageStart();
			this.goForward = true;
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0004BF98 File Offset: 0x0004A198
	public void DamageStopRight(int keepMoving)
	{
		if (this.rightHandTrail)
		{
			this.rightHandTrail.RemoveTrail();
		}
		if (this.rightSwingCheck)
		{
			this.rightSwingCheck.DamageStop();
		}
		if (keepMoving == 0)
		{
			this.goForward = false;
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0004BFD4 File Offset: 0x0004A1D4
	public void SetForwardSpeed(int newSpeed)
	{
		this.forwardSpeed = (float)newSpeed * this.defaultAnimSpeed;
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0004BFE8 File Offset: 0x0004A1E8
	public void EnrageTeleport(int teleportType = 0)
	{
		if (this.enraged || this.secondPhase)
		{
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
			base.Invoke("ResetAnimSpeed", 0.25f / this.eid.totalSpeedModifier);
		}
		if (this.target == null)
		{
			return;
		}
		base.transform.LookAt(this.target.headPosition);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0004C0B0 File Offset: 0x0004A2B0
	private void ResetAnimSpeed()
	{
		if (this.anim)
		{
			this.anim.speed = this.defaultAnimSpeed;
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0004C0D0 File Offset: 0x0004A2D0
	public void LookAtTarget(int instant = 0)
	{
		this.overrideRotation = true;
		if (this.target == null)
		{
			base.transform.rotation = Quaternion.identity;
			return;
		}
		this.overrideTarget = base.transform.position + (this.target.headPosition - base.transform.position).normalized * 999f;
		base.transform.LookAt(base.transform.position + (this.target.headPosition - base.transform.position).normalized * 999f);
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0004C188 File Offset: 0x0004A388
	public void FollowTarget()
	{
		if (!this.juggled)
		{
			this.overrideRotation = false;
		}
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0004C199 File Offset: 0x0004A399
	public void StopAction()
	{
		if (!this.juggled)
		{
			this.FollowTarget();
			this.inAction = false;
		}
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0004C1B0 File Offset: 0x0004A3B0
	public void ResetWingMat()
	{
		this.origWing.SetFloat("_OpacScale", 1f);
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0004C1C8 File Offset: 0x0004A3C8
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

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0004C25E File Offset: 0x0004A45E
	private void SpawnSummonedSwordsWindup()
	{
		this.currentWindup = Object.Instantiate<GameObject>(this.summonedSwordsWindup, base.transform.position, Quaternion.identity);
		this.currentWindup.transform.SetParent(base.transform, true);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0004C298 File Offset: 0x0004A498
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
		foreach (Projectile projectile in this.currentSwords.GetComponentsInChildren<Projectile>())
		{
			projectile.target = this.target;
			if (this.eid.totalDamageModifier != 1f)
			{
				projectile.damage *= this.eid.totalDamageModifier;
			}
		}
	}

	// Token: 0x04000DE6 RID: 3558
	private Animator anim;

	// Token: 0x04000DE7 RID: 3559
	private Machine mach;

	// Token: 0x04000DE8 RID: 3560
	private Rigidbody rb;

	// Token: 0x04000DE9 RID: 3561
	private EnemyIdentifier eid;

	// Token: 0x04000DEA RID: 3562
	private SkinnedMeshRenderer smr;

	// Token: 0x04000DEB RID: 3563
	private GabrielVoice voice;

	// Token: 0x04000DEC RID: 3564
	private Collider col;

	// Token: 0x04000DED RID: 3565
	public GameObject particles;

	// Token: 0x04000DEE RID: 3566
	public GameObject particlesEnraged;

	// Token: 0x04000DEF RID: 3567
	private Material origBody;

	// Token: 0x04000DF0 RID: 3568
	private Material origWing;

	// Token: 0x04000DF1 RID: 3569
	public Material enrageBody;

	// Token: 0x04000DF2 RID: 3570
	public Material enrageWing;

	// Token: 0x04000DF3 RID: 3571
	private int difficulty;

	// Token: 0x04000DF4 RID: 3572
	private bool valuesSet;

	// Token: 0x04000DF5 RID: 3573
	private bool active = true;

	// Token: 0x04000DF6 RID: 3574
	private bool inAction;

	// Token: 0x04000DF7 RID: 3575
	private bool goingLeft;

	// Token: 0x04000DF8 RID: 3576
	private bool goForward;

	// Token: 0x04000DF9 RID: 3577
	private float forwardSpeed;

	// Token: 0x04000DFA RID: 3578
	private float startCooldown = 2f;

	// Token: 0x04000DFB RID: 3579
	private float attackCooldown;

	// Token: 0x04000DFC RID: 3580
	public bool enraged;

	// Token: 0x04000DFD RID: 3581
	private GameObject currentEnrageEffect;

	// Token: 0x04000DFE RID: 3582
	public bool secondPhase;

	// Token: 0x04000DFF RID: 3583
	public float phaseChangeHealth;

	// Token: 0x04000E00 RID: 3584
	private float outOfSightTime;

	// Token: 0x04000E01 RID: 3585
	private int teleportAttempts;

	// Token: 0x04000E02 RID: 3586
	private int teleportInterval = 6;

	// Token: 0x04000E03 RID: 3587
	public GameObject teleportSound;

	// Token: 0x04000E04 RID: 3588
	public GameObject decoy;

	// Token: 0x04000E05 RID: 3589
	private bool overrideRotation;

	// Token: 0x04000E06 RID: 3590
	private bool stopRotation;

	// Token: 0x04000E07 RID: 3591
	private Vector3 overrideTarget;

	// Token: 0x04000E08 RID: 3592
	private LayerMask environmentMask;

	// Token: 0x04000E09 RID: 3593
	public Transform rightHand;

	// Token: 0x04000E0A RID: 3594
	public Transform leftHand;

	// Token: 0x04000E0B RID: 3595
	private GameObject rightHandWeapon;

	// Token: 0x04000E0C RID: 3596
	private GameObject leftHandWeapon;

	// Token: 0x04000E0D RID: 3597
	private WeaponTrail rightHandTrail;

	// Token: 0x04000E0E RID: 3598
	private WeaponTrail leftHandTrail;

	// Token: 0x04000E0F RID: 3599
	private SwingCheck2 rightSwingCheck;

	// Token: 0x04000E10 RID: 3600
	private SwingCheck2 leftSwingCheck;

	// Token: 0x04000E11 RID: 3601
	public GameObject sword;

	// Token: 0x04000E12 RID: 3602
	public GameObject zweiHander;

	// Token: 0x04000E13 RID: 3603
	public GameObject axe;

	// Token: 0x04000E14 RID: 3604
	public GameObject spear;

	// Token: 0x04000E15 RID: 3605
	public GameObject glaive;

	// Token: 0x04000E16 RID: 3606
	private bool spearing;

	// Token: 0x04000E17 RID: 3607
	private int spearAttacks;

	// Token: 0x04000E18 RID: 3608
	private bool dashing;

	// Token: 0x04000E19 RID: 3609
	private float forcedDashTime;

	// Token: 0x04000E1A RID: 3610
	private Vector3 dashTarget;

	// Token: 0x04000E1B RID: 3611
	public GameObject dashEffect;

	// Token: 0x04000E1C RID: 3612
	private int throws;

	// Token: 0x04000E1D RID: 3613
	private GameObject thrownObject;

	// Token: 0x04000E1E RID: 3614
	private bool threwAxes;

	// Token: 0x04000E1F RID: 3615
	private float[] moveChanceBonuses = new float[4];

	// Token: 0x04000E20 RID: 3616
	private int previousMove = -1;

	// Token: 0x04000E21 RID: 3617
	private int burstLength = 2;

	// Token: 0x04000E22 RID: 3618
	private bool juggled;

	// Token: 0x04000E23 RID: 3619
	private float juggleHp;

	// Token: 0x04000E24 RID: 3620
	private float juggleEndHp;

	// Token: 0x04000E25 RID: 3621
	private float juggleLength;

	// Token: 0x04000E26 RID: 3622
	public GameObject juggleEffect;

	// Token: 0x04000E27 RID: 3623
	private bool juggleFalling;

	// Token: 0x04000E28 RID: 3624
	public GameObject summonedSwords;

	// Token: 0x04000E29 RID: 3625
	private GameObject currentSwords;

	// Token: 0x04000E2A RID: 3626
	public GameObject summonedSwordsWindup;

	// Token: 0x04000E2B RID: 3627
	private GameObject currentWindup;

	// Token: 0x04000E2C RID: 3628
	private float summonedSwordsCooldown = 15f;

	// Token: 0x04000E2D RID: 3629
	public Transform head;

	// Token: 0x04000E2E RID: 3630
	private bool readyTaunt;

	// Token: 0x04000E2F RID: 3631
	private float defaultAnimSpeed = 1f;

	// Token: 0x04000E30 RID: 3632
	private bool bossVersion;

	// Token: 0x04000E31 RID: 3633
	[SerializeField]
	private GameObject genericOutro;

	// Token: 0x04000E32 RID: 3634
	private int dashAttempts;
}
