using System;
using System.Collections.Generic;
using TMPro;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class SubtitleController : MonoSingleton<SubtitleController>
{
	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x060019AE RID: 6574 RVA: 0x000D3235 File Offset: 0x000D1435
	// (set) Token: 0x060019AF RID: 6575 RVA: 0x000D3249 File Offset: 0x000D1449
	public bool SubtitlesEnabled
	{
		get
		{
			return this.subtitlesEnabled && !HideUI.Active;
		}
		set
		{
			this.subtitlesEnabled = value;
		}
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x000D3252 File Offset: 0x000D1452
	private void Start()
	{
		this.SubtitlesEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("subtitlesEnabled", false);
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000D326A File Offset: 0x000D146A
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x000D3292 File Offset: 0x000D1492
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x000D32B4 File Offset: 0x000D14B4
	private void OnPrefChanged(string key, object value)
	{
		if (key == "subtitlesEnabled" && value is bool)
		{
			bool flag = (bool)value;
			this.SubtitlesEnabled = flag;
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x000D32E4 File Offset: 0x000D14E4
	private void Update()
	{
		if (this.delayedSubs.Count > 0)
		{
			for (int i = this.delayedSubs.Count - 1; i >= 0; i--)
			{
				if (this.delayedSubs[i] == null || this.delayedSubs[i].origin == null || !this.delayedSubs[i].origin.activeInHierarchy)
				{
					this.delayedSubs.RemoveAt(i);
				}
				else
				{
					this.delayedSubs[i].time = Mathf.MoveTowards(this.delayedSubs[i].time, 0f, Time.deltaTime);
					if (this.delayedSubs[i].time <= 0f)
					{
						this.DisplaySubtitle(this.delayedSubs[i].caption, null, false);
						this.delayedSubs.RemoveAt(i);
					}
				}
			}
		}
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000D33DB File Offset: 0x000D15DB
	public void NotifyHoldEnd(Subtitle self)
	{
		if (this.previousSubtitle == self)
		{
			this.previousSubtitle = null;
		}
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000D33F2 File Offset: 0x000D15F2
	public void DisplaySubtitleTranslated(string translationKey)
	{
		bool flag = this.SubtitlesEnabled;
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000D33FC File Offset: 0x000D15FC
	public void DisplaySubtitle(string caption, AudioSource audioSource = null, bool ignoreSetting = false)
	{
		if (!(ignoreSetting ? (!HideUI.Active) : this.SubtitlesEnabled))
		{
			return;
		}
		Subtitle subtitle = Object.Instantiate<Subtitle>(this.subtitleLine, this.container, true);
		subtitle.GetComponentInChildren<TMP_Text>().text = caption;
		if (audioSource != null)
		{
			subtitle.distanceCheckObject = audioSource;
		}
		subtitle.gameObject.SetActive(true);
		if (!this.previousSubtitle)
		{
			subtitle.ContinueChain();
		}
		else
		{
			this.previousSubtitle.nextInChain = subtitle;
		}
		this.previousSubtitle = subtitle;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000D3484 File Offset: 0x000D1684
	public void DisplaySubtitle(string caption, float time, GameObject origin)
	{
		SubtitleController.SubtitleData subtitleData = new SubtitleController.SubtitleData();
		subtitleData.caption = caption;
		subtitleData.time = time;
		subtitleData.origin = origin;
		this.delayedSubs.Add(subtitleData);
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x000D34B8 File Offset: 0x000D16B8
	public void CancelSubtitle(GameObject origin)
	{
		if (this.delayedSubs.Count > 0)
		{
			for (int i = this.delayedSubs.Count - 1; i >= 0; i--)
			{
				if (this.delayedSubs[i].origin == origin)
				{
					this.delayedSubs.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04002401 RID: 9217
	[SerializeField]
	private Transform container;

	// Token: 0x04002402 RID: 9218
	[SerializeField]
	private Subtitle subtitleLine;

	// Token: 0x04002403 RID: 9219
	private Subtitle previousSubtitle;

	// Token: 0x04002404 RID: 9220
	private List<SubtitleController.SubtitleData> delayedSubs = new List<SubtitleController.SubtitleData>();

	// Token: 0x04002405 RID: 9221
	private bool subtitlesEnabled;

	// Token: 0x02000463 RID: 1123
	public class SubtitleData
	{
		// Token: 0x04002406 RID: 9222
		public string caption;

		// Token: 0x04002407 RID: 9223
		public float time;

		// Token: 0x04002408 RID: 9224
		public GameObject origin;
	}
}
