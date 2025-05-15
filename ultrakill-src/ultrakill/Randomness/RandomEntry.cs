using System;
using UnityEngine;

namespace Randomness
{
	// Token: 0x02000577 RID: 1399
	[Serializable]
	public class RandomEntry
	{
		// Token: 0x04002C0C RID: 11276
		[Min(0f)]
		[Tooltip("The bigger the weight, the bigger the chance.")]
		public int weight = 1;
	}
}
