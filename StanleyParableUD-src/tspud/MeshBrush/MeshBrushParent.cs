using System;
using System.Collections;
using UnityEngine;

namespace MeshBrush
{
	// Token: 0x02000252 RID: 594
	public class MeshBrushParent : MonoBehaviour
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x00043255 File Offset: 0x00041455
		private void Initialize()
		{
			this.paintedMeshes = base.GetComponentsInChildren<Transform>();
			this.meshFilters = base.GetComponentsInChildren<MeshFilter>();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x00043270 File Offset: 0x00041470
		public void FlagMeshesAsStatic()
		{
			this.Initialize();
			for (int i = this.paintedMeshes.Length - 1; i >= 0; i--)
			{
				this.paintedMeshes[i].gameObject.isStatic = true;
			}
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000432AC File Offset: 0x000414AC
		public void UnflagMeshesAsStatic()
		{
			this.Initialize();
			for (int i = this.paintedMeshes.Length - 1; i >= 0; i--)
			{
				this.paintedMeshes[i].gameObject.isStatic = false;
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000432E7 File Offset: 0x000414E7
		public int GetMeshCount()
		{
			this.Initialize();
			return this.meshFilters.Length;
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000432F8 File Offset: 0x000414F8
		public int GetTrisCount()
		{
			this.Initialize();
			if (this.meshFilters.Length != 0)
			{
				int num = 0;
				for (int i = this.meshFilters.Length - 1; i >= 0; i--)
				{
					num += this.meshFilters[i].sharedMesh.triangles.Length;
				}
				return num / 3;
			}
			return 0;
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00043347 File Offset: 0x00041547
		public void DeleteAllMeshes()
		{
			Object.DestroyImmediate(base.gameObject);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00043354 File Offset: 0x00041554
		public void CombinePaintedMeshes(bool autoSelect, MeshFilter[] meshFilters)
		{
			if (meshFilters == null || meshFilters.Length == 0)
			{
				Debug.LogError("MeshBrush: The meshFilters array you passed as an argument to the CombinePaintedMeshes function is empty or null... Combining action cancelled!");
				return;
			}
			this.localTransformationMatrix = base.transform.worldToLocalMatrix;
			this.materialToMesh = new Hashtable();
			int num = 0;
			for (long num2 = 0L; num2 < (long)meshFilters.Length; num2 += 1L)
			{
				this.currentMeshFilter = meshFilters[(int)(checked((IntPtr)num2))];
				num += this.currentMeshFilter.sharedMesh.vertexCount;
				if (num > 64000)
				{
					return;
				}
			}
			for (long num3 = 0L; num3 < (long)meshFilters.Length; num3 += 1L)
			{
				checked
				{
					this.currentMeshFilter = meshFilters[(int)((IntPtr)num3)];
					this.currentRenderer = meshFilters[(int)((IntPtr)num3)].GetComponent<Renderer>();
					this.instance = default(CombineUtility.MeshInstance);
					this.instance.mesh = this.currentMeshFilter.sharedMesh;
				}
				if (this.currentRenderer != null && this.currentRenderer.enabled && this.instance.mesh != null)
				{
					this.instance.transform = this.localTransformationMatrix * this.currentMeshFilter.transform.localToWorldMatrix;
					this.materials = this.currentRenderer.sharedMaterials;
					for (int i = 0; i < this.materials.Length; i++)
					{
						this.instance.subMeshIndex = Math.Min(i, this.instance.mesh.subMeshCount - 1);
						this.objects = (ArrayList)this.materialToMesh[this.materials[i]];
						if (this.objects != null)
						{
							this.objects.Add(this.instance);
						}
						else
						{
							this.objects = new ArrayList();
							this.objects.Add(this.instance);
							this.materialToMesh.Add(this.materials[i], this.objects);
						}
					}
					Object.DestroyImmediate(this.currentRenderer.gameObject);
				}
			}
			foreach (object obj in this.materialToMesh)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				this.elements = (ArrayList)dictionaryEntry.Value;
				this.instances = (CombineUtility.MeshInstance[])this.elements.ToArray(typeof(CombineUtility.MeshInstance));
				GameObject gameObject = new GameObject("Combined mesh");
				gameObject.transform.parent = base.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.AddComponent<MeshFilter>();
				gameObject.AddComponent<MeshRenderer>();
				gameObject.AddComponent<SaveCombinedMesh>();
				gameObject.GetComponent<Renderer>().material = (Material)dictionaryEntry.Key;
				gameObject.isStatic = true;
				this.currentMeshFilter = gameObject.GetComponent<MeshFilter>();
				this.currentMeshFilter.mesh = CombineUtility.Combine(this.instances, false);
			}
			base.gameObject.isStatic = true;
		}

		// Token: 0x04000CE4 RID: 3300
		private Transform[] paintedMeshes;

		// Token: 0x04000CE5 RID: 3301
		private MeshFilter[] meshFilters;

		// Token: 0x04000CE6 RID: 3302
		private Matrix4x4 localTransformationMatrix;

		// Token: 0x04000CE7 RID: 3303
		private Hashtable materialToMesh;

		// Token: 0x04000CE8 RID: 3304
		private MeshFilter currentMeshFilter;

		// Token: 0x04000CE9 RID: 3305
		private Renderer currentRenderer;

		// Token: 0x04000CEA RID: 3306
		private Material[] materials;

		// Token: 0x04000CEB RID: 3307
		private CombineUtility.MeshInstance instance;

		// Token: 0x04000CEC RID: 3308
		private CombineUtility.MeshInstance[] instances;

		// Token: 0x04000CED RID: 3309
		private ArrayList objects;

		// Token: 0x04000CEE RID: 3310
		private ArrayList elements;
	}
}
