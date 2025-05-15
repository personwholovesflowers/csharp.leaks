using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020001C7 RID: 455
public class Ferryman : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x0600091C RID: 2332 RVA: 0x0003C4CC File Offset: 0x0003A6CC
	private void Start()
	{
		this.mach = base.GetComponent<Machine>();
		this.rb = base.GetComponent<Rigidbody>();
		this.gce = base.GetComponentInChildren<GroundCheckEnemy>();
		this.path = new NavMeshPath();
		this.swingChecks = base.GetComponentsInChildren<SwingCheck2>();
		if (this.oarSimplifier)
		{
			this.originalOar = this.oarSimplifier.originalMaterial;
		}
		this.SetSpeed();
		this.SlowUpdate();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0003C53E File Offset: 0x0003A73E
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0003C548 File Offset: 0x0003A748
	private void SetSpeed()
	{
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
		if (!this.eid)
		{
			this.eid = base.GetComponent<EnemyIdentifier>();
		}
		if (!this.nma)
		{
			this.nma = base.GetComponent<NavMeshAgent>();
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
		if (this.difficulty == 2)
		{
			this.anim.speed = 0.9f;
		}
		else if (this.difficulty == 1)
		{
			this.anim.speed = 0.8f;
		}
		else if (this.difficulty == 0)
		{
			this.anim.speed = 0.6f;
		}
		else
		{
			this.anim.speed = 1f;
		}
		if (this.defaultMovementSpeed == 0f)
		{
			this.defaultMovementSpeed = this.nma.speed;
		}
		this.anim.speed *= this.eid.totalSpeedModifier;
		this.nma.speed = this.defaultMovementSpeed * this.eid.totalSpeedModifier;
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0003C698 File Offset: 0x0003A898
	private void OnDisable()
	{
		if (base.gameObject.scene.isLoaded && this.currentWindup)
		{
			this.currentWindup.SetActive(false);
		}
		this.inAction = false;
		this.tracking = false;
		this.moving = false;
		this.uppercutting = false;
		MonoSingleton<EnemyCooldowns>.Instance.RemoveFerryman(this);
		this.StopDamage();
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0003C700 File Offset: 0x0003A900
	private void OnEnable()
	{
		MonoSingleton<EnemyCooldowns>.Instance.AddFerryman(this);
		if (this.currentWindup)
		{
			this.currentWindup.SetActive(true);
		}
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0003C728 File Offset: 0x0003A928
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.1f);
		if (!this.nma || (!this.lightningCancellable && !this.nma.isOnNavMesh) || this.eid.target == null)
		{
			return;
		}
		bool flag = false;
		if (this.inPhaseChange)
		{
			if (!this.inAction && this.nma.isOnNavMesh)
			{
				this.nma.SetDestination(this.phaseChangePositions[this.currentPosition].position);
			}
			return;
		}
		if (!this.inAction || this.lightningCancellable)
		{
			RaycastHit raycastHit;
			if ((!this.eid.target.isOnGround || !NavMesh.CalculatePath(base.transform.position, this.PredictPlayerPos(true, 5f), this.nma.areaMask, this.path)) && Physics.Raycast(this.eid.target.position, Vector3.down, out raycastHit, float.PositiveInfinity, LayerMaskDefaults.Get(LMD.Environment)))
			{
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(raycastHit.point, out navMeshHit, 1f, this.nma.areaMask))
				{
					NavMesh.CalculatePath(base.transform.position, navMeshHit.position, this.nma.areaMask, this.path);
				}
				else
				{
					NavMesh.CalculatePath(base.transform.position, raycastHit.point, this.nma.areaMask, this.path);
					flag = true;
				}
			}
			if (!this.inAction && this.nma.isOnNavMesh)
			{
				this.nma.path = this.path;
			}
		}
		if (this.eid.target.position.y > base.transform.position.y + 20f || (this.path.status != NavMeshPathStatus.PathComplete && this.path.corners != null && this.path.corners.Length != 0 && (!this.eid.target.isOnGround || Vector3.Distance(this.path.corners[this.path.corners.Length - 1], this.PredictPlayerPos(true, 5f)) > 5f)))
		{
			flag = true;
		}
		else if (this.inAction && this.lightningCancellable)
		{
			this.CancelLightningBolt();
		}
		if (flag && this.difficulty >= 2)
		{
			this.lightningOutOfReachCharge += 0.1f * this.eid.totalSpeedModifier;
			if (!this.inAction && this.lightningOutOfReachCharge > 3f && MonoSingleton<EnemyCooldowns>.Instance.ferrymanCooldown <= 0f && this.eid.zapperer == null)
			{
				this.lightningOutOfReachCharge = 0f;
				this.LightningBolt(true);
				return;
			}
		}
		else
		{
			this.lightningOutOfReachCharge = 0f;
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0003CA10 File Offset: 0x0003AC10
	private void Update()
	{
		if (this.eid.target == null)
		{
			return;
		}
		this.PlayerStatus();
		this.anim.SetBool("Falling", !this.gce.onGround);
		if (this.mach.health < this.phaseChangeHealth && this.bossVersion && !this.hasPhaseChanged)
		{
			this.PhaseChange();
		}
		if (this.lightningBoltCooldown > 0f)
		{
			this.lightningBoltCooldown = Mathf.MoveTowards(this.lightningBoltCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier * (this.inPhaseChange ? 1f : 0.4f));
		}
		if (this.inPhaseChange && !this.inAction && this.eid.target != null)
		{
			if (Vector3.Distance(base.transform.position, this.phaseChangePositions[this.currentPosition].position) < 3.5f)
			{
				if (this.currentPosition < this.phaseChangePositions.Length - 1)
				{
					this.currentPosition++;
					this.nma.destination = this.phaseChangePositions[this.currentPosition].position;
					return;
				}
				if (!this.hasReachedFinalPosition)
				{
					base.transform.position = this.phaseChangePositions[this.phaseChangePositions.Length - 1].position;
					this.rb.isKinematic = true;
					this.rb.useGravity = false;
					this.hasReachedFinalPosition = true;
				}
				this.anim.SetBool("Running", false);
				if (!this.inAction && this.lightningBoltCooldown <= 0f && this.eid.zapperer == null)
				{
					this.LightningBolt(false);
				}
				if (!this.inAction)
				{
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(this.playerPos - base.transform.position), Time.deltaTime * 600f * this.eid.totalSpeedModifier);
					return;
				}
			}
			else
			{
				if (!this.nma || !this.nma.enabled || !this.nma.isOnNavMesh || !this.gce.onGround)
				{
					this.anim.SetBool("Falling", true);
					this.anim.SetBool("Running", true);
					this.rb.isKinematic = false;
					this.rb.useGravity = true;
					Vector3 vector = new Vector3(this.phaseChangePositions[this.currentPosition].position.x, base.transform.position.y, this.phaseChangePositions[this.currentPosition].position.z);
					base.transform.position = Vector3.MoveTowards(base.transform.position, vector, Time.deltaTime * Mathf.Max(10f, Vector3.Distance(base.transform.position, vector) * this.eid.totalSpeedModifier));
					return;
				}
				if (this.nma.pathStatus != NavMeshPathStatus.PathComplete && !this.jumping)
				{
					this.anim.SetBool("Falling", true);
					this.anim.SetBool("Running", true);
					this.rb.isKinematic = false;
					this.rb.useGravity = true;
					this.nma.enabled = false;
					ParticleSystem[] array = this.footstepParticles;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].Play();
					}
					this.Footstep(0.5f);
					base.transform.position += Vector3.up * 5f;
					this.rb.AddForce(Vector3.up * Mathf.Abs(base.transform.position.y - this.phaseChangePositions[this.currentPosition].position.y) * 2f, ForceMode.VelocityChange);
					this.jumping = true;
					this.swingAudioSource.pitch = Random.Range(2.9f, 3f);
					this.swingAudioSource.volume = 0.5f;
					this.swingAudioSource.Play();
					base.transform.rotation = Quaternion.LookRotation(new Vector3(this.phaseChangePositions[this.currentPosition].position.x, base.transform.position.y, this.phaseChangePositions[this.currentPosition].position.z) - base.transform.position);
					return;
				}
				this.nma.enabled = true;
				this.anim.SetBool("Running", true);
			}
			return;
		}
		if (this.nma && this.nma.isOnNavMesh && this.nma.velocity.magnitude > 2f && this.gce.onGround)
		{
			this.anim.SetBool("Running", true);
		}
		else
		{
			this.anim.SetBool("Running", false);
		}
		if (!this.inAction && this.eid.target != null)
		{
			if (this.gce.onGround)
			{
				if (this.difficulty >= 4 && this.lightningBoltCooldown <= 0f && MonoSingleton<EnemyCooldowns>.Instance.ferrymanCooldown <= 0f && this.eid.zapperer == null)
				{
					if (Random.Range(0f, 1f) > 0.5f)
					{
						this.LightningBolt(true);
					}
					else
					{
						this.lightningBoltCooldown = 0.4f;
					}
				}
				else if (Vector3.Distance(this.playerPos, base.transform.position) < 8f && (this.eid.target.position.y > base.transform.position.y + 5f || (this.eid.target.GetVelocity().y > 5f && !this.eid.target.isOnGround)))
				{
					if (this.playerRetreating && this.rollCooldown <= 0f)
					{
						this.Roll(false);
					}
					else if (Vector3.Distance(this.playerPos, base.transform.position) < 5f && this.eid.target.position.y < base.transform.position.y + 20f)
					{
						this.Uppercut();
					}
				}
				else if (Vector3.Distance(this.eid.target.position, base.transform.position) > 8f || (this.playerRetreating && MonoSingleton<NewMovement>.Instance.sliding))
				{
					if (this.vaultCooldown <= 0f && Vector3.Distance(this.eid.target.position, base.transform.position) < 35f && Vector3.Distance(this.eid.target.position, base.transform.position) > 30f && !this.playerApproaching && this.eid.target.position.y <= base.transform.position.y + 20f)
					{
						this.vaultCooldown = 2f;
						if (this.difficulty >= 3)
						{
							this.VaultSwing();
						}
						else
						{
							this.Vault();
						}
					}
					else if (Vector3.Distance(this.eid.target.position, base.transform.position) < 14f && this.playerRetreating && !this.playerAbove)
					{
						if (Random.Range(0f, 1f) < this.stingerChance || this.rollCooldown > 0f)
						{
							this.stingerChance = Mathf.Min(0.25f, this.stingerChance - 0.25f);
							this.Stinger();
						}
						else
						{
							this.stingerChance = Mathf.Max(0.75f, this.stingerChance + 0.25f);
							this.Roll(false);
						}
					}
				}
				else if (this.playerApproaching)
				{
					if (Random.Range(0f, 1f) < 0.25f)
					{
						if (Random.Range(0f, 1f) < 0.75f && this.rollCooldown <= 0f)
						{
							this.Roll(true);
						}
						else if (Random.Range(0f, 1f) < 0.5f)
						{
							this.KickCombo();
						}
						else
						{
							this.OarCombo();
						}
					}
					else if (Random.Range(0f, 1f) < this.overheadChance)
					{
						this.overheadChance = Mathf.Min(0.25f, this.overheadChance - 0.25f);
						this.Downslam();
					}
					else
					{
						this.overheadChance = Mathf.Max(0.75f, this.overheadChance + 0.25f);
						this.BackstepAttack();
					}
				}
				else if (Random.Range(0f, 1f) < this.kickComboChance)
				{
					this.kickComboChance = Mathf.Min(0.25f, this.kickComboChance - 0.25f);
					this.KickCombo();
				}
				else
				{
					this.kickComboChance = Mathf.Max(0.75f, this.kickComboChance + 0.25f);
					this.OarCombo();
				}
			}
		}
		else
		{
			this.nma.enabled = false;
			if (this.tracking && this.eid.target != null)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(this.PredictPlayerPos(false, 5f) - base.transform.position), Time.deltaTime * 600f * this.eid.totalSpeedModifier);
			}
			if (this.moving)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(base.transform.position + Vector3.up + base.transform.forward, Vector3.down, out raycastHit, Mathf.Max(22f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					this.rb.velocity = base.transform.forward * this.movingSpeed * this.anim.speed;
				}
				else
				{
					this.rb.velocity = Vector3.zero;
				}
			}
			if (this.uppercutting)
			{
				Vector3 vector2 = Vector3.up * 100f * this.anim.speed;
				if (Vector3.Distance(base.transform.position, this.playerPos) > 5f)
				{
					vector2 += base.transform.forward * Mathf.Min(100f, Vector3.Distance(base.transform.position, this.playerPos) * 40f) * this.anim.speed;
				}
				RaycastHit raycastHit2;
				if (Physics.Raycast(this.lastGroundedPosition + Vector3.up + base.transform.forward, Vector3.down, out raycastHit2, Mathf.Max(22f, base.transform.position.y - this.eid.target.position.y + 2.5f), LayerMaskDefaults.Get(LMD.Environment), QueryTriggerInteraction.Ignore))
				{
					this.rb.velocity = vector2;
				}
				else
				{
					this.rb.velocity = Vector3.up * 100f * this.anim.speed;
				}
			}
		}
		if (this.rollCooldown > 0f)
		{
			this.rollCooldown = Mathf.MoveTowards(this.rollCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.vaultCooldown > 0f)
		{
			this.vaultCooldown = Mathf.MoveTowards(this.vaultCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0003D6BC File Offset: 0x0003B8BC
	private void FixedUpdate()
	{
		if (this.gce.onGround && !this.moving && !this.uppercutting && !this.jumping)
		{
			this.nma.enabled = !this.inAction;
			this.rb.useGravity = false;
			this.rb.isKinematic = true;
		}
		else if (!this.gce.onGround && !this.inAction)
		{
			this.rb.useGravity = true;
			this.rb.isKinematic = false;
			this.nma.enabled = false;
			this.jumping = false;
			if (this.rb)
			{
				this.rb.AddForce(Vector3.down * 20f * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
		if (this.gce.onGround)
		{
			this.lastGroundedPosition = base.transform.position;
		}
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0003D7AC File Offset: 0x0003B9AC
	private void Downslam()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("Downslam");
		this.backTrailActive = false;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0003D820 File Offset: 0x0003BA20
	private void BackstepAttack()
	{
		this.SnapToGround();
		this.inAction = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("BackstepAttack");
		this.backTrailActive = true;
		this.StartMoving(-3.5f);
		this.knockBack = true;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0003D89C File Offset: 0x0003BA9C
	private void Stinger()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("Stinger");
		this.backTrailActive = true;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0003D910 File Offset: 0x0003BB10
	private void Vault()
	{
		this.SnapToGround();
		this.bodyTrail.emitting = true;
		this.inAction = true;
		this.tracking = true;
		this.StartMoving(0.5f);
		this.anim.SetTrigger("Vault");
		this.backTrailActive = false;
		this.useMain = false;
		this.useOar = false;
		this.useKick = true;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0003D974 File Offset: 0x0003BB74
	private void VaultSwing()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		this.StartMoving(0.5f);
		this.anim.SetTrigger("VaultSwing");
		this.backTrailActive = true;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0003D9CC File Offset: 0x0003BBCC
	private void KickCombo()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("KickCombo");
		this.useMain = true;
		this.useOar = false;
		this.useKick = true;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0003DA38 File Offset: 0x0003BC38
	private void OarCombo()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("OarCombo");
		this.backTrailActive = true;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0003DAAC File Offset: 0x0003BCAC
	private void Uppercut()
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = true;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.anim.SetTrigger("Uppercut");
		this.backTrailActive = true;
		this.useMain = true;
		this.useOar = true;
		this.useKick = false;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0003DB20 File Offset: 0x0003BD20
	public void Roll(bool toPlayerSide = false)
	{
		this.SnapToGround();
		this.inAction = true;
		this.tracking = false;
		if (this.nma.isOnNavMesh)
		{
			this.nma.SetDestination(base.transform.position);
		}
		this.nma.enabled = false;
		this.anim.SetTrigger("Roll");
		this.bodyTrail.emitting = true;
		if (!toPlayerSide)
		{
			base.transform.rotation = Quaternion.LookRotation(this.PredictPlayerPos(false, 20f) - base.transform.position);
		}
		else
		{
			float num = 5f;
			if (Random.Range(0f, 1f) > 0.5f)
			{
				num = -5f;
			}
			base.transform.rotation = Quaternion.LookRotation(this.playerPos + MonoSingleton<CameraController>.Instance.transform.right * num - base.transform.position);
		}
		this.StartMoving(5f);
		if (this.difficulty < 3)
		{
			this.rollCooldown = 5.5f - (float)(this.difficulty * 2);
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0003DC4C File Offset: 0x0003BE4C
	public void LightningBolt(bool quick = false)
	{
		MonoSingleton<EnemyCooldowns>.Instance.ferrymanCooldown += 6f;
		this.inAction = true;
		this.lightningBoltCooldown = (float)(8 - this.difficulty * 2);
		if (quick && this.difficulty >= 4 && this.lightningBoltCooldown < 3f)
		{
			this.lightningBoltCooldown = 3f;
		}
		this.tracking = true;
		if (quick && this.difficulty >= 4)
		{
			this.anim.SetTrigger("QuickLightningBolt");
			return;
		}
		this.anim.SetTrigger("LightningBolt");
		this.lightningCancellable = true;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0003DCE8 File Offset: 0x0003BEE8
	public void LightningBoltWindup(int quick = 0)
	{
		if (this.eid.dead)
		{
			return;
		}
		if (this.eid.zapperer != null)
		{
			this.GotParried();
			return;
		}
		if (this.currentWindup)
		{
			Object.Destroy(this.currentWindup);
		}
		if (this.oarSimplifier)
		{
			this.oarSimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.chargedOar);
		}
		this.currentWindup = Object.Instantiate<GameObject>(this.lightningBoltWindup, this.PredictPlayerPos(false, 5f), Quaternion.identity);
		if (this.eid.target != null)
		{
			Follow[] array = this.currentWindup.GetComponents<Follow>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].target = this.eid.target.targetTransform;
			}
		}
		if (base.transform.parent)
		{
			this.currentWindup.transform.SetParent(base.transform.parent, true);
		}
		foreach (Follow follow in this.currentWindup.GetComponents<Follow>())
		{
			if (follow.speed != 0f)
			{
				if (this.difficulty >= 3)
				{
					follow.speed *= 3f;
				}
				else if (this.difficulty == 2)
				{
					follow.speed *= 2f;
				}
				else if (this.difficulty == 1)
				{
					follow.speed /= 2f;
				}
				else
				{
					follow.enabled = false;
				}
				follow.speed *= this.eid.totalSpeedModifier;
			}
		}
		this.tracking = false;
		this.lightningBoltChimes.Play();
		if (quick == 1)
		{
			ObjectActivator objectActivator;
			if (this.currentWindup.TryGetComponent<ObjectActivator>(out objectActivator))
			{
				objectActivator.delay = 3f;
			}
			base.Invoke("LightningBoltWindupOver", 5f);
		}
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0003DECC File Offset: 0x0003C0CC
	public void LightningBoltWindupOver()
	{
		if (!this.currentWindup)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.unparryableFlash, this.currentWindup.transform.position + Vector3.up * 50f, Quaternion.LookRotation(Vector3.down));
		gameObject.transform.localScale *= 100f;
		gameObject.transform.SetParent(this.currentWindup.transform, true);
		base.Invoke("LightningBoltStrike", 0.5f);
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0003DF65 File Offset: 0x0003C165
	public void LightningBoltStrike()
	{
		this.SpawnLightningBolt(this.currentWindup.transform.position, false);
		Object.Destroy(this.currentWindup);
		this.lightningCancellable = false;
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003DF90 File Offset: 0x0003C190
	public void SpawnLightningBolt(Vector3 position, bool safeForPlayer = false)
	{
		if (this.oarSimplifier)
		{
			this.oarSimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.originalOar);
		}
		LightningStrikeExplosive lightningStrikeExplosive = Object.Instantiate<LightningStrikeExplosive>(this.lightningBolt, position, Quaternion.identity);
		lightningStrikeExplosive.safeForPlayer = safeForPlayer;
		lightningStrikeExplosive.damageMultiplier = this.eid.totalDamageModifier;
		if (base.transform.parent)
		{
			lightningStrikeExplosive.transform.SetParent(base.transform.parent, true);
		}
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0003E010 File Offset: 0x0003C210
	public void CancelLightningBolt()
	{
		if (this.oarSimplifier)
		{
			this.oarSimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.originalOar);
		}
		if (this.currentWindup)
		{
			Object.Destroy(this.currentWindup);
		}
		this.lightningCancellable = false;
		this.anim.Play("Idle");
		this.StopAction();
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003E071 File Offset: 0x0003C271
	public void OnDeath()
	{
		if (this.oarSimplifier)
		{
			this.oarSimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.originalOar);
		}
		Object.Destroy(this);
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0003E098 File Offset: 0x0003C298
	private void StartTracking()
	{
		this.tracking = true;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0003E0A1 File Offset: 0x0003C2A1
	private void StopTracking()
	{
		this.tracking = false;
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0003E0AC File Offset: 0x0003C2AC
	private void StartMoving(float speed)
	{
		this.movingSpeed = speed * 10f;
		this.moving = true;
		this.rb.isKinematic = false;
		foreach (ParticleSystem particleSystem in this.footstepParticles)
		{
			if (Mathf.Abs(particleSystem.transform.position.y - base.transform.position.y) < 1f)
			{
				particleSystem.Play();
			}
		}
		this.Footstep(0.75f);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0003E130 File Offset: 0x0003C330
	private void StopMoving()
	{
		this.bodyTrail.emitting = false;
		this.moving = false;
		this.rb.isKinematic = true;
		foreach (ParticleSystem particleSystem in this.footstepParticles)
		{
			if (Mathf.Abs(particleSystem.transform.position.y - base.transform.position.y) < 1f)
			{
				particleSystem.Play();
			}
		}
		this.Footstep(0.75f);
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0003E1B4 File Offset: 0x0003C3B4
	public void SlamHit()
	{
		Object.Instantiate<GameObject>(this.slamExplosion, new Vector3(this.frontTrail.transform.position.x, base.transform.position.y, this.frontTrail.transform.position.z), Quaternion.identity);
		foreach (ParticleSystem particleSystem in this.footstepParticles)
		{
			if (Mathf.Abs(particleSystem.transform.position.y - base.transform.position.y) < 1f)
			{
				particleSystem.Play();
			}
		}
		this.Footstep(0.75f);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003E268 File Offset: 0x0003C468
	private void Footstep(float volume = 0.5f)
	{
		if (volume == 0f)
		{
			volume = 0.5f;
		}
		this.footstepAudio.volume = volume;
		this.footstepAudio.pitch = Random.Range(1.15f, 1.35f);
		this.footstepAudio.Play();
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0003E2B5 File Offset: 0x0003C4B5
	private void StartUppercut()
	{
		this.uppercutting = true;
		this.rb.isKinematic = false;
		this.StartDamage(25);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003E2D2 File Offset: 0x0003C4D2
	private void StopUppercut()
	{
		this.uppercutting = false;
		this.rb.useGravity = true;
		this.rb.velocity = Vector3.up * 10f;
		this.StopDamage();
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0003E308 File Offset: 0x0003C508
	private void StartDamage(int damage = 25)
	{
		if (damage == 0)
		{
			damage = 25;
		}
		SwingCheck2[] array = this.swingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].damage = damage;
		}
		if (this.useMain)
		{
			this.mainSwingCheck.DamageStart();
		}
		if (this.useOar)
		{
			this.oarSwingCheck.DamageStart();
		}
		if (this.useKick)
		{
			this.kickSwingCheck.DamageStart();
		}
		if (this.useOar)
		{
			this.swingAudioSource.pitch = Random.Range(0.65f, 0.9f);
			this.swingAudioSource.volume = 1f;
		}
		else if (this.useKick)
		{
			this.swingAudioSource.pitch = Random.Range(2.1f, 2.55f);
			this.swingAudioSource.volume = 0.75f;
		}
		this.swingAudioSource.clip = this.swingSounds[Random.Range(0, this.swingSounds.Length)];
		this.swingAudioSource.Play();
		if (this.useMain || this.useOar)
		{
			this.frontTrail.emitting = true;
		}
		if (this.backTrailActive)
		{
			this.backTrail.emitting = true;
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003E434 File Offset: 0x0003C634
	private void StopDamage()
	{
		if (this.swingChecks == null)
		{
			return;
		}
		SwingCheck2[] array = this.swingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
		this.knockBack = false;
		this.frontTrail.emitting = false;
		this.backTrail.emitting = false;
		this.mach.parryable = false;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0003E494 File Offset: 0x0003C694
	public void TargetBeenHit()
	{
		if (this.eid.target != null && this.eid.target.isPlayer && this.knockBack)
		{
			MonoSingleton<NewMovement>.Instance.Launch((this.playerPos - base.transform.position).normalized * 2500f + Vector3.up * 250f, 8f, false);
		}
		SwingCheck2[] array = this.swingChecks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0003E534 File Offset: 0x0003C734
	private void StopAction()
	{
		if (this.moving)
		{
			this.StopMoving();
		}
		this.inAction = false;
		this.nma.enabled = true;
		this.tracking = false;
		if (this.bodyTrail.emitting)
		{
			this.bodyTrail.emitting = false;
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003E584 File Offset: 0x0003C784
	public void ParryableFlash()
	{
		Object.Instantiate<GameObject>(this.parryableFlash, this.head.position + (MonoSingleton<CameraController>.Instance.defaultPos - this.head.position).normalized, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.defaultPos - this.head.position), this.head).transform.localScale *= 0.025f;
		this.mach.ParryableCheck(false);
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0003E61C File Offset: 0x0003C81C
	public void UnparryableFlash()
	{
		Object.Instantiate<GameObject>(this.unparryableFlash, this.head.position + (MonoSingleton<CameraController>.Instance.defaultPos - this.head.position).normalized, Quaternion.LookRotation(MonoSingleton<CameraController>.Instance.defaultPos - this.head.position), this.head).transform.localScale *= 0.025f;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0003E6A8 File Offset: 0x0003C8A8
	public void GotParried()
	{
		this.SpawnLightningBolt(this.mach.chest.transform.position, true);
		this.eid.hitter = "";
		this.eid.hitterAttributes.Add(HitterAttribute.Electricity);
		this.eid.DeliverDamage(base.gameObject, Vector3.zero, base.transform.position, 1E-05f, false, 0f, null, false, false);
		if (this.currentWindup != null)
		{
			if (this.oarSimplifier)
			{
				this.oarSimplifier.ChangeMaterialNew(EnemySimplifier.MaterialState.normal, this.originalOar);
			}
			Object.Destroy(this.currentWindup);
		}
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003E75C File Offset: 0x0003C95C
	private void PlayerStatus()
	{
		this.playerPos = new Vector3(this.eid.target.position.x, base.transform.position.y, this.eid.target.position.z);
		this.playerAbove = this.eid.target.position.y > base.transform.position.y + 3f;
		this.playerBelow = this.eid.target.position.y < base.transform.position.y - 4f;
		Vector3 vector = new Vector3(this.eid.target.GetVelocity().x, 0f, this.eid.target.GetVelocity().z);
		if (vector.magnitude < 1f)
		{
			this.playerApproaching = false;
			this.playerRetreating = false;
			return;
		}
		float num = Mathf.Abs(Vector3.Angle(vector.normalized, this.playerPos - base.transform.position));
		this.playerRetreating = num < 80f;
		this.playerApproaching = num > 135f;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0003E8AC File Offset: 0x0003CAAC
	private Vector3 PredictPlayerPos(bool vertical = false, float maxPrediction = 5f)
	{
		if (this.eid.target == null)
		{
			return base.transform.position + base.transform.forward;
		}
		if (vertical)
		{
			if (this.difficulty <= 1)
			{
				return this.eid.target.position;
			}
			Vector3 vector = Vector3.zero;
			if (this.eid.target != null && this.eid.target.isPlayer)
			{
				vector = ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer) ? Vector3.down : Vector3.zero);
			}
			return this.eid.target.position + this.eid.target.GetVelocity().normalized * Mathf.Min(this.eid.target.GetVelocity().magnitude, 5f) + vector;
		}
		else
		{
			if (this.difficulty <= 1)
			{
				return this.playerPos;
			}
			Vector3 vector2 = new Vector3(this.eid.target.GetVelocity().x, 0f, this.eid.target.GetVelocity().z);
			return this.playerPos + vector2.normalized * Mathf.Min(vector2.magnitude, maxPrediction);
		}
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0003EA08 File Offset: 0x0003CC08
	private void SnapToGround()
	{
		RaycastHit raycastHit;
		if (!this.nma.isOnNavMesh && this.gce.onGround && Physics.Raycast(base.transform.position + Vector3.up * 0.1f, Vector3.down, out raycastHit, 2f, LayerMaskDefaults.Get(LMD.Environment)))
		{
			base.transform.position = raycastHit.point;
		}
		base.transform.rotation = Quaternion.LookRotation(this.playerPos - base.transform.position);
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0003EAA4 File Offset: 0x0003CCA4
	public void PhaseChange()
	{
		this.inPhaseChange = true;
		this.onPhaseChange.Invoke("");
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0003EABD File Offset: 0x0003CCBD
	public void EndPhaseChange()
	{
		this.inPhaseChange = false;
		this.hasPhaseChanged = true;
		if (!this.hasReachedFinalPosition)
		{
			base.transform.position = this.phaseChangePositions[this.phaseChangePositions.Length - 1].position;
		}
	}

	// Token: 0x04000B8B RID: 2955
	private Animator anim;

	// Token: 0x04000B8C RID: 2956
	private Machine mach;

	// Token: 0x04000B8D RID: 2957
	private NavMeshAgent nma;

	// Token: 0x04000B8E RID: 2958
	private Rigidbody rb;

	// Token: 0x04000B8F RID: 2959
	private GroundCheckEnemy gce;

	// Token: 0x04000B90 RID: 2960
	private EnemyIdentifier eid;

	// Token: 0x04000B91 RID: 2961
	private NavMeshPath path;

	// Token: 0x04000B92 RID: 2962
	private int difficulty = -1;

	// Token: 0x04000B93 RID: 2963
	private bool inAction;

	// Token: 0x04000B94 RID: 2964
	private bool tracking;

	// Token: 0x04000B95 RID: 2965
	private bool moving;

	// Token: 0x04000B96 RID: 2966
	private float movingSpeed;

	// Token: 0x04000B97 RID: 2967
	private bool uppercutting;

	// Token: 0x04000B98 RID: 2968
	private Vector3 playerPos;

	// Token: 0x04000B99 RID: 2969
	private bool playerApproaching;

	// Token: 0x04000B9A RID: 2970
	private bool playerRetreating;

	// Token: 0x04000B9B RID: 2971
	private bool playerAbove;

	// Token: 0x04000B9C RID: 2972
	private bool playerBelow;

	// Token: 0x04000B9D RID: 2973
	private float overheadChance = 0.5f;

	// Token: 0x04000B9E RID: 2974
	private float stingerChance = 0.5f;

	// Token: 0x04000B9F RID: 2975
	private float kickComboChance = 0.5f;

	// Token: 0x04000BA0 RID: 2976
	[HideInInspector]
	public float defaultMovementSpeed;

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private GameObject parryableFlash;

	// Token: 0x04000BA2 RID: 2978
	[SerializeField]
	private GameObject unparryableFlash;

	// Token: 0x04000BA3 RID: 2979
	[SerializeField]
	private Transform head;

	// Token: 0x04000BA4 RID: 2980
	[SerializeField]
	private GameObject slamExplosion;

	// Token: 0x04000BA5 RID: 2981
	[SerializeField]
	private GameObject lightningBoltWindup;

	// Token: 0x04000BA6 RID: 2982
	[HideInInspector]
	public GameObject currentWindup;

	// Token: 0x04000BA7 RID: 2983
	[SerializeField]
	private LightningStrikeExplosive lightningBolt;

	// Token: 0x04000BA8 RID: 2984
	[SerializeField]
	private AudioSource lightningBoltChimes;

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private EnemySimplifier oarSimplifier;

	// Token: 0x04000BAA RID: 2986
	private Material originalOar;

	// Token: 0x04000BAB RID: 2987
	[SerializeField]
	private Material chargedOar;

	// Token: 0x04000BAC RID: 2988
	[Header("SwingChecks")]
	[SerializeField]
	private SwingCheck2 mainSwingCheck;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private SwingCheck2 oarSwingCheck;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	private SwingCheck2 kickSwingCheck;

	// Token: 0x04000BAF RID: 2991
	private SwingCheck2[] swingChecks;

	// Token: 0x04000BB0 RID: 2992
	[SerializeField]
	private AudioSource swingAudioSource;

	// Token: 0x04000BB1 RID: 2993
	[SerializeField]
	private AudioClip[] swingSounds;

	// Token: 0x04000BB2 RID: 2994
	private bool useMain;

	// Token: 0x04000BB3 RID: 2995
	private bool useOar;

	// Token: 0x04000BB4 RID: 2996
	private bool useKick;

	// Token: 0x04000BB5 RID: 2997
	private bool knockBack;

	// Token: 0x04000BB6 RID: 2998
	[Header("Trails")]
	[SerializeField]
	private TrailRenderer frontTrail;

	// Token: 0x04000BB7 RID: 2999
	[SerializeField]
	private TrailRenderer backTrail;

	// Token: 0x04000BB8 RID: 3000
	[SerializeField]
	private TrailRenderer bodyTrail;

	// Token: 0x04000BB9 RID: 3001
	private bool backTrailActive;

	// Token: 0x04000BBA RID: 3002
	[Header("Footsteps")]
	[SerializeField]
	private ParticleSystem[] footstepParticles;

	// Token: 0x04000BBB RID: 3003
	[SerializeField]
	private AudioSource footstepAudio;

	// Token: 0x04000BBC RID: 3004
	private float rollCooldown;

	// Token: 0x04000BBD RID: 3005
	private float vaultCooldown;

	// Token: 0x04000BBE RID: 3006
	[Header("Boss Version")]
	[SerializeField]
	private bool bossVersion;

	// Token: 0x04000BBF RID: 3007
	[SerializeField]
	private float phaseChangeHealth;

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private Transform[] phaseChangePositions;

	// Token: 0x04000BC1 RID: 3009
	private int currentPosition;

	// Token: 0x04000BC2 RID: 3010
	[SerializeField]
	private UltrakillEvent onPhaseChange;

	// Token: 0x04000BC3 RID: 3011
	private bool inPhaseChange;

	// Token: 0x04000BC4 RID: 3012
	private bool hasPhaseChanged;

	// Token: 0x04000BC5 RID: 3013
	private bool hasReachedFinalPosition;

	// Token: 0x04000BC6 RID: 3014
	private bool jumping;

	// Token: 0x04000BC7 RID: 3015
	private float lightningBoltCooldown = 1.5f;

	// Token: 0x04000BC8 RID: 3016
	private float lightningOutOfReachCharge;

	// Token: 0x04000BC9 RID: 3017
	private bool lightningCancellable;

	// Token: 0x04000BCA RID: 3018
	private Vector3 lastGroundedPosition;
}
