using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200006A RID: 106
public abstract class MB3_MeshBakerCommon : MB3_MeshBakerRoot
{
	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000274 RID: 628
	public abstract MB3_MeshCombiner meshCombiner { get; }

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000275 RID: 629 RVA: 0x00010CEC File Offset: 0x0000EEEC
	// (set) Token: 0x06000276 RID: 630 RVA: 0x00010CF9 File Offset: 0x0000EEF9
	public override MB2_TextureBakeResults textureBakeResults
	{
		get
		{
			return this.meshCombiner.textureBakeResults;
		}
		set
		{
			this.meshCombiner.textureBakeResults = value;
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00010D08 File Offset: 0x0000EF08
	public override List<GameObject> GetObjectsToCombine()
	{
		if (!this.useObjsToMeshFromTexBaker)
		{
			if (this.objsToMesh == null)
			{
				this.objsToMesh = new List<GameObject>();
			}
			return this.objsToMesh;
		}
		MB3_TextureBaker mb3_TextureBaker = base.gameObject.GetComponent<MB3_TextureBaker>();
		if (mb3_TextureBaker == null)
		{
			mb3_TextureBaker = base.gameObject.transform.parent.GetComponent<MB3_TextureBaker>();
		}
		if (mb3_TextureBaker != null)
		{
			return mb3_TextureBaker.GetObjectsToCombine();
		}
		Debug.LogWarning("Use Objects To Mesh From Texture Baker was checked but no texture baker");
		return new List<GameObject>();
	}

	// Token: 0x06000278 RID: 632 RVA: 0x00010D84 File Offset: 0x0000EF84
	public void EnableDisableSourceObjectRenderers(bool show)
	{
		for (int i = 0; i < this.GetObjectsToCombine().Count; i++)
		{
			GameObject gameObject = this.GetObjectsToCombine()[i];
			if (gameObject != null)
			{
				Renderer renderer = MB_Utility.GetRenderer(gameObject);
				if (renderer != null)
				{
					renderer.enabled = show;
				}
				LODGroup componentInParent = renderer.GetComponentInParent<LODGroup>();
				if (componentInParent != null)
				{
					bool flag = true;
					LOD[] lods = componentInParent.GetLODs();
					for (int j = 0; j < lods.Length; j++)
					{
						for (int k = 0; k < lods[j].renderers.Length; k++)
						{
							if (lods[j].renderers[k] != renderer)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						componentInParent.enabled = show;
					}
				}
			}
		}
	}

	// Token: 0x06000279 RID: 633 RVA: 0x00010E53 File Offset: 0x0000F053
	public virtual void ClearMesh()
	{
		this.meshCombiner.ClearMesh();
	}

	// Token: 0x0600027A RID: 634 RVA: 0x00010E60 File Offset: 0x0000F060
	public virtual void DestroyMesh()
	{
		this.meshCombiner.DestroyMesh();
	}

	// Token: 0x0600027B RID: 635 RVA: 0x00010E6D File Offset: 0x0000F06D
	public virtual void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods)
	{
		this.meshCombiner.DestroyMeshEditor(editorMethods);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x00010E7B File Offset: 0x0000F07B
	public virtual int GetNumObjectsInCombined()
	{
		return this.meshCombiner.GetNumObjectsInCombined();
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00010E88 File Offset: 0x0000F088
	public virtual int GetNumVerticesFor(GameObject go)
	{
		return this.meshCombiner.GetNumVerticesFor(go);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00010E98 File Offset: 0x0000F098
	public MB3_TextureBaker GetTextureBaker()
	{
		MB3_TextureBaker component = base.GetComponent<MB3_TextureBaker>();
		if (component != null)
		{
			return component;
		}
		if (base.transform.parent != null)
		{
			return base.transform.parent.GetComponent<MB3_TextureBaker>();
		}
		return null;
	}

	// Token: 0x0600027F RID: 639
	public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

	// Token: 0x06000280 RID: 640
	public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource = true);

	// Token: 0x06000281 RID: 641 RVA: 0x00010EDC File Offset: 0x0000F0DC
	public virtual void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.Apply(uv2GenerationMethod);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00010F08 File Offset: 0x0000F108
	public virtual void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.Apply(triangles, vertices, normals, tangents, uvs, uv2, uv3, uv4, colors, bones, blendShapesFlag, uv2GenerationMethod);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00010F50 File Offset: 0x0000F150
	public virtual bool CombinedMeshContains(GameObject go)
	{
		return this.meshCombiner.CombinedMeshContains(go);
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00010F60 File Offset: 0x0000F160
	public virtual void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV1 = false, bool updateUV2 = false, bool updateColors = false, bool updateSkinningInfo = false)
	{
		this.meshCombiner.name = base.name + "-mesh";
		this.meshCombiner.UpdateGameObjects(gos, recalcBounds, updateVertices, updateNormals, updateTangents, updateUV, updateUV1, updateUV2, updateColors, updateSkinningInfo, false);
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00010FA5 File Offset: 0x0000F1A5
	public virtual void UpdateSkinnedMeshApproximateBounds()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBounds();
		}
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00010FBA File Offset: 0x0000F1BA
	public virtual void UpdateSkinnedMeshApproximateBoundsFromBones()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBoundsFromBones();
		}
	}

	// Token: 0x06000287 RID: 647 RVA: 0x00010FCF File Offset: 0x0000F1CF
	public virtual void UpdateSkinnedMeshApproximateBoundsFromBounds()
	{
		if (this._ValidateForUpdateSkinnedMeshBounds())
		{
			this.meshCombiner.UpdateSkinnedMeshApproximateBoundsFromBounds();
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00010FE4 File Offset: 0x0000F1E4
	protected virtual bool _ValidateForUpdateSkinnedMeshBounds()
	{
		if (this.meshCombiner.outputOption == MB2_OutputOptions.bakeMeshAssetsInPlace)
		{
			Debug.LogWarning("Can't UpdateSkinnedMeshApproximateBounds when output type is bakeMeshAssetsInPlace");
			return false;
		}
		if (this.meshCombiner.resultSceneObject == null)
		{
			Debug.LogWarning("Result Scene Object does not exist. No point in calling UpdateSkinnedMeshApproximateBounds.");
			return false;
		}
		if (this.meshCombiner.resultSceneObject.GetComponentInChildren<SkinnedMeshRenderer>() == null)
		{
			Debug.LogWarning("No SkinnedMeshRenderer on result scene object.");
			return false;
		}
		return true;
	}

	// Token: 0x04000287 RID: 647
	public List<GameObject> objsToMesh;

	// Token: 0x04000288 RID: 648
	public bool useObjsToMeshFromTexBaker = true;

	// Token: 0x04000289 RID: 649
	public bool clearBuffersAfterBake = true;

	// Token: 0x0400028A RID: 650
	public string bakeAssetsInPlaceFolderPath;

	// Token: 0x0400028B RID: 651
	[HideInInspector]
	public GameObject resultPrefab;
}
