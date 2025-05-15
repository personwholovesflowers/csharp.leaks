using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000273 RID: 627
	public interface MB2_EditorMethodsInterface
	{
		// Token: 0x06000EB4 RID: 3764
		void Clear();

		// Token: 0x06000EB5 RID: 3765
		void RestoreReadFlagsAndFormats(ProgressUpdateDelegate progressInfo);

		// Token: 0x06000EB6 RID: 3766
		void SetReadWriteFlag(Texture2D tx, bool isReadable, bool addToList);

		// Token: 0x06000EB7 RID: 3767
		void AddTextureFormat(Texture2D tx, bool isNormalMap);

		// Token: 0x06000EB8 RID: 3768
		void SaveAtlasToAssetDatabase(Texture2D atlas, ShaderTextureProperty texPropertyName, int atlasNum, Material resMat);

		// Token: 0x06000EB9 RID: 3769
		void SetMaterialTextureProperty(Material target, ShaderTextureProperty texPropName, string texturePath);

		// Token: 0x06000EBA RID: 3770
		void SetNormalMap(Texture2D tx);

		// Token: 0x06000EBB RID: 3771
		bool IsNormalMap(Texture2D tx);

		// Token: 0x06000EBC RID: 3772
		string GetPlatformString();

		// Token: 0x06000EBD RID: 3773
		void SetTextureSize(Texture2D tx, int size);

		// Token: 0x06000EBE RID: 3774
		bool IsCompressed(Texture2D tx);

		// Token: 0x06000EBF RID: 3775
		void CheckBuildSettings(long estimatedAtlasSize);

		// Token: 0x06000EC0 RID: 3776
		bool CheckPrefabTypes(MB_ObjsToCombineTypes prefabType, List<GameObject> gos);

		// Token: 0x06000EC1 RID: 3777
		bool ValidateSkinnedMeshes(List<GameObject> mom);

		// Token: 0x06000EC2 RID: 3778
		void CommitChangesToAssets();

		// Token: 0x06000EC3 RID: 3779
		void Destroy(Object o);
	}
}
