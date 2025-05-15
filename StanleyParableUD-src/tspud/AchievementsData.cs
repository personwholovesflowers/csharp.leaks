using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200008C RID: 140
[CreateAssetMenu(fileName = "AchievementsData", menuName = "Achievements List")]
public class AchievementsData : ScriptableObject
{
	// Token: 0x0600035F RID: 863 RVA: 0x00016AD4 File Offset: 0x00014CD4
	public AchievementData FindAchievement(AchievementID achievementID)
	{
		return this.achievementsList.Find((AchievementData x) => x.id == achievementID);
	}

	// Token: 0x04000358 RID: 856
	[SerializeField]
	private List<AchievementData> achievementsList;
}
