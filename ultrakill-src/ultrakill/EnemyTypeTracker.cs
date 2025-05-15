using System;

// Token: 0x0200017B RID: 379
public class EnemyTypeTracker
{
	// Token: 0x0600074D RID: 1869 RVA: 0x0002F38C File Offset: 0x0002D58C
	public EnemyTypeTracker(EnemyType enemyType)
	{
		this.type = enemyType;
	}

	// Token: 0x0400095D RID: 2397
	public EnemyType type;

	// Token: 0x0400095E RID: 2398
	public int amount;
}
