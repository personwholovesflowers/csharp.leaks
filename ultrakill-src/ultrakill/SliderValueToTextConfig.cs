using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000411 RID: 1041
[Serializable]
public class SliderValueToTextConfig
{
	// Token: 0x04002126 RID: 8486
	public DecimalType decimalType;

	// Token: 0x04002127 RID: 8487
	[FormerlySerializedAs("modifier")]
	public float multiplier = 1f;

	// Token: 0x04002128 RID: 8488
	public string suffix;

	// Token: 0x04002129 RID: 8489
	public string ifMax;

	// Token: 0x0400212A RID: 8490
	public string ifMin;

	// Token: 0x0400212B RID: 8491
	public Color minColor;

	// Token: 0x0400212C RID: 8492
	public Color maxColor;
}
