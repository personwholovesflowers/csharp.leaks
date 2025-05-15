using System;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class ScreenshotTool : MonoBehaviour
{
	// Token: 0x06000228 RID: 552 RVA: 0x00010048 File Offset: 0x0000E248
	public static Texture2D TakeScreenshot()
	{
		int width = Screen.width;
		int height = Screen.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, false, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0, false);
		texture2D.Apply(false);
		return texture2D;
	}
}
