using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
[CreateAssetMenu(fileName = "New SubtitleSizeProfile", menuName = "Stanley/Subtitle Size Profile")]
public class SubtitleSizeProfile : ScriptableObject
{
	// Token: 0x040009EA RID: 2538
	public float uiReferenceHeight = 1080f;

	// Token: 0x040009EB RID: 2539
	[Header("The English name of the language...")]
	public string Description = "";

	// Token: 0x040009EC RID: 2540
	[Header("The tag in i2Loc")]
	public string DescriptionLocalizationKey = "";
}
