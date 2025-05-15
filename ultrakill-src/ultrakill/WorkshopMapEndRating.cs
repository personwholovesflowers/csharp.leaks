using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000059 RID: 89
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class WorkshopMapEndRating : MonoSingleton<WorkshopMapEndRating>
{
	// Token: 0x060001B4 RID: 436 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void VoteUp()
	{
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void VoteDown()
	{
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void LeaveAComment()
	{
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void ToggleFavorite()
	{
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00008F4E File Offset: 0x0000714E
	public void JustContinue()
	{
		MonoSingleton<FinalRank>.Instance.LevelChange(true);
	}

	// Token: 0x040001C3 RID: 451
	[SerializeField]
	private GameObject container;

	// Token: 0x040001C4 RID: 452
	[SerializeField]
	private TMP_Text mapName;

	// Token: 0x040001C5 RID: 453
	[SerializeField]
	private Button voteUpButton;

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private GameObject votedUpObject;

	// Token: 0x040001C7 RID: 455
	[SerializeField]
	private Button voteDownButton;

	// Token: 0x040001C8 RID: 456
	[SerializeField]
	private GameObject votedDownObject;

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	private Texture2D placeholderThumbnail;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	private RawImage thumbnail;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	private PersistentColors nameColors;
}
