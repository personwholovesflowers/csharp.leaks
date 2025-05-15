using System;
using System.IO;
using UnityEngine;

// Token: 0x02000033 RID: 51
public class CustomGameContent
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x0600012C RID: 300 RVA: 0x0000698B File Offset: 0x00004B8B
	public string shortPath
	{
		get
		{
			if (!(this is CustomCampaign))
			{
				return Path.GetFileName(this.path);
			}
			return Path.GetFileName(this.path) + "/";
		}
	}

	// Token: 0x040000EE RID: 238
	public string name;

	// Token: 0x040000EF RID: 239
	public string uniqueId;

	// Token: 0x040000F0 RID: 240
	public string path;

	// Token: 0x040000F1 RID: 241
	public Texture2D thumbnail;

	// Token: 0x040000F2 RID: 242
	public DateTime lastModified;

	// Token: 0x040000F3 RID: 243
	public string author;

	// Token: 0x040000F4 RID: 244
	public string description;
}
