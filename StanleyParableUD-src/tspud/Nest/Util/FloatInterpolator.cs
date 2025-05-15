using System;
using UnityEngine;

namespace Nest.Util
{
	// Token: 0x02000237 RID: 567
	public struct FloatInterpolator
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0003C047 File Offset: 0x0003A247
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x0003C04F File Offset: 0x0003A24F
		public FloatInterpolator.Config Configuration { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0003C058 File Offset: 0x0003A258
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x0003C060 File Offset: 0x0003A260
		public float CurrentValue { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0003C069 File Offset: 0x0003A269
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x0003C071 File Offset: 0x0003A271
		public float TargetValue { get; set; }

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003C07A File Offset: 0x0003A27A
		public FloatInterpolator(float initialValue, float targetValue, FloatInterpolator.Config config)
		{
			this.Configuration = config;
			this.CurrentValue = initialValue;
			this.TargetValue = targetValue;
			this._velocity = 0f;
			this._timeElapsed = 0f;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0003C0A7 File Offset: 0x0003A2A7
		public float Step(float targetValue)
		{
			this.TargetValue = targetValue;
			return this.Step();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003C0B8 File Offset: 0x0003A2B8
		public float Step()
		{
			switch (this.Configuration.Interpolation)
			{
			case FloatInterpolator.Config.InterpolationType.Linear:
				this.CurrentValue = Ease.Linear(this._timeElapsed, this.Configuration.InterpolationSpeed, 0.1f, 1f) * this.TargetValue;
				break;
			case FloatInterpolator.Config.InterpolationType.Sine:
				this.CurrentValue = Ease.InOutSine(this._timeElapsed, this.Configuration.InterpolationSpeed, 0.1f, 1f) * this.TargetValue;
				break;
			case FloatInterpolator.Config.InterpolationType.Quadratic:
				this.CurrentValue = Ease.InQuad(this._timeElapsed, this.Configuration.InterpolationSpeed, 0.1f, 1f) * this.TargetValue;
				break;
			case FloatInterpolator.Config.InterpolationType.Exponential:
				this.CurrentValue = Ease.InOutExpo(this._timeElapsed, this.Configuration.InterpolationSpeed, 0.1f, 1f) * this.TargetValue;
				break;
			case FloatInterpolator.Config.InterpolationType.DampedSpring:
				this.CurrentValue = Ease.DampenedSpring(this.CurrentValue, this.TargetValue, ref this._velocity, this.Configuration.InterpolationSpeed);
				break;
			case FloatInterpolator.Config.InterpolationType.AnimatedCurve:
				this.CurrentValue = this.Configuration.Curve.Evaluate(this._timeElapsed) * this.TargetValue;
				break;
			default:
				this.CurrentValue = this.TargetValue;
				break;
			}
			this._timeElapsed += Time.deltaTime;
			return this.CurrentValue;
		}

		// Token: 0x04000C02 RID: 3074
		private float _velocity;

		// Token: 0x04000C03 RID: 3075
		private float _timeElapsed;

		// Token: 0x02000431 RID: 1073
		[Serializable]
		public class Config
		{
			// Token: 0x17000309 RID: 777
			// (get) Token: 0x060018B0 RID: 6320 RVA: 0x0007C4D6 File Offset: 0x0007A6D6
			// (set) Token: 0x060018B1 RID: 6321 RVA: 0x0007C4DE File Offset: 0x0007A6DE
			public FloatInterpolator.Config.InterpolationType Interpolation
			{
				get
				{
					return this._interpolationType;
				}
				set
				{
					this._interpolationType = value;
				}
			}

			// Token: 0x1700030A RID: 778
			// (get) Token: 0x060018B2 RID: 6322 RVA: 0x0007C4E7 File Offset: 0x0007A6E7
			// (set) Token: 0x060018B3 RID: 6323 RVA: 0x0007C4EF File Offset: 0x0007A6EF
			public AnimationCurve Curve
			{
				get
				{
					return this._curve;
				}
				set
				{
					this._curve = value;
				}
			}

			// Token: 0x1700030B RID: 779
			// (get) Token: 0x060018B4 RID: 6324 RVA: 0x0007C4F8 File Offset: 0x0007A6F8
			public bool Enabled
			{
				get
				{
					return this.Interpolation > FloatInterpolator.Config.InterpolationType.Instant;
				}
			}

			// Token: 0x1700030C RID: 780
			// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0007C503 File Offset: 0x0007A703
			// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0007C50B File Offset: 0x0007A70B
			public float InterpolationSpeed
			{
				get
				{
					return this._interpolationSpeed;
				}
				set
				{
					this._interpolationSpeed = value;
				}
			}

			// Token: 0x0400159E RID: 5534
			[SerializeField]
			private FloatInterpolator.Config.InterpolationType _interpolationType = FloatInterpolator.Config.InterpolationType.DampedSpring;

			// Token: 0x0400159F RID: 5535
			[SerializeField]
			[Range(0.1f, 50f)]
			private float _interpolationSpeed = 5f;

			// Token: 0x040015A0 RID: 5536
			[SerializeField]
			private AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

			// Token: 0x020004D5 RID: 1237
			public enum InterpolationType
			{
				// Token: 0x040017EE RID: 6126
				Instant,
				// Token: 0x040017EF RID: 6127
				Linear,
				// Token: 0x040017F0 RID: 6128
				Sine,
				// Token: 0x040017F1 RID: 6129
				Quadratic,
				// Token: 0x040017F2 RID: 6130
				Exponential,
				// Token: 0x040017F3 RID: 6131
				DampedSpring,
				// Token: 0x040017F4 RID: 6132
				AnimatedCurve
			}
		}
	}
}
