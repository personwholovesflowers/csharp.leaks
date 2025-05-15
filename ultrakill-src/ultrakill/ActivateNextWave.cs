using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class ActivateNextWave : MonoBehaviour
{
	// Token: 0x060000DA RID: 218 RVA: 0x00005793 File Offset: 0x00003993
	public void CountEnemies()
	{
		this.enemyCount = base.transform.childCount;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x000057A6 File Offset: 0x000039A6
	private void Awake()
	{
		this.linkedAnws = base.GetComponents<ActivateNextWave>();
	}

	// Token: 0x060000DC RID: 220 RVA: 0x000057B4 File Offset: 0x000039B4
	private void FixedUpdate()
	{
		if (this.deadEnemies < 0)
		{
			this.deadEnemies = 0;
		}
		if (!this.activated && this.deadEnemies >= this.enemyCount)
		{
			this.activated = true;
			if (!this.lastWave)
			{
				if (this.toActivate.Length != 0)
				{
					foreach (GameObject gameObject in this.toActivate)
					{
						if (gameObject != null)
						{
							gameObject.SetActive(true);
						}
					}
				}
				if (this.doors.Length != 0)
				{
					foreach (Door door in this.doors)
					{
						if (door != null)
						{
							door.Unlock();
						}
					}
				}
				base.Invoke("SpawnEnemy", (float)(this.noActivationDelay ? 0 : 1));
				return;
			}
			base.Invoke("EndWaves", (float)(this.noActivationDelay ? 0 : 1));
			if (!this.forEnemies)
			{
				MonoSingleton<TimeController>.Instance.SlowDown(0.15f);
			}
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x000058AC File Offset: 0x00003AAC
	private void SpawnEnemy()
	{
		if (this.nextEnemies.Length != 0)
		{
			if (this.nextEnemies[this.currentEnemy] != null)
			{
				this.nextEnemies[this.currentEnemy].SetActive(true);
			}
			this.currentEnemy++;
		}
		if (this.currentEnemy < this.nextEnemies.Length)
		{
			base.Invoke("SpawnEnemy", 0.1f);
			return;
		}
		Object.Destroy(this);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00005920 File Offset: 0x00003B20
	private void EndWaves()
	{
		if (this.toActivate.Length != 0 && !this.objectsActivated)
		{
			foreach (GameObject gameObject in this.toActivate)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			this.objectsActivated = true;
			this.EndWaves();
			return;
		}
		if (this.currentDoor < this.doors.Length)
		{
			this.doors[this.currentDoor].Unlock();
			if (this.doors[this.currentDoor] == this.doorForward)
			{
				this.doors[this.currentDoor].Open(false, true);
			}
			this.currentDoor++;
			base.Invoke("EndWaves", 0.1f);
			return;
		}
		if (!this.forEnemies)
		{
			MonoSingleton<MusicManager>.Instance.ArenaMusicEnd();
			this.slowDown = 1f;
		}
		if (this.killChallenge)
		{
			MonoSingleton<ChallengeManager>.Instance.ChallengeDone();
		}
		Object.Destroy(this);
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005A1C File Offset: 0x00003C1C
	public void AddDeadEnemy()
	{
		this.deadEnemies++;
		if (this.linkedAnws.Length > 1)
		{
			foreach (ActivateNextWave activateNextWave in this.linkedAnws)
			{
				if (activateNextWave != this)
				{
					activateNextWave.deadEnemies++;
				}
			}
		}
	}

	// Token: 0x04000073 RID: 115
	public bool lastWave;

	// Token: 0x04000074 RID: 116
	[HideInInspector]
	public bool activated;

	// Token: 0x04000075 RID: 117
	public int deadEnemies;

	// Token: 0x04000076 RID: 118
	public int enemyCount;

	// Token: 0x04000077 RID: 119
	private ActivateNextWave[] linkedAnws;

	// Token: 0x04000078 RID: 120
	public GameObject[] nextEnemies;

	// Token: 0x04000079 RID: 121
	private int currentEnemy;

	// Token: 0x0400007A RID: 122
	public Door[] doors;

	// Token: 0x0400007B RID: 123
	private int currentDoor;

	// Token: 0x0400007C RID: 124
	public GameObject[] toActivate;

	// Token: 0x0400007D RID: 125
	private bool objectsActivated;

	// Token: 0x0400007E RID: 126
	public Door doorForward;

	// Token: 0x0400007F RID: 127
	private float slowDown = 1f;

	// Token: 0x04000080 RID: 128
	public bool forEnemies;

	// Token: 0x04000081 RID: 129
	public bool killChallenge;

	// Token: 0x04000082 RID: 130
	public bool noActivationDelay;
}
