using System;
using UnityEngine;

namespace Logic
{
	// Token: 0x02000581 RID: 1409
	public abstract class MapVarWatcher<T> : MonoBehaviour
	{
		// Token: 0x06001FE7 RID: 8167 RVA: 0x00004AE3 File Offset: 0x00002CE3
		protected virtual void ProcessEvent(T value)
		{
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		protected virtual bool EvaluateState(T newValue)
		{
			return false;
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x00102B8C File Offset: 0x00100D8C
		protected virtual void CallEvents()
		{
			if (this.lastState)
			{
				this.onConditionMet.Invoke("");
				return;
			}
			this.onConditionMet.Revert();
		}

		// Token: 0x04002C32 RID: 11314
		[SerializeField]
		protected string variableName;

		// Token: 0x04002C33 RID: 11315
		[Tooltip("If true, the watcher will check its state immediately after being enabled or spawned.")]
		[SerializeField]
		protected bool evaluateOnEnable = true;

		// Token: 0x04002C34 RID: 11316
		[Tooltip("The component will be disabled after the event is executed")]
		[SerializeField]
		protected bool onlyActivateOnce;

		// Token: 0x04002C35 RID: 11317
		[Tooltip("Call the event every frame if the conditions are met")]
		[SerializeField]
		protected bool continuouslyActivateOnSuccess;

		// Token: 0x04002C36 RID: 11318
		protected bool lastState;

		// Token: 0x04002C37 RID: 11319
		protected bool registered;

		// Token: 0x04002C38 RID: 11320
		[SerializeField]
		protected UltrakillEvent onConditionMet;
	}
}
