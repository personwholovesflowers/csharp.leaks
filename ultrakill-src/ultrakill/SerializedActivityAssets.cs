using System;
using Discord;

// Token: 0x02000011 RID: 17
[Serializable]
public struct SerializedActivityAssets
{
	// Token: 0x060000B0 RID: 176 RVA: 0x00004AE8 File Offset: 0x00002CE8
	public ActivityAssets Deserialize()
	{
		return new ActivityAssets
		{
			LargeImage = this.LargeImage,
			LargeText = this.LargeText
		};
	}

	// Token: 0x04000049 RID: 73
	public string LargeImage;

	// Token: 0x0400004A RID: 74
	public string LargeText;
}
