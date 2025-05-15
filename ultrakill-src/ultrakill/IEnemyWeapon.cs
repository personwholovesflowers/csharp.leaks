using System;

// Token: 0x02000263 RID: 611
public interface IEnemyWeapon
{
	// Token: 0x06000D7A RID: 3450
	void UpdateTarget(EnemyTarget target);

	// Token: 0x06000D7B RID: 3451
	void Fire();

	// Token: 0x06000D7C RID: 3452
	void AltFire();

	// Token: 0x06000D7D RID: 3453
	void CancelAltCharge();
}
