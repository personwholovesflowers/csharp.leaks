using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x020001E7 RID: 487
	[RequireComponent(typeof(RectTransform))]
	public class Draggable : UIBehaviour, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x00033D7D File Offset: 0x00031F7D
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00033D91 File Offset: 0x00031F91
		public void OnDrag(PointerEventData eventData)
		{
			this._rectTransform.anchoredPosition += eventData.delta;
		}

		// Token: 0x04000AF9 RID: 2809
		private RectTransform _rectTransform;
	}
}
