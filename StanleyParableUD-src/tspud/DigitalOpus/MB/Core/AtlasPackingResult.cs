using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200027A RID: 634
	public class AtlasPackingResult
	{
		// Token: 0x04000D6E RID: 3438
		public int atlasX;

		// Token: 0x04000D6F RID: 3439
		public int atlasY;

		// Token: 0x04000D70 RID: 3440
		public int usedW;

		// Token: 0x04000D71 RID: 3441
		public int usedH;

		// Token: 0x04000D72 RID: 3442
		public Rect[] rects;

		// Token: 0x04000D73 RID: 3443
		public int[] srcImgIdxs;

		// Token: 0x04000D74 RID: 3444
		public object data;
	}
}
