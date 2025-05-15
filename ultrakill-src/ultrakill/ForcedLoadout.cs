using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
[Serializable]
public class ForcedLoadout
{
	// Token: 0x0400012A RID: 298
	public VariantSetting revolver;

	// Token: 0x0400012B RID: 299
	public VariantSetting altRevolver;

	// Token: 0x0400012C RID: 300
	public VariantSetting shotgun;

	// Token: 0x0400012D RID: 301
	public VariantSetting altShotgun;

	// Token: 0x0400012E RID: 302
	public VariantSetting nailgun;

	// Token: 0x0400012F RID: 303
	public VariantSetting altNailgun;

	// Token: 0x04000130 RID: 304
	public VariantSetting railcannon;

	// Token: 0x04000131 RID: 305
	public VariantSetting rocketLauncher;

	// Token: 0x04000132 RID: 306
	[Space]
	public ArmVariantSetting arm;
}
