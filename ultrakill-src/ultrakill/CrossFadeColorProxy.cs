using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F3 RID: 243
public class CrossFadeColorProxy : Graphic
{
	// Token: 0x060004C2 RID: 1218 RVA: 0x00020AE4 File Offset: 0x0001ECE4
	public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha, bool useRGB)
	{
		this.setter.CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha, useRGB);
	}

	// Token: 0x04000677 RID: 1655
	public ButtonTextColorSetter setter;
}
