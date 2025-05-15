using System;
using UnityEngine;

// Token: 0x020002DC RID: 732
public class MadnessGas : MonoBehaviour
{
	// Token: 0x06000FE5 RID: 4069 RVA: 0x00078DDC File Offset: 0x00076FDC
	private void OnTriggerEnter(Collider col)
	{
		EnemyIdentifier enemyIdentifier;
		if (col.gameObject.layer == 12 && col.TryGetComponent<EnemyIdentifier>(out enemyIdentifier) && (!enemyIdentifier.IgnorePlayer || !enemyIdentifier.AttackEnemies))
		{
			enemyIdentifier.madness = true;
			Transform transform = (enemyIdentifier.weakPoint ? enemyIdentifier.weakPoint.transform : ((enemyIdentifier.overrideCenter != null) ? enemyIdentifier.overrideCenter : enemyIdentifier.transform));
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.madnessEffect, transform.position, Quaternion.identity);
			gameObject.transform.SetParent(transform, true);
			enemyIdentifier.destroyOnDeath.Add(gameObject);
		}
	}
}
