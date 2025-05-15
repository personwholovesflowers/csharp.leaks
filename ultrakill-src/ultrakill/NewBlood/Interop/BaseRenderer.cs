using System;
using System.Runtime.InteropServices;

namespace NewBlood.Interop
{
	// Token: 0x020005E8 RID: 1512
	public struct BaseRenderer
	{
		// Token: 0x04002DB3 RID: 11699
		public unsafe void** __vftable;

		// Token: 0x04002DB4 RID: 11700
		public SharedRendererData m_RendererData;

		// Token: 0x04002DB5 RID: 11701
		public unsafe void* m_RendererProperties;

		// Token: 0x04002DB6 RID: 11702
		[MarshalAs(UnmanagedType.U1)]
		public bool m_ForceRenderingOff;
	}
}
