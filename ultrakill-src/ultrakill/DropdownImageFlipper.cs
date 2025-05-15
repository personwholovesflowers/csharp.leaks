using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003E9 RID: 1001
public class DropdownImageFlipper : MonoBehaviour
{
	// Token: 0x06001689 RID: 5769 RVA: 0x000B5285 File Offset: 0x000B3485
	private void Awake()
	{
		if (!this.targetImage)
		{
			this.targetImage = base.GetComponent<Image>();
		}
		this.rect = base.GetComponent<RectTransform>();
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x000B52AC File Offset: 0x000B34AC
	private void Update()
	{
		if (this.rect.pivot.y < 0.5f)
		{
			this.targetImage.sprite = this.flippedSprite;
			float y = this.targetImage.rectTransform.offsetMin.y;
			this.targetImage.rectTransform.offsetMin = new Vector2(this.targetImage.rectTransform.offsetMin.x, -this.targetImage.rectTransform.offsetMax.y);
			this.targetImage.rectTransform.offsetMax = new Vector2(this.targetImage.rectTransform.offsetMax.x, y);
			base.enabled = false;
		}
	}

	// Token: 0x04001F21 RID: 7969
	[SerializeField]
	private Image targetImage;

	// Token: 0x04001F22 RID: 7970
	[SerializeField]
	private Sprite flippedSprite;

	// Token: 0x04001F23 RID: 7971
	private RectTransform rect;
}
