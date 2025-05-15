using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[CreateAssetMenu(fileName = "New Platform Configuration", menuName = "Platform Configuration")]
public class PlatformConfigurations : ScriptableObject
{
	// Token: 0x060000E3 RID: 227 RVA: 0x00008774 File Offset: 0x00006974
	public bool OverrideExists(RuntimePlatform platform, ReleaseBundle bundle, out BundleConfiguration overrideConfiguration)
	{
		for (int i = 0; i < this.overrides.Length; i++)
		{
			DefaultOverride defaultOverride = this.overrides[i];
			if (defaultOverride.MatchesPlatform(platform))
			{
				return defaultOverride.OverrideExistsForBundle(bundle, out overrideConfiguration);
			}
		}
		overrideConfiguration = null;
		return false;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x000087B4 File Offset: 0x000069B4
	public bool LoadIntoMemory(RuntimePlatform platform)
	{
		for (int i = 0; i < this.overrides.Length; i++)
		{
			DefaultOverride defaultOverride = this.overrides[i];
			if (defaultOverride.MatchesPlatform(platform))
			{
				return defaultOverride.loadBundlesIntoMemory;
			}
		}
		return false;
	}

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private DefaultOverride[] overrides;
}
