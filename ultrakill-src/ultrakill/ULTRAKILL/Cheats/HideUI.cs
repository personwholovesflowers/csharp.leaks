using System;
using plog;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000630 RID: 1584
	public class HideUI : ICheat
	{
		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x0010E202 File Offset: 0x0010C402
		public static bool Active
		{
			get
			{
				return HideUI.Instance != null && HideUI.Instance.IsActive;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x0010E217 File Offset: 0x0010C417
		// (set) Token: 0x0600243A RID: 9274 RVA: 0x0010E21E File Offset: 0x0010C41E
		private static HideUI Instance { get; set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x0010E226 File Offset: 0x0010C426
		public string LongName
		{
			get
			{
				return "Hide UI";
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x0010E22D File Offset: 0x0010C42D
		public string Identifier
		{
			get
			{
				return "ultrakill.hide-ui";
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x0010E234 File Offset: 0x0010C434
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x0010E23C File Offset: 0x0010C43C
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x0010E244 File Offset: 0x0010C444
		public string Icon { get; }

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x0010E24C File Offset: 0x0010C44C
		// (set) Token: 0x06002441 RID: 9281 RVA: 0x0010E254 File Offset: 0x0010C454
		public bool IsActive { get; private set; }

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0010E260 File Offset: 0x0010C460
		public void Enable(CheatsManager manager)
		{
			HideUI.Instance = this;
			this.IsActive = true;
			this.hudControllers = Object.FindObjectsOfType<HudController>();
			foreach (HudController hudController in this.hudControllers)
			{
				if (hudController != null)
				{
					hudController.CheckSituation();
					hudController.SetStyleVisibleTemp(null, null);
				}
			}
			if (MonoSingleton<CanvasController>.Instance && MonoSingleton<CanvasController>.Instance.crosshair)
			{
				MonoSingleton<CanvasController>.Instance.crosshair.CheckCrossHair();
			}
			if (MonoSingleton<PowerUpMeter>.Instance)
			{
				MonoSingleton<PowerUpMeter>.Instance.UpdateMeter();
			}
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0010E308 File Offset: 0x0010C508
		public void Disable()
		{
			this.IsActive = false;
			if (this.hudControllers == null)
			{
				return;
			}
			foreach (HudController hudController in this.hudControllers)
			{
				if (hudController != null)
				{
					hudController.CheckSituation();
					hudController.SetStyleVisibleTemp(null, null);
				}
			}
			if (MonoSingleton<CanvasController>.Instance && MonoSingleton<CanvasController>.Instance.crosshair)
			{
				MonoSingleton<CanvasController>.Instance.crosshair.CheckCrossHair();
			}
			if (MonoSingleton<PowerUpMeter>.Instance)
			{
				MonoSingleton<PowerUpMeter>.Instance.UpdateMeter();
			}
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Update()
		{
		}

		// Token: 0x04002E9F RID: 11935
		private static readonly global::plog.Logger Log = new global::plog.Logger("HideUI");

		// Token: 0x04002EA5 RID: 11941
		private HudController[] hudControllers;
	}
}
