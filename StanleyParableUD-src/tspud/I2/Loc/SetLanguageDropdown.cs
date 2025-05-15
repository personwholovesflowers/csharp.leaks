using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002D2 RID: 722
	[AddComponentMenu("I2/Localization/SetLanguage Dropdown")]
	public class SetLanguageDropdown : MonoBehaviour
	{
		// Token: 0x06001288 RID: 4744 RVA: 0x00064C4C File Offset: 0x00062E4C
		private void OnEnable()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				return;
			}
			string currentLanguage = LocalizationManager.CurrentLanguage;
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			component.ClearOptions();
			component.AddOptions(allLanguages);
			component.value = allLanguages.IndexOf(currentLanguage);
			component.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
			component.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00064CD4 File Offset: 0x00062ED4
		private void OnValueChanged(int index)
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (index < 0)
			{
				index = 0;
				component.value = index;
			}
			LocalizationManager.CurrentLanguage = component.options[index].text;
		}
	}
}
