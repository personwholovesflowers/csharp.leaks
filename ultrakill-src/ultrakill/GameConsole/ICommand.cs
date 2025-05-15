using System;

namespace GameConsole
{
	// Token: 0x020005B4 RID: 1460
	public interface ICommand
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060020C5 RID: 8389
		string Name { get; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060020C6 RID: 8390
		string Description { get; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060020C7 RID: 8391
		string Command { get; }

		// Token: 0x060020C8 RID: 8392
		void Execute(Console con, string[] args);
	}
}
