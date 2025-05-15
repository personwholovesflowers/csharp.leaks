using System;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x02000239 RID: 569
	internal static class Ease
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x0003C258 File Offset: 0x0003A458
		public static float Linear(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return time / duration;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003C25D File Offset: 0x0003A45D
		public static float InSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return -Mathf.Cos(time / duration * Ease._piOver2) + 1f;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003C275 File Offset: 0x0003A475
		public static float OutSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return Mathf.Sin(time / duration * Ease._piOver2);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003C286 File Offset: 0x0003A486
		public static float InOutSine(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return -0.5f * (Mathf.Cos(3.1415927f * time / duration) - 1f);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003C2A3 File Offset: 0x0003A4A3
		public static float InQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time /= duration) * time;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0003C2AD File Offset: 0x0003A4AD
		public static float OutQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return -(time /= duration) * (time - 2f);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0003C2BE File Offset: 0x0003A4BE
		public static float InOutQuad(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * time * time;
			}
			return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0003C2FB File Offset: 0x0003A4FB
		public static float InCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time /= duration) * time * time;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0003C307 File Offset: 0x0003A507
		public static float OutCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time = time / duration - 1f) * time * time + 1f;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003C31F File Offset: 0x0003A51F
		public static float InOutCubic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * time * time * time;
			}
			return 0.5f * ((time -= 2f) * time * time + 2f);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0003C35A File Offset: 0x0003A55A
		public static float InQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time /= duration) * time * time * time;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0003C368 File Offset: 0x0003A568
		public static float OutQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return -((time = time / duration - 1f) * time * time * time - 1f);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0003C383 File Offset: 0x0003A583
		public static float InOutQuart(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * time * time * time * time;
			}
			return -0.5f * ((time -= 2f) * time * time * time - 2f);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0003C3C2 File Offset: 0x0003A5C2
		public static float InQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time /= duration) * time * time * time * time;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0003C3D2 File Offset: 0x0003A5D2
		public static float OutQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time = time / duration - 1f) * time * time * time * time + 1f;
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0003C3F0 File Offset: 0x0003A5F0
		public static float InOutQuint(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * time * time * time * time * time;
			}
			return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0003C43E File Offset: 0x0003A63E
		public static float InExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time != 0f)
			{
				return Mathf.Pow(2f, 10f * (time / duration - 1f));
			}
			return 0f;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0003C468 File Offset: 0x0003A668
		public static float OutExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time == duration)
			{
				return 1f;
			}
			return -Mathf.Pow(2f, -10f * time / duration) + 1f;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003C490 File Offset: 0x0003A690
		public static float InOutExpo(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time == 0f)
			{
				return 0f;
			}
			if (time == duration)
			{
				return 1f;
			}
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * Mathf.Pow(2f, 10f * (time - 1f));
			}
			return 0.5f * (-Mathf.Pow(2f, -10f * (time -= 1f)) + 2f);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0003C50F File Offset: 0x0003A70F
		public static float InCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return -(Mathf.Sqrt(1f - (time /= duration) * time) - 1f);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003C52C File Offset: 0x0003A72C
		public static float OutCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return Mathf.Sqrt(1f - (time = time / duration - 1f) * time);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0003C548 File Offset: 0x0003A748
		public static float InOutCirc(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return -0.5f * (Mathf.Sqrt(1f - time * time) - 1f);
			}
			return 0.5f * (Mathf.Sqrt(1f - (time -= 2f) * time) + 1f);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0003C5A8 File Offset: 0x0003A7A8
		public static float InElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time == 0f)
			{
				return 0f;
			}
			if ((time /= duration) == 1f)
			{
				return 1f;
			}
			if (period == 0f)
			{
				period = duration * 0.3f;
			}
			float num;
			if (overshootOrAmplitude < 1f)
			{
				overshootOrAmplitude = 1f;
				num = period / 4f;
			}
			else
			{
				num = period / Ease._twoPi * Mathf.Asin(1f / overshootOrAmplitude);
			}
			return -(overshootOrAmplitude * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * Ease._twoPi / period));
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x0003C648 File Offset: 0x0003A848
		public static float OutElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time == 0f)
			{
				return 0f;
			}
			if ((time /= duration) == 1f)
			{
				return 1f;
			}
			if (period == 0f)
			{
				period = duration * 0.3f;
			}
			float num;
			if (overshootOrAmplitude < 1f)
			{
				overshootOrAmplitude = 1f;
				num = period / 4f;
			}
			else
			{
				num = period / Ease._twoPi * Mathf.Asin(1f / overshootOrAmplitude);
			}
			return overshootOrAmplitude * Mathf.Pow(2f, -10f * time) * Mathf.Sin((time * duration - num) * Ease._twoPi / period) + 1f;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x0003C6E4 File Offset: 0x0003A8E4
		public static float InOutElastic(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if (time == 0f)
			{
				return 0f;
			}
			if ((time /= duration * 0.5f) == 2f)
			{
				return 1f;
			}
			if (period == 0f)
			{
				period = duration * 0.45000002f;
			}
			float num;
			if (overshootOrAmplitude < 1f)
			{
				overshootOrAmplitude = 1f;
				num = period / 4f;
			}
			else
			{
				num = period / Ease._twoPi * Mathf.Asin(1f / overshootOrAmplitude);
			}
			if (time < 1f)
			{
				return -0.5f * (overshootOrAmplitude * Mathf.Pow(2f, 10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * Ease._twoPi / period));
			}
			return overshootOrAmplitude * Mathf.Pow(2f, -10f * (time -= 1f)) * Mathf.Sin((time * duration - num) * Ease._twoPi / period) * 0.5f + 1f;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x0003C7D5 File Offset: 0x0003A9D5
		public static float InBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x0003C7EB File Offset: 0x0003A9EB
		public static float OutBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			return (time = time / duration - 1f) * time * ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003C810 File Offset: 0x0003AA10
		public static float InOutBack(float time, float duration = 1f, float overshootOrAmplitude = 0.1f, float period = 1f)
		{
			if ((time /= duration * 0.5f) < 1f)
			{
				return 0.5f * (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
			}
			return 0.5f * ((time -= 2f) * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003C87C File Offset: 0x0003AA7C
		public static float DampenedSpring(float current, float target, ref float velocity, float omega)
		{
			float deltaTime = Time.deltaTime;
			float num = velocity - (current - target) * (omega * omega * deltaTime);
			float num2 = 1f + omega * deltaTime;
			velocity = num / (num2 * num2);
			return current + velocity * deltaTime;
		}

		// Token: 0x04000C07 RID: 3079
		private static float _piOver2 = 1.5707964f;

		// Token: 0x04000C08 RID: 3080
		private static float _twoPi = 6.2831855f;
	}
}
