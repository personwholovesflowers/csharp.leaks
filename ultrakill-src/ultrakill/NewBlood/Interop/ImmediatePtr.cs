using System;
using System.Runtime.CompilerServices;

namespace NewBlood.Interop
{
	// Token: 0x020005EC RID: 1516
	public struct ImmediatePtr<[IsUnmanaged] T> where T : struct, ValueType
	{
		// Token: 0x04002DBC RID: 11708
		public unsafe T* m_Ptr;
	}
}
