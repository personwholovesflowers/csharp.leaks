using System;
using System.Text;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000276 RID: 630
	public class ObjectLog
	{
		// Token: 0x06000ECB RID: 3787 RVA: 0x00048250 File Offset: 0x00046450
		private void _CacheLogMessage(string msg)
		{
			if (this.logMessages.Length == 0)
			{
				return;
			}
			this.logMessages[this.pos] = msg;
			this.pos++;
			if (this.pos >= this.logMessages.Length)
			{
				this.pos = 0;
			}
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x0004828F File Offset: 0x0004648F
		public ObjectLog(short bufferSize)
		{
			this.logMessages = new string[(int)bufferSize];
		}

		// Token: 0x06000ECD RID: 3789 RVA: 0x000482A3 File Offset: 0x000464A3
		public void Log(MB2_LogLevel l, string msg, MB2_LogLevel currentThreshold)
		{
			MB2_Log.Log(l, msg, currentThreshold);
			this._CacheLogMessage(msg);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x000482B4 File Offset: 0x000464B4
		public void Error(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Error(msg, args));
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x000482C3 File Offset: 0x000464C3
		public void Warn(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Warn(msg, args));
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x000482D2 File Offset: 0x000464D2
		public void Info(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Info(msg, args));
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x000482E1 File Offset: 0x000464E1
		public void LogDebug(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.LogDebug(msg, args));
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x000482F0 File Offset: 0x000464F0
		public void Trace(string msg, params object[] args)
		{
			this._CacheLogMessage(MB2_Log.Trace(msg, args));
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00048300 File Offset: 0x00046500
		public string Dump()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			if (this.logMessages[this.logMessages.Length - 1] != null)
			{
				num = this.pos;
			}
			for (int i = 0; i < this.logMessages.Length; i++)
			{
				int num2 = (num + i) % this.logMessages.Length;
				if (this.logMessages[num2] == null)
				{
					break;
				}
				stringBuilder.AppendLine(this.logMessages[num2]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000D67 RID: 3431
		private int pos;

		// Token: 0x04000D68 RID: 3432
		private string[] logMessages;
	}
}
