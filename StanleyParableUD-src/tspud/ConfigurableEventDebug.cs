using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
[RequireComponent(typeof(ConfigurableEvent))]
public class ConfigurableEventDebug : MonoBehaviour
{
	// Token: 0x06000128 RID: 296 RVA: 0x000094E6 File Offset: 0x000076E6
	public void OnEvaluateDebug()
	{
		if (this.fireOnEvaluate)
		{
			Debug.Log("ConfigurableEventDebug " + base.name + " OnEvaluateDebug");
		}
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0000950A File Offset: 0x0000770A
	public void OnConditionMetDebug()
	{
		if (this.fireOnEvaluate)
		{
			Debug.Log("ConfigurableEventDebug " + base.name + " OnConditionMetDebug");
		}
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000952E File Offset: 0x0000772E
	public void OnConditionNotMetDebug()
	{
		if (this.fireOnEvaluate)
		{
			Debug.Log("ConfigurableEventDebug " + base.name + " OnConditionNotMetDebug");
		}
	}

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private bool fireOnEvaluate;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	private bool fireOnConditionMet;

	// Token: 0x04000192 RID: 402
	[SerializeField]
	private bool fireOnConditionNotMet;
}
