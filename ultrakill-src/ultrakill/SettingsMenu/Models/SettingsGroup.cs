using System;
using UnityEngine;

namespace SettingsMenu.Models
{
	// Token: 0x0200052C RID: 1324
	[CreateAssetMenu(fileName = "SettingsGroup", menuName = "ULTRAKILL/Settings/Group")]
	public class SettingsGroup : ScriptableObject
	{
		// Token: 0x06001E27 RID: 7719 RVA: 0x000FA650 File Offset: 0x000F8850
		public bool GetEnabled()
		{
			if (!this.preferenceKey.IsValid())
			{
				return false;
			}
			bool flag = false;
			SettingsGroupValueType settingsGroupValueType = this.valueType;
			if (settingsGroupValueType != SettingsGroupValueType.Bool)
			{
				if (settingsGroupValueType == SettingsGroupValueType.Int)
				{
					flag = this.preferenceKey.GetIntValue(0) >= this.minValue;
				}
			}
			else
			{
				flag = this.preferenceKey.GetBoolValue(false);
			}
			if (!this.invertValue)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000FA6B3 File Offset: 0x000F88B3
		public void SetEnabledBool(bool enabled)
		{
			if (!this.preferenceKey.IsValid())
			{
				return;
			}
			if (this.valueType != SettingsGroupValueType.Bool)
			{
				return;
			}
			this.preferenceKey.SetBoolValue(this.invertValue ? (!enabled) : enabled);
		}

		// Token: 0x04002A95 RID: 10901
		public SettingsGroupTogglingMode togglingMode;

		// Token: 0x04002A96 RID: 10902
		public PreferenceKey preferenceKey;

		// Token: 0x04002A97 RID: 10903
		public SettingsGroupValueType valueType;

		// Token: 0x04002A98 RID: 10904
		public bool invertValue;

		// Token: 0x04002A99 RID: 10905
		[HideInInspector]
		public int minValue;
	}
}
