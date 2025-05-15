using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using plog;

namespace Logic
{
	// Token: 0x0200059E RID: 1438
	public class VarStore
	{
		// Token: 0x06002032 RID: 8242 RVA: 0x0010473E File Offset: 0x0010293E
		public void Clear()
		{
			this.intStore.Clear();
			this.boolStore.Clear();
			this.floatStore.Clear();
			this.stringStore.Clear();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x0010476C File Offset: 0x0010296C
		public VarStore DuplicateStore()
		{
			return new VarStore
			{
				intStore = new Dictionary<string, int>(this.intStore),
				boolStore = new Dictionary<string, bool>(this.boolStore),
				floatStore = new Dictionary<string, float>(this.floatStore),
				stringStore = new Dictionary<string, string>(this.stringStore)
			};
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x001047C4 File Offset: 0x001029C4
		public static VarStore LoadPersistentStore()
		{
			VarStore varStore = new VarStore();
			string text = MapVarSaver.AssembleCurrentFilePath();
			if (!File.Exists(text))
			{
				return null;
			}
			string text2 = File.ReadAllText(text);
			if (string.IsNullOrEmpty(text2))
			{
				return null;
			}
			PersistentSavedStore persistentSavedStore = JsonConvert.DeserializeObject<PersistentSavedStore>(text2);
			if (persistentSavedStore == null)
			{
				return null;
			}
			if (persistentSavedStore.variables == null)
			{
				return null;
			}
			foreach (SavedVariable savedVariable in persistentSavedStore.variables)
			{
				if (((savedVariable != null) ? savedVariable.value : null) != null && !string.IsNullOrEmpty(savedVariable.value.type))
				{
					VarStore.LoadVariable(savedVariable, varStore);
					varStore.persistentKeys.Add(savedVariable.name);
				}
			}
			return varStore;
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00104890 File Offset: 0x00102A90
		public static void LoadVariable(SavedVariable variable, VarStore store)
		{
			VarStore.Log.Info("Loading variable: $" + variable.name, null, null, null);
			if (variable.value.type == "System.String")
			{
				if (variable.value.type == "System.String")
				{
					store.stringStore[variable.value.type] = variable.value.value.ToString();
					return;
				}
			}
			else if (variable.value.type == "System.Boolean")
			{
				if (variable.value.type == "System.Boolean")
				{
					store.boolStore[variable.value.type] = (variable.value.value as bool?).GetValueOrDefault();
					return;
				}
			}
			else if (variable.value.type == "System.Int32")
			{
				if (variable.value.type == "System.Int32")
				{
					store.intStore[variable.value.type] = int.Parse(variable.value.value.ToString());
					return;
				}
			}
			else if (variable.value.type == "System.Single")
			{
				if (variable.value.type == "System.Single")
				{
					store.floatStore[variable.value.type] = float.Parse(variable.value.value.ToString());
					return;
				}
			}
			else
			{
				VarStore.Log.Warning("Unknown variable type: " + variable.value.type + ", on variable: " + variable.name, null, null, null);
			}
		}

		// Token: 0x04002CA0 RID: 11424
		private static readonly Logger Log = new Logger("VarStore");

		// Token: 0x04002CA1 RID: 11425
		public HashSet<string> persistentKeys = new HashSet<string>();

		// Token: 0x04002CA2 RID: 11426
		public Dictionary<string, int> intStore = new Dictionary<string, int>();

		// Token: 0x04002CA3 RID: 11427
		public Dictionary<string, bool> boolStore = new Dictionary<string, bool>();

		// Token: 0x04002CA4 RID: 11428
		public Dictionary<string, float> floatStore = new Dictionary<string, float>();

		// Token: 0x04002CA5 RID: 11429
		public Dictionary<string, string> stringStore = new Dictionary<string, string>();
	}
}
