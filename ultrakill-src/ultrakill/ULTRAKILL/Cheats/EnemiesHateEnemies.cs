using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000611 RID: 1553
	public class EnemiesHateEnemies : ICheat
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x0010CD78 File Offset: 0x0010AF78
		public static bool Active
		{
			get
			{
				EnemiesHateEnemies lastInstance = EnemiesHateEnemies._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x0010CD96 File Offset: 0x0010AF96
		public string LongName
		{
			get
			{
				return "Enemies Attack Each Other";
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060022D4 RID: 8916 RVA: 0x0010CD9D File Offset: 0x0010AF9D
		public string Identifier
		{
			get
			{
				return "ultrakill.enemy-hate-enemy";
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x0010CDA4 File Offset: 0x0010AFA4
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x0010CDAC File Offset: 0x0010AFAC
		public string ButtonDisabledOverride { get; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x0010CDB4 File Offset: 0x0010AFB4
		public string Icon
		{
			get
			{
				return "enemy-hate-enemy";
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060022D8 RID: 8920 RVA: 0x0010CDBB File Offset: 0x0010AFBB
		// (set) Token: 0x060022D9 RID: 8921 RVA: 0x0010CDC3 File Offset: 0x0010AFC3
		public bool IsActive { get; private set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0010CDCC File Offset: 0x0010AFCC
		public bool DefaultState { get; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x0010CDD4 File Offset: 0x0010AFD4
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			EnemiesHateEnemies._lastInstance = this;
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x0010CDE3 File Offset: 0x0010AFE3
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E3E RID: 11838
		private static EnemiesHateEnemies _lastInstance;
	}
}
