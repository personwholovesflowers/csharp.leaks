using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062A RID: 1578
	public class Snapping : ICheat
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x0010DC38 File Offset: 0x0010BE38
		public static bool SnappingEnabled
		{
			get
			{
				Snapping lastInstance = Snapping._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x0010DC56 File Offset: 0x0010BE56
		public string LongName
		{
			get
			{
				return "Snapping";
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060023ED RID: 9197 RVA: 0x0010DC5D File Offset: 0x0010BE5D
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.snapping";
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x0010DC64 File Offset: 0x0010BE64
		public string ButtonEnabledOverride { get; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x0010DC6C File Offset: 0x0010BE6C
		public string ButtonDisabledOverride { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x0010DC74 File Offset: 0x0010BE74
		public string Icon
		{
			get
			{
				return "grid";
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x0010DC7B File Offset: 0x0010BE7B
		// (set) Token: 0x060023F2 RID: 9202 RVA: 0x0010DC83 File Offset: 0x0010BE83
		public bool IsActive { get; private set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060023F5 RID: 9205 RVA: 0x0010DC8C File Offset: 0x0010BE8C
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			Snapping._lastInstance = this;
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0010DC9B File Offset: 0x0010BE9B
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E8F RID: 11919
		private static Snapping _lastInstance;
	}
}
