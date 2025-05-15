using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class SequelNameSelectionRandomizer : MonoBehaviour
{
	// Token: 0x06000902 RID: 2306 RVA: 0x0002AEBC File Offset: 0x000290BC
	[ContextMenu("Randomize")]
	public void Randomize()
	{
		this.RandomizeGroup(this.prefixButtons, this.prefixIndex);
		this.RandomizeGroup(this.postFixButtons, this.postfixIndex);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0002AEE4 File Offset: 0x000290E4
	private void RandomizeGroup(SequelToggleButton[] buttons, IntConfigurable pfindex)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < pfindex.MaxValue; i++)
		{
			list.Add(i);
		}
		int[] array = new int[buttons.Length];
		for (int j = 0; j < buttons.Length; j++)
		{
			int num = Random.Range(0, list.Count);
			array[j] = list[num];
			list.RemoveAt(num);
		}
		for (int k = 0; k < buttons.Length; k++)
		{
			buttons[k].SetIndex(array[k]);
		}
	}

	// Token: 0x040008DF RID: 2271
	public SequelToggleButton[] prefixButtons;

	// Token: 0x040008E0 RID: 2272
	public IntConfigurable prefixIndex;

	// Token: 0x040008E1 RID: 2273
	public SequelToggleButton[] postFixButtons;

	// Token: 0x040008E2 RID: 2274
	public IntConfigurable postfixIndex;
}
