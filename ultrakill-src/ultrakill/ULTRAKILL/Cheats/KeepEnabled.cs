using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200061D RID: 1565
	public class KeepEnabled : ICheat
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x0010D4EF File Offset: 0x0010B6EF
		public string LongName
		{
			get
			{
				return "Keep Cheats Enabled";
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x0010D4F6 File Offset: 0x0010B6F6
		public string Identifier
		{
			get
			{
				return "ultrakill.keep-enabled";
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x0010D4FD File Offset: 0x0010B6FD
		public string ButtonEnabledOverride
		{
			get
			{
				return "STAY ACTIVE";
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x0010D504 File Offset: 0x0010B704
		public string ButtonDisabledOverride
		{
			get
			{
				return "DISABLE ON RELOAD";
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x0010D50B File Offset: 0x0010B70B
		public string Icon
		{
			get
			{
				return "warning";
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x0600235D RID: 9053 RVA: 0x0010D512 File Offset: 0x0010B712
		// (set) Token: 0x0600235E RID: 9054 RVA: 0x0010D51A File Offset: 0x0010B71A
		public bool IsActive { get; private set; }

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0010D523 File Offset: 0x0010B723
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0010D52C File Offset: 0x0010B72C
		public void Disable()
		{
			this.IsActive = false;
		}
	}
}
