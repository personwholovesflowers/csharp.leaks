using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C0 RID: 448
public class UIGraphicUtility : MonoBehaviour
{
	// Token: 0x06000A5D RID: 2653 RVA: 0x00030BBF File Offset: 0x0002EDBF
	public void SetColor(int colorIndex)
	{
		if (this.colorArray.Length == 0 || this.colorArray.Length <= colorIndex)
		{
			return;
		}
		if (this.graphic != null)
		{
			this.graphic.color = this.colorArray[colorIndex];
		}
	}

	// Token: 0x04000A51 RID: 2641
	[SerializeField]
	private MaskableGraphic graphic;

	// Token: 0x04000A52 RID: 2642
	[SerializeField]
	private Color[] colorArray;
}
