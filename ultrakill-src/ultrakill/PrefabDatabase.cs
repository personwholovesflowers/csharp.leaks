using System;
using UnityEngine;

// Token: 0x02000181 RID: 385
[CreateAssetMenu(fileName = "Prefab Database", menuName = "ULTRAKILL/Prefab Database")]
public class PrefabDatabase : ScriptableObject
{
	// Token: 0x040009AD RID: 2477
	[Header("Enemies")]
	public EndlessEnemy[] meleeEnemies;

	// Token: 0x040009AE RID: 2478
	public EndlessEnemy[] projectileEnemies;

	// Token: 0x040009AF RID: 2479
	public EndlessEnemy[] uncommonEnemies;

	// Token: 0x040009B0 RID: 2480
	public EndlessEnemy[] specialEnemies;

	// Token: 0x040009B1 RID: 2481
	[Header("Other Prefabs")]
	public GameObject jumpPad;

	// Token: 0x040009B2 RID: 2482
	public GameObject stairs;

	// Token: 0x040009B3 RID: 2483
	public GameObject hideousMass;
}
