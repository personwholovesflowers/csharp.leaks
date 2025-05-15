using System;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class NoBatchLimboWalls : MonoBehaviour
{
	// Token: 0x06001D2A RID: 7466 RVA: 0x000F4AA4 File Offset: 0x000F2CA4
	private void Start()
	{
		MeshRenderer[] array = Object.FindObjectsOfType<MeshRenderer>(true);
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			MeshRenderer meshRenderer = array[i];
			foreach (Material material in array[i].sharedMaterials)
			{
				if (material != null && material.IsKeywordEnabled("LIMBO_WALLS") && material.GetFloat("_OffAxisPanelFeature") == 1f)
				{
					meshRenderer.GetPropertyBlock(materialPropertyBlock);
					materialPropertyBlock.SetInteger("_BatchingID", num);
					meshRenderer.SetPropertyBlock(materialPropertyBlock);
					num++;
					break;
				}
			}
		}
	}
}
