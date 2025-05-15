using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000605 RID: 1541
	public class BlindEnemies : ICheat
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x0600223C RID: 8764 RVA: 0x0010C390 File Offset: 0x0010A590
		public static bool Blind
		{
			get
			{
				BlindEnemies lastInstance = BlindEnemies._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x0010C3AE File Offset: 0x0010A5AE
		public string LongName
		{
			get
			{
				return "Blind Enemies";
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x0010C3B5 File Offset: 0x0010A5B5
		public string Identifier
		{
			get
			{
				return "ultrakill.blind-enemies";
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x0010C3BC File Offset: 0x0010A5BC
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x0010C3C4 File Offset: 0x0010A5C4
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x0010C3CC File Offset: 0x0010A5CC
		public string Icon
		{
			get
			{
				return "blind";
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x0010C3D3 File Offset: 0x0010A5D3
		// (set) Token: 0x06002243 RID: 8771 RVA: 0x0010C3DB File Offset: 0x0010A5DB
		public bool IsActive { get; private set; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x0010C3E4 File Offset: 0x0010A5E4
		public bool DefaultState { get; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x0010C3EC File Offset: 0x0010A5EC
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			BlindEnemies._lastInstance = this;
		}

		// Token: 0x06002247 RID: 8775 RVA: 0x0010C3FB File Offset: 0x0010A5FB
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E07 RID: 11783
		private static BlindEnemies _lastInstance;
	}
}
