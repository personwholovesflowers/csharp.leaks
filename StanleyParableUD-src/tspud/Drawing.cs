using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class Drawing
{
	// Token: 0x060001D9 RID: 473 RVA: 0x0000CDEB File Offset: 0x0000AFEB
	public static Texture2D DrawLine(Vector2 from, Vector2 to, float w, Color col, Texture2D tex)
	{
		return Drawing.DrawLine(from, to, w, col, tex, false, Color.black, 0f);
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000CE04 File Offset: 0x0000B004
	public static Texture2D DrawLine(Vector2 from, Vector2 to, float w, Color col, Texture2D tex, bool stroke, Color strokeCol, float strokeWidth)
	{
		w = Mathf.Round(w);
		strokeWidth = Mathf.Round(strokeWidth);
		float num = w + strokeWidth;
		float num2 = Mathf.Clamp(Mathf.Min(from.y, to.y) - num, 0f, (float)tex.height);
		float num3 = Mathf.Clamp(Mathf.Min(from.x, to.x) - num, 0f, (float)tex.width);
		float num4 = Mathf.Clamp(Mathf.Max(from.y, to.y) + num, 0f, (float)tex.height);
		float num5 = Mathf.Clamp(Mathf.Max(from.x, to.x) + num, 0f, (float)tex.width);
		strokeWidth /= 2f;
		float num6 = (w - strokeWidth) * (w - strokeWidth);
		float num7 = (w + strokeWidth) * (w + strokeWidth);
		float num8 = (w + strokeWidth + 1f) * (w + strokeWidth + 1f);
		float num9 = w * w;
		float num10 = num5 - num3;
		float num11 = num4 - num2;
		Vector2 vector = new Vector2(num3, num2);
		Color[] pixels = tex.GetPixels((int)num3, (int)num2, (int)num10, (int)num11, 0);
		int num12 = 0;
		while ((float)num12 < num11)
		{
			int num13 = 0;
			while ((float)num13 < num10)
			{
				Vector2 vector2 = new Vector2((float)num13, (float)num12) + vector;
				Vector2 vector3 = vector2 + new Vector2(0.5f, 0.5f);
				float num14 = (vector3 - Mathfx.NearestPointStrict(from, to, vector3)).sqrMagnitude;
				if (num14 <= num8)
				{
					Vector2[] array = Drawing.Sample(vector2);
					Color color = Color.black;
					Color color2 = pixels[num12 * (int)num10 + num13];
					for (int i = 0; i < array.Length; i++)
					{
						num14 = (array[i] - Mathfx.NearestPointStrict(from, to, array[i])).sqrMagnitude;
						if (stroke)
						{
							if (num14 <= num7 && num14 >= num6)
							{
								color += strokeCol;
							}
							else if (num14 < num9)
							{
								color += col;
							}
							else
							{
								color += color2;
							}
						}
						else if (num14 < num9)
						{
							color += col;
						}
						else
						{
							color += color2;
						}
					}
					color /= (float)array.Length;
					pixels[num12 * (int)num10 + num13] = color;
				}
				num13++;
			}
			num12++;
		}
		tex.SetPixels((int)num3, (int)num2, (int)num10, (int)num11, pixels, 0);
		tex.Apply();
		return tex;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000D098 File Offset: 0x0000B298
	public static Texture2D Paint(Vector2 pos, float rad, Color col, float hardness, Texture2D tex)
	{
		Vector2 vector = new Vector2(Mathf.Clamp(pos.x - rad, 0f, (float)tex.width), Mathf.Clamp(pos.y - rad, 0f, (float)tex.height));
		Vector2 vector2 = new Vector2(Mathf.Clamp(pos.x + rad, 0f, (float)tex.width), Mathf.Clamp(pos.y + rad, 0f, (float)tex.height));
		float num = Mathf.Round(vector2.x - vector.x);
		float num2 = Mathf.Round(vector2.y - vector.y);
		float num3 = (rad + 1f) * (rad + 1f);
		Color[] pixels = tex.GetPixels((int)vector.x, (int)vector.y, (int)num, (int)num2, 0);
		int num4 = 0;
		while ((float)num4 < num2)
		{
			int num5 = 0;
			while ((float)num5 < num)
			{
				Vector2 vector3 = new Vector2((float)num5, (float)num4) + vector;
				float num6 = (vector3 + new Vector2(0.5f, 0.5f) - pos).sqrMagnitude;
				if (num6 <= num3)
				{
					Vector2[] array = Drawing.Sample(vector3);
					Color color = Color.black;
					for (int i = 0; i < array.Length; i++)
					{
						num6 = Mathfx.GaussFalloff(Vector2.Distance(array[i], pos), rad) * hardness;
						if (num6 > 0f)
						{
							color += Color.Lerp(pixels[num4 * (int)num + num5], col, num6);
						}
						else
						{
							color += pixels[num4 * (int)num + num5];
						}
					}
					color /= (float)array.Length;
					pixels[num4 * (int)num + num5] = color;
				}
				num5++;
			}
			num4++;
		}
		tex.SetPixels((int)vector.x, (int)vector.y, (int)num, (int)num2, pixels, 0);
		return tex;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000D296 File Offset: 0x0000B496
	public static void PaintLine(Vector2 from, Vector2 to, float rad, Color col, float strokeHardness, ref Texture2D tex)
	{
		Drawing.PaintLine(new Drawing.Stroke
		{
			start = from,
			end = to,
			width = rad,
			color = col,
			hardness = strokeHardness
		}, ref tex);
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
	public static void PaintLine(Drawing.Stroke stroke, ref Texture2D tex)
	{
		float width = stroke.width;
		float num = Mathf.Clamp(Mathf.Min(stroke.start.y, stroke.end.y) - width, 0f, (float)tex.height);
		float num2 = Mathf.Clamp(Mathf.Min(stroke.start.x, stroke.end.x) - width, 0f, (float)tex.width);
		float num3 = Mathf.Clamp(Mathf.Max(stroke.start.y, stroke.end.y) + width, 0f, (float)tex.height);
		float num4 = Mathf.Clamp(Mathf.Max(stroke.start.x, stroke.end.x) + width, 0f, (float)tex.width) - num2;
		float num5 = num3 - num;
		float num6 = (stroke.width + 1f) * (stroke.width + 1f);
		Vector2 vector = new Vector2(num2, num);
		Vector2 vector2 = default(Vector2);
		Vector2 vector3 = default(Vector2);
		for (int i = 0; i < (int)num5; i++)
		{
			for (int j = 0; j < (int)num4; j++)
			{
				vector2.Set((float)j, (float)i);
				vector2 += vector;
				vector3 = vector2 + Vector2.one * 0.5f;
				float num7 = (vector3 - Mathfx.NearestPointStrict(stroke.start, stroke.end, vector3)).sqrMagnitude;
				if (num7 <= num6)
				{
					num7 = Mathfx.GaussFalloff(Mathf.Sqrt(num7), stroke.width) * stroke.hardness;
					if (num7 > 0f)
					{
						tex.SetPixel(j + (int)num2, i + (int)num, stroke.color);
					}
				}
			}
		}
		tex.Apply(false);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
	public static void MergeTextures(ref Texture2D baseTex, ref Texture2D drawingTex, int downScalingRatio)
	{
		for (int i = 0; i < baseTex.height; i++)
		{
			for (int j = 0; j < baseTex.width; j++)
			{
				Color pixel = drawingTex.GetPixel(j / downScalingRatio, i / downScalingRatio);
				if (!(pixel == Color.clear))
				{
					baseTex.SetPixel(j, i, pixel);
				}
			}
		}
		baseTex.Apply(false);
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000D504 File Offset: 0x0000B704
	public static void DrawBezier(Drawing.BezierPoint[] points, float rad, Color col, Texture2D tex)
	{
		rad = Mathf.Round(rad);
		if (points.Length <= 1)
		{
			return;
		}
		Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
		Vector2 vector2 = new Vector2(0f, 0f);
		for (int i = 0; i < points.Length - 1; i++)
		{
			Vector2 main = points[i].main;
			Vector2 control = points[i].control2;
			Vector2 control2 = points[i + 1].control1;
			Vector2 main2 = points[i + 1].main;
			Drawing.BezierCurve bezierCurve = new Drawing.BezierCurve(main, control, control2, main2);
			points[i].curve2 = bezierCurve;
			points[i + 1].curve1 = bezierCurve;
			vector.x = Mathf.Min(vector.x, bezierCurve.rect.x);
			vector.y = Mathf.Min(vector.y, bezierCurve.rect.y);
			vector2.x = Mathf.Max(vector2.x, bezierCurve.rect.x + bezierCurve.rect.width);
			vector2.y = Mathf.Max(vector2.y, bezierCurve.rect.y + bezierCurve.rect.height);
		}
		vector -= new Vector2(rad, rad);
		vector2 += new Vector2(rad, rad);
		Vector2 vector3 = new Vector2(Mathf.Clamp(vector.x, 0f, (float)tex.width), Mathf.Clamp(vector.y, 0f, (float)tex.height));
		Vector2 vector4 = new Vector2(Mathf.Clamp(vector2.x - vector.x, 0f, (float)tex.width - vector3.x), Mathf.Clamp(vector2.y - vector.y, 0f, (float)tex.height - vector3.y));
		Color[] pixels = tex.GetPixels((int)vector3.x, (int)vector3.y, (int)vector4.x, (int)vector4.y, 0);
		int num = 0;
		while ((float)num < vector4.y)
		{
			int num2 = 0;
			while ((float)num2 < vector4.x)
			{
				Vector2 vector5 = new Vector2((float)num2 + vector3.x, (float)num + vector3.y);
				if (Mathfx.IsNearBeziers(vector5, points, rad + 2f))
				{
					Vector2[] array = Drawing.Sample(vector5);
					Color color = Color.black;
					Color color2 = pixels[num * (int)vector4.x + num2];
					for (int j = 0; j < array.Length; j++)
					{
						if (Mathfx.IsNearBeziers(array[j], points, rad))
						{
							color += col;
						}
						else
						{
							color += color2;
						}
					}
					color /= (float)array.Length;
					pixels[num * (int)vector4.x + num2] = color;
				}
				num2++;
			}
			num++;
		}
		tex.SetPixels((int)vector3.x, (int)vector3.y, (int)vector4.x, (int)vector4.y, pixels, 0);
		tex.Apply();
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000D820 File Offset: 0x0000BA20
	public static void AddP(List<Vector2> tmpList, Vector2 p, float ix, float iy)
	{
		float num = p.x + ix;
		float num2 = p.y + iy;
		tmpList.Add(new Vector2(num, num2));
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000D84C File Offset: 0x0000BA4C
	public static Vector2[] Sample(Vector2 p)
	{
		List<Vector2> list = new List<Vector2>(32);
		switch (Drawing.NumSamples)
		{
		case Drawing.Samples.None:
			Drawing.AddP(list, p, 0.5f, 0.5f);
			break;
		case Drawing.Samples.Samples2:
			Drawing.AddP(list, p, 0.25f, 0.5f);
			Drawing.AddP(list, p, 0.75f, 0.5f);
			break;
		case Drawing.Samples.Samples4:
			Drawing.AddP(list, p, 0.25f, 0.5f);
			Drawing.AddP(list, p, 0.75f, 0.5f);
			Drawing.AddP(list, p, 0.5f, 0.25f);
			Drawing.AddP(list, p, 0.5f, 0.75f);
			break;
		case Drawing.Samples.Samples8:
			Drawing.AddP(list, p, 0.25f, 0.5f);
			Drawing.AddP(list, p, 0.75f, 0.5f);
			Drawing.AddP(list, p, 0.5f, 0.25f);
			Drawing.AddP(list, p, 0.5f, 0.75f);
			Drawing.AddP(list, p, 0.25f, 0.25f);
			Drawing.AddP(list, p, 0.75f, 0.25f);
			Drawing.AddP(list, p, 0.25f, 0.75f);
			Drawing.AddP(list, p, 0.75f, 0.75f);
			break;
		case Drawing.Samples.Samples16:
			Drawing.AddP(list, p, 0f, 0f);
			Drawing.AddP(list, p, 0.3f, 0f);
			Drawing.AddP(list, p, 0.7f, 0f);
			Drawing.AddP(list, p, 1f, 0f);
			Drawing.AddP(list, p, 0f, 0.3f);
			Drawing.AddP(list, p, 0.3f, 0.3f);
			Drawing.AddP(list, p, 0.7f, 0.3f);
			Drawing.AddP(list, p, 1f, 0.3f);
			Drawing.AddP(list, p, 0f, 0.7f);
			Drawing.AddP(list, p, 0.3f, 0.7f);
			Drawing.AddP(list, p, 0.7f, 0.7f);
			Drawing.AddP(list, p, 1f, 0.7f);
			Drawing.AddP(list, p, 0f, 1f);
			Drawing.AddP(list, p, 0.3f, 1f);
			Drawing.AddP(list, p, 0.7f, 1f);
			Drawing.AddP(list, p, 1f, 1f);
			break;
		case Drawing.Samples.Samples32:
			Drawing.AddP(list, p, 0f, 0f);
			Drawing.AddP(list, p, 1f, 0f);
			Drawing.AddP(list, p, 0f, 1f);
			Drawing.AddP(list, p, 1f, 1f);
			Drawing.AddP(list, p, 0.2f, 0.2f);
			Drawing.AddP(list, p, 0.4f, 0.2f);
			Drawing.AddP(list, p, 0.6f, 0.2f);
			Drawing.AddP(list, p, 0.8f, 0.2f);
			Drawing.AddP(list, p, 0.2f, 0.4f);
			Drawing.AddP(list, p, 0.4f, 0.4f);
			Drawing.AddP(list, p, 0.6f, 0.4f);
			Drawing.AddP(list, p, 0.8f, 0.4f);
			Drawing.AddP(list, p, 0.2f, 0.6f);
			Drawing.AddP(list, p, 0.4f, 0.6f);
			Drawing.AddP(list, p, 0.6f, 0.6f);
			Drawing.AddP(list, p, 0.8f, 0.6f);
			Drawing.AddP(list, p, 0.2f, 0.8f);
			Drawing.AddP(list, p, 0.4f, 0.8f);
			Drawing.AddP(list, p, 0.6f, 0.8f);
			Drawing.AddP(list, p, 0.8f, 0.8f);
			Drawing.AddP(list, p, 0.5f, 0f);
			Drawing.AddP(list, p, 0.5f, 1f);
			Drawing.AddP(list, p, 0f, 0.5f);
			Drawing.AddP(list, p, 1f, 0.5f);
			Drawing.AddP(list, p, 0.5f, 0.5f);
			break;
		case Drawing.Samples.RotatedDisc:
		{
			Drawing.AddP(list, p, 0f, 0f);
			Drawing.AddP(list, p, 1f, 0f);
			Drawing.AddP(list, p, 0f, 1f);
			Drawing.AddP(list, p, 1f, 1f);
			Vector2 vector = new Vector2(p.x + 0.5f, p.y + 0.5f);
			Drawing.AddP(list, vector, 0.258f, 0.965f);
			Drawing.AddP(list, vector, -0.965f, -0.258f);
			Drawing.AddP(list, vector, 0.965f, 0.258f);
			Drawing.AddP(list, vector, 0.258f, -0.965f);
			break;
		}
		}
		return list.ToArray();
	}

	// Token: 0x04000218 RID: 536
	public static Drawing.Samples NumSamples = Drawing.Samples.Samples4;

	// Token: 0x02000366 RID: 870
	public enum Samples
	{
		// Token: 0x04001250 RID: 4688
		None,
		// Token: 0x04001251 RID: 4689
		Samples2,
		// Token: 0x04001252 RID: 4690
		Samples4,
		// Token: 0x04001253 RID: 4691
		Samples8,
		// Token: 0x04001254 RID: 4692
		Samples16,
		// Token: 0x04001255 RID: 4693
		Samples32,
		// Token: 0x04001256 RID: 4694
		RotatedDisc
	}

	// Token: 0x02000367 RID: 871
	public class Stroke
	{
		// Token: 0x060015E5 RID: 5605 RVA: 0x00074C5B File Offset: 0x00072E5B
		public void Set(Vector2 Start, Vector2 End, float Width, float Hardness, Color Color)
		{
			this.start = Start;
			this.end = End;
			this.width = Width;
			this.hardness = Hardness;
			this.color = Color;
		}

		// Token: 0x04001257 RID: 4695
		public Vector2 start;

		// Token: 0x04001258 RID: 4696
		public Vector2 end;

		// Token: 0x04001259 RID: 4697
		public float width;

		// Token: 0x0400125A RID: 4698
		public float hardness;

		// Token: 0x0400125B RID: 4699
		public Color color;
	}

	// Token: 0x02000368 RID: 872
	public class BezierPoint
	{
		// Token: 0x060015E7 RID: 5607 RVA: 0x00074C82 File Offset: 0x00072E82
		internal BezierPoint(Vector2 m, Vector2 l, Vector2 r)
		{
			this.main = m;
			this.control1 = l;
			this.control2 = r;
		}

		// Token: 0x0400125C RID: 4700
		internal Vector2 main;

		// Token: 0x0400125D RID: 4701
		internal Vector2 control1;

		// Token: 0x0400125E RID: 4702
		internal Vector2 control2;

		// Token: 0x0400125F RID: 4703
		internal Drawing.BezierCurve curve1;

		// Token: 0x04001260 RID: 4704
		internal Drawing.BezierCurve curve2;
	}

	// Token: 0x02000369 RID: 873
	public class BezierCurve
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x00074CA0 File Offset: 0x00072EA0
		internal Vector2 Get(float t)
		{
			int num = (int)Mathf.Round(t * (float)(this.points.Length - 1));
			return this.points[num];
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00074CD0 File Offset: 0x00072ED0
		private void Init(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			vector.x = Mathf.Min(vector.x, p0.x);
			vector.x = Mathf.Min(vector.x, p1.x);
			vector.x = Mathf.Min(vector.x, p2.x);
			vector.x = Mathf.Min(vector.x, p3.x);
			vector.y = Mathf.Min(vector.y, p0.y);
			vector.y = Mathf.Min(vector.y, p1.y);
			vector.y = Mathf.Min(vector.y, p2.y);
			vector.y = Mathf.Min(vector.y, p3.y);
			vector2.x = Mathf.Max(vector2.x, p0.x);
			vector2.x = Mathf.Max(vector2.x, p1.x);
			vector2.x = Mathf.Max(vector2.x, p2.x);
			vector2.x = Mathf.Max(vector2.x, p3.x);
			vector2.y = Mathf.Max(vector2.y, p0.y);
			vector2.y = Mathf.Max(vector2.y, p1.y);
			vector2.y = Mathf.Max(vector2.y, p2.y);
			vector2.y = Mathf.Max(vector2.y, p3.y);
			this.rect = new Rect(vector.x, vector.y, vector2.x - vector.x, vector2.y - vector.y);
			List<Vector2> list = new List<Vector2>();
			Vector2 vector3 = Mathfx.CubicBezier(0f, p0, p1, p2, p3);
			Vector2 vector4 = Mathfx.CubicBezier(0.05f, p0, p1, p2, p3);
			Vector2 vector5 = Mathfx.CubicBezier(0.1f, p0, p1, p2, p3);
			Vector2 vector6 = Mathfx.CubicBezier(0.15f, p0, p1, p2, p3);
			Vector2 vector7 = Mathfx.CubicBezier(0.5f, p0, p1, p2, p3);
			Vector2 vector8 = Mathfx.CubicBezier(0.55f, p0, p1, p2, p3);
			Vector2 vector9 = Mathfx.CubicBezier(0.6f, p0, p1, p2, p3);
			this.aproxLength = Vector2.Distance(vector3, vector4) + Vector2.Distance(vector4, vector5) + Vector2.Distance(vector5, vector6) + Vector2.Distance(vector7, vector8) + Vector2.Distance(vector8, vector9);
			Debug.Log(string.Concat(new object[]
			{
				Vector2.Distance(vector3, vector4),
				"     ",
				Vector2.Distance(vector5, vector6),
				"   ",
				Vector2.Distance(vector8, vector9)
			}));
			this.aproxLength *= 4f;
			float num = 0.5f / this.aproxLength;
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				list.Add(Mathfx.CubicBezier(num2, p0, p1, p2, p3));
			}
			this.points = list.ToArray();
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x0007500F File Offset: 0x0007320F
		internal BezierCurve(Vector2 main, Vector2 control1, Vector2 control2, Vector2 end)
		{
			this.Init(main, control1, control2, end);
		}

		// Token: 0x04001261 RID: 4705
		internal Vector2[] points;

		// Token: 0x04001262 RID: 4706
		internal float aproxLength;

		// Token: 0x04001263 RID: 4707
		internal Rect rect;
	}
}
