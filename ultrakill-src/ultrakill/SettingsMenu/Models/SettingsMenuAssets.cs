using System;
using SettingsMenu.Components;
using UnityEngine;

namespace SettingsMenu.Models
{
	// Token: 0x02000534 RID: 1332
	[CreateAssetMenu(fileName = "SettingsMenuAssets", menuName = "ULTRAKILL/Settings/MenuAssets")]
	public class SettingsMenuAssets : ScriptableObject
	{
		// Token: 0x06001E2D RID: 7725 RVA: 0x000FA7C5 File Offset: 0x000F89C5
		public SettingsBuilderBase GetBuilderFor(SettingsItemType itemType)
		{
			switch (itemType)
			{
			case SettingsItemType.Toggle:
				return this.togglePrefab;
			case SettingsItemType.Dropdown:
				return this.dropdownPrefab;
			case SettingsItemType.Slider:
				return this.sliderPrefab;
			case SettingsItemType.Button:
				return this.actionButtonPrefab;
			default:
				return null;
			}
		}

		// Token: 0x04002AC1 RID: 10945
		public SettingsItemBuilder itemPrefab;

		// Token: 0x04002AC2 RID: 10946
		public SettingsCategoryBuilder categoryTitlePrefab;

		// Token: 0x04002AC3 RID: 10947
		public SettingsBuilderBase togglePrefab;

		// Token: 0x04002AC4 RID: 10948
		public SettingsBuilderBase dropdownPrefab;

		// Token: 0x04002AC5 RID: 10949
		public SettingsBuilderBase sliderPrefab;

		// Token: 0x04002AC6 RID: 10950
		public SettingsBuilderBase actionButtonPrefab;

		// Token: 0x04002AC7 RID: 10951
		public SettingsRestoreDefaultButton resetButtonPrefab;

		// Token: 0x04002AC8 RID: 10952
		public SettingsRestoreDefaultButton smallResetButtonPrefab;
	}
}
