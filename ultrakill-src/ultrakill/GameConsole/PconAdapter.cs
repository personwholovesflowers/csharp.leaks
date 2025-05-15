using System;
using System.Reflection;
using GameConsole.pcon;
using pcon.core;
using pcon.core.Models;
using plog;

namespace GameConsole
{
	// Token: 0x020005B7 RID: 1463
	public class PconAdapter
	{
		// Token: 0x060020D2 RID: 8402 RVA: 0x00107AF4 File Offset: 0x00105CF4
		public bool PConLibraryExists()
		{
			if (this.pconAssmebly != null)
			{
				return true;
			}
			PconAdapter.Log.Info("Looking for the pcon.unity library...", null, null, null);
			string text = "pcon.unity";
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (assembly.FullName.StartsWith(text))
				{
					PconAdapter.Log.Info("Found the pcon.unity library!", null, null, null);
					this.pconAssmebly = assembly;
					this.pconClientType = this.pconAssmebly.GetType("pcon.PConClient");
					return true;
				}
			}
			return false;
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00107B88 File Offset: 0x00105D88
		public void StartPConClient(Action<string> onExecute, Action onGameModified)
		{
			if (!this.PConLibraryExists())
			{
				return;
			}
			if (this.startCalled)
			{
				return;
			}
			PconAdapter.Log.Info("Starting the pcon.unity client...", null, null, null);
			this.startCalled = true;
			MethodInfo method = this.pconClientType.GetMethod("StartClient", BindingFlags.Static | BindingFlags.Public);
			if (method != null)
			{
				PconAdapter.Log.Info("Starting the pcon.unity client!", null, null, null);
				PCon.MountHandler(new Handler
				{
					onExecute = onExecute,
					onGameModified = onGameModified
				});
				method.Invoke(null, new object[1]);
				MonoSingleton<MapVarRelay>.Instance.enabled = true;
				PCon.RegisterFeature("ultrakill");
				return;
			}
			PconAdapter.Log.Info("Could not find the pcon.unity client's StartClient method!", null, null, null);
		}

		// Token: 0x04002D20 RID: 11552
		private static readonly Logger Log = new Logger("PconAdapter");

		// Token: 0x04002D21 RID: 11553
		private Assembly pconAssmebly;

		// Token: 0x04002D22 RID: 11554
		private Type pconClientType;

		// Token: 0x04002D23 RID: 11555
		private bool startCalled;
	}
}
