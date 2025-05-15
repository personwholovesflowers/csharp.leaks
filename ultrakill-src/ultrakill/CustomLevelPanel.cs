using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000052 RID: 82
public class CustomLevelPanel : MonoBehaviour
{
	// Token: 0x060001A2 RID: 418 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void SetDetailsTab(int tabIndex)
	{
	}

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private PersistentColors nameColors;

	// Token: 0x04000199 RID: 409
	[Space]
	[SerializeField]
	private TMP_Text title;

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private RawImage thumbnail;

	// Token: 0x0400019B RID: 411
	[Space]
	[SerializeField]
	private GameObject workshopInfoContainer;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private Button likeButton;

	// Token: 0x0400019D RID: 413
	[SerializeField]
	private TMP_Text likeCount;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	private Image likeIcon;

	// Token: 0x0400019F RID: 415
	[SerializeField]
	private Button subscribeButton;

	// Token: 0x040001A0 RID: 416
	[SerializeField]
	private Sprite subscribeSprite;

	// Token: 0x040001A1 RID: 417
	[SerializeField]
	private Sprite subscribedSprite;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private Image subscribeIcon;

	// Token: 0x040001A3 RID: 419
	[SerializeField]
	private Button dislikeButton;

	// Token: 0x040001A4 RID: 420
	[SerializeField]
	private TMP_Text dislikeCount;

	// Token: 0x040001A5 RID: 421
	[SerializeField]
	private Image dislikeIcon;

	// Token: 0x040001A6 RID: 422
	[Space]
	[SerializeField]
	private GameObject downloadArrowIcon;

	// Token: 0x040001A7 RID: 423
	[Space]
	[SerializeField]
	public HudOpenEffect detailsOpenEffect;

	// Token: 0x040001A8 RID: 424
	[SerializeField]
	private TMP_Text description;

	// Token: 0x040001A9 RID: 425
	[SerializeField]
	private CustomLevelStats stats;

	// Token: 0x040001AA RID: 426
	[Space]
	[SerializeField]
	private Image descriptionButton;

	// Token: 0x040001AB RID: 427
	[SerializeField]
	private Image statsButton;

	// Token: 0x040001AC RID: 428
	[SerializeField]
	private TMP_Text descriptionButtonLabel;

	// Token: 0x040001AD RID: 429
	[SerializeField]
	private TMP_Text statsButtonLabel;

	// Token: 0x040001AE RID: 430
	[Space]
	[SerializeField]
	private Button selfButton;

	// Token: 0x040001AF RID: 431
	[NonSerialized]
	public Vector2? originalDetailsSize;

	// Token: 0x040001B0 RID: 432
	private string uniqueId;
}
