using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002E0 RID: 736
	public class ClampAttribute : PropertyAttribute
	{
		// Token: 0x0600132E RID: 4910 RVA: 0x00066345 File Offset: 0x00064545
		public ClampAttribute(float aMin, float aMax)
		{
			this.mMin = aMin;
			this.mMax = aMax;
		}

		// Token: 0x04000F0E RID: 3854
		public float mMin;

		// Token: 0x04000F0F RID: 3855
		public float mMax;
	}
}
