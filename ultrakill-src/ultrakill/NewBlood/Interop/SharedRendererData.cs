using System;
using UnityEngine;

namespace NewBlood.Interop
{
	// Token: 0x020005F9 RID: 1529
	public struct SharedRendererData
	{
		// Token: 0x04002DD5 RID: 11733
		public TransformInfo m_TransformInfo;

		// Token: 0x04002DD6 RID: 11734
		public StaticBatchInfo m_StaticBatchInfo;

		// Token: 0x04002DD7 RID: 11735
		public GlobalLayeringData m_GlobalLayeringData;

		// Token: 0x04002DD8 RID: 11736
		public Vector4 m_LightmapST_0;

		// Token: 0x04002DD9 RID: 11737
		public Vector4 m_LightmapST_1;

		// Token: 0x04002DDA RID: 11738
		public LightmapIndices m_LightmapIndex;

		// Token: 0x04002DDB RID: 11739
		private uint _bitfield1;

		// Token: 0x04002DDC RID: 11740
		public uint m_RenderingLayerMask;

		// Token: 0x04002DDD RID: 11741
		public int m_RendererPriority;
	}
}
