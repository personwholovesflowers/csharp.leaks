using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002F2 RID: 754
	public class RecolorTree
	{
		// Token: 0x0600139E RID: 5022 RVA: 0x0006856C File Offset: 0x0006676C
		public RecolorTree(Mesh aMesh, Matrix4x4? aTransform = null, bool aX = true, bool aY = true, bool aZ = true)
		{
			if (aMesh == null)
			{
				this.Create(new Vector3[] { Vector3.zero }, new Color[] { Color.white }, aTransform, aX, aY, aZ);
				return;
			}
			Vector3[] vertices = aMesh.vertices;
			Color[] array = aMesh.colors;
			if (array == null || array.Length == 0)
			{
				array = new Color[vertices.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Color.white;
				}
			}
			this.Create(vertices, array, aTransform, aX, aY, aZ);
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x000685FF File Offset: 0x000667FF
		public RecolorTree(Vector3[] aPoints, Color[] aColors, Matrix4x4? aTransform = null, bool aX = true, bool aY = true, bool aZ = true)
		{
			this.Create(aPoints, aColors, aTransform, aX, aY, aZ);
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00068616 File Offset: 0x00066816
		public RecolorTree(List<Vector3> aPoints, List<Color> aColors, Matrix4x4? aTransform = null, bool aX = true, bool aY = true, bool aZ = true)
		{
			this.Create(aPoints, aColors, aTransform, aX, aY, aZ);
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00068630 File Offset: 0x00066830
		public RecolorTree.TreePoint Get(Vector3 aAt)
		{
			RecolorTree.TreePoint treePoint = null;
			float num = 0f;
			this.root.GetNearest(this.settings, 0, aAt, ref treePoint, ref num);
			return treePoint;
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00068660 File Offset: 0x00066860
		public void Recolor(ref Mesh aMesh, Matrix4x4? aTransform = null)
		{
			if (aMesh == null)
			{
				return;
			}
			Vector3[] vertices = aMesh.vertices;
			aMesh.colors = this.Recolor(vertices, aTransform);
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00068690 File Offset: 0x00066890
		public void Recolor(Vector3[] aPoints, ref Color[] aColors, Matrix4x4? aTransform = null)
		{
			if (aPoints.Length != aColors.Length)
			{
				Debug.LogError("Arguments must be the same length!");
			}
			if (aTransform != null)
			{
				for (int i = 0; i < aPoints.Length; i++)
				{
					aColors[i] = this.Get(aTransform.Value.MultiplyPoint(aPoints[i])).data;
				}
				return;
			}
			for (int j = 0; j < aPoints.Length; j++)
			{
				aColors[j] = this.Get(aPoints[j]).data;
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0006871C File Offset: 0x0006691C
		public Color[] Recolor(Vector3[] aAt, Matrix4x4? aTransform = null)
		{
			Color[] array = new Color[aAt.Length];
			if (aTransform != null)
			{
				for (int i = 0; i < aAt.Length; i++)
				{
					array[i] = this.Get(aTransform.Value.MultiplyPoint(aAt[i])).data;
				}
			}
			else
			{
				for (int j = 0; j < aAt.Length; j++)
				{
					array[j] = this.Get(aAt[j]).data;
				}
			}
			return array;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x0006879C File Offset: 0x0006699C
		public List<Color> Recolor(List<Vector3> aAt, Matrix4x4? aTransform = null)
		{
			List<Color> list = new List<Color>(aAt.Count);
			if (aTransform != null)
			{
				for (int i = 0; i < aAt.Count; i++)
				{
					list[i] = this.Get(aTransform.Value.MultiplyPoint(aAt[i])).data;
				}
			}
			else
			{
				for (int j = 0; j < aAt.Count; j++)
				{
					list[j] = this.Get(aAt[j]).data;
				}
			}
			return list;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00068824 File Offset: 0x00066A24
		public void Recolor(List<Vector3> aPoints, ref List<Color> aColors, Matrix4x4? aTransform = null)
		{
			if (aTransform != null)
			{
				for (int i = 0; i < aPoints.Count; i++)
				{
					aColors.Add(this.Get(aTransform.Value.MultiplyPoint(aPoints[i])).data);
				}
				return;
			}
			for (int j = 0; j < aPoints.Count; j++)
			{
				aColors.Add(this.Get(aPoints[j]).data);
			}
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000688A0 File Offset: 0x00066AA0
		public void DrawTree()
		{
			RecolorTree.TreeSettings treeSettings = new RecolorTree.TreeSettings(true, true, true);
			this.root.Draw(treeSettings, 0, Vector3.zero);
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x000688C8 File Offset: 0x00066AC8
		private void Create(Vector3[] aPoints, Color[] aColors, Matrix4x4? aTransform, bool aX, bool aY, bool aZ)
		{
			if (aPoints.Length != aColors.Length)
			{
				Debug.LogError("Arguments must be the same length!");
			}
			List<RecolorTree.TreePoint> list = new List<RecolorTree.TreePoint>(aPoints.Length);
			for (int i = 0; i < aPoints.Length; i++)
			{
				Vector3 vector = aPoints[i];
				if (aTransform != null)
				{
					vector = aTransform.Value.MultiplyPoint(vector);
				}
				list.Add(new RecolorTree.TreePoint(vector, aColors[i]));
			}
			this.settings = new RecolorTree.TreeSettings(aX, aY, aZ);
			this.root = new RecolorTree.TreeNode(this.settings, list, 0);
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x00068958 File Offset: 0x00066B58
		private void Create(List<Vector3> aPoints, List<Color> aColors, Matrix4x4? aTransform, bool aX, bool aY, bool aZ)
		{
			if (aPoints.Count != aColors.Count)
			{
				Debug.LogError("Arguments must be the same length!");
			}
			List<RecolorTree.TreePoint> list = new List<RecolorTree.TreePoint>(aPoints.Count);
			for (int i = 0; i < aPoints.Count; i++)
			{
				Vector3 vector = aPoints[i];
				if (aTransform != null)
				{
					vector = aTransform.Value.MultiplyPoint(vector);
				}
				list.Add(new RecolorTree.TreePoint(vector, aColors[i]));
			}
			this.settings = new RecolorTree.TreeSettings(aX, aY, aZ);
			this.root = new RecolorTree.TreeNode(this.settings, list, 0);
		}

		// Token: 0x04000F4C RID: 3916
		private static RecolorTree.SortX sortX = new RecolorTree.SortX();

		// Token: 0x04000F4D RID: 3917
		private static RecolorTree.SortY sortY = new RecolorTree.SortY();

		// Token: 0x04000F4E RID: 3918
		private static RecolorTree.SortZ sortZ = new RecolorTree.SortZ();

		// Token: 0x04000F4F RID: 3919
		private RecolorTree.TreeNode root;

		// Token: 0x04000F50 RID: 3920
		private RecolorTree.TreeSettings settings;

		// Token: 0x020004A5 RID: 1189
		private class SortX : IComparer<RecolorTree.TreePoint>
		{
			// Token: 0x06001A06 RID: 6662 RVA: 0x00082512 File Offset: 0x00080712
			public int Compare(RecolorTree.TreePoint a, RecolorTree.TreePoint b)
			{
				return a.point.x.CompareTo(b.point.x);
			}
		}

		// Token: 0x020004A6 RID: 1190
		private class SortY : IComparer<RecolorTree.TreePoint>
		{
			// Token: 0x06001A08 RID: 6664 RVA: 0x0008252F File Offset: 0x0008072F
			public int Compare(RecolorTree.TreePoint a, RecolorTree.TreePoint b)
			{
				return a.point.y.CompareTo(b.point.y);
			}
		}

		// Token: 0x020004A7 RID: 1191
		private class SortZ : IComparer<RecolorTree.TreePoint>
		{
			// Token: 0x06001A0A RID: 6666 RVA: 0x0008254C File Offset: 0x0008074C
			public int Compare(RecolorTree.TreePoint a, RecolorTree.TreePoint b)
			{
				return a.point.z.CompareTo(b.point.z);
			}
		}

		// Token: 0x020004A8 RID: 1192
		public class TreeSettings
		{
			// Token: 0x06001A0C RID: 6668 RVA: 0x0008256C File Offset: 0x0008076C
			public TreeSettings(bool aUseX, bool aUseY, bool aUseZ)
			{
				List<int> list = new List<int>(3);
				if (aUseX)
				{
					list.Add(0);
				}
				if (aUseY)
				{
					list.Add(1);
				}
				if (aUseZ)
				{
					list.Add(2);
				}
				this.axes = list.ToArray();
			}

			// Token: 0x06001A0D RID: 6669 RVA: 0x000825B0 File Offset: 0x000807B0
			public int GetAxis(int aDepth)
			{
				int num = aDepth % this.axes.Length;
				return this.axes[num];
			}

			// Token: 0x06001A0E RID: 6670 RVA: 0x000825D0 File Offset: 0x000807D0
			public float AxisDist(int aAxis, Vector3 a, Vector3 b)
			{
				if (aAxis == 0)
				{
					return Mathf.Abs(a.x - b.x);
				}
				if (aAxis == 1)
				{
					return Mathf.Abs(a.y - b.y);
				}
				if (aAxis == 2)
				{
					return Mathf.Abs(a.z - b.z);
				}
				return 0f;
			}

			// Token: 0x0400178E RID: 6030
			private int[] axes;
		}

		// Token: 0x020004A9 RID: 1193
		public class TreePoint
		{
			// Token: 0x06001A0F RID: 6671 RVA: 0x00082626 File Offset: 0x00080826
			public TreePoint(Vector3 aPoint, Color aData)
			{
				this.point = aPoint;
				this.data = aData;
			}

			// Token: 0x0400178F RID: 6031
			public Vector3 point;

			// Token: 0x04001790 RID: 6032
			public Color data;
		}

		// Token: 0x020004AA RID: 1194
		private class TreeNode
		{
			// Token: 0x1700033C RID: 828
			// (get) Token: 0x06001A10 RID: 6672 RVA: 0x0008263C File Offset: 0x0008083C
			public bool IsLeaf
			{
				get
				{
					return this.left == null && this.right == null;
				}
			}

			// Token: 0x06001A11 RID: 6673 RVA: 0x00082654 File Offset: 0x00080854
			public TreeNode(RecolorTree.TreeSettings aSettings, List<RecolorTree.TreePoint> aPoints, int aDepth)
			{
				int axis = aSettings.GetAxis(aDepth);
				if (axis == 0)
				{
					aPoints.Sort(RecolorTree.sortX);
				}
				else if (axis == 1)
				{
					aPoints.Sort(RecolorTree.sortY);
				}
				else if (axis == 2)
				{
					aPoints.Sort(RecolorTree.sortZ);
				}
				int num = aPoints.Count / 2;
				this.point = aPoints[num];
				List<RecolorTree.TreePoint> range = aPoints.GetRange(0, num);
				List<RecolorTree.TreePoint> range2 = aPoints.GetRange(num + 1, aPoints.Count - (num + 1));
				if (range.Count > 0)
				{
					this.left = new RecolorTree.TreeNode(aSettings, range, aDepth + 1);
				}
				if (range2.Count > 0)
				{
					this.right = new RecolorTree.TreeNode(aSettings, range2, aDepth + 1);
				}
			}

			// Token: 0x06001A12 RID: 6674 RVA: 0x00082704 File Offset: 0x00080904
			public void GetNearest(RecolorTree.TreeSettings aSettings, int aDepth, Vector3 aPt, ref RecolorTree.TreePoint aClosest, ref float aClosestDist)
			{
				if (this.IsLeaf)
				{
					float sqrMagnitude = (this.point.point - aPt).sqrMagnitude;
					if (aClosest == null || sqrMagnitude < aClosestDist)
					{
						aClosest = this.point;
						aClosestDist = sqrMagnitude;
					}
					return;
				}
				int axis = aSettings.GetAxis(aDepth);
				bool flag = false;
				if (axis == 0)
				{
					flag = aPt.x <= this.point.point.x;
				}
				else if (axis == 1)
				{
					flag = aPt.y <= this.point.point.y;
				}
				else if (axis == 2)
				{
					flag = aPt.z <= this.point.point.z;
				}
				RecolorTree.TreeNode treeNode = (flag ? this.left : this.right);
				RecolorTree.TreeNode treeNode2 = (flag ? this.right : this.left);
				if (treeNode == null)
				{
					treeNode = treeNode2;
					treeNode2 = null;
				}
				treeNode.GetNearest(aSettings, aDepth + 1, aPt, ref aClosest, ref aClosestDist);
				float sqrMagnitude2 = (this.point.point - aPt).sqrMagnitude;
				if (sqrMagnitude2 < aClosestDist)
				{
					aClosest = this.point;
					aClosestDist = sqrMagnitude2;
				}
				if (treeNode2 != null)
				{
					float num = aSettings.AxisDist(axis, this.point.point, aPt);
					if (num * num <= aClosestDist)
					{
						treeNode2.GetNearest(aSettings, aDepth + 1, aPt, ref aClosest, ref aClosestDist);
					}
				}
			}

			// Token: 0x06001A13 RID: 6675 RVA: 0x00082858 File Offset: 0x00080A58
			public void Draw(RecolorTree.TreeSettings aSettings, int aDepth, Vector3 aPt)
			{
				int axis = aSettings.GetAxis(aDepth);
				if (axis == 0)
				{
					Gizmos.color = Color.red;
					Gizmos.DrawLine(this.point.point + Vector3.left, this.point.point + Vector3.right);
				}
				else if (axis == 1)
				{
					Gizmos.color = Color.green;
					Gizmos.DrawLine(this.point.point + Vector3.up, this.point.point + Vector3.down);
				}
				else if (axis == 2)
				{
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(this.point.point + Vector3.forward, this.point.point + Vector3.back);
				}
				if (this.left != null)
				{
					this.left.Draw(aSettings, aDepth + 1, this.point.point);
					Gizmos.color = this.point.data;
					Gizmos.DrawLine(this.point.point, this.left.point.point);
				}
				if (this.right != null)
				{
					this.right.Draw(aSettings, aDepth + 1, this.point.point);
					Gizmos.color = this.point.data;
					Gizmos.DrawLine(this.point.point, this.right.point.point);
				}
			}

			// Token: 0x04001791 RID: 6033
			private RecolorTree.TreePoint point;

			// Token: 0x04001792 RID: 6034
			private RecolorTree.TreeNode left;

			// Token: 0x04001793 RID: 6035
			private RecolorTree.TreeNode right;
		}
	}
}
