using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000019 RID: 25
[DefaultExecutionOrder(500)]
public class ActivateArena : MonoBehaviour
{
	// Token: 0x060000D3 RID: 211 RVA: 0x00005448 File Offset: 0x00003648
	private void OnEnable()
	{
		if (!this.activated && this.activateOnEnable)
		{
			if (DisableEnemySpawns.DisableArenaTriggers)
			{
				return;
			}
			if (this.waitForStatus > 0)
			{
				if (this.astat == null)
				{
					this.astat = base.GetComponentInParent<ArenaStatus>();
				}
				if (this.astat == null || this.astat.currentStatus < this.waitForStatus)
				{
					return;
				}
			}
			this.Activate();
		}
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x000054B8 File Offset: 0x000036B8
	private void Update()
	{
		if (this.playerIn && this.astat && this.astat.currentStatus >= this.waitForStatus && !this.activated)
		{
			this.Activate();
		}
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000054F0 File Offset: 0x000036F0
	private void OnTriggerEnter(Collider other)
	{
		if (DisableEnemySpawns.DisableArenaTriggers)
		{
			return;
		}
		if (this.waitForStatus > 0)
		{
			if (this.astat == null)
			{
				this.astat = base.GetComponentInParent<ArenaStatus>();
			}
			if (this.astat == null)
			{
				return;
			}
			if (this.astat.currentStatus < this.waitForStatus)
			{
				if ((!this.forEnemy && other.gameObject.CompareTag("Player") && !this.activated) || (this.forEnemy && other.gameObject.CompareTag("Enemy") && !this.activated))
				{
					this.playerIn = true;
				}
				return;
			}
		}
		if ((!this.forEnemy && other.gameObject.CompareTag("Player") && !this.activated) || (this.forEnemy && other.gameObject.CompareTag("Enemy") && !this.activated))
		{
			this.Activate();
		}
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000055E4 File Offset: 0x000037E4
	private void OnTriggerExit(Collider other)
	{
		if ((!this.forEnemy && other.gameObject.CompareTag("Player") && !this.activated) || (this.forEnemy && other.gameObject.CompareTag("Enemy") && !this.activated))
		{
			this.playerIn = false;
		}
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000563C File Offset: 0x0000383C
	public void Activate()
	{
		if (DisableEnemySpawns.DisableArenaTriggers || this.activated)
		{
			return;
		}
		this.activated = true;
		if (!this.onlyWave && !this.forEnemy)
		{
			MonoSingleton<MusicManager>.Instance.ArenaMusicStart();
		}
		if (this.doors.Length != 0)
		{
			foreach (Door door in this.doors)
			{
				if (!door.gameObject.activeSelf)
				{
					door.gameObject.SetActive(true);
				}
				door.Lock();
			}
			if (this.enemies.Length != 0)
			{
				base.Invoke("SpawnEnemy", 1f);
				return;
			}
			Object.Destroy(this);
			return;
		}
		else
		{
			if (this.enemies.Length != 0)
			{
				this.SpawnEnemy();
				return;
			}
			Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000056F4 File Offset: 0x000038F4
	private void SpawnEnemy()
	{
		if (this.currentEnemy >= this.enemies.Length)
		{
			Object.Destroy(this);
			return;
		}
		float num = 0.1f;
		if (this.enemies[this.currentEnemy] != null)
		{
			if (this.enemies[this.currentEnemy].activeSelf)
			{
				num = 0f;
			}
			else
			{
				this.enemies[this.currentEnemy].SetActive(true);
			}
		}
		this.currentEnemy++;
		if (this.currentEnemy < this.enemies.Length)
		{
			base.Invoke("SpawnEnemy", num);
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x04000069 RID: 105
	public bool onlyWave;

	// Token: 0x0400006A RID: 106
	[HideInInspector]
	public bool activated;

	// Token: 0x0400006B RID: 107
	public Door[] doors;

	// Token: 0x0400006C RID: 108
	public GameObject[] enemies;

	// Token: 0x0400006D RID: 109
	private int currentEnemy;

	// Token: 0x0400006E RID: 110
	public bool forEnemy;

	// Token: 0x0400006F RID: 111
	public int waitForStatus;

	// Token: 0x04000070 RID: 112
	public bool activateOnEnable;

	// Token: 0x04000071 RID: 113
	private ArenaStatus astat;

	// Token: 0x04000072 RID: 114
	private bool playerIn;
}
