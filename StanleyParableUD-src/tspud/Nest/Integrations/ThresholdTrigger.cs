using System;
using UnityEngine;
using UnityEngine.Events;

namespace Nest.Integrations
{
	// Token: 0x02000244 RID: 580
	[AddComponentMenu("Cast/Integrations/Threshold Trigger")]
	public class ThresholdTrigger : BaseIntegration
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x0003D57A File Offset: 0x0003B77A
		// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x0003D582 File Offset: 0x0003B782
		public float threshold
		{
			get
			{
				return this._threshold;
			}
			set
			{
				this._threshold = value;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0003D58B File Offset: 0x0003B78B
		// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x0003D593 File Offset: 0x0003B793
		public float offDelay
		{
			get
			{
				return this._delayToOff;
			}
			set
			{
				this._delayToOff = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x0003D59C File Offset: 0x0003B79C
		public override float InputValue
		{
			set
			{
				this._currentValue = value;
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0003D5A8 File Offset: 0x0003B7A8
		private void Update()
		{
			if (this._currentValue >= this._threshold)
			{
				if (this._currentState != ThresholdTrigger.State.Enabled)
				{
					this._onEvent.Invoke();
					this._currentState = ThresholdTrigger.State.Enabled;
				}
				this._delayTimer = 0f;
				return;
			}
			if (this._currentValue < this._threshold && this._currentState != ThresholdTrigger.State.Disabled)
			{
				this._delayTimer += Time.deltaTime;
				if (this._delayTimer >= this._delayToOff)
				{
					this._offEvent.Invoke();
					this._currentState = ThresholdTrigger.State.Disabled;
				}
			}
		}

		// Token: 0x04000C2A RID: 3114
		[SerializeField]
		private float _threshold = 0.01f;

		// Token: 0x04000C2B RID: 3115
		[SerializeField]
		private float _delayToOff;

		// Token: 0x04000C2C RID: 3116
		[SerializeField]
		private UnityEvent _onEvent;

		// Token: 0x04000C2D RID: 3117
		[SerializeField]
		private UnityEvent _offEvent;

		// Token: 0x04000C2E RID: 3118
		private ThresholdTrigger.State _currentState;

		// Token: 0x04000C2F RID: 3119
		private float _currentValue;

		// Token: 0x04000C30 RID: 3120
		private float _delayTimer;

		// Token: 0x02000437 RID: 1079
		private enum State
		{
			// Token: 0x040015B2 RID: 5554
			Dormant,
			// Token: 0x040015B3 RID: 5555
			Enabled,
			// Token: 0x040015B4 RID: 5556
			Disabled
		}
	}
}
