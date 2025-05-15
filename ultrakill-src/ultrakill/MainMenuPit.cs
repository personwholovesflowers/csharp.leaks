using System;
using UnityEngine;

// Token: 0x020002E1 RID: 737
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class MainMenuPit : MonoSingleton<MainMenuPit>
{
	// Token: 0x06001006 RID: 4102 RVA: 0x00079FBC File Offset: 0x000781BC
	protected override void Awake()
	{
		if (MonoSingleton<MainMenuPit>.Instance != null && MonoSingleton<MainMenuPit>.Instance != this)
		{
			Object.Destroy(MonoSingleton<MainMenuPit>.Instance.gameObject);
		}
		base.Awake();
	}
}
