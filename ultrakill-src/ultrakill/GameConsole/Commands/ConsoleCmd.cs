using System;
using System.Collections.Generic;
using System.Linq;
using GameConsole.CommandTree;
using plog;
using UnityEngine.InputSystem;

namespace GameConsole.Commands
{
	// Token: 0x020005C3 RID: 1475
	public class ConsoleCmd : CommandRoot, IConsoleLogger
	{
		// Token: 0x060020FC RID: 8444 RVA: 0x001084FD File Offset: 0x001066FD
		public ConsoleCmd(Console con)
			: base(con)
		{
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x00108516 File Offset: 0x00106716
		public Logger Log { get; } = new Logger("ConsoleCmd");

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x0010851E File Offset: 0x0010671E
		public override string Name
		{
			get
			{
				return "Console";
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00108525 File Offset: 0x00106725
		public override string Description
		{
			get
			{
				return "Used for configuring the console";
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x0010852C File Offset: 0x0010672C
		protected override Branch BuildTree(Console con)
		{
			string text = "console";
			Node[] array = new Node[5];
			array[0] = base.BoolMenu("hide_badge", () => con.errorBadge.hidden, delegate(bool value)
			{
				con.errorBadge.SetEnabled(value, true);
			}, false, false);
			array[1] = base.BoolMenu("force_stacktrace_extraction", () => con.ExtractStackTraces, new Action<bool>(con.SetForceStackTraceExtraction), false, false);
			array[2] = CommandRoot.Leaf<string, string>("change_bind", delegate(string bind, string key)
			{
				if (con.binds.defaultBinds.ContainsKey(bind.ToLower()))
				{
					con.binds.Rebind(bind.ToLower(), key);
					return;
				}
				this.Log.Error(bind.ToLower() + " is not a valid bind.", null, null, null);
				this.Log.Info("Listing valid binds:", null, null, null);
				this.ListDefaults(con);
			}, true);
			array[3] = CommandRoot.Leaf("list_binds", delegate
			{
				this.Log.Info("Listing binds:", null, null, null);
				foreach (KeyValuePair<string, InputActionState> keyValuePair in con.binds.registeredBinds)
				{
					this.Log.Info(keyValuePair.Key + "  -  " + keyValuePair.Value.Action.bindings.First<InputBinding>().path, null, null, null);
				}
			}, false);
			array[4] = CommandRoot.Leaf("reset", delegate
			{
				MonoSingleton<Console>.Instance.consoleWindow.ResetWindow();
			}, false);
			return CommandRoot.Branch(text, array);
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00108614 File Offset: 0x00106814
		private void ListDefaults(Console con)
		{
			foreach (KeyValuePair<string, string> keyValuePair in con.binds.defaultBinds)
			{
				this.Log.Info(keyValuePair.Key + "  -  " + keyValuePair.Value, null, null, null);
			}
		}
	}
}
