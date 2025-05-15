using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
[CreateAssetMenu(fileName = "New SubtitleProfile", menuName = "Stanley/Subtitle Profile")]
public class SubtitleProfile : ScriptableObject
{
	// Token: 0x040009E6 RID: 2534
	public float FontSize = 30f;

	// Token: 0x040009E7 RID: 2535
	public float TextboxWidth = 2000f;

	// Token: 0x040009E8 RID: 2536
	public string DescriptionKey = "";

	// Token: 0x040009E9 RID: 2537
	[Header("The English name of the language...")]
	public string DescriptionIni2Loc = "";
}
