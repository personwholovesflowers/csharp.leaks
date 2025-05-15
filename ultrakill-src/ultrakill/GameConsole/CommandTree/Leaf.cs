using System;

namespace GameConsole.CommandTree
{
	// Token: 0x020005DB RID: 1499
	public class Leaf : Node
	{
		// Token: 0x060021A4 RID: 8612 RVA: 0x0010A4E5 File Offset: 0x001086E5
		public Leaf(Delegate onExecute, bool requireCheats)
			: base(requireCheats)
		{
			this.onExecute = onExecute;
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x0010A4F5 File Offset: 0x001086F5
		public Leaf(Delegate onExecute)
			: base(false)
		{
			this.onExecute = onExecute;
		}

		// Token: 0x04002D7B RID: 11643
		public readonly Delegate onExecute;
	}
}
