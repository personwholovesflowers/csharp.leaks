using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[CreateAssetMenu(fileName = "SubtitleSizeProfileData.asset", menuName = "Data/Subtitle Size Profile Data")]
public class SubtitleSizeProfileData : ScriptableObject
{
	// Token: 0x040009ED RID: 2541
	[Header("Profiles")]
	public SubtitleSizeProfile[] sizeProfiles;
}
