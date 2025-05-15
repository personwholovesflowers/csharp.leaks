using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020003E4 RID: 996
public sealed class SelectionRedirector : Selectable
{
	// Token: 0x06001679 RID: 5753 RVA: 0x000B4FAC File Offset: 0x000B31AC
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.Selectables == null)
		{
			return;
		}
		foreach (Selectable selectable in this.Selectables)
		{
			if (selectable != null && selectable.isActiveAndEnabled)
			{
				base.StartCoroutine(this.SelectAtEndOfFrame(selectable));
				return;
			}
		}
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x000B5002 File Offset: 0x000B3202
	private IEnumerator SelectAtEndOfFrame(Selectable selectable)
	{
		yield return new WaitForEndOfFrame();
		selectable.Select();
		yield break;
	}

	// Token: 0x04001F0C RID: 7948
	public Selectable[] Selectables;
}
