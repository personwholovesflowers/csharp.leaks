using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class SS_LightmapSwapper : MonoBehaviour
{
	// Token: 0x06000073 RID: 115 RVA: 0x00005444 File Offset: 0x00003644
	private void OnValidate()
	{
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00005448 File Offset: 0x00003648
	public void GetLightmapIndex()
	{
		if (!this.ren)
		{
			Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].lightmapIndex > -1)
				{
					this.ren = componentsInChildren[i];
					break;
				}
			}
		}
		if (this.ren)
		{
			this.lightmapIndex = this.ren.lightmapIndex;
			return;
		}
		this.lightmapIndex = -1;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000054B4 File Offset: 0x000036B4
	public void SwapLightmap(Texture2D tex, int index)
	{
		this.lightmaps = LightmapSettings.lightmaps;
		int num = this.lightmaps.Length;
		if (num <= index || num == 0)
		{
			return;
		}
		this.lightmaps[index].lightmapColor = tex;
		LightmapSettings.lightmaps = this.lightmaps;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000054F6 File Offset: 0x000036F6
	public void SwapLightmapFromArray(int index)
	{
		this.GetLightmapIndex();
		if (this.lightmapIndex > -1)
		{
			this.SwapLightmap(this.lightmapArray[index], this.lightmapIndex);
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0000551B File Offset: 0x0000371B
	public void RestoreLightmap(Texture2D tex, int index)
	{
		this.lightmaps = LightmapSettings.lightmaps;
		this.lightmaps[index].lightmapColor = tex;
		LightmapSettings.lightmaps = this.lightmaps;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00005541 File Offset: 0x00003741
	[ContextMenu("Swap Lightmap")]
	public void SwapLightmap()
	{
		this.GetLightmapIndex();
		if (this.lightmapIndex > -1)
		{
			this.SwapLightmap(this.newLightmap, this.lightmapIndex);
			if (this.lpStorage)
			{
				this.lpStorage.ApplyHarmonics();
			}
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x0000557C File Offset: 0x0000377C
	[ContextMenu("Restore Lightmap")]
	public void RestoreLightmap()
	{
		if (this.savedLightmap)
		{
			this.GetLightmapIndex();
			if (this.lightmapIndex > -1)
			{
				this.RestoreLightmap(this.savedLightmap, this.lightmapIndex);
			}
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000055AC File Offset: 0x000037AC
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(0.1f);
		if (this.applyNewLightmapOnEnable && this.newLightmap != null)
		{
			this.SwapLightmap();
		}
		yield break;
	}

	// Token: 0x04000097 RID: 151
	public bool applyNewLightmapOnEnable = true;

	// Token: 0x04000098 RID: 152
	[Space]
	private int lightmapIndex = -1;

	// Token: 0x04000099 RID: 153
	public Texture2D newLightmap;

	// Token: 0x0400009A RID: 154
	public Texture2D[] lightmapArray;

	// Token: 0x0400009B RID: 155
	[NonSerialized]
	public Texture2D savedLightmap;

	// Token: 0x0400009C RID: 156
	private Renderer ren;

	// Token: 0x0400009D RID: 157
	private LightmapData[] lightmaps;

	// Token: 0x0400009E RID: 158
	public SS_LightProbeStorage lpStorage;
}
