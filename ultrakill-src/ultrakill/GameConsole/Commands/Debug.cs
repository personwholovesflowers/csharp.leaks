using System;
using System.Collections;
using System.Collections.Generic;
using GameConsole.CommandTree;
using plog;
using plog.Models;
using UnityEngine;

namespace GameConsole.Commands
{
	// Token: 0x020005C6 RID: 1478
	public class Debug : CommandRoot, IConsoleLogger
	{
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600210B RID: 8459 RVA: 0x00108824 File Offset: 0x00106A24
		public global::plog.Logger Log { get; } = new global::plog.Logger("Debug");

		// Token: 0x0600210C RID: 8460 RVA: 0x0010882C File Offset: 0x00106A2C
		public Debug(Console con)
			: base(con)
		{
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600210D RID: 8461 RVA: 0x00108845 File Offset: 0x00106A45
		public override string Name
		{
			get
			{
				return "Debug";
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x0010884C File Offset: 0x00106A4C
		public override string Description
		{
			get
			{
				return "Console debug stuff.";
			}
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x00108854 File Offset: 0x00106A54
		protected override Branch BuildTree(Console con)
		{
			string text2 = "debug";
			Node[] array = new Node[11];
			array[0] = CommandRoot.Leaf<string>("burst_print", delegate(string count)
			{
				con.StartCoroutine(this.BurstPrint(con, int.Parse(count), Level.Info));
			}, false);
			array[1] = CommandRoot.Leaf<string>("bulk_print", delegate(string count)
			{
				int num = int.Parse(count);
				for (int i = 0; i < num; i++)
				{
					this.Log.Info("Bulk print " + i.ToString(), null, null, null);
				}
			}, false);
			array[2] = CommandRoot.Leaf<string>("print_logger_test", delegate(string count)
			{
				int num2 = int.Parse(count);
				for (int j = 0; j < num2; j++)
				{
					new global::plog.Logger(Guid.NewGuid().ToString()).Info("Bulk print " + j.ToString(), null, null, null);
				}
			}, false);
			array[3] = CommandRoot.Leaf("toggle_overlay", delegate
			{
				Debug.AgonyDebugOverlay = !Debug.AgonyDebugOverlay;
				this.Log.Info("AgonyDebugOverlay: " + Debug.AgonyDebugOverlay.ToString(), null, null, null);
			}, false);
			array[4] = CommandRoot.Leaf("error", delegate
			{
				throw new Exception("Umm, ermm, guuh!!");
			}, false);
			array[5] = CommandRoot.Leaf<string>("log", delegate(string text)
			{
				Debug.Log(text);
			}, false);
			array[6] = CommandRoot.Leaf<string>("freeze_game", delegate(string confrm)
			{
				if (!(confrm == "pretty_please"))
				{
					this.Log.Info("Usage: freeze_game pretty_please", null, null, null);
					return;
				}
				goto IL_000D;
				for (;;)
				{
					IL_000D:
					goto IL_000D;
				}
			}, false);
			array[7] = CommandRoot.Leaf<string>("timescale", delegate(string timescale)
			{
				Time.timeScale = float.Parse(timescale);
			}, true);
			array[8] = CommandRoot.Leaf("die_respawn", delegate
			{
				this.Log.Info("Killing and immediately respawning player...", null, null, null);
				bool paused = MonoSingleton<OptionsManager>.Instance.paused;
				if (paused)
				{
					MonoSingleton<OptionsManager>.Instance.UnPause();
				}
				con.StartCoroutine(this.KillRespawnDelayed(paused));
			}, true);
			array[9] = CommandRoot.Leaf("total_secrets", delegate
			{
				this.Log.Info(GameProgressSaver.GetTotalSecretsFound().ToString(), null, null, null);
			}, false);
			array[10] = CommandRoot.Leaf("auto_register", delegate
			{
				this.Log.Info("Attempting to auto register all commands...", null, null, null);
				List<ICommand> list = new List<ICommand>();
				foreach (Type type in typeof(ICommand).Assembly.GetTypes())
				{
					if (!con.registeredCommandTypes.Contains(type) && typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface)
					{
						list.Add((ICommand)Activator.CreateInstance(type));
					}
				}
				con.RegisterCommands(list);
			}, false);
			return CommandRoot.Branch(text2, array);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x001089F2 File Offset: 0x00106BF2
		private IEnumerator BurstPrint(Console console, int count, Level type)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				this.Log.Record("Hello World " + i.ToString(), type, null, null, null);
				yield return new WaitForSecondsRealtime(3f / (float)count);
				num = i;
			}
			yield break;
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x00108A0F File Offset: 0x00106C0F
		private IEnumerator KillRespawnDelayed(bool wasPaused)
		{
			yield return new WaitForEndOfFrame();
			MonoSingleton<NewMovement>.Instance.GetHurt(999999, false, 1f, false, true, 0.35f, false);
			MonoSingleton<StatsManager>.Instance.Restart();
			if (wasPaused)
			{
				MonoSingleton<OptionsManager>.Instance.Pause();
			}
			yield break;
		}

		// Token: 0x04002D4B RID: 11595
		public static bool AgonyDebugOverlay = true;
	}
}
