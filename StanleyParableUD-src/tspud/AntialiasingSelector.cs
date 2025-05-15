using System;
using StanleyUI;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class AntialiasingSelector : MonoBehaviour, ISettingsIntListener
{
	// Token: 0x060006E4 RID: 1764 RVA: 0x00005444 File Offset: 0x00003644
	[QuickReference(typeof(VideoSettingsController))]
	public void SetValue(int val)
	{
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x00024B63 File Offset: 0x00022D63
	public void PrintAll(int index)
	{
		this.PrintAAOption(index);
		this.PrintAAValue(index);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x00024B73 File Offset: 0x00022D73
	public void PrintAAOption(int index)
	{
		StringValueChangedEvent onPrintAAOption = this.OnPrintAAOption;
		if (onPrintAAOption == null)
		{
			return;
		}
		onPrintAAOption.Invoke(VideoSettingsController.IndexToAAOption(index));
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x00024B8B File Offset: 0x00022D8B
	public void PrintAAValue(int index)
	{
		IntValueChangedEvent onPrintAAValue = this.OnPrintAAValue;
		if (onPrintAAValue == null)
		{
			return;
		}
		onPrintAAValue.Invoke(VideoSettingsController.IndexToAAValue(index));
	}

	// Token: 0x0400072B RID: 1835
	[SerializeField]
	private StringValueChangedEvent OnPrintAAOption;

	// Token: 0x0400072C RID: 1836
	[SerializeField]
	private IntValueChangedEvent OnPrintAAValue;
}
