using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x020001EF RID: 495
	[RequireComponent(typeof(RectTransform))]
	public class ShowOnHover : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x00034742 File Offset: 0x00032942
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0003474A File Offset: 0x0003294A
		public bool forcedVisible
		{
			get
			{
				return this._forcedVisible;
			}
			set
			{
				if (this._forcedVisible != value)
				{
					this._forcedVisible = value;
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00034762 File Offset: 0x00032962
		protected override void Start()
		{
			base.Start();
			this.UpdateVisibility();
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00034770 File Offset: 0x00032970
		private void UpdateVisibility()
		{
			this.SetVisible(this.ShouldBeVisible());
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0003477E File Offset: 0x0003297E
		private bool ShouldBeVisible()
		{
			return this._forcedVisible || this._isPointerOver;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00034790 File Offset: 0x00032990
		private void SetVisible(bool visible)
		{
			if (this.targetGroup)
			{
				this.targetGroup.alpha = (visible ? 1f : 0f);
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000347B9 File Offset: 0x000329B9
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._isPointerOver = true;
			this.UpdateVisibility();
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000347C8 File Offset: 0x000329C8
		public void OnPointerExit(PointerEventData eventData)
		{
			this._isPointerOver = false;
			this.UpdateVisibility();
		}

		// Token: 0x04000B1F RID: 2847
		public CanvasGroup targetGroup;

		// Token: 0x04000B20 RID: 2848
		private bool _forcedVisible;

		// Token: 0x04000B21 RID: 2849
		private bool _isPointerOver;
	}
}
