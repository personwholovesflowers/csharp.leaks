using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x020002CB RID: 715
public class LeviathanTail : MonoBehaviour, IHitTargetCallback
{
	// Token: 0x06000F94 RID: 3988 RVA: 0x00073B8C File Offset: 0x00071D8C
	private void Awake()
	{
		this.tails = base.GetComponentsInChildren<SwingCheck2>();
		this.anim = base.GetComponent<Animator>();
		EnemyIdentifier enemyIdentifier = (this.lcon ? this.lcon.eid : this.lcon.GetComponent<EnemyIdentifier>());
		SwingCheck2[] array = this.tails;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].eid = enemyIdentifier;
		}
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00073BF8 File Offset: 0x00071DF8
	private void Update()
	{
		if (this.idling && !BlindEnemies.Blind)
		{
			this.idling = false;
			this.anim.speed = this.GetAnimSpeed() * this.lcon.eid.totalSpeedModifier;
			AudioSource[] array = this.spawnAuds;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Play();
			}
		}
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00073C5C File Offset: 0x00071E5C
	public void TargetBeenHit()
	{
		SwingCheck2[] array = this.tails;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DamageStop();
		}
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00073C88 File Offset: 0x00071E88
	private void SwingStart()
	{
		foreach (SwingCheck2 swingCheck in this.tails)
		{
			swingCheck.DamageStart();
			swingCheck.col.isTrigger = true;
		}
		Object.Instantiate<AudioSource>(this.swingSound, base.transform.position, Quaternion.identity).pitch = 0.5f;
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x00073CE4 File Offset: 0x00071EE4
	public void SwingEnd()
	{
		if (this.tails == null || this.tails.Length == 0)
		{
			return;
		}
		foreach (SwingCheck2 swingCheck in this.tails)
		{
			swingCheck.DamageStop();
			swingCheck.col.isTrigger = false;
		}
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00073D2C File Offset: 0x00071F2C
	private void ActionOver()
	{
		base.gameObject.SetActive(false);
		this.lcon.SubAttackOver();
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x00073D48 File Offset: 0x00071F48
	public void ChangePosition()
	{
		if (this.lcon.secondPhase)
		{
			Vector3 vector = this.secondSpawnPositions[0];
			float num = Vector3.Distance(base.transform.parent.position + this.secondSpawnPositions[0], this.lcon.head.Target.position);
			if (this.secondSpawnPositions.Length > 1)
			{
				for (int i = 1; i < this.secondSpawnPositions.Length; i++)
				{
					float num2 = Vector3.Distance(base.transform.parent.position + this.secondSpawnPositions[i], this.lcon.head.Target.position);
					if (num2 < num)
					{
						num = num2;
						vector = this.secondSpawnPositions[i];
					}
				}
			}
			base.transform.localPosition = vector;
		}
		else
		{
			int num3 = Random.Range(0, this.spawnPositions.Length);
			if (this.spawnPositions.Length > 1 && num3 == this.previousSpawnPosition)
			{
				num3++;
			}
			if (num3 >= this.spawnPositions.Length)
			{
				num3 = 0;
			}
			if (this.lcon.head && this.lcon.head.gameObject.activeInHierarchy && Vector3.Distance(this.spawnPositions[num3], new Vector3(this.lcon.head.transform.localPosition.x, this.spawnPositions[num3].y, this.lcon.head.transform.localPosition.z)) < 10f)
			{
				num3++;
			}
			if (num3 >= this.spawnPositions.Length)
			{
				num3 = 0;
			}
			base.transform.localPosition = this.spawnPositions[num3];
			this.previousSpawnPosition = num3;
		}
		bool flag = Random.Range(0f, 1f) > 0.5f;
		base.transform.localPosition += (flag ? (Vector3.up * -30.5f) : (Vector3.up * -4.5f));
		base.transform.localScale = new Vector3((float)(flag ? (-1) : 1), 1f, 1f);
		if (this.lcon.difficulty <= 2)
		{
			this.spawnAuds[0].clip = (flag ? this.swingLowSound : this.swingHighSound);
		}
		if (this.lcon.secondPhase)
		{
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - new Vector3(this.lcon.head.transform.position.x, base.transform.position.y, this.lcon.head.transform.position.z));
		}
		else
		{
			base.transform.rotation = Quaternion.LookRotation(base.transform.position - new Vector3(base.transform.parent.position.x, base.transform.position.y, base.transform.parent.position.z));
		}
		base.gameObject.SetActive(true);
		this.anim.Rebind();
		this.anim.Update(0f);
		if (BlindEnemies.Blind)
		{
			this.idling = true;
			this.anim.speed = 0f;
			return;
		}
		this.anim.speed = this.GetAnimSpeed() * this.lcon.eid.totalSpeedModifier;
		AudioSource[] array = this.spawnAuds;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Play();
		}
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x00074130 File Offset: 0x00072330
	private void BigSplash()
	{
		Object.Instantiate<GameObject>(this.lcon.bigSplash, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), Quaternion.LookRotation(Vector3.up));
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00074190 File Offset: 0x00072390
	private float GetAnimSpeed()
	{
		switch (this.lcon.difficulty)
		{
		case 0:
			return 0.45f;
		case 1:
			return 0.65f;
		case 2:
			return 0.85f;
		case 3:
			return 1f;
		case 4:
			return 1.5f;
		case 5:
			return 2f;
		default:
			return 1f;
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x000741F4 File Offset: 0x000723F4
	public void Death()
	{
		this.SwingEnd();
		if (!this.anim)
		{
			this.anim = base.GetComponent<Animator>();
		}
		this.anim.speed = 1f;
		this.anim.Play("Death", 0, 0f);
	}

	// Token: 0x040014F4 RID: 5364
	private SwingCheck2[] tails;

	// Token: 0x040014F5 RID: 5365
	public Vector3[] spawnPositions;

	// Token: 0x040014F6 RID: 5366
	public Vector3[] secondSpawnPositions;

	// Token: 0x040014F7 RID: 5367
	private int previousSpawnPosition;

	// Token: 0x040014F8 RID: 5368
	private Animator anim;

	// Token: 0x040014F9 RID: 5369
	[HideInInspector]
	public LeviathanController lcon;

	// Token: 0x040014FA RID: 5370
	[SerializeField]
	private AudioSource swingSound;

	// Token: 0x040014FB RID: 5371
	[SerializeField]
	private AudioSource[] spawnAuds;

	// Token: 0x040014FC RID: 5372
	[SerializeField]
	private AudioClip swingHighSound;

	// Token: 0x040014FD RID: 5373
	[SerializeField]
	private AudioClip swingLowSound;

	// Token: 0x040014FE RID: 5374
	private bool idling;
}
