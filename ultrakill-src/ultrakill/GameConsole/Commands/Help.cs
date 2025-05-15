using System;
using System.Collections.Generic;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005CD RID: 1485
	public class Help : ICommand, IConsoleLogger
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06002139 RID: 8505 RVA: 0x00108E94 File Offset: 0x00107094
		public Logger Log { get; } = new Logger("Help");

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x00108E9C File Offset: 0x0010709C
		public string Name
		{
			get
			{
				return "Help";
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600213B RID: 8507 RVA: 0x00108EA3 File Offset: 0x001070A3
		public string Description
		{
			get
			{
				return "Helps you with things, does helpful things, lists things maybe??? Just a helpful pal.";
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x00108EAA File Offset: 0x001070AA
		public string Command
		{
			get
			{
				return "help";
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x00108EB4 File Offset: 0x001070B4
		public void Execute(Console con, string[] args)
		{
			if (args.Length == 0)
			{
				this.Log.Info("Listing recognized commands:", null, null, null);
				foreach (KeyValuePair<string, ICommand> keyValuePair in con.recognizedCommands)
				{
					this.Log.Info("<b>" + keyValuePair.Key + "</b> - " + keyValuePair.Value.Description, null, null, null);
				}
				return;
			}
			if (con.recognizedCommands.ContainsKey(args[0].ToLower()))
			{
				this.Log.Info("<b>" + args[0].ToLower() + "</b> - " + con.recognizedCommands[args[0].ToLower()].Description, null, null, null);
				return;
			}
			this.Log.Info("Command not found.", null, null, null);
		}
	}
}
