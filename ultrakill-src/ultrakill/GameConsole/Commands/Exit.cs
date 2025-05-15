using System;
using plog;
using UnityEngine;

namespace GameConsole.Commands
{
	// Token: 0x020005CC RID: 1484
	public class Exit : ICommand, IConsoleLogger
	{
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x00108E3F File Offset: 0x0010703F
		public global::plog.Logger Log { get; } = new global::plog.Logger("Exit");

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06002134 RID: 8500 RVA: 0x00108E47 File Offset: 0x00107047
		public string Name
		{
			get
			{
				return "Exit";
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06002135 RID: 8501 RVA: 0x00108E4E File Offset: 0x0010704E
		public string Description
		{
			get
			{
				return "Quits the game.";
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x00108E55 File Offset: 0x00107055
		public string Command
		{
			get
			{
				return this.Name.ToLower();
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00108E62 File Offset: 0x00107062
		public void Execute(Console con, string[] args)
		{
			this.Log.Info("Goodbye \ud83d\udc4b", null, null, null);
			Application.Quit();
		}
	}
}
