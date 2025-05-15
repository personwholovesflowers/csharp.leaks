using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200061A RID: 1562
	public class Invincibility : ICheat
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x0010D38C File Offset: 0x0010B58C
		public static bool Enabled
		{
			get
			{
				Invincibility lastInstance = Invincibility._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06002337 RID: 9015 RVA: 0x0010D3AA File Offset: 0x0010B5AA
		public string LongName
		{
			get
			{
				return "Invincibility";
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x0010D3B1 File Offset: 0x0010B5B1
		public string Identifier
		{
			get
			{
				return "ultrakill.invincibility";
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x0010D3B8 File Offset: 0x0010B5B8
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x0010D3C0 File Offset: 0x0010B5C0
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x0010D3C8 File Offset: 0x0010B5C8
		public string Icon
		{
			get
			{
				return "invincibility";
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x0010D3CF File Offset: 0x0010B5CF
		// (set) Token: 0x0600233D RID: 9021 RVA: 0x0010D3D7 File Offset: 0x0010B5D7
		public bool IsActive { get; private set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x0010D3E0 File Offset: 0x0010B5E0
		public bool DefaultState { get; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600233F RID: 9023 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x0010D3E8 File Offset: 0x0010B5E8
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			Invincibility._lastInstance = this;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x0010D3F7 File Offset: 0x0010B5F7
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E66 RID: 11878
		private static Invincibility _lastInstance;
	}
}
