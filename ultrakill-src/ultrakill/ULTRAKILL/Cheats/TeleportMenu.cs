using System;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062F RID: 1583
	public class TeleportMenu : ICheat
	{
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0010E1A5 File Offset: 0x0010C3A5
		public string LongName
		{
			get
			{
				return "Teleport Menu";
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x0010E1AC File Offset: 0x0010C3AC
		public string Identifier
		{
			get
			{
				return "ultrakill.teleport-menu";
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x0010E1B3 File Offset: 0x0010C3B3
		public string ButtonEnabledOverride
		{
			get
			{
				return "CLOSE";
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x0010D9F3 File Offset: 0x0010BBF3
		public string ButtonDisabledOverride
		{
			get
			{
				return "OPEN";
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x0010E1BA File Offset: 0x0010C3BA
		public string Icon
		{
			get
			{
				return "teleport";
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x0010E1C1 File Offset: 0x0010C3C1
		public bool IsActive { get; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x0010E1C9 File Offset: 0x0010C3C9
		public bool DefaultState { get; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0010E1D1 File Offset: 0x0010C3D1
		public void Enable(CheatsManager manager)
		{
			if (!GameStateManager.Instance.IsStateActive("sandbox-spawn-menu"))
			{
				MonoSingleton<CheatsManager>.Instance.HideMenu();
				MonoSingleton<OptionsManager>.Instance.UnPause();
			}
			MonoSingleton<CheatsController>.Instance.ShowTeleportPanel();
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Disable()
		{
		}
	}
}
