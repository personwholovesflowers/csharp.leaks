using System;
using System.Linq;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class CombinedBossBar : MonoBehaviour, IEnemyHealthDetails
{
	// Token: 0x0600042C RID: 1068 RVA: 0x0001CF14 File Offset: 0x0001B114
	private void OnEnable()
	{
		BossHealthBar bossHealthBar;
		if (!base.TryGetComponent<BossHealthBar>(out bossHealthBar))
		{
			base.gameObject.AddComponent<BossHealthBar>();
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001CF37 File Offset: 0x0001B137
	public string FullName
	{
		get
		{
			return this.fullName;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001CF3F File Offset: 0x0001B13F
	public float Health
	{
		get
		{
			return this.enemies.Sum(delegate(EnemyIdentifier x)
			{
				if (!(x == null))
				{
					return Mathf.Max(0f, x.Health);
				}
				return 0f;
			});
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600042F RID: 1071 RVA: 0x0001CF6B File Offset: 0x0001B16B
	public bool Dead
	{
		get
		{
			return this.enemies.All((EnemyIdentifier x) => x == null || x.dead);
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000430 RID: 1072 RVA: 0x0001CF97 File Offset: 0x0001B197
	public bool Blessed
	{
		get
		{
			return this.enemies.All((EnemyIdentifier x) => x == null || x.blessed);
		}
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001CFC4 File Offset: 0x0001B1C4
	public void ForceGetHealth()
	{
		if (this.enemies == null || this.enemies.Length == 0)
		{
			return;
		}
		EnemyIdentifier[] array = this.enemies;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ForceGetHealth();
		}
	}

	// Token: 0x04000541 RID: 1345
	public string fullName;

	// Token: 0x04000542 RID: 1346
	public EnemyIdentifier[] enemies;
}
