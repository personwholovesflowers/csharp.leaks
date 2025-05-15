using System;
using System.Collections.Generic;
using SettingsMenu.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x0200053C RID: 1340
	public class SettingsDropdown : SettingsBuilderBase
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000FAA35 File Offset: 0x000F8C35
		// (set) Token: 0x06001E40 RID: 7744 RVA: 0x000FAA42 File Offset: 0x000F8C42
		public TMP_Dropdown.DropdownEvent onValueChanged
		{
			get
			{
				return this.dropdown.onValueChanged;
			}
			set
			{
				this.dropdown.onValueChanged = value;
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x000FAA50 File Offset: 0x000F8C50
		public override void ConfigureFrom(SettingsItemBuilder itemBuilder, SettingsPageBuilder pageBuilder)
		{
			this.dropdown.ClearOptions();
			this.item = itemBuilder.asset;
			List<string> dropdownItems = this.item.GetDropdownItems();
			this.dropdown.AddOptions(dropdownItems);
			this.LoadCurrentValue();
			this.dropdown.onValueChanged.AddListener(new UnityAction<int>(itemBuilder.ValueChanged<int>));
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000FAAAE File Offset: 0x000F8CAE
		public override void SetSelected()
		{
			SettingsMenu.SetSelected(this.dropdown);
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000FAABC File Offset: 0x000F8CBC
		public override void AttachRestoreDefaultButton(SettingsRestoreDefaultButton restoreDefaultButton)
		{
			if (this.item.valueType != ValueType.BoolCombination)
			{
				restoreDefaultButton.dropdown = this.dropdown;
				return;
			}
			DropdownCombinationRestoreDefaultButton dropdownCombinationRestoreDefaultButton = restoreDefaultButton.gameObject.AddComponent<DropdownCombinationRestoreDefaultButton>();
			dropdownCombinationRestoreDefaultButton.buttonContainer = restoreDefaultButton.buttonContainer;
			dropdownCombinationRestoreDefaultButton.dropdown = this.dropdown;
			dropdownCombinationRestoreDefaultButton.defaultCombination = this.item.defaultCombination;
			dropdownCombinationRestoreDefaultButton.combinations = this.item.combinationOptions;
			Button button;
			if (dropdownCombinationRestoreDefaultButton.buttonContainer.TryGetComponent<Button>(out button))
			{
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(new UnityAction(dropdownCombinationRestoreDefaultButton.RestoreDefault));
			}
			else
			{
				Debug.LogError("Button not found in container. Unable to register onClick event.");
			}
			Object.Destroy(restoreDefaultButton);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000FAB6D File Offset: 0x000F8D6D
		public void SetDropdownItems(List<string> items, bool reloadValue = true)
		{
			this.dropdown.ClearOptions();
			this.dropdown.AddOptions(items);
			if (!reloadValue)
			{
				return;
			}
			this.LoadCurrentValue();
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000FAB90 File Offset: 0x000F8D90
		public void SetDropdownValue(int value, bool notify = false)
		{
			if (notify)
			{
				this.dropdown.value = value;
			}
			else
			{
				this.dropdown.SetValueWithoutNotify(value);
			}
			this.dropdown.RefreshShownValue();
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x000FABBC File Offset: 0x000F8DBC
		private void LoadCurrentValue()
		{
			ValueType valueType = this.item.valueType;
			if (valueType != ValueType.Int)
			{
				if (valueType != ValueType.BoolCombination)
				{
					return;
				}
				if (this.item.combinationOptions.Count != this.dropdown.options.Count)
				{
					throw new Exception("Dropdown items count does not match the combination options count");
				}
				int num = 0;
				for (int i = 0; i < this.item.combinationOptions.Count; i++)
				{
					ref DropdownCombinationRestoreDefaultButton.CombinationOption ptr = this.item.combinationOptions[i];
					bool flag = true;
					foreach (DropdownCombinationRestoreDefaultButton.BooleanPrefOption booleanPrefOption in ptr.subOptions)
					{
						if (booleanPrefOption.preferenceKey.GetBoolValue(false) != booleanPrefOption.expectedValue)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						num = i;
						break;
					}
				}
				this.dropdown.SetValueWithoutNotify(num);
			}
			else if (this.item.preferenceKey.IsValid())
			{
				int intValue = this.item.preferenceKey.GetIntValue(0);
				this.dropdown.SetValueWithoutNotify(intValue);
				return;
			}
		}

		// Token: 0x04002ADC RID: 10972
		[SerializeField]
		private TMP_Dropdown dropdown;

		// Token: 0x04002ADD RID: 10973
		private SettingsItem item;
	}
}
