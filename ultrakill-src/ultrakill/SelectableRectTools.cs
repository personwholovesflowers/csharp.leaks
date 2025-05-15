using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003E3 RID: 995
public class SelectableRectTools : MonoBehaviour
{
	// Token: 0x06001674 RID: 5748 RVA: 0x000B4E94 File Offset: 0x000B3094
	private void Awake()
	{
		if (this.target == null)
		{
			this.target = base.GetComponent<Selectable>();
		}
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x000B4EB0 File Offset: 0x000B30B0
	private void OnEnable()
	{
		if (this.autoSwitchForDown)
		{
			foreach (Selectable selectable in this.prioritySwitch)
			{
				if (selectable.gameObject.activeSelf && selectable.IsInteractable() && selectable.enabled)
				{
					this.ChangeSelectOnDown(selectable);
					break;
				}
			}
		}
		if (this.autoSwitchForUp)
		{
			foreach (Selectable selectable2 in this.prioritySwitch)
			{
				if (selectable2.gameObject.activeSelf && selectable2.IsInteractable() && selectable2.enabled)
				{
					this.ChangeSelectOnUp(selectable2);
					return;
				}
			}
		}
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x000B4F4C File Offset: 0x000B314C
	public void ChangeSelectOnUp(Selectable newElement)
	{
		Navigation navigation = this.target.navigation;
		navigation.selectOnUp = newElement;
		this.target.navigation = navigation;
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x000B4F7C File Offset: 0x000B317C
	public void ChangeSelectOnDown(Selectable newElement)
	{
		Navigation navigation = this.target.navigation;
		navigation.selectOnDown = newElement;
		this.target.navigation = navigation;
	}

	// Token: 0x04001F08 RID: 7944
	[SerializeField]
	private Selectable target;

	// Token: 0x04001F09 RID: 7945
	[SerializeField]
	private bool autoSwitchForDown;

	// Token: 0x04001F0A RID: 7946
	[SerializeField]
	private bool autoSwitchForUp;

	// Token: 0x04001F0B RID: 7947
	[SerializeField]
	private Selectable[] prioritySwitch;
}
