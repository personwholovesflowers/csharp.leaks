using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000610 RID: 1552
	public class DisableEnemySpawns : ICheat
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x0010CD04 File Offset: 0x0010AF04
		public static bool DisableArenaTriggers
		{
			get
			{
				DisableEnemySpawns lastInstance = DisableEnemySpawns._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x0010CD22 File Offset: 0x0010AF22
		public string LongName
		{
			get
			{
				return "Disable Enemy Spawns";
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x0010CD29 File Offset: 0x0010AF29
		public string Identifier
		{
			get
			{
				return "ultrakill.disable-enemy-spawns";
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060022C8 RID: 8904 RVA: 0x0010CD30 File Offset: 0x0010AF30
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x0010CD38 File Offset: 0x0010AF38
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060022CA RID: 8906 RVA: 0x0010CD40 File Offset: 0x0010AF40
		public string Icon
		{
			get
			{
				return "no-enemies";
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0010CD47 File Offset: 0x0010AF47
		// (set) Token: 0x060022CC RID: 8908 RVA: 0x0010CD4F File Offset: 0x0010AF4F
		public bool IsActive { get; private set; }

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x0010CD58 File Offset: 0x0010AF58
		public bool DefaultState { get; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x0010CD60 File Offset: 0x0010AF60
		public void Enable(CheatsManager manager)
		{
			DisableEnemySpawns._lastInstance = this;
			this.IsActive = true;
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x0010CD6F File Offset: 0x0010AF6F
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E39 RID: 11833
		private static DisableEnemySpawns _lastInstance;
	}
}
