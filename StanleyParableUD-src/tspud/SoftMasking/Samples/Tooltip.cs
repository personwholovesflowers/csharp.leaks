using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x020001F2 RID: 498
	public class Tooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000B6D RID: 2925 RVA: 0x00034900 File Offset: 0x00032B00
		public void LateUpdate()
		{
			Vector2 vector;
			if (this.tooltip.gameObject.activeInHierarchy && RectTransformUtility.ScreenPointToLocalPointInRectangle(this.tooltip.parent.GetComponent<RectTransform>(), Input.mousePosition, null, out vector))
			{
				this.tooltip.anchoredPosition = vector + new Vector2(10f, -20f);
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00034963 File Offset: 0x00032B63
		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			this.tooltip.gameObject.SetActive(true);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00034976 File Offset: 0x00032B76
		void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
		{
			this.tooltip.gameObject.SetActive(false);
		}

		// Token: 0x04000B26 RID: 2854
		public RectTransform tooltip;
	}
}
