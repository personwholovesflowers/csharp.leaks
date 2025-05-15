using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000317 RID: 791
	public class Triangulator
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0006B60D File Offset: 0x0006980D
		public List<Vector2> Points
		{
			get
			{
				return this.m_points;
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0006B615 File Offset: 0x00069815
		public Triangulator(Vector2[] points)
		{
			this.m_points = new List<Vector2>(points);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0006B634 File Offset: 0x00069834
		public Triangulator(Vector2[] points, bool invertY = true)
		{
			if (invertY)
			{
				this.m_points = new List<Vector2>();
				for (int i = 0; i < points.Length; i++)
				{
					this.m_points.Add(new Vector2(points[i].x, 1f - points[i].y));
				}
				return;
			}
			this.m_points = new List<Vector2>(points);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x0006B6AC File Offset: 0x000698AC
		public int[] Triangulate()
		{
			List<int> list = new List<int>();
			int count = this.m_points.Count;
			if (count < 3)
			{
				return list.ToArray();
			}
			int[] array = new int[count];
			if (this.Area() > 0f)
			{
				for (int i = 0; i < count; i++)
				{
					array[i] = i;
				}
			}
			else
			{
				for (int j = 0; j < count; j++)
				{
					array[j] = count - 1 - j;
				}
			}
			int k = count;
			int num = 2 * k;
			int num2 = 0;
			int num3 = k - 1;
			while (k > 2)
			{
				if (num-- <= 0)
				{
					return list.ToArray();
				}
				int num4 = num3;
				if (k <= num4)
				{
					num4 = 0;
				}
				num3 = num4 + 1;
				if (k <= num3)
				{
					num3 = 0;
				}
				int num5 = num3 + 1;
				if (k <= num5)
				{
					num5 = 0;
				}
				if (this.Snip(num4, num3, num5, k, array))
				{
					int num6 = array[num4];
					int num7 = array[num3];
					int num8 = array[num5];
					list.Add(num6);
					list.Add(num7);
					list.Add(num8);
					num2++;
					int num9 = num3;
					for (int l = num3 + 1; l < k; l++)
					{
						array[num9] = array[l];
						num9++;
					}
					k--;
					num = 2 * k;
				}
			}
			list.Reverse();
			return list.ToArray();
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0006B7EC File Offset: 0x000699EC
		private float Area()
		{
			int count = this.m_points.Count;
			float num = 0f;
			int num2 = count - 1;
			int i = 0;
			while (i < count)
			{
				Vector2 vector = this.m_points[num2];
				Vector2 vector2 = this.m_points[i];
				num += vector.x * vector2.y - vector2.x * vector.y;
				num2 = i++;
			}
			return num * 0.5f;
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0006B864 File Offset: 0x00069A64
		private bool Snip(int u, int v, int w, int n, int[] V)
		{
			Vector2 vector = this.m_points[V[u]];
			Vector2 vector2 = this.m_points[V[v]];
			Vector2 vector3 = this.m_points[V[w]];
			if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
			{
				return false;
			}
			for (int i = 0; i < n; i++)
			{
				if (i != u && i != v && i != w)
				{
					Vector2 vector4 = this.m_points[V[i]];
					if (this.InsideTriangle(vector4, vector, vector2, vector3))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0006B91C File Offset: 0x00069B1C
		private bool InsideTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
		{
			bool flag = pt.Cross(v1, v2) < 0f;
			bool flag2 = pt.Cross(v2, v3) < 0f;
			bool flag3 = pt.Cross(v3, v1) < 0f;
			return flag == flag2 && flag2 == flag3;
		}

		// Token: 0x0400102D RID: 4141
		private List<Vector2> m_points = new List<Vector2>();
	}
}
