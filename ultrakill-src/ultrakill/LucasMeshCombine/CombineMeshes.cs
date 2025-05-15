using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LucasMeshCombine
{
	// Token: 0x0200057B RID: 1403
	public class CombineMeshes : MonoBehaviour
	{
		// Token: 0x06001FD7 RID: 8151 RVA: 0x00102360 File Offset: 0x00100560
		private void Awake()
		{
			List<MeshCombineData> list = new List<MeshCombineData>();
			foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
			{
				MeshFilter meshFilter;
				if (meshRenderer.gameObject.isStatic && !MeshCombineManager.Instance.ProcessedMeshRenderers.Contains(meshRenderer) && meshRenderer.gameObject.TryGetComponent<MeshFilter>(out meshFilter) && meshRenderer.sharedMaterials.Length == meshFilter.sharedMesh.subMeshCount)
				{
					for (int j = 0; j < meshFilter.sharedMesh.subMeshCount; j++)
					{
						Material material = meshRenderer.sharedMaterials[j];
						if (MeshCombineManager.Instance.AllowedShadersToBatch.Contains(material.shader))
						{
							list.Add(new MeshCombineData(base.gameObject, meshRenderer, meshFilter, (Texture2D)material.mainTexture, j));
						}
					}
				}
			}
			MeshCombineManager.Instance.AddCombineDatas(list);
		}
	}
}
