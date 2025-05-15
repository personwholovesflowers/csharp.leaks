using System;
using GameConsole.CommandTree;

namespace GameConsole.Commands
{
	// Token: 0x020005D1 RID: 1489
	public class Pcon : CommandRoot
	{
		// Token: 0x0600215A RID: 8538 RVA: 0x001095B1 File Offset: 0x001077B1
		public Pcon(Console con)
			: base(con)
		{
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x0600215B RID: 8539 RVA: 0x001095BA File Offset: 0x001077BA
		public override string Name
		{
			get
			{
				return "pcon";
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x001095C1 File Offset: 0x001077C1
		public override string Description
		{
			get
			{
				return "pcon commands";
			}
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x001095C8 File Offset: 0x001077C8
		protected override Branch BuildTree(Console con)
		{
			string text = "pcon";
			Node[] array = new Node[2];
			array[0] = CommandRoot.Leaf("connect", delegate
			{
				MonoSingleton<Console>.Instance.StartPCon();
			}, false);
			array[1] = base.BoolMenu("autostart", () => MonoSingleton<PrefsManager>.Instance.GetBoolLocal("pcon.autostart", false), delegate(bool value)
			{
				MonoSingleton<PrefsManager>.Instance.SetBoolLocal("pcon.autostart", value);
				if (value)
				{
					MonoSingleton<Console>.Instance.StartPCon();
				}
			}, false, false);
			return CommandRoot.Branch(text, array);
		}
	}
}
