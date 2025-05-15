using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000324 RID: 804
public class OptionsDropdown : MonoBehaviour
{
	// Token: 0x06001282 RID: 4738 RVA: 0x000943F0 File Offset: 0x000925F0
	private void Awake()
	{
		this.dropdown.SetValueWithoutNotify(MonoSingleton<PrefsManager>.Instance.GetInt(this.prefName, this.dropdown.value));
		this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x0009443F File Offset: 0x0009263F
	private void OnValueChanged(int value)
	{
		MonoSingleton<PrefsManager>.Instance.SetInt(this.prefName, value);
	}

	// Token: 0x04001975 RID: 6517
	public TMP_Dropdown dropdown;

	// Token: 0x04001976 RID: 6518
	public string prefName;
}
