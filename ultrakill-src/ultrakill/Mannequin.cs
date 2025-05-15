using System;
using ULTRAKILL.Cheats.UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020002E7 RID: 743
public class Mannequin : MonoBehaviour
{
	// Token: 0x17000163 RID: 355
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x0007A861 File Offset: 0x00078A61
	private static bool debug
	{
		get
		{
			return MannequinDebugGizmos.Enabled;
		}
	}

	// Token: 0x0600101B RID: 4123 RVA: 0x0007A868 File Offset: 0x00078A68
	private void Awake()
	{
		this.anim = base.GetComponent<Animator>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.mach = base.GetComponent<Machine>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.rb = base.GetComponent<Rigidbody>();
		this.sc = base.GetComponentInChildren<SwingCheck2>();
		this.col = base.GetComponent<Collider>();
		this.nmp = new NavMeshPath();
	}

	// Token: 0x0600101C RID: 4124 RVA: 0x0007A8D4 File Offset: 0x00078AD4
	private void Start()
	{
		this.GetValues();
		this.SlowUpdate();
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0007A8E2 File Offset: 0x00078AE2
	private void OnEnable()
	{
		this.CancelActions(false);
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x0007A8EC File Offset: 0x00078AEC
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		if (this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.skitterSound.priority = Random.Range(100, 200);
		this.SetSpeed();
		if (this.behavior == MannequinBehavior.Random)
		{
			this.ChangeBehavior(false);
		}
	}

	// Token: 0x0600101F RID: 4127 RVA: 0x0007A96C File Offset: 0x00078B6C
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0007A974 File Offset: 0x00078B74
	private void SetSpeed()
	{
		this.GetValues();
		if (this.difficulty == 0)
		{
			this.anim.speed = 0.75f;
			this.walkSpeed = 10f;
			this.skitterSpeed = 32f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.85f;
			this.walkSpeed = 12f;
			this.skitterSpeed = 48f;
		}
		else if (this.difficulty >= 4)
		{
			this.anim.speed = 1.25f;
			this.walkSpeed = 20f;
			this.skitterSpeed = 64f;
		}
		else
		{
			this.anim.speed = 1f;
			this.walkSpeed = 16f;
			this.skitterSpeed = 64f;
		}
		this.walkSpeed *= this.eid.totalSpeedModifier;
		this.skitterSpeed *= this.eid.totalSpeedModifier;
		this.anim.speed *= this.eid.totalSpeedModifier;
		if (this.difficulty <= 2)
		{
			this.anim.SetFloat("DifficultyDependentSpeed", 0.66f);
			return;
		}
		this.anim.SetFloat("DifficultyDependentSpeed", 1f);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0007AAC4 File Offset: 0x00078CC4
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.1f);
		if (!this.inAction && this.eid.target != null)
		{
			if (this.mach.gc.onGround)
			{
				this.nma.enabled = true;
			}
			if (this.nma.enabled && this.mach.gc.onGround && this.nma.isOnNavMesh)
			{
				this.canCling = true;
				if (this.meleeCooldown <= 0f && Vector3.Distance(this.eid.target.position, base.transform.position) < 5f)
				{
					this.MeleeAttack();
					return;
				}
				if (this.behavior == MannequinBehavior.Melee || Physics.Raycast(this.eid.overrideCenter.position, this.eid.target.position - this.eid.overrideCenter.position, Vector3.Distance(this.eid.target.position, this.eid.overrideCenter.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore))
				{
					if (!this.stationary)
					{
						this.randomMovementTarget = base.transform.position;
						this.MoveToTarget(this.GetTargetPosition(), true, false);
						return;
					}
				}
				else
				{
					if (this.projectileCooldown <= 0f)
					{
						this.ProjectileAttack();
						return;
					}
					if (!this.stationary && Vector3.Distance(this.eid.target.position, base.transform.position) > 50f)
					{
						this.SetMovementTarget(this.eid.target.position - base.transform.position, Vector3.Distance(this.eid.target.position, base.transform.position) - 40f);
						return;
					}
					if (!this.stationary)
					{
						if (this.behavior == MannequinBehavior.RunAway && Vector3.Distance(this.eid.target.position, base.transform.position) < 15f)
						{
							this.SetMovementTarget(base.transform.position - this.eid.target.position, 20f - Vector3.Distance(this.eid.target.position, base.transform.position));
							return;
						}
						RaycastHit raycastHit;
						if (this.canCling && this.behavior == MannequinBehavior.Jump && this.jumpCooldown <= 0f && Physics.Raycast(base.transform.position + Vector3.up, Vector3.up, out raycastHit, 40f, LayerMaskDefaults.Get(LMD.Environment)) && !Physics.Raycast(raycastHit.point - Vector3.up * 3f, this.eid.target.position - (raycastHit.point - Vector3.up * 3f), Vector3.Distance(this.eid.target.position, raycastHit.point - Vector3.up * 3f), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore))
						{
							this.Jump();
							return;
						}
						if (Vector3.Distance(base.transform.position, this.randomMovementTarget) < 5f)
						{
							this.SetMovementTarget(Random.onUnitSphere, -1f);
							return;
						}
						this.nma.SetDestination(this.randomMovementTarget);
						return;
					}
				}
			}
			else if (this.clinging)
			{
				if (!this.stationary && Physics.Raycast(this.eid.overrideCenter.position, this.eid.target.position - this.eid.overrideCenter.position, Vector3.Distance(this.eid.target.position, this.eid.overrideCenter.position), LayerMaskDefaults.Get(LMD.EnvironmentAndBigEnemies), QueryTriggerInteraction.Ignore))
				{
					this.inControl = true;
					this.anim.SetBool("InControl", true);
					this.Uncling();
					return;
				}
				if (this.projectileCooldown <= 0f && this.clungMovementTarget == null)
				{
					this.ProjectileAttack();
					return;
				}
			}
		}
		else
		{
			this.nma.enabled = false;
		}
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0007AF4C File Offset: 0x0007914C
	private void Update()
	{
		if (this.mach.gc.onGround && this.nma.velocity.magnitude > 3f)
		{
			this.anim.SetBool("Walking", true);
		}
		else
		{
			this.anim.SetBool("Walking", false);
		}
		this.anim.SetBool("Skittering", this.skitterMode);
		this.anim.SetBool("InControl", this.inControl);
		this.nma.speed = (this.skitterMode ? this.skitterSpeed : this.walkSpeed);
		if (this.skitterMode && this.nma.velocity.magnitude > 3f && !this.skitterSound.isPlaying)
		{
			this.skitterSound.pitch = Random.Range(0.9f, 1.1f);
			this.skitterSound.Play();
			this.skitterSound.time = Random.Range(0f, this.skitterSound.clip.length);
		}
		else
		{
			this.skitterSound.Stop();
		}
		if ((this.inAction || this.clinging) && this.trackTarget && this.eid.target != null)
		{
			float num = Vector3.Dot(base.transform.up, this.eid.target.position - base.transform.position);
			Quaternion quaternion = Quaternion.LookRotation(this.eid.target.position - base.transform.up * num - base.transform.position, base.transform.up);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Mathf.Max(Quaternion.Angle(base.transform.rotation, quaternion), 10f) * 10f * Time.deltaTime);
		}
		if (this.meleeCooldown > 0f)
		{
			this.meleeCooldown = Mathf.MoveTowards(this.meleeCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.projectileCooldown > 0f)
		{
			this.projectileCooldown = Mathf.MoveTowards(this.projectileCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.jumpCooldown > 0f)
		{
			this.jumpCooldown = Mathf.MoveTowards(this.jumpCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.behavior == MannequinBehavior.Melee && !this.inAction && this.meleeBehaviorCancel > 0f)
		{
			this.meleeBehaviorCancel = Mathf.MoveTowards(this.meleeBehaviorCancel, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.meleeBehaviorCancel <= 0f)
			{
				this.ChangeBehavior(true);
			}
		}
		if (((this.nma.enabled && !this.inAction && this.behavior == MannequinBehavior.RunAway) || this.behavior == MannequinBehavior.Wander) && this.nma.velocity.magnitude > 2f)
		{
			Vector3 vector = this.eid.overrideCenter.position + Vector3.up * 0.5f;
			Vector3 normalized = this.nma.velocity.normalized;
			normalized.y = 0f;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(vector, normalized), out raycastHit, 6f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				Ray ray = new Ray(vector, Quaternion.Euler(0f, -90f, 0f) * normalized);
				Ray ray2 = new Ray(vector, Quaternion.Euler(0f, 90f, 0f) * normalized);
				float num2 = 2f;
				RaycastHit raycastHit2;
				if (Physics.Raycast(ray, out raycastHit2, num2, LayerMaskDefaults.Get(LMD.Environment)) || Physics.Raycast(ray2, out raycastHit2, num2, LayerMaskDefaults.Get(LMD.Environment)))
				{
					if (Mannequin.debug)
					{
						Debug.Log("Space too tight, ignoring cling attempt", base.gameObject);
					}
					return;
				}
				this.clungMovementTarget = null;
				this.ClingToSurface(raycastHit);
				this.RelocateWhileClinging(ClungMannequinMovementDirection.Vertical);
			}
		}
		if (this.clungMovementTarget != null && this.clinging && !this.inAction)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.clungMovementTarget.Value, 30f * Time.deltaTime * this.eid.totalSpeedModifier);
			if (Vector3.Distance(base.transform.position, this.clungMovementTarget.Value) < 0.1f)
			{
				if (Mannequin.debug)
				{
					Debug.Log("Reached clung movement target", base.gameObject);
				}
				this.clungMovementTarget = null;
				this.skitterMode = false;
				RaycastHit raycastHit3;
				if (Physics.Raycast(new Ray(base.transform.position, Vector3.down), out raycastHit3, 3f, LayerMaskDefaults.Get(LMD.Environment)))
				{
					if (Mannequin.debug)
					{
						Debug.Log("We've hit the floor while cling walking. Let's jump off", base.gameObject);
					}
					this.Uncling();
				}
			}
		}
		if (this.clinging && (this.clungSurfaceCollider == null || !this.clungSurfaceCollider.enabled || !this.clungSurfaceCollider.gameObject.activeInHierarchy))
		{
			this.Uncling();
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0007B4F0 File Offset: 0x000796F0
	private void FixedUpdate()
	{
		if (this.inAction && this.moveForward && !Physics.Raycast(base.transform.position + Vector3.up * 3f, base.transform.forward, 55f * Time.fixedDeltaTime * this.eid.totalSpeedModifier, LayerMaskDefaults.Get(LMD.Player), QueryTriggerInteraction.Ignore))
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward, Vector3.down, out raycastHit, (this.eid.target == null) ? 22f : Mathf.Max(22f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.rb.velocity = base.transform.forward * 55f * this.anim.speed * this.eid.totalSpeedModifier;
			}
			else
			{
				this.rb.velocity = Vector3.zero;
			}
		}
		if (this.canCling && !this.mach.gc.onGround)
		{
			this.CheckClings();
		}
		this.anim.SetFloat("ySpeed", this.rb.isKinematic ? 0f : this.rb.velocity.y);
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0007B69C File Offset: 0x0007989C
	private void LateUpdate()
	{
		if (this.aiming)
		{
			if (this.trackTarget && this.eid.target != null)
			{
				this.aimPoint = this.aimBone.position - this.eid.target.position;
			}
			this.aimBone.LookAt(this.aimBone.position + this.aimPoint, base.transform.up);
		}
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0007B718 File Offset: 0x00079918
	private float EvaluateMaxClingWalkDistance(Vector3 origin, Vector3 movementDirection, Vector3 backToWallDirection, float maxDistance = 20f, float incrementLength = 1.5f)
	{
		float num = 0f;
		Vector3 vector = origin;
		Vector3 vector2 = this.clingNormal * this.clungMovementTolerance;
		while (num < maxDistance)
		{
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(new Ray(vector + vector2, backToWallDirection), out raycastHit, this.clungMovementTolerance * 2f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore);
			if (Vector3.Angle(raycastHit.normal, this.clingNormal) >= 5f)
			{
				flag = false;
			}
			if (!flag)
			{
				return num - incrementLength * 1.5f;
			}
			if (Physics.Raycast(new Ray(vector + vector2 - movementDirection.normalized * 0.1f, movementDirection), out raycastHit, incrementLength * 1.25f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				return num - incrementLength * 1.5f;
			}
			num += incrementLength;
			vector += movementDirection * incrementLength;
		}
		if (num == 0f)
		{
			return 0f;
		}
		return num - incrementLength * 1.5f;
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0007B820 File Offset: 0x00079A20
	private void RelocateWhileClinging(ClungMannequinMovementDirection direction)
	{
		Vector3 position = base.transform.position;
		Vector3 vector;
		if (Mathf.Abs(Vector3.Dot(this.clingNormal, Vector3.up)) < 0.99f)
		{
			vector = Vector3.Cross(this.clingNormal, Vector3.up).normalized;
		}
		else
		{
			vector = Vector3.Cross(this.clingNormal, Vector3.right).normalized;
		}
		Vector3 normalized = Vector3.Cross(this.clingNormal, vector).normalized;
		Vector3 vector2;
		if (direction == ClungMannequinMovementDirection.Horizontal)
		{
			vector2 = vector;
		}
		else
		{
			vector2 = normalized;
		}
		float num = this.EvaluateMaxClingWalkDistance(position, vector2, -this.clingNormal, 20f, 1.5f);
		float num2 = Random.Range(-this.EvaluateMaxClingWalkDistance(position, -vector2, -this.clingNormal, 20f, 1.5f), num);
		if (Mathf.Abs(num2) <= 2f)
		{
			return;
		}
		Vector3 vector3 = position + vector2 * num2;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(vector3 + this.clingNormal * this.clungMovementTolerance, -this.clingNormal), out raycastHit, this.clungMovementTolerance * 2f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			if (Mannequin.debug)
			{
				Debug.Log(string.Format("Accounting for bump at target. Distance: {0}", Vector3.Distance(vector3, raycastHit.point)), base.gameObject);
			}
			vector3 = raycastHit.point;
		}
		this.MoveToTarget(vector3, true, true);
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0007B9A0 File Offset: 0x00079BA0
	private void CheckClings()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, Vector3.up, out raycastHit, this.firstClingCheck ? 9.5f : 7f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore) && raycastHit.normal.y <= 0f)
		{
			this.ClingToSurface(raycastHit);
		}
		else if (this.firstClingCheck || new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z).magnitude > 3f)
		{
			Collider[] array = Physics.OverlapSphere(this.col.bounds.center, 2f, LayerMaskDefaults.Get(LMD.Environment));
			if (array == null || array.Length == 0)
			{
				return;
			}
			if (Physics.Raycast(this.col.bounds.center, array[0].ClosestPoint(this.col.bounds.center) - this.col.bounds.center, out raycastHit, this.firstClingCheck ? 3.5f : 2f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.ClingToSurface(raycastHit);
			}
		}
		this.firstClingCheck = false;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0007BAF8 File Offset: 0x00079CF8
	private void ClingToSurface(RaycastHit hit)
	{
		this.CancelActions(true);
		Vector3 point = hit.point;
		Vector3 normal = hit.normal;
		this.canCling = false;
		this.clinging = true;
		this.clungSurfaceCollider = hit.collider;
		this.skitterMode = false;
		this.mach.gc.ForceOff();
		base.transform.position = point;
		base.transform.up = normal;
		this.trackTarget = true;
		this.clingNormal = normal.normalized;
		this.nma.enabled = false;
		this.mach.overrideFalling = true;
		this.rb.isKinematic = true;
		this.rb.useGravity = false;
		this.anim.SetBool("Clinging", true);
		this.anim.Play("WallCling");
		if (!this.firstClingCheck)
		{
			Object.Instantiate<AudioSource>(this.clingSound, base.transform.position, Quaternion.identity);
		}
		this.projectileCooldown = Random.Range(0f, 0.5f);
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0007BC04 File Offset: 0x00079E04
	public void Uncling()
	{
		this.clinging = false;
		this.clungSurfaceCollider = null;
		this.CancelActions(true);
		Vector3 vector = new Vector3(this.clingNormal.x * 2f, this.clingNormal.y * 6f, this.clingNormal.z * 2f);
		RaycastHit raycastHit;
		if (Mathf.Abs(vector.y) < 6f && Physics.Raycast(new Ray(this.col.bounds.center, Vector3.up), out raycastHit, 4f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			vector.y = -6f;
		}
		if (this.eid && this.eid.target != null)
		{
			base.transform.LookAt(new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z));
		}
		else
		{
			base.transform.LookAt(base.transform.position + this.clingNormal);
		}
		base.transform.position += vector;
		this.trackTarget = false;
		base.Invoke("DelayedGroundCheckReenable", 0.1f);
		this.jumpCooldown = 2f;
		this.skitterMode = false;
		this.attacksWhileClinging = 0;
		this.mach.overrideFalling = false;
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		if (this.inControl)
		{
			this.rb.AddForce(Vector3.down * 50f, ForceMode.VelocityChange);
		}
		this.anim.SetBool("Clinging", false);
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0007BDDC File Offset: 0x00079FDC
	private void MeleeAttack()
	{
		if (this.inAction)
		{
			return;
		}
		this.inAction = true;
		this.mostRecentAction = "Melee Attack";
		this.meleeCooldown = 2f / this.eid.totalSpeedModifier;
		this.nma.enabled = false;
		this.anim.Play("MeleeAttack");
		this.trackTarget = true;
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0007BE40 File Offset: 0x0007A040
	private void ProjectileAttack()
	{
		if (this.inAction)
		{
			return;
		}
		this.inAction = true;
		this.mostRecentAction = "Projectile Attack";
		this.projectileCooldown = Random.Range(6f - (float)this.difficulty, 8f - (float)this.difficulty) / this.eid.totalSpeedModifier;
		this.nma.enabled = false;
		this.anim.Play(this.clinging ? "WallClingProjectile" : "ProjectileAttack");
		this.trackTarget = true;
		this.aiming = true;
		this.chargingProjectile = true;
		if (this.clinging)
		{
			this.attacksWhileClinging++;
		}
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0007BEF0 File Offset: 0x0007A0F0
	private void Jump()
	{
		if (this.inAction)
		{
			return;
		}
		this.inAction = true;
		this.jumping = true;
		this.mach.overrideFalling = true;
		this.skitterMode = false;
		this.mostRecentAction = "Jump";
		this.nma.enabled = false;
		this.jumpCooldown = 2f;
		this.anim.SetBool("Jump", true);
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0007BF5C File Offset: 0x0007A15C
	private void JumpNow()
	{
		this.mach.gc.ForceOff();
		base.Invoke("DelayedGroundCheckReenable", 0.1f);
		this.rb.isKinematic = false;
		this.rb.useGravity = true;
		this.rb.AddForce(Vector3.up * 100f, ForceMode.VelocityChange);
		this.inControl = true;
		this.skitterMode = false;
		this.anim.SetBool("Jump", false);
		this.anim.SetBool("InControl", this.inControl);
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0007BFF4 File Offset: 0x0007A1F4
	private void MoveToTarget(Vector3 target, bool forceSkitter = false, bool clungMode = false)
	{
		if (clungMode)
		{
			if (Mannequin.debug)
			{
				Debug.Log("Starting clung movement");
			}
			this.clungMovementTarget = new Vector3?(target);
			this.skitterMode = true;
			return;
		}
		if (this.inAction)
		{
			return;
		}
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(target, out navMeshHit, 15f, this.nma.areaMask))
		{
			target = navMeshHit.position;
		}
		this.nma.CalculatePath(target, this.nmp);
		this.skitterMode = forceSkitter || ((this.difficulty >= 3 || Random.Range(0f, 1f) > 0.5f) && Vector3.Distance(base.transform.position, target) > 15f);
		this.nma.path = this.nmp;
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0007C0C0 File Offset: 0x0007A2C0
	public void OnDeath()
	{
		if (this.currentChargeProjectile)
		{
			Object.Destroy(this.currentChargeProjectile);
		}
		KeepInBounds keepInBounds;
		if (base.TryGetComponent<KeepInBounds>(out keepInBounds))
		{
			Object.Destroy(keepInBounds);
		}
		this.skitterSound.Stop();
		this.sc.DamageStop();
		TrailRenderer[] array = this.trails;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].emitting = false;
		}
		this.mach.parryable = false;
		Object.Destroy(this);
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0007C13C File Offset: 0x0007A33C
	private void StopTracking(int parryable = 0)
	{
		if (this.eid.target != null)
		{
			base.transform.LookAt(base.transform.position + (new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z) - base.transform.position));
		}
		this.trackTarget = false;
		if (parryable > 0)
		{
			this.mach.parryable = true;
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.parryableFlash, this.eid.weakPoint.transform.position + this.eid.weakPoint.transform.forward * -0.35f, Quaternion.identity);
			gameObject.transform.LookAt(MonoSingleton<CameraController>.Instance.GetDefaultPos());
			gameObject.transform.localScale *= 3f;
			gameObject.transform.SetParent(this.eid.weakPoint.transform, true);
		}
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0007C272 File Offset: 0x0007A472
	private void SwingStart(int limb = 0)
	{
		this.moveForward = true;
		this.rb.isKinematic = false;
		this.sc.DamageStart();
		if (limb < this.trails.Length)
		{
			this.trails[limb].emitting = true;
		}
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0007C2AC File Offset: 0x0007A4AC
	private void SwingEnd(int parryEnd = 0)
	{
		this.moveForward = false;
		if (this.eid.gce.onGround)
		{
			this.rb.isKinematic = true;
		}
		else
		{
			this.rb.velocity = new Vector3(0f, this.rb.velocity.y, 0f);
		}
		this.sc.DamageStop();
		TrailRenderer[] array = this.trails;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].emitting = false;
		}
		if (parryEnd > 0)
		{
			this.mach.parryable = false;
		}
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0007C344 File Offset: 0x0007A544
	private void ChargeProjectile()
	{
		if (this.currentChargeProjectile)
		{
			Object.Destroy(this.currentChargeProjectile);
		}
		if (!this.chargingProjectile)
		{
			return;
		}
		this.currentChargeProjectile = Object.Instantiate<GameObject>(this.chargeProjectile, this.shootPoint.position, this.shootPoint.rotation);
		this.currentChargeProjectile.transform.SetParent(this.shootPoint, true);
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0007C3B0 File Offset: 0x0007A5B0
	private void ShootProjectile()
	{
		if (this.currentChargeProjectile)
		{
			Object.Destroy(this.currentChargeProjectile);
		}
		if (this.projectile == null || this.projectile.Equals(null))
		{
			this.trackTarget = false;
			this.chargingProjectile = false;
			return;
		}
		Projectile projectile = Object.Instantiate<Projectile>(this.projectile, this.shootPoint.position, (this.eid.target != null) ? Quaternion.LookRotation(this.eid.target.position - this.shootPoint.position) : this.shootPoint.rotation);
		projectile.target = this.eid.target;
		projectile.safeEnemyType = EnemyType.Mannequin;
		if (this.difficulty <= 2)
		{
			projectile.turningSpeedMultiplier = 0.75f;
		}
		this.trackTarget = false;
		this.chargingProjectile = false;
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0007C494 File Offset: 0x0007A694
	public void ChangeBehavior(bool noMelee = false)
	{
		if (!this.dontChangeBehavior)
		{
			if (!noMelee && !this.stationary && !this.dontMeleeBehavior && Random.Range(0f, 1f) < 0.35f)
			{
				this.meleeBehaviorCancel = 3.5f;
				this.behavior = MannequinBehavior.Melee;
			}
			else
			{
				this.behavior = (MannequinBehavior)Random.Range(2, 5);
			}
		}
		this.randomMovementTarget = base.transform.position;
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0007C504 File Offset: 0x0007A704
	public void ResetMovementTarget()
	{
		this.randomMovementTarget = base.transform.position;
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0007C517 File Offset: 0x0007A717
	private void StopAiming()
	{
		this.aiming = false;
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0007C520 File Offset: 0x0007A720
	public void Landing()
	{
		this.mach.parryable = false;
		if (this.difficulty >= 4)
		{
			this.inControl = true;
		}
		if (this.inControl)
		{
			return;
		}
		this.anim.Play("Landing");
		this.inAction = true;
		this.mostRecentAction = "Landing";
		this.inControl = true;
		this.nma.enabled = false;
		this.randomMovementTarget = base.transform.position;
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0007C598 File Offset: 0x0007A798
	public void StopAction()
	{
		this.StopAction(true);
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0007C5A4 File Offset: 0x0007A7A4
	public void StopAction(bool changeBehavior = true)
	{
		if (this.clinging && !this.stationary && !this.dontAutoDrop && this.attacksWhileClinging >= ((Random.Range(0f, 1f) > 0.5f) ? 2 : 4))
		{
			this.attacksWhileClinging = 0;
			this.inControl = true;
			this.anim.SetBool("InControl", true);
			this.Uncling();
		}
		if (this.clinging)
		{
			if (this.inAction && !this.jumping)
			{
				this.RelocateWhileClinging((Random.Range(0f, 1f) > 0.5f) ? ClungMannequinMovementDirection.Horizontal : ClungMannequinMovementDirection.Vertical);
			}
		}
		else
		{
			this.clungMovementTarget = null;
			this.jumping = false;
			this.mach.overrideFalling = false;
		}
		this.trackTarget = this.clinging;
		this.aiming = false;
		this.inAction = false;
		this.mach.parryable = false;
		this.moveForward = false;
		this.chargingProjectile = false;
		if (changeBehavior)
		{
			this.ChangeBehavior(false);
		}
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0007C6AB File Offset: 0x0007A8AB
	public void CancelActions(bool changeBehavior = true)
	{
		if (this.moveForward)
		{
			this.SwingEnd(0);
		}
		this.StopAction(changeBehavior);
		if (this.currentChargeProjectile)
		{
			Object.Destroy(this.currentChargeProjectile);
		}
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0007C6DC File Offset: 0x0007A8DC
	public void SetMovementTarget(Vector3 direction, float distance = -1f)
	{
		direction.y = 0f;
		if (distance == -1f)
		{
			distance = Random.Range(5f, 25f);
		}
		RaycastHit raycastHit;
		RaycastHit raycastHit2;
		if (Physics.Raycast(this.eid.overrideCenter.position, direction, out raycastHit, distance, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			NavMeshHit navMeshHit;
			if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 5f, this.nma.areaMask))
			{
				this.randomMovementTarget = navMeshHit.position;
			}
			else if (Physics.SphereCast(raycastHit.point, 1f, Vector3.down, out raycastHit, distance, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
			{
				this.randomMovementTarget = raycastHit.point;
			}
		}
		else if (Physics.Raycast(this.eid.overrideCenter.position + direction.normalized * distance, Vector3.down, out raycastHit2, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			this.randomMovementTarget = raycastHit2.point;
		}
		if (this.nma && this.nma.enabled && this.nma.isOnNavMesh && this.mach.gc.onGround)
		{
			this.MoveToTarget(this.randomMovementTarget, false, false);
		}
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x0007C832 File Offset: 0x0007AA32
	private void DelayedGroundCheckReenable()
	{
		this.mach.gc.StopForceOff();
		if (this.jumping)
		{
			this.jumping = false;
			this.mach.overrideFalling = false;
			this.inAction = false;
		}
	}

	// Token: 0x0600103E RID: 4158 RVA: 0x0007C868 File Offset: 0x0007AA68
	private float GetRealDistance(NavMeshPath path)
	{
		if (path.status == NavMeshPathStatus.PathInvalid || path.corners.Length <= 1)
		{
			return Vector3.Distance(base.transform.position, this.GetTargetPosition());
		}
		float num = 0f;
		if (path.corners.Length > 1)
		{
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
		}
		return num;
	}

	// Token: 0x0600103F RID: 4159 RVA: 0x0007C8E8 File Offset: 0x0007AAE8
	private Vector3 GetTargetPosition()
	{
		RaycastHit raycastHit;
		if (((this.eid.target.isPlayer && !MonoSingleton<NewMovement>.Instance.gc.onGround) || (this.eid.target.isEnemy && this.eid.target.enemyIdentifier && (!this.eid.target.enemyIdentifier.gce || !this.eid.target.enemyIdentifier.gce.onGround))) && Physics.Raycast(this.eid.target.position, Vector3.down, out raycastHit, 200f, LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
		{
			return raycastHit.point;
		}
		return this.eid.target.position;
	}

	// Token: 0x040015F1 RID: 5617
	private bool gotValues;

	// Token: 0x040015F2 RID: 5618
	private Animator anim;

	// Token: 0x040015F3 RID: 5619
	private NavMeshAgent nma;

	// Token: 0x040015F4 RID: 5620
	private NavMeshPath nmp;

	// Token: 0x040015F5 RID: 5621
	private Machine mach;

	// Token: 0x040015F6 RID: 5622
	private EnemyIdentifier eid;

	// Token: 0x040015F7 RID: 5623
	private Rigidbody rb;

	// Token: 0x040015F8 RID: 5624
	private SwingCheck2 sc;

	// Token: 0x040015F9 RID: 5625
	public GameObject bloodSpray;

	// Token: 0x040015FA RID: 5626
	private bool skitterMode;

	// Token: 0x040015FB RID: 5627
	private float walkSpeed = 22f;

	// Token: 0x040015FC RID: 5628
	private float skitterSpeed = 64f;

	// Token: 0x040015FD RID: 5629
	private int difficulty;

	// Token: 0x040015FE RID: 5630
	public bool inAction;

	// Token: 0x040015FF RID: 5631
	public MannequinBehavior behavior;

	// Token: 0x04001600 RID: 5632
	public bool dontChangeBehavior;

	// Token: 0x04001601 RID: 5633
	public bool dontAutoDrop;

	// Token: 0x04001602 RID: 5634
	public bool dontMeleeBehavior;

	// Token: 0x04001603 RID: 5635
	public bool stationary;

	// Token: 0x04001604 RID: 5636
	private Vector3 randomMovementTarget;

	// Token: 0x04001605 RID: 5637
	private bool trackTarget;

	// Token: 0x04001606 RID: 5638
	private bool moveForward;

	// Token: 0x04001607 RID: 5639
	[SerializeField]
	private TrailRenderer[] trails;

	// Token: 0x04001608 RID: 5640
	[SerializeField]
	private Transform shootPoint;

	// Token: 0x04001609 RID: 5641
	private bool aiming;

	// Token: 0x0400160A RID: 5642
	[SerializeField]
	private Transform aimBone;

	// Token: 0x0400160B RID: 5643
	private Vector3 aimPoint;

	// Token: 0x0400160C RID: 5644
	public Projectile projectile;

	// Token: 0x0400160D RID: 5645
	public GameObject chargeProjectile;

	// Token: 0x0400160E RID: 5646
	[HideInInspector]
	public GameObject currentChargeProjectile;

	// Token: 0x0400160F RID: 5647
	private bool chargingProjectile;

	// Token: 0x04001610 RID: 5648
	private float meleeCooldown = 0.5f;

	// Token: 0x04001611 RID: 5649
	private float projectileCooldown = 1f;

	// Token: 0x04001612 RID: 5650
	private float jumpCooldown = 2f;

	// Token: 0x04001613 RID: 5651
	private float meleeBehaviorCancel = 3.5f;

	// Token: 0x04001614 RID: 5652
	public bool inControl;

	// Token: 0x04001615 RID: 5653
	private bool canCling = true;

	// Token: 0x04001616 RID: 5654
	[HideInInspector]
	public bool clinging;

	// Token: 0x04001617 RID: 5655
	private Collider clungSurfaceCollider;

	// Token: 0x04001618 RID: 5656
	private int attacksWhileClinging;

	// Token: 0x04001619 RID: 5657
	private Vector3 clingNormal;

	// Token: 0x0400161A RID: 5658
	private Vector3? clungMovementTarget;

	// Token: 0x0400161B RID: 5659
	[SerializeField]
	private float clungMovementTolerance = 1.25f;

	// Token: 0x0400161C RID: 5660
	private bool firstClingCheck = true;

	// Token: 0x0400161D RID: 5661
	public AudioSource clingSound;

	// Token: 0x0400161E RID: 5662
	private Collider col;

	// Token: 0x0400161F RID: 5663
	[SerializeField]
	private AudioSource skitterSound;

	// Token: 0x04001620 RID: 5664
	public string mostRecentAction;

	// Token: 0x04001621 RID: 5665
	[HideInInspector]
	public bool jumping;
}
