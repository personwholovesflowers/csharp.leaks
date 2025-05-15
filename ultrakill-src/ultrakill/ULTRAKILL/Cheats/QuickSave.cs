using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000627 RID: 1575
	public class QuickSave : ICheat
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x0010DA8E File Offset: 0x0010BC8E
		public string LongName
		{
			get
			{
				return "Quick Save";
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060023CD RID: 9165 RVA: 0x0010DA95 File Offset: 0x0010BC95
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.quick-save";
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x0010DA9C File Offset: 0x0010BC9C
		public string ButtonEnabledOverride
		{
			get
			{
				return "SAVE";
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x0010DAA3 File Offset: 0x0010BCA3
		public string ButtonDisabledOverride
		{
			get
			{
				return "NEW SAVE";
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x0010DAAA File Offset: 0x0010BCAA
		public string Icon
		{
			get
			{
				return "save";
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x0010DAB1 File Offset: 0x0010BCB1
		public bool IsActive
		{
			get
			{
				return MonoSingleton<SandboxSaver>.Instance != null && !string.IsNullOrEmpty(MonoSingleton<SandboxSaver>.Instance.activeSave);
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x0010DAD4 File Offset: 0x0010BCD4
		public void Enable(CheatsManager manager)
		{
			MonoSingleton<SandboxSaver>.Instance.QuickSave();
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x0010DAE0 File Offset: 0x0010BCE0
		public void Disable()
		{
			if (MonoSingleton<PrefsManager>.Instance.GetBool("sandboxSaveOverwriteWarnings", false))
			{
				MonoSingleton<SandboxSaveConfirmation>.Instance.DisplayDialog();
				return;
			}
			MonoSingleton<SandboxSaver>.Instance.Save(MonoSingleton<SandboxSaver>.Instance.activeSave);
		}
	}
}
