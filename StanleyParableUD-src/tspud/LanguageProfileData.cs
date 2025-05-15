using System;
using UnityEngine;

// Token: 0x020000F9 RID: 249
[CreateAssetMenu(fileName = "LanguageProfileData.asset", menuName = "Data/Language Profile Data")]
public class LanguageProfileData : ScriptableObject
{
	// Token: 0x04000660 RID: 1632
	[Header("Profiles")]
	public SubtitleProfile[] profiles;
}
