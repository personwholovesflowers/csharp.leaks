using System;
using System.Collections.Generic;
using GameConsole.CommandTree;
using Logic;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005CF RID: 1487
	public class MapVar : CommandRoot, IConsoleLogger
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x0010917C File Offset: 0x0010737C
		public Logger Log { get; } = new Logger("MapVar");

		// Token: 0x06002147 RID: 8519 RVA: 0x00109184 File Offset: 0x00107384
		public MapVar(Console con)
			: base(con)
		{
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x0010919D File Offset: 0x0010739D
		public override string Name
		{
			get
			{
				return "MapVar";
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x001091A4 File Offset: 0x001073A4
		public override string Description
		{
			get
			{
				return "Map variables";
			}
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x001091AC File Offset: 0x001073AC
		protected override Branch BuildTree(Console con)
		{
			string text = "mapvar";
			Node[] array = new Node[11];
			array[0] = CommandRoot.Leaf("reset", delegate
			{
				MonoSingleton<MapVarManager>.Instance.ResetStores();
				this.Log.Info("Stores have been reset.", null, null, null);
			}, true);
			array[1] = CommandRoot.Leaf("stash_info", delegate
			{
				bool hasStashedStore = MonoSingleton<MapVarManager>.Instance.HasStashedStore;
				this.Log.Info("Stash exists: " + hasStashedStore.ToString(), null, null, null);
			}, true);
			array[2] = CommandRoot.Leaf("stash_stores", delegate
			{
				MonoSingleton<MapVarManager>.Instance.StashStore();
				this.Log.Info("Stores have been stashed.", null, null, null);
			}, true);
			array[3] = CommandRoot.Leaf("restore_stash", delegate
			{
				MonoSingleton<MapVarManager>.Instance.RestoreStashedStore();
				this.Log.Info("Stores have been restored.", null, null, null);
			}, true);
			array[4] = CommandRoot.Leaf("list", delegate
			{
				List<VariableSnapshot> allVariables = MonoSingleton<MapVarManager>.Instance.GetAllVariables();
				foreach (VariableSnapshot variableSnapshot in allVariables)
				{
					this.Log.Info(string.Format("{0} ({1}) - <color=orange>{2}</color>", variableSnapshot.name, MapVar.GetFriendlyTypeName(variableSnapshot.type), variableSnapshot.value), null, null, null);
				}
				if (allVariables.Count == 0)
				{
					this.Log.Info("No map variables have been set.", null, null, null);
				}
			}, true);
			array[5] = base.BoolMenu("logging", () => MapVarManager.LoggingEnabled, delegate(bool value)
			{
				MapVarManager.LoggingEnabled = value;
			}, false, true);
			array[6] = CommandRoot.Leaf<string, int>("set_int", delegate(string variableName, int value)
			{
				MonoSingleton<MapVarManager>.Instance.SetInt(variableName, value, false);
			}, true);
			array[7] = CommandRoot.Leaf<string, bool>("set_bool", delegate(string variableName, bool value)
			{
				MonoSingleton<MapVarManager>.Instance.SetBool(variableName, value, false);
			}, true);
			array[8] = CommandRoot.Leaf<string>("toggle_bool", delegate(string variableName)
			{
				MonoSingleton<MapVarManager>.Instance.SetBool(variableName, !MonoSingleton<MapVarManager>.Instance.GetBool(variableName).GetValueOrDefault(), false);
			}, true);
			array[9] = CommandRoot.Leaf<string, float>("set_float", delegate(string variableName, float value)
			{
				MonoSingleton<MapVarManager>.Instance.SetFloat(variableName, value, false);
			}, true);
			array[10] = CommandRoot.Leaf<string, string>("set_string", delegate(string variableName, string value)
			{
				MonoSingleton<MapVarManager>.Instance.SetString(variableName, value, false);
			}, true);
			return CommandRoot.Branch(text, array);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x00109380 File Offset: 0x00107580
		public static string GetFriendlyTypeName(Type type)
		{
			if (type == typeof(int))
			{
				return "int";
			}
			if (type == typeof(float))
			{
				return "float";
			}
			if (type == typeof(string))
			{
				return "string";
			}
			if (type == typeof(bool))
			{
				return "bool";
			}
			return type.Name;
		}
	}
}
