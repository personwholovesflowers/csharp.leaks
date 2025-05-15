using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000098 RID: 152
public class BrightnessSliderEvent : MonoBehaviour
{
	// Token: 0x060003AE RID: 942 RVA: 0x00018014 File Offset: 0x00016214
	private void Awake()
	{
		Color color = Color.Lerp(this.remappedColorMinimumValue, this.remappedColorMaximumValue, 0.5f);
		this.imageToChange.color = color;
	}

	// Token: 0x060003AF RID: 943 RVA: 0x00018044 File Offset: 0x00016244
	public void ValueChanged(float value)
	{
		Color color = Color.Lerp(this.remappedColorMinimumValue, this.remappedColorMaximumValue, value);
		this.imageToChange.color = color;
	}

	// Token: 0x040003A0 RID: 928
	[SerializeField]
	private Image imageToChange;

	// Token: 0x040003A1 RID: 929
	[SerializeField]
	private Color remappedColorMinimumValue;

	// Token: 0x040003A2 RID: 930
	[SerializeField]
	private Color remappedColorMaximumValue;

	// Token: 0x02000396 RID: 918
	public enum SliderEventTypes
	{
		// Token: 0x04001322 RID: 4898
		ChangeColor,
		// Token: 0x04001323 RID: 4899
		ChangeSprite
	}
}
