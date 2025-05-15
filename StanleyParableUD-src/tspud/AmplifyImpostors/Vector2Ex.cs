﻿using System;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000315 RID: 789
	public static class Vector2Ex
	{
		// Token: 0x0600140A RID: 5130 RVA: 0x0006AE03 File Offset: 0x00069003
		public static float Cross(this Vector2 O, Vector2 A, Vector2 B)
		{
			return (A.x - O.x) * (B.y - O.y) - (A.y - O.y) * (B.x - O.x);
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0006AE3C File Offset: 0x0006903C
		public static float TriangleArea(this Vector2 O, Vector2 A, Vector2 B)
		{
			return Mathf.Abs((A.x - B.x) * (O.y - A.y) - (A.x - O.x) * (B.y - A.y)) * 0.5f;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0006AE8C File Offset: 0x0006908C
		public static float TriangleArea(this Vector3 O, Vector3 A, Vector3 B)
		{
			return Mathf.Abs((A.x - B.x) * (O.y - A.y) - (A.x - O.x) * (B.y - A.y)) * 0.5f;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0006AEDC File Offset: 0x000690DC
		public static Vector2[] ConvexHull(Vector2[] P)
		{
			if (P.Length > 1)
			{
				int num = P.Length;
				int num2 = 0;
				Vector2[] array = new Vector2[2 * num];
				Comparison<Vector2> comparison = delegate(Vector2 a, Vector2 b)
				{
					if (a.x == b.x)
					{
						return a.y.CompareTo(b.y);
					}
					return a.x.CompareTo(b.x);
				};
				Array.Sort<Vector2>(P, comparison);
				for (int i = 0; i < num; i++)
				{
					while (num2 >= 2 && P[i].Cross(array[num2 - 2], array[num2 - 1]) <= 0f)
					{
						num2--;
					}
					array[num2++] = P[i];
				}
				int j = num - 2;
				int num3 = num2 + 1;
				while (j >= 0)
				{
					while (num2 >= num3 && P[j].Cross(array[num2 - 2], array[num2 - 1]) <= 0f)
					{
						num2--;
					}
					array[num2++] = P[j];
					j--;
				}
				if (num2 > 1)
				{
					Array.Resize<Vector2>(ref array, num2 - 1);
				}
				return array;
			}
			if (P.Length <= 1)
			{
				return P;
			}
			return null;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0006AFEC File Offset: 0x000691EC
		public static Vector2[] ScaleAlongNormals(Vector2[] P, float scaleAmount)
		{
			Vector2[] array = new Vector2[P.Length];
			for (int i = 0; i < array.Length; i++)
			{
				int num = i - 1;
				int num2 = i + 1;
				if (i == 0)
				{
					num = P.Length - 1;
				}
				if (i == P.Length - 1)
				{
					num2 = 0;
				}
				Vector2 vector = P[i] - P[num];
				Vector2 vector2 = P[i] - P[num2];
				Vector2 normalized = (vector.normalized + vector2.normalized).normalized;
				array[i] = normalized;
			}
			for (int j = 0; j < array.Length; j++)
			{
				P[j] += array[j] * scaleAmount;
			}
			return P;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0006B0B4 File Offset: 0x000692B4
		private static Vector2[] ReduceLeastSignificantVertice(Vector2[] P)
		{
			float num = 0f;
			int smallestIndex = 0;
			int num2 = 0;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < P.Length; i++)
			{
				int num3 = i + 1;
				int num4 = i + 2;
				int num5 = i + 3;
				if (num3 >= P.Length)
				{
					num3 -= P.Length;
				}
				if (num4 >= P.Length)
				{
					num4 -= P.Length;
				}
				if (num5 >= P.Length)
				{
					num5 -= P.Length;
				}
				Vector2 intersectionPointCoordinates = Vector2Ex.GetIntersectionPointCoordinates(P[i], P[num3], P[num4], P[num5]);
				if (i == 0)
				{
					num = intersectionPointCoordinates.TriangleArea(P[num3], P[num4]);
					if (Vector2Ex.OutsideBounds(intersectionPointCoordinates) > 0f)
					{
						num += Vector2Ex.OutsideBounds(intersectionPointCoordinates) * 1f;
					}
					smallestIndex = num3;
					num2 = num4;
					vector = intersectionPointCoordinates;
				}
				else
				{
					float num6 = intersectionPointCoordinates.TriangleArea(P[num3], P[num4]);
					if (Vector2Ex.OutsideBounds(intersectionPointCoordinates) > 0f)
					{
						num6 += Vector2Ex.OutsideBounds(intersectionPointCoordinates) * 1f;
					}
					if (num6 < num && Vector2Ex.OutsideBounds(intersectionPointCoordinates) <= 0f)
					{
						num = num6;
						smallestIndex = num3;
						num2 = num4;
						vector = intersectionPointCoordinates;
					}
				}
			}
			P[num2] = vector;
			return Array.FindAll<Vector2>(P, (Vector2 x) => Array.IndexOf<Vector2>(P, x) != smallestIndex);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0006B278 File Offset: 0x00069478
		public static Vector2[] ReduceVertices(Vector2[] P, int maxVertices)
		{
			if (maxVertices == 4)
			{
				Rect rect = new Rect(P[0].x, P[0].y, 0f, 0f);
				for (int i = 0; i < P.Length; i++)
				{
					rect.xMin = Mathf.Min(rect.xMin, P[i].x);
					rect.xMax = Mathf.Max(rect.xMax, P[i].x);
					rect.yMin = Mathf.Min(rect.yMin, P[i].y);
					rect.yMax = Mathf.Max(rect.yMax, P[i].y);
				}
				P = new Vector2[]
				{
					new Vector2(rect.xMin, rect.yMin),
					new Vector2(rect.xMax, rect.yMin),
					new Vector2(rect.xMax, rect.yMax),
					new Vector2(rect.xMin, rect.yMax)
				};
			}
			else
			{
				int num = Math.Max(0, P.Length - maxVertices);
				for (int j = 0; j < num; j++)
				{
					P = Vector2Ex.ReduceLeastSignificantVertice(P);
				}
			}
			return P;
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0006B3D8 File Offset: 0x000695D8
		private static Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2)
		{
			float num = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
			if (num == 0f)
			{
				return (Vector2.Lerp(A2, B1, 0.5f) - Vector2.one * 0.5f) * 1000f + Vector2.one * 500f;
			}
			float num2 = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / num;
			return new Vector2(B1.x + (B2.x - B1.x) * num2, B1.y + (B2.y - B1.y) * num2);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0006B4D0 File Offset: 0x000696D0
		private static float OutsideBounds(Vector2 P)
		{
			P -= Vector2.one * 0.5f;
			float num = Mathf.Clamp01(Mathf.Abs(P.y) - 0.5f);
			return Mathf.Clamp01(Mathf.Abs(P.x) - 0.5f) + num;
		}
	}
}
