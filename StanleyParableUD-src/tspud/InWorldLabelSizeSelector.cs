using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class InWorldLabelSizeSelector : MonoBehaviour
{
	// Token: 0x060006E9 RID: 1769 RVA: 0x00024BA3 File Offset: 0x00022DA3
	public void PrintLocalizationTag(int index)
	{
		StringValueChangedEvent onPrintLocalizationTag = this.OnPrintLocalizationTag;
		if (onPrintLocalizationTag == null)
		{
			return;
		}
		onPrintLocalizationTag.Invoke(InWorldLabelManager.Instance.sizeProfiles[index].i2LocalizationTerm);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00024BC6 File Offset: 0x00022DC6
	public void PrintFontSize(int index)
	{
		FloatValueChangedEvent onPrintFontSizeValue = this.OnPrintFontSizeValue;
		if (onPrintFontSizeValue == null)
		{
			return;
		}
		onPrintFontSizeValue.Invoke(InWorldLabelManager.Instance.sizeProfiles[index].fontSize);
	}

	// Token: 0x0400072D RID: 1837
	[SerializeField]
	private StringValueChangedEvent OnPrintLocalizationTag;

	// Token: 0x0400072E RID: 1838
	[SerializeField]
	private FloatValueChangedEvent OnPrintFontSizeValue;
}
