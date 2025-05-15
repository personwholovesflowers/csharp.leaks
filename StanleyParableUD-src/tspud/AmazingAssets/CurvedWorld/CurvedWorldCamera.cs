using System;
using UnityEngine;

namespace AmazingAssets.CurvedWorld
{
	// Token: 0x0200033D RID: 829
	[ExecuteAlways]
	[RequireComponent(typeof(Camera))]
	public class CurvedWorldCamera : MonoBehaviour
	{
		// Token: 0x06001550 RID: 5456 RVA: 0x00070FF6 File Offset: 0x0006F1F6
		private void OnEnable()
		{
			if (this.activeCamera == null)
			{
				this.activeCamera = base.GetComponent<Camera>();
			}
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00071012 File Offset: 0x0006F212
		private void OnDisable()
		{
			if (this.activeCamera != null)
			{
				this.activeCamera.ResetCullingMatrix();
			}
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00070FF6 File Offset: 0x0006F1F6
		private void Start()
		{
			if (this.activeCamera == null)
			{
				this.activeCamera = base.GetComponent<Camera>();
			}
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00071030 File Offset: 0x0006F230
		private void Update()
		{
			if (this.activeCamera == null)
			{
				this.activeCamera = base.GetComponent<Camera>();
			}
			if (this.activeCamera == null)
			{
				base.enabled = false;
				return;
			}
			if (this.nearClipPlane >= this.activeCamera.farClipPlane)
			{
				this.nearClipPlane = this.activeCamera.farClipPlane - 0.01f;
			}
			if (this.matrixType == CurvedWorldCamera.MATRIX_TYPE.Perspective)
			{
				this.fieldOfView = Mathf.Clamp(this.fieldOfView, 1f, 179f);
				this.activeCamera.cullingMatrix = Matrix4x4.Perspective(this.fieldOfView, 1f, this.activeCamera.nearClipPlane, this.activeCamera.farClipPlane) * this.activeCamera.worldToCameraMatrix;
				return;
			}
			this.size = ((this.size < 1f) ? 1f : this.size);
			this.activeCamera.cullingMatrix = Matrix4x4.Ortho(-this.size, this.size, -this.size, this.size, this.nearClipPlaneSameAsCamera ? this.activeCamera.nearClipPlane : this.nearClipPlane, this.activeCamera.farClipPlane) * this.activeCamera.worldToCameraMatrix;
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x0007117C File Offset: 0x0006F37C
		private void Reset()
		{
			if (this.activeCamera != null)
			{
				this.activeCamera.ResetCullingMatrix();
				this.fieldOfView = this.activeCamera.fieldOfView;
				this.size = this.activeCamera.orthographicSize;
				this.nearClipPlane = this.activeCamera.nearClipPlane;
			}
		}

		// Token: 0x04001165 RID: 4453
		public CurvedWorldCamera.MATRIX_TYPE matrixType;

		// Token: 0x04001166 RID: 4454
		[Range(1f, 179f)]
		public float fieldOfView = 60f;

		// Token: 0x04001167 RID: 4455
		public float size = 5f;

		// Token: 0x04001168 RID: 4456
		public bool nearClipPlaneSameAsCamera = true;

		// Token: 0x04001169 RID: 4457
		public float nearClipPlane = 0.3f;

		// Token: 0x0400116A RID: 4458
		private Camera activeCamera;

		// Token: 0x020004C2 RID: 1218
		public enum MATRIX_TYPE
		{
			// Token: 0x040017DF RID: 6111
			Perspective,
			// Token: 0x040017E0 RID: 6112
			Orthographic
		}
	}
}
