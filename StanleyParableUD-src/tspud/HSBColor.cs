using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[Serializable]
public struct HSBColor
{
	// Token: 0x060001E7 RID: 487 RVA: 0x0000E0CC File Offset: 0x0000C2CC
	public HSBColor(float h, float s, float b, float a)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = a;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000E0EB File Offset: 0x0000C2EB
	public HSBColor(float h, float s, float b)
	{
		this.h = h;
		this.s = s;
		this.b = b;
		this.a = 1f;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000E110 File Offset: 0x0000C310
	public HSBColor(Color col)
	{
		HSBColor hsbcolor = HSBColor.FromColor(col);
		this.h = hsbcolor.h;
		this.s = hsbcolor.s;
		this.b = hsbcolor.b;
		this.a = hsbcolor.a;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000E154 File Offset: 0x0000C354
	public static HSBColor FromColor(Color color)
	{
		HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
		float r = color.r;
		float g = color.g;
		float num = color.b;
		float num2 = Mathf.Max(r, Mathf.Max(g, num));
		if (num2 <= 0f)
		{
			return hsbcolor;
		}
		float num3 = Mathf.Min(r, Mathf.Min(g, num));
		float num4 = num2 - num3;
		if (num2 > num3)
		{
			if (g == num2)
			{
				hsbcolor.h = (num - r) / num4 * 60f + 120f;
			}
			else if (num == num2)
			{
				hsbcolor.h = (r - g) / num4 * 60f + 240f;
			}
			else if (num > g)
			{
				hsbcolor.h = (g - num) / num4 * 60f + 360f;
			}
			else
			{
				hsbcolor.h = (g - num) / num4 * 60f;
			}
			if (hsbcolor.h < 0f)
			{
				hsbcolor.h += 360f;
			}
		}
		else
		{
			hsbcolor.h = 0f;
		}
		hsbcolor.h *= 0.0027777778f;
		hsbcolor.s = num4 / num2 * 1f;
		hsbcolor.b = num2;
		return hsbcolor;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000E298 File Offset: 0x0000C498
	public static Color ToColor(HSBColor hsbColor)
	{
		float num = hsbColor.b;
		float num2 = hsbColor.b;
		float num3 = hsbColor.b;
		if (hsbColor.s != 0f)
		{
			float num4 = hsbColor.b;
			float num5 = hsbColor.b * hsbColor.s;
			float num6 = hsbColor.b - num5;
			float num7 = hsbColor.h * 360f;
			if (num7 < 60f)
			{
				num = num4;
				num2 = num7 * num5 / 60f + num6;
				num3 = num6;
			}
			else if (num7 < 120f)
			{
				num = -(num7 - 120f) * num5 / 60f + num6;
				num2 = num4;
				num3 = num6;
			}
			else if (num7 < 180f)
			{
				num = num6;
				num2 = num4;
				num3 = (num7 - 120f) * num5 / 60f + num6;
			}
			else if (num7 < 240f)
			{
				num = num6;
				num2 = -(num7 - 240f) * num5 / 60f + num6;
				num3 = num4;
			}
			else if (num7 < 300f)
			{
				num = (num7 - 240f) * num5 / 60f + num6;
				num2 = num6;
				num3 = num4;
			}
			else if (num7 <= 360f)
			{
				num = num4;
				num2 = num6;
				num3 = -(num7 - 360f) * num5 / 60f + num6;
			}
			else
			{
				num = 0f;
				num2 = 0f;
				num3 = 0f;
			}
		}
		return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
	}

	// Token: 0x060001EC RID: 492 RVA: 0x0000E40C File Offset: 0x0000C60C
	public Color ToColor()
	{
		return HSBColor.ToColor(this);
	}

	// Token: 0x060001ED RID: 493 RVA: 0x0000E41C File Offset: 0x0000C61C
	public override string ToString()
	{
		return string.Concat(new object[] { "H:", this.h, " S:", this.s, " B:", this.b });
	}

	// Token: 0x060001EE RID: 494 RVA: 0x0000E478 File Offset: 0x0000C678
	public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
	{
		float num;
		for (num = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num < 0f; num += 360f)
		{
		}
		while (num > 360f)
		{
			num -= 360f;
		}
		return new HSBColor(num / 360f, Mathf.Lerp(a.s, b.s, t), Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
	}

	// Token: 0x060001EF RID: 495 RVA: 0x0000E50C File Offset: 0x0000C70C
	public static void Test()
	{
		HSBColor hsbcolor = new HSBColor(Color.red);
		Debug.Log("red: " + hsbcolor);
		hsbcolor = new HSBColor(Color.green);
		Debug.Log("green: " + hsbcolor);
		hsbcolor = new HSBColor(Color.blue);
		Debug.Log("blue: " + hsbcolor);
		hsbcolor = new HSBColor(Color.grey);
		Debug.Log("grey: " + hsbcolor);
		hsbcolor = new HSBColor(Color.white);
		Debug.Log("white: " + hsbcolor);
		hsbcolor = new HSBColor(new Color(0.4f, 1f, 0.84f, 1f));
		Debug.Log("0.4, 1f, 0.84: " + hsbcolor);
		Debug.Log("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + HSBColor.ToColor(new HSBColor(new Color(0.643137f, 0.321568f, 0.329411f))));
	}

	// Token: 0x04000219 RID: 537
	public float h;

	// Token: 0x0400021A RID: 538
	public float s;

	// Token: 0x0400021B RID: 539
	public float b;

	// Token: 0x0400021C RID: 540
	public float a;
}
