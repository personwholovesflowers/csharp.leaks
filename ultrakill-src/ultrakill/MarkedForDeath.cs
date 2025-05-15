using System;
using UnityEngine;

// Token: 0x020002E9 RID: 745
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class MarkedForDeath : MonoSingleton<MarkedForDeath>
{
	// Token: 0x06001046 RID: 4166 RVA: 0x0007CB30 File Offset: 0x0007AD30
	private new void OnEnable()
	{
		EnemyIdentifier[] array = Object.FindObjectsOfType<EnemyIdentifier>();
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (!array[i].dead)
			{
				array[i].PlayerMarkedForDeath();
			}
		}
	}
}
