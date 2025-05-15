using System;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005CB RID: 1483
	public class Echo : ICommand, IConsoleLogger
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600212D RID: 8493 RVA: 0x00108DE5 File Offset: 0x00106FE5
		public Logger Log { get; } = new Logger("Echo");

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x00108DED File Offset: 0x00106FED
		public string Name
		{
			get
			{
				return "Echo";
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x00108DF4 File Offset: 0x00106FF4
		public string Description
		{
			get
			{
				return "Echo the given text";
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x00108DFB File Offset: 0x00106FFB
		public string Command
		{
			get
			{
				return "echo";
			}
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x00108E02 File Offset: 0x00107002
		public void Execute(Console con, string[] args)
		{
			this.Log.Info("Echoing: " + string.Join(" ", args), null, null, null);
		}
	}
}
