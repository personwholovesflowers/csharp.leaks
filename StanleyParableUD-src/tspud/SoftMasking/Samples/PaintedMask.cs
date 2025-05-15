using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x020001ED RID: 493
	[RequireComponent(typeof(RectTransform))]
	public class PaintedMask : UIBehaviour
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x000341E4 File Offset: 0x000323E4
		protected override void Start()
		{
			base.Start();
			this._renderTexture = new RenderTexture((int)this.maskSize.x, (int)this.maskSize.y, 0, RenderTextureFormat.ARGB32);
			this._renderTexture.Create();
			this.renderCamera.targetTexture = this._renderTexture;
			this.targetMask.renderTexture = this._renderTexture;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000B46 RID: 2886 RVA: 0x0003424C File Offset: 0x0003244C
		private Vector2 maskSize
		{
			get
			{
				return ((RectTransform)this.targetMask.transform).rect.size;
			}
		}

		// Token: 0x04000B10 RID: 2832
		public Camera renderCamera;

		// Token: 0x04000B11 RID: 2833
		public SoftMask targetMask;

		// Token: 0x04000B12 RID: 2834
		private RenderTexture _renderTexture;
	}
}
