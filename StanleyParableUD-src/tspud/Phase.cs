using System;
using System.Collections;
using Nest.Integrations;
using UnityEngine;

// Token: 0x0200007C RID: 124
[Serializable]
public class Phase : BaseIntegration
{
	// Token: 0x060002F5 RID: 757 RVA: 0x00014878 File Offset: 0x00012A78
	private void Awake()
	{
		this.Index = base.transform.GetSiblingIndex();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0001488B File Offset: 0x00012A8B
	public IEnumerator PhaseRoutine(PhaseItem[] itemArray)
	{
		int num;
		for (int i = 0; i < itemArray.Length; i = num + 1)
		{
			itemArray[i].PhaseEvent.Invoke();
			float duration = itemArray[i].Duration;
			if (duration > 0f)
			{
				yield return new WaitForSeconds(duration);
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0400030D RID: 781
	public PhaseItem[] EnterItems;

	// Token: 0x0400030E RID: 782
	public PhaseItem[] LeaveItems;

	// Token: 0x0400030F RID: 783
	public int Index;
}
