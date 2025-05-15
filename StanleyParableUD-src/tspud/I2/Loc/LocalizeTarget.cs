using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002AA RID: 682
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : Object
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x000613E8 File Offset: 0x0005F5E8
		public override bool IsValid(Localize cmp)
		{
			if (this.mTarget != null)
			{
				Component component = this.mTarget as Component;
				if (component != null && component.gameObject != cmp.gameObject)
				{
					this.mTarget = default(T);
				}
			}
			if (this.mTarget == null)
			{
				this.mTarget = cmp.GetComponent<T>();
			}
			return this.mTarget != null;
		}

		// Token: 0x04000E64 RID: 3684
		public T mTarget;
	}
}
