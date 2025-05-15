using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020003D5 RID: 981
public class ScrollingText : MonoBehaviour
{
	// Token: 0x06001637 RID: 5687 RVA: 0x000B3112 File Offset: 0x000B1312
	private void Awake()
	{
		if (!this.gotValues)
		{
			this.GetValues();
		}
	}

	// Token: 0x06001638 RID: 5688 RVA: 0x000B3122 File Offset: 0x000B1322
	private void GetValues()
	{
		if (this.gotValues)
		{
			return;
		}
		this.gotValues = true;
		this.text = base.GetComponent<TMP_Text>();
		this.message = this.text.text;
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001639 RID: 5689 RVA: 0x000B315D File Offset: 0x000B135D
	private void OnEnable()
	{
		if (this.oneTime && this.activated)
		{
			return;
		}
		this.activated = true;
		this.messageRoutine = base.StartCoroutine(this.PrepText());
	}

	// Token: 0x0600163A RID: 5690 RVA: 0x000B318C File Offset: 0x000B138C
	private void OnDisable()
	{
		if (this.messageRoutine != null)
		{
			if (this.oneTime && this.activated)
			{
				this.text.text = this.message;
				this.onComplete.Invoke("");
			}
			base.StopCoroutine(this.messageRoutine);
		}
	}

	// Token: 0x0600163B RID: 5691 RVA: 0x000B31DE File Offset: 0x000B13DE
	private IEnumerator PrepText()
	{
		yield return ScrollingText.ShowText(this.text, this.message, this.secondsBetweenLetters, this.aud, this.fillMissingText, false, this.writingCursor);
		UltrakillEvent ultrakillEvent = this.onComplete;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		this.messageRoutine = null;
		yield break;
	}

	// Token: 0x0600163C RID: 5692 RVA: 0x000B31ED File Offset: 0x000B13ED
	public static IEnumerator ShowText(TMP_Text text, string message, float secondsBetweenLetters = 0.005f, AudioSource clickAudio = null, bool fillMissingText = false, bool skipLineBreaks = false, bool writingCursor = false)
	{
		TimeSince textTimer = 0f;
		int currentLetter = 0;
		text.text = "";
		while (currentLetter < message.Length)
		{
			while (textTimer >= secondsBetweenLetters && currentLetter < message.Length)
			{
				textTimer -= secondsBetweenLetters;
				int num;
				if (message[currentLetter] == '<')
				{
					while (message[currentLetter] != '>')
					{
						if (currentLetter > message.Length)
						{
							break;
						}
						num = currentLetter;
						currentLetter = num + 1;
					}
				}
				else if (currentLetter < message.Length - 1)
				{
					while (currentLetter < message.Length - 1 && ((!writingCursor && message[currentLetter + 1] == ' ') || (skipLineBreaks && message[currentLetter + 1] == '\n')))
					{
						num = currentLetter;
						currentLetter = num + 1;
					}
				}
				num = currentLetter;
				currentLetter = num + 1;
				text.text = message.Substring(0, currentLetter);
				if (currentLetter < message.Length)
				{
					if (fillMissingText)
					{
						if (writingCursor)
						{
							text.text = string.Concat(new string[]
							{
								text.text,
								"<mark=#",
								ColorUtility.ToHtmlStringRGB(text.color),
								">",
								message[currentLetter].ToString(),
								"</mark><alpha=#00>",
								message.Substring(currentLetter + 1)
							});
						}
						else
						{
							text.text = text.text + "<alpha=#00>" + message.Substring(currentLetter);
						}
					}
					else if (writingCursor)
					{
						text.text += "█";
					}
				}
				if (clickAudio && message[currentLetter - 1] != '\n' && message[currentLetter - 1] != ' ')
				{
					clickAudio.Play();
				}
			}
			yield return new WaitForSeconds(secondsBetweenLetters);
		}
		yield break;
	}

	// Token: 0x04001E91 RID: 7825
	public bool oneTime;

	// Token: 0x04001E92 RID: 7826
	[HideInInspector]
	public bool activated;

	// Token: 0x04001E93 RID: 7827
	[HideInInspector]
	public bool gotValues;

	// Token: 0x04001E94 RID: 7828
	[HideInInspector]
	public TMP_Text text;

	// Token: 0x04001E95 RID: 7829
	[HideInInspector]
	public string message;

	// Token: 0x04001E96 RID: 7830
	[HideInInspector]
	public AudioSource aud;

	// Token: 0x04001E97 RID: 7831
	private Coroutine messageRoutine;

	// Token: 0x04001E98 RID: 7832
	public float secondsBetweenLetters = 0.005f;

	// Token: 0x04001E99 RID: 7833
	public bool fillMissingText;

	// Token: 0x04001E9A RID: 7834
	public bool writingCursor;

	// Token: 0x04001E9B RID: 7835
	public UltrakillEvent onComplete;
}
