using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class SubtitleConfigurator : Configurator
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000708 RID: 1800 RVA: 0x00024EF9 File Offset: 0x000230F9
	private SubtitleProfile[] Profiles
	{
		get
		{
			return this.profileData.profiles;
		}
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x00024F06 File Offset: 0x00023106
	private new void Start()
	{
		base.Start();
		int num = this.profileData.profiles.Length;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00024F1C File Offset: 0x0002311C
	public override void ApplyData()
	{
		int intValue = this.configurable.GetIntValue();
		if (intValue < 0 || intValue >= this.Profiles.Length)
		{
			return;
		}
		this.PrintFontSize(this.Profiles[intValue]);
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x00024F53 File Offset: 0x00023153
	public override void PrintValue(Configurable _configurable)
	{
		this.OnPrintValue.Invoke(this.Profiles[_configurable.GetIntValue()].DescriptionKey);
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x00024F72 File Offset: 0x00023172
	public void PrintFontSize(SubtitleProfile profile)
	{
		this.OnPrintFontSize.Invoke(profile.FontSize);
	}

	// Token: 0x04000745 RID: 1861
	[Header("Profiles")]
	[SerializeField]
	private LanguageProfileData profileData;

	// Token: 0x04000746 RID: 1862
	[SerializeField]
	private FloatValueChangedEvent OnPrintFontSize;
}
