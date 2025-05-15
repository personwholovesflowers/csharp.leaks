using System;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x02000202 RID: 514
	public interface ISelectableHolderScreen
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000BE6 RID: 3046
		Selectable DefaultSelectable { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000BE7 RID: 3047
		// (set) Token: 0x06000BE8 RID: 3048
		Selectable LastSelectable { get; set; }
	}
}
