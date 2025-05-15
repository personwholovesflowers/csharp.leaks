using System;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class SubtitleSizeSelector : MonoBehaviour
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00024C84 File Offset: 0x00022E84
	private SubtitleSizeProfile[] Profiles
	{
		get
		{
			return this.profileData.sizeProfiles;
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x00024C91 File Offset: 0x00022E91
	public void PrintLocalizationTag(int index)
	{
		StringValueChangedEvent onPrintLocalizationTag = this.OnPrintLocalizationTag;
		if (onPrintLocalizationTag == null)
		{
			return;
		}
		onPrintLocalizationTag.Invoke(this.Profiles[index].DescriptionLocalizationKey);
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00024CB0 File Offset: 0x00022EB0
	public void PrintUIHeightReferenceValue(int index)
	{
		FloatValueChangedEvent onPrintUIHeightReferenceValue = this.OnPrintUIHeightReferenceValue;
		if (onPrintUIHeightReferenceValue == null)
		{
			return;
		}
		onPrintUIHeightReferenceValue.Invoke(this.Profiles[index].uiReferenceHeight);
	}

	// Token: 0x04000735 RID: 1845
	[Header("Profiles")]
	[SerializeField]
	private SubtitleSizeProfileData profileData;

	// Token: 0x04000736 RID: 1846
	[SerializeField]
	private StringValueChangedEvent OnPrintLocalizationTag;

	// Token: 0x04000737 RID: 1847
	[SerializeField]
	private FloatValueChangedEvent OnPrintUIHeightReferenceValue;
}
