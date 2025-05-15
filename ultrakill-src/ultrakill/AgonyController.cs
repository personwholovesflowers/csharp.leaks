using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class AgonyController : MonoSingleton<AgonyController>
{
	// Token: 0x040000B6 RID: 182
	[SerializeField]
	private GameObject reloadPrompt;
}
