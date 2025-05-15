using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class ActivateNextWaveHP : MonoBehaviour
{
	// Token: 0x060000E1 RID: 225 RVA: 0x00005A88 File Offset: 0x00003C88
	private void Update()
	{
		if (!this.activated && (!this.target || (this.target.gameObject.activeInHierarchy && this.target.health <= this.health) || (this.health <= 0f && this.target.dead)))
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
				base.Invoke("SpawnEnemy", 1f);
				return;
			}
			base.Invoke("EndWaves", 1f);
			if (!this.forEnemies)
			{
				MonoSingleton<TimeController>.Instance.SlowDown(0.15f);
			}
		}
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005B68 File Offset: 0x00003D68
	private void SpawnEnemy()
	{
		if (this.nextEnemies.Length != 0 && this.nextEnemies.Length > this.currentEnemy)
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

	// Token: 0x060000E3 RID: 227 RVA: 0x00005BEC File Offset: 0x00003DEC
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
			MonoSingleton<AudioMixerController>.Instance.allSound.SetFloat("allPitch", 1f);
			MonoSingleton<AudioMixerController>.Instance.doorSound.SetFloat("allPitch", 1f);
		}
		Object.Destroy(this);
	}

	// Token: 0x04000083 RID: 131
	public bool lastWave;

	// Token: 0x04000084 RID: 132
	private bool activated;

	// Token: 0x04000085 RID: 133
	public EnemyIdentifier target;

	// Token: 0x04000086 RID: 134
	public float health;

	// Token: 0x04000087 RID: 135
	public GameObject[] nextEnemies;

	// Token: 0x04000088 RID: 136
	private int currentEnemy;

	// Token: 0x04000089 RID: 137
	public Door[] doors;

	// Token: 0x0400008A RID: 138
	private int currentDoor;

	// Token: 0x0400008B RID: 139
	public GameObject[] toActivate;

	// Token: 0x0400008C RID: 140
	private bool objectsActivated;

	// Token: 0x0400008D RID: 141
	public Door doorForward;

	// Token: 0x0400008E RID: 142
	public bool forEnemies;
}
