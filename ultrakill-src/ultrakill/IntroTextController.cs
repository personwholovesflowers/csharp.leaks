using System;
using SettingsMenu.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// Token: 0x02000284 RID: 644
public class IntroTextController : MonoBehaviour
{
	// Token: 0x06000E4D RID: 3661 RVA: 0x0006A4E0 File Offset: 0x000686E0
	private void Awake()
	{
		this.audmix = new AudioMixer[]
		{
			MonoSingleton<AudioMixerController>.Instance.allSound,
			MonoSingleton<AudioMixerController>.Instance.musicSound
		};
		this.firstTime = !GameProgressSaver.GetIntro();
		if (this.firstTime && !GameStateManager.Instance.introCheckComplete)
		{
			this.soundSlider.value = 100f;
			this.sfxSlider.value = 0f;
			this.musicSlider.value = 0f;
			MonoSingleton<PrefsManager>.Instance.SetFloat("allVolume", 1f);
			MonoSingleton<PrefsManager>.Instance.SetFloat("musicVolume", 0f);
			MonoSingleton<PrefsManager>.Instance.SetFloat("sfxVolume", 0f);
			this.page1Screen.SetActive(true);
			return;
		}
		this.soundSlider.value = MonoSingleton<PrefsManager>.Instance.GetFloat("allVolume", 0f) * 100f;
		this.sfxSlider.value = MonoSingleton<PrefsManager>.Instance.GetFloat("sfxVolume", 0f) * 100f;
		this.musicSlider.value = MonoSingleton<PrefsManager>.Instance.GetFloat("musicVolume", 0f) * 100f;
		this.page1SecondTimeScreen.SetActive(true);
	}

	// Token: 0x06000E4E RID: 3662 RVA: 0x0006A62C File Offset: 0x0006882C
	public void DoneWithSetting()
	{
		if (this.page1Screen.activeSelf)
		{
			this.page1Screen.GetComponent<IntroText>().DoneWithSetting();
		}
		if (this.page1SecondTimeScreen.activeSelf)
		{
			this.page1SecondTimeScreen.GetComponent<IntroText>().DoneWithSetting();
		}
	}

	// Token: 0x06000E4F RID: 3663 RVA: 0x0006A668 File Offset: 0x00068868
	public void ApplyPreset(SettingsPreset preset)
	{
		preset.Apply();
	}

	// Token: 0x06000E50 RID: 3664 RVA: 0x0006A670 File Offset: 0x00068870
	private void Start()
	{
		float num = 0f;
		this.audmix[0].GetFloat("allVolume", out num);
		Debug.Log("Mixer Volume " + num.ToString());
		AudioMixer[] array = this.audmix;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat("allVolume", -80f);
		}
		this.audmix[0].GetFloat("allVolume", out num);
		Debug.Log("Mixer Volume " + num.ToString());
		base.Invoke("SlowDown", 0.1f);
		MonoSingleton<OptionsManager>.Instance.inIntro = true;
		this.rb = MonoSingleton<NewMovement>.Instance.GetComponent<Rigidbody>();
		this.rb.velocity = Vector3.zero;
		this.rb.useGravity = false;
	}

	// Token: 0x06000E51 RID: 3665 RVA: 0x0006A748 File Offset: 0x00068948
	private void SlowDown()
	{
		this.inMenu = true;
	}

	// Token: 0x06000E52 RID: 3666 RVA: 0x0006A754 File Offset: 0x00068954
	private void Update()
	{
		if (this.inMenu)
		{
			this.rb.velocity = Vector3.zero;
			this.rb.useGravity = false;
			if (this.page2Screen.activeSelf)
			{
				MonoSingleton<NewMovement>.Instance.GetComponent<Rigidbody>().useGravity = true;
				this.inMenu = false;
			}
			if (!this.firstTime && MonoSingleton<InputManager>.Instance.InputSource.Pause.WasPerformedThisFrame)
			{
				this.inMenu = false;
				this.introOver = true;
				this.skipped = true;
				MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.down * 100f;
				return;
			}
		}
		else if (this.introOver)
		{
			if (!this.img)
			{
				this.img = base.GetComponent<Image>();
				if (this.page1Screen.activeSelf)
				{
					this.page1Text = this.page1Screen.GetComponent<TMP_Text>();
				}
				else if (this.page1SecondTimeScreen.activeSelf)
				{
					this.page1Text = this.page1SecondTimeScreen.GetComponent<TMP_Text>();
				}
				this.page2Text = this.page2Screen.GetComponent<TMP_Text>();
				this.fadeValue = 1f;
				this.page2NoFade.SetActive(!this.skipped);
				MonoSingleton<AudioMixerController>.Instance.forceOff = false;
			}
			if (this.fadeValue > 0f)
			{
				this.fadeValue = Mathf.MoveTowards(this.fadeValue, 0f, Time.deltaTime * (this.skipped ? 1f : 0.375f));
				Color color = this.img.color;
				color.a = this.fadeValue;
				this.img.color = color;
				foreach (AudioMixer audioMixer in this.audmix)
				{
					float num = 0f;
					audioMixer.GetFloat("allVolume", out num);
					if (audioMixer == MonoSingleton<AudioMixerController>.Instance.musicSound && MonoSingleton<AudioMixerController>.Instance.musicVolume > 0f)
					{
						audioMixer.SetFloat("allVolume", Mathf.MoveTowards(num, Mathf.Log10(MonoSingleton<AudioMixerController>.Instance.musicVolume) * 20f, Time.deltaTime * Mathf.Abs(num)));
					}
					else if (audioMixer == MonoSingleton<AudioMixerController>.Instance.allSound)
					{
						audioMixer.SetFloat("allVolume", Mathf.MoveTowards(num, Mathf.Log10(MonoSingleton<AudioMixerController>.Instance.sfxVolume) * 20f, Time.deltaTime * Mathf.Abs(num)));
					}
				}
				if (this.page1Text)
				{
					color = this.page1Text.color;
					color.a = this.fadeValue;
					this.page1Text.color = color;
				}
				color = this.page2Text.color;
				color.a = this.fadeValue;
				this.page2Text.color = color;
				return;
			}
			if (this.introOverWait > 0f)
			{
				if (this.introOverWait == 1f)
				{
					foreach (AudioMixer audioMixer2 in this.audmix)
					{
						if (audioMixer2 == MonoSingleton<AudioMixerController>.Instance.musicSound && MonoSingleton<AudioMixerController>.Instance.musicVolume > 0f)
						{
							audioMixer2.SetFloat("allVolume", Mathf.Log10(MonoSingleton<AudioMixerController>.Instance.musicVolume) * 20f);
						}
						else if (audioMixer2 == MonoSingleton<AudioMixerController>.Instance.allSound && MonoSingleton<AudioMixerController>.Instance.sfxVolume > 0f)
						{
							audioMixer2.SetFloat("allVolume", Mathf.Log10(MonoSingleton<AudioMixerController>.Instance.sfxVolume) * 20f);
						}
					}
				}
				this.introOverWait = Mathf.MoveTowards(this.introOverWait, 0f, Time.deltaTime);
				return;
			}
			MonoSingleton<OptionsManager>.Instance.inIntro = false;
			GameObject[] array2 = this.deactivateOnIntroEnd;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].SetActive(false);
			}
		}
	}

	// Token: 0x040012BD RID: 4797
	public bool firstTime;

	// Token: 0x040012BE RID: 4798
	public GameObject page1Screen;

	// Token: 0x040012BF RID: 4799
	public GameObject page1SecondTimeScreen;

	// Token: 0x040012C0 RID: 4800
	public GameObject page2Screen;

	// Token: 0x040012C1 RID: 4801
	public GameObject page2NoFade;

	// Token: 0x040012C2 RID: 4802
	public GameObject[] deactivateOnIntroEnd;

	// Token: 0x040012C3 RID: 4803
	public Slider soundSlider;

	// Token: 0x040012C4 RID: 4804
	public Slider sfxSlider;

	// Token: 0x040012C5 RID: 4805
	public Slider musicSlider;

	// Token: 0x040012C6 RID: 4806
	private AudioMixer[] audmix;

	// Token: 0x040012C7 RID: 4807
	private Image img;

	// Token: 0x040012C8 RID: 4808
	private TMP_Text page1Text;

	// Token: 0x040012C9 RID: 4809
	private TMP_Text page2Text;

	// Token: 0x040012CA RID: 4810
	private float fadeValue;

	// Token: 0x040012CB RID: 4811
	private bool inMenu;

	// Token: 0x040012CC RID: 4812
	public bool introOver;

	// Token: 0x040012CD RID: 4813
	private bool skipped;

	// Token: 0x040012CE RID: 4814
	private float introOverWait = 1f;

	// Token: 0x040012CF RID: 4815
	private Rigidbody rb;
}
