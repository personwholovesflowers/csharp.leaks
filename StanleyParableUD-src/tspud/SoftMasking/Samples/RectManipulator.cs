using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x020001EE RID: 494
	[RequireComponent(typeof(RectTransform))]
	public class RectManipulator : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x00034276 File Offset: 0x00032476
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.HighlightIcon(true, false);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00034280 File Offset: 0x00032480
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this._isManipulatedNow)
			{
				this.HighlightIcon(false, false);
			}
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00034294 File Offset: 0x00032494
		private void HighlightIcon(bool highlight, bool instant = false)
		{
			if (this.icon)
			{
				float num = (highlight ? this.selectedAlpha : this.normalAlpha);
				float num2 = (instant ? 0f : this.transitionDuration);
				this.icon.CrossFadeAlpha(num, num2, true);
			}
			if (this.showOnHover)
			{
				this.showOnHover.forcedVisible = highlight;
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x000342F8 File Offset: 0x000324F8
		protected override void Start()
		{
			base.Start();
			this.HighlightIcon(false, true);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00034308 File Offset: 0x00032508
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._isManipulatedNow = true;
			this.RememberStartTransform();
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00034318 File Offset: 0x00032518
		private void RememberStartTransform()
		{
			if (this.targetTransform)
			{
				this._startAnchoredPosition = this.targetTransform.anchoredPosition;
				this._startSizeDelta = this.targetTransform.sizeDelta;
				this._startRotation = this.targetTransform.localRotation.eulerAngles.z;
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00034374 File Offset: 0x00032574
		public void OnDrag(PointerEventData eventData)
		{
			if (this.targetTransform == null || this.parentTransform == null || !this._isManipulatedNow)
			{
				return;
			}
			Vector2 vector = this.ToParentSpace(eventData.pressPosition, eventData.pressEventCamera);
			Vector2 vector2 = this.ToParentSpace(eventData.position, eventData.pressEventCamera);
			this.DoRotate(vector, vector2);
			Vector2 vector3 = vector2 - vector;
			this.DoMove(vector3);
			this.DoResize(vector3);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000343EC File Offset: 0x000325EC
		private Vector2 ToParentSpace(Vector2 position, Camera eventCamera)
		{
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentTransform, position, eventCamera, out vector);
			return vector;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0003440A File Offset: 0x0003260A
		private RectTransform parentTransform
		{
			get
			{
				return this.targetTransform.parent as RectTransform;
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003441C File Offset: 0x0003261C
		private void DoMove(Vector2 parentSpaceMovement)
		{
			if (this.Is(RectManipulator.ManipulationType.Move))
			{
				this.MoveTo(this._startAnchoredPosition + parentSpaceMovement);
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00034439 File Offset: 0x00032639
		private bool Is(RectManipulator.ManipulationType expected)
		{
			return (this.manipulation & expected) == expected;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00034446 File Offset: 0x00032646
		private void MoveTo(Vector2 desiredAnchoredPosition)
		{
			this.targetTransform.anchoredPosition = this.ClampPosition(desiredAnchoredPosition);
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003445C File Offset: 0x0003265C
		private Vector2 ClampPosition(Vector2 position)
		{
			Vector2 vector = this.parentTransform.rect.size / 2f;
			return new Vector2(Mathf.Clamp(position.x, -vector.x, vector.x), Mathf.Clamp(position.y, -vector.y, vector.y));
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000344BC File Offset: 0x000326BC
		private void DoRotate(Vector2 startParentPoint, Vector2 targetParentPoint)
		{
			if (this.Is(RectManipulator.ManipulationType.Rotate))
			{
				Vector2 vector = startParentPoint - this.targetTransform.localPosition;
				Vector2 vector2 = targetParentPoint - this.targetTransform.localPosition;
				float num = this.DeltaRotation(vector, vector2);
				this.targetTransform.localRotation = Quaternion.AngleAxis(this._startRotation + num, Vector3.forward);
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00034528 File Offset: 0x00032728
		private float DeltaRotation(Vector2 startLever, Vector2 endLever)
		{
			float num = Mathf.Atan2(startLever.y, startLever.x) * 57.29578f;
			float num2 = Mathf.Atan2(endLever.y, endLever.x) * 57.29578f;
			return Mathf.DeltaAngle(num, num2);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0003456C File Offset: 0x0003276C
		private void DoResize(Vector2 parentSpaceMovement)
		{
			Vector3 vector = Quaternion.Inverse(this.targetTransform.rotation) * parentSpaceMovement;
			Vector2 vector2 = this.ProjectResizeOffset(vector);
			if (vector2.sqrMagnitude > 0f)
			{
				this.SetSizeDirected(vector2, this.ResizeSign());
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000345C0 File Offset: 0x000327C0
		private Vector2 ProjectResizeOffset(Vector2 localOffset)
		{
			bool flag = this.Is(RectManipulator.ManipulationType.ResizeLeft) || this.Is(RectManipulator.ManipulationType.ResizeRight);
			bool flag2 = this.Is(RectManipulator.ManipulationType.ResizeUp) || this.Is(RectManipulator.ManipulationType.ResizeDown);
			return new Vector2(flag ? localOffset.x : 0f, flag2 ? localOffset.y : 0f);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00034619 File Offset: 0x00032819
		private Vector2 ResizeSign()
		{
			return new Vector2(this.Is(RectManipulator.ManipulationType.ResizeLeft) ? (-1f) : 1f, this.Is(RectManipulator.ManipulationType.ResizeDown) ? (-1f) : 1f);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0003464C File Offset: 0x0003284C
		private void SetSizeDirected(Vector2 localOffset, Vector2 sizeSign)
		{
			Vector2 vector = this.ClampSize(this._startSizeDelta + Vector2.Scale(localOffset, sizeSign));
			this.targetTransform.sizeDelta = vector;
			Vector2 vector2 = Vector2.Scale((vector - this._startSizeDelta) / 2f, sizeSign);
			this.MoveTo(this._startAnchoredPosition + this.targetTransform.TransformVector(vector2));
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x000346C2 File Offset: 0x000328C2
		private Vector2 ClampSize(Vector2 size)
		{
			return new Vector2(Mathf.Max(size.x, this.minSize.x), Mathf.Max(size.y, this.minSize.y));
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000346F5 File Offset: 0x000328F5
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isManipulatedNow = false;
			if (!eventData.hovered.Contains(base.gameObject))
			{
				this.HighlightIcon(false, false);
			}
		}

		// Token: 0x04000B13 RID: 2835
		public RectTransform targetTransform;

		// Token: 0x04000B14 RID: 2836
		public RectManipulator.ManipulationType manipulation;

		// Token: 0x04000B15 RID: 2837
		public ShowOnHover showOnHover;

		// Token: 0x04000B16 RID: 2838
		[Header("Limits")]
		public Vector2 minSize;

		// Token: 0x04000B17 RID: 2839
		[Header("Display")]
		public Graphic icon;

		// Token: 0x04000B18 RID: 2840
		public float normalAlpha = 0.2f;

		// Token: 0x04000B19 RID: 2841
		public float selectedAlpha = 1f;

		// Token: 0x04000B1A RID: 2842
		public float transitionDuration = 0.2f;

		// Token: 0x04000B1B RID: 2843
		private bool _isManipulatedNow;

		// Token: 0x04000B1C RID: 2844
		private Vector2 _startAnchoredPosition;

		// Token: 0x04000B1D RID: 2845
		private Vector2 _startSizeDelta;

		// Token: 0x04000B1E RID: 2846
		private float _startRotation;

		// Token: 0x02000413 RID: 1043
		[Flags]
		public enum ManipulationType
		{
			// Token: 0x0400153B RID: 5435
			None = 0,
			// Token: 0x0400153C RID: 5436
			Move = 1,
			// Token: 0x0400153D RID: 5437
			ResizeLeft = 2,
			// Token: 0x0400153E RID: 5438
			ResizeUp = 4,
			// Token: 0x0400153F RID: 5439
			ResizeRight = 8,
			// Token: 0x04001540 RID: 5440
			ResizeDown = 16,
			// Token: 0x04001541 RID: 5441
			ResizeUpLeft = 6,
			// Token: 0x04001542 RID: 5442
			ResizeUpRight = 12,
			// Token: 0x04001543 RID: 5443
			ResizeDownLeft = 18,
			// Token: 0x04001544 RID: 5444
			ResizeDownRight = 24,
			// Token: 0x04001545 RID: 5445
			Rotate = 32
		}
	}
}
