using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000121 RID: 289
public class SelectOnSubmit : MonoBehaviour, ISubmitHandler, IEventSystemHandler
{
	// Token: 0x060006E2 RID: 1762 RVA: 0x00024B48 File Offset: 0x00022D48
	public void OnSubmit(BaseEventData eventData)
	{
		if (this.targetSelectable != null)
		{
			this.targetSelectable.Select();
		}
	}

	// Token: 0x0400072A RID: 1834
	[SerializeField]
	private Selectable targetSelectable;
}
