using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000106 RID: 262
[RequireComponent(typeof(LogicCase))]
public class LogicCaseCustomWeights : MonoBehaviour
{
	// Token: 0x04000695 RID: 1685
	public List<LogicCaseCustomWeights.CaseWeight> caseWeightOverrides;

	// Token: 0x020003C9 RID: 969
	[Serializable]
	public class CaseWeight
	{
		// Token: 0x040013FF RID: 5119
		public Outputs caseNumber = Outputs.OnCase01;

		// Token: 0x04001400 RID: 5120
		public float weight = 1f;
	}
}
