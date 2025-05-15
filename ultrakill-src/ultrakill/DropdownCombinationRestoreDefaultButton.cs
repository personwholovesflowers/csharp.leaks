using System;
using System.Collections.Generic;
using SettingsMenu.Models;
using TMPro;
using UnityEngine;

// Token: 0x02000128 RID: 296
public class DropdownCombinationRestoreDefaultButton : MonoBehaviour
{
	// Token: 0x06000598 RID: 1432 RVA: 0x000273C1 File Offset: 0x000255C1
	private void Start()
	{
		this.dropdown.onValueChanged.AddListener(delegate(int _)
		{
			this.isValueDirty = true;
		});
		this.UpdateSelf();
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x000273E5 File Offset: 0x000255E5
	public void RestoreDefault()
	{
		this.dropdown.value = this.defaultCombination;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x000273F8 File Offset: 0x000255F8
	private void UpdateSelf()
	{
		ref DropdownCombinationRestoreDefaultButton.CombinationOption ptr = this.combinations[this.defaultCombination];
		bool flag = true;
		foreach (DropdownCombinationRestoreDefaultButton.BooleanPrefOption booleanPrefOption in ptr.subOptions)
		{
			if (booleanPrefOption.preferenceKey.GetBoolValue(false) != booleanPrefOption.expectedValue)
			{
				flag = false;
				break;
			}
		}
		this.buttonContainer.SetActive(!flag);
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00027480 File Offset: 0x00025680
	private void LateUpdate()
	{
		if (!this.isValueDirty)
		{
			return;
		}
		this.isValueDirty = false;
		this.UpdateSelf();
	}

	// Token: 0x040007CA RID: 1994
	public GameObject buttonContainer;

	// Token: 0x040007CB RID: 1995
	public int defaultCombination;

	// Token: 0x040007CC RID: 1996
	public List<DropdownCombinationRestoreDefaultButton.CombinationOption> combinations;

	// Token: 0x040007CD RID: 1997
	public TMP_Dropdown dropdown;

	// Token: 0x040007CE RID: 1998
	private bool isValueDirty;

	// Token: 0x02000129 RID: 297
	[Serializable]
	public struct CombinationOption
	{
		// Token: 0x040007CF RID: 1999
		public List<DropdownCombinationRestoreDefaultButton.BooleanPrefOption> subOptions;
	}

	// Token: 0x0200012A RID: 298
	[Serializable]
	public struct BooleanPrefOption
	{
		// Token: 0x040007D0 RID: 2000
		public PreferenceKey preferenceKey;

		// Token: 0x040007D1 RID: 2001
		public bool expectedValue;
	}
}
