using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
[CreateAssetMenu(fileName = "AchievementsData", menuName = "Achievement Data")]
public class AchievementData : ScriptableObject
{
	// Token: 0x06000345 RID: 837 RVA: 0x00016252 File Offset: 0x00014452
	public string TitleTerm(bool calledAchievements)
	{
		if (!calledAchievements && this.add_TROPHY_toTagIfTrophy)
		{
			return this.titleTag + "_TROPHY";
		}
		return this.titleTag;
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00016276 File Offset: 0x00014476
	public string DescriptionTerm(bool calledAchievements)
	{
		if (!calledAchievements && this.add_TROPHY_toTagIfTrophy)
		{
			return this.descriptionTag + "_TROPHY";
		}
		return this.descriptionTag;
	}

	// Token: 0x0400033C RID: 828
	public AchievementID id;

	// Token: 0x0400033D RID: 829
	public Sprite textureFound;

	// Token: 0x0400033E RID: 830
	public Sprite textureNotFound;

	// Token: 0x0400033F RID: 831
	public string steamID;

	// Token: 0x04000340 RID: 832
	public string steamAPIName;

	// Token: 0x04000341 RID: 833
	[SerializeField]
	private string titleTag;

	// Token: 0x04000342 RID: 834
	[SerializeField]
	private string descriptionTag;

	// Token: 0x04000343 RID: 835
	[SerializeField]
	private bool add_TROPHY_toTagIfTrophy;

	// Token: 0x04000344 RID: 836
	public StringConfigurable dateFoundConfigurable;
}
