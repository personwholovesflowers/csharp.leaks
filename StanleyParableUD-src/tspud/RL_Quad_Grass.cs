using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000177 RID: 375
public class RL_Quad_Grass : MonoBehaviour
{
	// Token: 0x060008CF RID: 2255 RVA: 0x00029E0C File Offset: 0x0002800C
	[ContextMenu("Create Mesh")]
	public void CreateMesh()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh mesh = component.sharedMesh;
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		float num = (float)this.layers * this.separation;
		List<Vector3> list = new List<Vector3>();
		List<Color32> list2 = new List<Color32>();
		List<Vector2> list3 = new List<Vector2>();
		List<int> list4 = new List<int>();
		for (int i = 0; i < this.layers; i++)
		{
			float num2 = -num / 2f;
			float num3 = num / 2f;
			float num4 = -num / 2f + (float)i * this.separation;
			float num5 = (float)i / (float)this.layers;
			int num6 = 1;
			for (int j = 0; j < num6; j++)
			{
				int count = list.Count;
				float num7 = (float)j / (float)num6;
				float num8 = Mathf.Lerp(num2, num3, num7);
				float num9 = 1f;
				list.Add(new Vector3(num8, 0f, num4));
				list.Add(new Vector3(num8, num9, num4));
				list2.Add(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
				list2.Add(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
				list3.Add(new Vector2(0f, num7));
				list3.Add(new Vector2(1f, num7));
				list4.Add(count);
				list4.Add(count + 1);
				list4.Add(count + 2);
				list4.Add(count + 1);
				list4.Add(count + 3);
				list4.Add(count + 2);
			}
			list.Add(new Vector3(num3, 0f, num4));
			list.Add(new Vector3(num3, 1f, num4));
			list2.Add(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
			list2.Add(new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
			list3.Add(new Vector2(0f, 1f));
			list3.Add(new Vector2(1f, 1f));
		}
		int count2 = list.Count;
		for (int k = 0; k < count2; k++)
		{
			Vector3 vector = new Vector3(list[k].z, list[k].y, list[k].x);
			list.Add(vector);
			list3.Add(list3[k]);
			list2.Add(new Color32(0, byte.MaxValue, 0, byte.MaxValue));
		}
		int count3 = list4.Count;
		for (int l = 0; l < count3; l++)
		{
			list4.Add(count2 + list4[l]);
		}
		mesh.SetVertices(list);
		mesh.SetUVs(0, list3);
		mesh.SetColors(list2);
		mesh.SetTriangles(list4, 0);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();
		mesh.UploadMeshData(false);
		component.sharedMesh = mesh;
	}

	// Token: 0x0400089C RID: 2204
	[InspectorButton("CreateMesh", "Create Mesh")]
	public float maxDistance = 20f;

	// Token: 0x0400089D RID: 2205
	public int layers = 160;

	// Token: 0x0400089E RID: 2206
	public float separation = 0.5f;
}
