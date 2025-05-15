using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x020001E8 RID: 488
	[RequireComponent(typeof(Camera))]
	public class HorizontalFovSetter : MonoBehaviour
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x00033DB7 File Offset: 0x00031FB7
		public void Awake()
		{
			this._camera = base.GetComponent<Camera>();
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00033DC5 File Offset: 0x00031FC5
		public void Update()
		{
			this._camera.fieldOfView = this.horizontalFov / this._camera.aspect;
		}

		// Token: 0x04000AFA RID: 2810
		public float horizontalFov;

		// Token: 0x04000AFB RID: 2811
		private Camera _camera;
	}
}
