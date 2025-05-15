using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x020001EB RID: 491
	[RequireComponent(typeof(RectTransform))]
	public class MaskPainter : UIBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06000B37 RID: 2871 RVA: 0x00033FD8 File Offset: 0x000321D8
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00033FEC File Offset: 0x000321EC
		protected override void Start()
		{
			base.Start();
			this.spot.enabled = false;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00034000 File Offset: 0x00032200
		public void OnPointerDown(PointerEventData eventData)
		{
			this.UpdateStrokeByEvent(eventData, false);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003400A File Offset: 0x0003220A
		public void OnDrag(PointerEventData eventData)
		{
			this.UpdateStrokeByEvent(eventData, true);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00034014 File Offset: 0x00032214
		private void UpdateStrokeByEvent(PointerEventData eventData, bool drawTrail = false)
		{
			this.UpdateStrokePosition(eventData.position, drawTrail);
			this.UpdateStrokeColor(eventData.button);
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00034030 File Offset: 0x00032230
		private void UpdateStrokePosition(Vector2 screenPosition, bool drawTrail = false)
		{
			Vector2 vector;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rectTransform, screenPosition, null, out vector))
			{
				Vector2 anchoredPosition = this.stroke.anchoredPosition;
				this.stroke.anchoredPosition = vector;
				if (drawTrail)
				{
					this.SetUpTrail(anchoredPosition);
				}
				this.spot.enabled = true;
			}
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0003407C File Offset: 0x0003227C
		private void SetUpTrail(Vector2 prevPosition)
		{
			Vector2 vector = this.stroke.anchoredPosition - prevPosition;
			this.stroke.localRotation = Quaternion.AngleAxis(Mathf.Atan2(vector.y, vector.x) * 57.29578f, Vector3.forward);
			this.spot.rectTransform.offsetMin = new Vector2(-vector.magnitude, 0f);
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x000340E9 File Offset: 0x000322E9
		private void UpdateStrokeColor(PointerEventData.InputButton pressedButton)
		{
			this.spot.materialForRendering.SetInt("_BlendOp", (pressedButton == PointerEventData.InputButton.Left) ? 0 : 2);
		}

		// Token: 0x04000B07 RID: 2823
		public Graphic spot;

		// Token: 0x04000B08 RID: 2824
		public RectTransform stroke;

		// Token: 0x04000B09 RID: 2825
		private RectTransform _rectTransform;
	}
}
