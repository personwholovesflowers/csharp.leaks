using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000606 RID: 1542
	public class EnemyIdentifierDebug : ICheat
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x0010C404 File Offset: 0x0010A604
		public static bool Active
		{
			get
			{
				EnemyIdentifierDebug lastInstance = EnemyIdentifierDebug._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600224A RID: 8778 RVA: 0x0010C422 File Offset: 0x0010A622
		public string LongName
		{
			get
			{
				return "Enemy Identifier Debug";
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x0010C429 File Offset: 0x0010A629
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.enemy-identifier";
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600224C RID: 8780 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x0010C430 File Offset: 0x0010A630
		// (set) Token: 0x06002252 RID: 8786 RVA: 0x0010C438 File Offset: 0x0010A638
		public bool IsActive { get; private set; }

		// Token: 0x06002253 RID: 8787 RVA: 0x0010C441 File Offset: 0x0010A641
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			EnemyIdentifierDebug._lastInstance = this;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x0010C450 File Offset: 0x0010A650
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E0D RID: 11789
		private static EnemyIdentifierDebug _lastInstance;
	}
}
