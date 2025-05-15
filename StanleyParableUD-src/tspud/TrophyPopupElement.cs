using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BF RID: 447
public class TrophyPopupElement : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (set) Token: 0x06000A58 RID: 2648 RVA: 0x00030B08 File Offset: 0x0002ED08
	public AchievementID ID
	{
		set
		{
			AchievementData achievementData = this.data.FindAchievement(value);
			if (achievementData == null)
			{
				return;
			}
			this.trophyImage.sprite = achievementData.textureFound;
			if (this.title != null)
			{
				this.title.Term = achievementData.TitleTerm(true);
			}
			if (this.desc != null)
			{
				this.desc.Term = achievementData.DescriptionTerm(true);
			}
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00030B7D File Offset: 0x0002ED7D
	private void OnDestroy()
	{
		GameMaster.OnPrepareLoadingLevel -= this.SelfDestroy;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x00030B90 File Offset: 0x0002ED90
	private IEnumerator Start()
	{
		GameMaster.OnPrepareLoadingLevel += this.SelfDestroy;
		this.audioCollectionPlay.Play();
		yield return new WaitForGameSeconds(this.timeToLive);
		this.SelfDestroy();
		yield break;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00030B9F File Offset: 0x0002ED9F
	private void SelfDestroy()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000A4B RID: 2635
	public Image trophyImage;

	// Token: 0x04000A4C RID: 2636
	public Localize title;

	// Token: 0x04000A4D RID: 2637
	public Localize desc;

	// Token: 0x04000A4E RID: 2638
	public float timeToLive = 5f;

	// Token: 0x04000A4F RID: 2639
	public AchievementsData data;

	// Token: 0x04000A50 RID: 2640
	public PlaySoundFromAudioCollection audioCollectionPlay;
}
