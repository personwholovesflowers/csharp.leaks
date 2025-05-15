using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x0200018C RID: 396
public class SettingsCharacterSliderEventTrigger : MonoBehaviour, ISelectHandler, IEventSystemHandler, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// Token: 0x0600092A RID: 2346 RVA: 0x0002B412 File Offset: 0x00029612
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.dontInvokeOnPointerEnterWhileDragging && this.dragging)
		{
			return;
		}
		SettingsCharacterSliderEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerEnter;
		if (baseEventDataEvent == null)
		{
			return;
		}
		baseEventDataEvent.Invoke(eventData);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0002B436 File Offset: 0x00029636
	public void OnBeginDrag(PointerEventData eventData)
	{
		this.dragging = true;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00005444 File Offset: 0x00003644
	public void OnDrag(PointerEventData eventData)
	{
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0002B43F File Offset: 0x0002963F
	public void OnEndDrag(PointerEventData eventData)
	{
		this.dragging = false;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0002B448 File Offset: 0x00029648
	public void OnSelect(BaseEventData eventData)
	{
		if (this.dontInvokeOnSelectWhileDragging && this.dragging)
		{
			return;
		}
		SettingsCharacterSliderEventTrigger.BaseEventDataEvent baseEventDataEvent = this.onSelect;
		if (baseEventDataEvent == null)
		{
			return;
		}
		baseEventDataEvent.Invoke(eventData);
	}

	// Token: 0x040008FB RID: 2299
	[SerializeField]
	private SettingsCharacterSliderEventTrigger.BaseEventDataEvent pointerEnter;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private bool dontInvokeOnPointerEnterWhileDragging = true;

	// Token: 0x040008FD RID: 2301
	[Space(30f)]
	[SerializeField]
	private SettingsCharacterSliderEventTrigger.BaseEventDataEvent onSelect;

	// Token: 0x040008FE RID: 2302
	[SerializeField]
	private bool dontInvokeOnSelectWhileDragging = true;

	// Token: 0x040008FF RID: 2303
	private bool dragging;

	// Token: 0x020003E9 RID: 1001
	[Serializable]
	public class BaseEventDataEvent : UnityEvent<BaseEventData>
	{
	}
}
