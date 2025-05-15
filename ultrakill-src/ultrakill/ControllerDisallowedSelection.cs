using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DC RID: 220
public class ControllerDisallowedSelection : MonoBehaviour
{
	// Token: 0x0600044C RID: 1100 RVA: 0x0001DCF4 File Offset: 0x0001BEF4
	public void ApplyFallbackSelection()
	{
		if (this.fallbackSelectable != null)
		{
			this.fallbackSelectable.Select();
		}
	}

	// Token: 0x040005E8 RID: 1512
	public Selectable fallbackSelectable;
}
