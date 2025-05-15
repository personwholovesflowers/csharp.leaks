using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x020001EC RID: 492
	public class Minimap : MonoBehaviour
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x00034107 File Offset: 0x00032307
		public void LateUpdate()
		{
			this.map.anchoredPosition = -this.marker.anchoredPosition * this._zoom;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0003412F File Offset: 0x0003232F
		public void ZoomIn()
		{
			this._zoom = this.Clamp(this._zoom + this.zoomStep);
			this.map.localScale = Vector3.one * this._zoom;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00034165 File Offset: 0x00032365
		public void ZoomOut()
		{
			this._zoom = this.Clamp(this._zoom - this.zoomStep);
			this.map.localScale = Vector3.one * this._zoom;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x0003419B File Offset: 0x0003239B
		private float Clamp(float zoom)
		{
			return Mathf.Clamp(zoom, this.minZoom, this.maxZoom);
		}

		// Token: 0x04000B0A RID: 2826
		public RectTransform map;

		// Token: 0x04000B0B RID: 2827
		public RectTransform marker;

		// Token: 0x04000B0C RID: 2828
		[Space]
		public float minZoom = 0.8f;

		// Token: 0x04000B0D RID: 2829
		public float maxZoom = 1.4f;

		// Token: 0x04000B0E RID: 2830
		public float zoomStep = 0.2f;

		// Token: 0x04000B0F RID: 2831
		private float _zoom = 1f;
	}
}
