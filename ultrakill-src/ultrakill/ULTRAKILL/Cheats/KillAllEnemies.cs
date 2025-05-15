using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200061E RID: 1566
	public class KillAllEnemies : ICheat
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x0010D535 File Offset: 0x0010B735
		public string LongName
		{
			get
			{
				return "Kill All Enemies";
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06002365 RID: 9061 RVA: 0x0010D53C File Offset: 0x0010B73C
		public string Identifier
		{
			get
			{
				return "ultrakill.kill-all-enemies";
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002367 RID: 9063 RVA: 0x0010D543 File Offset: 0x0010B743
		public string ButtonDisabledOverride
		{
			get
			{
				return "Kill All";
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x0010D54A File Offset: 0x0010B74A
		public string Icon
		{
			get
			{
				return "death";
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06002369 RID: 9065 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool IsActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x0600236C RID: 9068 RVA: 0x0010D554 File Offset: 0x0010B754
		public void Enable(CheatsManager manager)
		{
			foreach (EnemyIdentifier enemyIdentifier in MonoSingleton<EnemyTracker>.Instance.GetCurrentEnemies())
			{
				enemyIdentifier.InstaKill();
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Disable()
		{
		}
	}
}
