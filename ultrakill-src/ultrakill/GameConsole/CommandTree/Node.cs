using System;

namespace GameConsole.CommandTree
{
	// Token: 0x020005DA RID: 1498
	public abstract class Node
	{
		// Token: 0x060021A3 RID: 8611 RVA: 0x0010A4D6 File Offset: 0x001086D6
		protected Node(bool requireCheats = false)
		{
			this.requireCheats = requireCheats;
		}

		// Token: 0x04002D7A RID: 11642
		public readonly bool requireCheats;
	}
}
