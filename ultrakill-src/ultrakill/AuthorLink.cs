using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
[Serializable]
public class AuthorLink
{
	// Token: 0x040000A7 RID: 167
	public LinkPlatform platform;

	// Token: 0x040000A8 RID: 168
	public string username;

	// Token: 0x040000A9 RID: 169
	public string displayName;

	// Token: 0x040000AA RID: 170
	[Header("Optional")]
	public string description;
}
