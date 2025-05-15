using System;

namespace I2.Loc
{
	// Token: 0x020002B1 RID: 689
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x060011D9 RID: 4569 RVA: 0x00061D3D File Offset: 0x0005FF3D
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
