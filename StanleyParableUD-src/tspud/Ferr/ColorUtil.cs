using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002E1 RID: 737
	public static class ColorUtil
	{
		// Token: 0x0600132F RID: 4911 RVA: 0x0006635C File Offset: 0x0006455C
		public static Color HSL(float aHue, float aSaturation, float aLuminance)
		{
			float num = aHue % 1f * 360f / 60f;
			float num2 = (1f - Mathf.Abs(2f * aLuminance - 1f)) * aSaturation;
			float num3 = num2 * (1f - Mathf.Abs(num % 2f - 1f));
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			if (num < 1f)
			{
				num4 = num2;
				num5 = num3;
			}
			else if (num < 2f)
			{
				num4 = num3;
				num5 = num2;
			}
			else if (num < 3f)
			{
				num5 = num2;
				num6 = num3;
			}
			else if (num < 4f)
			{
				num5 = num3;
				num6 = num2;
			}
			else if (num < 5f)
			{
				num4 = num3;
				num6 = num2;
			}
			else
			{
				num4 = num2;
				num6 = num3;
			}
			float num7 = aLuminance - 0.5f * num2;
			return new Color(num4 + num7, num5 + num7, num6 + num7, 1f);
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0006643C File Offset: 0x0006463C
		public static Vector3 ToHSV(float aR, float aG, float aB)
		{
			float num = Mathf.Max(aR, Mathf.Max(aG, aB));
			float num2 = Mathf.Min(aR, Mathf.Min(aG, aB));
			float num3 = Mathf.Atan2(2f * aR - aG - aB, ColorUtil.sqrt3 * (aG - aB));
			float num4 = ((num == 0f) ? 0f : (1f - 1f * num2 / num));
			float num5 = num;
			return new Vector3(num3, num4, num5);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x000664A8 File Offset: 0x000646A8
		public static Color HSV(float aHue, float aSaturation, float aValue)
		{
			float num = aHue % 1f * 360f / 60f;
			float num2 = aValue * aSaturation;
			float num3 = num2 * (1f - Mathf.Abs(num % 2f - 1f));
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			if (num < 1f)
			{
				num4 = num2;
				num5 = num3;
			}
			else if (num < 2f)
			{
				num4 = num3;
				num5 = num2;
			}
			else if (num < 3f)
			{
				num5 = num2;
				num6 = num3;
			}
			else if (num < 4f)
			{
				num5 = num3;
				num6 = num2;
			}
			else if (num < 5f)
			{
				num4 = num3;
				num6 = num2;
			}
			else
			{
				num4 = num2;
				num6 = num3;
			}
			float num7 = aValue - num2;
			return new Color(num4 + num7, num5 + num7, num6 + num7);
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x00066568 File Offset: 0x00064768
		public static Color HCL(float aHue, float aChroma, float aLuma)
		{
			float num = aHue % 1f * 360f / 60f;
			float num2 = aChroma * (1f - Mathf.Abs(num % 2f - 1f));
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			if (num < 1f)
			{
				num3 = aChroma;
				num4 = num2;
			}
			else if (num < 2f)
			{
				num3 = num2;
				num4 = aChroma;
			}
			else if (num < 3f)
			{
				num4 = aChroma;
				num5 = num2;
			}
			else if (num < 4f)
			{
				num4 = num2;
				num5 = aChroma;
			}
			else if (num < 5f)
			{
				num3 = num2;
				num5 = aChroma;
			}
			else
			{
				num3 = aChroma;
				num5 = num2;
			}
			float num6 = aLuma - (0.3f * num3 + 0.59f * num4 + 0.11f * num5);
			return new Color(num3 + num6, num4 + num6, num5 + num6);
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x00066634 File Offset: 0x00064834
		public static Color GetColorBand(Color[] aColorBand, float aValue)
		{
			aValue %= 1f;
			Color color = Color.white;
			if (aColorBand != null)
			{
				int num = (int)(aValue * (float)aColorBand.Length);
				int num2 = (int)Mathf.Min(aValue * (float)aColorBand.Length + 1f, (float)(aColorBand.Length - 1));
				num = Mathf.Max(0, num);
				num = Mathf.Min(aColorBand.Length - 1, num);
				Color color2 = aColorBand[num];
				Color color3 = aColorBand[num2];
				float num3 = aValue - (float)num * (1f / (float)aColorBand.Length);
				color = Color.Lerp(color2, color3, num3 / (1f / (float)aColorBand.Length));
			}
			return color;
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x000666C0 File Offset: 0x000648C0
		public static Color FromHex(string aHex)
		{
			if (aHex.Length != 8)
			{
				return Color.red;
			}
			return new Color((float)Convert.ToInt32(aHex[0].ToString() + aHex[1].ToString()) / 255f, (float)Convert.ToInt32(aHex[2].ToString() + aHex[3].ToString()) / 255f, (float)Convert.ToInt32(aHex[4].ToString() + aHex[5].ToString()) / 255f, (float)Convert.ToInt32(aHex[6].ToString() + aHex[7].ToString()) / 255f);
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x000667A0 File Offset: 0x000649A0
		public static string ToHex(Color aColor)
		{
			return string.Format("{0:X}{1:X}{2:X}{3:X}", new object[]
			{
				(int)(aColor.r * 255f),
				(int)(aColor.g * 255f),
				(int)(aColor.b * 255f),
				(int)(aColor.a * 255f)
			});
		}

		// Token: 0x04000F10 RID: 3856
		private static float sqrt3 = (float)Math.Sqrt(3.0);
	}
}
