using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062C RID: 1580
	public class CrashMode : ICheat
	{
		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x06002405 RID: 9221 RVA: 0x0010DD0E File Offset: 0x0010BF0E
		public string LongName
		{
			get
			{
				return "Clash Mode";
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x0010DD15 File Offset: 0x0010BF15
		public string Identifier
		{
			get
			{
				return "ultrakill.clash-mode";
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06002409 RID: 9225 RVA: 0x0010DD1C File Offset: 0x0010BF1C
		public string Icon
		{
			get
			{
				return "clash";
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x0010DD23 File Offset: 0x0010BF23
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600240B RID: 9227 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x0010DD2B File Offset: 0x0010BF2B
		public void Enable(CheatsManager manager)
		{
			if (MonoSingleton<PlayerTracker>.Instance.levelStarted)
			{
				MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.flight");
				MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.noclip");
			}
			this.active = true;
			MonoSingleton<PlayerTracker>.Instance.ChangeToPlatformer();
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x0010DD68 File Offset: 0x0010BF68
		public void Disable()
		{
			this.active = false;
			MonoSingleton<PlayerTracker>.Instance.ChangeToFPS();
		}

		// Token: 0x04002E95 RID: 11925
		private bool active;
	}
}
