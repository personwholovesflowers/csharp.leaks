using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020002F0 RID: 752
public class MenuChallengeHover : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600108F RID: 4239 RVA: 0x0007EF49 File Offset: 0x0007D149
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.panelToActivate.SetActive(true);
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x0007EF57 File Offset: 0x0007D157
	public void OnPointerExit(PointerEventData eventData)
	{
		this.panelToActivate.SetActive(false);
	}

	// Token: 0x04001687 RID: 5767
	[SerializeField]
	private GameObject panelToActivate;
}
