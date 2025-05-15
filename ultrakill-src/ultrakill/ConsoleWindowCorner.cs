using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200020B RID: 523
public class ConsoleWindowCorner : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06000B14 RID: 2836 RVA: 0x0004FE3E File Offset: 0x0004E03E
	public void OnPointerDown(PointerEventData eventData)
	{
		this.consoleWindow.StartResize(eventData, this.corner);
		this.dragging = true;
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x0004FE59 File Offset: 0x0004E059
	public void OnPointerUp(PointerEventData eventData)
	{
		this.consoleWindow.StopResize(eventData, this.corner);
		this.dragging = false;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x0004FE74 File Offset: 0x0004E074
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.hovering = true;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0004FE7D File Offset: 0x0004E07D
	public void OnPointerExit(PointerEventData eventData)
	{
		this.hovering = false;
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0004FE86 File Offset: 0x0004E086
	private void Update()
	{
		this.feedbackIcon.SetActive(this.dragging || this.hovering);
	}

	// Token: 0x04000EBA RID: 3770
	[SerializeField]
	private ConsoleWindow consoleWindow;

	// Token: 0x04000EBB RID: 3771
	[SerializeField]
	private GameObject feedbackIcon;

	// Token: 0x04000EBC RID: 3772
	[SerializeField]
	private Vector2Int corner = new Vector2Int(0, 0);

	// Token: 0x04000EBD RID: 3773
	private bool dragging;

	// Token: 0x04000EBE RID: 3774
	private bool hovering;
}
