using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DD RID: 221
public class GameText : HammerEntity
{
	// Token: 0x0600057A RID: 1402 RVA: 0x0001EF77 File Offset: 0x0001D177
	public void Input_Display()
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DisplayTextNoFade());
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0001EF8C File Offset: 0x0001D18C
	private IEnumerator DisplayTextNoFade()
	{
		this.canvas.enabled = true;
		yield return new WaitForGameSeconds(this.holdDuration);
		this.canvas.enabled = false;
		yield break;
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x0001EF9B File Offset: 0x0001D19B
	private IEnumerator DisplayText()
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		fint32 endInTime = startTime + this.inDuration;
		fint32 startOutTime = endInTime + this.holdDuration;
		fint32 endOutTime = startOutTime + this.outDuration;
		float num = 0f;
		this.color.a = num;
		Color sColor = this.shadow.color;
		sColor.a = num;
		this.text.color = this.color;
		this.shadow.color = sColor;
		this.canvas.enabled = true;
		while (Singleton<GameMaster>.Instance.GameTime < endInTime)
		{
			num = Mathf.InverseLerp(startTime, endInTime, Singleton<GameMaster>.Instance.GameTime);
			this.color.a = num;
			sColor.a = num;
			this.text.color = this.color;
			this.shadow.color = sColor;
			yield return new WaitForEndOfFrame();
		}
		num = 1f;
		this.color.a = num;
		sColor.a = num;
		this.text.color = this.color;
		this.shadow.color = sColor;
		yield return new WaitForGameSeconds(this.holdDuration);
		while (Singleton<GameMaster>.Instance.GameTime < endOutTime)
		{
			num = Mathf.InverseLerp(endOutTime, startOutTime, Singleton<GameMaster>.Instance.GameTime);
			this.color.a = num;
			sColor.a = num;
			this.text.color = this.color;
			this.shadow.color = sColor;
			yield return new WaitForEndOfFrame();
		}
		num = 0f;
		this.color.a = num;
		sColor.a = num;
		this.text.color = this.color;
		this.shadow.color = sColor;
		this.canvas.enabled = false;
		yield break;
	}

	// Token: 0x04000539 RID: 1337
	public Color color = Color.white;

	// Token: 0x0400053A RID: 1338
	public float inDuration;

	// Token: 0x0400053B RID: 1339
	public float outDuration;

	// Token: 0x0400053C RID: 1340
	public float holdDuration;

	// Token: 0x0400053D RID: 1341
	public string message = "";

	// Token: 0x0400053E RID: 1342
	public Canvas canvas;

	// Token: 0x0400053F RID: 1343
	public Text text;

	// Token: 0x04000540 RID: 1344
	public Text shadow;
}
