using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F4 RID: 756
	public class RestoreMesh : MonoBehaviour
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00068ACF File Offset: 0x00066CCF
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x00068AD7 File Offset: 0x00066CD7
		public Mesh OriginalMesh
		{
			get
			{
				return this._originalMesh;
			}
			set
			{
				this._originalMesh = value;
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00068AE0 File Offset: 0x00066CE0
		public void Restore(bool aMaintainColors = true)
		{
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component == null)
			{
				Debug.LogError("No mesh filter to restore to!", base.gameObject);
				return;
			}
			RecolorTree recolorTree = null;
			if (aMaintainColors)
			{
				recolorTree = new RecolorTree(component.sharedMesh, null, true, true, true);
			}
			component.sharedMesh = this._originalMesh;
			if (aMaintainColors)
			{
				ProceduralMeshUtil.EnsureProceduralMesh(component, true);
				Mesh sharedMesh = component.sharedMesh;
				recolorTree.Recolor(ref sharedMesh, null);
			}
		}

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		private Mesh _originalMesh;
	}
}
