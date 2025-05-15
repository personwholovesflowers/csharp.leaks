using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019A RID: 410
public class EnemySpawnRadius : MonoBehaviour
{
	// Token: 0x06000842 RID: 2114 RVA: 0x0003955C File Offset: 0x0003775C
	private void Start()
	{
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		base.Invoke("SlowUpdate", 1f);
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00039580 File Offset: 0x00037780
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 1f);
		if (this.currentEnemies == null || this.currentEnemies.Count == 0)
		{
			return;
		}
		for (int i = this.currentEnemies.Count - 1; i >= 0; i--)
		{
			if (this.spawnedObjects[i] == null || this.currentEnemies[i].dead)
			{
				this.spawnedObjects.RemoveAt(i);
				this.currentEnemies.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0003960C File Offset: 0x0003780C
	private void Update()
	{
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		if (this.cooldown <= 0f)
		{
			if (this.currentEnemies.Count < this.maximumEnemyCount)
			{
				this.SpawnEnemy();
				return;
			}
			this.cooldown = 2f;
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00039668 File Offset: 0x00037868
	public void SpawnEnemy()
	{
		for (int i = 0; i < 3; i++)
		{
			Vector3 normalized = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position + normalized * Random.Range(this.minimumDistance, this.maximumDistance), Vector3.down, out raycastHit, 25f, LayerMaskDefaults.Get(LMD.Environment)))
			{
				this.cooldown = this.spawnCooldown;
				GameObject gameObject = Object.Instantiate<GameObject>(this.spawnables[Random.Range(0, this.spawnables.Length)], raycastHit.point, Quaternion.identity);
				gameObject.transform.SetParent(this.gz.transform, true);
				this.spawnedObjects.Add(gameObject);
				EnemyIdentifier componentInChildren = gameObject.GetComponentInChildren<EnemyIdentifier>();
				if (componentInChildren)
				{
					this.currentEnemies.Add(componentInChildren);
					if (this.spawnAsPuppets)
					{
						componentInChildren.puppet = true;
					}
				}
				else
				{
					this.currentEnemies.Add(null);
				}
				gameObject.SetActive(true);
				return;
			}
		}
		this.cooldown = 1f;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x000397AC File Offset: 0x000379AC
	public void KillAllEnemies()
	{
		for (int i = this.currentEnemies.Count - 1; i >= 0; i--)
		{
			if (this.currentEnemies[i] != null)
			{
				this.currentEnemies[i].InstaKill();
				this.spawnedObjects.RemoveAt(i);
				this.currentEnemies.RemoveAt(i);
			}
		}
	}

	// Token: 0x04000B1B RID: 2843
	public GameObject[] spawnables;

	// Token: 0x04000B1C RID: 2844
	private List<GameObject> spawnedObjects = new List<GameObject>();

	// Token: 0x04000B1D RID: 2845
	private List<EnemyIdentifier> currentEnemies = new List<EnemyIdentifier>();

	// Token: 0x04000B1E RID: 2846
	public float minimumDistance;

	// Token: 0x04000B1F RID: 2847
	public float maximumDistance;

	// Token: 0x04000B20 RID: 2848
	public float spawnCooldown;

	// Token: 0x04000B21 RID: 2849
	private float cooldown;

	// Token: 0x04000B22 RID: 2850
	public int maximumEnemyCount;

	// Token: 0x04000B23 RID: 2851
	public bool spawnAsPuppets = true;

	// Token: 0x04000B24 RID: 2852
	private GoreZone gz;
}
