using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000622 RID: 1570
	public class PauseTimedBombs : ICheat
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x0010D918 File Offset: 0x0010BB18
		public static bool Paused
		{
			get
			{
				return PauseTimedBombs._lastInstance != null && PauseTimedBombs._lastInstance.IsActive;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x0010D92D File Offset: 0x0010BB2D
		public string LongName
		{
			get
			{
				return "Pause Timed Bombs";
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x0010D934 File Offset: 0x0010BB34
		public string Identifier
		{
			get
			{
				return "ultrakill.pause-timed-bombs";
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x0010D93B File Offset: 0x0010BB3B
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x0010D943 File Offset: 0x0010BB43
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x0010D94B File Offset: 0x0010BB4B
		// (set) Token: 0x06002397 RID: 9111 RVA: 0x0010D953 File Offset: 0x0010BB53
		public bool IsActive { get; private set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06002398 RID: 9112 RVA: 0x0010D95C File Offset: 0x0010BB5C
		public bool DefaultState { get; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06002399 RID: 9113 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0010D964 File Offset: 0x0010BB64
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			PauseTimedBombs._lastInstance = this;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x0010D973 File Offset: 0x0010BB73
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E82 RID: 11906
		private static PauseTimedBombs _lastInstance;
	}
}
