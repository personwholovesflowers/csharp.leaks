using System;
using UnityEngine;

namespace Polybrush
{
	// Token: 0x0200021F RID: 543
	[ExecuteInEditMode]
	public class z_AdditionalVertexStreams : MonoBehaviour
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0003661A File Offset: 0x0003481A
		private MeshRenderer meshRenderer
		{
			get
			{
				if (this._meshRenderer == null)
				{
					this._meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
				}
				return this._meshRenderer;
			}
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x00036641 File Offset: 0x00034841
		private void Start()
		{
			this.SetAdditionalVertexStreamsMesh(this.m_AdditionalVertexStreamMesh);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x0003664F File Offset: 0x0003484F
		public void SetAdditionalVertexStreamsMesh(Mesh mesh)
		{
			this.m_AdditionalVertexStreamMesh = mesh;
			this.meshRenderer.additionalVertexStreams = mesh;
		}

		// Token: 0x04000BBE RID: 3006
		public Mesh m_AdditionalVertexStreamMesh;

		// Token: 0x04000BBF RID: 3007
		private MeshRenderer _meshRenderer;
	}
}
