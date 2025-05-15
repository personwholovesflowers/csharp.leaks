using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class UIHighlightHotFix : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDeselectHandler
{
	// Token: 0x0600072F RID: 1839 RVA: 0x000257BC File Offset: 0x000239BC
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!EventSystem.current.alreadySelecting && Singleton<GameMaster>.Instance.MouseMoved)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
			return;
		}
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x000257E7 File Offset: 0x000239E7
	public void OnDeselect(BaseEventData eventData)
	{
		base.GetComponent<Selectable>().OnPointerExit(null);
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x000257F5 File Offset: 0x000239F5
	public void OnPointerExit(PointerEventData eventData)
	{
		if (!EventSystem.current.alreadySelecting && Singleton<GameMaster>.Instance.MouseMoved)
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
