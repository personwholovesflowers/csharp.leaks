using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000262 RID: 610
	[Serializable]
	public class MB3_MeshBakerGrouperKMeans : MB3_MeshBakerGrouperCore
	{
		// Token: 0x06000E79 RID: 3705 RVA: 0x00046898 File Offset: 0x00044A98
		public MB3_MeshBakerGrouperKMeans(GrouperData data)
		{
			this.d = data;
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000468C8 File Offset: 0x00044AC8
		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			Dictionary<string, List<Renderer>> dictionary = new Dictionary<string, List<Renderer>>();
			List<GameObject> list = new List<GameObject>();
			int num = 20;
			foreach (GameObject gameObject in selection)
			{
				if (!(gameObject == null))
				{
					GameObject gameObject2 = gameObject;
					Renderer component = gameObject2.GetComponent<Renderer>();
					if (component is MeshRenderer || component is SkinnedMeshRenderer)
					{
						list.Add(gameObject2);
					}
				}
			}
			if (list.Count > 0 && num > 0 && num < list.Count)
			{
				MB3_KMeansClustering mb3_KMeansClustering = new MB3_KMeansClustering(list, num);
				mb3_KMeansClustering.Cluster();
				this.clusterCenters = new Vector3[num];
				this.clusterSizes = new float[num];
				for (int i = 0; i < num; i++)
				{
					List<Renderer> cluster = mb3_KMeansClustering.GetCluster(i, out this.clusterCenters[i], out this.clusterSizes[i]);
					if (cluster.Count > 0)
					{
						dictionary.Add("Cluster_" + i, cluster);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x000469F0 File Offset: 0x00044BF0
		public override void DrawGizmos(Bounds sceneObjectBounds)
		{
			if (this.clusterCenters != null && this.clusterSizes != null && this.clusterCenters.Length == this.clusterSizes.Length)
			{
				for (int i = 0; i < this.clusterSizes.Length; i++)
				{
					Gizmos.DrawWireSphere(this.clusterCenters[i], this.clusterSizes[i]);
				}
			}
		}

		// Token: 0x04000D22 RID: 3362
		public int numClusters = 4;

		// Token: 0x04000D23 RID: 3363
		public Vector3[] clusterCenters = new Vector3[0];

		// Token: 0x04000D24 RID: 3364
		public float[] clusterSizes = new float[0];
	}
}
