using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class ConfigurableAction : MonoBehaviour
{
	// Token: 0x060003E1 RID: 993 RVA: 0x00018A6B File Offset: 0x00016C6B
	public void IntConfigurable_Increment(IntConfigurable intConfigurable)
	{
		intConfigurable.IncreaseValue();
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00018A73 File Offset: 0x00016C73
	public void IntConfigurable_Decrement(IntConfigurable intConfigurable)
	{
		intConfigurable.DecreaseValue();
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00018A7B File Offset: 0x00016C7B
	public void InitAndSave(Configurable configurable)
	{
		configurable.Init();
		configurable.SaveToDiskAll();
	}
}
