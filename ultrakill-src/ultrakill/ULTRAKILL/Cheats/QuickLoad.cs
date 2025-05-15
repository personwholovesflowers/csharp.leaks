using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000626 RID: 1574
	public class QuickLoad : ICheat
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x0010DA4C File Offset: 0x0010BC4C
		public string LongName
		{
			get
			{
				return "Quick Load";
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x0010DA53 File Offset: 0x0010BC53
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.quick-load";
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x0010DA5A File Offset: 0x0010BC5A
		public string ButtonDisabledOverride
		{
			get
			{
				return "LOAD LATEST SAVE";
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x0010DA61 File Offset: 0x0010BC61
		public string Icon
		{
			get
			{
				return "quick-load";
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060023C5 RID: 9157 RVA: 0x0010DA68 File Offset: 0x0010BC68
		// (set) Token: 0x060023C6 RID: 9158 RVA: 0x0010DA70 File Offset: 0x0010BC70
		public bool IsActive { get; private set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x0010DA79 File Offset: 0x0010BC79
		public void Enable(CheatsManager manager)
		{
			MonoSingleton<SandboxSaver>.Instance.QuickLoad();
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x0010DA85 File Offset: 0x0010BC85
		public void Disable()
		{
			this.IsActive = false;
		}
	}
}
