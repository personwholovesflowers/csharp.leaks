using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
[DefaultExecutionOrder(-300)]
public class MapInfo : MapInfoBase
{
	// Token: 0x06000132 RID: 306 RVA: 0x00006B08 File Offset: 0x00004D08
	internal override void Awake()
	{
		base.Awake();
		if (MapInfo.Instance == null)
		{
			MapInfo.Instance = this;
		}
	}

	// Token: 0x04000105 RID: 261
	public new static MapInfo Instance;

	// Token: 0x04000106 RID: 262
	public string uniqueId;

	// Token: 0x04000107 RID: 263
	public string mapName;

	// Token: 0x04000108 RID: 264
	public string description;

	// Token: 0x04000109 RID: 265
	public string author;

	// Token: 0x0400010A RID: 266
	[Header("Has to be 640x480")]
	public Texture2D thumbnail;

	// Token: 0x0400010B RID: 267
	[Header("Map Configuration")]
	public bool renderSkybox;
}
