using System;
using System.Collections.Generic;
using Malee.List;
using UnityEngine;

namespace Bundlelizer
{
	// Token: 0x020002FB RID: 763
	[CreateAssetMenu(fileName = "DefaultAssetMapping", menuName = "Bundelizer/Asset Bundle Mapping")]
	public class AssetMap : ScriptableObject
	{
		// Token: 0x060013D9 RID: 5081 RVA: 0x000690BC File Offset: 0x000672BC
		public bool IsExcluded(string path)
		{
			for (int i = 0; i < this.ExcludePaths.Count; i++)
			{
				if (this.ExcludePaths[i].KeywordMatch(path))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000690F8 File Offset: 0x000672F8
		public bool IsExcludedFromSceneDependencies(string path)
		{
			for (int i = 0; i < this.ExcludePaths.Count; i++)
			{
				AssetMap.ExcludePath excludePath = this.ExcludePaths[i];
				if (excludePath.ExcludeBehaviour == AssetMap.ExcludeBehaviours.ExcludeFromSceneDependencies && excludePath.KeywordMatch(path))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000F62 RID: 3938
		[Reorderable]
		public AssetMap.ExcludePathList ExcludePaths;

		// Token: 0x04000F63 RID: 3939
		public List<AssetEntry> UpdatableAssets = new List<AssetEntry>();

		// Token: 0x04000F64 RID: 3940
		public List<AssetEntry> UpdatableScenes = new List<AssetEntry>();

		// Token: 0x04000F65 RID: 3941
		public List<AssetEntry> UnbundledTextures = new List<AssetEntry>();

		// Token: 0x04000F66 RID: 3942
		public List<AssetEntry> UnbundledAudio = new List<AssetEntry>();

		// Token: 0x04000F67 RID: 3943
		public List<AssetEntry> UnbundledShaders = new List<AssetEntry>();

		// Token: 0x04000F68 RID: 3944
		public List<AssetEntry> UnbundledMaterial = new List<AssetEntry>();

		// Token: 0x04000F69 RID: 3945
		public List<AssetEntry> UnbundledMeshes = new List<AssetEntry>();

		// Token: 0x04000F6A RID: 3946
		public List<AssetEntry> UnbundledModels = new List<AssetEntry>();

		// Token: 0x04000F6B RID: 3947
		public List<AssetEntry> UnbundledOther = new List<AssetEntry>();

		// Token: 0x04000F6C RID: 3948
		public List<AssetEntry> MigrationHistory = new List<AssetEntry>();

		// Token: 0x04000F6D RID: 3949
		public List<AssetEntry> UnusedTextures = new List<AssetEntry>();

		// Token: 0x04000F6E RID: 3950
		public List<AssetEntry> UnusedAudio = new List<AssetEntry>();

		// Token: 0x04000F6F RID: 3951
		public List<AssetEntry> UnusedShaders = new List<AssetEntry>();

		// Token: 0x04000F70 RID: 3952
		public List<AssetEntry> UnusedMaterials = new List<AssetEntry>();

		// Token: 0x04000F71 RID: 3953
		public List<AssetEntry> UnusedMeshes = new List<AssetEntry>();

		// Token: 0x04000F72 RID: 3954
		public List<AssetEntry> UnusedModels = new List<AssetEntry>();

		// Token: 0x04000F73 RID: 3955
		public List<AssetEntry> UnusedOther = new List<AssetEntry>();

		// Token: 0x04000F74 RID: 3956
		public List<DuplicateEntry> DuplicateAssets = new List<DuplicateEntry>();

		// Token: 0x04000F75 RID: 3957
		public List<DuplicateEntry> DuplicateSceneAssets = new List<DuplicateEntry>();

		// Token: 0x04000F76 RID: 3958
		public bool FoldTextures;

		// Token: 0x04000F77 RID: 3959
		public bool FoldAudio;

		// Token: 0x04000F78 RID: 3960
		public bool FoldShaders;

		// Token: 0x04000F79 RID: 3961
		public bool FoldMaterial;

		// Token: 0x04000F7A RID: 3962
		public bool FoldMeshes;

		// Token: 0x04000F7B RID: 3963
		public bool FoldModels;

		// Token: 0x04000F7C RID: 3964
		public bool FoldOther;

		// Token: 0x04000F7D RID: 3965
		public bool FoldHistory;

		// Token: 0x04000F7E RID: 3966
		public bool FoldUnusedTextures;

		// Token: 0x04000F7F RID: 3967
		public bool FoldUnusedAudio;

		// Token: 0x04000F80 RID: 3968
		public bool FoldUnusedShaders;

		// Token: 0x04000F81 RID: 3969
		public bool FoldUnusedMaterials;

		// Token: 0x04000F82 RID: 3970
		public bool FoldUnusedMeshes;

		// Token: 0x04000F83 RID: 3971
		public bool FoldUnusedModels;

		// Token: 0x04000F84 RID: 3972
		public bool FoldUnusedOther;

		// Token: 0x04000F85 RID: 3973
		[SerializeField]
		[HideInInspector]
		public Vector2 ScrollPos;

		// Token: 0x04000F86 RID: 3974
		[SerializeField]
		public List<string> WhiteList = new List<string>();

		// Token: 0x04000F87 RID: 3975
		[SerializeField]
		public List<string> BlackList = new List<string>();

		// Token: 0x04000F88 RID: 3976
		[SerializeField]
		public List<AssetEntry> SelectedList;

		// Token: 0x04000F89 RID: 3977
		[SerializeField]
		public int SelectedIndex;

		// Token: 0x020004B1 RID: 1201
		public enum PresetBehaviours
		{
			// Token: 0x040017BD RID: 6077
			None,
			// Token: 0x040017BE RID: 6078
			Apply
		}

		// Token: 0x020004B2 RID: 1202
		public enum ExcludeBehaviours
		{
			// Token: 0x040017C0 RID: 6080
			ExcludeAll,
			// Token: 0x040017C1 RID: 6081
			ExcludeFromSceneDependencies
		}

		// Token: 0x020004B3 RID: 1203
		[Serializable]
		public class ExcludePathList : ReorderableArray<AssetMap.ExcludePath>
		{
		}

		// Token: 0x020004B4 RID: 1204
		[Serializable]
		public class ExcludePath
		{
			// Token: 0x06001A39 RID: 6713 RVA: 0x00082E8C File Offset: 0x0008108C
			public bool KeywordMatch(string path)
			{
				for (int i = 0; i < this.ExcludeFilepathKeywords.Length; i++)
				{
					if (path.Contains(this.ExcludeFilepathKeywords[i]))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x040017C2 RID: 6082
			public string[] ExcludeFilepathKeywords;

			// Token: 0x040017C3 RID: 6083
			public AssetMap.ExcludeBehaviours ExcludeBehaviour;
		}
	}
}
