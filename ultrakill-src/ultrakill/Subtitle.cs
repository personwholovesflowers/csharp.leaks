using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000460 RID: 1120
public class Subtitle : MonoBehaviour
{
	// Token: 0x060019A1 RID: 6561 RVA: 0x000D2E78 File Offset: 0x000D1078
	private void Awake()
	{
		this.group = base.GetComponent<CanvasGroup>();
		this.rectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x060019A2 RID: 6562 RVA: 0x000D2E94 File Offset: 0x000D1094
	private void OnEnable()
	{
		this.Fit();
		string text = new Regex("<[^>]*>").Replace(this.uiText.text, "");
		this.holdFor = this.holdForBase + (float)text.Length * this.holdForPerChar;
		this.currentAlpha = 0f;
		this.isFadingIn = true;
	}

	// Token: 0x060019A3 RID: 6563 RVA: 0x000D2EF4 File Offset: 0x000D10F4
	public void ContinueChain()
	{
		this.chainContinue = true;
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x000D2F00 File Offset: 0x000D1100
	private void Update()
	{
		if (this.distanceCheckObject == null)
		{
			this.baseAlpha = 1f;
		}
		else
		{
			float num = Vector3.Distance(MonoSingleton<CameraController>.Instance.transform.position, this.distanceCheckObject.transform.position);
			float num2 = this.distanceCheckObject.minDistance + (this.distanceCheckObject.maxDistance - this.distanceCheckObject.minDistance) * 0.5f;
			if (num <= num2)
			{
				this.baseAlpha = 1f;
			}
			else
			{
				float num3 = num - num2;
				float num4 = this.distanceCheckObject.maxDistance - num2;
				float num5 = Mathf.Clamp01(num3 / num4);
				switch (this.distanceCheckObject.rolloffMode)
				{
				case AudioRolloffMode.Logarithmic:
					this.baseAlpha = 1f - Mathf.Clamp01(Mathf.Log10(num3) / Mathf.Log10(num4));
					break;
				case AudioRolloffMode.Linear:
					this.baseAlpha = 1f - num5;
					break;
				case AudioRolloffMode.Custom:
					this.baseAlpha = this.distanceCheckObject.GetCustomCurve(AudioSourceCurveType.CustomRolloff).Evaluate(num5);
					break;
				}
			}
		}
		if (this.isFadingIn)
		{
			this.currentAlpha += this.fadeInSpeed * Time.deltaTime;
			if (this.currentAlpha >= 1f)
			{
				this.currentAlpha = 1f;
				this.isFadingIn = false;
				this.holdingSince = 0f;
			}
			this.group.alpha = this.currentAlpha * this.baseAlpha;
			return;
		}
		if (this.isFadingOut)
		{
			this.currentAlpha -= this.fadeOutSpeed * Time.deltaTime;
			if (this.currentAlpha <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
			this.group.alpha = this.currentAlpha * this.baseAlpha;
			return;
		}
		if (this.distanceCheckObject != null)
		{
			this.group.alpha = this.currentAlpha * this.baseAlpha;
		}
		if (this.holdingSince > this.holdFor && this.chainContinue)
		{
			this.isFadingOut = true;
			MonoSingleton<SubtitleController>.Instance.NotifyHoldEnd(this);
			if (this.nextInChain)
			{
				this.nextInChain.ContinueChain();
			}
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000D313B File Offset: 0x000D133B
	private void Fit()
	{
		base.StartCoroutine(this.FitAsync());
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x000D314A File Offset: 0x000D134A
	private IEnumerator FitAsync()
	{
		yield return new WaitForFixedUpdate();
		float preferredSize = LayoutUtility.GetPreferredSize(this.uiText.rectTransform, 0);
		this.uiText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize);
		this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredSize + this.paddingHorizontal * 2f);
		yield break;
	}

	// Token: 0x040023ED RID: 9197
	public AudioSource distanceCheckObject;

	// Token: 0x040023EE RID: 9198
	public Subtitle nextInChain;

	// Token: 0x040023EF RID: 9199
	[SerializeField]
	private float fadeInSpeed = 0.001f;

	// Token: 0x040023F0 RID: 9200
	[SerializeField]
	private float holdForBase = 2f;

	// Token: 0x040023F1 RID: 9201
	[SerializeField]
	private float holdForPerChar = 0.1f;

	// Token: 0x040023F2 RID: 9202
	[SerializeField]
	private float fadeOutSpeed = 0.0001f;

	// Token: 0x040023F3 RID: 9203
	[SerializeField]
	private float paddingHorizontal;

	// Token: 0x040023F4 RID: 9204
	[SerializeField]
	private TMP_Text uiText;

	// Token: 0x040023F5 RID: 9205
	private CanvasGroup group;

	// Token: 0x040023F6 RID: 9206
	private float currentAlpha;

	// Token: 0x040023F7 RID: 9207
	private bool isFadingIn;

	// Token: 0x040023F8 RID: 9208
	private bool chainContinue;

	// Token: 0x040023F9 RID: 9209
	private float holdFor;

	// Token: 0x040023FA RID: 9210
	private bool isFadingOut;

	// Token: 0x040023FB RID: 9211
	private TimeSince holdingSince;

	// Token: 0x040023FC RID: 9212
	private RectTransform rectTransform;

	// Token: 0x040023FD RID: 9213
	private float baseAlpha = 1f;
}
