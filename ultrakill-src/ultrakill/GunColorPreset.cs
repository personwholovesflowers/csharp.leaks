using System;
using UnityEngine;

// Token: 0x02000239 RID: 569
[Serializable]
public class GunColorPreset
{
	// Token: 0x06000C3C RID: 3132 RVA: 0x000572DD File Offset: 0x000554DD
	public GunColorPreset(Color a, Color b, Color c)
	{
		this.color1 = a;
		this.color2 = b;
		this.color3 = c;
	}

	// Token: 0x0400100E RID: 4110
	public Color color1;

	// Token: 0x0400100F RID: 4111
	public Color color2;

	// Token: 0x04001010 RID: 4112
	public Color color3;
}
