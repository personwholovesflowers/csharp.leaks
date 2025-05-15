using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

// Token: 0x02000449 RID: 1097
public class Stalker : MonoBehaviour, IEnemyRelationshipLogic
{
	// Token: 0x060018D3 RID: 6355 RVA: 0x000C90CC File Offset: 0x000C72CC
	private void Awake()
	{
		this.mach = base.GetComponent<Machine>();
		this.lit = base.GetComponentInChildren<Light>();
		this.lightAud = this.lit.GetComponent<AudioSource>();
		this.anim = base.GetComponent<Animator>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.nma = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000C9128 File Offset: 0x000C7328
	private void Start()
	{
		this.maxHp = this.mach.health;
		this.currentColor = this.lightColors[0];
		this.lightAud.clip = this.lightSounds[0];
		this.lightAud.loop = false;
		this.lightAud.pitch = 1f;
		this.lightAud.volume = 0.35f;
		this.SetSpeed();
		this.NavigationUpdate();
		this.SlowUpdate();
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000C91A9 File Offset: 0x000C73A9
	private void UpdateBuff()
	{
		this.SetSpeed();
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000C91B4 File Offset: 0x000C73B4
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
		if (this.defaultMovementSpeed == 0f)
		{
			this.defaultMovementSpeed = this.nma.speed;
		}
		this.nma.speed = this.defaultMovementSpeed * this.eid.totalSpeedModifier;
		this.anim.speed = this.eid.totalSpeedModifier;
		this.anim.SetFloat("ExplodeSpeed", this.explodeSpeed);
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000C92B3 File Offset: 0x000C74B3
	private void OnDisable()
	{
		if (this.exploding)
		{
			this.exploding = false;
			this.explosionCharge = this.prepareTime;
			this.inAction = false;
			this.blinking = false;
		}
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000C92E0 File Offset: 0x000C74E0
	private void NavigationUpdate()
	{
		if (this.nma && this.nma.isOnNavMesh)
		{
			if (this.eid.target == null)
			{
				NavMeshAgent navMeshAgent = this.nma;
				if (navMeshAgent != null)
				{
					navMeshAgent.SetDestination(base.transform.position);
				}
			}
			else if (this.eid.target != null && !this.inAction && this.mach && this.mach.grounded)
			{
				NavMeshAgent navMeshAgent2 = this.nma;
				if (navMeshAgent2 != null)
				{
					navMeshAgent2.SetDestination(this.eid.target.position);
				}
			}
			else if (this.mach && this.mach.grounded && this.inAction)
			{
				NavMeshAgent navMeshAgent3 = this.nma;
				if (navMeshAgent3 != null)
				{
					navMeshAgent3.SetDestination(base.transform.position);
				}
			}
		}
		base.Invoke("NavigationUpdate", 0.1f);
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000C93E4 File Offset: 0x000C75E4
	private void SlowUpdate()
	{
		if (this.inAction || (this.mach && !this.mach.grounded) || (this.nma && !this.nma.isOnNavMesh) || !this.eid.AttackEnemies)
		{
			base.Invoke("SlowUpdate", 0.5f);
			return;
		}
		List<EnemyIdentifier> currentEnemies = MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies();
		if (currentEnemies != null && currentEnemies.Count > 0)
		{
			bool flag = false;
			bool flag2 = false;
			float num = float.PositiveInfinity;
			EnemyIdentifier enemyIdentifier = null;
			for (int i = 6; i >= 0; i--)
			{
				for (int j = 0; j < currentEnemies.Count; j++)
				{
					if (!currentEnemies[j].flying && !currentEnemies[j].puppet && !currentEnemies[j].sandified && MonoSingleton<EnemyTracker>.Instance.GetEnemyRank(currentEnemies[j]) == i)
					{
						float num2 = Vector3.Distance(base.transform.position, currentEnemies[j].transform.position);
						if (num2 < num && ((currentEnemies[j].enemyType != EnemyType.MaliciousFace && this.CheckForPath(currentEnemies[j].transform.position)) || (currentEnemies[j].enemyType == EnemyType.MaliciousFace && this.CheckForOffsetPath(currentEnemies[j]))))
						{
							if (this.eid.target != null && currentEnemies[j].transform != this.eid.target.targetTransform)
							{
								if (!MonoSingleton<StalkerController>.Instance.CheckIfTargetTaken(currentEnemies[j].transform))
								{
									if (this.eid.target != null && MonoSingleton<StalkerController>.Instance.CheckIfTargetTaken(this.eid.target.targetTransform))
									{
										MonoSingleton<StalkerController>.Instance.targets.Remove(this.eid.target.targetTransform);
									}
									if (enemyIdentifier != null && MonoSingleton<StalkerController>.Instance.CheckIfTargetTaken(enemyIdentifier.transform))
									{
										MonoSingleton<StalkerController>.Instance.targets.Remove(enemyIdentifier.transform);
									}
									enemyIdentifier = currentEnemies[j];
									MonoSingleton<StalkerController>.Instance.targets.Add(currentEnemies[j].transform);
									if (num2 < 100f)
									{
										flag = true;
									}
									else
									{
										flag2 = true;
									}
									num = Vector3.Distance(base.transform.position, currentEnemies[j].transform.position);
								}
								else if (!flag)
								{
									enemyIdentifier = currentEnemies[j];
									flag2 = true;
									num = Vector3.Distance(base.transform.position, currentEnemies[j].transform.position);
								}
							}
							else
							{
								if (num2 < 100f)
								{
									enemyIdentifier = currentEnemies[j];
									flag = true;
								}
								else
								{
									enemyIdentifier = currentEnemies[j];
									flag2 = true;
								}
								num = Vector3.Distance(base.transform.position, currentEnemies[j].transform.position);
							}
						}
					}
				}
				if (flag)
				{
					this.eid.target = new EnemyTarget(enemyIdentifier.transform);
					enemyIdentifier.buffTargeter = this.eid;
					break;
				}
			}
			if (!flag && flag2)
			{
				this.eid.target = new EnemyTarget(enemyIdentifier.transform);
				enemyIdentifier.buffTargeter = this.eid;
			}
			else if (!flag)
			{
				if (this.eid.target != null)
				{
					if (MonoSingleton<StalkerController>.Instance.CheckIfTargetTaken(this.eid.target.targetTransform))
					{
						MonoSingleton<StalkerController>.Instance.targets.Remove(this.eid.target.targetTransform);
					}
					EnemyIdentifier enemyIdentifier2;
					if (this.eid.target.targetTransform.TryGetComponent<EnemyIdentifier>(out enemyIdentifier2) && enemyIdentifier2.buffTargeter == this.eid)
					{
						enemyIdentifier2.buffTargeter = null;
					}
				}
				EnemyScanner enemyScanner = this.eid.enemyScanner;
				if (enemyScanner != null)
				{
					enemyScanner.Reset();
				}
				this.eid.target = EnemyTarget.TrackPlayerIfAllowed();
			}
		}
		base.Invoke("SlowUpdate", 0.5f);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000C982C File Offset: 0x000C7A2C
	private void Update()
	{
		if (this.exploding)
		{
			if (this.countDownAmount < 2f)
			{
				this.countDownAmount = Mathf.MoveTowards(this.countDownAmount, 2f, Time.deltaTime * this.explodeSpeed * this.eid.totalSpeedModifier);
			}
			else
			{
				this.exploding = false;
				this.SandExplode(0);
			}
		}
		else if (this.explosionCharge < this.prepareTime)
		{
			this.explosionCharge = Mathf.MoveTowards(this.explosionCharge, this.prepareTime, Time.deltaTime * this.eid.totalSpeedModifier);
			if (this.explosionCharge > this.prepareWarningTime)
			{
				this.blinking = true;
			}
		}
		else
		{
			if (this.lit.color != this.lightColors[1] * (this.mach.health / this.maxHp))
			{
				this.blinking = false;
				this.currentColor = this.lightColors[1];
				this.lightAud.clip = this.lightSounds[1];
				this.lightAud.loop = true;
				this.lightAud.pitch = 0.5f;
				this.lightAud.volume = 0.65f;
				this.lightAud.Play();
			}
			if (this.explosionCharge < this.prepareTime + 1f)
			{
				this.explosionCharge = Mathf.MoveTowards(this.explosionCharge, this.prepareTime + 1f, Time.deltaTime * this.eid.totalSpeedModifier);
			}
			else if (this.eid.target != null && Vector3.Distance(base.transform.position, this.eid.target.position) < 8f && !this.exploding)
			{
				this.exploding = true;
				this.Countdown();
			}
		}
		if (!this.inAction)
		{
			if (this.nma.velocity.magnitude > 5f)
			{
				this.anim.SetBool("Running", true);
			}
			else
			{
				this.anim.SetBool("Running", false);
			}
			if (this.nma.velocity.magnitude > 0f)
			{
				this.anim.SetBool("Walking", true);
			}
			else
			{
				this.anim.SetBool("Walking", false);
			}
			if (!this.mach.grounded)
			{
				this.anim.SetBool("Falling", true);
			}
			else
			{
				this.anim.SetBool("Falling", false);
			}
		}
		if (this.blinking)
		{
			if (this.blinkTimer > 0f)
			{
				this.blinkTimer = Mathf.MoveTowards(this.blinkTimer, 0f, Time.deltaTime);
			}
			else
			{
				if (this.lit.color != Color.black)
				{
					this.lit.color = Color.black;
					AudioSource audioSource = this.lightAud;
					if (audioSource != null)
					{
						audioSource.Stop();
					}
				}
				else
				{
					this.lit.color = this.currentColor * ((this.mach.health + 0.2f) / (this.maxHp + 0.2f));
					AudioSource audioSource2 = this.lightAud;
					if (audioSource2 != null)
					{
						audioSource2.Play();
					}
				}
				this.blinkTimer = 0.1f;
			}
		}
		else
		{
			this.lit.color = this.currentColor * ((this.mach.health + 0.2f) / (this.maxHp + 0.2f));
			this.blinkTimer = 0f;
		}
		if (this.canRenderer)
		{
			this.canRenderer.material.SetColor("_EmissiveColor", this.lit.color);
		}
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x000C9BF4 File Offset: 0x000C7DF4
	public void Countdown()
	{
		this.inAction = true;
		this.blinking = true;
		this.currentColor = this.lightColors[2];
		this.lightAud.clip = this.lightSounds[2];
		this.lightAud.loop = false;
		this.lightAud.pitch = 1f;
		this.lightAud.volume = 0.65f;
		this.explosionCharge = 0f;
		this.countDownAmount = 0f;
		Object.Instantiate<GameObject>(this.screamSound, base.transform);
		this.anim.SetTrigger("Explode");
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000C9C98 File Offset: 0x000C7E98
	public void SandExplode(int onDeath = 1)
	{
		if (this.exploded)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.explosion.ToAsset(), base.transform.position + Vector3.up * 2.5f, Quaternion.identity);
		if (onDeath != 1)
		{
			gameObject.transform.localScale *= 1.5f;
		}
		if (this.eid.stuckMagnets.Count > 0)
		{
			float num = 0.75f;
			if (this.eid.stuckMagnets.Count > 1)
			{
				num -= 0.125f * (float)(this.eid.stuckMagnets.Count - 1);
			}
			gameObject.transform.localScale *= num;
		}
		if (this.eid.target != null && this.eid.target.enemyIdentifier && this.eid.target.enemyIdentifier.sandified)
		{
			if (MonoSingleton<StalkerController>.Instance.CheckIfTargetTaken(this.eid.target.targetTransform))
			{
				MonoSingleton<StalkerController>.Instance.targets.Remove(this.eid.target.targetTransform);
			}
			EnemyIdentifier enemyIdentifier;
			if (this.eid.target.targetTransform.TryGetComponent<EnemyIdentifier>(out enemyIdentifier) && enemyIdentifier.buffTargeter == this.eid)
			{
				enemyIdentifier.buffTargeter = null;
			}
		}
		if ((this.difficulty > 3 || this.eid.blessed || InvincibleEnemies.Enabled) && onDeath != 1)
		{
			this.exploding = false;
			this.countDownAmount = 0f;
			this.explosionCharge = 0f;
			this.currentColor = this.lightColors[0];
			this.lightAud.clip = this.lightSounds[0];
			this.blinking = false;
			return;
		}
		this.exploded = true;
		if (!this.mach.limp && onDeath != 1)
		{
			this.mach.GoLimp();
			this.eid.Death();
		}
		if (this.eid.drillers.Count != 0)
		{
			for (int i = this.eid.drillers.Count - 1; i >= 0; i--)
			{
				Object.Destroy(this.eid.drillers[i].gameObject);
			}
		}
		base.gameObject.SetActive(false);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060018DD RID: 6365 RVA: 0x000C9F0C File Offset: 0x000C810C
	public bool CheckForPath(Vector3 pathTarget)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		this.nma.CalculatePath(pathTarget, navMeshPath);
		return navMeshPath != null && navMeshPath.status == NavMeshPathStatus.PathComplete;
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x000C9F3C File Offset: 0x000C813C
	public bool CheckForOffsetPath(EnemyIdentifier ed)
	{
		NavMeshAgent navMeshAgent;
		return ed.TryGetComponent<NavMeshAgent>(out navMeshAgent) && this.CheckForPath(ed.transform.position + Vector3.down * navMeshAgent.height * navMeshAgent.baseOffset * ed.transform.localScale.y);
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x000C9F9B File Offset: 0x000C819B
	public void StopAction()
	{
		this.inAction = false;
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x000C9FA4 File Offset: 0x000C81A4
	public void Step()
	{
		Object.Instantiate<GameObject>(this.stepSound, base.transform.position, Quaternion.identity);
	}

	// Token: 0x060018E1 RID: 6369 RVA: 0x0002D245 File Offset: 0x0002B445
	public bool ShouldAttackEnemies()
	{
		return true;
	}

	// Token: 0x060018E2 RID: 6370 RVA: 0x000C9FC2 File Offset: 0x000C81C2
	public bool ShouldIgnorePlayer()
	{
		return false;
	}

	// Token: 0x040022A8 RID: 8872
	private EnemyIdentifier eid;

	// Token: 0x040022A9 RID: 8873
	private Machine mach;

	// Token: 0x040022AA RID: 8874
	private int difficulty = -1;

	// Token: 0x040022AB RID: 8875
	private NavMeshAgent nma;

	// Token: 0x040022AC RID: 8876
	[HideInInspector]
	public float defaultMovementSpeed;

	// Token: 0x040022AD RID: 8877
	private Animator anim;

	// Token: 0x040022AE RID: 8878
	private bool inAction;

	// Token: 0x040022AF RID: 8879
	private float explosionCharge;

	// Token: 0x040022B0 RID: 8880
	private float countDownAmount;

	// Token: 0x040022B1 RID: 8881
	private bool exploding;

	// Token: 0x040022B2 RID: 8882
	private bool exploded;

	// Token: 0x040022B3 RID: 8883
	public AssetReference explosion;

	// Token: 0x040022B4 RID: 8884
	private float maxHp;

	// Token: 0x040022B5 RID: 8885
	private Light lit;

	// Token: 0x040022B6 RID: 8886
	private Color currentColor;

	// Token: 0x040022B7 RID: 8887
	public Color[] lightColors;

	// Token: 0x040022B8 RID: 8888
	private bool blinking;

	// Token: 0x040022B9 RID: 8889
	private float blinkTimer;

	// Token: 0x040022BA RID: 8890
	private AudioSource lightAud;

	// Token: 0x040022BB RID: 8891
	public AudioClip[] lightSounds;

	// Token: 0x040022BC RID: 8892
	public SkinnedMeshRenderer canRenderer;

	// Token: 0x040022BD RID: 8893
	public GameObject stepSound;

	// Token: 0x040022BE RID: 8894
	public GameObject screamSound;

	// Token: 0x040022BF RID: 8895
	private float explodeSpeed = 1f;

	// Token: 0x040022C0 RID: 8896
	public float prepareTime = 5f;

	// Token: 0x040022C1 RID: 8897
	public float prepareWarningTime = 3f;
}
