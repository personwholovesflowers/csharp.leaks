using System;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class FitToCanvas : MonoBehaviour
{
	// Token: 0x060004E7 RID: 1255 RVA: 0x0001C9D9 File Offset: 0x0001ABD9
	private void Awake()
	{
		this.RT = base.GetComponent<RectTransform>();
		this.canvasRT = base.transform.parent.GetComponent<RectTransform>();
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0001CA00 File Offset: 0x0001AC00
	private void Update()
	{
		this.RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.canvasRT.rect.width);
		this.RT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.canvasRT.rect.height);
	}

	// Token: 0x040004C2 RID: 1218
	private RectTransform RT;

	// Token: 0x040004C3 RID: 1219
	private RectTransform canvasRT;
}
