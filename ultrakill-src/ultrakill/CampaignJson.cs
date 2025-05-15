using System;
using Newtonsoft.Json;

// Token: 0x02000035 RID: 53
[Serializable]
public class CampaignJson
{
	// Token: 0x040000F9 RID: 249
	public string name;

	// Token: 0x040000FA RID: 250
	public string author;

	// Token: 0x040000FB RID: 251
	public string uuid;

	// Token: 0x040000FC RID: 252
	public CampaignLevel[] levels;

	// Token: 0x040000FD RID: 253
	[JsonIgnore]
	public string path;
}
