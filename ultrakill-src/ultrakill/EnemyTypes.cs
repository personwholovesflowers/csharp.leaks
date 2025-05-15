using System;
using System.Collections.Generic;

// Token: 0x0200018E RID: 398
public static class EnemyTypes
{
	// Token: 0x060007A7 RID: 1959 RVA: 0x00032D88 File Offset: 0x00030F88
	public static string GetEnemyName(EnemyType type)
	{
		switch (type)
		{
		case EnemyType.Gabriel:
			return "Gabriel, Judge of Hell";
		case EnemyType.FleshPrison:
		case EnemyType.MinosPrime:
		case EnemyType.Idol:
			break;
		case EnemyType.Sisyphus:
			return "Sisyphean Insurrectionist";
		case EnemyType.Turret:
			return "Sentry";
		case EnemyType.V2Second:
			return "V2";
		default:
			if (type == EnemyType.Mandalore)
			{
				return "Mysterious Druid Knight (& Owl)";
			}
			if (type == EnemyType.GabrielSecond)
			{
				return "Gabriel, Apostate of Hate";
			}
			break;
		}
		return Enum.GetName(typeof(EnemyType), type);
	}

	// Token: 0x04000A1D RID: 2589
	public static HashSet<Type> types = new HashSet<Type>
	{
		typeof(Zombie),
		typeof(ZombieMelee),
		typeof(Stalker),
		typeof(Statue),
		typeof(StatueBoss),
		typeof(Mass),
		typeof(Drone),
		typeof(DroneFlesh),
		typeof(Machine),
		typeof(V2),
		typeof(SpiderBody),
		typeof(Gutterman),
		typeof(Guttertank),
		typeof(Sisyphus),
		typeof(MortarLauncher)
	};
}
