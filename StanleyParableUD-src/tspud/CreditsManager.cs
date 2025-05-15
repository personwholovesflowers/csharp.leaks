using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A5 RID: 165
public class CreditsManager : MonoBehaviour
{
	// Token: 0x060003F4 RID: 1012 RVA: 0x00018D26 File Offset: 0x00016F26
	public void ShowMenuCredits()
	{
		this.creditsAnimator.playbackTime = 0f;
		this.creditsAnimator.Rebind();
		this.creditsCanvasGroup.alpha = 1f;
		this.creditsAudioSource.Play();
	}

	// Token: 0x040003EA RID: 1002
	[SerializeField]
	private Animator creditsAnimator;

	// Token: 0x040003EB RID: 1003
	[SerializeField]
	private AudioSource creditsAudioSource;

	// Token: 0x040003EC RID: 1004
	[SerializeField]
	private Image creditsBlackBackgrounds;

	// Token: 0x040003ED RID: 1005
	[SerializeField]
	private CanvasGroup creditsCanvasGroup;
}
