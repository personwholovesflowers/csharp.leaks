using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000287 RID: 647
public class IntroViolenceScreen : MonoBehaviour
{
	// Token: 0x06000E55 RID: 3669 RVA: 0x0006ABE4 File Offset: 0x00068DE4
	private void Start()
	{
		this.img = base.GetComponent<Image>();
		this.vp = base.GetComponent<VideoPlayer>();
		this.vp.SetDirectAudioVolume(0, MonoSingleton<PrefsManager>.Instance.GetFloat("allVolume", 0f) / 2f);
		AudioSource audioSource;
		if (this.loadingScreen && this.loadingScreen.TryGetComponent<AudioSource>(out audioSource))
		{
			audioSource.volume = MonoSingleton<PrefsManager>.Instance.GetFloat("allVolume", 0f) / 2f;
		}
		Application.targetFrameRate = Screen.currentResolution.refreshRate;
		QualitySettings.vSyncCount = (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("vSync", false) ? 1 : 0);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = false;
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0006ACA4 File Offset: 0x00068EA4
	private string GetTargetScene()
	{
		this.shouldLoadTutorial = !GameProgressSaver.GetIntro() || !GameProgressSaver.GetTutorial();
		if (!this.shouldLoadTutorial)
		{
			return "Main Menu";
		}
		return "Tutorial";
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0006ACD4 File Offset: 0x00068ED4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
		{
			this.Skip();
		}
		if (Gamepad.current != null && (Gamepad.current.startButton.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame))
		{
			this.Skip();
		}
		if (!this.videoOver && this.vp.isPaused)
		{
			this.videoOver = true;
			this.vp.Stop();
			base.Invoke("FadeOut", 1f);
		}
		if (this.fade)
		{
			this.fadeAmount = Mathf.MoveTowards(this.fadeAmount, this.targetAlpha, Time.deltaTime);
			Color color = this.img.color;
			color.a = this.fadeAmount;
			if (this.targetAlpha == 1f)
			{
				this.img.color = color;
			}
			else
			{
				this.red.color = color;
			}
			if (this.fadeAmount == this.targetAlpha)
			{
				if (this.fadeAmount == 1f)
				{
					this.fade = false;
					this.targetAlpha = 0f;
					base.Invoke("Red", 1.5f);
					base.Invoke("FadeOut", 3f);
					return;
				}
				SceneHelper.LoadScene(this.GetTargetScene(), false);
				base.enabled = false;
			}
		}
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x0006AE30 File Offset: 0x00069030
	private void Skip()
	{
		if (this.vp.isPlaying)
		{
			this.vp.Stop();
			base.Invoke("FadeOut", 1f);
			return;
		}
		if (this.fade)
		{
			this.img.enabled = false;
			this.targetAlpha = 0f;
			return;
		}
		base.CancelInvoke("FadeOut");
		this.img.enabled = false;
		this.targetAlpha = 0f;
		this.fade = true;
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0006AEB0 File Offset: 0x000690B0
	private void Red()
	{
		this.red.color = new Color(1f, 1f, 1f, 1f);
		this.red.GetComponent<AudioSource>().Play();
		this.img.enabled = false;
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x0006AEFD File Offset: 0x000690FD
	private void FadeOut()
	{
		this.fade = true;
	}

	// Token: 0x040012EC RID: 4844
	private Image img;

	// Token: 0x040012ED RID: 4845
	private float fadeAmount;

	// Token: 0x040012EE RID: 4846
	private bool fade;

	// Token: 0x040012EF RID: 4847
	private float targetAlpha = 1f;

	// Token: 0x040012F0 RID: 4848
	private VideoPlayer vp;

	// Token: 0x040012F1 RID: 4849
	private bool videoOver;

	// Token: 0x040012F2 RID: 4850
	[SerializeField]
	private GameObject loadingScreen;

	// Token: 0x040012F3 RID: 4851
	[SerializeField]
	private Image red;

	// Token: 0x040012F4 RID: 4852
	private bool shouldLoadTutorial;

	// Token: 0x040012F5 RID: 4853
	private bool bundlesLoaded;
}
