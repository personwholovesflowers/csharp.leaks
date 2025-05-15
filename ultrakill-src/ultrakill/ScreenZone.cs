using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020003CE RID: 974
public class ScreenZone : MonoBehaviour
{
	// Token: 0x0600161C RID: 5660 RVA: 0x000B2A65 File Offset: 0x000B0C65
	private void Awake()
	{
		if (this.music)
		{
			this.originalVolume = this.music.volume;
		}
		if (this.jingleMusic)
		{
			this.originalJingleVolume = this.jingleMusic.volume;
		}
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x000B2AA4 File Offset: 0x000B0CA4
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		if (this.inZone)
		{
			this.UpdatePlayerState(false);
		}
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x000B2AD8 File Offset: 0x000B0CD8
	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}
		if (this.gc == null)
		{
			this.gc = other.GetComponentInChildren<GunControl>();
		}
		if (this.pun == null)
		{
			this.pun = other.GetComponentInChildren<FistControl>();
		}
		this.inZone = true;
		if (this.raycaster == null)
		{
			this.raycaster = base.GetComponentInChildren<GraphicRaycaster>(true);
		}
		if (this.raycaster == null)
		{
			this.raycaster = base.transform.parent.GetComponentInChildren<GraphicRaycaster>(true);
		}
		if (this.raycaster)
		{
			if (ControllerPointer.raycasters.Contains(this.raycaster))
			{
				ControllerPointer.raycasters.Remove(this.raycaster);
			}
			ControllerPointer.raycasters.Add(this.raycaster);
		}
		UnityEvent unityEvent = this.onEnterZone;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this.PlayMusic();
		this.hasEntered = true;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x000B2BD4 File Offset: 0x000B0DD4
	private void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag("Player"))
		{
			return;
		}
		if (this.gc == null)
		{
			this.gc = other.GetComponentInChildren<GunControl>();
		}
		if (this.raycaster && ControllerPointer.raycasters.Contains(this.raycaster))
		{
			ControllerPointer.raycasters.Remove(this.raycaster);
		}
		UnityEvent unityEvent = this.onExitZone;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		this.inZone = false;
		this.UpdatePlayerState(false);
		this.StopMusic();
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x000B2C64 File Offset: 0x000B0E64
	public virtual void UpdatePlayerState(bool active)
	{
		if (this.touchMode == active)
		{
			return;
		}
		if (active)
		{
			if (this.gc != null)
			{
				this.gc.NoWeapon();
			}
			if (this.pun != null)
			{
				this.pun.ShopMode();
			}
		}
		else
		{
			if (this.gc != null)
			{
				this.gc.YesWeapon();
			}
			if (this.pun != null)
			{
				this.pun.StopShop();
			}
		}
		this.touchMode = active;
	}

	// Token: 0x06001621 RID: 5665 RVA: 0x000B2CEC File Offset: 0x000B0EEC
	protected virtual void Update()
	{
		if (this.jingleMusic != null)
		{
			if (MonoSingleton<AudioMixerController>.Instance.optionsMusicVolume == 0f)
			{
				this.jingleMusic.volume = 0f;
			}
			else
			{
				this.jingleMusic.volume = this.originalJingleVolume;
			}
		}
		if (this.music != null)
		{
			if (MonoSingleton<AudioMixerController>.Instance.optionsMusicVolume == 0f)
			{
				this.music.volume = 0f;
			}
			else
			{
				this.music.volume = this.originalVolume;
			}
		}
		if (!this.inZone)
		{
			return;
		}
		float num = Vector3.Angle(MonoSingleton<CameraController>.Instance.transform.forward, (this.angleSourceOverride == null) ? base.transform.forward : this.angleSourceOverride.forward);
		this.UpdatePlayerState(this.angleLimit == 0f || num <= this.angleLimit);
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x000B2DE3 File Offset: 0x000B0FE3
	private void PlayMusic()
	{
		if (this.musicRoutine != null)
		{
			base.StopCoroutine(this.musicRoutine);
		}
		this.musicRoutine = base.StartCoroutine(this.PlayMusicRoutine());
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x000B2E0B File Offset: 0x000B100B
	private void StopMusic()
	{
		if (this.musicRoutine != null)
		{
			base.StopCoroutine(this.musicRoutine);
		}
		this.musicRoutine = base.StartCoroutine(this.StopMusicRoutine());
	}

	// Token: 0x06001624 RID: 5668 RVA: 0x000B2E33 File Offset: 0x000B1033
	private IEnumerator PlayMusicRoutine()
	{
		if (!this.muteMusic)
		{
			yield break;
		}
		if (this.jingleMusic != null && !this.hasEntered)
		{
			this.jingleMusic.Play();
			yield return new WaitUntil(() => this.jingleMusic.time >= this.jingleEndTime);
		}
		if (this.music != null)
		{
			if (!this.music.isPlaying)
			{
				this.music.Play();
			}
			while (this.music.pitch < 1f)
			{
				this.music.pitch = Mathf.MoveTowards(this.music.pitch, 1f, Time.deltaTime);
				yield return null;
			}
			this.music.pitch = 1f;
		}
		this.musicRoutine = null;
		yield break;
	}

	// Token: 0x06001625 RID: 5669 RVA: 0x000B2E42 File Offset: 0x000B1042
	private IEnumerator StopMusicRoutine()
	{
		if (!this.muteMusic)
		{
			yield break;
		}
		if (this.jingleMusic != null)
		{
			this.jingleMusic.Stop();
		}
		if (this.music != null)
		{
			while (this.music.pitch > 0f)
			{
				this.music.pitch = Mathf.MoveTowards(this.music.pitch, 0f, Time.deltaTime);
				yield return null;
			}
			this.music.pitch = 0f;
		}
		this.musicRoutine = null;
		yield break;
	}

	// Token: 0x04001E6B RID: 7787
	protected bool inZone;

	// Token: 0x04001E6C RID: 7788
	protected bool touchMode;

	// Token: 0x04001E6D RID: 7789
	private GunControl gc;

	// Token: 0x04001E6E RID: 7790
	private FistControl pun;

	// Token: 0x04001E6F RID: 7791
	[SerializeField]
	private AudioSource music;

	// Token: 0x04001E70 RID: 7792
	private float originalVolume;

	// Token: 0x04001E71 RID: 7793
	[SerializeField]
	private AudioSource jingleMusic;

	// Token: 0x04001E72 RID: 7794
	[SerializeField]
	private float jingleEndTime = 1f;

	// Token: 0x04001E73 RID: 7795
	private float originalJingleVolume;

	// Token: 0x04001E74 RID: 7796
	public bool muteMusic;

	// Token: 0x04001E75 RID: 7797
	[SerializeField]
	private float angleLimit;

	// Token: 0x04001E76 RID: 7798
	[SerializeField]
	private Transform angleSourceOverride;

	// Token: 0x04001E77 RID: 7799
	[Space]
	[SerializeField]
	protected UnityEvent onEnterZone = new UnityEvent();

	// Token: 0x04001E78 RID: 7800
	[SerializeField]
	protected UnityEvent onExitZone = new UnityEvent();

	// Token: 0x04001E79 RID: 7801
	protected GraphicRaycaster raycaster;

	// Token: 0x04001E7A RID: 7802
	private bool hasEntered;

	// Token: 0x04001E7B RID: 7803
	private Coroutine musicRoutine;
}
