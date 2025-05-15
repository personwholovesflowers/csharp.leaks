using System;
using SettingsMenu.Models;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SettingsMenu.Components
{
	// Token: 0x02000548 RID: 1352
	[Serializable]
	public struct SettingsButtonEvent
	{
		// Token: 0x04002B06 RID: 11014
		public SettingsItem buttonItem;

		// Token: 0x04002B07 RID: 11015
		[FormerlySerializedAs("callback")]
		public UnityEvent onClickEvent;
	}
}
