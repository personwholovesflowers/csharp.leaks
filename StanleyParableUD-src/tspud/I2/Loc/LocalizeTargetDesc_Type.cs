using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002AD RID: 685
	public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<G> where T : Object where G : LocalizeTarget<T>
	{
		// Token: 0x060011B4 RID: 4532 RVA: 0x00061499 File Offset: 0x0005F699
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.GetComponent<T>() != null;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x000614AC File Offset: 0x0005F6AC
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			T component = cmp.GetComponent<T>();
			if (component == null)
			{
				return null;
			}
			G g = ScriptableObject.CreateInstance<G>();
			g.mTarget = component;
			return g;
		}
	}
}
