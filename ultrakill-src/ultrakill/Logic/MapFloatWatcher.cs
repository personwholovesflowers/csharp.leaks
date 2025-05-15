using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x0200058D RID: 1421
	[DefaultExecutionOrder(10)]
	public class MapFloatWatcher : MapVarWatcher<float?>
	{
		// Token: 0x06001FFC RID: 8188 RVA: 0x00103154 File Offset: 0x00101354
		private void OnEnable()
		{
			if (!this.registered)
			{
				if (MonoSingleton<MapVarManager>.Instance == null)
				{
					Debug.LogError("Unable to register MapFloatWatcher. Missing map variable manager.");
					return;
				}
				MonoSingleton<MapVarManager>.Instance.RegisterFloatWatcher(this.variableName, delegate(float val)
				{
					this.ProcessEvent(new float?(val));
				});
				this.registered = true;
			}
			if (this.evaluateOnEnable)
			{
				this.ProcessEvent(MonoSingleton<MapVarManager>.Instance.GetFloat(this.variableName));
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x001031C2 File Offset: 0x001013C2
		private void Update()
		{
			if (!this.continuouslyActivateOnSuccess)
			{
				return;
			}
			if (!this.lastState)
			{
				return;
			}
			this.CallEvents();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x001031DC File Offset: 0x001013DC
		protected override void ProcessEvent(float? value)
		{
			base.ProcessEvent(value);
			float? num = this.lastValue;
			float? num2 = value;
			if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
			{
				return;
			}
			this.lastValue = value;
			bool flag = this.EvaluateState(value);
			if (this.watchMode != FloatWatchMode.AnyChange && flag == this.lastState)
			{
				return;
			}
			this.lastState = flag;
			this.CallEvents();
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0010324C File Offset: 0x0010144C
		protected override bool EvaluateState(float? newValue)
		{
			switch (this.watchMode)
			{
			case FloatWatchMode.GreaterThan:
			{
				float? num = newValue;
				float num2 = this.targetValue;
				return (num.GetValueOrDefault() > num2) & (num != null);
			}
			case FloatWatchMode.LessThan:
			{
				float? num = newValue;
				float num2 = this.targetValue;
				return (num.GetValueOrDefault() < num2) & (num != null);
			}
			case FloatWatchMode.EqualTo:
			{
				float? num = newValue;
				float num2 = this.targetValue;
				return (num.GetValueOrDefault() == num2) & (num != null);
			}
			case FloatWatchMode.NotEqualTo:
			{
				float? num = newValue;
				float num2 = this.targetValue;
				return !((num.GetValueOrDefault() == num2) & (num != null));
			}
			case FloatWatchMode.AnyChange:
				return newValue != null;
			default:
				return false;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x001032F8 File Offset: 0x001014F8
		protected override void CallEvents()
		{
			base.CallEvents();
			this.onConditionMetWithValue.Invoke(this.lastValue ?? (-1f));
		}

		// Token: 0x04002C65 RID: 11365
		[SerializeField]
		private FloatWatchMode watchMode;

		// Token: 0x04002C66 RID: 11366
		[SerializeField]
		private UnityEventFloat onConditionMetWithValue;

		// Token: 0x04002C67 RID: 11367
		[SerializeField]
		private float targetValue = 3f;

		// Token: 0x04002C68 RID: 11368
		private float? lastValue;
	}
}
