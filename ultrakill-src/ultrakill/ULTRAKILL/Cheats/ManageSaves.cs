using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000625 RID: 1573
	public class ManageSaves : ICheat
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x0010D9E5 File Offset: 0x0010BBE5
		public string LongName
		{
			get
			{
				return "Manage Saves";
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060023B5 RID: 9141 RVA: 0x0010D9EC File Offset: 0x0010BBEC
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.save-menu";
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x0010D9F3 File Offset: 0x0010BBF3
		public string ButtonDisabledOverride
		{
			get
			{
				return "OPEN";
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x0010D9FA File Offset: 0x0010BBFA
		public string Icon
		{
			get
			{
				return "load";
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060023B9 RID: 9145 RVA: 0x0010DA01 File Offset: 0x0010BC01
		// (set) Token: 0x060023BA RID: 9146 RVA: 0x0010DA09 File Offset: 0x0010BC09
		public bool IsActive { get; private set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060023BC RID: 9148 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0010DA12 File Offset: 0x0010BC12
		public void Enable(CheatsManager manager)
		{
			if (!GameStateManager.Instance.IsStateActive("sandbox-spawn-menu"))
			{
				MonoSingleton<CheatsManager>.Instance.ShowMenu();
				MonoSingleton<OptionsManager>.Instance.Pause();
			}
			MonoSingleton<SandboxHud>.Instance.ShowSavesMenu();
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0010DA43 File Offset: 0x0010BC43
		public void Disable()
		{
			this.IsActive = false;
		}
	}
}
