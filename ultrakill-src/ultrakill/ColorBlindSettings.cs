using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class ColorBlindSettings : MonoSingleton<ColorBlindSettings>
{
	// Token: 0x06000420 RID: 1056 RVA: 0x0001CA0C File Offset: 0x0001AC0C
	public void UpdateEnemyColors()
	{
		EnemySimplifier[] array = Object.FindObjectsOfType<EnemySimplifier>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateColors();
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001CA38 File Offset: 0x0001AC38
	public void UpdateHudColors()
	{
		StaminaMeter[] array = Object.FindObjectsOfType<StaminaMeter>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateColors();
		}
		ColorBlindGet[] array2 = Object.FindObjectsOfType<ColorBlindGet>();
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].UpdateColor();
		}
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001CA80 File Offset: 0x0001AC80
	public void UpdateWeaponColors()
	{
		WeaponIcon weaponIcon = Object.FindObjectOfType<WeaponIcon>();
		if (weaponIcon)
		{
			weaponIcon.UpdateIcon();
		}
		MonoSingleton<FistControl>.Instance.UpdateFistIcon();
		ColorBlindGet[] array = Object.FindObjectsOfType<ColorBlindGet>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].UpdateColor();
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001CAC8 File Offset: 0x0001ACC8
	public Color GetEnemyColor(EnemyType ect)
	{
		switch (ect)
		{
		case EnemyType.Cerberus:
			return this.cerberusColor;
		case EnemyType.Drone:
			return this.droneColor;
		case EnemyType.Filth:
			return this.filthColor;
		case EnemyType.MaliciousFace:
			return this.maliciousColor;
		case EnemyType.Mindflayer:
			return this.mindflayerColor;
		case EnemyType.Streetcleaner:
			return this.streetcleanerColor;
		case EnemyType.Swordsmachine:
			return this.swordsmachineColor;
		case EnemyType.V2:
			return this.v2Color;
		case EnemyType.Virtue:
			return this.virtueColor;
		case EnemyType.Stalker:
			return this.stalkerColor;
		case EnemyType.Stray:
			return this.strayColor;
		case EnemyType.Schism:
			return this.schismColor;
		case EnemyType.Soldier:
			return this.shotgunnerColor;
		case EnemyType.Sisyphus:
			return this.sisyphusColor;
		case EnemyType.Turret:
			return this.turretColor;
		case EnemyType.Idol:
			return this.idolColor;
		case EnemyType.Ferryman:
			return this.ferrymanColor;
		case EnemyType.Mannequin:
			return this.mannequinColor;
		case EnemyType.Gutterman:
			return this.guttermanColor;
		case EnemyType.Guttertank:
			return this.guttertankColor;
		}
		return Color.black;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001CC00 File Offset: 0x0001AE00
	public Color GetHudColor(HudColorType hct)
	{
		switch (hct)
		{
		case HudColorType.health:
			return this.healthBarColor;
		case HudColorType.healthAfterImage:
			return this.healthBarAfterImageColor;
		case HudColorType.antiHp:
			return this.antiHpColor;
		case HudColorType.overheal:
			return this.overHealColor;
		case HudColorType.healthText:
			return this.healthBarTextColor;
		case HudColorType.stamina:
			return this.staminaColor;
		case HudColorType.staminaCharging:
			return this.staminaChargingColor;
		case HudColorType.staminaEmpty:
			return this.staminaEmptyColor;
		case HudColorType.railcannonFull:
			return this.railcannonFullColor;
		case HudColorType.railcannonCharging:
			return this.railcannonChargingColor;
		default:
			return Color.white;
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001CC88 File Offset: 0x0001AE88
	public void SetEnemyColor(EnemyType ect, Color color)
	{
		switch (ect)
		{
		case EnemyType.Cerberus:
			this.cerberusColor = color;
			break;
		case EnemyType.Drone:
			this.droneColor = color;
			break;
		case EnemyType.Filth:
			this.filthColor = color;
			break;
		case EnemyType.MaliciousFace:
			this.maliciousColor = color;
			break;
		case EnemyType.Mindflayer:
			this.mindflayerColor = color;
			break;
		case EnemyType.Streetcleaner:
			this.streetcleanerColor = color;
			break;
		case EnemyType.Swordsmachine:
			this.swordsmachineColor = color;
			break;
		case EnemyType.V2:
			this.v2Color = color;
			break;
		case EnemyType.Virtue:
			this.virtueColor = color;
			break;
		case EnemyType.Stalker:
			this.stalkerColor = color;
			break;
		case EnemyType.Stray:
			this.strayColor = color;
			break;
		case EnemyType.Schism:
			this.schismColor = color;
			break;
		case EnemyType.Soldier:
			this.shotgunnerColor = color;
			break;
		case EnemyType.Sisyphus:
			this.sisyphusColor = color;
			break;
		case EnemyType.Turret:
			this.turretColor = color;
			break;
		case EnemyType.Idol:
			this.idolColor = color;
			break;
		case EnemyType.Ferryman:
			this.ferrymanColor = color;
			break;
		case EnemyType.Mannequin:
			this.mannequinColor = color;
			break;
		case EnemyType.Gutterman:
			this.guttermanColor = color;
			break;
		case EnemyType.Guttertank:
			this.guttertankColor = color;
			break;
		}
		this.UpdateEnemyColors();
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001CDF4 File Offset: 0x0001AFF4
	public void SetHudColor(HudColorType hct, Color color)
	{
		switch (hct)
		{
		case HudColorType.health:
			this.healthBarColor = color;
			break;
		case HudColorType.healthAfterImage:
			this.healthBarAfterImageColor = color;
			break;
		case HudColorType.antiHp:
			this.antiHpColor = color;
			break;
		case HudColorType.overheal:
			this.overHealColor = color;
			break;
		case HudColorType.healthText:
			this.healthBarTextColor = color;
			break;
		case HudColorType.stamina:
			this.staminaColor = color;
			break;
		case HudColorType.staminaCharging:
			this.staminaChargingColor = color;
			break;
		case HudColorType.staminaEmpty:
			this.staminaEmptyColor = color;
			break;
		case HudColorType.railcannonFull:
			this.railcannonFullColor = color;
			break;
		case HudColorType.railcannonCharging:
			this.railcannonChargingColor = color;
			break;
		}
		this.UpdateHudColors();
	}

	// Token: 0x0400051A RID: 1306
	public Color[] variationColors;

	// Token: 0x0400051B RID: 1307
	[Header("HUD Colors")]
	public Color healthBarColor;

	// Token: 0x0400051C RID: 1308
	public Color healthBarAfterImageColor;

	// Token: 0x0400051D RID: 1309
	public Color antiHpColor;

	// Token: 0x0400051E RID: 1310
	public Color overHealColor;

	// Token: 0x0400051F RID: 1311
	public Color healthBarTextColor;

	// Token: 0x04000520 RID: 1312
	public Color staminaColor;

	// Token: 0x04000521 RID: 1313
	public Color staminaChargingColor;

	// Token: 0x04000522 RID: 1314
	public Color staminaEmptyColor;

	// Token: 0x04000523 RID: 1315
	public Color railcannonFullColor;

	// Token: 0x04000524 RID: 1316
	public Color railcannonChargingColor;

	// Token: 0x04000525 RID: 1317
	[Header("Enemy Colors")]
	public Color filthColor;

	// Token: 0x04000526 RID: 1318
	public Color strayColor;

	// Token: 0x04000527 RID: 1319
	public Color schismColor;

	// Token: 0x04000528 RID: 1320
	public Color shotgunnerColor;

	// Token: 0x04000529 RID: 1321
	public Color stalkerColor;

	// Token: 0x0400052A RID: 1322
	public Color sisyphusColor;

	// Token: 0x0400052B RID: 1323
	public Color ferrymanColor;

	// Token: 0x0400052C RID: 1324
	public Color droneColor;

	// Token: 0x0400052D RID: 1325
	public Color streetcleanerColor;

	// Token: 0x0400052E RID: 1326
	public Color swordsmachineColor;

	// Token: 0x0400052F RID: 1327
	public Color mindflayerColor;

	// Token: 0x04000530 RID: 1328
	public Color v2Color;

	// Token: 0x04000531 RID: 1329
	public Color turretColor;

	// Token: 0x04000532 RID: 1330
	public Color guttermanColor;

	// Token: 0x04000533 RID: 1331
	public Color guttertankColor;

	// Token: 0x04000534 RID: 1332
	public Color maliciousColor;

	// Token: 0x04000535 RID: 1333
	public Color cerberusColor;

	// Token: 0x04000536 RID: 1334
	public Color idolColor;

	// Token: 0x04000537 RID: 1335
	public Color mannequinColor;

	// Token: 0x04000538 RID: 1336
	public Color virtueColor;

	// Token: 0x04000539 RID: 1337
	public Color enrageColor;
}
