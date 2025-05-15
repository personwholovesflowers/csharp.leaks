using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class SubtitleSelector : MonoBehaviour
{
	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00024C39 File Offset: 0x00022E39
	private SubtitleProfile[] Profiles
	{
		get
		{
			return this.profileData.profiles;
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x00024C46 File Offset: 0x00022E46
	public void PrintLanguageTag(int index)
	{
		StringValueChangedEvent onPrintLanguageTag = this.OnPrintLanguageTag;
		if (onPrintLanguageTag == null)
		{
			return;
		}
		onPrintLanguageTag.Invoke(this.Profiles[index].DescriptionKey);
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x00024C65 File Offset: 0x00022E65
	public void PrintFontSize(int index)
	{
		FloatValueChangedEvent onPrintFontSize = this.OnPrintFontSize;
		if (onPrintFontSize == null)
		{
			return;
		}
		onPrintFontSize.Invoke(this.Profiles[index].FontSize);
	}

	// Token: 0x04000732 RID: 1842
	[Header("Profiles")]
	[SerializeField]
	private LanguageProfileData profileData;

	// Token: 0x04000733 RID: 1843
	[SerializeField]
	private StringValueChangedEvent OnPrintLanguageTag;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	private FloatValueChangedEvent OnPrintFontSize;
}
