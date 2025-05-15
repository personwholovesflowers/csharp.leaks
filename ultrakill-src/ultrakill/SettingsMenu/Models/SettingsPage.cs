using System;
using UnityEngine;

namespace SettingsMenu.Models
{
	// Token: 0x02000535 RID: 1333
	[CreateAssetMenu(fileName = "SettingsPage", menuName = "ULTRAKILL/Settings/Page")]
	public class SettingsPage : ScriptableObject
	{
		// Token: 0x04002AC9 RID: 10953
		public SettingsCategory[] categories;
	}
}
