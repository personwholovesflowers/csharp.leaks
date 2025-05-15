using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000607 RID: 1543
	public class ForceBossBars : ICheat
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x0010C459 File Offset: 0x0010A659
		public static bool Active
		{
			get
			{
				return ForceBossBars._lastInstance != null && ForceBossBars._lastInstance.IsActive;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x0010C46E File Offset: 0x0010A66E
		public string LongName
		{
			get
			{
				return "Force Enemy Boss Bars";
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06002258 RID: 8792 RVA: 0x0010C475 File Offset: 0x0010A675
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.force-boss-bars";
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x0010C47C File Offset: 0x0010A67C
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600225A RID: 8794 RVA: 0x0010C484 File Offset: 0x0010A684
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x0010C48C File Offset: 0x0010A68C
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x0010C494 File Offset: 0x0010A694
		public bool IsActive { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600225E RID: 8798 RVA: 0x0010C49D File Offset: 0x0010A69D
		public bool DefaultState { get; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x0010C4A5 File Offset: 0x0010A6A5
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			ForceBossBars._lastInstance = this;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x0010C4B4 File Offset: 0x0010A6B4
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E12 RID: 11794
		private static ForceBossBars _lastInstance;
	}
}
