using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x02000584 RID: 1412
	[DefaultExecutionOrder(10)]
	public class MapBoolWatcher : MapVarWatcher<bool?>
	{
		// Token: 0x06001FED RID: 8173 RVA: 0x00102C34 File Offset: 0x00100E34
		private void OnEnable()
		{
			if (!this.registered)
			{
				if (MonoSingleton<MapVarManager>.Instance == null)
				{
					Debug.LogError("Unable to register MapBoolWatcher. Missing map variable manager.");
					return;
				}
				MonoSingleton<MapVarManager>.Instance.RegisterBoolWatcher(this.variableName, delegate(bool val)
				{
					this.ProcessEvent(new bool?(val));
				});
				this.registered = true;
			}
			if (this.evaluateOnEnable)
			{
				this.ProcessEvent(MonoSingleton<MapVarManager>.Instance.GetBool(this.variableName));
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x00102CA2 File Offset: 0x00100EA2
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

		// Token: 0x06001FEF RID: 8175 RVA: 0x00102CBC File Offset: 0x00100EBC
		protected override void ProcessEvent(bool? value)
		{
			base.ProcessEvent(value);
			if (this.watchMode != BoolWatchMode.IsFalseOrNull)
			{
				bool? flag = this.lastValue;
				bool? flag2 = value;
				if ((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null)))
				{
					return;
				}
			}
			this.lastValue = value;
			bool flag3 = this.EvaluateState(value);
			if (flag3 == this.lastState)
			{
				return;
			}
			this.lastState = flag3;
			this.CallEvents();
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00102D2C File Offset: 0x00100F2C
		protected override bool EvaluateState(bool? newValue)
		{
			switch (this.watchMode)
			{
			case BoolWatchMode.IsTrue:
				return newValue != null && newValue.Value;
			case BoolWatchMode.IsFalse:
				return newValue != null && !newValue.Value;
			case BoolWatchMode.IsFalseOrNull:
				return newValue == null || !newValue.Value;
			case BoolWatchMode.AnyValue:
				return newValue != null;
			default:
				return false;
			}
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x00102DA0 File Offset: 0x00100FA0
		protected override void CallEvents()
		{
			base.CallEvents();
			this.onConditionMetWithValue.Invoke(this.lastValue.GetValueOrDefault());
		}

		// Token: 0x04002C3E RID: 11326
		[SerializeField]
		private BoolWatchMode watchMode;

		// Token: 0x04002C3F RID: 11327
		[SerializeField]
		private UnityEventBool onConditionMetWithValue;

		// Token: 0x04002C40 RID: 11328
		private bool? lastValue;
	}
}
