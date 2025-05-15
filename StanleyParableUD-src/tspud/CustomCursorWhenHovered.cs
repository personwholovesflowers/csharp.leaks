using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020000AA RID: 170
public class CustomCursorWhenHovered : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000409 RID: 1033 RVA: 0x0001917F File Offset: 0x0001737F
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.cursor.SetCursorToThis();
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001918C File Offset: 0x0001738C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.cursor.ResetCursor();
	}

	// Token: 0x04000403 RID: 1027
	public CursorProfile cursor;
}
