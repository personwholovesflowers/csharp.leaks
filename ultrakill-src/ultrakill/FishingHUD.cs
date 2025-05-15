using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fishing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020001E3 RID: 483
public class FishingHUD : MonoSingleton<FishingHUD>
{
	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00043CAC File Offset: 0x00041EAC
	private float containerHeight
	{
		get
		{
			return this.struggleNub.rectTransform.parent.GetComponent<RectTransform>().rect.height;
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00043CDC File Offset: 0x00041EDC
	private void Start()
	{
		this.fishIconTemplate.gameObject.SetActive(false);
		foreach (KeyValuePair<FishObject, bool> keyValuePair in MonoSingleton<FishManager>.Instance.recognizedFishes)
		{
			Image image = Object.Instantiate<Image>(this.fishIconTemplate, this.fishIconContainer, false);
			image.gameObject.SetActive(true);
			image.sprite = keyValuePair.Key.blockedIcon;
			image.color = Color.black;
			this.fishHudIcons.Add(keyValuePair.Key, image);
			Image component = image.GetComponentInChildren<FishIconGlow>().GetComponent<Image>();
			component.sprite = keyValuePair.Key.blockedIcon;
			component.color = new Color(1f, 1f, 1f, 0f);
		}
		this.fishIconContainer.gameObject.SetActive(false);
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00043DE0 File Offset: 0x00041FE0
	public void ShowHUD()
	{
		if (this.hudDisabled)
		{
			return;
		}
		this.fishIconContainer.gameObject.SetActive(true);
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00043DFC File Offset: 0x00041FFC
	public void DisableHUD()
	{
		this.fishIconContainer.gameObject.SetActive(false);
		this.hudDisabled = true;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00043E16 File Offset: 0x00042016
	public void SetFishHooked(bool hooked)
	{
		this.hookedContainer.SetActive(hooked);
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00043E24 File Offset: 0x00042024
	private void OnFishUnlocked(FishObject obj)
	{
		this.fishHudIcons[obj].sprite = obj.icon;
		this.fishHudIcons[obj].color = Color.white;
		this.fishHudIcons[obj].GetComponentInChildren<FishIconGlow>().Blink();
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00043E74 File Offset: 0x00042074
	protected override void OnEnable()
	{
		base.OnEnable();
		FishManager instance = MonoSingleton<FishManager>.Instance;
		instance.onFishUnlocked = (Action<FishObject>)Delegate.Combine(instance.onFishUnlocked, new Action<FishObject>(this.OnFishUnlocked));
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00043EA2 File Offset: 0x000420A2
	private void OnDisable()
	{
		if (!MonoSingleton<FishManager>.Instance)
		{
			return;
		}
		FishManager instance = MonoSingleton<FishManager>.Instance;
		instance.onFishUnlocked = (Action<FishObject>)Delegate.Remove(instance.onFishUnlocked, new Action<FishObject>(this.OnFishUnlocked));
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00043ED8 File Offset: 0x000420D8
	public void SetState(FishingRodState state)
	{
		if (!this.struggleContainer.activeSelf && state == FishingRodState.FishStruggle)
		{
			this.outOfWaterMessage.SetActive(false);
		}
		this.powerMeterContainer.SetActive(state == FishingRodState.SelectingPower || state == FishingRodState.Throwing);
		this.struggleContainer.SetActive(state == FishingRodState.FishStruggle);
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00043F27 File Offset: 0x00042127
	public void SetPowerMeter(float value, bool canFish)
	{
		this.powerMeter.value = value;
		this.powerMeter.targetGraphic.color = (canFish ? Color.white : Color.red);
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00043F54 File Offset: 0x00042154
	private void Update()
	{
		float num = Mathf.Sin(this.struggleProgressSlider.value * 20f);
		this.fishIcon.localRotation = Quaternion.Euler(0f, 0f, num * 10f);
		if (this.struggleContainer.activeSelf)
		{
			Color color = Color.Lerp(FishingHUD.orangeColor, Color.white, this.timeSinceLMBReleased * 4f);
			Color color2 = Color.Lerp(FishingHUD.orangeColor, Color.white, this.timeSinceRMBReleased * 4f);
			this.struggleLMB.color = color;
			this.struggleRMB.color = color2;
			this.upArrow.color = color2;
			this.downArrow.color = color;
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00044018 File Offset: 0x00042218
	public void ShowFishCaught(bool show = true, FishObject fish = null)
	{
		if (!show)
		{
			base.StopAllCoroutines();
		}
		else
		{
			this.timeSinceFishCaught = 0f;
		}
		this.fishSizeContainer.SetActive(false);
		this.fishCaughtContainer.SetActive(show);
		if (show && fish != null)
		{
			this.fishCaughtText.text = "<size=28>You caught</size> <color=orange>" + fish.fishName + "</color>";
		}
		foreach (object obj in this.fishRenderContainer.transform)
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		if (show && fish != null)
		{
			this.amountOfCatches++;
			GameObject gameObject = fish.InstantiateDumb();
			gameObject.transform.SetParent(this.fishRenderContainer.transform);
			gameObject.transform.localPosition = Vector3.zero;
			SandboxUtils.SetLayerDeep(gameObject.transform, LayerMask.NameToLayer("VirtualRender"));
			gameObject.transform.localScale *= fish.previewSizeMulti;
			this.audioSource.clip = this.snareroll;
			this.audioSource.Play();
			base.StartCoroutine(this.ShowSize());
		}
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0004417C File Offset: 0x0004237C
	public void ShowOutOfWater()
	{
		this.outOfWaterMessage.SetActive(true);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0004418C File Offset: 0x0004238C
	public void SetStruggleProgress(float progress, Sprite fishIconLocked, Sprite fishIconUnlocked)
	{
		this.struggleProgressSlider.value = progress;
		this.struggleProgressIcon.sprite = fishIconUnlocked;
		this.struggleProgressIconOverlay.sprite = fishIconLocked;
		Color color = this.struggleProgressIconOverlay.color;
		color.a = 1f - progress;
		this.struggleProgressIconOverlay.color = color;
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x000441E3 File Offset: 0x000423E3
	public void SetStruggleSatisfied(bool satisfied)
	{
		this.struggleNub.color = (satisfied ? Color.green : Color.white);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00044200 File Offset: 0x00042400
	public void SetPlayerStrugglePosition(float pos)
	{
		this.struggleNub.rectTransform.anchoredPosition = new Vector2(0f, -pos * this.containerHeight);
		if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad)
		{
			this.struggleLMB.text = MonoSingleton<InputManager>.Instance.InputSource.Fire1.Action.bindings.First<InputBinding>().ToDisplayString((InputBinding.DisplayStringOptions)0, null);
			this.struggleRMB.text = MonoSingleton<InputManager>.Instance.InputSource.Fire2.Action.bindings.First<InputBinding>().ToDisplayString((InputBinding.DisplayStringOptions)0, null);
		}
		else
		{
			this.struggleLMB.text = "LMB";
			this.struggleRMB.text = "RMB";
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			this.struggleLMB.color = new Color(1f, 0.5f, 0.1f);
			this.downArrow.color = new Color(1f, 0.5f, 0.1f);
			this.timeSinceLMBReleased = 0f;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed)
		{
			this.struggleRMB.color = new Color(1f, 0.5f, 0.1f);
			this.upArrow.color = new Color(1f, 0.5f, 0.1f);
			this.timeSinceRMBReleased = 0f;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0004439C File Offset: 0x0004259C
	public void SetFishDesire(float top, float bottom)
	{
		this.desireBar.offsetMin = new Vector2(this.desireBar.offsetMin.x, (1f - bottom) * this.containerHeight);
		this.desireBar.offsetMax = new Vector2(this.desireBar.offsetMax.x, -top * this.containerHeight);
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00044400 File Offset: 0x00042600
	private IEnumerator ShowSize()
	{
		if (this.longWaits == 2 && Random.Range(0f, 1f) > 0.75f)
		{
			this.longWaits++;
			yield return new WaitForSeconds(7.5f);
			this.ShowFishCaught(false, null);
			MonoSingleton<FishManager>.Instance.UpdateFishCount();
		}
		else
		{
			yield return new WaitForSeconds(this.RandomizeWaitTime());
			this.fishSizeContainer.SetActive(true);
			this.audioSource.clip = this.snarehit;
			this.audioSource.Play();
			MonoSingleton<FishManager>.Instance.UpdateFishCount();
		}
		yield break;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x0004440F File Offset: 0x0004260F
	private IEnumerator AutoDismissFishCaught(float time)
	{
		yield return new WaitForSeconds(time);
		this.ShowFishCaught(false, null);
		yield break;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00044428 File Offset: 0x00042628
	private float RandomizeWaitTime()
	{
		float num = Random.Range(0f, 1f);
		if (this.amountOfCatches < 3 || num <= 0.66f)
		{
			base.StartCoroutine(this.AutoDismissFishCaught(4f));
			return 1f;
		}
		float num2 = Random.Range(2f, 6f);
		base.StartCoroutine(this.AutoDismissFishCaught(num2 + 3f));
		if (num2 >= 4.5f && this.longWaits != 2)
		{
			this.longWaits++;
		}
		return num2;
	}

	// Token: 0x04000CBA RID: 3258
	[SerializeField]
	private GameObject powerMeterContainer;

	// Token: 0x04000CBB RID: 3259
	[SerializeField]
	private Slider powerMeter;

	// Token: 0x04000CBC RID: 3260
	[SerializeField]
	private GameObject hookedContainer;

	// Token: 0x04000CBD RID: 3261
	[Space]
	[SerializeField]
	private GameObject fishCaughtContainer;

	// Token: 0x04000CBE RID: 3262
	[SerializeField]
	private Text fishCaughtText;

	// Token: 0x04000CBF RID: 3263
	[SerializeField]
	private GameObject fishRenderContainer;

	// Token: 0x04000CC0 RID: 3264
	[SerializeField]
	private GameObject fishSizeContainer;

	// Token: 0x04000CC1 RID: 3265
	[Space]
	[SerializeField]
	private GameObject struggleContainer;

	// Token: 0x04000CC2 RID: 3266
	[SerializeField]
	private GameObject outOfWaterMessage;

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	private Image struggleProgressIcon;

	// Token: 0x04000CC4 RID: 3268
	[SerializeField]
	private Image struggleProgressIconOverlay;

	// Token: 0x04000CC5 RID: 3269
	[SerializeField]
	private Image struggleNub;

	// Token: 0x04000CC6 RID: 3270
	[SerializeField]
	private RectTransform desireBar;

	// Token: 0x04000CC7 RID: 3271
	[SerializeField]
	private RectTransform fishIcon;

	// Token: 0x04000CC8 RID: 3272
	[SerializeField]
	private Slider struggleProgressSlider;

	// Token: 0x04000CC9 RID: 3273
	[SerializeField]
	private Text struggleLMB;

	// Token: 0x04000CCA RID: 3274
	[SerializeField]
	private Text struggleRMB;

	// Token: 0x04000CCB RID: 3275
	[SerializeField]
	private Image upArrow;

	// Token: 0x04000CCC RID: 3276
	[SerializeField]
	private Image downArrow;

	// Token: 0x04000CCD RID: 3277
	[Space]
	[SerializeField]
	private Image fishIconTemplate;

	// Token: 0x04000CCE RID: 3278
	[SerializeField]
	private Transform fishIconContainer;

	// Token: 0x04000CCF RID: 3279
	private Dictionary<FishObject, Image> fishHudIcons = new Dictionary<FishObject, Image>();

	// Token: 0x04000CD0 RID: 3280
	private static Color orangeColor = new Color(1f, 0.5f, 0.1f);

	// Token: 0x04000CD1 RID: 3281
	private TimeSince timeSinceLMBReleased;

	// Token: 0x04000CD2 RID: 3282
	private TimeSince timeSinceRMBReleased;

	// Token: 0x04000CD3 RID: 3283
	[HideInInspector]
	public TimeSince timeSinceFishCaught;

	// Token: 0x04000CD4 RID: 3284
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x04000CD5 RID: 3285
	[SerializeField]
	private AudioClip snareroll;

	// Token: 0x04000CD6 RID: 3286
	[SerializeField]
	private AudioClip snarehit;

	// Token: 0x04000CD7 RID: 3287
	private int amountOfCatches;

	// Token: 0x04000CD8 RID: 3288
	private int longWaits;

	// Token: 0x04000CD9 RID: 3289
	private bool hudDisabled;
}
