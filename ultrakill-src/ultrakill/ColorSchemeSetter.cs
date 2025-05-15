using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class ColorSchemeSetter : MonoBehaviour
{
	// Token: 0x06000428 RID: 1064 RVA: 0x0001CE97 File Offset: 0x0001B097
	public void Apply()
	{
		if (this.replaceDitherUserSetting)
		{
			Shader.SetGlobalFloat("_DitherStrength", this.ditheringAmount);
		}
		if (this.enforceMapColorPalette)
		{
			MonoSingleton<PostProcessV2_Handler>.Instance.ApplyMapColorPalette(this.mapDefinedPalette);
		}
		if (this.oneTime)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001CED7 File Offset: 0x0001B0D7
	private void OnTriggerEnter(Collider other)
	{
		if (!this.applyOnPlayerTriggerEnter)
		{
			return;
		}
		if (other.CompareTag("Player"))
		{
			this.Apply();
		}
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001CEF5 File Offset: 0x0001B0F5
	private void OnTriggerExit(Collider other)
	{
		if (!this.applyOnPlayerTriggerExit)
		{
			return;
		}
		if (other.CompareTag("Player"))
		{
			this.Apply();
		}
	}

	// Token: 0x0400053A RID: 1338
	public bool replaceDitherUserSetting;

	// Token: 0x0400053B RID: 1339
	public float ditheringAmount;

	// Token: 0x0400053C RID: 1340
	public bool enforceMapColorPalette;

	// Token: 0x0400053D RID: 1341
	public Texture mapDefinedPalette;

	// Token: 0x0400053E RID: 1342
	public bool applyOnPlayerTriggerEnter;

	// Token: 0x0400053F RID: 1343
	public bool applyOnPlayerTriggerExit;

	// Token: 0x04000540 RID: 1344
	public bool oneTime;
}
