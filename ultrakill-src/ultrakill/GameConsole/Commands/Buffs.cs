using System;
using GameConsole.CommandTree;
using plog;
using UnityEngine;

namespace GameConsole.Commands
{
	// Token: 0x020005C0 RID: 1472
	internal class Buffs : CommandRoot, IConsoleLogger
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x001081B4 File Offset: 0x001063B4
		public global::plog.Logger Log { get; } = new global::plog.Logger("Buffs");

		// Token: 0x060020E7 RID: 8423 RVA: 0x001081BC File Offset: 0x001063BC
		public Buffs(Console con)
			: base(con)
		{
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x001081D5 File Offset: 0x001063D5
		public override string Name
		{
			get
			{
				return "Buffs";
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060020E9 RID: 8425 RVA: 0x001081DC File Offset: 0x001063DC
		public override string Description
		{
			get
			{
				return "Modify buffs for enemies";
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x001081E4 File Offset: 0x001063E4
		protected override Branch BuildTree(Console con)
		{
			string text = "buffs";
			Node[] array = new Node[5];
			array[0] = base.BoolMenu("forceradiance", () => OptionsManager.forceRadiance, delegate(bool value)
			{
				OptionsManager.forceRadiance = value;
				EnemyIdentifier[] array2 = Object.FindObjectsOfType<EnemyIdentifier>();
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].UpdateBuffs(false, true);
				}
			}, false, true);
			array[1] = base.BoolMenu("forcesand", () => OptionsManager.forceSand, delegate(bool value)
			{
				OptionsManager.forceSand = value;
				EnemyIdentifier[] array3 = Object.FindObjectsOfType<EnemyIdentifier>();
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j].Sandify(false);
				}
			}, false, true);
			array[2] = base.BoolMenu("forcepuppet", () => OptionsManager.forcePuppet, delegate(bool value)
			{
				OptionsManager.forcePuppet = value;
				EnemyIdentifier[] array4 = Object.FindObjectsOfType<EnemyIdentifier>();
				for (int k = 0; k < array4.Length; k++)
				{
					array4[k].PuppetSpawn();
				}
			}, false, true);
			array[3] = base.BoolMenu("forcebossbars", () => OptionsManager.forceBossBars, delegate(bool value)
			{
				OptionsManager.forceBossBars = value;
				EnemyIdentifier[] array5 = Object.FindObjectsOfType<EnemyIdentifier>();
				for (int l = 0; l < array5.Length; l++)
				{
					array5[l].BossBar(value);
				}
			}, false, true);
			array[4] = CommandRoot.Branch("radiancetier", new Node[]
			{
				CommandRoot.Leaf("get", delegate
				{
					this.Log.Info(string.Format("Current radiance tier is {0}", OptionsManager.radianceTier), null, null, null);
				}, false),
				CommandRoot.Leaf<float>("set", delegate(float amt)
				{
					this.Log.Info(string.Format("Set current radiance tier to {0}", amt), null, null, null);
					OptionsManager.radianceTier = amt;
					EnemyIdentifier[] array6 = Object.FindObjectsOfType<EnemyIdentifier>();
					for (int m = 0; m < array6.Length; m++)
					{
						array6[m].UpdateBuffs(false, true);
					}
				}, true)
			});
			return CommandRoot.Branch(text, array);
		}
	}
}
