using System;
using GameConsole.CommandTree;
using plog;
using UnityEngine.InputSystem;

namespace GameConsole.Commands
{
	// Token: 0x020005CE RID: 1486
	internal class InputCommands : CommandRoot, IConsoleLogger
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600213F RID: 8511 RVA: 0x00108FC8 File Offset: 0x001071C8
		public Logger Log { get; } = new Logger("Input");

		// Token: 0x06002140 RID: 8512 RVA: 0x00108FD0 File Offset: 0x001071D0
		public InputCommands(Console con)
			: base(con)
		{
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06002141 RID: 8513 RVA: 0x00108FE9 File Offset: 0x001071E9
		public override string Name
		{
			get
			{
				return "Input";
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06002142 RID: 8514 RVA: 0x00108FF0 File Offset: 0x001071F0
		public override string Description
		{
			get
			{
				return "Modify inputs";
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00108FF8 File Offset: 0x001071F8
		protected override Branch BuildTree(Console con)
		{
			return CommandRoot.Branch("input", new Node[]
			{
				CommandRoot.Branch("mouse", new Node[] { CommandRoot.Leaf<float>("sensitivity", delegate(float amount)
				{
					this.Log.Info(string.Format("Set mouse sensitivity to {0}", amount), null, null, null);
					MonoSingleton<PrefsManager>.Instance.SetFloatLocal("mouseSensitivity", amount);
				}, false) }),
				CommandRoot.Leaf<string>("bindings", delegate(string name)
				{
					InputAction inputAction = MonoSingleton<InputManager>.Instance.InputSource.Actions.FindAction(name, false);
					if (inputAction == null)
					{
						this.Log.Error("No action found with name or id '" + name + "'", null, null, null);
						return;
					}
					this.Log.Info("'" + name + "' has the following bindings:", null, null, null);
					foreach (InputBinding inputBinding in inputAction.bindings)
					{
						if (inputBinding.isPartOfComposite)
						{
							this.Log.Info("-- " + inputBinding.path, null, null, null);
						}
						else
						{
							this.Log.Info("- " + inputBinding.path, null, null, null);
						}
					}
				}, false)
			});
		}
	}
}
