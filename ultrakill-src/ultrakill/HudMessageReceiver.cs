using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000253 RID: 595
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class HudMessageReceiver : MonoSingleton<HudMessageReceiver>
{
	// Token: 0x06000D1C RID: 3356 RVA: 0x0006414C File Offset: 0x0006234C
	private void Start()
	{
		this.img = base.GetComponent<Image>();
		this.text = base.GetComponentInChildren<TMP_Text>();
		this.aud = base.GetComponent<AudioSource>();
		this.clickAud = this.text.GetComponent<AudioSource>();
		this.hoe = base.GetComponent<HudOpenEffect>();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0006419A File Offset: 0x0006239A
	private void Done()
	{
		this.img.enabled = false;
		this.text.enabled = false;
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x000641B4 File Offset: 0x000623B4
	public void SendHudMessage(string newmessage, string newinput = "", string newmessage2 = "", int delay = 0, bool silent = false, bool inputBeenProcessed = false, bool automaticTimer = true)
	{
		this.message = newmessage;
		this.input = newinput;
		this.message2 = newmessage2;
		this.noSound = silent;
		this.timer = automaticTimer;
		this.inputPreProcessed = inputBeenProcessed;
		base.Invoke("ShowHudMessage", (float)delay);
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x000641F4 File Offset: 0x000623F4
	private void ShowHudMessage()
	{
		if (this.input == "")
		{
			this.fullMessage = this.message;
		}
		else if (this.inputPreProcessed)
		{
			this.fullMessage = this.message + this.input + this.message2;
		}
		else
		{
			KeyCode keyCode = MonoSingleton<InputManager>.Instance.Inputs[this.input];
			string text;
			if (keyCode == KeyCode.Mouse0)
			{
				text = "Left Mouse Button";
			}
			else if (keyCode == KeyCode.Mouse1)
			{
				text = "Right Mouse Button";
			}
			else if (keyCode == KeyCode.Mouse2)
			{
				text = "Middle Mouse Button";
			}
			else
			{
				text = keyCode.ToString();
			}
			this.fullMessage = this.message + text + this.message2;
		}
		this.fullMessage = this.fullMessage.Replace('$', '\n');
		this.text.text = "";
		this.hoe.Force();
		if (!this.noSound)
		{
			this.aud.Play();
		}
		if (this.messageRoutine != null)
		{
			base.StopCoroutine(this.messageRoutine);
		}
		this.messageRoutine = base.StartCoroutine(this.PrepText());
		base.CancelInvoke("Done");
		if (this.timer)
		{
			base.Invoke("Done", 5f);
		}
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x00064349 File Offset: 0x00062549
	private IEnumerator PrepText()
	{
		this.text.enabled = true;
		this.img.enabled = true;
		yield return ScrollingText.ShowText(this.text, this.fullMessage, 0.005f, this.clickAud, false, true, true);
		this.messageRoutine = null;
		yield break;
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x00064358 File Offset: 0x00062558
	public void ForceEnable()
	{
		this.img.enabled = true;
		this.text.enabled = true;
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x00064372 File Offset: 0x00062572
	public void ClearMessage()
	{
		base.CancelInvoke("Done");
		this.Done();
	}

	// Token: 0x040011A2 RID: 4514
	private Image img;

	// Token: 0x040011A3 RID: 4515
	[HideInInspector]
	public TMP_Text text;

	// Token: 0x040011A4 RID: 4516
	private AudioSource aud;

	// Token: 0x040011A5 RID: 4517
	private AudioSource clickAud;

	// Token: 0x040011A6 RID: 4518
	private HudOpenEffect hoe;

	// Token: 0x040011A7 RID: 4519
	private string message;

	// Token: 0x040011A8 RID: 4520
	private string input;

	// Token: 0x040011A9 RID: 4521
	private bool inputPreProcessed;

	// Token: 0x040011AA RID: 4522
	private string message2;

	// Token: 0x040011AB RID: 4523
	private bool noSound;

	// Token: 0x040011AC RID: 4524
	private Coroutine messageRoutine;

	// Token: 0x040011AD RID: 4525
	private bool timer;

	// Token: 0x040011AE RID: 4526
	private string fullMessage;
}
