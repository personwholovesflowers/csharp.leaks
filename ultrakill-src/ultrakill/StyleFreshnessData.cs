using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200045C RID: 1116
[Serializable]
public class StyleFreshnessData
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06001994 RID: 6548 RVA: 0x000D27FF File Offset: 0x000D09FF
	public float span
	{
		get
		{
			return Mathf.Abs(this.max - this.min);
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06001995 RID: 6549 RVA: 0x000D2813 File Offset: 0x000D0A13
	public float justAboveMin
	{
		get
		{
			return this.min + this.span * 0.05f;
		}
	}

	// Token: 0x040023D4 RID: 9172
	public StyleFreshnessState state;

	// Token: 0x040023D5 RID: 9173
	public string text;

	// Token: 0x040023D6 RID: 9174
	public float scoreMultiplier;

	// Token: 0x040023D7 RID: 9175
	public float min;

	// Token: 0x040023D8 RID: 9176
	public float max;

	// Token: 0x040023D9 RID: 9177
	public Slider slider;
}
