using System;
using System.Collections.Generic;
using System.IO;

namespace Bundlelizer
{
	// Token: 0x020002FA RID: 762
	[Serializable]
	public class AssetEntry : IComparable
	{
		// Token: 0x060013D4 RID: 5076 RVA: 0x00068FC1 File Offset: 0x000671C1
		public AssetEntry(string assetPath, bool doUpdate)
		{
			this.AssetPath = assetPath;
			this.DoUpdate = doUpdate;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00068FE2 File Offset: 0x000671E2
		public void SetNewPath(string newPath)
		{
			this.PreviousAssetPath = this.AssetPath;
			this.AssetPath = newPath;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00068FF7 File Offset: 0x000671F7
		public void SetBundleInfo(string currentBundle, string currentVariant, string newBundle, string newVariant)
		{
			this.CurrentBundle = currentBundle;
			this.CurrentVariant = currentVariant;
			this.NewBundle = newBundle;
			this.NewVariant = newVariant;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00069018 File Offset: 0x00067218
		public void UpdateName(string newName, string extension)
		{
			string fileName = Path.GetFileName(this.AssetPath);
			string text = this.AssetPath.Remove(this.AssetPath.Length - fileName.Length);
			this.AssetPath = text + newName + extension;
			if (this.PreviousAssetPath != null && this.PreviousAssetPath != "")
			{
				string text2 = this.PreviousAssetPath.Remove(this.PreviousAssetPath.Length - fileName.Length);
				this.PreviousAssetPath = text2 + newName + extension;
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x000690A3 File Offset: 0x000672A3
		public int CompareTo(object obj)
		{
			return this.AssetPath.CompareTo((obj as AssetEntry).AssetPath);
		}

		// Token: 0x04000F5A RID: 3930
		public string AssetPath;

		// Token: 0x04000F5B RID: 3931
		public string PreviousAssetPath;

		// Token: 0x04000F5C RID: 3932
		public bool DoUpdate;

		// Token: 0x04000F5D RID: 3933
		public string CurrentBundle;

		// Token: 0x04000F5E RID: 3934
		public string CurrentVariant;

		// Token: 0x04000F5F RID: 3935
		public string NewBundle;

		// Token: 0x04000F60 RID: 3936
		public string NewVariant;

		// Token: 0x04000F61 RID: 3937
		public List<string> Dependants = new List<string>();
	}
}
