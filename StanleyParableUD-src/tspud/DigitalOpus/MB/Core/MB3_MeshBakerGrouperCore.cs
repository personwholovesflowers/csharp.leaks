using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200025E RID: 606
	[Serializable]
	public abstract class MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000E69 RID: 3689
		public abstract Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection);

		// Token: 0x06000E6A RID: 3690
		public abstract void DrawGizmos(Bounds sourceObjectBounds);

		// Token: 0x06000E6B RID: 3691 RVA: 0x00045AC8 File Offset: 0x00043CC8
		public void DoClustering(MB3_TextureBaker tb, MB3_MeshBakerGrouper grouper)
		{
			Dictionary<string, List<Renderer>> dictionary = this.FilterIntoGroups(tb.GetObjectsToCombine());
			if (this.d.clusterOnLMIndex)
			{
				Dictionary<string, List<Renderer>> dictionary2 = new Dictionary<string, List<Renderer>>();
				foreach (string text in dictionary.Keys)
				{
					List<Renderer> list = dictionary[text];
					Dictionary<int, List<Renderer>> dictionary3 = this.GroupByLightmapIndex(list);
					foreach (int num in dictionary3.Keys)
					{
						string text2 = text + "-LM-" + num;
						dictionary2.Add(text2, dictionary3[num]);
					}
				}
				dictionary = dictionary2;
			}
			if (this.d.clusterByLODLevel)
			{
				Dictionary<string, List<Renderer>> dictionary4 = new Dictionary<string, List<Renderer>>();
				foreach (string text3 in dictionary.Keys)
				{
					using (List<Renderer>.Enumerator enumerator3 = dictionary[text3].GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							Renderer r = enumerator3.Current;
							if (!(r == null))
							{
								bool flag = false;
								LODGroup componentInParent = r.GetComponentInParent<LODGroup>();
								if (componentInParent != null)
								{
									LOD[] lods = componentInParent.GetLODs();
									Predicate<Renderer> <>9__0;
									for (int i = 0; i < lods.Length; i++)
									{
										Renderer[] renderers = lods[i].renderers;
										Predicate<Renderer> predicate;
										if ((predicate = <>9__0) == null)
										{
											predicate = (<>9__0 = (Renderer x) => x == r);
										}
										if (Array.Find<Renderer>(renderers, predicate) != null)
										{
											flag = true;
											string text4 = string.Format("{0}_LOD{1}", text3, i);
											List<Renderer> list2;
											if (!dictionary4.TryGetValue(text4, out list2))
											{
												list2 = new List<Renderer>();
												dictionary4.Add(text4, list2);
											}
											if (!list2.Contains(r))
											{
												list2.Add(r);
											}
										}
									}
								}
								if (!flag)
								{
									string text5 = string.Format("{0}_LOD0", text3);
									List<Renderer> list3;
									if (!dictionary4.TryGetValue(text5, out list3))
									{
										list3 = new List<Renderer>();
										dictionary4.Add(text5, list3);
									}
									if (!list3.Contains(r))
									{
										list3.Add(r);
									}
								}
							}
						}
					}
				}
				dictionary = dictionary4;
			}
			int num2 = 0;
			foreach (string text6 in dictionary.Keys)
			{
				List<Renderer> list4 = dictionary[text6];
				if (list4.Count > 1 || grouper.data.includeCellsWithOnlyOneRenderer)
				{
					this.AddMeshBaker(tb, text6, list4);
				}
				else
				{
					num2++;
				}
			}
			Debug.Log(string.Format("Found {0} cells with Renderers. Not creating bakers for {1} because there is only one mesh in the cell. Creating {2} bakers.", dictionary.Count, num2, dictionary.Count - num2));
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x00045E70 File Offset: 0x00044070
		private Dictionary<int, List<Renderer>> GroupByLightmapIndex(List<Renderer> gaws)
		{
			Dictionary<int, List<Renderer>> dictionary = new Dictionary<int, List<Renderer>>();
			for (int i = 0; i < gaws.Count; i++)
			{
				List<Renderer> list;
				if (dictionary.ContainsKey(gaws[i].lightmapIndex))
				{
					list = dictionary[gaws[i].lightmapIndex];
				}
				else
				{
					list = new List<Renderer>();
					dictionary.Add(gaws[i].lightmapIndex, list);
				}
				list.Add(gaws[i]);
			}
			return dictionary;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x00045EE8 File Offset: 0x000440E8
		private void AddMeshBaker(MB3_TextureBaker tb, string key, List<Renderer> gaws)
		{
			int num = 0;
			for (int i = 0; i < gaws.Count; i++)
			{
				Mesh mesh = MB_Utility.GetMesh(gaws[i].gameObject);
				if (mesh != null)
				{
					num += mesh.vertexCount;
				}
			}
			GameObject gameObject = new GameObject("MeshBaker-" + key);
			gameObject.transform.position = Vector3.zero;
			MB3_MeshBakerCommon mb3_MeshBakerCommon;
			if (num >= 65535)
			{
				mb3_MeshBakerCommon = gameObject.AddComponent<MB3_MultiMeshBaker>();
				mb3_MeshBakerCommon.useObjsToMeshFromTexBaker = false;
			}
			else
			{
				mb3_MeshBakerCommon = gameObject.AddComponent<MB3_MeshBaker>();
				mb3_MeshBakerCommon.useObjsToMeshFromTexBaker = false;
			}
			mb3_MeshBakerCommon.textureBakeResults = tb.textureBakeResults;
			mb3_MeshBakerCommon.transform.parent = tb.transform;
			for (int j = 0; j < gaws.Count; j++)
			{
				mb3_MeshBakerCommon.GetObjectsToCombine().Add(gaws[j].gameObject);
			}
		}

		// Token: 0x04000D21 RID: 3361
		public GrouperData d;
	}
}
