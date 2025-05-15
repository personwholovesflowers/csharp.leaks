using System;

namespace NewBlood.Interop
{
	// Token: 0x020005EF RID: 1519
	public struct MemLabelId
	{
		// Token: 0x060021DC RID: 8668 RVA: 0x0010B23F File Offset: 0x0010943F
		public MemLabelId(MemLabelIdentifier identifier, ushort salt, uint rootReferenceIndex)
		{
			this.identifier = identifier;
		}

		// Token: 0x04002DC0 RID: 11712
		public MemLabelIdentifier identifier;
	}
}
