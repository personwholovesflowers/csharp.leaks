using System;
using System.Linq;

namespace GameConsole.CommandTree
{
	// Token: 0x020005DC RID: 1500
	public class Branch : Node
	{
		// Token: 0x060021A6 RID: 8614 RVA: 0x0010A505 File Offset: 0x00108705
		public Branch(string name, bool requireCheats, params Node[] children)
			: base(requireCheats)
		{
			this.name = name;
			this.children = children;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0010A51C File Offset: 0x0010871C
		public Branch(string name, params Node[] children)
			: base(false)
		{
			this.name = name;
			this.children = children;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0010A534 File Offset: 0x00108734
		public Branch(string name, bool requireCheats = false, params Delegate[] onLeafExecutes)
			: base(requireCheats)
		{
			this.name = name;
			Node[] array = onLeafExecutes.Select((Delegate onExecute) => new Leaf(onExecute, requireCheats)).ToArray<Leaf>();
			this.children = array;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0010A580 File Offset: 0x00108780
		public Branch(string name, params Delegate[] onLeafExecutes)
			: base(false)
		{
			this.name = name;
			Node[] array = onLeafExecutes.Select((Delegate onExecute) => new Leaf(onExecute, false)).ToArray<Leaf>();
			this.children = array;
		}

		// Token: 0x04002D7C RID: 11644
		public readonly string name;

		// Token: 0x04002D7D RID: 11645
		public readonly Node[] children;
	}
}
