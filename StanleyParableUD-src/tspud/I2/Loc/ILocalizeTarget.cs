using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002A9 RID: 681
	public abstract class ILocalizeTarget : ScriptableObject
	{
		// Token: 0x060011A2 RID: 4514
		public abstract bool IsValid(Localize cmp);

		// Token: 0x060011A3 RID: 4515
		public abstract void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		// Token: 0x060011A4 RID: 4516
		public abstract void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation);

		// Token: 0x060011A5 RID: 4517
		public abstract bool CanUseSecondaryTerm();

		// Token: 0x060011A6 RID: 4518
		public abstract bool AllowMainTermToBeRTL();

		// Token: 0x060011A7 RID: 4519
		public abstract bool AllowSecondTermToBeRTL();

		// Token: 0x060011A8 RID: 4520
		public abstract eTermType GetPrimaryTermType(Localize cmp);

		// Token: 0x060011A9 RID: 4521
		public abstract eTermType GetSecondaryTermType(Localize cmp);
	}
}
