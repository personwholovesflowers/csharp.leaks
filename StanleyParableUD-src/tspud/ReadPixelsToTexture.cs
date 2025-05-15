using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class ReadPixelsToTexture : MonoBehaviour
{
	// Token: 0x06000890 RID: 2192 RVA: 0x000288F4 File Offset: 0x00026AF4
	private void Update()
	{
		if (!this.rendering)
		{
			base.StartCoroutine(this.RenderScreen());
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0002890B File Offset: 0x00026B0B
	private IEnumerator RenderScreen()
	{
		this.mat.SetTexture("_MainTex", this.tex);
		while ((float)this.frameCounter < this.skipFrames)
		{
			this.frameCounter++;
			yield return null;
		}
		this.frameCounter = 0;
		this.rendering = false;
		yield break;
	}

	// Token: 0x0400085E RID: 2142
	[SerializeField]
	private Material mat;

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	private float skipFrames;

	// Token: 0x04000860 RID: 2144
	private int frameCounter;

	// Token: 0x04000861 RID: 2145
	private bool rendering;

	// Token: 0x04000862 RID: 2146
	private RenderTexture tex;
}
