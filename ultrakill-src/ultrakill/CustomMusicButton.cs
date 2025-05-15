using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000161 RID: 353
public class CustomMusicButton : CustomContentButton, IDragHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060006DA RID: 1754 RVA: 0x0002D3A6 File Offset: 0x0002B5A6
	public void OnPointerDown(PointerEventData eventData)
	{
		this.dragPoint = new Vector3?(this.GetScreenPositionInCanvasSpace(eventData.position));
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002D3C4 File Offset: 0x0002B5C4
	public void OnPointerUp(PointerEventData eventData)
	{
		this.dragPoint = null;
		UnityEvent<GameObject, Vector3> unityEvent = this.onDrop;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(base.gameObject, this.GetScreenPositionInCanvasSpace(eventData.position));
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Update()
	{
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0002D3FC File Offset: 0x0002B5FC
	private Vector2 GetScreenPositionInCanvasSpace(Vector2 screenPos)
	{
		Vector2 vector;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTransform, screenPos, MonoSingleton<CameraController>.Instance.cam, out vector);
		return vector;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00004AE3 File Offset: 0x00002CE3
	public void OnDrag(PointerEventData eventData)
	{
	}

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private RectTransform canvasTransform;

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private ControllerPointer pointer;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	private UnityEvent<GameObject, Vector3> onDrag;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private UnityEvent<GameObject, Vector3> onDrop;

	// Token: 0x040008E9 RID: 2281
	private Vector3? dragPoint;
}
