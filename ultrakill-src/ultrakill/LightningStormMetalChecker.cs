using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CE RID: 718
public class LightningStormMetalChecker : MonoBehaviour
{
	// Token: 0x06000FA5 RID: 4005 RVA: 0x000743C6 File Offset: 0x000725C6
	private void Start()
	{
		base.Invoke("Check", Random.Range(this.frequencyMinimum / 3f, this.frequencyMaximum / 3f));
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x000743F0 File Offset: 0x000725F0
	private void Check()
	{
		List<EnemyIdentifier> currentEnemies = MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies();
		if (currentEnemies.Count > 0)
		{
			for (int i = 0; i < currentEnemies.Count; i++)
			{
				if (currentEnemies[i].enemyType != EnemyType.Ferryman && (currentEnemies[i].nailsAmount >= 40 || currentEnemies[i].stuckMagnets.Count > 0) && OutdoorsChecker.CheckIfPositionOutdoors(currentEnemies[i].transform.position + Vector3.up * 0.25f))
				{
					Transform positionFromEnemy = this.GetPositionFromEnemy(currentEnemies[i]);
					GameObject gameObject = Object.Instantiate<GameObject>(this.boltWarning, positionFromEnemy.position, Quaternion.identity);
					Follow follow = gameObject.AddComponent<Follow>();
					follow.target = positionFromEnemy;
					follow.destroyIfNoTarget = true;
					this.boltWarnings.Add(gameObject);
				}
			}
		}
		Harpoon[] array = Object.FindObjectsOfType<Harpoon>();
		if (array != null && array.Length != 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].drill)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.boltWarning, array[j].transform.position, Quaternion.identity);
					Follow follow2 = gameObject2.AddComponent<Follow>();
					follow2.target = array[j].transform;
					follow2.destroyIfNoTarget = true;
					this.boltWarnings.Add(gameObject2);
				}
			}
		}
		if (this.boltWarnings.Count > 0)
		{
			base.Invoke("Check", Random.Range(this.frequencyMinimum, this.frequencyMaximum));
			base.Invoke("SummonLightning", 3f);
			return;
		}
		base.Invoke("Check", Random.Range(this.frequencyMinimum / 3f, this.frequencyMaximum / 3f));
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x000745AC File Offset: 0x000727AC
	private void SummonLightning()
	{
		if (this.boltWarnings != null && this.boltWarnings.Count > 0)
		{
			for (int i = this.boltWarnings.Count - 1; i >= 0; i--)
			{
				if (this.boltWarnings[i] != null)
				{
					if (OutdoorsChecker.CheckIfPositionOutdoors(this.boltWarnings[i].transform.position))
					{
						GameObject gameObject = Object.Instantiate<GameObject>(this.lightningBolt, this.boltWarnings[i].transform.position, Quaternion.identity);
						LightningStrikeExplosive lightningStrikeExplosive;
						if ((this.damageMultiplier != 1f || this.enemyDamageMultiplier != 1f) && gameObject.TryGetComponent<LightningStrikeExplosive>(out lightningStrikeExplosive))
						{
							lightningStrikeExplosive.damageMultiplier = this.damageMultiplier;
							lightningStrikeExplosive.enemyDamageMultiplier = this.enemyDamageMultiplier;
						}
					}
					Object.Destroy(this.boltWarnings[i]);
				}
			}
		}
		this.boltWarnings.Clear();
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x000746A8 File Offset: 0x000728A8
	private Transform GetPositionFromEnemy(EnemyIdentifier eid)
	{
		if (eid.stuckMagnets.Count > 0)
		{
			for (int i = 0; i < eid.stuckMagnets.Count; i++)
			{
				if (eid.stuckMagnets[i] != null)
				{
					return eid.stuckMagnets[i].transform;
				}
			}
		}
		if (eid.nails.Count > 0)
		{
			for (int j = 0; j < eid.nails.Count; j++)
			{
				if (eid.nails[j] != null)
				{
					return eid.nails[j].transform;
				}
			}
		}
		return eid.transform;
	}

	// Token: 0x04001507 RID: 5383
	public GameObject lightningBolt;

	// Token: 0x04001508 RID: 5384
	public GameObject boltWarning;

	// Token: 0x04001509 RID: 5385
	private List<GameObject> boltWarnings = new List<GameObject>();

	// Token: 0x0400150A RID: 5386
	public float frequencyMinimum = 10f;

	// Token: 0x0400150B RID: 5387
	public float frequencyMaximum = 20f;

	// Token: 0x0400150C RID: 5388
	public float damageMultiplier = 1f;

	// Token: 0x0400150D RID: 5389
	public float enemyDamageMultiplier = 1f;
}
