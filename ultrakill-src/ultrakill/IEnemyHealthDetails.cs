using System;

// Token: 0x02000261 RID: 609
public interface IEnemyHealthDetails
{
	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000D73 RID: 3443
	string FullName { get; }

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000D74 RID: 3444
	float Health { get; }

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000D75 RID: 3445
	bool Dead { get; }

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x06000D76 RID: 3446
	bool Blessed { get; }

	// Token: 0x06000D77 RID: 3447
	void ForceGetHealth();
}
