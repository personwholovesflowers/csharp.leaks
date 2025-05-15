using System;
using UnityEngine;

// Token: 0x02000119 RID: 281
public class MenuScaler : MonoBehaviour
{
	// Token: 0x060006BC RID: 1724 RVA: 0x00023FF8 File Offset: 0x000221F8
	private void Start()
	{
		this.RT = base.GetComponent<RectTransform>();
		this.anchoredPos = this.RT.anchoredPosition3D;
		this.widthHeight = new Vector2(this.RT.rect.width, this.RT.rect.height);
		this.scale = this.RT.localScale;
		this.canvasRT = base.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x00024078 File Offset: 0x00022278
	private void Update()
	{
		this.RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.widthHeight.x * (this.canvasRT.rect.width / 512f));
		this.RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.widthHeight.y * (this.canvasRT.rect.width / 256f));
		Vector3 vector = new Vector3(this.anchoredPos.x * (this.canvasRT.rect.width / 512f), this.anchoredPos.y * (this.canvasRT.rect.height / 256f), 0f);
		this.RT.anchoredPosition3D = vector;
		this.RT.localScale = new Vector3(this.scale.x * (this.canvasRT.rect.width / 512f), this.scale.y * (this.canvasRT.rect.height / 256f), 1f);
	}

	// Token: 0x0400070A RID: 1802
	private RectTransform RT;

	// Token: 0x0400070B RID: 1803
	private Vector3 anchoredPos;

	// Token: 0x0400070C RID: 1804
	private Vector2 widthHeight;

	// Token: 0x0400070D RID: 1805
	private Vector3 scale;

	// Token: 0x0400070E RID: 1806
	private RectTransform canvasRT;
}
