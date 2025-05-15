using System;
using System.Collections.Generic;
using System.Globalization;
using Logic;
using pcon.core.Interfaces;

namespace GameConsole.pcon
{
	// Token: 0x020005BA RID: 1466
	public class MapVarsMessage : ISend
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060020DB RID: 8411 RVA: 0x00107C93 File Offset: 0x00105E93
		public string type
		{
			get
			{
				return "ultrakill.mapvars";
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00107C9C File Offset: 0x00105E9C
		public MapVarsMessage(VarStore store)
		{
			this.clear = true;
			this.variables = new List<MapVarField>();
			foreach (KeyValuePair<string, int> keyValuePair in store.intStore)
			{
				this.variables.Add(new MapVarField
				{
					name = keyValuePair.Key,
					value = new MapVarValue
					{
						value = keyValuePair.Value.ToString(),
						type = typeof(int).FullName
					}
				});
			}
			foreach (KeyValuePair<string, bool> keyValuePair2 in store.boolStore)
			{
				this.variables.Add(new MapVarField
				{
					name = keyValuePair2.Key,
					value = new MapVarValue
					{
						value = keyValuePair2.Value,
						type = typeof(bool).FullName
					}
				});
			}
			foreach (KeyValuePair<string, float> keyValuePair3 in store.floatStore)
			{
				this.variables.Add(new MapVarField
				{
					name = keyValuePair3.Key,
					value = new MapVarValue
					{
						value = keyValuePair3.Value.ToString(CultureInfo.InvariantCulture),
						type = typeof(float).FullName
					}
				});
			}
			foreach (KeyValuePair<string, string> keyValuePair4 in store.stringStore)
			{
				this.variables.Add(new MapVarField
				{
					name = keyValuePair4.Key,
					value = new MapVarValue
					{
						value = keyValuePair4.Value,
						type = typeof(string).FullName
					}
				});
			}
		}

		// Token: 0x04002D24 RID: 11556
		private const string Type = "ultrakill.mapvars";

		// Token: 0x04002D25 RID: 11557
		public List<MapVarField> variables;

		// Token: 0x04002D26 RID: 11558
		public bool clear;
	}
}
