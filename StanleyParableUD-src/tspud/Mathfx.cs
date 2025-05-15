using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class Mathfx
{
	// Token: 0x060001F0 RID: 496 RVA: 0x0000E625 File Offset: 0x0000C825
	public static float Hermite(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value * value * (3f - 2f * value));
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x0000E63F File Offset: 0x0000C83F
	public static float Sinerp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, Mathf.Sin(value * 3.1415927f * 0.5f));
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0000E65A File Offset: 0x0000C85A
	public static float Coserp(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, 1f - Mathf.Cos(value * 3.1415927f * 0.5f));
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x0000E67C File Offset: 0x0000C87C
	public static float Berp(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x0000E6E0 File Offset: 0x0000C8E0
	public static float SmoothStep(float x, float min, float max)
	{
		x = Mathf.Clamp(x, min, max);
		float num = (x - min) / (max - min);
		float num2 = (x - min) / (max - min);
		return -2f * num * num * num + 3f * num2 * num2;
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x0000E71C File Offset: 0x0000C91C
	public static float Lerp(float start, float end, float value)
	{
		return (1f - value) * start + value * end;
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0000E72C File Offset: 0x0000C92C
	public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = Vector3.Normalize(lineEnd - lineStart);
		float num = Vector3.Dot(point - lineStart, vector) / Vector3.Dot(vector, vector);
		return lineStart + num * vector;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0000E76C File Offset: 0x0000C96C
	public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		Vector3 vector = lineEnd - lineStart;
		Vector3 vector2 = Vector3.Normalize(vector);
		float num = Vector3.Dot(point - lineStart, vector2) / Vector3.Dot(vector2, vector2);
		return lineStart + Mathf.Clamp(num, 0f, Vector3.Magnitude(vector)) * vector2;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0000E7BC File Offset: 0x0000C9BC
	public static Vector2 NearestPointStrict(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
	{
		Vector2 vector = lineEnd - lineStart;
		Vector2 vector2 = Mathfx.Normalize(vector);
		float num = Vector2.Dot(point - lineStart, vector2) / Vector2.Dot(vector2, vector2);
		return lineStart + Mathf.Clamp(num, 0f, vector.magnitude) * vector2;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0000E80C File Offset: 0x0000CA0C
	public static float Bounce(float x)
	{
		return Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0000E835 File Offset: 0x0000CA35
	public static bool Approx(float val, float about, float range)
	{
		return Mathf.Abs(val - about) < range;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x0000E844 File Offset: 0x0000CA44
	public static bool Approx(Vector3 val, Vector3 about, float range)
	{
		return (val - about).sqrMagnitude < range * range;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x0000E865 File Offset: 0x0000CA65
	public static float GaussFalloff(float distance, float inRadius)
	{
		return Mathf.Clamp01(Mathf.Pow(360f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000E88C File Offset: 0x0000CA8C
	public static float Clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num5;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			num5 = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			num5 = start + num4;
		}
		else
		{
			num5 = start + (end - start) * value;
		}
		return num5;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
	public static Vector2 RotateVector(Vector2 vector, float rad)
	{
		rad *= 0.017453292f;
		return new Vector2(vector.x * Mathf.Cos(rad) - vector.y * Mathf.Sin(rad), vector.x * Mathf.Sin(rad) + vector.y * Mathf.Cos(rad));
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000E94C File Offset: 0x0000CB4C
	public static Vector2 IntersectPoint(Vector2 start1, Vector2 start2, Vector2 dir1, Vector2 dir2)
	{
		if (dir1.x == dir2.x)
		{
			return Vector2.zero;
		}
		float num = dir1.y / dir1.x;
		float num2 = dir2.y / dir2.x;
		if (num == num2)
		{
			return Vector2.zero;
		}
		Vector2 vector = new Vector2(num, start1.y - start1.x * num);
		Vector2 vector2 = new Vector2(num2, start2.y - start2.x * num2);
		float num3 = vector2.y - vector.y;
		float num4 = vector.x - vector2.x;
		float num5 = num3 / num4;
		float num6 = vector.x * num5 + vector.y;
		return new Vector2(num5, num6);
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000EA00 File Offset: 0x0000CC00
	public static Vector2 ThreePointCircle(Vector2 a1, Vector2 a2, Vector2 a3)
	{
		Vector2 vector = a2 - a1;
		vector /= 2f;
		Vector2 vector2 = a1 + vector;
		vector = Mathfx.RotateVector(vector, 90f);
		Vector2 vector3 = vector;
		vector = a3 - a2;
		vector /= 2f;
		Vector2 vector4 = a2 + vector;
		vector = Mathfx.RotateVector(vector, 90f);
		Vector2 vector5 = vector;
		return Mathfx.IntersectPoint(vector2, vector4, vector3, vector5);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000EA68 File Offset: 0x0000CC68
	public static Vector2 CubicBezier(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		return Vector2.zero;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000EA70 File Offset: 0x0000CC70
	public static Vector2 NearestPointOnBezier(Vector2 p, Drawing.BezierCurve c, float accuracy, bool doubleAc)
	{
		float num = float.PositiveInfinity;
		float num2 = 0f;
		Vector2 vector = Vector2.zero;
		for (float num3 = 0f; num3 < 1f; num3 += accuracy)
		{
			Vector2 vector2 = c.Get(num3);
			float sqrMagnitude = (p - vector2).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				num2 = num3;
				vector = vector2;
			}
		}
		if (!doubleAc)
		{
			return vector;
		}
		float num4 = Mathf.Clamp01(num2 - accuracy);
		float num5 = Mathf.Clamp01(num2 + accuracy);
		for (float num6 = num4; num6 < num5; num6 += accuracy / 10f)
		{
			Vector2 vector3 = c.Get(num6);
			float sqrMagnitude2 = (p - vector3).sqrMagnitude;
			if (sqrMagnitude2 < num)
			{
				num = sqrMagnitude2;
				vector = vector3;
			}
		}
		return vector;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0000EB30 File Offset: 0x0000CD30
	public static bool IsNearBezierTest(Vector2 p, Drawing.BezierCurve c, float accuracy, float maxDist)
	{
		Vector2 vector = c.Get(0f);
		for (float num = accuracy; num < 1f; num += accuracy)
		{
			Vector2 vector2 = c.Get(num);
			float sqrMagnitude = (p - vector2).sqrMagnitude;
			float sqrMagnitude2 = (vector - vector2 + new Vector2(maxDist, maxDist)).sqrMagnitude;
			if (sqrMagnitude <= sqrMagnitude2 * 2f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0000EB9C File Offset: 0x0000CD9C
	public static Vector2 NearestPointOnBezier(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float num = float.PositiveInfinity;
		float num2 = 0f;
		Vector2 vector = Vector2.zero;
		for (float num3 = 0f; num3 < 1f; num3 += 0.01f)
		{
			Vector2 vector2 = Mathfx.CubicBezier(num3, p0, p1, p2, p3);
			float sqrMagnitude = (p - vector2).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				num = sqrMagnitude;
				num2 = num3;
				vector = vector2;
			}
		}
		float num4 = Mathf.Clamp01(num2 - 0.01f);
		float num5 = Mathf.Clamp01(num2 + 0.01f);
		for (float num6 = num4; num6 < num5; num6 += 0.001f)
		{
			Vector2 vector3 = Mathfx.CubicBezier(num6, p0, p1, p2, p3);
			float sqrMagnitude2 = (p - vector3).sqrMagnitude;
			if (sqrMagnitude2 < num)
			{
				num = sqrMagnitude2;
				vector = vector3;
			}
		}
		return vector;
	}

	// Token: 0x06000205 RID: 517 RVA: 0x0000EC68 File Offset: 0x0000CE68
	public static bool IsNearBezier(Vector2 p, Drawing.BezierPoint point1, Drawing.BezierPoint point2, float rad)
	{
		if (point1.curve2 != point2.curve1)
		{
			Debug.LogError("Curves Not The Same");
			return false;
		}
		Drawing.BezierCurve curve = point1.curve2;
		Rect rect = curve.rect;
		rect.x -= rad;
		rect.y -= rad;
		rect.width += rad * 2f;
		rect.height += rad * 2f;
		if (!rect.Contains(p))
		{
			return false;
		}
		Vector2 vector = Mathfx.NearestPointOnBezier(p, curve, 0.1f, false);
		float num = point1.curve2.aproxLength / 10f;
		return (vector - p).sqrMagnitude < num * 3f * (num * 3f) && (Mathfx.NearestPointOnBezier(p, curve, 0.01f, true) - p).sqrMagnitude <= rad * rad;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x0000ED54 File Offset: 0x0000CF54
	public static bool IsNearBeziers(Vector2 p, Drawing.BezierPoint[] points, float rad)
	{
		for (int i = 0; i < points.Length - 1; i++)
		{
			if (Mathfx.IsNearBezier(p, points[i], points[i + 1], rad))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000ED88 File Offset: 0x0000CF88
	public static Vector2 NearestPointOnCircle(Vector2 p, Vector2 center, float w)
	{
		Vector2 vector = p - center;
		vector = Mathfx.Normalize(vector);
		vector *= w;
		return center + vector;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
	public static Vector2 Normalize(Vector2 p)
	{
		float magnitude = p.magnitude;
		return p / magnitude;
	}
}
