using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
[Serializable]
public class BundleConfiguration
{
	// Token: 0x060000E9 RID: 233 RVA: 0x00008847 File Offset: 0x00006A47
	public string GetCustomVariant()
	{
		return this.customVariant;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000884F File Offset: 0x00006A4F
	public bool MatchesReferences(ReleaseBundle bundleToCheck)
	{
		return this.bundleReference == bundleToCheck;
	}

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private ReleaseBundle bundleReference;

	// Token: 0x0400015A RID: 346
	[SerializeField]
	private string customVariant;

	// Token: 0x0400015B RID: 347
	public BundleConfiguration.IncludeOptions IncludeOption;

	// Token: 0x02000353 RID: 851
	public enum IncludeOptions
	{
		// Token: 0x04001208 RID: 4616
		UseDefault,
		// Token: 0x04001209 RID: 4617
		UseVariant,
		// Token: 0x0400120A RID: 4618
		DoNotInclude
	}
}
