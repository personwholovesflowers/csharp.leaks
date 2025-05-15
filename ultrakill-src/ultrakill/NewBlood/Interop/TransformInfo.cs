using System;
using UnityEngine;

namespace NewBlood.Interop
{
	// Token: 0x020005FB RID: 1531
	public struct TransformInfo
	{
		// Token: 0x04002DE0 RID: 11744
		public Matrix4x4 worldMatrix;

		// Token: 0x04002DE1 RID: 11745
		public Matrix4x4 prevWorldMatrix;

		// Token: 0x04002DE2 RID: 11746
		public Bounds worldAABB;

		// Token: 0x04002DE3 RID: 11747
		public Bounds localAABB;

		// Token: 0x04002DE4 RID: 11748
		public int motionVectorFrameIndex;

		// Token: 0x04002DE5 RID: 11749
		public TransformType transformType;

		// Token: 0x04002DE6 RID: 11750
		public ushort lateLatchIndex;
	}
}
