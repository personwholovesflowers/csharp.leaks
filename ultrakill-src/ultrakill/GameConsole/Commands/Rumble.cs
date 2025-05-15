using System;
using System.Collections.Generic;
using System.Reflection;
using GameConsole.CommandTree;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005D4 RID: 1492
	public class Rumble : CommandRoot, IConsoleLogger
	{
		// Token: 0x17000290 RID: 656
		// (get) Token: 0x0600217D RID: 8573 RVA: 0x00109E18 File Offset: 0x00108018
		public Logger Log { get; } = new Logger("Rumble");

		// Token: 0x0600217E RID: 8574 RVA: 0x00109E20 File Offset: 0x00108020
		public Rumble(Console con)
			: base(con)
		{
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x00109E39 File Offset: 0x00108039
		public override string Name
		{
			get
			{
				return "Rumble";
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x00109E40 File Offset: 0x00108040
		public override string Description
		{
			get
			{
				return "Command for managing ULTRAKILL's controller rumble system";
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00109E48 File Offset: 0x00108048
		protected override Branch BuildTree(Console con)
		{
			string text = "rumble";
			Node[] array = new Node[6];
			array[0] = CommandRoot.Leaf("status", delegate
			{
				this.Log.Info(string.Format("Pending Vibrations ({0}):", MonoSingleton<RumbleManager>.Instance.pendingVibrations.Count), null, null, null);
				foreach (KeyValuePair<RumbleKey, PendingVibration> keyValuePair in MonoSingleton<RumbleManager>.Instance.pendingVibrations)
				{
					this.Log.Info(string.Format(" - {0} ({1}) for {2} seconds", keyValuePair.Key, keyValuePair.Value.Intensity, keyValuePair.Value.Duration), null, null, null);
				}
				this.Log.Info(string.Empty, null, null, null);
				this.Log.Info(string.Format("Current Intensity: {0}", MonoSingleton<RumbleManager>.Instance.currentIntensity), null, null, null);
			}, false);
			array[1] = CommandRoot.Leaf("list", delegate
			{
				this.Log.Info("Available Keys:", null, null, null);
				PropertyInfo[] properties = typeof(RumbleProperties).GetProperties();
				for (int i = 0; i < properties.Length; i++)
				{
					string text2 = properties[i].GetValue(null) as string;
					this.Log.Info(" - " + text2, null, null, null);
				}
			}, false);
			array[2] = CommandRoot.Leaf<string>("vibrate", delegate(string key)
			{
				MonoSingleton<RumbleManager>.Instance.SetVibration(new RumbleKey(key));
			}, false);
			array[3] = CommandRoot.Leaf<string>("stop", delegate(string key)
			{
				MonoSingleton<RumbleManager>.Instance.StopVibration(new RumbleKey(key));
			}, false);
			array[4] = CommandRoot.Leaf("stop_all", delegate
			{
				MonoSingleton<RumbleManager>.Instance.StopAllVibrations();
			}, false);
			array[5] = CommandRoot.Leaf("toggle_preview", delegate
			{
				DebugUI.previewRumble = !DebugUI.previewRumble;
			}, false);
			return CommandRoot.Branch(text, array);
		}
	}
}
