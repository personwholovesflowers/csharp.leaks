using System;

namespace GameConsole.Commands
{
	// Token: 0x020005C2 RID: 1474
	public class Clear : ICommand
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x001084E0 File Offset: 0x001066E0
		public string Name
		{
			get
			{
				return "Clear";
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x001084E7 File Offset: 0x001066E7
		public string Description
		{
			get
			{
				return "Clears the console.";
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x001084EE File Offset: 0x001066EE
		public string Command
		{
			get
			{
				return "clear";
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x001084F5 File Offset: 0x001066F5
		public void Execute(Console con, string[] args)
		{
			con.Clear();
		}
	}
}
