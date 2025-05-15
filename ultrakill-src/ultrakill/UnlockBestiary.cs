using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class UnlockBestiary : MonoBehaviour
{
	// Token: 0x06001AE2 RID: 6882 RVA: 0x000DD0ED File Offset: 0x000DB2ED
	private void Start()
	{
		MonoSingleton<BestiaryData>.Instance.SetEnemy(this.enemy, this.fullUnlock ? 2 : 1);
	}

	// Token: 0x040025D9 RID: 9689
	public EnemyType enemy;

	// Token: 0x040025DA RID: 9690
	public bool fullUnlock;
}
