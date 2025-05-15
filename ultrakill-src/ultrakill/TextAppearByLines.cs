using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class TextAppearByLines : MonoBehaviour
{
	// Token: 0x06001A31 RID: 6705 RVA: 0x000D82B2 File Offset: 0x000D64B2
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.tmp = base.GetComponent<TMP_Text>();
		this.fullText = this.tmp.text;
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x000D82DD File Offset: 0x000D64DD
	private void OnEnable()
	{
		this.tmp.text = "";
		this.coroutine = base.StartCoroutine(this.AppearText());
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000D8301 File Offset: 0x000D6501
	private IEnumerator AppearText()
	{
		int currentChar = 0;
		while (currentChar < this.fullText.Length)
		{
			if (this.aud)
			{
				if (this.fullText[currentChar] == '<')
				{
					this.aud.clip = this.warningSound;
				}
				else
				{
					this.aud.clip = this.errorSound;
				}
				this.aud.Play();
			}
			int num;
			while (currentChar < this.fullText.Length && this.fullText[currentChar] != '\n')
			{
				num = currentChar;
				currentChar = num + 1;
			}
			this.tmp.text = this.fullText.Substring(0, currentChar);
			num = currentChar;
			currentChar = num + 1;
			yield return new WaitForSeconds(this.delay);
		}
		this.coroutine = null;
		yield break;
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x000D8310 File Offset: 0x000D6510
	private void OnDisable()
	{
		this.Stop();
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000D8318 File Offset: 0x000D6518
	public void Stop()
	{
		this.aud.Stop();
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
			this.coroutine = null;
		}
	}

	// Token: 0x040024B4 RID: 9396
	[SerializeField]
	private float delay;

	// Token: 0x040024B5 RID: 9397
	private AudioSource aud;

	// Token: 0x040024B6 RID: 9398
	private TMP_Text tmp;

	// Token: 0x040024B7 RID: 9399
	private string fullText;

	// Token: 0x040024B8 RID: 9400
	private Coroutine coroutine;

	// Token: 0x040024B9 RID: 9401
	[SerializeField]
	private AudioClip errorSound;

	// Token: 0x040024BA RID: 9402
	[SerializeField]
	private AudioClip warningSound;
}
