using System;
using SettingsMenu.Models;
using UnityEngine.Events;

namespace SettingsMenu.Components
{
	// Token: 0x02000549 RID: 1353
	[Serializable]
	public struct SettingsGroupInterrupt
	{
		// Token: 0x04002B08 RID: 11016
		public SettingsGroup group;

		// Token: 0x04002B09 RID: 11017
		public bool suppressDefaultEnable;

		// Token: 0x04002B0A RID: 11018
		public UnityEvent onEnableEvent;
	}
}
