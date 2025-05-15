using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200060D RID: 1549
	public class SandboxArmDebug : ICheat
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x0010C9EF File Offset: 0x0010ABEF
		public static bool DebugActive
		{
			get
			{
				return SandboxArmDebug._lastInstance != null && SandboxArmDebug._lastInstance.active;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x0010CA04 File Offset: 0x0010AC04
		public string LongName
		{
			get
			{
				return "Sandbox Arm Debug";
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0010CA0B File Offset: 0x0010AC0B
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.sandbox-arm";
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x0010CA12 File Offset: 0x0010AC12
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x060022A5 RID: 8869 RVA: 0x0010CA1A File Offset: 0x0010AC1A
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x0010CA22 File Offset: 0x0010AC22
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x0010CA2A File Offset: 0x0010AC2A
		public bool DefaultState { get; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x0010CA32 File Offset: 0x0010AC32
		public void Enable(CheatsManager manager)
		{
			this.active = true;
			SandboxArmDebug._lastInstance = this;
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x0010CA41 File Offset: 0x0010AC41
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x04002E2F RID: 11823
		private bool active;

		// Token: 0x04002E30 RID: 11824
		private static SandboxArmDebug _lastInstance;
	}
}
