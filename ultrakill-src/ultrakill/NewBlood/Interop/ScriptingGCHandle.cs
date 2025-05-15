using System;
using System.Runtime.InteropServices;

namespace NewBlood.Interop
{
	// Token: 0x020005F7 RID: 1527
	public struct ScriptingGCHandle
	{
		// Token: 0x04002DD1 RID: 11729
		public GCHandle m_Handle;

		// Token: 0x04002DD2 RID: 11730
		public ScriptingGCHandleWeakness m_Weakness;

		// Token: 0x04002DD3 RID: 11731
		public unsafe void* m_Object;
	}
}
