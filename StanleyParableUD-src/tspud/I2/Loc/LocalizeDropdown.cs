using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002A7 RID: 679
	[AddComponentMenu("I2/Localization/Localize Dropdown")]
	public class LocalizeDropdown : MonoBehaviour
	{
		// Token: 0x06001155 RID: 4437 RVA: 0x0005F935 File Offset: 0x0005DB35
		public void Start()
		{
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
			this.OnLocalize();
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0005F94E File Offset: 0x0005DB4E
		public void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0005F961 File Offset: 0x0005DB61
		private void OnEnable()
		{
			if (this._Terms.Count == 0)
			{
				this.FillValues();
			}
			this.OnLocalize();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0005F97C File Offset: 0x0005DB7C
		public void OnLocalize()
		{
			if (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			this.UpdateLocalization();
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0005F9B8 File Offset: 0x0005DBB8
		private void FillValues()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null && I2Utils.IsPlaying())
			{
				this.FillValuesTMPro();
				return;
			}
			foreach (Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0005FA34 File Offset: 0x0005DC34
		public void UpdateLocalization()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				this.UpdateLocalizationTMPro();
				return;
			}
			component.options.Clear();
			foreach (string text in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(text, true, 0, true, false, null, null);
				component.options.Add(new Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0005FAC4 File Offset: 0x0005DCC4
		public void UpdateLocalizationTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			component.options.Clear();
			foreach (string text in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(text, true, 0, true, false, null, null);
				component.options.Add(new TMP_Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0005FB50 File Offset: 0x0005DD50
		private void FillValuesTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			foreach (TMP_Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x04000E53 RID: 3667
		public List<string> _Terms = new List<string>();
	}
}
