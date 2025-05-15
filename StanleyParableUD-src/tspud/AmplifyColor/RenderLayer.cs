using System;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x0200031B RID: 795
	[Serializable]
	public struct RenderLayer
	{
		// Token: 0x06001420 RID: 5152 RVA: 0x0006B9E1 File Offset: 0x00069BE1
		public RenderLayer(LayerMask mask, Color color)
		{
			this.mask = mask;
			this.color = color;
		}

		// Token: 0x0400103A RID: 4154
		public LayerMask mask;

		// Token: 0x0400103B RID: 4155
		public Color color;
	}
}
