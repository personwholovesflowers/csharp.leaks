using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D6 RID: 214
public class FogController : HammerEntity
{
	// Token: 0x060004EA RID: 1258 RVA: 0x0001CA4B File Offset: 0x0001AC4B
	private void Awake()
	{
		if (this.fogEnable)
		{
			this.Input_TurnOn();
		}
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x0001CA5B File Offset: 0x0001AC5B
	public void Input_TurnOn()
	{
		if (RenderSettings.fog)
		{
			base.StartCoroutine(this.FadeToFrom());
			return;
		}
		base.StartCoroutine(this.FadeIn());
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001CA7F File Offset: 0x0001AC7F
	public void Input_TurnOff()
	{
		base.StartCoroutine(this.FadeOut());
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x0001CA8E File Offset: 0x0001AC8E
	private IEnumerator FadeIn()
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		fint32 endTime = startTime + this.fadeDuration;
		RenderSettings.fogColor = this.fogColor;
		RenderSettings.fogMode = FogMode.Linear;
		float startStart = this.fogStart + 100f;
		float endStart = this.fogEnd / this.maxDensity + 150f;
		RenderSettings.fogEndDistance = endStart;
		RenderSettings.fogStartDistance = startStart;
		RenderSettings.fog = true;
		while (Singleton<GameMaster>.Instance.GameTime <= endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			RenderSettings.fogEndDistance = Mathf.Lerp(endStart, this.fogEnd / this.maxDensity, num);
			RenderSettings.fogStartDistance = Mathf.Lerp(startStart, this.fogStart, num);
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x0001CA9D File Offset: 0x0001AC9D
	private IEnumerator FadeToFrom()
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		fint32 endTime = startTime + this.fadeDuration;
		Color startColor = RenderSettings.fogColor;
		RenderSettings.fogMode = FogMode.Linear;
		float startStart = RenderSettings.fogStartDistance;
		float endStart = RenderSettings.fogEndDistance;
		while (Singleton<GameMaster>.Instance.GameTime <= endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			RenderSettings.fogEndDistance = Mathf.Lerp(endStart, this.fogEnd / this.maxDensity, num);
			RenderSettings.fogStartDistance = Mathf.Lerp(startStart, this.fogStart, num);
			RenderSettings.fogColor = Color.Lerp(startColor, this.fogColor, num);
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0001CAAC File Offset: 0x0001ACAC
	private IEnumerator FadeOut()
	{
		fint32 startTime = Singleton<GameMaster>.Instance.GameTime;
		fint32 endTime = startTime + this.fadeDuration;
		float startEnd = this.fogStart + 100f;
		float endEnd = this.fogEnd / this.maxDensity + 150f;
		while (Singleton<GameMaster>.Instance.GameTime <= endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			RenderSettings.fogEndDistance = Mathf.Lerp(this.fogEnd / this.maxDensity, endEnd, num);
			RenderSettings.fogStartDistance = Mathf.Lerp(this.fogStart, startEnd, num);
			yield return new WaitForEndOfFrame();
		}
		RenderSettings.fog = false;
		yield break;
	}

	// Token: 0x040004C4 RID: 1220
	public Color fogColor = Color.black;

	// Token: 0x040004C5 RID: 1221
	public bool fogEnable;

	// Token: 0x040004C6 RID: 1222
	public float fogStart;

	// Token: 0x040004C7 RID: 1223
	public float fogEnd = 20f;

	// Token: 0x040004C8 RID: 1224
	public float maxDensity = 1f;

	// Token: 0x040004C9 RID: 1225
	private float fadeDuration = 2f;
}
