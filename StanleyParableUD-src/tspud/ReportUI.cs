using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000054 RID: 84
public class ReportUI : MonoBehaviour
{
	// Token: 0x0600021C RID: 540 RVA: 0x0000FB70 File Offset: 0x0000DD70
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		if (!this.useHardcodedCredentials)
		{
			if (PlayerPrefs.HasKey(Application.productName + "_REPORTUSERNAME"))
			{
				this.inputUsername.text = PlayerPrefs.GetString(Application.productName + "_REPORTUSERNAME");
			}
			if (PlayerPrefs.HasKey(Application.productName + "_REPORTPASSWORD"))
			{
				this.inputPassword.text = PlayerPrefs.GetString(Application.productName + "_REPORTPASSWORD");
			}
			if (PlayerPrefs.HasKey(Application.productName + "_REPORTSMTP"))
			{
				this.inputSmpt.text = PlayerPrefs.GetString(Application.productName + "_REPORTSMTP");
			}
			if (PlayerPrefs.HasKey(Application.productName + "_REPORTRECEIVER"))
			{
				this.inputReceiver.text = PlayerPrefs.GetString(Application.productName + "_REPORTRECEIVER");
			}
		}
		this.audioSource = base.gameObject.AddComponent<AudioSource>();
		this.audioSource.clip = this.screenshotSFX;
		this.canvas.enabled = false;
		this.Tool.gameObject.SetActive(false);
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000FCA6 File Offset: 0x0000DEA6
	private void Start()
	{
		Painter painter = this.painter;
		painter._OnDisable = (Action)Delegate.Combine(painter._OnDisable, new Action(this.UpdateUITexture));
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0000FCD0 File Offset: 0x0000DED0
	public void ReportIssue()
	{
		List<RawImage> list = this.screenshotSlots.FindAll((RawImage ri) => ri.texture != null);
		List<Texture2D> list2 = new List<Texture2D>();
		foreach (RawImage rawImage in list)
		{
			list2.Add((Texture2D)rawImage.texture);
		}
		try
		{
			string text = "";
			if (this.reporter.SendReport(this.inputUsername.text, this.inputPassword.text, this.inputSmpt.text, this.inputReceiver.text, this.inputTitle.text, this.inputDescription.text, list2, out text))
			{
				this.inputTitle.text = "";
				this.inputDescription.text = "";
				this.RemoveScreenshot(0);
				this.RemoveScreenshot(1);
				this.RemoveScreenshot(2);
				if (!this.useHardcodedCredentials)
				{
					PlayerPrefs.SetString(Application.productName + "_REPORTUSERNAME", this.inputUsername.text);
					PlayerPrefs.SetString(Application.productName + "_REPORTPASSWORD", this.inputPassword.text);
					PlayerPrefs.SetString(Application.productName + "_REPORTRECEIVER", this.inputReceiver.text);
					PlayerPrefs.SetString(Application.productName + "_REPORTSMTP", this.inputSmpt.text);
				}
				this.canvas.enabled = false;
				this.Tool.gameObject.SetActive(false);
				base.StartCoroutine(this.ShowStatus("Report sent!"));
			}
			else
			{
				this.canvas.enabled = false;
				this.Tool.gameObject.SetActive(false);
				base.StartCoroutine(this.ShowStatus("Error: " + text));
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			base.StartCoroutine(this.ShowStatus("Error: " + ex.Message));
		}
	}

	// Token: 0x0600021F RID: 543 RVA: 0x0000FF28 File Offset: 0x0000E128
	public void TakeScreenshotButton()
	{
		base.StartCoroutine(this.TakeScreenshotRoutine());
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000FF37 File Offset: 0x0000E137
	private IEnumerator ShowStatus(string message)
	{
		this.StatusText.text = message;
		this.StatusText.enabled = true;
		yield return new WaitForSeconds(3.5f);
		this.StatusText.enabled = false;
		yield break;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000FF4D File Offset: 0x0000E14D
	public IEnumerator TakeScreenshotRoutine()
	{
		int index = this.screenshotSlots.FindIndex((RawImage ri) => ri.texture == null);
		if (index < 0)
		{
			yield break;
		}
		bool canvasActive = this.canvas.enabled;
		if (canvasActive)
		{
			this.canvas.enabled = false;
		}
		yield return new WaitForEndOfFrame();
		this.screenshotSlots[index].texture = ScreenshotTool.TakeScreenshot();
		this.screenshotSlots[index].color = Color.white;
		this.audioSource.Play();
		if (canvasActive)
		{
			this.canvas.enabled = true;
		}
		yield break;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000FF5C File Offset: 0x0000E15C
	public void OpenPainter(int screenshotIndex)
	{
		this._screenshotIndex = screenshotIndex;
		if (this.screenshotSlots[screenshotIndex].texture != null)
		{
			this.painter.enabled = true;
			this.painter.baseTex = (Texture2D)this.screenshotSlots[screenshotIndex].texture;
		}
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
	public void UpdateUITexture()
	{
		this.screenshotSlots[this._screenshotIndex].texture = this.painter.baseTex;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000FFD9 File Offset: 0x0000E1D9
	public void BroadcastOpenReportTool()
	{
		if (ReportUI.OnOpenReportTool != null)
		{
			ReportUI.OnOpenReportTool();
		}
		ReportUI.REPORTUIACTIVE = true;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000FFF2 File Offset: 0x0000E1F2
	public void BroadcastCloseReportTool()
	{
		ReportUI.REPORTUIACTIVE = false;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000FFFA File Offset: 0x0000E1FA
	public void RemoveScreenshot(int screenshotIndex)
	{
		this.screenshotSlots[screenshotIndex].texture = null;
		this.screenshotSlots[screenshotIndex].color = new Color(0.06082088f, 0.2481493f, 0.326f, 1f);
	}

	// Token: 0x04000238 RID: 568
	public static Action OnOpenReportTool;

	// Token: 0x04000239 RID: 569
	public static bool REPORTUIACTIVE;

	// Token: 0x0400023A RID: 570
	public bool pauseGameOnActive = true;

	// Token: 0x0400023B RID: 571
	[SerializeField]
	public bool useHardcodedCredentials;

	// Token: 0x0400023C RID: 572
	public Canvas canvas;

	// Token: 0x0400023D RID: 573
	public InputField inputUsername;

	// Token: 0x0400023E RID: 574
	public InputField inputPassword;

	// Token: 0x0400023F RID: 575
	public InputField inputTitle;

	// Token: 0x04000240 RID: 576
	public InputField inputDescription;

	// Token: 0x04000241 RID: 577
	public InputField inputSmpt;

	// Token: 0x04000242 RID: 578
	public InputField inputReceiver;

	// Token: 0x04000243 RID: 579
	public Painter painter;

	// Token: 0x04000244 RID: 580
	public List<RawImage> screenshotSlots;

	// Token: 0x04000245 RID: 581
	public Text StatusText;

	// Token: 0x04000246 RID: 582
	public GameObject Tool;

	// Token: 0x04000247 RID: 583
	[SerializeField]
	private AudioClip screenshotSFX;

	// Token: 0x04000248 RID: 584
	private AudioSource audioSource;

	// Token: 0x04000249 RID: 585
	[SerializeField]
	private EmailReporter reporter;

	// Token: 0x0400024A RID: 586
	private bool paused;

	// Token: 0x0400024B RID: 587
	private int _screenshotIndex;
}
