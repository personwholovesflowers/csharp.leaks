using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062D RID: 1581
	public class GhostDroneMode : ICheat
	{
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x0010DD7B File Offset: 0x0010BF7B
		public static bool Enabled
		{
			get
			{
				return GhostDroneMode._lastInstance != null && GhostDroneMode._lastInstance.active;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x0010DD90 File Offset: 0x0010BF90
		public string LongName
		{
			get
			{
				return "Drone Haunting";
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x0010DD97 File Offset: 0x0010BF97
		public string Identifier
		{
			get
			{
				return "ultrakill.ghost-drone-mode";
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x0010DD9E File Offset: 0x0010BF9E
		public string Icon
		{
			get
			{
				return "ghost";
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x0010DDA5 File Offset: 0x0010BFA5
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x0010DDAD File Offset: 0x0010BFAD
		public void Enable(CheatsManager manager)
		{
			this.active = true;
			GhostDroneMode._lastInstance = this;
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x0010DDBC File Offset: 0x0010BFBC
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x04002E96 RID: 11926
		private static GhostDroneMode _lastInstance;

		// Token: 0x04002E97 RID: 11927
		private bool active;
	}
}
