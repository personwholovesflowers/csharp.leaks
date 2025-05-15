using System;
using UnityEngine;

namespace SettingsMenu.Components
{
	// Token: 0x02000543 RID: 1347
	public abstract class SettingsLogicBase : MonoBehaviour
	{
		// Token: 0x06001E64 RID: 7780
		public abstract void Initialize(SettingsMenu settingsMenu);

		// Token: 0x06001E65 RID: 7781
		public abstract void OnPrefChanged(string key, object value);
	}
}
