using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000139 RID: 313
public class MiscTrigger : MonoBehaviour
{
	// Token: 0x06000759 RID: 1881 RVA: 0x0002606A File Offset: 0x0002426A
	private void Awake()
	{
		StanleyController.OnActuallyJumping = (Action)Delegate.Combine(StanleyController.OnActuallyJumping, new Action(this.OnActuallyJumping));
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0002608C File Offset: 0x0002428C
	private void OnDestroy()
	{
		StanleyController.OnActuallyJumping = (Action)Delegate.Remove(StanleyController.OnActuallyJumping, new Action(this.OnActuallyJumping));
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x000260AE File Offset: 0x000242AE
	private void OnActuallyJumping()
	{
		if (this.condition == MiscTrigger.Condition.IsJumping)
		{
			this.OnConditionMet.Invoke();
		}
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x000260C4 File Offset: 0x000242C4
	public void Invoke()
	{
		MiscTrigger.Condition condition = this.condition;
	}

	// Token: 0x04000789 RID: 1929
	[SerializeField]
	private MiscTrigger.Condition condition;

	// Token: 0x0400078A RID: 1930
	[SerializeField]
	private UnityEvent OnConditionMet;

	// Token: 0x020003D9 RID: 985
	public enum Condition
	{
		// Token: 0x0400143B RID: 5179
		IsJumping
	}
}
