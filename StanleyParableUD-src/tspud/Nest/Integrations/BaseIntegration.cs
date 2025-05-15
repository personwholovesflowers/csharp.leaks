using System;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x02000240 RID: 576
	public class BaseIntegration : MonoBehaviour
	{
		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003D134 File Offset: 0x0003B334
		// (set) Token: 0x06000D91 RID: 3473 RVA: 0x0003D134 File Offset: 0x0003B334
		public virtual float InputValue
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}
	}
}
