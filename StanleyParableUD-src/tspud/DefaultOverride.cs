using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[Serializable]
public class DefaultOverride
{
	// Token: 0x060000E6 RID: 230 RVA: 0x000087F6 File Offset: 0x000069F6
	public bool MatchesPlatform(RuntimePlatform platformToCheck)
	{
		return this.platform == platformToCheck;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00008804 File Offset: 0x00006A04
	public bool OverrideExistsForBundle(ReleaseBundle bundleToCheck, out BundleConfiguration overrideConfiguration)
	{
		for (int i = 0; i < this.CustomBundleConfigurations.Length; i++)
		{
			BundleConfiguration bundleConfiguration = this.CustomBundleConfigurations[i];
			if (bundleConfiguration.MatchesReferences(bundleToCheck))
			{
				overrideConfiguration = bundleConfiguration;
				return true;
			}
		}
		overrideConfiguration = null;
		return false;
	}

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private RuntimePlatform platform;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	public bool loadBundlesIntoMemory;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private BundleConfiguration[] CustomBundleConfigurations;
}
