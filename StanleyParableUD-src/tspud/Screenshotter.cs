using System;
using UnityEngine;

// Token: 0x020000CE RID: 206
public class Screenshotter : MonoBehaviour
{
	// Token: 0x060004BF RID: 1215 RVA: 0x0001BD5E File Offset: 0x00019F5E
	[ContextMenu("ScreenshotF")]
	private void ScreenshotF()
	{
		base.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Front.png");
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0001BD89 File Offset: 0x00019F89
	[ContextMenu("ScreenshotB")]
	private void ScreenshotB()
	{
		base.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Back.png");
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x0001BDB4 File Offset: 0x00019FB4
	[ContextMenu("ScreenshotL")]
	private void ScreenshotL()
	{
		base.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Left.png");
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x0001BDDF File Offset: 0x00019FDF
	[ContextMenu("ScreenshotR")]
	private void ScreenshotR()
	{
		base.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Right.png");
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x0001BE0A File Offset: 0x0001A00A
	[ContextMenu("ScreenshotU")]
	private void ScreenshotU()
	{
		base.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Up.png");
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001BE35 File Offset: 0x0001A035
	[ContextMenu("ScreenshotD")]
	private void ScreenshotD()
	{
		base.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		ScreenCapture.CaptureScreenshot("Sky Down.png");
	}
}
