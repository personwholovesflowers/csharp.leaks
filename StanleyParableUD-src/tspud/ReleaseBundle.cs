using System;
using Malee.List;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class ReleaseBundle : ScriptableObject
{
	// Token: 0x0400015C RID: 348
	public string RootFolder;

	// Token: 0x0400015D RID: 349
	public bool CanBeSceneExclusive = true;

	// Token: 0x0400015E RID: 350
	public bool NotInProjectRoot;

	// Token: 0x0400015F RID: 351
	public bool ForceInclusion;

	// Token: 0x04000160 RID: 352
	[Reorderable]
	public ReleaseBundle.VariantPresetsCollection VariantsAndPresets;

	// Token: 0x02000354 RID: 852
	[Serializable]
	public class VariantPresetsCollection : ReorderableArray<ReleaseBundle.VariantPresetPair>
	{
	}

	// Token: 0x02000355 RID: 853
	[Serializable]
	public class VariantPresetPair
	{
		// Token: 0x0400120B RID: 4619
		public string VariantName;
	}
}
