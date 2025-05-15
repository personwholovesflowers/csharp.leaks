using System;
using UnityEngine;

// Token: 0x020002BA RID: 698
public class SlideLengthChallenge : MonoBehaviour
{
	// Token: 0x06000F1C RID: 3868 RVA: 0x0006FDDD File Offset: 0x0006DFDD
	private void Update()
	{
		if (MonoSingleton<NewMovement>.Instance.longestSlide >= this.slideLength)
		{
			MonoSingleton<ChallengeManager>.Instance.challengeDone = true;
		}
	}

	// Token: 0x04001446 RID: 5190
	public float slideLength;
}
