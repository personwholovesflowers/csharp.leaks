using System;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x0200023D RID: 573
	public class VectorPid
	{
		// Token: 0x06000D84 RID: 3460 RVA: 0x0003CEDD File Offset: 0x0003B0DD
		public VectorPid(float pFactor, float factor, float dFactor)
		{
			this.PFactor = pFactor;
			this.IFactor = factor;
			this.DFactor = dFactor;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0003CEFC File Offset: 0x0003B0FC
		public Vector3 Update(Vector3 currentError, float timeFrame)
		{
			this._integral += currentError * timeFrame;
			Vector3 vector = (currentError - this._lastError) / timeFrame;
			this._lastError = currentError;
			return currentError * this.PFactor + this._integral * this.IFactor + vector * this.DFactor;
		}

		// Token: 0x04000C0F RID: 3087
		public float PFactor;

		// Token: 0x04000C10 RID: 3088
		public float IFactor;

		// Token: 0x04000C11 RID: 3089
		public float DFactor;

		// Token: 0x04000C12 RID: 3090
		private Vector3 _integral;

		// Token: 0x04000C13 RID: 3091
		private Vector3 _lastError;
	}
}
