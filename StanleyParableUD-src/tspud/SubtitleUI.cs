using System;
using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001AA RID: 426
public class SubtitleUI : MonoBehaviour
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x0002F4D8 File Offset: 0x0002D6D8
	private void Awake()
	{
		this.subtitlesAlpha = 0f;
		this.SubtitleFadeGroup.alpha = 0f;
		IntConfigurable intConfigurable = this.subtitleSizeIndex;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnSubtitleSizeChange));
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002F527 File Offset: 0x0002D727
	private void OnDestroy()
	{
		IntConfigurable intConfigurable = this.subtitleSizeIndex;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnSubtitleSizeChange));
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0002F550 File Offset: 0x0002D750
	private void OnSubtitleSizeChange(LiveData data)
	{
		Vector2 vector = new Vector2(1.7777778f, 1f);
		this.subtitleCanvasScaler.referenceResolution = vector * this.subtitleSizeProfiles.sizeProfiles[data.IntValue].uiReferenceHeight;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0002F598 File Offset: 0x0002D798
	private void Update()
	{
		if (this.fadingOut)
		{
			this.subtitlesAlpha -= Singleton<GameMaster>.Instance.GameDeltaTime / this.subtitleMicroFadeTime;
		}
		if (this.fadingOut && this.subtitlesAlpha <= 0f)
		{
			this.DEBUG_OUTPUT("alpha set to 0, ending fade out");
			this.subtitlesAlpha = 0f;
			this.fadingOut = false;
		}
		this.SubtitleFadeGroup.alpha = this.subtitlesAlpha;
		this.SubtitleFadeGroup.alpha *= (float)(this.subtitleToggle.GetBooleanValue() ? 1 : 0);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0002F634 File Offset: 0x0002D834
	private string GetWordWrappedText(string text, TMP_Text tmp)
	{
		return text;
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x0002F642 File Offset: 0x0002D842
	private IEnumerator SubSubtitleRunner(string splittableString, float clipLength, string originalKeyword)
	{
		this.DEBUG_OUTPUT("SubSubtitleRunner " + splittableString);
		if (splittableString.StartsWith("<split="))
		{
			splittableString = " " + splittableString;
		}
		string[] subSubtitles = splittableString.Split(new string[] { "<split=" }, StringSplitOptions.None);
		List<float> subSplits = new List<float>();
		for (int j = 1; j < subSubtitles.Length; j++)
		{
			int num = subSubtitles[j].IndexOf('>');
			subSplits.Add(float.Parse(subSubtitles[j].Substring(0, num)));
			subSubtitles[j] = subSubtitles[j].Substring(num + 1);
		}
		subSplits.Add(clipLength);
		this.totalTimeElapsed = 0f;
		int num2;
		for (int i = 0; i < subSubtitles.Length; i = num2 + 1)
		{
			this.DEBUG_OUTPUT(string.Format("SubSubtitleRunner::SubtitleRenderer.text {0}/{1} = {2}", i, subSubtitles.Length, subSubtitles[i]));
			float timeToWait = subSplits[i] - this.totalTimeElapsed;
			while (this.fadingOut)
			{
				yield return null;
				timeToWait -= Time.deltaTime * ChoreoMaster.GameSpeed;
			}
			this.SubtitleRenderer.text = this.GetWordWrappedText(subSubtitles[i], this.SubtitleRenderer);
			if (subSubtitles[i].Trim() != "" && this.subtitleDebugInfo.GetBooleanValue())
			{
				this.SubtitleRenderer.text = string.Format("{0}\n{1} ({2:0.00}/{3:0.00})", new object[]
				{
					this.SubtitleRenderer.text,
					originalKeyword,
					subSplits[i],
					clipLength
				});
			}
			float fadeInStartTime = 0f;
			float fadeInEndTime = this.subtitleMicroFadeTime;
			float fadeOutStartTime = timeToWait - this.subtitleBlinkTime - this.subtitleMicroFadeTime;
			float fadeOutEndTime = timeToWait - this.subtitleBlinkTime;
			this.endTime = timeToWait;
			float time = Time.time;
			this.subtitlesAlpha = 0f;
			this.timeElapsed = 0f;
			if (string.IsNullOrEmpty(this.SubtitleRenderer.text.Trim()))
			{
				this.subtitlesAlpha = 0f;
				while (this.timeElapsed < this.endTime)
				{
					yield return null;
					this.timeElapsed += Time.deltaTime * ChoreoMaster.GameSpeed;
				}
			}
			else
			{
				while (this.timeElapsed < this.endTime)
				{
					if (fadeInStartTime <= this.timeElapsed && this.timeElapsed < fadeInEndTime)
					{
						this.subtitlesAlpha = Mathf.InverseLerp(fadeInStartTime, fadeInEndTime, this.timeElapsed);
					}
					if (fadeInEndTime <= this.timeElapsed && this.timeElapsed < fadeOutStartTime)
					{
						this.subtitlesAlpha = 1f;
					}
					if (fadeOutStartTime <= this.timeElapsed && this.timeElapsed < fadeOutEndTime)
					{
						this.subtitlesAlpha = 1f - Mathf.InverseLerp(fadeOutStartTime, fadeOutEndTime, this.timeElapsed);
					}
					if (fadeOutEndTime <= this.timeElapsed && this.timeElapsed <= this.endTime)
					{
						this.subtitlesAlpha = 0f;
					}
					yield return null;
					this.timeElapsed += Time.deltaTime * ChoreoMaster.GameSpeed;
				}
			}
			this.totalTimeElapsed += timeToWait;
			num2 = i;
		}
		this.SubSubTitleRoutinePlaying_DEBUG = false;
		this.SubSubTitleRoutine = null;
		yield break;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0002F666 File Offset: 0x0002D866
	private void DEBUG_OUTPUT(string log)
	{
		if (this.DEBUG_OUTPUT_ON)
		{
			Debug.Log("SubtitleUI::" + log);
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0002F680 File Offset: 0x0002D880
	private void FadeOutAndStartSubtitle(string subtitle, float clipLength, string originalKeyword)
	{
		this.HideSubtitlesWithFade();
		this.StartSubSubtitleRunner(subtitle, clipLength, originalKeyword);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0002F691 File Offset: 0x0002D891
	private void StartSubSubtitleRunner(string subtitle, float clipLength, string originalKeyword)
	{
		this.StopSubSubtitleRunner();
		this.SubSubTitleRoutinePlaying_DEBUG = true;
		this.SubSubTitleRoutine = base.StartCoroutine(this.SubSubtitleRunner(subtitle, clipLength, originalKeyword));
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0002F6B5 File Offset: 0x0002D8B5
	private void StopSubSubtitleRunner()
	{
		if (this.SubSubTitleRoutine != null)
		{
			this.SubSubTitleRoutinePlaying_DEBUG = false;
			base.StopCoroutine(this.SubSubTitleRoutine);
			this.SubSubTitleRoutine = null;
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0002F6DC File Offset: 0x0002D8DC
	public void NewSubtitle(string keyword, float clipLength)
	{
		string empty = string.Empty;
		bool flag = LocalizationManager.TryGetTranslation(keyword, out empty, true, 0, true, false, null, null);
		if (keyword == "silence")
		{
			flag = false;
		}
		if (flag)
		{
			this.FadeOutAndStartSubtitle(empty, clipLength, keyword);
			return;
		}
		this.HideSubtitlesWithFade();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0002F720 File Offset: 0x0002D920
	public void HideSubtitlesWithFade()
	{
		if (this.fadingOut && this.subtitlesAlpha > 0f)
		{
			return;
		}
		this.fadingOut = true;
		this.StopSubSubtitleRunner();
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0002F745 File Offset: 0x0002D945
	public void HideSubtitlesImmediate()
	{
		this.SubtitleFadeGroup.alpha = 0f;
		this.subtitlesAlpha = 0f;
		this.fadingOut = false;
		this.StopSubSubtitleRunner();
	}

	// Token: 0x040009EE RID: 2542
	public CanvasGroup SubtitleFadeGroup;

	// Token: 0x040009EF RID: 2543
	public TextMeshProUGUI SubtitleRenderer;

	// Token: 0x040009F0 RID: 2544
	private bool fadingOut;

	// Token: 0x040009F1 RID: 2545
	private float subtitlesAlpha = 1f;

	// Token: 0x040009F2 RID: 2546
	public float subtitleBlinkTime = 0.08f;

	// Token: 0x040009F3 RID: 2547
	public float subtitleMicroFadeTime = 0.02f;

	// Token: 0x040009F4 RID: 2548
	public BooleanConfigurable subtitleDebugInfo;

	// Token: 0x040009F5 RID: 2549
	public BooleanConfigurable subtitleToggle;

	// Token: 0x040009F6 RID: 2550
	public IntConfigurable subtitleLanguageIndex;

	// Token: 0x040009F7 RID: 2551
	public IntConfigurable subtitleSizeIndex;

	// Token: 0x040009F8 RID: 2552
	public CanvasScaler subtitleCanvasScaler;

	// Token: 0x040009F9 RID: 2553
	public SubtitleSizeProfileData subtitleSizeProfiles;

	// Token: 0x040009FA RID: 2554
	private Coroutine fadeOutRoutine;

	// Token: 0x040009FB RID: 2555
	private float totalTimeElapsed;

	// Token: 0x040009FC RID: 2556
	private float endTime;

	// Token: 0x040009FD RID: 2557
	private float timeElapsed;

	// Token: 0x040009FE RID: 2558
	private bool DEBUG_OUTPUT_ON;

	// Token: 0x040009FF RID: 2559
	public bool SubSubTitleRoutinePlaying_DEBUG;

	// Token: 0x04000A00 RID: 2560
	private Coroutine SubSubTitleRoutine;
}
