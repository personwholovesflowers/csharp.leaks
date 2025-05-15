using System;
using UnityEngine;

namespace ScriptableObjects
{
	// Token: 0x02000554 RID: 1364
	[CreateAssetMenu(fileName = "TipOfTheDay", menuName = "ULTRAKILL/TipOfTheDay")]
	public class TipOfTheDay : ScriptableObject
	{
		// Token: 0x04002B32 RID: 11058
		[TextArea(2, 8)]
		public string tip;
	}
}
