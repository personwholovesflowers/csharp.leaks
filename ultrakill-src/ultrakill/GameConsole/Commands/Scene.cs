using System;
using plog;
using UnityEngine;

namespace GameConsole.Commands
{
	// Token: 0x020005D6 RID: 1494
	public class Scene : ICommand, IConsoleLogger
	{
		// Token: 0x17000293 RID: 659
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x0010A0FA File Offset: 0x001082FA
		public global::plog.Logger Log { get; } = new global::plog.Logger("Scene");

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x0010A102 File Offset: 0x00108302
		public string Name
		{
			get
			{
				return "Scene";
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x0010A109 File Offset: 0x00108309
		public string Description
		{
			get
			{
				return "Loads a scene.";
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x0010A110 File Offset: 0x00108310
		public string Command
		{
			get
			{
				return "scene";
			}
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x0010A118 File Offset: 0x00108318
		public void Execute(Console con, string[] args)
		{
			if (con.CheatBlocker())
			{
				return;
			}
			if (args.Length == 0)
			{
				this.Log.Info("Usage: scene <scene name>", null, null, null);
				return;
			}
			string text = string.Join(" ", args);
			if (!Debug.isDebugBuild && MonoSingleton<SceneHelper>.Instance.IsSceneSpecial(text))
			{
				this.Log.Info("Scene is special and cannot be loaded in release mode. \ud83e\udd7a", null, null, null);
				return;
			}
			SceneHelper.LoadScene(text, false);
		}
	}
}
