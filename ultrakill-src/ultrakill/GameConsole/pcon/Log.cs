using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using pcon.core.Interfaces;

namespace GameConsole.pcon
{
	// Token: 0x020005BE RID: 1470
	public class Log : ISend
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x0010811E File Offset: 0x0010631E
		public string type
		{
			get
			{
				return "pcon.log";
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00108128 File Offset: 0x00106328
		private void ComputeHash()
		{
			string text = this.message;
			int num;
			if (text == null)
			{
				string text2 = this.stacktrace;
				num = (0 ^ ((text2 != null) ? new int?(text2.GetHashCode()) : null)) ?? (0 ^ this.level.GetHashCode());
			}
			else
			{
				num = text.GetHashCode();
			}
			this.hash = num;
		}

		// Token: 0x04002D2E RID: 11566
		private const string Type = "pcon.log";

		// Token: 0x04002D2F RID: 11567
		public string message;

		// Token: 0x04002D30 RID: 11568
		public string stacktrace;

		// Token: 0x04002D31 RID: 11569
		[JsonConverter(typeof(StringEnumConverter), new object[] { typeof(CamelCaseNamingStrategy) })]
		public PConLogLevel level;

		// Token: 0x04002D32 RID: 11570
		public long timestamp;

		// Token: 0x04002D33 RID: 11571
		public IEnumerable<int> tags;

		// Token: 0x04002D34 RID: 11572
		public int hash;
	}
}
