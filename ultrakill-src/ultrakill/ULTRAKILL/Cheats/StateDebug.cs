using System;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200060F RID: 1551
	public class StateDebug : ICheat, ICheatGUI
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x0010CBAD File Offset: 0x0010ADAD
		public string LongName
		{
			get
			{
				return "Game State Debug";
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x0010CBB4 File Offset: 0x0010ADB4
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.game-state";
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x0010CBBB File Offset: 0x0010ADBB
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060022BC RID: 8892 RVA: 0x0010CBC3 File Offset: 0x0010ADC3
		public string ButtonDisabledOverride { get; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060022BE RID: 8894 RVA: 0x0010CBCB File Offset: 0x0010ADCB
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x0010CBD3 File Offset: 0x0010ADD3
		public bool DefaultState { get; }

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x0010CBDB File Offset: 0x0010ADDB
		public void Enable(CheatsManager manager)
		{
			this.active = true;
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x0010CBE4 File Offset: 0x0010ADE4
		public void Disable()
		{
			this.active = false;
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x0010CBF0 File Offset: 0x0010ADF0
		public void OnGUI()
		{
			GUILayout.Label("Game State:", Array.Empty<GUILayoutOption>());
			GUILayout.Label("opman paused: " + MonoSingleton<OptionsManager>.Instance.paused.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.Label("opman frozen: " + MonoSingleton<OptionsManager>.Instance.frozen.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.Label("fc shopping: " + MonoSingleton<FistControl>.Instance.shopping.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.Label("gc activated: " + MonoSingleton<GunControl>.Instance.activated.ToString(), Array.Empty<GUILayoutOption>());
			if (MonoSingleton<WeaponCharges>.Instance)
			{
				GUILayout.Label("rc: " + MonoSingleton<WeaponCharges>.Instance.rocketCount.ToString(), Array.Empty<GUILayoutOption>());
				if (MonoSingleton<WeaponCharges>.Instance.rocketCount != 0 && MonoSingleton<WeaponCharges>.Instance.rocketFrozen)
				{
					GUILayout.Label("ts: " + MonoSingleton<WeaponCharges>.Instance.timeSinceIdleFrozen.ToString(), Array.Empty<GUILayoutOption>());
				}
			}
		}

		// Token: 0x04002E38 RID: 11832
		private bool active;
	}
}
