using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000280 RID: 640
public class IntroText : MonoBehaviour
{
	// Token: 0x06000E33 RID: 3635 RVA: 0x000696CA File Offset: 0x000678CA
	private void Start()
	{
		this.txt = base.GetComponent<TMP_Text>();
		this.fullString = this.txt.text;
		this.aud = base.GetComponent<AudioSource>();
		base.StartCoroutine(this.TextAppear());
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x00069704 File Offset: 0x00067904
	private void Update()
	{
		if (this.waitingForInput)
		{
			if (!Input.GetKeyDown(KeyCode.Y))
			{
				Gamepad current = Gamepad.current;
				if (current == null || !current.buttonSouth.wasPressedThisFrame)
				{
					if (!Input.GetKeyDown(KeyCode.N))
					{
						Gamepad current2 = Gamepad.current;
						if (current2 == null || !current2.buttonEast.wasPressedThisFrame)
						{
							return;
						}
					}
					this.Over();
					return;
				}
			}
			this.waitingForInput = false;
			return;
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00069769 File Offset: 0x00067969
	public void DoneWithSetting()
	{
		this.calibrationWindows[this.calibrated].SetActive(false);
		this.calibrated++;
		this.readyToContinue = true;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x00069793 File Offset: 0x00067993
	private IEnumerator TextAppear()
	{
		int i = this.fullString.Length;
		int k;
		for (int j = 0; j < i; j = k + 1)
		{
			char c = this.fullString[j];
			float waitTime = 0.035f;
			bool playSound = true;
			char c2 = c;
			if (c == IntroTextOperators.PauseWithEllipsis)
			{
				this.sb = new StringBuilder(this.fullString);
				this.sb[j] = ' ';
				this.fullString = this.sb.ToString();
				this.txt.text = this.fullString.Substring(0, j);
				this.tempString = this.txt.text;
				this.doneWithDots = false;
				this.dotsAmount = 2;
				base.StartCoroutine(this.DotsAppear());
				yield return new WaitUntil(() => this.doneWithDots);
			}
			else if (c == IntroTextOperators.ShortPauseWithEllipsis)
			{
				this.sb = new StringBuilder(this.fullString);
				this.sb[j] = ' ';
				this.fullString = this.sb.ToString();
				this.txt.text = this.fullString.Substring(0, j);
				this.tempString = this.txt.text;
				this.doneWithDots = false;
				this.dotsAmount = 1;
				base.StartCoroutine(this.DotsAppear());
				yield return new WaitUntil(() => this.doneWithDots);
			}
			else if (c == IntroTextOperators.Pause)
			{
				this.sb = new StringBuilder(this.fullString);
				this.sb[j] = ' ';
				this.fullString = this.sb.ToString();
				playSound = false;
				waitTime = 0.75f;
				this.txt.text = this.fullString.Substring(0, j);
			}
			else if (c2 != ' ')
			{
				if (c == IntroTextOperators.DrawYes)
				{
					this.sb = new StringBuilder(this.fullString);
					if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad)
					{
						if (Gamepad.current.buttonSouth.displayName == "Cross")
						{
							this.sb[j] = 'X';
						}
						else
						{
							this.sb[j] = 'A';
						}
					}
					else
					{
						this.sb[j] = 'Y';
					}
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.DrawNo)
				{
					this.sb = new StringBuilder(this.fullString);
					if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad)
					{
						if (Gamepad.current.buttonEast.displayName == "Circle")
						{
							this.sb[j] = 'O';
						}
						else
						{
							this.sb[j] = 'B';
						}
					}
					else
					{
						this.sb[j] = 'N';
					}
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.WaitForYesNo)
				{
					waitTime = 0f;
					this.sb = new StringBuilder(this.fullString);
					this.sb[j] = ' ';
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j);
					this.waitingForInput = true;
					yield return new WaitUntil(() => !this.waitingForInput);
				}
				else if (c == IntroTextOperators.ActivateOnTextTrigger)
				{
					waitTime = 0f;
					this.sb = new StringBuilder(this.fullString);
					this.sb[j] = ' ';
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j);
					GameObject[] array = this.activateOnTextTrigger;
					for (k = 0; k < array.Length; k++)
					{
						array[k].SetActive(true);
					}
				}
				else if (c == IntroTextOperators.EndIntro)
				{
					base.GetComponentInParent<IntroTextController>().introOver = true;
					GameProgressSaver.SetIntro(true);
					waitTime = 0f;
					this.sb = new StringBuilder(this.fullString);
					this.sb[j] = ' ';
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.ShowCalibrationMenu)
				{
					this.sb = new StringBuilder(this.fullString);
					this.sb[j] = ' ';
					this.fullString = this.sb.ToString();
					this.txt.text = this.fullString.Substring(0, j) + "<color=red>ERROR</color>";
					yield return new WaitForSecondsRealtime(1f);
					this.calibrationWindows[this.calibrated].SetActive(true);
					this.readyToContinue = false;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					yield return new WaitUntil(() => this.readyToContinue);
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
					this.tempString = "<color=green>OK</color>";
					this.fullString = this.fullString.Insert(j, this.tempString);
					j += this.tempString.Length;
					i += this.tempString.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.BeginColorRed)
				{
					this.colorString = "<color=red>";
					this.PlaceColor(j);
					j += this.colorString.Length;
					i += this.colorString.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.BeginColorGreen)
				{
					this.colorString = "<color=green>";
					this.PlaceColor(j);
					j += this.colorString.Length;
					i += this.colorString.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.BeginColorGrey)
				{
					this.colorString = "<color=grey>";
					this.PlaceColor(j);
					j += this.colorString.Length;
					i += this.colorString.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.BeginColorBlue)
				{
					this.colorString = "<color=#4C99E6>";
					this.PlaceColor(j);
					j += this.colorString.Length;
					i += this.colorString.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else if (c == IntroTextOperators.EndColor)
				{
					this.colorsClosePositions.Add(j);
					this.sb = new StringBuilder(this.fullString);
					this.sb[j] = ' ';
					this.fullString = this.sb.ToString();
					string text = "</color>";
					this.fullString = this.fullString.Insert(j, text);
					j += text.Length;
					i += text.Length;
					this.txt.text = this.fullString.Substring(0, j);
				}
				else
				{
					this.txt.text = this.fullString.Substring(0, j);
				}
			}
			else
			{
				waitTime = 0f;
				this.txt.text = this.fullString.Substring(0, j);
			}
			i = this.fullString.Length;
			if (waitTime != 0f && playSound)
			{
				this.aud.Play();
			}
			if (this.colorsPositions.Count > this.colorsClosePositions.Count)
			{
				TMP_Text tmp_Text = this.txt;
				tmp_Text.text += "</color>";
			}
			yield return new WaitForSecondsRealtime(waitTime);
			k = j;
		}
		this.Over();
		yield break;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x000697A4 File Offset: 0x000679A4
	private void PlaceColor(int i)
	{
		this.colorsPositions.Add(i);
		this.sb = new StringBuilder(this.fullString);
		this.sb[i] = ' ';
		this.fullString = this.sb.ToString();
		this.fullString = this.fullString.Insert(i, this.colorString);
	}

	// Token: 0x06000E38 RID: 3640 RVA: 0x00069808 File Offset: 0x00067A08
	private void Over()
	{
		GameObject[] array = this.activateOnEnd;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(true);
		}
		array = this.deactivateOnEnd;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x06000E39 RID: 3641 RVA: 0x00069851 File Offset: 0x00067A51
	private IEnumerator DotsAppear()
	{
		int num;
		for (int i = 0; i < this.dotsAmount; i = num + 1)
		{
			this.txt.text = this.tempString;
			this.aud.Play();
			yield return new WaitForSecondsRealtime(0.25f);
			this.txt.text = this.tempString + ".";
			this.aud.Play();
			yield return new WaitForSecondsRealtime(0.25f);
			this.txt.text = this.tempString + "..";
			this.aud.Play();
			yield return new WaitForSecondsRealtime(0.25f);
			this.txt.text = this.tempString + "...";
			this.aud.Play();
			yield return new WaitForSecondsRealtime(0.25f);
			num = i;
		}
		this.doneWithDots = true;
		yield break;
	}

	// Token: 0x0400129F RID: 4767
	private TMP_Text txt;

	// Token: 0x040012A0 RID: 4768
	private string fullString;

	// Token: 0x040012A1 RID: 4769
	private string tempString;

	// Token: 0x040012A2 RID: 4770
	private StringBuilder sb;

	// Token: 0x040012A3 RID: 4771
	private bool doneWithDots;

	// Token: 0x040012A4 RID: 4772
	private bool readyToContinue;

	// Token: 0x040012A5 RID: 4773
	private bool waitingForInput;

	// Token: 0x040012A6 RID: 4774
	private AudioSource aud;

	// Token: 0x040012A7 RID: 4775
	private int dotsAmount = 3;

	// Token: 0x040012A8 RID: 4776
	private int calibrated;

	// Token: 0x040012A9 RID: 4777
	public GameObject[] calibrationWindows;

	// Token: 0x040012AA RID: 4778
	public GameObject[] activateOnEnd;

	// Token: 0x040012AB RID: 4779
	public GameObject[] deactivateOnEnd;

	// Token: 0x040012AC RID: 4780
	public GameObject[] activateOnTextTrigger;

	// Token: 0x040012AD RID: 4781
	private string colorString;

	// Token: 0x040012AE RID: 4782
	private List<int> colorsPositions = new List<int>();

	// Token: 0x040012AF RID: 4783
	private List<int> colorsClosePositions = new List<int>();
}
