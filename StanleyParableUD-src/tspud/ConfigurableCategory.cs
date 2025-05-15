using System;
using System.Collections.Generic;

// Token: 0x02000037 RID: 55
[Serializable]
public class ConfigurableCategory
{
	// Token: 0x0400019B RID: 411
	public string CategoryName;

	// Token: 0x0400019C RID: 412
	public List<ConfigurableSubCategory> SubCategories = new List<ConfigurableSubCategory>();
}
