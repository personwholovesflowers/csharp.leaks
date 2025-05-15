using System;
using System.Collections.Generic;

// Token: 0x02000038 RID: 56
[Serializable]
public class ConfigurableSubCategory
{
	// Token: 0x0400019D RID: 413
	public string SubCategoryName;

	// Token: 0x0400019E RID: 414
	public List<Configurable> Configurables = new List<Configurable>();

	// Token: 0x0400019F RID: 415
	public List<ConfigurableAvailabilities> ConfigurableAvailability = new List<ConfigurableAvailabilities>();
}
