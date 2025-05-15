using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000209 RID: 521
public class ConsoleTitleBar : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000B05 RID: 2821 RVA: 0x0004F9CF File Offset: 0x0004DBCF
	public void OnPointerDown(PointerEventData eventData)
	{
		this.consoleWindow.StartDrag(eventData);
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0004F9DD File Offset: 0x0004DBDD
	public void OnPointerUp(PointerEventData eventData)
	{
		this.consoleWindow.EndDrag(eventData);
	}

	// Token: 0x04000EB1 RID: 3761
	[SerializeField]
	private ConsoleWindow consoleWindow;
}
