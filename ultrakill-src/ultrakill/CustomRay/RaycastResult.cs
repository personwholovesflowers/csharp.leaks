using System;
using UnityEngine;

namespace CustomRay
{
	// Token: 0x02000574 RID: 1396
	public class RaycastResult : IComparable<RaycastResult>
	{
		// Token: 0x06001FC4 RID: 8132 RVA: 0x00101EAC File Offset: 0x001000AC
		public RaycastResult(RaycastHit hit)
		{
			this.distance = hit.distance;
			this.transform = hit.transform;
			this.rrhit = hit;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00101ED5 File Offset: 0x001000D5
		public int CompareTo(RaycastResult other)
		{
			return this.distance.CompareTo(other.distance);
		}

		// Token: 0x04002C00 RID: 11264
		public float distance;

		// Token: 0x04002C01 RID: 11265
		public Transform transform;

		// Token: 0x04002C02 RID: 11266
		public RaycastHit rrhit;
	}
}
