using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002AC RID: 684
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x060011B1 RID: 4529 RVA: 0x00061479 File Offset: 0x0005F679
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00061485 File Offset: 0x0005F685
		public override Type GetTargetType()
		{
			return typeof(T);
		}
	}
}
