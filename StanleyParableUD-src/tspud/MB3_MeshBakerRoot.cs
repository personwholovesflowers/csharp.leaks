using System;
using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200006C RID: 108
public abstract class MB3_MeshBakerRoot : MonoBehaviour
{
	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600028D RID: 653
	// (set) Token: 0x0600028E RID: 654
	[HideInInspector]
	public abstract MB2_TextureBakeResults textureBakeResults { get; set; }

	// Token: 0x0600028F RID: 655 RVA: 0x00008FD9 File Offset: 0x000071D9
	public virtual List<GameObject> GetObjectsToCombine()
	{
		return null;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00011160 File Offset: 0x0000F360
	public static bool DoCombinedValidate(MB3_MeshBakerRoot mom, MB_ObjsToCombineTypes objToCombineType, MB2_EditorMethodsInterface editorMethods, MB2_ValidationLevel validationLevel)
	{
		if (mom.textureBakeResults == null)
		{
			Debug.LogError("Need to set Texture Bake Result on " + mom);
			return false;
		}
		if (mom is MB3_MeshBakerCommon)
		{
			MB3_TextureBaker textureBaker = ((MB3_MeshBakerCommon)mom).GetTextureBaker();
			if (textureBaker != null && textureBaker.textureBakeResults != mom.textureBakeResults)
			{
				Debug.LogWarning("Texture Bake Result on this component is not the same as the Texture Bake Result on the MB3_TextureBaker.");
			}
		}
		Dictionary<int, MB_Utility.MeshAnalysisResult> dictionary = null;
		if (validationLevel == MB2_ValidationLevel.robust)
		{
			dictionary = new Dictionary<int, MB_Utility.MeshAnalysisResult>();
		}
		List<GameObject> objectsToCombine = mom.GetObjectsToCombine();
		for (int i = 0; i < objectsToCombine.Count; i++)
		{
			GameObject gameObject = objectsToCombine[i];
			if (gameObject == null)
			{
				Debug.LogError("The list of objects to combine contains a null at position." + i + " Select and use [shift] delete to remove");
				return false;
			}
			for (int j = i + 1; j < objectsToCombine.Count; j++)
			{
				if (objectsToCombine[i] == objectsToCombine[j])
				{
					Debug.LogError(string.Concat(new object[] { "The list of objects to combine contains duplicates at ", i, " and ", j }));
					return false;
				}
			}
			if (MB_Utility.GetGOMaterials(gameObject).Length == 0)
			{
				Debug.LogError("Object " + gameObject + " in the list of objects to be combined does not have a material");
				return false;
			}
			Mesh mesh = MB_Utility.GetMesh(gameObject);
			if (mesh == null)
			{
				Debug.LogError("Object " + gameObject + " in the list of objects to be combined does not have a mesh");
				return false;
			}
			if (mesh != null && !Application.isEditor && Application.isPlaying && mom.textureBakeResults.doMultiMaterial && validationLevel >= MB2_ValidationLevel.robust)
			{
				MB_Utility.MeshAnalysisResult meshAnalysisResult;
				if (!dictionary.TryGetValue(mesh.GetInstanceID(), out meshAnalysisResult))
				{
					MB_Utility.doSubmeshesShareVertsOrTris(mesh, ref meshAnalysisResult);
					dictionary.Add(mesh.GetInstanceID(), meshAnalysisResult);
				}
				if (meshAnalysisResult.hasOverlappingSubmeshVerts)
				{
					Debug.LogWarning("Object " + objectsToCombine[i] + " in the list of objects to combine has overlapping submeshes (submeshes share vertices). If the UVs associated with the shared vertices are important then this bake may not work. If you are using multiple materials then this object can only be combined with objects that use the exact same set of textures (each atlas contains one texture). There may be other undesirable side affects as well. Mesh Master, available in the asset store can fix overlapping submeshes.");
				}
			}
		}
		if (mom is MB3_MeshBaker)
		{
			List<GameObject> objectsToCombine2 = mom.GetObjectsToCombine();
			if (objectsToCombine2 == null || objectsToCombine2.Count == 0)
			{
				Debug.LogError("No meshes to combine. Please assign some meshes to combine.");
				return false;
			}
			if (mom is MB3_MeshBaker && ((MB3_MeshBaker)mom).meshCombiner.renderType == MB_RenderType.skinnedMeshRenderer && !editorMethods.ValidateSkinnedMeshes(objectsToCombine2))
			{
				return false;
			}
		}
		if (editorMethods != null)
		{
			editorMethods.CheckPrefabTypes(objToCombineType, objectsToCombine);
		}
		return true;
	}

	// Token: 0x04000290 RID: 656
	public Vector3 sortAxis;

	// Token: 0x02000377 RID: 887
	public class ZSortObjects
	{
		// Token: 0x06001612 RID: 5650 RVA: 0x000758C0 File Offset: 0x00073AC0
		public void SortByDistanceAlongAxis(List<GameObject> gos)
		{
			if (this.sortAxis == Vector3.zero)
			{
				Debug.LogError("The sort axis cannot be the zero vector.");
				return;
			}
			Debug.Log("Z sorting meshes along axis numObjs=" + gos.Count);
			List<MB3_MeshBakerRoot.ZSortObjects.Item> list = new List<MB3_MeshBakerRoot.ZSortObjects.Item>();
			Quaternion quaternion = Quaternion.FromToRotation(this.sortAxis, Vector3.forward);
			for (int i = 0; i < gos.Count; i++)
			{
				if (gos[i] != null)
				{
					MB3_MeshBakerRoot.ZSortObjects.Item item = new MB3_MeshBakerRoot.ZSortObjects.Item();
					item.point = gos[i].transform.position;
					item.go = gos[i];
					item.point = quaternion * item.point;
					list.Add(item);
				}
			}
			list.Sort(new MB3_MeshBakerRoot.ZSortObjects.ItemComparer());
			for (int j = 0; j < gos.Count; j++)
			{
				gos[j] = list[j].go;
			}
		}

		// Token: 0x04001290 RID: 4752
		public Vector3 sortAxis;

		// Token: 0x020004D2 RID: 1234
		public class Item
		{
			// Token: 0x040017E5 RID: 6117
			public GameObject go;

			// Token: 0x040017E6 RID: 6118
			public Vector3 point;
		}

		// Token: 0x020004D3 RID: 1235
		public class ItemComparer : IComparer<MB3_MeshBakerRoot.ZSortObjects.Item>
		{
			// Token: 0x06001A5E RID: 6750 RVA: 0x0008305A File Offset: 0x0008125A
			public int Compare(MB3_MeshBakerRoot.ZSortObjects.Item a, MB3_MeshBakerRoot.ZSortObjects.Item b)
			{
				return (int)Mathf.Sign(b.point.z - a.point.z);
			}
		}
	}
}
