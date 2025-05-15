using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

// Token: 0x02000454 RID: 1108
public class Streetcleaner : MonoBehaviour
{
	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06001942 RID: 6466 RVA: 0x000CF38F File Offset: 0x000CD58F
	private EnemyTarget target
	{
		get
		{
			return this.eid.target;
		}
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000CF39C File Offset: 0x000CD59C
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.anim = base.GetComponentInChildren<Animator>();
		this.nma = base.GetComponent<NavMeshAgent>();
		this.enviroMask = LayerMaskDefaults.Get(LMD.Environment);
		this.enviroMask |= 2048;
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000CF404 File Offset: 0x000CD604
	private void Start()
	{
		if (!this.dead)
		{
			this.playergc = MonoSingleton<NewMovement>.Instance.gc;
			this.handTrail = base.GetComponentInChildren<TrailRenderer>();
			this.handTrail.emitting = false;
			this.warningFlame = this.firePoint.GetComponentInChildren<SpriteRenderer>().transform;
			this.warningFlame.localScale = Vector3.zero;
			this.firePart = this.firePoint.GetComponentInChildren<ParticleSystem>();
			this.fireLight = this.firePoint.GetComponentInChildren<Light>();
			this.fireLight.enabled = false;
			this.fireAud = this.firePoint.GetComponent<AudioSource>();
			this.torsoDefaultRotation = Quaternion.Inverse(base.transform.rotation) * this.aimBone.rotation;
			if (this.eid.difficultyOverride >= 0)
			{
				this.difficulty = this.eid.difficultyOverride;
			}
			else
			{
				this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
			}
			this.gc = base.GetComponentInChildren<GroundCheckEnemy>();
			this.mach = base.GetComponent<Machine>();
			this.SlowUpdate();
		}
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x000CF524 File Offset: 0x000CD724
	private void OnDisable()
	{
		if (this.dodging)
		{
			this.StopMoving();
		}
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x000CF534 File Offset: 0x000CD734
	private void SlowUpdate()
	{
		if (!this.dead)
		{
			if (this.playerInAir && this.nma != null && this.nma.enabled && this.nma.isOnNavMesh && this.target != null)
			{
				if (Physics.Raycast(this.target.position + Vector3.up * 0.1f, Vector3.down, out this.rhit, 20f, this.enviroMask))
				{
					this.nma.SetDestination(this.rhit.point);
				}
				else
				{
					this.nma.SetDestination(this.target.position);
				}
			}
			base.Invoke("SlowUpdate", 0.25f);
		}
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x000CF610 File Offset: 0x000CD810
	private void Update()
	{
		if (this.dodgeCooldown != 0f)
		{
			this.dodgeCooldown = Mathf.MoveTowards(this.dodgeCooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.difficulty <= 2 && this.cooldown > 0f)
		{
			this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime * this.eid.totalSpeedModifier);
		}
		if (this.target == null)
		{
			if (!this.dead)
			{
				if (this.nma && this.nma.isOnNavMesh)
				{
					this.nma.isStopped = true;
					this.nma.ResetPath();
				}
				if (this.anim)
				{
					this.anim.SetBool("Running", false);
					this.anim.SetBool("Walking", false);
				}
			}
			return;
		}
		if (!this.dead)
		{
			if (this.nma.enabled && this.nma.isOnNavMesh)
			{
				if (this.target.isPlayer)
				{
					if (this.playergc != null && !this.playergc.onGround)
					{
						this.playerInAir = true;
					}
					else
					{
						this.playerInAir = false;
						this.nma.SetDestination(this.target.position);
					}
				}
				else
				{
					this.playerInAir = false;
					this.nma.SetDestination(this.target.position);
					if (this.nma.isStopped)
					{
						this.nma.isStopped = false;
					}
				}
				if ((!this.attacking && this.GetDistanceToTarget() > 6f) || (this.attacking && this.GetDistanceToTarget() > 16f))
				{
					if (this.difficulty >= 4)
					{
						this.anim.SetFloat("RunSpeed", 1.5f);
					}
					else
					{
						this.anim.SetFloat("RunSpeed", 1f);
					}
					if (this.difficulty >= 4)
					{
						this.nma.speed = 24f;
					}
					else if (this.difficulty >= 2)
					{
						this.nma.speed = 16f;
					}
					else if (this.difficulty == 1)
					{
						this.nma.speed = 14f;
					}
					else if (this.difficulty == 0)
					{
						this.nma.speed = 10f;
					}
					this.nma.speed *= this.eid.totalSpeedModifier;
					if (this.attacking)
					{
						this.StopFire();
					}
				}
				else if (this.GetDistanceToTarget() < 6f)
				{
					if (this.difficulty >= 4)
					{
						this.anim.SetFloat("RunSpeed", 1.25f);
					}
					else
					{
						this.anim.SetFloat("RunSpeed", 1f);
					}
					if (this.difficulty >= 4)
					{
						this.nma.speed = 20f;
					}
					else if (this.difficulty >= 2)
					{
						this.nma.speed = 16f;
					}
					else if (this.difficulty == 1)
					{
						this.nma.speed = 7f;
					}
					else if (this.difficulty == 0)
					{
						this.nma.speed = 1f;
					}
					this.nma.speed *= this.eid.totalSpeedModifier;
					if (!this.attacking && (this.difficulty > 2 || this.cooldown <= 0f))
					{
						this.attacking = true;
						GameObject gameObject = Object.Instantiate<GameObject>(this.warningFlash, this.firePoint.transform.position, this.firePoint.transform.rotation);
						gameObject.transform.localScale = Vector3.one * 8f;
						gameObject.transform.SetParent(this.firePoint.transform, true);
						if (this.difficulty >= 2)
						{
							base.Invoke("StartFire", 0.5f / this.eid.totalSpeedModifier);
						}
						else
						{
							base.Invoke("StartFire", 1f / this.eid.totalSpeedModifier);
						}
					}
				}
				float num = 12f;
				if (this.difficulty == 1)
				{
					num = 10f;
				}
				else if (this.difficulty == 0)
				{
					num = 4f;
				}
				if (this.nma.velocity.magnitude > num && !this.attacking)
				{
					this.anim.SetBool("Running", true);
					this.anim.SetBool("Walking", true);
				}
				else if (this.nma.velocity.magnitude > 2f)
				{
					this.anim.SetBool("Running", false);
					this.anim.SetBool("Walking", true);
				}
				else
				{
					this.anim.SetBool("Running", false);
					this.anim.SetBool("Walking", false);
				}
			}
			if (this.damaging)
			{
				this.TryIgniteStains();
			}
		}
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000CFB1C File Offset: 0x000CDD1C
	private void FixedUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		if (!this.dead && this.dodging)
		{
			this.rb.velocity = base.transform.forward * -1f * this.dodgeSpeed * this.eid.totalSpeedModifier;
			this.dodgeSpeed = this.dodgeSpeed * 0.95f / this.eid.totalSpeedModifier;
		}
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x000CFB9C File Offset: 0x000CDD9C
	private void LateUpdate()
	{
		if (this.difficulty < 4)
		{
			return;
		}
		if (this.attacking && this.target != null)
		{
			float num = (float)((this.difficulty == 5) ? 90 : 35);
			Quaternion rotation = this.aimBone.rotation;
			Quaternion quaternion = Quaternion.RotateTowards(this.aimBone.rotation, Quaternion.LookRotation(this.eid.target.headPosition - this.aimBone.position, Vector3.up), num);
			Quaternion quaternion2 = Quaternion.Inverse(base.transform.rotation * this.torsoDefaultRotation) * this.aimBone.rotation;
			if (Vector3.Dot(Vector3.up, quaternion * Vector3.forward) > 0f)
			{
				this.aimBone.rotation = quaternion * quaternion2;
			}
			Quaternion quaternion3 = Quaternion.Inverse(rotation) * this.aimBone.rotation;
			quaternion3 = Quaternion.Euler(-quaternion3.eulerAngles.y, quaternion3.eulerAngles.z, -quaternion3.eulerAngles.x);
			this.flameThrowerBone.rotation = this.flameThrowerBone.rotation * quaternion3;
		}
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000CFCDC File Offset: 0x000CDEDC
	public void StartFire()
	{
		this.fireAud.Play();
		this.firePart.Play();
		this.fireLight.enabled = true;
		base.Invoke("StartDamaging", 0.15f / this.eid.totalSpeedModifier);
		if (this.difficulty == 0)
		{
			base.Invoke("StopFire", 0.5f / this.eid.totalSpeedModifier);
			return;
		}
		if (this.difficulty <= 2)
		{
			base.Invoke("StopFire", 1f / this.eid.totalSpeedModifier);
		}
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000CFD71 File Offset: 0x000CDF71
	public void StartDamaging()
	{
		this.damaging = true;
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000CFD7C File Offset: 0x000CDF7C
	public void StopFire()
	{
		if (this.fireAud && this.fireAud.isPlaying)
		{
			this.fireAud.Stop();
			Object.Instantiate<GameObject>(this.fireStopSound, this.firePoint.transform.position, Quaternion.identity);
		}
		this.attacking = false;
		base.CancelInvoke("StartFire");
		base.CancelInvoke("StartDamaging");
		this.firePart.Stop();
		this.fireLight.enabled = false;
		this.warningFlame.localScale = Vector3.zero;
		this.damaging = false;
		if (this.difficulty <= 2)
		{
			if (this.difficulty == 2)
			{
				this.cooldown = 1f;
			}
			else if (this.difficulty == 1)
			{
				this.cooldown = 1.5f;
			}
			else if (this.difficulty == 0)
			{
				this.cooldown = 2f;
			}
			base.CancelInvoke("StopFire");
		}
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000CFE70 File Offset: 0x000CE070
	public void Dodge()
	{
		if (!this.dead && this.dodgeCooldown == 0f)
		{
			this.dodgeCooldown = Random.Range(2f, 4f);
			this.dodgeSpeed = 60f;
			this.nma.enabled = false;
			this.rb.isKinematic = false;
			this.eid.hookIgnore = true;
			this.StopFire();
			if ((Random.Range(0f, 1f) > 0.5f && !Physics.Raycast(base.transform.position + Vector3.up, base.transform.right * -1f, 5f, this.enviroMask, QueryTriggerInteraction.Ignore)) || Physics.Raycast(base.transform.position + Vector3.up, base.transform.right, 5f, this.enviroMask, QueryTriggerInteraction.Ignore))
			{
				base.transform.LookAt(base.transform.position + base.transform.right);
			}
			else
			{
				base.transform.LookAt(base.transform.position + base.transform.right * -1f);
			}
			Object.Instantiate<GameObject>(this.dodgeSound, base.transform.position, Quaternion.identity);
			this.anim.SetTrigger("Dodge");
			this.dodging = true;
		}
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000D0000 File Offset: 0x000CE200
	public void StopMoving()
	{
		if (!this.dead)
		{
			this.dodging = false;
			this.nma.enabled = false;
			this.eid.hookIgnore = false;
			if (this.gc.onGround)
			{
				this.rb.isKinematic = true;
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(this.gc.transform.position, out navMeshHit, 4f, 1))
				{
					this.nma.Warp(navMeshHit.position);
					this.nma.enabled = true;
				}
			}
			this.rb.velocity = Vector3.zero;
		}
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void DodgeEnd()
	{
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000D009E File Offset: 0x000CE29E
	public void DeflectShot()
	{
		if (!this.dead)
		{
			this.anim.SetLayerWeight(1, 1f);
			this.anim.SetTrigger("Deflect");
			this.handTrail.emitting = true;
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000D00D5 File Offset: 0x000CE2D5
	public void SlapOver()
	{
		if (!this.dead)
		{
			this.handTrail.emitting = false;
		}
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000D00EB File Offset: 0x000CE2EB
	public void OverrideOver()
	{
		if (!this.dead)
		{
			this.anim.SetLayerWeight(1, 0f);
		}
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000D0108 File Offset: 0x000CE308
	private float GetDistanceToTarget()
	{
		float num = Vector3.Distance(base.transform.position, this.target.position);
		if (this.target != null && this.target.isEnemy && this.target.enemyIdentifier != null)
		{
			num *= this.target.enemyIdentifier.GetReachDistanceMultiplier();
		}
		return num;
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000D0170 File Offset: 0x000CE370
	private void TryIgniteStains()
	{
		Vector3 position = this.firePoint.transform.position;
		Vector3 forward = this.firePoint.transform.forward;
		Vector3 vector = position + forward * 3.75f;
		Vector3 vector2 = position + forward * 7.5f;
		Vector3 vector3 = position + forward * 15f;
		StainVoxelManager instance = MonoSingleton<StainVoxelManager>.Instance;
		if (instance == null)
		{
			return;
		}
		instance.TryIgniteAt(position, 1);
		instance.TryIgniteAt(vector, 1);
		instance.TryIgniteAt(vector2, 1);
		instance.TryIgniteAt(vector3, 1);
	}

	// Token: 0x0400236E RID: 9070
	private Animator anim;

	// Token: 0x0400236F RID: 9071
	private NavMeshAgent nma;

	// Token: 0x04002370 RID: 9072
	private Rigidbody rb;

	// Token: 0x04002371 RID: 9073
	public bool dead;

	// Token: 0x04002372 RID: 9074
	private TrailRenderer handTrail;

	// Token: 0x04002373 RID: 9075
	private LayerMask enviroMask;

	// Token: 0x04002374 RID: 9076
	public bool dodging;

	// Token: 0x04002375 RID: 9077
	private float dodgeSpeed;

	// Token: 0x04002376 RID: 9078
	private float dodgeCooldown;

	// Token: 0x04002377 RID: 9079
	public GameObject dodgeSound;

	// Token: 0x04002378 RID: 9080
	public Transform hose;

	// Token: 0x04002379 RID: 9081
	public Transform hoseTarget;

	// Token: 0x0400237A RID: 9082
	public GameObject canister;

	// Token: 0x0400237B RID: 9083
	public AssetReference explosion;

	// Token: 0x0400237C RID: 9084
	public bool canisterHit;

	// Token: 0x0400237D RID: 9085
	public GameObject firePoint;

	// Token: 0x0400237E RID: 9086
	private Transform warningFlame;

	// Token: 0x0400237F RID: 9087
	private ParticleSystem firePart;

	// Token: 0x04002380 RID: 9088
	private Light fireLight;

	// Token: 0x04002381 RID: 9089
	private AudioSource fireAud;

	// Token: 0x04002382 RID: 9090
	public GameObject fireStopSound;

	// Token: 0x04002383 RID: 9091
	public bool damaging;

	// Token: 0x04002384 RID: 9092
	private bool attacking;

	// Token: 0x04002385 RID: 9093
	public GameObject warningFlash;

	// Token: 0x04002386 RID: 9094
	[SerializeField]
	private Transform aimBone;

	// Token: 0x04002387 RID: 9095
	private Quaternion torsoDefaultRotation;

	// Token: 0x04002388 RID: 9096
	[SerializeField]
	private Transform flameThrowerBone;

	// Token: 0x04002389 RID: 9097
	private int difficulty;

	// Token: 0x0400238A RID: 9098
	private float cooldown;

	// Token: 0x0400238B RID: 9099
	private RaycastHit rhit;

	// Token: 0x0400238C RID: 9100
	private GroundCheck playergc;

	// Token: 0x0400238D RID: 9101
	private bool playerInAir;

	// Token: 0x0400238E RID: 9102
	private GroundCheckEnemy gc;

	// Token: 0x0400238F RID: 9103
	private Machine mach;

	// Token: 0x04002390 RID: 9104
	[HideInInspector]
	public EnemyIdentifier eid;
}
