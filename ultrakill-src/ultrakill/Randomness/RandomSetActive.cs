using System;

namespace Randomness
{
	// Token: 0x0200057A RID: 1402
	public class RandomSetActive : RandomBase<RandomGameObjectEntry>
	{
		// Token: 0x06001FD4 RID: 8148 RVA: 0x001022F7 File Offset: 0x001004F7
		public override void PerformTheAction(RandomEntry entry)
		{
			RandomGameObjectEntry randomGameObjectEntry = (RandomGameObjectEntry)entry;
			if (randomGameObjectEntry == null)
			{
				return;
			}
			randomGameObjectEntry.targetObject.SetActive(true);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x00102310 File Offset: 0x00100510
		public override void RandomizeWithCount(int count)
		{
			if (this.resetStatesOnRandomize)
			{
				RandomGameObjectEntry[] entries = this.entries;
				for (int i = 0; i < entries.Length; i++)
				{
					entries[i].targetObject.SetActive(false);
				}
			}
			base.RandomizeWithCount(count);
		}

		// Token: 0x04002C14 RID: 11284
		public bool resetStatesOnRandomize = true;
	}
}
