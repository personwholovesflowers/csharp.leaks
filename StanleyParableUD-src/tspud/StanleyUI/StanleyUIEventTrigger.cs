using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace StanleyUI
{
	// Token: 0x02000208 RID: 520
	public class StanleyUIEventTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IMoveHandler, ISelectHandler
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x000360B7 File Offset: 0x000342B7
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.OnPointerEnterOnlyInvokesOnMouseMove || !Singleton<GameMaster>.Instance.MouseMoved || eventData.dragging)
			{
				return;
			}
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerEnter;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x000360E7 File Offset: 0x000342E7
		public void OnPointerExit(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerExit;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x000360FA File Offset: 0x000342FA
		public void OnPointerClick(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerClick;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003610D File Offset: 0x0003430D
		public void OnMove(AxisEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.move;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00036120 File Offset: 0x00034320
		public void OnSelect(BaseEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.select;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00036133 File Offset: 0x00034333
		public void OnDeselect(BaseEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.deselect;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00036146 File Offset: 0x00034346
		public void OnPointerDown(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerDown;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00036159 File Offset: 0x00034359
		public void OnPointerUp(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.pointerUp;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003616C File Offset: 0x0003436C
		public void OnDrop(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.drop;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0003617F File Offset: 0x0003437F
		public void OnEndDrag(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.endDrag;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00036192 File Offset: 0x00034392
		public void OnBeginDrag(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.beginDrag;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x000361A5 File Offset: 0x000343A5
		public void OnDrag(PointerEventData eventData)
		{
			StanleyUIEventTrigger.BaseEventDataEvent baseEventDataEvent = this.drag;
			if (baseEventDataEvent == null)
			{
				return;
			}
			baseEventDataEvent.Invoke(eventData);
		}

		// Token: 0x04000B66 RID: 2918
		[SerializeField]
		private bool OnPointerEnterOnlyInvokesOnMouseMove = true;

		// Token: 0x04000B67 RID: 2919
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent pointerEnter;

		// Token: 0x04000B68 RID: 2920
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent pointerExit;

		// Token: 0x04000B69 RID: 2921
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent pointerClick;

		// Token: 0x04000B6A RID: 2922
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent pointerDown;

		// Token: 0x04000B6B RID: 2923
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent pointerUp;

		// Token: 0x04000B6C RID: 2924
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent drop;

		// Token: 0x04000B6D RID: 2925
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent beginDrag;

		// Token: 0x04000B6E RID: 2926
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent drag;

		// Token: 0x04000B6F RID: 2927
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent endDrag;

		// Token: 0x04000B70 RID: 2928
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent move;

		// Token: 0x04000B71 RID: 2929
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent select;

		// Token: 0x04000B72 RID: 2930
		[SerializeField]
		private StanleyUIEventTrigger.BaseEventDataEvent deselect;

		// Token: 0x0200041F RID: 1055
		[Serializable]
		public class BaseEventDataEvent : UnityEvent<BaseEventData>
		{
		}
	}
}
