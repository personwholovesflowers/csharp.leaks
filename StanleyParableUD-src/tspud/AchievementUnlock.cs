using System;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class AchievementUnlock : MonoBehaviour
{
	// Token: 0x0600035D RID: 861 RVA: 0x00016AC5 File Offset: 0x00014CC5
	public void Unlock()
	{
		PlatformAchievements.UnlockAchievement(this.achievementToUnlock);
	}

	// Token: 0x04000357 RID: 855
	[SerializeField]
	private AchievementID achievementToUnlock;
}
