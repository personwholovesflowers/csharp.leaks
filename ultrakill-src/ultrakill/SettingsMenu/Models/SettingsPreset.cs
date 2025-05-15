using System;
using plog;
using UnityEngine;

namespace SettingsMenu.Models
{
	// Token: 0x02000536 RID: 1334
	[CreateAssetMenu(fileName = "SettingsPreset", menuName = "ULTRAKILL/Settings/Preset")]
	public class SettingsPreset : ScriptableObject
	{
		// Token: 0x06001E30 RID: 7728 RVA: 0x000FA7FC File Offset: 0x000F89FC
		public void Apply()
		{
			SettingsPreset.Log.Info("Applying settings preset " + base.name, null, null, null);
			foreach (PreferenceEntry preferenceEntry in this.preferences)
			{
				SettingsPreset.Log.Info(string.Format("Applying preference {0}", preferenceEntry), null, null, null);
				preferenceEntry.Apply();
			}
		}

		// Token: 0x04002ACA RID: 10954
		private static readonly global::plog.Logger Log = new global::plog.Logger("SettingsPreset");

		// Token: 0x04002ACB RID: 10955
		public PreferenceEntry[] preferences;
	}
}
