using System;

namespace I2.Loc
{
	// Token: 0x020002AB RID: 683
	public abstract class ILocalizeTargetDescriptor
	{
		// Token: 0x060011AD RID: 4525
		public abstract bool CanLocalize(Localize cmp);

		// Token: 0x060011AE RID: 4526
		public abstract ILocalizeTarget CreateTarget(Localize cmp);

		// Token: 0x060011AF RID: 4527
		public abstract Type GetTargetType();

		// Token: 0x04000E65 RID: 3685
		public string Name;

		// Token: 0x04000E66 RID: 3686
		public int Priority;
	}
}
