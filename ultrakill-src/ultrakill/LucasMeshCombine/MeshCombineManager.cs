using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace LucasMeshCombine
{
	// Token: 0x0200057D RID: 1405
	public class MeshCombineManager : MonoBehaviour
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x00102474 File Offset: 0x00100674
		// (set) Token: 0x06001FDB RID: 8155 RVA: 0x0010247B File Offset: 0x0010067B
		public static MeshCombineManager Instance { get; private set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00102483 File Offset: 0x00100683
		public Shader[] AllowedShadersToBatch
		{
			get
			{
				return this.allowedShadersToBatch;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x0010248B File Offset: 0x0010068B
		public HashSet<MeshRenderer> ProcessedMeshRenderers
		{
			get
			{
				return this.processedMeshRenderers;
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x00102493 File Offset: 0x00100693
		public void AddCombineDatas(List<MeshCombineData> meshCombineDatas)
		{
			this.combineSets.Add(meshCombineDatas);
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x001024A1 File Offset: 0x001006A1
		private void Awake()
		{
			if (MeshCombineManager.Instance != null)
			{
				Object.Destroy(base.gameObject);
			}
			MeshCombineManager.Instance = this;
			this.processedMeshRenderers.Clear();
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x001024CC File Offset: 0x001006CC
		private void Start()
		{
			this.textureAtlas = new Texture2D(1, 1)
			{
				filterMode = FilterMode.Point
			};
			if (this.combinedMeshMaterial == null)
			{
				this.combinedMeshMaterial = new Material(this.atlasedShader)
				{
					name = "Combined Mesh Material",
					hideFlags = HideFlags.HideAndDontSave,
					mainTexture = this.textureAtlas
				};
			}
			foreach (List<MeshCombineData> list in this.combineSets)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Texture2D texture = list[i].Texture;
					if (!this.textures.Contains(texture))
					{
						if (!texture.isReadable)
						{
							Debug.LogWarning("Mesh Combine: Texture not readable " + texture.name, texture);
						}
						else
						{
							this.textures.Add(texture);
						}
					}
				}
			}
			Rect[] array = this.textureAtlas.PackTextures(this.textures.ToArray(), 0, 8192);
			List<Vector4> list2 = new List<Vector4>();
			foreach (List<MeshCombineData> list3 in this.combineSets)
			{
				if (list3.Count != 0)
				{
					List<CombineInstance> list4 = new List<CombineInstance>(list3.Count);
					for (int j = 0; j < list3.Count; j++)
					{
						MeshCombineData meshCombineData = list3[j];
						Mesh mesh = meshCombineData.MeshFilter.mesh;
						if (!mesh.name.StartsWith("Combined Mesh"))
						{
							if (!mesh.isReadable)
							{
								Debug.LogWarning("Mesh Combine: Mesh isn't readable! Couldn't combine mesh " + mesh.name + " on GameObject " + meshCombineData.MeshFilter.gameObject.name, meshCombineData.MeshFilter.gameObject);
							}
							else
							{
								int num = this.textures.IndexOf(meshCombineData.Texture);
								if (num >= 0)
								{
									Rect rect = array[num];
									SubMeshDescriptor subMesh = mesh.GetSubMesh(meshCombineData.SubMeshIndex);
									Vector4[] array2 = new Vector4[mesh.vertexCount];
									mesh.GetUVs(3, list2);
									this.oldMeshUvs[mesh] = array2.ToArray<Vector4>();
									for (int k = 0; k < array2.Length; k++)
									{
										if (k >= subMesh.firstVertex && k < subMesh.firstVertex + subMesh.vertexCount)
										{
											array2[k] = new Vector4(rect.xMin, rect.yMin, rect.xMax, rect.yMax);
										}
										else if (list2.Count > k)
										{
											array2[k] = list2[k];
										}
									}
									mesh.SetUVs(3, array2);
									mesh.UploadMeshData(false);
									list4.Add(new CombineInstance
									{
										mesh = mesh,
										subMeshIndex = meshCombineData.SubMeshIndex,
										transform = meshCombineData.MeshRenderer.localToWorldMatrix
									});
									meshCombineData.MeshRenderer.enabled = false;
								}
							}
						}
					}
					if (list4.Count == 0)
					{
						Debug.LogWarning("Mesh Combine: Mesh combination on GameObject " + list3[0].Parent.name + " is not effective (zero mesh combinations). You may want to remove it, or turn off static batching.", list3[0].Parent);
						for (int l = 0; l < list3.Count; l++)
						{
							MeshCombineData meshCombineData2 = list3[l];
							Mesh mesh2 = meshCombineData2.MeshFilter.mesh;
							if (!mesh2.name.StartsWith("Combined Mesh") && mesh2.isReadable && this.textures.IndexOf(meshCombineData2.Texture) >= 0)
							{
								meshCombineData2.MeshRenderer.enabled = true;
								mesh2.SetUVs(3, this.oldMeshUvs[mesh2]);
							}
						}
					}
					else
					{
						Mesh mesh3 = new Mesh();
						mesh3.CombineMeshes(list4.ToArray());
						mesh3.Optimize();
						GameObject gameObject = new GameObject("Combined Mesh");
						gameObject.transform.parent = list3[0].Parent.transform;
						gameObject.isStatic = true;
						gameObject.layer = list3[0].MeshRenderer.gameObject.layer;
						this.createdMeshes.Add(mesh3);
						this.createdObjects.Add(gameObject);
						MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
						meshRenderer.sharedMaterial = this.combinedMeshMaterial;
						meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
						meshRenderer.receiveShadows = false;
						meshRenderer.lightProbeUsage = LightProbeUsage.Off;
						meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
						gameObject.AddComponent<MeshFilter>().sharedMesh = mesh3;
						if (list4.Count <= 1)
						{
							Debug.LogWarning("Mesh Combine: Mesh combination on GameObject " + list3[0].Parent.name + " is not effective (less than two mesh combinations). You may want to remove it.", list3[0].Parent);
						}
					}
				}
			}
			this.textures.Clear();
			this.combineSets.Clear();
			this.processedMeshRenderers.Clear();
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00102A10 File Offset: 0x00100C10
		private void OnDestroy()
		{
			foreach (Mesh mesh in this.createdMeshes)
			{
				Object.Destroy(mesh);
			}
			foreach (GameObject gameObject in this.createdObjects)
			{
				Object.Destroy(gameObject);
			}
			foreach (MeshRenderer meshRenderer in this.processedMeshRenderers)
			{
				if (!(meshRenderer == null))
				{
					meshRenderer.enabled = true;
				}
			}
			MeshCombineManager.Instance = null;
			Object.Destroy(this.textureAtlas);
			Object.Destroy(this.combinedMeshMaterial);
		}

		// Token: 0x04002C1A RID: 11290
		[SerializeField]
		private Shader[] allowedShadersToBatch;

		// Token: 0x04002C1B RID: 11291
		[SerializeField]
		private Shader atlasedShader;

		// Token: 0x04002C1C RID: 11292
		[SerializeField]
		private Texture2D textureAtlas;

		// Token: 0x04002C1D RID: 11293
		private readonly Dictionary<Mesh, Vector4[]> oldMeshUvs = new Dictionary<Mesh, Vector4[]>();

		// Token: 0x04002C1E RID: 11294
		private readonly List<List<MeshCombineData>> combineSets = new List<List<MeshCombineData>>();

		// Token: 0x04002C1F RID: 11295
		private readonly List<Texture2D> textures = new List<Texture2D>();

		// Token: 0x04002C20 RID: 11296
		private readonly HashSet<MeshRenderer> processedMeshRenderers = new HashSet<MeshRenderer>();

		// Token: 0x04002C21 RID: 11297
		private readonly List<Mesh> createdMeshes = new List<Mesh>();

		// Token: 0x04002C22 RID: 11298
		private readonly List<GameObject> createdObjects = new List<GameObject>();

		// Token: 0x04002C23 RID: 11299
		private Material combinedMeshMaterial;
	}
}
