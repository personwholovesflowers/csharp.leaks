using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000608 RID: 1544
	public class GunControlDebug : ICheat
	{
		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x0010C4C0 File Offset: 0x0010A6C0
		public static bool GunControlActivated
		{
			get
			{
				GunControlDebug lastInstance = GunControlDebug._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x0010C4DE File Offset: 0x0010A6DE
		public string LongName
		{
			get
			{
				return "Gun Control Debug";
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x0010C4E5 File Offset: 0x0010A6E5
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.gun-control";
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06002266 RID: 8806 RVA: 0x0010C4EC File Offset: 0x0010A6EC
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x0010C4F4 File Offset: 0x0010A6F4
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x0010C4FC File Offset: 0x0010A6FC
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x0010C504 File Offset: 0x0010A704
		public bool IsActive { get; private set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x0010C50D File Offset: 0x0010A70D
		public bool DefaultState { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600226C RID: 8812 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x0010C515 File Offset: 0x0010A715
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			GunControlDebug._lastInstance = this;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x0010C524 File Offset: 0x0010A724
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E17 RID: 11799
		private static GunControlDebug _lastInstance;
	}
}
