using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000617 RID: 1559
	public class InfinitePowerUps : ICheat
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06002315 RID: 8981 RVA: 0x0010D240 File Offset: 0x0010B440
		public static bool Enabled
		{
			get
			{
				InfinitePowerUps lastInstance = InfinitePowerUps._lastInstance;
				return lastInstance != null && lastInstance.IsActive;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x0010D25E File Offset: 0x0010B45E
		public string LongName
		{
			get
			{
				return "Infinite Power-Ups";
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x0010D265 File Offset: 0x0010B465
		public string Identifier
		{
			get
			{
				return "ultrakill.infinite-power-ups";
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0010D26C File Offset: 0x0010B46C
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06002319 RID: 8985 RVA: 0x0010D274 File Offset: 0x0010B474
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0010D27C File Offset: 0x0010B47C
		public string Icon
		{
			get
			{
				return "infinite-power-ups";
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x0010D283 File Offset: 0x0010B483
		// (set) Token: 0x0600231C RID: 8988 RVA: 0x0010D28B File Offset: 0x0010B48B
		public bool IsActive { get; private set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x0010D294 File Offset: 0x0010B494
		public bool DefaultState { get; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x0010D29C File Offset: 0x0010B49C
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			InfinitePowerUps._lastInstance = this;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x0010D2AB File Offset: 0x0010B4AB
		public void Disable()
		{
			this.IsActive = false;
		}

		// Token: 0x04002E5A RID: 11866
		private static InfinitePowerUps _lastInstance;
	}
}
