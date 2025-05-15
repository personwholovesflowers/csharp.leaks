using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000623 RID: 1571
	public class ClearMap : ICheat
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600239D RID: 9117 RVA: 0x0010D97C File Offset: 0x0010BB7C
		public string LongName
		{
			get
			{
				return "Clear Map";
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600239E RID: 9118 RVA: 0x0010D983 File Offset: 0x0010BB83
		public string Identifier
		{
			get
			{
				return "ultrakill.sandbox.clear";
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x0600239F RID: 9119 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x0010D98A File Offset: 0x0010BB8A
		public string ButtonDisabledOverride
		{
			get
			{
				return "CLEAR";
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060023A1 RID: 9121 RVA: 0x0010D991 File Offset: 0x0010BB91
		public string Icon
		{
			get
			{
				return "delete";
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060023A2 RID: 9122 RVA: 0x0010D998 File Offset: 0x0010BB98
		// (set) Token: 0x060023A3 RID: 9123 RVA: 0x0010D9A0 File Offset: 0x0010BBA0
		public bool IsActive { get; private set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060023A4 RID: 9124 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060023A5 RID: 9125 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0010D9A9 File Offset: 0x0010BBA9
		public void Enable(CheatsManager manager)
		{
			SandboxSaver.Clear();
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0010D9B0 File Offset: 0x0010BBB0
		public void Disable()
		{
			this.IsActive = false;
		}
	}
}
