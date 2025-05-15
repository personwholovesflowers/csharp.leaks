using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020003F8 RID: 1016
public class ShopZone : ScreenZone
{
	// Token: 0x060016C7 RID: 5831 RVA: 0x000B6630 File Offset: 0x000B4830
	private void Start()
	{
		this.shopCanvas = base.GetComponentInChildren<Canvas>(true);
		if (this.shopCanvas != null)
		{
			this.shopCanvas.gameObject.SetActive(false);
		}
		this.originalMusicVolume = MonoSingleton<AudioMixerController>.Instance.optionsMusicVolume;
		this.musicTarget = this.originalMusicVolume;
		if (this.tipOfTheDay != null)
		{
			MapInfoBase instance = MapInfoBase.Instance;
			if (instance != null && instance.tipOfTheDay != null)
			{
				this.tipOfTheDay.text = instance.tipOfTheDay.tip;
			}
		}
		MonoSingleton<CheckPointsController>.Instance.AddShop(this);
		this.onEnterZone.AddListener(new UnityAction(this.TurnOn));
		this.onExitZone.AddListener(new UnityAction(this.TurnOff));
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x000B6700 File Offset: 0x000B4900
	protected override void Update()
	{
		base.Update();
		if (this.muteMusic && this.fading && MonoSingleton<AudioMixerController>.Instance.musicVolume != this.musicTarget)
		{
			MonoSingleton<AudioMixerController>.Instance.SetMusicVolume(Mathf.MoveTowards(MonoSingleton<AudioMixerController>.Instance.musicVolume, Mathf.Min(this.musicTarget, MonoSingleton<AudioMixerController>.Instance.optionsMusicVolume), this.originalMusicVolume * Time.deltaTime));
			if (MonoSingleton<AudioMixerController>.Instance.musicVolume == this.musicTarget)
			{
				this.fading = false;
			}
		}
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x000B6788 File Offset: 0x000B4988
	public void TurnOn()
	{
		if (!this.inUse && !this.forcedOff)
		{
			this.inUse = true;
			if (this.shopCanvas != null)
			{
				this.shopCanvas.gameObject.SetActive(true);
			}
			if (this.shopcats == null)
			{
				this.shopcats = base.GetComponentsInChildren<ShopCategory>();
			}
			ShopCategory[] array = this.shopcats;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].CheckGear();
			}
			if (this.raycaster = null)
			{
				this.raycaster = this.shopCanvas.GetComponent<GraphicRaycaster>();
			}
			if (ControllerPointer.raycasters.Contains(this.raycaster))
			{
				ControllerPointer.raycasters.Remove(this.raycaster);
			}
			ControllerPointer.raycasters.Add(this.raycaster);
			if (this.muteMusic)
			{
				this.fading = true;
				this.musicTarget = 0f;
			}
		}
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x000B6874 File Offset: 0x000B4A74
	public void TurnOff()
	{
		if (this.inUse)
		{
			if (this.shopCanvas != null)
			{
				this.shopCanvas.gameObject.SetActive(false);
			}
			this.inUse = false;
			this.shopCanvas.gameObject.SetActive(false);
			if (ControllerPointer.raycasters.Contains(this.raycaster))
			{
				ControllerPointer.raycasters.Remove(this.raycaster);
			}
			if (this.firstVariationBuy)
			{
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Cycle through <color=orange>EQUIPPED</color> variations with '<color=orange>", "ChangeVariation", "</color>'.", 0, false, false, true);
				this.firstVariationBuy = false;
				PlayerPrefs.SetInt("FirVar", 0);
			}
			if (this.muteMusic)
			{
				this.fading = true;
				this.musicTarget = this.originalMusicVolume;
			}
		}
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x000B6938 File Offset: 0x000B4B38
	public void ForceOff()
	{
		this.TurnOff();
		this.forcedOff = true;
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x000B6947 File Offset: 0x000B4B47
	public void StopForceOff()
	{
		this.forcedOff = false;
		if (this.inZone)
		{
			this.TurnOn();
		}
	}

	// Token: 0x04001F88 RID: 8072
	private bool inUse;

	// Token: 0x04001F89 RID: 8073
	private Canvas shopCanvas;

	// Token: 0x04001F8A RID: 8074
	public bool firstVariationBuy;

	// Token: 0x04001F8B RID: 8075
	private ShopMother shom;

	// Token: 0x04001F8C RID: 8076
	private ShopCategory[] shopcats;

	// Token: 0x04001F8D RID: 8077
	private float originalMusicVolume;

	// Token: 0x04001F8E RID: 8078
	private float musicTarget = 1f;

	// Token: 0x04001F8F RID: 8079
	private bool fading;

	// Token: 0x04001F90 RID: 8080
	public bool forcedOff;

	// Token: 0x04001F91 RID: 8081
	public TMP_Text tipOfTheDay;
}
