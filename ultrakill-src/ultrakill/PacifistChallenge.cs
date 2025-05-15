using System;
using UnityEngine;

// Token: 0x020002B9 RID: 697
public class PacifistChallenge : MonoBehaviour
{
	// Token: 0x06000F1A RID: 3866 RVA: 0x0006FDAC File Offset: 0x0006DFAC
	private void Update()
	{
		if (MonoSingleton<StyleCalculator>.Instance)
		{
			if (!MonoSingleton<StyleCalculator>.Instance.enemiesShot)
			{
				MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
				return;
			}
			MonoSingleton<ChallengeManager>.Instance.challengeDone = false;
		}
	}
}
