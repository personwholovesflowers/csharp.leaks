using System;
using UnityEngine;

// Token: 0x0200012F RID: 303
public static class EasingFunction
{
	// Token: 0x060005B8 RID: 1464 RVA: 0x00027CE4 File Offset: 0x00025EE4
	public static float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00027CF0 File Offset: 0x00025EF0
	public static float Spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00027D54 File Offset: 0x00025F54
	public static float EaseInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00027D62 File Offset: 0x00025F62
	public static float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00027D78 File Offset: 0x00025F78
	public static float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return -end * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x00027DCC File Offset: 0x00025FCC
	public static float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00027DDC File Offset: 0x00025FDC
	public static float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x00027DFC File Offset: 0x00025FFC
	public static float EaseInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x00027E4D File Offset: 0x0002604D
	public static float EaseInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00027E5F File Offset: 0x0002605F
	public static float EaseOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x00027E84 File Offset: 0x00026084
	public static float EaseInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return -end * 0.5f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x00027EDA File Offset: 0x000260DA
	public static float EaseInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00027EEE File Offset: 0x000260EE
	public static float EaseOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00027F14 File Offset: 0x00026114
	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00027F6D File Offset: 0x0002616D
	public static float EaseInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value * 1.5707964f) + end + start;
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00027F87 File Offset: 0x00026187
	public static float EaseOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * 1.5707964f) + start;
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00027F9E File Offset: 0x0002619E
	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(3.1415927f * value) - 1f) + start;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00027FC2 File Offset: 0x000261C2
	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00027FE4 File Offset: 0x000261E4
	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00028008 File Offset: 0x00026208
	public static float EaseInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00028078 File Offset: 0x00026278
	public static float EaseInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00028098 File Offset: 0x00026298
	public static float EaseOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x000280BC File Offset: 0x000262BC
	public static float EaseInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00028128 File Offset: 0x00026328
	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - EasingFunction.EaseOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00028154 File Offset: 0x00026354
	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x000281F0 File Offset: 0x000263F0
	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EasingFunction.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return EasingFunction.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00028254 File Offset: 0x00026454
	public static float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00028288 File Offset: 0x00026488
	public static float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x000282C4 File Offset: 0x000264C4
	public static float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00028340 File Offset: 0x00026540
	public static float EaseInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x000283E4 File Offset: 0x000265E4
	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00028480 File Offset: 0x00026680
	public static float EaseInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002856E File Offset: 0x0002676E
	public static float LinearD(float start, float end, float value)
	{
		return end - start;
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00028573 File Offset: 0x00026773
	public static float EaseInQuadD(float start, float end, float value)
	{
		return 2f * (end - start) * value;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00028580 File Offset: 0x00026780
	public static float EaseOutQuadD(float start, float end, float value)
	{
		end -= start;
		return -end * value - end * (value - 2f);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00028595 File Offset: 0x00026795
	public static float EaseInOutQuadD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value;
		}
		value -= 1f;
		return end * (1f - value);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x000285C3 File Offset: 0x000267C3
	public static float EaseInCubicD(float start, float end, float value)
	{
		return 3f * (end - start) * value * value;
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x000285D2 File Offset: 0x000267D2
	public static float EaseOutCubicD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 3f * end * value * value;
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000285ED File Offset: 0x000267ED
	public static float EaseInOutCubicD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 1.5f * end * value * value;
		}
		value -= 2f;
		return 1.5f * end * value * value;
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00028625 File Offset: 0x00026825
	public static float EaseInQuartD(float start, float end, float value)
	{
		return 4f * (end - start) * value * value * value;
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x00028636 File Offset: 0x00026836
	public static float EaseOutQuartD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -4f * end * value * value * value;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00028653 File Offset: 0x00026853
	public static float EaseInOutQuartD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2f * end * value * value * value;
		}
		value -= 2f;
		return -2f * end * value * value * value;
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002868F File Offset: 0x0002688F
	public static float EaseInQuintD(float start, float end, float value)
	{
		return 5f * (end - start) * value * value * value * value;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x000286A2 File Offset: 0x000268A2
	public static float EaseOutQuintD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 5f * end * value * value * value * value;
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x000286C1 File Offset: 0x000268C1
	public static float EaseInOutQuintD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2.5f * end * value * value * value * value;
		}
		value -= 2f;
		return 2.5f * end * value * value * value * value;
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00028701 File Offset: 0x00026901
	public static float EaseInSineD(float start, float end, float value)
	{
		return (end - start) * 0.5f * 3.1415927f * Mathf.Sin(1.5707964f * value);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0002871F File Offset: 0x0002691F
	public static float EaseOutSineD(float start, float end, float value)
	{
		end -= start;
		return 1.5707964f * end * Mathf.Cos(value * 1.5707964f);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0002873A File Offset: 0x0002693A
	public static float EaseInOutSineD(float start, float end, float value)
	{
		end -= start;
		return end * 0.5f * 3.1415927f * Mathf.Sin(3.1415927f * value);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0002875B File Offset: 0x0002695B
	public static float EaseInExpoD(float start, float end, float value)
	{
		return 6.931472f * (end - start) * Mathf.Pow(2f, 10f * (value - 1f));
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002877E File Offset: 0x0002697E
	public static float EaseOutExpoD(float start, float end, float value)
	{
		end -= start;
		return 3.465736f * end * Mathf.Pow(2f, 1f - 10f * value);
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x000287A4 File Offset: 0x000269A4
	public static float EaseInOutExpoD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 3.465736f * end * Mathf.Pow(2f, 10f * (value - 1f));
		}
		value -= 1f;
		return 3.465736f * end / Mathf.Pow(2f, 10f * value);
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x00028809 File Offset: 0x00026A09
	public static float EaseInCircD(float start, float end, float value)
	{
		return (end - start) * value / Mathf.Sqrt(1f - value * value);
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0002881F File Offset: 0x00026A1F
	public static float EaseOutCircD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * value / Mathf.Sqrt(1f - value * value);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x00028844 File Offset: 0x00026A44
	public static float EaseInOutCircD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value / (2f * Mathf.Sqrt(1f - value * value));
		}
		value -= 2f;
		return -end * value / (2f * Mathf.Sqrt(1f - value * value));
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x000288A4 File Offset: 0x00026AA4
	public static float EaseInBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return EasingFunction.EaseOutBounceD(0f, end, num - value);
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x000288CC File Offset: 0x00026ACC
	public static float EaseOutBounceD(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return 2f * end * 7.5625f * value;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return 2f * end * 7.5625f * value;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return 2f * end * 7.5625f * value;
		}
		value -= 0.95454544f;
		return 2f * end * 7.5625f * value;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00028960 File Offset: 0x00026B60
	public static float EaseInOutBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EasingFunction.EaseInBounceD(0f, end, value * 2f) * 0.5f;
		}
		return EasingFunction.EaseOutBounceD(0f, end, value * 2f - num) * 0.5f;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000289B8 File Offset: 0x00026BB8
	public static float EaseInBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		return 3f * (num + 1f) * (end - start) * value * value - 2f * num * (end - start) * value;
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000289F0 File Offset: 0x00026BF0
	public static float EaseOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00028A34 File Offset: 0x00026C34
	public static float EaseInOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return 0.5f * end * (num + 1f) * value * value + end * value * ((num + 1f) * value - num);
		}
		value -= 2f;
		num *= 1.525f;
		return 0.5f * end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x00028AC1 File Offset: 0x00026CC1
	public static float EaseInElasticD(float start, float end, float value)
	{
		return EasingFunction.EaseOutElasticD(start, end, 1f - value);
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x00028AD4 File Offset: 0x00026CD4
	public static float EaseOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * 3.1415927f * num * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / num2 - 3.465736f * num3 * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin(6.2831855f * (num * value - num4) / num2);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x00028B94 File Offset: 0x00026D94
	public static float EaseInOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			value -= 1f;
			return -3.465736f * num3 * Mathf.Pow(2f, 10f * value) * Mathf.Sin(6.2831855f * (num * value - 2f) / num2) - num3 * 3.1415927f * num * Mathf.Pow(2f, 10f * value) * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / num2;
		}
		value -= 1f;
		return num3 * 3.1415927f * num * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / (num2 * Mathf.Pow(2f, 10f * value)) - 3.465736f * num3 * Mathf.Sin(6.2831855f * (num * value - num4) / num2) / Mathf.Pow(2f, 10f * value);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00028CC4 File Offset: 0x00026EC4
	public static float SpringD(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		end -= start;
		return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) * Mathf.Sin(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) * (3.1415927f * (2.5f * value * value * value + 0.2f) + 23.561945f * value * value * value) * Mathf.Cos(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + 1f) - 6f * end * (Mathf.Pow(1f - value, 2.2f) * Mathf.Sin(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + value / 5f);
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00028DCC File Offset: 0x00026FCC
	public static EasingFunction.Function GetEasingFunction(EasingFunction.Ease easingFunction)
	{
		if (easingFunction == EasingFunction.Ease.EaseInQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseInSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseInExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCirc);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCirc);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCirc);
		}
		if (easingFunction == EasingFunction.Ease.Linear)
		{
			return new EasingFunction.Function(EasingFunction.Linear);
		}
		if (easingFunction == EasingFunction.Ease.Spring)
		{
			return new EasingFunction.Function(EasingFunction.Spring);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseInElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInElastic);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutElastic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutElastic);
		}
		return null;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x00029010 File Offset: 0x00027210
	public static EasingFunction.Function GetEasingFunctionDerivative(EasingFunction.Ease easingFunction)
	{
		if (easingFunction == EasingFunction.Ease.EaseInQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCircD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCircD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCircD);
		}
		if (easingFunction == EasingFunction.Ease.Linear)
		{
			return new EasingFunction.Function(EasingFunction.LinearD);
		}
		if (easingFunction == EasingFunction.Ease.Spring)
		{
			return new EasingFunction.Function(EasingFunction.SpringD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInElasticD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutElasticD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutElasticD);
		}
		return null;
	}

	// Token: 0x040007E2 RID: 2018
	private const float NATURAL_LOG_OF_2 = 0.6931472f;

	// Token: 0x02000130 RID: 304
	public enum Ease
	{
		// Token: 0x040007E4 RID: 2020
		EaseInQuad,
		// Token: 0x040007E5 RID: 2021
		EaseOutQuad,
		// Token: 0x040007E6 RID: 2022
		EaseInOutQuad,
		// Token: 0x040007E7 RID: 2023
		EaseInCubic,
		// Token: 0x040007E8 RID: 2024
		EaseOutCubic,
		// Token: 0x040007E9 RID: 2025
		EaseInOutCubic,
		// Token: 0x040007EA RID: 2026
		EaseInQuart,
		// Token: 0x040007EB RID: 2027
		EaseOutQuart,
		// Token: 0x040007EC RID: 2028
		EaseInOutQuart,
		// Token: 0x040007ED RID: 2029
		EaseInQuint,
		// Token: 0x040007EE RID: 2030
		EaseOutQuint,
		// Token: 0x040007EF RID: 2031
		EaseInOutQuint,
		// Token: 0x040007F0 RID: 2032
		EaseInSine,
		// Token: 0x040007F1 RID: 2033
		EaseOutSine,
		// Token: 0x040007F2 RID: 2034
		EaseInOutSine,
		// Token: 0x040007F3 RID: 2035
		EaseInExpo,
		// Token: 0x040007F4 RID: 2036
		EaseOutExpo,
		// Token: 0x040007F5 RID: 2037
		EaseInOutExpo,
		// Token: 0x040007F6 RID: 2038
		EaseInCirc,
		// Token: 0x040007F7 RID: 2039
		EaseOutCirc,
		// Token: 0x040007F8 RID: 2040
		EaseInOutCirc,
		// Token: 0x040007F9 RID: 2041
		Linear,
		// Token: 0x040007FA RID: 2042
		Spring,
		// Token: 0x040007FB RID: 2043
		EaseInBounce,
		// Token: 0x040007FC RID: 2044
		EaseOutBounce,
		// Token: 0x040007FD RID: 2045
		EaseInOutBounce,
		// Token: 0x040007FE RID: 2046
		EaseInBack,
		// Token: 0x040007FF RID: 2047
		EaseOutBack,
		// Token: 0x04000800 RID: 2048
		EaseInOutBack,
		// Token: 0x04000801 RID: 2049
		EaseInElastic,
		// Token: 0x04000802 RID: 2050
		EaseOutElastic,
		// Token: 0x04000803 RID: 2051
		EaseInOutElastic
	}

	// Token: 0x02000131 RID: 305
	// (Invoke) Token: 0x060005FB RID: 1531
	public delegate float Function(float s, float e, float v);
}
