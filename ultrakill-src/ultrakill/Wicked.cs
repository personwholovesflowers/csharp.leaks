using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020004CD RID: 1229
public class Wicked : MonoBehaviour
{
	// Token: 0x06001C1A RID: 7194 RVA: 0x000E93AC File Offset: 0x000E75AC
	private void Start()
	{
		this.player = MonoSingleton<PlayerTracker>.Instance.GetPlayer().gameObject;
		this.nma = base.GetComponent<NavMeshAgent>();
		this.aud = base.GetComponent<AudioSource>();
		this.eid = base.GetComponent<EnemyIdentifier>();
		this.anim = base.GetComponent<Animator>();
		if (this.targetPoint == null)
		{
			this.targetPoint = this.patrolPoints[Random.Range(0, this.patrolPoints.Length)];
		}
	}

	// Token: 0x06001C1B RID: 7195 RVA: 0x000E9428 File Offset: 0x000E7628
	private void Update()
	{
		if (this.alwaysRunning || this.playerSpotTime > 0f)
		{
			this.anim.SetBool("Running", true);
			this.nma.speed = 22f * this.eid.totalSpeedModifier;
		}
		else
		{
			this.anim.SetBool("Running", false);
			this.nma.speed = 8f * this.eid.totalSpeedModifier;
		}
		if (!BlindEnemies.Blind && !Physics.Raycast(base.transform.position + Vector3.up * 2f, this.player.transform.position - base.transform.position + Vector3.up * 2f, Vector3.Distance(base.transform.position + Vector3.up * 2f, this.player.transform.position), LayerMaskDefaults.Get(LMD.Environment)))
		{
			if (!this.lineOfSight && !this.aud.isPlaying)
			{
				this.aud.Play();
			}
			this.lineOfSight = true;
			this.playerSpotTime = 5f;
		}
		else
		{
			this.lineOfSight = false;
			if (this.playerSpotTime != 0f)
			{
				this.playerSpotTime = Mathf.MoveTowards(this.playerSpotTime, 0f, Time.deltaTime);
			}
		}
		if (this.playerSpotTime <= 0f)
		{
			if (Vector3.Distance(base.transform.position, this.targetPoint.position) < 1f)
			{
				this.targetPoint = this.patrolPoints[Random.Range(0, this.patrolPoints.Length)];
			}
			this.nma.SetDestination(this.targetPoint.position);
			return;
		}
		this.nma.SetDestination(this.player.transform.position);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x000E962C File Offset: 0x000E782C
	public void GetHit()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Object.Instantiate<GameObject>(this.hitSound, base.transform.position, Quaternion.identity);
		Vector3 vector = this.patrolPoints[0].position;
		float num = 0f;
		for (int i = 0; i < this.patrolPoints.Length; i++)
		{
			if (Vector3.Distance(this.patrolPoints[i].position, this.player.transform.position) > num)
			{
				num = Vector3.Distance(this.patrolPoints[i].position, this.player.transform.position);
				vector = this.patrolPoints[i].position;
			}
		}
		if (this.eid && this.eid.hooked)
		{
			MonoSingleton<HookArm>.Instance.StopThrow(1f, true);
		}
		MonoSingleton<BestiaryData>.Instance.SetEnemy(EnemyType.Wicked, 2);
		if (this.aud.isPlaying)
		{
			this.aud.Stop();
		}
		this.nma.Warp(vector);
		this.playerSpotTime = 0f;
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x000E9748 File Offset: 0x000E7948
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
			{
				MonoSingleton<NewMovement>.Instance.GetHurt(Mathf.RoundToInt(999f * this.eid.totalDamageModifier), false, 1f, false, false, 0.35f, false);
			}
			else
			{
				MonoSingleton<PlatformerMovement>.Instance.Explode(false);
			}
		}
		this.GetHit();
	}

	// Token: 0x06001C1E RID: 7198 RVA: 0x000E97B4 File Offset: 0x000E79B4
	public void AlwaysRunning(bool isOn)
	{
		this.alwaysRunning = isOn;
	}

	// Token: 0x0400279F RID: 10143
	public Transform[] patrolPoints;

	// Token: 0x040027A0 RID: 10144
	public Transform targetPoint;

	// Token: 0x040027A1 RID: 10145
	private GameObject player;

	// Token: 0x040027A2 RID: 10146
	public float playerSpotTime;

	// Token: 0x040027A3 RID: 10147
	private AudioSource aud;

	// Token: 0x040027A4 RID: 10148
	private NavMeshAgent nma;

	// Token: 0x040027A5 RID: 10149
	private Animator anim;

	// Token: 0x040027A6 RID: 10150
	public GameObject hitSound;

	// Token: 0x040027A7 RID: 10151
	private bool lineOfSight;

	// Token: 0x040027A8 RID: 10152
	private EnemyIdentifier eid;

	// Token: 0x040027A9 RID: 10153
	public bool alwaysRunning;
}
