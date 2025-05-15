using System;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x02000243 RID: 579
	public class MirrorLocalTransform : BaseIntegration
	{
		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0003D53B File Offset: 0x0003B73B
		// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x0003D543 File Offset: 0x0003B743
		public override float InputValue
		{
			get
			{
				return base.InputValue;
			}
			set
			{
				base.InputValue = value;
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0003D54C File Offset: 0x0003B74C
		public void Update()
		{
			this.MirroredTransform.localPosition = base.transform.localPosition;
			this.MirroredTransform.localRotation = base.transform.localRotation;
		}

		// Token: 0x04000C29 RID: 3113
		public Transform MirroredTransform;
	}
}
