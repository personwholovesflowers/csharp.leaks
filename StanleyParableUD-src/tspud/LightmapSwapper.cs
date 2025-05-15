using System;
using UnityEngine;

// Token: 0x020000FD RID: 253
public class LightmapSwapper : MonoBehaviour
{
	// Token: 0x06000616 RID: 1558 RVA: 0x00021A10 File Offset: 0x0001FC10
	private void Start()
	{
		this.lightmapNative = LightmapSettings.lightmaps;
		this.lightmapResource = new LightmapData[this.lightmapNative.Length];
		for (int i = 0; i < this.lightmapNative.Length; i++)
		{
			this.lightmapResource[i] = new LightmapData();
			this.lightmapResource[i].lightmapColor = Resources.Load("ropetest/" + this.lightmapNative[i].lightmapColor.name) as Texture2D;
			this.lightmapResource[i].lightmapDir = Resources.Load("ropetest/" + this.lightmapNative[i].lightmapDir.name) as Texture2D;
		}
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00021AC4 File Offset: 0x0001FCC4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			this.Swap();
		}
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x00021AD5 File Offset: 0x0001FCD5
	private void Swap()
	{
		if (this.toggle)
		{
			LightmapSettings.lightmaps = this.lightmapNative;
		}
		else
		{
			LightmapSettings.lightmaps = this.lightmapResource;
		}
		this.toggle = !this.toggle;
	}

	// Token: 0x0400066A RID: 1642
	private LightmapData[] lightmapNative;

	// Token: 0x0400066B RID: 1643
	private LightmapData[] lightmapResource;

	// Token: 0x0400066C RID: 1644
	private bool toggle;
}
