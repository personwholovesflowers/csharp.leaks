using System;
using UnityEngine;

// Token: 0x020002BC RID: 700
public class StyleComboRank : MonoBehaviour
{
	// Token: 0x06000F21 RID: 3873 RVA: 0x0006FE61 File Offset: 0x0006E061
	private void Update()
	{
		if (MonoSingleton<StyleHUD>.Instance.maxReachedRank >= this.rankToReach)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
		}
	}

	// Token: 0x04001449 RID: 5193
	public int rankToReach;
}
