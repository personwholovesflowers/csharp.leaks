using System;
using UnityEngine;

// Token: 0x0200017A RID: 378
[CreateAssetMenu(menuName = "ULTRAKILL/Endless Enemy Data")]
public class EndlessEnemy : ScriptableObject
{
	// Token: 0x04000958 RID: 2392
	public EnemyType enemyType;

	// Token: 0x04000959 RID: 2393
	public GameObject prefab;

	// Token: 0x0400095A RID: 2394
	public int spawnCost;

	// Token: 0x0400095B RID: 2395
	public int spawnWave;

	// Token: 0x0400095C RID: 2396
	public int costIncreasePerSpawn;
}
