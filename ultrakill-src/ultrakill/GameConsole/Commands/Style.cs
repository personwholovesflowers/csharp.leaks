using System;
using GameConsole.CommandTree;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005D8 RID: 1496
	internal class Style : CommandRoot, IConsoleLogger
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x0010A2AC File Offset: 0x001084AC
		public Logger Log { get; } = new Logger("Style");

		// Token: 0x06002197 RID: 8599 RVA: 0x0010A2B4 File Offset: 0x001084B4
		public Style(Console con)
			: base(con)
		{
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x0010A2CD File Offset: 0x001084CD
		public override string Name
		{
			get
			{
				return "Style";
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06002199 RID: 8601 RVA: 0x0010A2D4 File Offset: 0x001084D4
		public override string Description
		{
			get
			{
				return "Modify your style score";
			}
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x0010A2DC File Offset: 0x001084DC
		protected override Branch BuildTree(Console con)
		{
			string text = "style";
			Node[] array = new Node[2];
			array[0] = base.BoolMenu("meter", () => MonoSingleton<StyleHUD>.Instance.forceMeterOn, delegate(bool value)
			{
				MonoSingleton<StyleHUD>.Instance.forceMeterOn = value;
			}, false, true);
			array[1] = CommandRoot.Branch("freshness", new Node[]
			{
				CommandRoot.Leaf("get", delegate
				{
					this.Log.Info(string.Format("Current weapon freshness is {0}", MonoSingleton<StyleHUD>.Instance.GetFreshness(MonoSingleton<GunControl>.Instance.currentWeapon)), null, null, null);
				}, false),
				CommandRoot.Leaf<float>("set", delegate(float amt)
				{
					this.Log.Info(string.Format("Set current weapon freshness to {0}", amt), null, null, null);
					MonoSingleton<StyleHUD>.Instance.SetFreshness(MonoSingleton<GunControl>.Instance.currentWeapon, amt);
				}, true),
				CommandRoot.Leaf<int, StyleFreshnessState>("lock_state", delegate(int slot, StyleFreshnessState state)
				{
					this.Log.Info(string.Format("Locking slot {0} to {1}", slot, Enum.GetName(typeof(StyleFreshnessState), state)), null, null, null);
					MonoSingleton<StyleHUD>.Instance.LockFreshness(slot, new StyleFreshnessState?(state), null);
				}, true),
				CommandRoot.Leaf<int>("unlock", delegate(int slot)
				{
					this.Log.Info(string.Format("Unlocking slot {0}", slot), null, null, null);
					MonoSingleton<StyleHUD>.Instance.UnlockFreshness(slot);
				}, true)
			});
			return CommandRoot.Branch(text, array);
		}
	}
}
