using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class ColorCorrection : HammerEntity
{
	// Token: 0x060003DC RID: 988 RVA: 0x00005444 File Offset: 0x00003644
	private void Awake()
	{
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00018A13 File Offset: 0x00016C13
	public override void Input_Enable()
	{
		base.Input_Enable();
		StanleyController.Instance.StartPostProcessFade(this.lut, 0f, 1f, this.fadeInTime);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00018A3B File Offset: 0x00016C3B
	public override void Input_Disable()
	{
		base.Input_Disable();
		StanleyController.Instance.StartPostProcessFade(this.lut, 1f, 0f, this.fadeOutTime);
	}

	// Token: 0x040003DC RID: 988
	public Texture2D lut;

	// Token: 0x040003DD RID: 989
	public float fadeInTime;

	// Token: 0x040003DE RID: 990
	public float fadeOutTime;
}
