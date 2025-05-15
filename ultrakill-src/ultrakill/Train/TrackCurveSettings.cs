using System;
using UnityEngine;

namespace Train
{
	// Token: 0x02000524 RID: 1316
	[Serializable]
	public class TrackCurveSettings
	{
		// Token: 0x04002A6A RID: 10858
		[HideInInspector]
		public PathInterpolation curve;

		// Token: 0x04002A6B RID: 10859
		[HideInInspector]
		public Transform handle;

		// Token: 0x04002A6C RID: 10860
		[HideInInspector]
		[Range(1f, 90f)]
		public float angle = 90f;

		// Token: 0x04002A6D RID: 10861
		[HideInInspector]
		public bool flipCurve;
	}
}
