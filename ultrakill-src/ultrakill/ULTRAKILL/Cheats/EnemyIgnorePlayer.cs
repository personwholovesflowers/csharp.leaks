using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000612 RID: 1554
	public class EnemyIgnorePlayer : ICheat
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x0010CDEC File Offset: 0x0010AFEC
		public static bool Active
		{
			get
			{
				EnemyIgnorePlayer lastInstance = EnemyIgnorePlayer._lastInstance;
				return lastInstance != null && lastInstance.active;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060022E0 RID: 8928 RVA: 0x0010CE0A File Offset: 0x0010B00A
		public string LongName
		{
			get
			{
				return "Enemies Ignore Player";
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x0010CE11 File Offset: 0x0010B011
		public string Identifier
		{
			get
			{
				return "ultrakill.enemy-ignore-player";
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x0010CE18 File Offset: 0x0010B018
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x0010CE20 File Offset: 0x0010B020
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x0010CE28 File Offset: 0x0010B028
		public string Icon
		{
			get
			{
				return "enemy-ignore-player";
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x0010CE2F File Offset: 0x0010B02F
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x0010CE37 File Offset: 0x0010B037
		public bool DefaultState { get; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x0010CE3F File Offset: 0x0010B03F
		public void Enable(CheatsManager manager)
		{
			this.active = true;
			EnemyIgnorePlayer._lastInstance = this;
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x0010CE4E File Offset: 0x0010B04E
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x04002E43 RID: 11843
		private static EnemyIgnorePlayer _lastInstance;

		// Token: 0x04002E47 RID: 11847
		private bool active;
	}
}
