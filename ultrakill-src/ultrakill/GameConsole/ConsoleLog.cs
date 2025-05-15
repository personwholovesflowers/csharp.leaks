using System;
using plog;
using plog.Models;

namespace GameConsole
{
	// Token: 0x020005AE RID: 1454
	[Serializable]
	public class ConsoleLog
	{
		// Token: 0x060020AB RID: 8363 RVA: 0x00106F68 File Offset: 0x00105168
		public ConsoleLog(Log log, Logger source)
		{
			this.log = log;
			this.timeSinceLogged = 0f;
			this.source = source;
			if (log.Level == Level.Error && log.StackTrace != null)
			{
				this.expanded = true;
			}
		}

		// Token: 0x04002CF2 RID: 11506
		public Log log;

		// Token: 0x04002CF3 RID: 11507
		public Logger source;

		// Token: 0x04002CF4 RID: 11508
		public UnscaledTimeSince timeSinceLogged;

		// Token: 0x04002CF5 RID: 11509
		public bool expanded;
	}
}
