using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000609 RID: 1545
	public class HideCheatsStatus : ICheat
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06002270 RID: 8816 RVA: 0x0010C530 File Offset: 0x0010A730
		public static bool HideStatus
		{
			get
			{
				HideCheatsStatus lastInstance = HideCheatsStatus._lastInstance;
				return (lastInstance != null && lastInstance.active) || HideUI.Active;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06002271 RID: 8817 RVA: 0x0010C555 File Offset: 0x0010A755
		public string LongName
		{
			get
			{
				return "Hide Cheats Enabled Status";
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06002272 RID: 8818 RVA: 0x0010C55C File Offset: 0x0010A75C
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.hide-cheats-status";
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06002273 RID: 8819 RVA: 0x0010C563 File Offset: 0x0010A763
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06002274 RID: 8820 RVA: 0x0010C56B File Offset: 0x0010A76B
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06002275 RID: 8821 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x0010C573 File Offset: 0x0010A773
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06002277 RID: 8823 RVA: 0x0010C57B File Offset: 0x0010A77B
		public bool DefaultState { get; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0010C583 File Offset: 0x0010A783
		public void Enable(CheatsManager manager)
		{
			this.active = true;
			HideCheatsStatus._lastInstance = this;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0010C592 File Offset: 0x0010A792
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x04002E18 RID: 11800
		private static HideCheatsStatus _lastInstance;

		// Token: 0x04002E1C RID: 11804
		private bool active;
	}
}
