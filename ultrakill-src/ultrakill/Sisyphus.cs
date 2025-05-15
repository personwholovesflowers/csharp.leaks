using System;
using System.Collections;
using System.Collections.Generic;
using NewBlood.IK;
using SettingsMenu.Components.Pages;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000400 RID: 1024
[RequireComponent(typeof(Solver3D))]
public class Sisyphus : MonoBehaviour
{
	// Token: 0x06001708 RID: 5896 RVA: 0x000BB0B4 File Offset: 0x000B92B4
	private SisyAttackAnimationDetails GetAnimationDetails(Sisyphus.AttackType type)
	{
		switch (type)
		{
		case Sisyphus.AttackType.OverheadSlam:
			return this.overheadSlamAnim;
		case Sisyphus.AttackType.HorizontalSwing:
			return this.horizontalSwingAnim;
		case Sisyphus.AttackType.Stab:
			return this.groundStabAnim;
		case Sisyphus.AttackType.AirStab:
			return this.airStabAnim;
		default:
			return null;
		}
	}

	// Token: 0x06001709 RID: 5897 RVA: 0x000BB0EC File Offset: 0x000B92EC
	private void Awake()
	{
		this.nma = base.GetComponent<NavMeshAgent>();
		this.sc = base.GetComponentInChildren<SwingCheck2>();
		this.rb = base.GetComponent<Rigidbody>();
		this.gce = base.GetComponentInChildren<GroundCheckEnemy>();
		this.col = base.GetComponent<Collider>();
		this.aud = base.GetComponent<AudioSource>();
		this.mach = base.GetComponent<Machine>();
	}

	// Token: 0x0600170A RID: 5898 RVA: 0x000BB150 File Offset: 0x000B9350
	private void Start()
	{
		this.m_Solver.Initialize();
		IKChain3D chain = this.m_Solver.GetChain(0);
		this.m_Transforms = new Transform[chain.transformCount];
		this.m_Transforms[this.m_Transforms.Length - 1] = chain.effector;
		for (int i = this.m_Transforms.Length - 2; i >= 0; i--)
		{
			this.m_Transforms[i] = this.m_Transforms[i + 1].parent;
		}
		float num = 0f;
		this.m_NormalizedDistances = new float[this.m_Transforms.Length - 1];
		for (int j = 0; j < this.m_NormalizedDistances.Length; j++)
		{
			this.m_NormalizedDistances[j] = Vector3.Distance(this.m_Transforms[j].position, this.m_Transforms[j + 1].position);
			num += this.m_NormalizedDistances[j];
		}
		for (int k = 0; k < this.m_NormalizedDistances.Length; k++)
		{
			this.m_NormalizedDistances[k] /= num;
		}
		this.m_StartPose = new Pose(this.m_Boulder.localPosition, this.m_Boulder.localRotation);
		if (this.difficulty < 4)
		{
			this.cooldown = 3f;
		}
		else
		{
			this.cooldown = 1f;
		}
		if (this.nma)
		{
			this.nma.enabled = true;
		}
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		this.anim.SetFloat(Sisyphus.s_SwingAnimSpeed, 1f * this.eid.totalSpeedModifier);
		Physics.IgnoreCollision(this.col, this.boulderCol);
		this.SetSpeed();
		this.boulderCb.sisy = this;
		if (this.jumpOnSpawn && this.eid.target != null)
		{
			this.Jump(this.eid.target.position, false);
		}
	}

	// Token: 0x0600170B RID: 5899 RVA: 0x000BB330 File Offset: 0x000B9530
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x000BB338 File Offset: 0x000B9538
	private void SetSpeed()
	{
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
		this.anim.SetFloat("DownedSpeed", (float)((this.difficulty >= 4) ? 2 : 1));
		if (this.difficulty <= 1)
		{
			this.anim.SetFloat("StompSpeed", 0.75f * this.eid.totalSpeedModifier);
			return;
		}
		if (this.difficulty == 2)
		{
			this.anim.SetFloat("StompSpeed", 0.875f * this.eid.totalSpeedModifier);
			return;
		}
		this.anim.SetFloat("StompSpeed", 1f * this.eid.totalSpeedModifier);
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x000BB434 File Offset: 0x000B9634
	private void OnDisable()
	{
		if (this.co != null)
		{
			base.StopCoroutine(this.co);
		}
		this.StopAction();
		this.ResetBoulderPose();
		this.SwingStop();
		if (this.eid.target != null)
		{
			this.swingArmSpeed = Mathf.Max(0.01f, Vector3.Distance(base.transform.position, this.eid.target.position) / 100f) * this.eid.totalSpeedModifier;
		}
		if (this.gce && !this.gce.onGround)
		{
			this.rb.isKinematic = false;
			this.rb.useGravity = true;
		}
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x000BB4E8 File Offset: 0x000B96E8
	private void OnEnable()
	{
		this.SetSpeed();
		this.anim.SetFloat(Sisyphus.s_SwingAnimSpeed, 1f * this.eid.totalSpeedModifier);
	}

	// Token: 0x0600170F RID: 5903 RVA: 0x000BB514 File Offset: 0x000B9714
	private void LateUpdate()
	{
		this.ChangeArmLength(Vector3.Distance(this.m_Transforms[0].position, this.m_Boulder.position));
		this.m_Solver.UpdateIK(1f);
		this.m_Transforms[this.m_Transforms.Length - 1].position = this.m_Boulder.position;
		if (!this.isParried)
		{
			this.m_Boulder.rotation = this.originalBoulder.rotation;
			this.m_Boulder.Rotate(Vector3.right * -90f, Space.Self);
			this.m_Boulder.Rotate(Vector3.up * -5f, Space.Self);
			return;
		}
		this.originalBoulder.transform.up = this.m_Boulder.transform.forward;
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x000BB5EC File Offset: 0x000B97EC
	private void ChangeArmLength(float targetLength)
	{
		for (int i = 0; i < this.m_NormalizedDistances.Length; i++)
		{
			Vector3 vector = Vector3.Normalize(this.m_Transforms[i + 1].position - this.m_Transforms[i].position);
			float num = targetLength * this.m_NormalizedDistances[i];
			this.m_Transforms[i + 1].position = this.m_Transforms[i].position + vector * num;
		}
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x000BB668 File Offset: 0x000B9868
	private void FixedUpdate()
	{
		if (this.eid.target != null)
		{
			if (this.inAction && (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Walking") || this.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
			{
				this.stuckInActionTimer = Mathf.MoveTowards(this.stuckInActionTimer, 2f, Time.fixedDeltaTime);
				if (this.stuckInActionTimer == 2f)
				{
					this.inAction = false;
				}
			}
			else
			{
				this.stuckInActionTimer = 0f;
			}
		}
		else
		{
			this.anim.SetBool("Walking", false);
			if (this.nma.enabled && this.nma.isOnNavMesh)
			{
				this.nma.SetDestination(base.transform.position);
			}
		}
		if (this.gce.onGround && !this.nma.isOnNavMesh && !this.nma.isOnOffMeshLink && this.eid.target != null)
		{
			if (this.gce.onGround && !this.nma.isOnNavMesh && !this.nma.isOnOffMeshLink && !this.inAction)
			{
				this.stuckChecker = Mathf.MoveTowards(this.stuckChecker, 3f, Time.fixedDeltaTime);
				if (this.stuckChecker >= 3f && !this.jumping)
				{
					this.stuckChecker = 2f;
					this.superJumping = true;
					this.Jump(this.eid.target.position, false);
				}
			}
			else
			{
				this.stuckChecker = 0f;
			}
		}
		if (this.gce.onGround && !this.superJumping && !this.inAction && this.rb.useGravity && !this.rb.isKinematic)
		{
			this.nma.enabled = true;
			this.rb.isKinematic = true;
			this.rb.useGravity = false;
			this.jumping = false;
			this.inAction = true;
			if (this.superKnockdownWindow > 0f)
			{
				this.downed = true;
				this.Knockdown(base.transform.position + base.transform.forward);
				MonoSingleton<StyleHUD>.Instance.AddPoints(60, "ultrakill.insurrknockdown", null, this.eid, -1, "", "");
				base.Invoke("Undown", 4f);
			}
			else
			{
				this.anim.Play("Landing");
				if (this.difficulty >= 1)
				{
					RaycastHit[] array = Physics.RaycastAll(base.transform.position + Vector3.up * 4f, Vector3.down, 6f, LayerMaskDefaults.Get(LMD.Environment));
					PhysicalShockwave physicalShockwave = null;
					if (array.Length != 0)
					{
						bool flag = false;
						foreach (RaycastHit raycastHit in array)
						{
							if (raycastHit.collider != this.boulderCol)
							{
								physicalShockwave = Object.Instantiate<PhysicalShockwave>(this.m_ShockwavePrefab, raycastHit.point, Quaternion.identity);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							physicalShockwave = Object.Instantiate<PhysicalShockwave>(this.m_ShockwavePrefab, base.transform.position, Quaternion.identity);
						}
					}
					else
					{
						physicalShockwave = Object.Instantiate<PhysicalShockwave>(this.m_ShockwavePrefab, base.transform.position, Quaternion.identity);
					}
					if (physicalShockwave)
					{
						physicalShockwave.transform.SetParent(this.gz.transform);
						physicalShockwave.speed *= this.eid.totalSpeedModifier;
						physicalShockwave.damage = Mathf.RoundToInt((float)physicalShockwave.damage * this.eid.totalDamageModifier);
					}
				}
			}
			if (this.fallEnemiesHit.Count > 0)
			{
				foreach (EnemyIdentifier enemyIdentifier in this.fallEnemiesHit)
				{
					Collider collider;
					if (enemyIdentifier != null && !enemyIdentifier.dead && enemyIdentifier.TryGetComponent<Collider>(out collider))
					{
						Physics.IgnoreCollision(this.col, collider, false);
					}
				}
				this.fallEnemiesHit.Clear();
			}
		}
		else if (!this.gce.onGround && this.rb.useGravity && !this.rb.isKinematic)
		{
			foreach (RaycastHit raycastHit2 in Physics.SphereCastAll(this.col.bounds.center, 2.5f, this.rb.velocity, this.rb.velocity.magnitude * Time.fixedDeltaTime + 6f, LayerMaskDefaults.Get(LMD.EnemiesAndEnvironment)))
			{
				EnemyIdentifierIdentifier enemyIdentifierIdentifier;
				if (LayerMaskDefaults.IsMatchingLayer(raycastHit2.transform.gameObject.layer, LMD.Environment))
				{
					Breakable breakable;
					Glass glass;
					if (raycastHit2.transform.TryGetComponent<Breakable>(out breakable) && !breakable.playerOnly && !breakable.specialCaseOnly && !breakable.precisionOnly)
					{
						breakable.Break();
					}
					else if (raycastHit2.transform.TryGetComponent<Glass>(out glass))
					{
						glass.Shatter();
					}
				}
				else if (raycastHit2.transform.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier.eid && enemyIdentifierIdentifier.eid != this.eid && !this.fallEnemiesHit.Contains(enemyIdentifierIdentifier.eid))
				{
					this.FallKillEnemy(enemyIdentifierIdentifier.eid);
				}
			}
			foreach (RaycastHit raycastHit3 in Physics.SphereCastAll(this.col.bounds.center, 2.5f, this.rb.velocity, this.rb.velocity.magnitude * Time.fixedDeltaTime + 6f, 4096))
			{
				EnemyIdentifier enemyIdentifier2;
				if (raycastHit3.transform != base.transform && raycastHit3.transform.TryGetComponent<EnemyIdentifier>(out enemyIdentifier2) && !this.fallEnemiesHit.Contains(enemyIdentifier2))
				{
					this.FallKillEnemy(enemyIdentifier2);
				}
			}
		}
		if (!this.inAction && this.gce.onGround && !this.jumping && this.eid.target != null)
		{
			if (this.cooldown > 0f)
			{
				this.forceCorrectOrientation = false;
				if (this.eid.target != null && Vector3.Distance(base.transform.position, this.eid.target.position) < 10f)
				{
					this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * 3f * this.eid.totalSpeedModifier);
				}
				else
				{
					this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
				}
				if (!this.stationary)
				{
					RaycastHit raycastHit4;
					if (this.nma.enabled && this.nma.isOnNavMesh && Physics.Raycast(this.eid.target.position, Vector3.down, out raycastHit4, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
					{
						NavMeshHit navMeshHit;
						if (NavMesh.SamplePosition(this.eid.target.position, out navMeshHit, 1f, this.nma.areaMask))
						{
							this.nma.SetDestination(navMeshHit.position);
						}
						else
						{
							this.nma.SetDestination(raycastHit4.point);
						}
					}
					else if (this.nma.enabled && this.nma.isOnNavMesh)
					{
						this.nma.SetDestination(this.eid.target.position);
					}
					if (this.nma.velocity.magnitude < 1f)
					{
						this.anim.SetBool("Walking", false);
					}
					else
					{
						this.anim.SetBool("Walking", true);
					}
				}
			}
			else if (Vector3.Distance(base.transform.position, this.eid.target.position) < 8f && this.difficulty != 0)
			{
				this.inAction = true;
				this.aud.pitch = Random.Range(1.4f, 1.6f);
				this.aud.PlayOneShot(this.stompVoice);
				this.anim.SetTrigger(Sisyphus.s_Stomp);
			}
			else
			{
				if ((this.attacksPerformed >= Random.Range(2, 4) || Vector3.Distance(base.transform.position, this.eid.target.position) > 100f) && Physics.Raycast(this.eid.target.position, Vector3.down, 50f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					this.Jump(this.eid.target.position, false);
					this.attacksPerformed = 0;
					return;
				}
				int num = Random.Range(0, 4);
				bool flag2 = false;
				int num2 = 0;
				while ((num == this.previousAttack || (num == 3 && this.previouslyJumped)) && num2 < 10)
				{
					num2++;
					num = Random.Range(0, 4);
					if (num2 == 10)
					{
						Debug.LogError("While method in Sisyphus' attack choosing function hit the failsafe", this);
					}
				}
				if (this.TestAttack(num))
				{
					flag2 = true;
				}
				else
				{
					int[] array3 = new int[] { 0, 1, 2, 3 };
					int num3 = 4;
					if (this.previouslyJumped)
					{
						num3 = 3;
					}
					for (int j = 0; j < num3; j++)
					{
						int num4 = array3[j];
						int num5 = Random.Range(j, num3);
						array3[j] = array3[num5];
						array3[num5] = num4;
					}
					for (int k = 0; k < 4; k++)
					{
						if (array3[k] != num && this.TestAttack(array3[k]))
						{
							flag2 = true;
							num = array3[k];
							break;
						}
					}
				}
				this.forceCorrectOrientation = false;
				if (flag2)
				{
					if (!this.stationary && this.nma.isOnNavMesh)
					{
						this.nma.SetDestination(base.transform.position);
					}
					this.inAction = true;
					this.cooldown = (float)(3 - this.difficulty / 2);
					this.previousAttack = num;
					this.previouslyJumped = false;
					switch (num)
					{
					case 0:
						this.m_AttackType = Sisyphus.AttackType.OverheadSlam;
						base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
						this.anim.SetTrigger(Sisyphus.s_OverheadSlam);
						this.trackingX = 1f;
						this.trackingY = 0.15f;
						break;
					case 1:
						this.m_AttackType = Sisyphus.AttackType.HorizontalSwing;
						this.anim.SetTrigger(Sisyphus.s_HorizontalSwing);
						this.trackingX = 0f;
						this.trackingY = 1f;
						break;
					case 2:
						this.m_AttackType = Sisyphus.AttackType.Stab;
						this.anim.SetTrigger(Sisyphus.s_Stab);
						this.trackingX = 0.9f;
						this.trackingY = 0.5f;
						break;
					case 3:
						this.m_AttackType = Sisyphus.AttackType.AirStab;
						base.StartCoroutine(this.AirStab());
						this.Jump(true);
						this.trackingX = 0f;
						this.trackingY = 0.9f;
						break;
					}
					if (num < this.attackVoices.Length && num != 3)
					{
						if (num == 1)
						{
							this.aud.pitch = Random.Range(1.4f, 1.6f);
						}
						else
						{
							this.aud.pitch = Random.Range(0.9f, 1.1f);
						}
						this.aud.PlayOneShot(this.attackVoices[num]);
					}
					if (num != 3)
					{
						Object.Instantiate<GameObject>(this.attackFlash, this.m_Boulder);
					}
					this.attacksPerformed++;
				}
				else
				{
					this.Jump(this.eid.target.position, false);
				}
			}
		}
		else if (this.inAction)
		{
			if (!this.gce.onGround)
			{
				this.rb.useGravity = false;
			}
			if (!this.dontFacePlayer)
			{
				this.RotateTowardsTarget();
			}
		}
		else if (this.jumping)
		{
			this.RotateTowardsTarget();
		}
		if (this.jumping)
		{
			Vector3 vector = new Vector3(this.jumpTarget.x, base.transform.position.y, this.jumpTarget.z);
			if (!Physics.Raycast(this.col.bounds.center, vector - base.transform.position, Vector3.Distance(base.transform.position, vector) * Time.fixedDeltaTime * 2f + 2f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector, Vector3.Distance(base.transform.position, vector) * Time.fixedDeltaTime * 2f);
			}
			if (this.superJumping)
			{
				RaycastHit raycastHit5;
				bool flag3 = Physics.SphereCast(base.transform.position, 3f, this.rb.velocity.normalized, out raycastHit5, this.rb.velocity.magnitude * Time.fixedDeltaTime, LayerMaskDefaults.Get(LMD.Environment));
				if (!flag3 && this.didCollide && Physics.SphereCast(base.transform.position, 3f, -this.rb.velocity.normalized, out raycastHit5, this.rb.velocity.magnitude * Time.fixedDeltaTime, LayerMaskDefaults.Get(LMD.Environment)))
				{
					Object.Instantiate<GameObject>(this.rubble, raycastHit5.point, Quaternion.LookRotation(raycastHit5.normal));
					this.didCollide = false;
				}
				if (flag3 && !this.didCollide)
				{
					this.didCollide = true;
					if (Vector3.Distance(base.transform.position + this.rb.velocity * Time.fixedDeltaTime, this.jumpTarget) > 3f)
					{
						Object.Instantiate<GameObject>(this.rubble, raycastHit5.point, Quaternion.LookRotation(raycastHit5.normal));
					}
				}
				if (this.rb.velocity.y >= 0f)
				{
					this.col.isTrigger = Vector3.Distance(base.transform.position, this.jumpTarget) > 1f;
				}
				else if (this.jumpTarget.y + 8f > base.transform.position.y + Mathf.Abs(this.rb.velocity.y) * Time.fixedDeltaTime)
				{
					this.col.isTrigger = false;
					this.superJumping = false;
					base.transform.position = vector;
				}
			}
		}
		if (!this.inAction && !this.gce.onGround)
		{
			this.rb.useGravity = true;
		}
		if (!this.rb.isKinematic && this.rb.useGravity)
		{
			this.rb.velocity -= Vector3.up * 200f * Time.fixedDeltaTime;
		}
		if (!this.jumping && !this.rb.isKinematic && !this.inAction)
		{
			this.anim.Play("Jump", -1, 0.95f);
			return;
		}
		if (this.gce.onGround && !this.inAction && !this.superJumping)
		{
			this.superJumping = false;
			this.jumping = false;
		}
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x000BC6A0 File Offset: 0x000BA8A0
	private void Update()
	{
		if (this.superKnockdownWindow > 0f)
		{
			this.superKnockdownWindow = Mathf.MoveTowards(this.superKnockdownWindow, 0f, Time.deltaTime);
		}
	}

	// Token: 0x06001713 RID: 5907 RVA: 0x000BC6CC File Offset: 0x000BA8CC
	private bool TestAttack(int attack)
	{
		float num = Vector3.Distance(base.transform.position, this.eid.target.position);
		LayerMask layerMask = LayerMaskDefaults.Get(LMD.Environment);
		switch (attack)
		{
		case 0:
			return !Physics.Raycast(base.transform.position, Vector3.up, num, layerMask) && !Physics.Raycast(base.transform.position + Vector3.up * num, this.eid.target.position - base.transform.position, num, layerMask) && !Physics.Raycast(this.eid.target.position, Vector3.up, num, layerMask);
		case 1:
		{
			Vector3 position = base.transform.position;
			float num2 = Vector3.Distance(this.eid.target.position, position);
			float num3 = this.eid.target.position.y - position.y;
			Vector3 vector = position + base.transform.up * 5f + base.transform.right * -num2;
			Vector3 vector2 = position + base.transform.up * 5f + Vector3.up * num3 * 2f + base.transform.right * num2;
			return !Physics.Raycast(base.transform.position + Vector3.up * 3f, -base.transform.right, num, layerMask) && !Physics.Raycast(vector, this.eid.target.position - vector, Vector3.Distance(vector, this.eid.target.position), layerMask) && !Physics.Raycast(base.transform.position + Vector3.up * 3f, base.transform.right, num, layerMask) && !Physics.Raycast(vector2, this.eid.target.position - vector2, Vector3.Distance(vector2, this.eid.target.position), layerMask);
		}
		case 2:
		{
			Vector3 vector3 = this.eid.target.position + Vector3.up * 3f;
			RaycastHit raycastHit;
			return !Physics.SphereCast(base.transform.position + Vector3.up * 3f, 1.75f, Quaternion.LookRotation(vector3 - base.transform.position, Vector3.up).eulerAngles, out raycastHit, Vector3.Distance(base.transform.position, this.eid.target.position), layerMask);
		}
		case 3:
		{
			Vector3 vector4 = base.transform.position + Vector3.up * 73f;
			Vector3 vector5 = Vector3.Normalize(this.eid.target.position - vector4);
			return !Physics.Raycast(base.transform.position + Vector3.up * 3f, base.transform.up, 70f, layerMask) && !Physics.Raycast(vector4, vector5, Vector3.Distance(vector4, this.eid.target.position), layerMask);
		}
		default:
			return false;
		}
	}

	// Token: 0x06001714 RID: 5908 RVA: 0x000BCAAB File Offset: 0x000BACAB
	public bool CanFit(Vector3 point)
	{
		return !Physics.Raycast(point, Vector3.up, 11f, LayerMaskDefaults.Get(LMD.Environment));
	}

	// Token: 0x06001715 RID: 5909 RVA: 0x000BCACD File Offset: 0x000BACCD
	private IEnumerator AirStab()
	{
		this.superJumping = false;
		yield return new WaitForSeconds(1f);
		Object.Instantiate<GameObject>(this.attackFlash, this.m_Boulder).transform.localScale *= 5f;
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.PlayOneShot(this.attackVoices[3]);
		this.trackingX = 0.9f;
		this.trackingY = 0.9f;
		this.rb.isKinematic = true;
		this.anim.SetTrigger(Sisyphus.s_AirStab);
		yield break;
	}

	// Token: 0x06001716 RID: 5910 RVA: 0x000BCADC File Offset: 0x000BACDC
	private IEnumerator AirStabAttack(float time)
	{
		if (this.eid.target == null)
		{
			yield break;
		}
		this.airStabCancelled = true;
		this.rb.isKinematic = true;
		this.ResetBoulderPose();
		Vector3 start = this.m_Boulder.position;
		float t = 0f;
		time *= this.swingArmSpeed * this.GetAnimationDetails(Sisyphus.AttackType.AirStab).finalDurationMulti;
		Vector3 attackTarget = base.transform.position + (base.transform.forward * Vector3.Distance(base.transform.position, this.eid.target.position) + base.transform.right * 3f) * this.airStabOvershoot;
		this.sc.DamageStart();
		while (this.swinging)
		{
			Vector3 vector = Vector3.LerpUnclamped(start, attackTarget, t / time);
			this.trail.transform.forward = vector - this.m_Boulder.position;
			this.m_Boulder.position = vector;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			if (Physics.OverlapSphere(this.m_Boulder.position, 3.75f, LayerMaskDefaults.Get(LMD.Environment)).Length != 0)
			{
				this.SlamShockwave();
				this.SwingStop();
				this.swinging = false;
				this.trackingX = 0f;
				this.trackingY = 0f;
			}
		}
		this.trackingX = 0.75f;
		this.trackingY = 0f;
		this.SwingStop();
		yield break;
	}

	// Token: 0x06001717 RID: 5911 RVA: 0x000BCAF4 File Offset: 0x000BACF4
	public void ExtendArm(float time)
	{
		SisyAttackAnimationDetails animationDetails = this.GetAnimationDetails(this.m_AttackType);
		this.boulderCol.enabled = false;
		this.trail.emitting = true;
		this.swingParticle.Play();
		this.swinging = true;
		this.swingAudio.Play();
		this.boulderCb.launchable = true;
		float num = Vector3.Distance(base.transform.position, this.GetActualTargetPos());
		if (num < 10f)
		{
			num = 10f;
		}
		num -= 10f;
		this.swingArmSpeed = Mathf.Clamp(num / animationDetails.boulderDistanceDivide, animationDetails.minBoulderSpeed, animationDetails.maxBoulderSpeed);
		if (this.m_AttackType == Sisyphus.AttackType.AirStab)
		{
			num *= 0.35f;
			this.swingArmSpeed /= this.airStabOvershoot;
		}
		float num2 = 1f - num / animationDetails.boulderDistanceDivide;
		num2 *= animationDetails.speedDistanceMulti;
		num2 = Mathf.Clamp(num2, animationDetails.minAnimSpeedCap, animationDetails.maxAnimSpeedCap);
		Sisyphus.AttackType attackType = this.m_AttackType;
		float num3 = 1f;
		if (this.difficulty >= 4)
		{
			num3 = 1.5f;
		}
		else if (this.difficulty == 3)
		{
			num3 = 1.25f;
		}
		else if (this.difficulty == 1)
		{
			num3 = 0.75f;
		}
		else if (this.difficulty == 0)
		{
			num3 = 0.5f;
		}
		num3 *= this.eid.totalSpeedModifier;
		num2 *= num3;
		this.swingArmSpeed /= num3;
		this.anim.SetFloat(Sisyphus.s_SwingAnimSpeed, num2);
		if (this.m_AttackType == Sisyphus.AttackType.OverheadSlam)
		{
			this.co = base.StartCoroutine(this.OverheadSlamAttack(time));
			return;
		}
		if (this.m_AttackType == Sisyphus.AttackType.HorizontalSwing)
		{
			this.co = base.StartCoroutine(this.HorizontalSwingAttack(time));
			return;
		}
		if (this.m_AttackType == Sisyphus.AttackType.Stab)
		{
			this.co = base.StartCoroutine(this.StabAttack(time));
			return;
		}
		if (this.m_AttackType == Sisyphus.AttackType.AirStab)
		{
			this.co = base.StartCoroutine(this.AirStabAttack(time));
		}
	}

	// Token: 0x06001718 RID: 5912 RVA: 0x000BCCE0 File Offset: 0x000BAEE0
	public void RetractArm(float time)
	{
		this.inAction = false;
		this.anim.SetFloat(Sisyphus.s_SwingAnimSpeed, 1f * this.eid.totalSpeedModifier);
		if (this.eid.target != null)
		{
			this.swingArmSpeed = Mathf.Max(0.01f, Vector3.Distance(base.transform.position, this.eid.target.position) / 100f);
		}
		this.TryToRetractArm(time);
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x000BCD60 File Offset: 0x000BAF60
	private Vector3 GetActualTargetPos()
	{
		if (this.eid.target == null)
		{
			return base.transform.position;
		}
		switch (this.m_AttackType)
		{
		case Sisyphus.AttackType.OverheadSlam:
		{
			Vector3 position = base.transform.position;
			position.y = this.eid.target.position.y;
			return position + base.transform.forward * (Vector3.Distance(position, this.eid.target.position) - 0.5f) - base.transform.forward;
		}
		case Sisyphus.AttackType.HorizontalSwing:
		{
			Vector3 vector = this.eid.target.position - base.transform.forward * 3f;
			vector.y = this.eid.target.position.y;
			return vector;
		}
		case Sisyphus.AttackType.AirStab:
			return this.eid.target.position + base.transform.right * 10f;
		}
		return this.eid.target.position;
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x000BCE98 File Offset: 0x000BB098
	private bool SwingCheck(bool noExplosion = false)
	{
		if (Physics.OverlapSphere(this.m_Boulder.position, 0.75f, LayerMaskDefaults.Get(LMD.Environment)).Length != 0)
		{
			if (!noExplosion)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, this.m_Boulder.position + this.m_Boulder.forward, Quaternion.identity);
				this.SetupExplosion(gameObject);
			}
			this.SwingStop();
			return true;
		}
		return false;
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x000BCF08 File Offset: 0x000BB108
	private void SetupExplosion(GameObject temp)
	{
		PhysicalShockwave physicalShockwave;
		if (temp.TryGetComponent<PhysicalShockwave>(out physicalShockwave))
		{
			physicalShockwave.target = this.eid.target;
		}
		if (this.difficulty <= 2 || this.eid.totalDamageModifier != 1f || this.eid.totalSpeedModifier != 1f)
		{
			foreach (Explosion explosion in temp.GetComponentsInChildren<Explosion>())
			{
				if (this.difficulty <= 2)
				{
					explosion.maxSize *= 0.66f;
					explosion.speed /= 0.66f;
				}
				explosion.maxSize *= this.eid.totalDamageModifier;
				explosion.speed *= this.eid.totalDamageModifier;
				explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.eid.totalDamageModifier);
			}
		}
	}

	// Token: 0x0600171C RID: 5916 RVA: 0x000BCFFA File Offset: 0x000BB1FA
	private IEnumerator HorizontalSwingAttack(float time)
	{
		this.ResetBoulderPose();
		float t = 0f;
		time *= this.swingArmSpeed * this.GetAnimationDetails(Sisyphus.AttackType.HorizontalSwing).finalDurationMulti;
		Vector3 actualTarget = this.GetActualTargetPos();
		this.sc.DamageStart();
		while (t < time / 3f && this.swinging)
		{
			float num = Vector3.Distance(actualTarget, base.transform.position);
			Vector3 vector = base.transform.position + base.transform.up * 5f + base.transform.right * -num;
			Vector3 vector2 = this.m_Boulder.parent.TransformPoint(this.m_StartPose.position);
			Debug.DrawLine(base.transform.position, vector, Color.red, 8f);
			Vector3 vector3 = Vector3.Lerp(vector2, vector, t / (time / 2f));
			this.trail.transform.forward = vector3 - this.m_Boulder.position;
			this.m_Boulder.transform.position = vector3;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			if (this.SwingCheck(true))
			{
				yield return new WaitForSeconds(0.5f);
				this.RetractArm(0.5f);
				yield break;
			}
		}
		t = 0f;
		float progressEnd = time / 1.5f;
		float yPos = actualTarget.y;
		while (t < progressEnd && this.swinging)
		{
			float num2 = t / progressEnd;
			if (num2 <= 0.5f)
			{
				actualTarget = this.GetActualTargetPos();
				actualTarget.y = yPos + 2f;
			}
			Vector3 position = base.transform.position;
			float num3 = Vector3.Distance(actualTarget, position);
			float num4 = actualTarget.y - position.y;
			Vector3 vector4 = position + base.transform.up * 5f + base.transform.right * -num3;
			Vector3 vector5 = position + base.transform.up * 5f + Vector3.up * num4 * 2f + base.transform.right * num3;
			this.trackingY = 1f;
			Quaternion quaternion = Quaternion.LookRotation(vector4 - position, Vector3.up);
			Quaternion quaternion2 = Quaternion.LookRotation(vector5 - position, Vector3.up);
			Quaternion quaternion3 = Quaternion.LookRotation(actualTarget - position, Vector3.up);
			Quaternion quaternion4 = ((num2 > 0.5f) ? Quaternion.Lerp(quaternion3, quaternion2, (num2 - 0.5f) * 2f) : Quaternion.Lerp(quaternion, quaternion3, num2 * 2f));
			Vector3 vector6 = position + quaternion4 * Vector3.forward * num3;
			this.trail.transform.forward = vector6 - this.m_Boulder.position;
			this.m_Boulder.position = vector6;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			if (this.SwingCheck(false))
			{
				yield return new WaitForSeconds(0.5f);
				this.RetractArm(0.5f);
				yield break;
			}
		}
		this.SwingStop();
		this.TryToRetractArm(2f);
		yield break;
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x000BD010 File Offset: 0x000BB210
	private IEnumerator OverheadSlamAttack(float time)
	{
		this.ResetBoulderPose();
		Vector3 start = this.m_Boulder.position;
		float t = 0f;
		time *= this.swingArmSpeed * this.GetAnimationDetails(Sisyphus.AttackType.OverheadSlam).finalDurationMulti;
		this.sc.DamageStart();
		Vector3 vector = this.GetActualTargetPos();
		while (t < time)
		{
			Vector3 vector2 = Vector3.Lerp(start, vector, t / time);
			vector2.y += Vector3.Distance(start, vector) * Mathf.Sin(Mathf.Clamp01(t / time) * 3.1415927f);
			this.trail.transform.forward = vector2 - this.m_Boulder.position;
			this.m_Boulder.position = vector2;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			vector = this.GetActualTargetPos();
		}
		if (this.swinging)
		{
			if (Physics.OverlapSphere(this.m_Boulder.position, 5f, LayerMaskDefaults.Get(LMD.Environment)).Length != 0)
			{
				this.SlamShockwave();
				this.SwingStop();
			}
			else
			{
				bool hit = false;
				t = 0f;
				while (!hit)
				{
					Vector3 position = this.m_Boulder.position;
					position.y -= Time.deltaTime * this.swingArmSpeed * 400f;
					this.trail.transform.forward = position - this.m_Boulder.position;
					this.m_Boulder.position = position;
					if (Physics.OverlapSphere(this.m_Boulder.position, 5f, LayerMaskDefaults.Get(LMD.Environment)).Length != 0)
					{
						this.SlamShockwave();
						this.SwingStop();
						hit = true;
					}
					yield return new WaitForEndOfFrame();
					if (t > 1.5f)
					{
						hit = true;
					}
					t += Time.deltaTime;
				}
			}
		}
		this.trackingY = 0f;
		this.sc.DamageStop();
		yield return new WaitForSeconds(1f);
		this.TryToRetractArm(2f);
		yield break;
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x000BD028 File Offset: 0x000BB228
	private void SlamShockwave()
	{
		Collider[] array = Physics.OverlapSphere(this.m_Boulder.position, 3.5f, LayerMaskDefaults.Get(LMD.Environment));
		if (array.Length != 0)
		{
			float num = 5f;
			Vector3 vector = this.m_Boulder.position;
			for (int i = 0; i < array.Length; i++)
			{
				Vector3 vector2 = array[i].ClosestPoint(this.m_Boulder.position);
				if (Vector3.Distance(this.m_Boulder.position, vector2) < num)
				{
					vector = vector2;
					num = Vector3.Distance(this.m_Boulder.position, vector2);
				}
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, vector + Vector3.up * 0.1f, Quaternion.identity);
			this.m_Boulder.position = vector;
			this.SetupExplosion(gameObject);
			return;
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(this.explosion, this.m_Boulder.position, Quaternion.identity);
		this.m_Boulder.position -= Vector3.up * 2f;
		this.SetupExplosion(gameObject2);
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x000BD146 File Offset: 0x000BB346
	private IEnumerator StabAttack(float time)
	{
		if (this.eid.target == null)
		{
			yield break;
		}
		this.ResetBoulderPose();
		Vector3 start = this.m_Boulder.position;
		float t = 0f;
		time *= this.swingArmSpeed * this.GetAnimationDetails(Sisyphus.AttackType.Stab).finalDurationMulti;
		Vector3 vector = this.eid.target.position + Vector3.up * 3f;
		Vector3 attackTarget = base.transform.position + base.transform.forward * Vector3.Distance(base.transform.position, vector);
		attackTarget.y = vector.y;
		this.trackingX = 0f;
		this.trackingY = 0f;
		this.sc.DamageStart();
		bool canCancel = false;
		while (this.swinging)
		{
			Vector3 vector2 = Vector3.LerpUnclamped(start, attackTarget, t / time);
			if (!canCancel && Vector3.Distance(start, vector2) >= 20f)
			{
				canCancel = true;
			}
			this.trail.transform.forward = vector2 - this.m_Boulder.position;
			this.m_Boulder.position = vector2;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
			if (canCancel && Physics.OverlapSphere(this.m_Boulder.position, 2f, LayerMaskDefaults.Get(LMD.Environment)).Length != 0)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, this.m_Boulder.position + this.m_Boulder.forward, Quaternion.identity);
				this.SetupExplosion(gameObject);
				this.anim.Play(Sisyphus.s_Stab, -1, 0.73f);
				this.SwingStop();
				yield return new WaitForSeconds(0.5f);
				this.RetractArm(0.5f);
			}
		}
		this.sc.DamageStop();
		yield break;
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x000BD15C File Offset: 0x000BB35C
	public void TryToRetractArm(float time)
	{
		if (!this.swinging)
		{
			return;
		}
		this.swinging = false;
		this.boulderCol.enabled = true;
		this.boulderCb.Unlaunch(false);
		this.SwingStop();
		this.co = base.StartCoroutine(this.RetractArmAsync(time));
		this.isParried = false;
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000BD1B4 File Offset: 0x000BB3B4
	public void SwingStop()
	{
		this.trail.emitting = false;
		ParticleSystem particleSystem = this.swingParticle;
		if (particleSystem != null)
		{
			particleSystem.Stop();
		}
		SwingCheck2 swingCheck = this.sc;
		if (swingCheck != null)
		{
			swingCheck.DamageStop();
		}
		AudioSource audioSource = this.swingAudio;
		if (audioSource != null)
		{
			audioSource.Stop();
		}
		Cannonball cannonball = this.boulderCb;
		if (cannonball != null)
		{
			cannonball.Unlaunch(false);
		}
		this.isParried = false;
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x000BD219 File Offset: 0x000BB419
	private IEnumerator RetractArmAsync(float time)
	{
		float t = 0f;
		Vector3 boulderStart = this.m_Boulder.position;
		Transform oldBoulderParent = this.m_Boulder.parent;
		Vector3 bossStart = base.transform.position;
		if (this.pullSelfRetract)
		{
			this.m_Boulder.SetParent(base.transform.parent ? base.transform.parent : null);
		}
		while (t < time)
		{
			Vector3 vector;
			if (this.pullSelfRetract)
			{
				vector = this.m_Boulder.transform.position;
			}
			else
			{
				vector = this.m_Boulder.parent.TransformPoint(this.m_StartPose.position);
			}
			(this.pullSelfRetract ? base.transform : this.m_Boulder).position = Vector3.Lerp(this.pullSelfRetract ? bossStart : boulderStart, vector, t / time);
			bool flag = this.pullSelfRetract;
			yield return new WaitForEndOfFrame();
			t += Time.deltaTime;
		}
		if (this.pullSelfRetract)
		{
			this.m_Boulder.SetParent(oldBoulderParent);
			this.rb.isKinematic = false;
			this.rb.useGravity = true;
			base.transform.rotation = Quaternion.identity;
			this.rb.AddForce(Vector3.down * 300f, ForceMode.VelocityChange);
			this.pullSelfRetract = false;
			this.ResetBoulderPose();
			this.StopAction();
		}
		yield break;
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000BD22F File Offset: 0x000BB42F
	private void Jump(bool noEnd = false)
	{
		this.Jump(base.transform.position, noEnd);
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x000BD244 File Offset: 0x000BB444
	private void Jump(Vector3 target, bool noEnd = false)
	{
		if (this.jumping || this.stationary)
		{
			return;
		}
		this.previouslyJumped = true;
		if (RaycastHelper.RaycastAndDebugDraw(target, Vector3.up, 50f, LayerMaskDefaults.Get(LMD.Environment)) || RaycastHelper.RaycastAndDebugDraw(this.col.bounds.center, Vector3.up, 25f, LayerMaskDefaults.Get(LMD.Environment)) || RaycastHelper.RaycastAndDebugDraw(this.col.bounds.center, target - this.col.bounds.center, Vector3.Distance(this.col.bounds.center, target), LayerMaskDefaults.Get(LMD.Environment)))
		{
			this.superJumping = true;
		}
		this.didCollide = false;
		this.jumpTarget = target;
		if (this.superJumping)
		{
			NavMeshHit navMeshHit;
			RaycastHit raycastHit;
			if (NavMesh.SamplePosition(target, out navMeshHit, 2f, this.nma.areaMask))
			{
				this.jumpTarget = navMeshHit.position;
			}
			else if (Physics.Raycast(target, -Vector3.up, out raycastHit, 3f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.jumpTarget = raycastHit.point;
			}
			if (!this.CanFit(this.jumpTarget))
			{
				int num = 60;
				float num2 = Random.Range(0f, 360f);
				for (int i = 0; i < 360; i += num)
				{
					Vector3 vector = this.jumpTarget + Quaternion.Euler(0f, (float)i + num2, 0f) * Vector3.forward * 12f;
					vector += Vector3.up * 2f;
					if (!Physics.Linecast(this.jumpTarget + Vector3.up * 2f, vector, LayerMaskDefaults.Get(LMD.Environment)))
					{
						Debug.DrawRay(vector, Vector3.down * 50f, Color.yellow, 50f);
						RaycastHit raycastHit2;
						if (Physics.Raycast(vector, Vector3.down, out raycastHit2, 50f, LayerMaskDefaults.Get(LMD.Environment)))
						{
							if (!this.CanFit(raycastHit2.point))
							{
								i += num;
								continue;
							}
							this.jumpTarget = raycastHit2.point;
							break;
						}
					}
				}
			}
		}
		this.jumping = true;
		this.anim.Play("Jump");
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		if (this.superJumping)
		{
			this.col.isTrigger = true;
		}
		this.nma.enabled = false;
		Object.Instantiate<GameObject>(this.rubble, base.transform.position, base.transform.rotation);
		this.rb.velocity = Vector3.zero;
		this.rb.AddForce(Vector3.up * Mathf.Max(50f, 100f + Vector3.Distance(base.transform.position, target)), ForceMode.VelocityChange);
		this.trackingX = 0f;
		this.trackingY = 1f;
		this.inAction = true;
		if (!noEnd)
		{
			base.Invoke("StopAction", 0.5f);
		}
	}

	// Token: 0x06001725 RID: 5925 RVA: 0x000BD594 File Offset: 0x000BB794
	private void FlyToArm()
	{
		if (this.airStabCancelled)
		{
			return;
		}
		this.inAction = false;
		this.pullSelfRetract = true;
		this.forceCorrectOrientation = true;
		this.trackingX = 0.3f;
		this.aud.pitch = Random.Range(1.4f, 1.6f);
		this.aud.PlayOneShot(this.attackVoices[3]);
		this.anim.SetFloat(Sisyphus.s_SwingAnimSpeed, 1f);
		this.swinging = true;
		this.TryToRetractArm(0.4f);
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x000BD620 File Offset: 0x000BB820
	private void CancelAirStab()
	{
		Vector3 position = base.transform.position;
		if (this.eid.target != null)
		{
			position.y = this.eid.target.position.y;
		}
		if (this.eid.target != null && Vector3.Distance(position, this.eid.target.position) > Vector3.Distance(this.m_Boulder.position, this.eid.target.position) && !this.swinging)
		{
			this.airStabCancelled = false;
			return;
		}
		this.inAction = false;
		this.airStabCancelled = true;
		this.pullSelfRetract = false;
		this.swinging = true;
		this.RetractArm(1f);
		this.anim.SetTrigger(Sisyphus.s_AirStabCancel);
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.nma.enabled = false;
		this.rb.velocity = Vector3.zero;
		this.forceCorrectOrientation = true;
		this.trackingX = 0.3f;
		this.trackingY = 1f;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x000BD740 File Offset: 0x000BB940
	public void Death()
	{
		this.aud.pitch = Random.Range(0.9f, 1.1f);
		this.aud.PlayOneShot(this.deathVoice);
		GoreZone componentInParent = base.GetComponentInParent<GoreZone>();
		foreach (Transform transform in this.legs)
		{
			transform.parent = componentInParent.gibZone;
			foreach (Rigidbody rigidbody in transform.GetComponentsInChildren<Rigidbody>())
			{
				rigidbody.isKinematic = false;
				rigidbody.useGravity = true;
			}
		}
		EnemyIdentifierIdentifier[] componentsInChildren2 = base.GetComponentsInChildren<EnemyIdentifierIdentifier>();
		if (GraphicsSettings.bloodEnabled)
		{
			foreach (EnemyIdentifierIdentifier enemyIdentifierIdentifier in componentsInChildren2)
			{
				GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Head, enemyIdentifierIdentifier.eid, false);
				if (gore)
				{
					gore.transform.position = enemyIdentifierIdentifier.transform.position;
					gore.transform.SetParent(componentInParent.goreZone, true);
					gore.SetActive(true);
				}
				for (int k = 0; k < 3; k++)
				{
					GameObject gib = MonoSingleton<BloodsplatterManager>.Instance.GetGib(BSType.gib);
					if (gib)
					{
						gib.transform.SetPositionAndRotation(enemyIdentifierIdentifier.transform.position, Random.rotation);
						gib.transform.SetParent(componentInParent.gibZone, true);
						gib.transform.localScale *= 4f;
					}
				}
			}
		}
		this.armature.localScale = Vector3.zero;
		Collider[] componentsInChildren3 = base.GetComponentsInChildren<Collider>();
		for (int l = componentsInChildren3.Length - 1; l >= 0; l--)
		{
			Object.Destroy(componentsInChildren3[l]);
		}
		Object.Destroy(this.m_Boulder.gameObject);
		Object.Destroy(this.anim);
		Object.Destroy(this);
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x000BD921 File Offset: 0x000BBB21
	private void StopAction()
	{
		this.inAction = false;
		this.dontFacePlayer = false;
		this.knockedDownByCannonball = false;
	}

	// Token: 0x06001729 RID: 5929 RVA: 0x000BD938 File Offset: 0x000BBB38
	private void ResetBoulderPose()
	{
		this.m_Boulder.localPosition = this.m_StartPose.position;
		this.m_Boulder.localRotation = this.m_StartPose.rotation;
		this.boulderCb.Unlaunch(true);
		this.isParried = false;
	}

	// Token: 0x0600172A RID: 5930 RVA: 0x000BD984 File Offset: 0x000BBB84
	private void RotateTowardsTarget()
	{
		if (this.eid.target == null)
		{
			return;
		}
		Vector3 position = this.eid.target.position;
		if (this.gce.onGround || this.forceCorrectOrientation)
		{
			position = new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z);
		}
		Quaternion quaternion = Quaternion.LookRotation(position - base.transform.position);
		float num = (Quaternion.Angle(base.transform.rotation, quaternion) * 10f + 30f) * Time.fixedDeltaTime;
		float num2 = base.transform.rotation.eulerAngles.x;
		float num3 = base.transform.rotation.eulerAngles.y;
		while (num2 - quaternion.eulerAngles.x > 180f)
		{
			num2 -= 360f;
		}
		while (num2 - quaternion.eulerAngles.x < -180f)
		{
			num2 += 360f;
		}
		while (num3 - quaternion.eulerAngles.y > 180f)
		{
			num3 -= 360f;
		}
		while (num3 - quaternion.eulerAngles.y < -180f)
		{
			num3 += 360f;
		}
		float num4 = 1f;
		if (this.difficulty == 1)
		{
			num4 = 0.75f;
		}
		else if (this.difficulty == 0)
		{
			num4 = 0.5f;
		}
		base.transform.rotation = Quaternion.Euler(Mathf.MoveTowards(num2, quaternion.eulerAngles.x, num * this.trackingX * num4), Mathf.MoveTowards(num3, quaternion.eulerAngles.y, num * this.trackingY * num4), Mathf.MoveTowards(base.transform.rotation.eulerAngles.z, quaternion.eulerAngles.z, num));
	}

	// Token: 0x0600172B RID: 5931 RVA: 0x000BDB98 File Offset: 0x000BBD98
	public void StompExplosion()
	{
		Vector3 vector = base.transform.position + Vector3.up;
		if (Physics.Raycast(vector, this.eid.target.position - vector, Vector3.Distance(this.eid.target.position, vector), LayerMaskDefaults.Get(LMD.Environment)))
		{
			vector = base.transform.position + Vector3.up * 5f;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.explosion, vector, Quaternion.identity);
		if (this.difficulty <= 2 || this.eid.totalDamageModifier != 1f || this.eid.totalSpeedModifier != 1f)
		{
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				if (this.difficulty >= 3)
				{
					explosion.maxSize *= 1.5f;
					explosion.speed *= 1.5f;
				}
				explosion.maxSize *= this.eid.totalDamageModifier;
				explosion.speed *= this.eid.totalDamageModifier;
				explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.eid.totalDamageModifier);
			}
		}
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x000BDD00 File Offset: 0x000BBF00
	public void PlayHurtSound(int type = 0)
	{
		if (this.currentHurtSound)
		{
			if (type == 0)
			{
				return;
			}
			Object.Destroy(this.currentHurtSound);
		}
		this.currentHurtSound = Object.Instantiate<GameObject>(this.hurtSounds[type], base.transform.position, Quaternion.identity);
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x000BDD4C File Offset: 0x000BBF4C
	public void GotParried()
	{
		this.isParried = true;
		if (this.co != null)
		{
			base.StopCoroutine(this.co);
		}
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x000BDD6C File Offset: 0x000BBF6C
	public void Knockdown(Vector3 boulderPos)
	{
		if (!this.pullSelfRetract)
		{
			if (this.co != null)
			{
				base.StopCoroutine(this.co);
			}
			if (!this.knockedDownByCannonball)
			{
				base.transform.LookAt(new Vector3(boulderPos.x, base.transform.position.y, boulderPos.z));
			}
			if (!this.inAction && this.gce.onGround)
			{
				this.inAction = true;
				if (!this.stationary && this.nma.isOnNavMesh)
				{
					this.nma.SetDestination(base.transform.position);
				}
			}
		}
		if (!this.gce.onGround)
		{
			this.superKnockdownWindow = 0.25f;
		}
		this.dontFacePlayer = true;
		if (this.gce.onGround && !this.knockedDownByCannonball)
		{
			this.knockedDownByCannonball = true;
			this.anim.Play("Knockdown");
		}
		this.PlayHurtSound(2);
		this.trackingX = 0f;
		this.trackingY = 0f;
		if (this.knockedDownByCannonball)
		{
			base.Invoke("CheckLoop", 0.85f);
		}
		GameObject gore = MonoSingleton<BloodsplatterManager>.Instance.GetGore(GoreType.Splatter, this.eid, false);
		if (gore)
		{
			gore.transform.position = boulderPos;
			gore.transform.up = base.transform.forward;
			gore.transform.SetParent(base.GetComponentInParent<GoreZone>().goreZone, true);
			gore.SetActive(true);
			Bloodsplatter bloodsplatter;
			if (gore.TryGetComponent<Bloodsplatter>(out bloodsplatter))
			{
				bloodsplatter.GetReady();
			}
		}
		if (!this.pullSelfRetract)
		{
			this.ResetBoulderPose();
			this.SwingStop();
		}
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x000BDF13 File Offset: 0x000BC113
	public void FallSound()
	{
		MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
		Object.Instantiate<GameObject>(this.fallSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x000BDF40 File Offset: 0x000BC140
	private void FallKillEnemy(EnemyIdentifier eid)
	{
		eid.hitter = "enemy";
		this.fallEnemiesHit.Add(eid);
		Collider collider;
		if (eid.TryGetComponent<Collider>(out collider))
		{
			Physics.IgnoreCollision(this.col, collider, true);
		}
		EnemyIdentifier.FallOnEnemy(eid);
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x000BDF84 File Offset: 0x000BC184
	public void CheckLoop()
	{
		if (this.downed)
		{
			this.anim.SetFloat("DownedSpeed", 0f);
			base.Invoke("CheckLoop", 0.1f);
			return;
		}
		this.anim.SetFloat("DownedSpeed", (float)((this.difficulty >= 4) ? 2 : 1));
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x000BDFDD File Offset: 0x000BC1DD
	private void Undown()
	{
		this.downed = false;
	}

	// Token: 0x04002029 RID: 8233
	private static readonly int s_SwingAnimSpeed = Animator.StringToHash("SwingSpeed");

	// Token: 0x0400202A RID: 8234
	private float swingArmSpeed;

	// Token: 0x0400202B RID: 8235
	private static readonly int s_OverheadSlam = Animator.StringToHash("OverheadSlam");

	// Token: 0x0400202C RID: 8236
	private static readonly int s_HorizontalSwing = Animator.StringToHash("HorizontalSwing");

	// Token: 0x0400202D RID: 8237
	private static readonly int s_Stab = Animator.StringToHash("Stab");

	// Token: 0x0400202E RID: 8238
	private static readonly int s_AirStab = Animator.StringToHash("AirStab");

	// Token: 0x0400202F RID: 8239
	private static readonly int s_AirStabCancel = Animator.StringToHash("AirStabCancel");

	// Token: 0x04002030 RID: 8240
	private static readonly int s_Stomp = Animator.StringToHash("Stomp");

	// Token: 0x04002031 RID: 8241
	[SerializeField]
	private Solver3D m_Solver;

	// Token: 0x04002032 RID: 8242
	[SerializeField]
	private Animator anim;

	// Token: 0x04002033 RID: 8243
	[SerializeField]
	private Transform m_Boulder;

	// Token: 0x04002034 RID: 8244
	[SerializeField]
	private Collider boulderCol;

	// Token: 0x04002035 RID: 8245
	[SerializeField]
	private PhysicalShockwave m_ShockwavePrefab;

	// Token: 0x04002036 RID: 8246
	[SerializeField]
	private GameObject explosion;

	// Token: 0x04002037 RID: 8247
	private Pose m_StartPose;

	// Token: 0x04002038 RID: 8248
	private Sisyphus.AttackType m_AttackType;

	// Token: 0x04002039 RID: 8249
	private float[] m_NormalizedDistances;

	// Token: 0x0400203A RID: 8250
	private Transform[] m_Transforms;

	// Token: 0x0400203B RID: 8251
	private bool didCollide;

	// Token: 0x0400203C RID: 8252
	private bool airStabCancelled;

	// Token: 0x0400203D RID: 8253
	private bool pullSelfRetract;

	// Token: 0x0400203E RID: 8254
	private bool swinging;

	// Token: 0x0400203F RID: 8255
	private bool inAction;

	// Token: 0x04002040 RID: 8256
	private float stuckInActionTimer;

	// Token: 0x04002041 RID: 8257
	private int attacksPerformed;

	// Token: 0x04002042 RID: 8258
	private int previousAttack = -1;

	// Token: 0x04002043 RID: 8259
	private bool previouslyJumped;

	// Token: 0x04002044 RID: 8260
	private float cooldown;

	// Token: 0x04002045 RID: 8261
	private NavMeshAgent nma;

	// Token: 0x04002046 RID: 8262
	private SwingCheck2 sc;

	// Token: 0x04002047 RID: 8263
	private float airStabOvershoot = 2f;

	// Token: 0x04002048 RID: 8264
	private float stabOvershoot = 1.1f;

	// Token: 0x04002049 RID: 8265
	private GroundCheckEnemy gce;

	// Token: 0x0400204A RID: 8266
	private Rigidbody rb;

	// Token: 0x0400204B RID: 8267
	private bool jumping;

	// Token: 0x0400204C RID: 8268
	private Vector3 jumpTarget;

	// Token: 0x0400204D RID: 8269
	private bool superJumping;

	// Token: 0x0400204E RID: 8270
	private float trackingX;

	// Token: 0x0400204F RID: 8271
	private float trackingY;

	// Token: 0x04002050 RID: 8272
	private bool forceCorrectOrientation;

	// Token: 0x04002051 RID: 8273
	private Collider col;

	// Token: 0x04002052 RID: 8274
	[SerializeField]
	private GameObject rubble;

	// Token: 0x04002053 RID: 8275
	[SerializeField]
	private TrailRenderer trail;

	// Token: 0x04002054 RID: 8276
	[SerializeField]
	private ParticleSystem swingParticle;

	// Token: 0x04002055 RID: 8277
	[SerializeField]
	private AudioSource swingAudio;

	// Token: 0x04002056 RID: 8278
	public bool stationary;

	// Token: 0x04002057 RID: 8279
	private AudioSource aud;

	// Token: 0x04002058 RID: 8280
	[SerializeField]
	private AudioClip[] attackVoices;

	// Token: 0x04002059 RID: 8281
	[SerializeField]
	private AudioClip stompVoice;

	// Token: 0x0400205A RID: 8282
	[SerializeField]
	private AudioClip deathVoice;

	// Token: 0x0400205B RID: 8283
	[SerializeField]
	private GameObject[] hurtSounds;

	// Token: 0x0400205C RID: 8284
	private GameObject currentHurtSound;

	// Token: 0x0400205D RID: 8285
	[SerializeField]
	private Transform[] legs;

	// Token: 0x0400205E RID: 8286
	[SerializeField]
	private Transform armature;

	// Token: 0x0400205F RID: 8287
	private int difficulty = -1;

	// Token: 0x04002060 RID: 8288
	[SerializeField]
	private GameObject attackFlash;

	// Token: 0x04002061 RID: 8289
	private float stuckChecker;

	// Token: 0x04002062 RID: 8290
	private EnemyIdentifier eid;

	// Token: 0x04002063 RID: 8291
	private GoreZone gz;

	// Token: 0x04002064 RID: 8292
	private Machine mach;

	// Token: 0x04002065 RID: 8293
	private Coroutine co;

	// Token: 0x04002066 RID: 8294
	[SerializeField]
	private Cannonball boulderCb;

	// Token: 0x04002067 RID: 8295
	private bool isParried;

	// Token: 0x04002068 RID: 8296
	[SerializeField]
	private Transform originalBoulder;

	// Token: 0x04002069 RID: 8297
	[HideInInspector]
	public bool knockedDownByCannonball;

	// Token: 0x0400206A RID: 8298
	[SerializeField]
	private GameObject fallSound;

	// Token: 0x0400206B RID: 8299
	private List<EnemyIdentifier> fallEnemiesHit = new List<EnemyIdentifier>();

	// Token: 0x0400206C RID: 8300
	[Header("Animations")]
	[SerializeField]
	private SisyAttackAnimationDetails overheadSlamAnim;

	// Token: 0x0400206D RID: 8301
	[SerializeField]
	private SisyAttackAnimationDetails horizontalSwingAnim;

	// Token: 0x0400206E RID: 8302
	[SerializeField]
	private SisyAttackAnimationDetails groundStabAnim;

	// Token: 0x0400206F RID: 8303
	[SerializeField]
	private SisyAttackAnimationDetails airStabAnim;

	// Token: 0x04002070 RID: 8304
	[HideInInspector]
	public bool downed;

	// Token: 0x04002071 RID: 8305
	public bool jumpOnSpawn;

	// Token: 0x04002072 RID: 8306
	private bool dontFacePlayer;

	// Token: 0x04002073 RID: 8307
	private float superKnockdownWindow;

	// Token: 0x02000401 RID: 1025
	private enum AttackType
	{
		// Token: 0x04002075 RID: 8309
		OverheadSlam,
		// Token: 0x04002076 RID: 8310
		HorizontalSwing,
		// Token: 0x04002077 RID: 8311
		Stab,
		// Token: 0x04002078 RID: 8312
		AirStab
	}
}
