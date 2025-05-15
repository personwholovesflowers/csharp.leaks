using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200025D RID: 605
	[Serializable]
	public class GrouperData
	{
		// Token: 0x04000D18 RID: 3352
		public bool clusterOnLMIndex;

		// Token: 0x04000D19 RID: 3353
		public bool clusterByLODLevel;

		// Token: 0x04000D1A RID: 3354
		public Vector3 origin;

		// Token: 0x04000D1B RID: 3355
		public Vector3 cellSize;

		// Token: 0x04000D1C RID: 3356
		public int pieNumSegments = 4;

		// Token: 0x04000D1D RID: 3357
		public Vector3 pieAxis = Vector3.up;

		// Token: 0x04000D1E RID: 3358
		public int height = 1;

		// Token: 0x04000D1F RID: 3359
		public float maxDistBetweenClusters = 1f;

		// Token: 0x04000D20 RID: 3360
		public bool includeCellsWithOnlyOneRenderer = true;
	}
}
