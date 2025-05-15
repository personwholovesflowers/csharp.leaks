using System;
using StanleyUI;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class QualitySelector : MonoBehaviour, ISettingsIntListener
{
	// Token: 0x060006EC RID: 1772 RVA: 0x00024BE9 File Offset: 0x00022DE9
	private string IndexToQualitySettingString(int index)
	{
		switch (index)
		{
		case 0:
			return "Low";
		case 1:
			return "Medium";
		case 2:
			return "High";
		default:
			return "None";
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00024C16 File Offset: 0x00022E16
	public void PrintQualityLocalizationTag(int index)
	{
		StringValueChangedEvent onPrintQualityLocalizationTag = this.OnPrintQualityLocalizationTag;
		if (onPrintQualityLocalizationTag == null)
		{
			return;
		}
		onPrintQualityLocalizationTag.Invoke("Menu_Quality_" + this.IndexToQualitySettingString(index));
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00005444 File Offset: 0x00003644
	[QuickReference(typeof(VideoSettingsController))]
	public void SetValue(int val)
	{
	}

	// Token: 0x0400072F RID: 1839
	[SerializeField]
	private StringValueChangedEvent OnPrintQualityLocalizationTag;

	// Token: 0x04000730 RID: 1840
	[Header("Other video options that need to be force updated")]
	[SerializeField]
	private IntConfigurable antiAliasingConfigurable;

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private BooleanConfigurable vSyncConfigurable;
}
