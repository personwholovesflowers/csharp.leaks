using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class SimpleMeshCombiner : MonoBehaviour
{
	// Token: 0x06001B96 RID: 7062 RVA: 0x000E48C8 File Offset: 0x000E2AC8
	public void CombineMeshes()
	{
		Transform parent = base.transform.parent;
		Vector3 position = base.transform.position;
		Quaternion rotation = base.transform.rotation;
		Vector3 localScale = base.transform.localScale;
		base.transform.parent = null;
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		MeshFilter[] componentsInChildren = base.GetComponentsInChildren<MeshFilter>(true);
		Dictionary<Material, List<CombineInstance>> dictionary = new Dictionary<Material, List<CombineInstance>>();
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			if (!(meshFilter.sharedMesh == null))
			{
				if (meshFilter.gameObject.isStatic)
				{
					string text = "cannot process static mesh ";
					GameObject gameObject = meshFilter.gameObject;
					Debug.LogWarning(text + ((gameObject != null) ? gameObject.ToString() : null));
				}
				else
				{
					MeshRenderer component = meshFilter.GetComponent<MeshRenderer>();
					if (!(component == null) && component.sharedMaterials.Length != 0)
					{
						for (int j = 0; j < meshFilter.sharedMesh.subMeshCount; j++)
						{
							Material material = component.sharedMaterials[j];
							if (!dictionary.ContainsKey(material))
							{
								dictionary[material] = new List<CombineInstance>();
							}
							CombineInstance combineInstance = new CombineInstance
							{
								mesh = meshFilter.sharedMesh,
								subMeshIndex = j,
								transform = meshFilter.transform.localToWorldMatrix
							};
							dictionary[material].Add(combineInstance);
						}
					}
				}
			}
		}
		List<CombineInstance> list = new List<CombineInstance>();
		List<Material> list2 = new List<Material>();
		foreach (KeyValuePair<Material, List<CombineInstance>> keyValuePair in dictionary)
		{
			List<CombineInstance> value = keyValuePair.Value;
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(value.ToArray(), true, true);
			list.Add(new CombineInstance
			{
				mesh = mesh,
				subMeshIndex = 0,
				transform = Matrix4x4.identity
			});
			list2.Add(keyValuePair.Key);
		}
		Mesh mesh2 = new Mesh();
		mesh2.CombineMeshes(list.ToArray(), false, false);
		MeshFilter meshFilter2 = base.gameObject.AddComponent<MeshFilter>();
		Renderer renderer = base.gameObject.AddComponent<MeshRenderer>();
		meshFilter2.sharedMesh = mesh2;
		renderer.materials = list2.ToArray();
		base.transform.SetParent(parent, false);
		base.transform.SetPositionAndRotation(position, rotation);
		base.transform.localScale = localScale;
		if (this.removeAllChildren)
		{
			for (int k = 0; k < base.transform.childCount; k++)
			{
				Object.Destroy(base.transform.GetChild(k).gameObject);
			}
		}
	}

	// Token: 0x040026D3 RID: 9939
	public bool removeAllChildren = true;
}
