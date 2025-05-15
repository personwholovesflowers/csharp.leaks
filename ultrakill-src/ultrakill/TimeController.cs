using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000480 RID: 1152
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class TimeController : MonoSingleton<TimeController>
{
	// Token: 0x06001A64 RID: 6756 RVA: 0x000D9760 File Offset: 0x000D7960
	protected override void Awake()
	{
		base.Awake();
		this.audmix = new AudioMixer[]
		{
			MonoSingleton<AudioMixerController>.Instance.allSound,
			MonoSingleton<AudioMixerController>.Instance.goreSound,
			MonoSingleton<AudioMixerController>.Instance.musicSound,
			MonoSingleton<AudioMixerController>.Instance.doorSound
		};
		if (MonoSingleton<AssistController>.Instance && MonoSingleton<AssistController>.Instance.majorEnabled)
		{
			this.timeScale = MonoSingleton<AssistController>.Instance.gameSpeed;
		}
		else
		{
			this.timeScale = 1f;
		}
		Time.timeScale = this.timeScale * this.timeScaleModifier;
		if (MonoSingleton<OptionsManager>.Instance.mainMenu)
		{
			AudioMixer[] array = this.audmix;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetFloat("allPitch", this.timeScale / (MonoSingleton<AssistController>.Instance.majorEnabled ? MonoSingleton<AssistController>.Instance.gameSpeed : 1f));
			}
		}
		this.parryFlashEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("parryFlash", false);
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000D9863 File Offset: 0x000D7A63
	protected override void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000D9885 File Offset: 0x000D7A85
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x000D98A8 File Offset: 0x000D7AA8
	private void OnPrefChanged(string id, object value)
	{
		if (id == "parryFlash" && value is bool)
		{
			bool flag = (bool)value;
			this.parryFlashEnabled = flag;
		}
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x000D98D8 File Offset: 0x000D7AD8
	private void Update()
	{
		if (this.controlTimeScale)
		{
			if (MonoSingleton<AssistController>.Instance.majorEnabled && this.timeScale != MonoSingleton<AssistController>.Instance.gameSpeed)
			{
				this.timeScale = MonoSingleton<AssistController>.Instance.gameSpeed;
				Time.timeScale = this.timeScale * this.timeScaleModifier;
				return;
			}
			if (!MonoSingleton<AssistController>.Instance.majorEnabled && this.timeScale != 1f)
			{
				this.timeScale = 1f;
				Time.timeScale = this.timeScale * this.timeScaleModifier;
			}
		}
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000D9964 File Offset: 0x000D7B64
	private void FixedUpdate()
	{
		if (MonoSingleton<OptionsManager>.Instance.paused && !MonoSingleton<OptionsManager>.Instance.mainMenu)
		{
			return;
		}
		if (this.slowDown < this.timeScale * this.timeScaleModifier)
		{
			this.slowDown = Mathf.MoveTowards(this.slowDown, this.timeScale * this.timeScaleModifier, 0.02f);
			Time.timeScale = this.slowDown;
			if (this.controlPitch)
			{
				AudioMixer[] array = this.audmix;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat("allPitch", this.slowDown / this.timeScale / (MonoSingleton<AssistController>.Instance.majorEnabled ? MonoSingleton<AssistController>.Instance.gameSpeed : 1f));
				}
				return;
			}
		}
		else if (this.controlPitch)
		{
			AudioMixer[] array = this.audmix;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetFloat("allPitch", this.timeScale / (MonoSingleton<AssistController>.Instance.majorEnabled ? MonoSingleton<AssistController>.Instance.gameSpeed : 1f));
			}
		}
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000D9A7C File Offset: 0x000D7C7C
	public void ParryFlash()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.parryLight, MonoSingleton<PlayerTracker>.Instance.GetTarget().position, Quaternion.identity, MonoSingleton<PlayerTracker>.Instance.GetTarget());
		Light light;
		if (this.parryFlashEnabled)
		{
			if (this.parryFlash != null)
			{
				this.parryFlash.SetActive(true);
			}
			base.Invoke("HideFlash", 0.1f);
		}
		else if (gameObject.TryGetComponent<Light>(out light))
		{
			light.enabled = false;
		}
		this.TrueStop(0.25f);
		MonoSingleton<CameraController>.Instance.CameraShake(0.5f);
		MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.ParryFlash);
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000D9B22 File Offset: 0x000D7D22
	private void HideFlash()
	{
		GameObject gameObject = this.parryFlash;
		if (gameObject != null)
		{
			gameObject.SetActive(false);
		}
		if (MonoSingleton<CrowdReactions>.Instance && MonoSingleton<CrowdReactions>.Instance.enabled)
		{
			MonoSingleton<CrowdReactions>.Instance.React(MonoSingleton<CrowdReactions>.Instance.cheer);
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000D9B62 File Offset: 0x000D7D62
	public void SlowDown(float amount)
	{
		if (amount <= 0f)
		{
			amount = 0.01f;
		}
		this.slowDown = amount;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000D9B7A File Offset: 0x000D7D7A
	public void HitStop(float length)
	{
		if (length > this.currentStop)
		{
			this.currentStop = length;
			Time.timeScale = 0f;
			base.StartCoroutine(this.TimeIsStopped(length, false));
		}
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x000D9BA8 File Offset: 0x000D7DA8
	public void TrueStop(float length)
	{
		if (length > this.currentStop)
		{
			this.currentStop = length;
			if (this.controlPitch)
			{
				AudioMixer[] array = this.audmix;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat("allPitch", 0f);
				}
			}
			Time.timeScale = 0f;
			base.StartCoroutine(this.TimeIsStopped(length, true));
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x000D9C0E File Offset: 0x000D7E0E
	private IEnumerator TimeIsStopped(float length, bool trueStop)
	{
		yield return new WaitForSecondsRealtime(length);
		this.ContinueTime(length, trueStop);
		yield break;
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000D9C2C File Offset: 0x000D7E2C
	private void ContinueTime(float length, bool trueStop)
	{
		if (length >= this.currentStop)
		{
			Time.timeScale = this.timeScale * this.timeScaleModifier;
			if (trueStop && this.controlPitch)
			{
				AudioMixer[] array = this.audmix;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat("allPitch", 1f);
				}
			}
			this.currentStop = 0f;
		}
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x000D9C92 File Offset: 0x000D7E92
	public void RestoreTime()
	{
		Time.timeScale = this.timeScale * this.timeScaleModifier;
		this.currentStop = 0f;
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000D9CB4 File Offset: 0x000D7EB4
	public void SetAllPitch(float pitch)
	{
		AudioMixer[] array = this.audmix;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat("allPitch", pitch);
		}
	}

	// Token: 0x040024FD RID: 9469
	[SerializeField]
	private GameObject parryLight;

	// Token: 0x040024FE RID: 9470
	[SerializeField]
	private GameObject parryFlash;

	// Token: 0x040024FF RID: 9471
	private float currentStop;

	// Token: 0x04002500 RID: 9472
	private AudioMixer[] audmix;

	// Token: 0x04002501 RID: 9473
	[HideInInspector]
	public bool controlTimeScale = true;

	// Token: 0x04002502 RID: 9474
	[HideInInspector]
	public bool controlPitch = true;

	// Token: 0x04002503 RID: 9475
	[HideInInspector]
	public bool parryFlashEnabled = true;

	// Token: 0x04002504 RID: 9476
	public float timeScale = 1f;

	// Token: 0x04002505 RID: 9477
	public float timeScaleModifier = 1f;

	// Token: 0x04002506 RID: 9478
	private float slowDown = 1f;
}
