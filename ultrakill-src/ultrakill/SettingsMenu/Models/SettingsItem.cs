using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SettingsMenu.Models
{
	// Token: 0x0200052F RID: 1327
	public class SettingsItem : ScriptableObject
	{
		// Token: 0x06001E2A RID: 7722 RVA: 0x000FA6E6 File Offset: 0x000F88E6
		public string GetLabel(bool capitalize = true)
		{
			if (!capitalize)
			{
				return this.label;
			}
			return this.label.ToUpper();
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000FA700 File Offset: 0x000F8900
		public List<string> GetDropdownItems()
		{
			List<string> list = new List<string>();
			SettingsDropdownType settingsDropdownType = this.dropdownType;
			if (settingsDropdownType != SettingsDropdownType.Enum)
			{
				if (settingsDropdownType != SettingsDropdownType.List)
				{
					return list;
				}
			}
			else
			{
				Type type = Type.GetType(this.dropdownEnumType);
				if (type == null)
				{
					list.Add("Invalid Type");
					return list;
				}
				using (IEnumerator enumerator = Enum.GetValues(type).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						list.Add(obj.ToString());
					}
					return list;
				}
			}
			list.AddRange(this.dropdownList);
			return list;
		}

		// Token: 0x04002AA0 RID: 10912
		public string label;

		// Token: 0x04002AA1 RID: 10913
		public SettingsItemType itemType;

		// Token: 0x04002AA2 RID: 10914
		public SettingsItemStyle style;

		// Token: 0x04002AA3 RID: 10915
		public SettingsGroup group;

		// Token: 0x04002AA4 RID: 10916
		public PlatformRequirements platformRequirements;

		// Token: 0x04002AA5 RID: 10917
		[HideInInspector]
		public bool noResetButton;

		// Token: 0x04002AA6 RID: 10918
		[HideInInspector]
		public SettingsDropdownType dropdownType = SettingsDropdownType.List;

		// Token: 0x04002AA7 RID: 10919
		[HideInInspector]
		public string dropdownEnumType;

		// Token: 0x04002AA8 RID: 10920
		[HideInInspector]
		public string[] dropdownList;

		// Token: 0x04002AA9 RID: 10921
		[HideInInspector]
		public SliderConfig sliderConfig;

		// Token: 0x04002AAA RID: 10922
		[HideInInspector]
		public string buttonLabel;

		// Token: 0x04002AAB RID: 10923
		[HideInInspector]
		public string sideNote;

		// Token: 0x04002AAC RID: 10924
		[HideInInspector]
		public PreferenceKey preferenceKey;

		// Token: 0x04002AAD RID: 10925
		[HideInInspector]
		[Tooltip("The multiplier applied to values before displaying them to the user, that is also reversed before saving.\n\nFor example a multiplier of 100, will cause the value of 0.5 to be displayed as 50 on a slider, while still being saved as 0.5")]
		public float valueDisplayMultiplayer = 100f;

		// Token: 0x04002AAE RID: 10926
		[HideInInspector]
		public ValueType valueType = ValueType.Bool;

		// Token: 0x04002AAF RID: 10927
		[FormerlySerializedAs("dropdownCombinationOptions")]
		[HideInInspector]
		public List<DropdownCombinationRestoreDefaultButton.CombinationOption> combinationOptions;

		// Token: 0x04002AB0 RID: 10928
		[HideInInspector]
		public int defaultCombination;
	}
}
