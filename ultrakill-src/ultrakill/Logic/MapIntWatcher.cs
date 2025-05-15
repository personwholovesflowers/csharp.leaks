using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x02000592 RID: 1426
	[DefaultExecutionOrder(10)]
	public class MapIntWatcher : MapVarWatcher<int?>
	{
		// Token: 0x06002006 RID: 8198 RVA: 0x0010346C File Offset: 0x0010166C
		private void OnEnable()
		{
			if (!this.registered)
			{
				if (MonoSingleton<MapVarManager>.Instance == null)
				{
					Debug.LogError("Unable to register MapIntWatcher. Missing map variable manager.");
					return;
				}
				MonoSingleton<MapVarManager>.Instance.RegisterIntWatcher(this.variableName, delegate(int val)
				{
					this.ProcessEvent(new int?(val));
				});
				this.registered = true;
			}
			if (this.evaluateOnEnable)
			{
				this.ProcessEvent(MonoSingleton<MapVarManager>.Instance.GetInt(this.variableName));
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x001034DA File Offset: 0x001016DA
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

		// Token: 0x06002008 RID: 8200 RVA: 0x001034F4 File Offset: 0x001016F4
		protected override void ProcessEvent(int? value)
		{
			base.ProcessEvent(value);
			int? num = this.lastValue;
			int? num2 = value;
			if ((num.GetValueOrDefault() == num2.GetValueOrDefault()) & (num != null == (num2 != null)))
			{
				return;
			}
			this.lastValue = value;
			bool flag = this.EvaluateState(value);
			if (this.watchMode != IntWatchMode.AnyChange && flag == this.lastState)
			{
				return;
			}
			this.lastState = flag;
			this.CallEvents();
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x00103564 File Offset: 0x00101764
		protected override bool EvaluateState(int? newValue)
		{
			switch (this.watchMode)
			{
			case IntWatchMode.GreaterThan:
			{
				int? num = newValue;
				int num2 = this.targetValue;
				return (num.GetValueOrDefault() > num2) & (num != null);
			}
			case IntWatchMode.LessThan:
			{
				int? num = newValue;
				int num2 = this.targetValue;
				return (num.GetValueOrDefault() < num2) & (num != null);
			}
			case IntWatchMode.EqualTo:
			{
				int? num = newValue;
				int num2 = this.targetValue;
				return (num.GetValueOrDefault() == num2) & (num != null);
			}
			case IntWatchMode.NotEqualTo:
			{
				int? num = newValue;
				int num2 = this.targetValue;
				return !((num.GetValueOrDefault() == num2) & (num != null));
			}
			case IntWatchMode.AnyChange:
				return newValue != null;
			default:
				return false;
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x00103610 File Offset: 0x00101810
		protected override void CallEvents()
		{
			base.CallEvents();
			this.onConditionMetWithValue.Invoke(this.lastValue ?? (-1));
		}

		// Token: 0x04002C7B RID: 11387
		[SerializeField]
		private IntWatchMode watchMode;

		// Token: 0x04002C7C RID: 11388
		[SerializeField]
		private UnityEventInt onConditionMetWithValue;

		// Token: 0x04002C7D RID: 11389
		[SerializeField]
		private int targetValue;

		// Token: 0x04002C7E RID: 11390
		private int? lastValue;
	}
}
