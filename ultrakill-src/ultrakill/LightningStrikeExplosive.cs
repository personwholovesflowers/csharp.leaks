using System;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class LightningStrikeExplosive : MonoBehaviour
{
	// Token: 0x06000FB2 RID: 4018 RVA: 0x00074D4C File Offset: 0x00072F4C
	private void Start()
	{
		GameObject gameObject = null;
		bool flag = false;
		if (MonoSingleton<CoinList>.Instance && MonoSingleton<CoinList>.Instance.revolverCoinsList.Count > 0)
		{
			int i = 0;
			while (i < MonoSingleton<CoinList>.Instance.revolverCoinsList.Count)
			{
				if (MonoSingleton<CoinList>.Instance.revolverCoinsList[i].transform.position.y > MonoSingleton<PlayerTracker>.Instance.GetPlayer().position.y && Vector2.Distance(new Vector2(MonoSingleton<CoinList>.Instance.revolverCoinsList[i].transform.position.x, MonoSingleton<CoinList>.Instance.revolverCoinsList[i].transform.position.z), new Vector2(base.transform.position.x, base.transform.position.z)) < 2f)
				{
					flag = true;
					gameObject = Object.Instantiate<GameObject>(this.reflected, base.transform.position + Vector3.up * 100f, Quaternion.LookRotation(Vector3.down));
					RevolverBeam revolverBeam;
					if (this.damageMultiplier != 1f && gameObject.TryGetComponent<RevolverBeam>(out revolverBeam))
					{
						revolverBeam.damage *= this.damageMultiplier;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		if (!flag)
		{
			gameObject = Object.Instantiate<GameObject>(this.normal, base.transform.position, Quaternion.identity);
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				if (this.damageMultiplier != 1f || this.enemyDamageMultiplier != 1f)
				{
					explosion.damage = Mathf.RoundToInt((float)explosion.damage * this.damageMultiplier);
					explosion.enemyDamageMultiplier *= this.enemyDamageMultiplier;
					explosion.maxSize *= this.damageMultiplier;
					explosion.speed *= this.damageMultiplier;
				}
				if (this.safeForPlayer)
				{
					explosion.canHit = AffectedSubjects.EnemiesOnly;
				}
			}
		}
		if (base.transform.parent)
		{
			gameObject.transform.SetParent(base.transform.parent, true);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400152B RID: 5419
	public GameObject normal;

	// Token: 0x0400152C RID: 5420
	public GameObject reflected;

	// Token: 0x0400152D RID: 5421
	public bool safeForPlayer;

	// Token: 0x0400152E RID: 5422
	public float damageMultiplier = 1f;

	// Token: 0x0400152F RID: 5423
	public float enemyDamageMultiplier = 1f;
}
