using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200027B RID: 635
	[Serializable]
	public class MB3_AgglomerativeClustering
	{
		// Token: 0x06000F11 RID: 3857 RVA: 0x00049DD4 File Offset: 0x00047FD4
		private float euclidean_distance(Vector3 a, Vector3 b)
		{
			return Vector3.Distance(a, b);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00049DE0 File Offset: 0x00047FE0
		public bool agglomerate(ProgressUpdateCancelableDelegate progFunc)
		{
			this.wasCanceled = true;
			if (progFunc != null)
			{
				this.wasCanceled = progFunc("Filling Priority Queue:", 0f);
			}
			if (this.items.Count <= 1)
			{
				this.clusters = new MB3_AgglomerativeClustering.ClusterNode[0];
				return false;
			}
			this.clusters = new MB3_AgglomerativeClustering.ClusterNode[this.items.Count * 2 - 1];
			for (int i = 0; i < this.items.Count; i++)
			{
				this.clusters[i] = new MB3_AgglomerativeClustering.ClusterNode(this.items[i], i);
			}
			int num = this.items.Count;
			List<MB3_AgglomerativeClustering.ClusterNode> list = new List<MB3_AgglomerativeClustering.ClusterNode>();
			for (int j = 0; j < num; j++)
			{
				this.clusters[j].isUnclustered = true;
				list.Add(this.clusters[j]);
			}
			int num2 = 0;
			new Stopwatch().Start();
			float num3 = 0f;
			long num4 = GC.GetTotalMemory(false) / 1000000L;
			PriorityQueue<float, MB3_AgglomerativeClustering.ClusterDistance> priorityQueue = new PriorityQueue<float, MB3_AgglomerativeClustering.ClusterDistance>();
			int num5 = 0;
			while (list.Count > 1)
			{
				int num6 = 0;
				num2++;
				if (priorityQueue.Count == 0)
				{
					num5++;
					num4 = GC.GetTotalMemory(false) / 1000000L;
					if (progFunc != null)
					{
						this.wasCanceled = progFunc(string.Concat(new object[]
						{
							"Refilling Q:",
							(float)(this.items.Count - list.Count) * 100f / (float)this.items.Count,
							" unclustered:",
							list.Count,
							" inQ:",
							priorityQueue.Count,
							" usedMem:",
							num4
						}), (float)(this.items.Count - list.Count) / (float)this.items.Count);
					}
					num3 = this._RefillPriorityQWithSome(priorityQueue, list, this.clusters, progFunc);
					if (priorityQueue.Count == 0)
					{
						break;
					}
				}
				KeyValuePair<float, MB3_AgglomerativeClustering.ClusterDistance> keyValuePair = priorityQueue.Dequeue();
				while (!keyValuePair.Value.a.isUnclustered || !keyValuePair.Value.b.isUnclustered)
				{
					if (priorityQueue.Count == 0)
					{
						num5++;
						num4 = GC.GetTotalMemory(false) / 1000000L;
						if (progFunc != null)
						{
							this.wasCanceled = progFunc(string.Concat(new object[]
							{
								"Creating clusters:",
								(float)(this.items.Count - list.Count) * 100f / (float)this.items.Count,
								" unclustered:",
								list.Count,
								" inQ:",
								priorityQueue.Count,
								" usedMem:",
								num4
							}), (float)(this.items.Count - list.Count) / (float)this.items.Count);
						}
						num3 = this._RefillPriorityQWithSome(priorityQueue, list, this.clusters, progFunc);
						if (priorityQueue.Count == 0)
						{
							break;
						}
					}
					keyValuePair = priorityQueue.Dequeue();
					num6++;
				}
				num++;
				MB3_AgglomerativeClustering.ClusterNode clusterNode = new MB3_AgglomerativeClustering.ClusterNode(keyValuePair.Value.a, keyValuePair.Value.b, num - 1, num2, keyValuePair.Key, this.clusters);
				list.Remove(keyValuePair.Value.a);
				list.Remove(keyValuePair.Value.b);
				keyValuePair.Value.a.isUnclustered = false;
				keyValuePair.Value.b.isUnclustered = false;
				int num7 = num - 1;
				if (num7 == this.clusters.Length)
				{
					Debug.LogError("how did this happen");
				}
				this.clusters[num7] = clusterNode;
				list.Add(clusterNode);
				clusterNode.isUnclustered = true;
				for (int k = 0; k < list.Count - 1; k++)
				{
					float num8 = this.euclidean_distance(clusterNode.centroid, list[k].centroid);
					if (num8 < num3)
					{
						priorityQueue.Add(new KeyValuePair<float, MB3_AgglomerativeClustering.ClusterDistance>(num8, new MB3_AgglomerativeClustering.ClusterDistance(clusterNode, list[k])));
					}
				}
				if (this.wasCanceled)
				{
					break;
				}
				num4 = GC.GetTotalMemory(false) / 1000000L;
				if (progFunc != null)
				{
					this.wasCanceled = progFunc(string.Concat(new object[]
					{
						"Creating clusters:",
						(float)(this.items.Count - list.Count) * 100f / (float)this.items.Count,
						" unclustered:",
						list.Count,
						" inQ:",
						priorityQueue.Count,
						" usedMem:",
						num4
					}), (float)(this.items.Count - list.Count) / (float)this.items.Count);
				}
			}
			if (progFunc != null)
			{
				this.wasCanceled = progFunc("Finished clustering:", 100f);
			}
			return !this.wasCanceled;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0004A31C File Offset: 0x0004851C
		private float _RefillPriorityQWithSome(PriorityQueue<float, MB3_AgglomerativeClustering.ClusterDistance> pq, List<MB3_AgglomerativeClustering.ClusterNode> unclustered, MB3_AgglomerativeClustering.ClusterNode[] clusters, ProgressUpdateCancelableDelegate progFunc)
		{
			List<float> list = new List<float>(2048);
			for (int i = 0; i < unclustered.Count; i++)
			{
				for (int j = i + 1; j < unclustered.Count; j++)
				{
					list.Add(this.euclidean_distance(unclustered[i].centroid, unclustered[j].centroid));
				}
				this.wasCanceled = progFunc("Refilling Queue Part A:", (float)i / ((float)unclustered.Count * 2f));
				if (this.wasCanceled)
				{
					return 10f;
				}
			}
			if (list.Count == 0)
			{
				return 1E+11f;
			}
			float num = MB3_AgglomerativeClustering.NthSmallestElement<float>(list, 2048);
			for (int k = 0; k < unclustered.Count; k++)
			{
				for (int l = k + 1; l < unclustered.Count; l++)
				{
					int idx = unclustered[k].idx;
					int idx2 = unclustered[l].idx;
					float num2 = this.euclidean_distance(unclustered[k].centroid, unclustered[l].centroid);
					if (num2 <= num)
					{
						pq.Add(new KeyValuePair<float, MB3_AgglomerativeClustering.ClusterDistance>(num2, new MB3_AgglomerativeClustering.ClusterDistance(clusters[idx], clusters[idx2])));
					}
				}
				this.wasCanceled = progFunc("Refilling Queue Part B:", (float)(unclustered.Count + k) / ((float)unclustered.Count * 2f));
				if (this.wasCanceled)
				{
					return 10f;
				}
			}
			return num;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0004A494 File Offset: 0x00048694
		public int TestRun(List<GameObject> gos)
		{
			List<MB3_AgglomerativeClustering.item_s> list = new List<MB3_AgglomerativeClustering.item_s>();
			for (int i = 0; i < gos.Count; i++)
			{
				list.Add(new MB3_AgglomerativeClustering.item_s
				{
					go = gos[i],
					coord = gos[i].transform.position
				});
			}
			this.items = list;
			if (this.items.Count > 0)
			{
				this.agglomerate(null);
			}
			return 0;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0004A508 File Offset: 0x00048708
		public static void Main()
		{
			List<float> list = new List<float>();
			list.AddRange(new float[] { 19f, 18f, 17f, 16f, 15f, 10f, 11f, 12f, 13f, 14f });
			Debug.Log("Loop quick select 10 times.");
			Debug.Log(MB3_AgglomerativeClustering.NthSmallestElement<float>(list, 0));
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0004A550 File Offset: 0x00048750
		public static T NthSmallestElement<T>(List<T> array, int n) where T : IComparable<T>
		{
			if (n < 0)
			{
				n = 0;
			}
			if (n > array.Count - 1)
			{
				n = array.Count - 1;
			}
			if (array.Count == 0)
			{
				throw new ArgumentException("Array is empty.", "array");
			}
			if (array.Count == 1)
			{
				return array[0];
			}
			return MB3_AgglomerativeClustering.QuickSelectSmallest<T>(array, n)[n];
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0004A5B0 File Offset: 0x000487B0
		private static List<T> QuickSelectSmallest<T>(List<T> input, int n) where T : IComparable<T>
		{
			int num = 0;
			int i = input.Count - 1;
			int num2 = n;
			Random random = new Random();
			while (i > num)
			{
				num2 = MB3_AgglomerativeClustering.QuickSelectPartition<T>(input, num, i, num2);
				if (num2 == n)
				{
					break;
				}
				if (num2 > n)
				{
					i = num2 - 1;
				}
				else
				{
					num = num2 + 1;
				}
				num2 = random.Next(num, i);
			}
			return input;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x0004A600 File Offset: 0x00048800
		private static int QuickSelectPartition<T>(List<T> array, int startIndex, int endIndex, int pivotIndex) where T : IComparable<T>
		{
			T t = array[pivotIndex];
			MB3_AgglomerativeClustering.Swap<T>(array, pivotIndex, endIndex);
			for (int i = startIndex; i < endIndex; i++)
			{
				T t2 = array[i];
				if (t2.CompareTo(t) <= 0)
				{
					MB3_AgglomerativeClustering.Swap<T>(array, i, startIndex);
					startIndex++;
				}
			}
			MB3_AgglomerativeClustering.Swap<T>(array, endIndex, startIndex);
			return startIndex;
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x0004A658 File Offset: 0x00048858
		private static void Swap<T>(List<T> array, int index1, int index2)
		{
			if (index1 == index2)
			{
				return;
			}
			T t = array[index1];
			array[index1] = array[index2];
			array[index2] = t;
		}

		// Token: 0x04000D75 RID: 3445
		public List<MB3_AgglomerativeClustering.item_s> items = new List<MB3_AgglomerativeClustering.item_s>();

		// Token: 0x04000D76 RID: 3446
		public MB3_AgglomerativeClustering.ClusterNode[] clusters;

		// Token: 0x04000D77 RID: 3447
		public bool wasCanceled;

		// Token: 0x04000D78 RID: 3448
		private const int MAX_PRIORITY_Q_SIZE = 2048;

		// Token: 0x0200045A RID: 1114
		[Serializable]
		public class ClusterNode
		{
			// Token: 0x060018FB RID: 6395 RVA: 0x0007CE68 File Offset: 0x0007B068
			public ClusterNode(MB3_AgglomerativeClustering.item_s ii, int index)
			{
				this.leaf = ii;
				this.idx = index;
				this.leafs = new int[1];
				this.leafs[0] = index;
				this.centroid = ii.coord;
				this.height = 0;
			}

			// Token: 0x060018FC RID: 6396 RVA: 0x0007CEB8 File Offset: 0x0007B0B8
			public ClusterNode(MB3_AgglomerativeClustering.ClusterNode a, MB3_AgglomerativeClustering.ClusterNode b, int index, int h, float dist, MB3_AgglomerativeClustering.ClusterNode[] clusters)
			{
				this.cha = a;
				this.chb = b;
				this.idx = index;
				this.leafs = new int[a.leafs.Length + b.leafs.Length];
				Array.Copy(a.leafs, this.leafs, a.leafs.Length);
				Array.Copy(b.leafs, 0, this.leafs, a.leafs.Length, b.leafs.Length);
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.leafs.Length; i++)
				{
					vector += clusters[this.leafs[i]].centroid;
				}
				this.centroid = vector / (float)this.leafs.Length;
				this.height = h;
				this.distToMergedCentroid = dist;
			}

			// Token: 0x04001629 RID: 5673
			public MB3_AgglomerativeClustering.item_s leaf;

			// Token: 0x0400162A RID: 5674
			public MB3_AgglomerativeClustering.ClusterNode cha;

			// Token: 0x0400162B RID: 5675
			public MB3_AgglomerativeClustering.ClusterNode chb;

			// Token: 0x0400162C RID: 5676
			public int height;

			// Token: 0x0400162D RID: 5677
			public float distToMergedCentroid;

			// Token: 0x0400162E RID: 5678
			public Vector3 centroid;

			// Token: 0x0400162F RID: 5679
			public int[] leafs;

			// Token: 0x04001630 RID: 5680
			public int idx;

			// Token: 0x04001631 RID: 5681
			public bool isUnclustered = true;
		}

		// Token: 0x0200045B RID: 1115
		[Serializable]
		public class item_s
		{
			// Token: 0x04001632 RID: 5682
			public GameObject go;

			// Token: 0x04001633 RID: 5683
			public Vector3 coord;
		}

		// Token: 0x0200045C RID: 1116
		public class ClusterDistance
		{
			// Token: 0x060018FE RID: 6398 RVA: 0x0007CF93 File Offset: 0x0007B193
			public ClusterDistance(MB3_AgglomerativeClustering.ClusterNode aa, MB3_AgglomerativeClustering.ClusterNode bb)
			{
				this.a = aa;
				this.b = bb;
			}

			// Token: 0x04001634 RID: 5684
			public MB3_AgglomerativeClustering.ClusterNode a;

			// Token: 0x04001635 RID: 5685
			public MB3_AgglomerativeClustering.ClusterNode b;
		}
	}
}
