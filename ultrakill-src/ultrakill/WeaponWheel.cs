using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x020004C9 RID: 1225
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class WeaponWheel : MonoSingleton<WeaponWheel>
{
	// Token: 0x06001C07 RID: 7175 RVA: 0x000E89E5 File Offset: 0x000E6BE5
	private void Start()
	{
		base.gameObject.SetActive(false);
		this.background.SetActive(true);
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x000E8A00 File Offset: 0x000E6C00
	private new void OnEnable()
	{
		if (MonoSingleton<InputManager>.Instance == null)
		{
			return;
		}
		Time.timeScale = 0.25f;
		MonoSingleton<TimeController>.Instance.timeScaleModifier = 0.25f;
		this.selectedSegment = -1;
		this.direction = Vector2.zero;
		GameStateManager.Instance.RegisterState(new GameState("weapon-wheel", base.gameObject)
		{
			timerModifier = new float?(4f),
			cameraInputLock = LockMode.Lock
		});
	}

	// Token: 0x06001C09 RID: 7177 RVA: 0x000E8A77 File Offset: 0x000E6C77
	private void OnDisable()
	{
		if (MonoSingleton<TimeController>.Instance)
		{
			MonoSingleton<TimeController>.Instance.timeScaleModifier = 1f;
			MonoSingleton<TimeController>.Instance.RestoreTime();
		}
		if (MonoSingleton<FistControl>.Instance)
		{
			MonoSingleton<FistControl>.Instance.RefreshArm();
		}
	}

	// Token: 0x06001C0A RID: 7178 RVA: 0x000E8AB4 File Offset: 0x000E6CB4
	private void Update()
	{
		if (!MonoSingleton<GunControl>.Instance || !MonoSingleton<GunControl>.Instance.activated || MonoSingleton<OptionsManager>.Instance.paused || MonoSingleton<NewMovement>.Instance.dead || GameStateManager.Instance.PlayerInputLocked)
		{
			base.gameObject.SetActive(false);
			return;
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.WasCanceledThisFrame || MonoSingleton<InputManager>.Instance.InputSource.PrevWeapon.WasCanceledThisFrame || MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.WasCanceledThisFrame || MonoSingleton<InputManager>.Instance.InputSource.PreviousVariation.WasCanceledThisFrame)
		{
			if (this.selectedSegment != -1)
			{
				int num = this.segments[this.selectedSegment].slotIndex + 1;
				MonoSingleton<GunControl>.Instance.SwitchWeapon(num, null, false, false, false);
			}
			base.gameObject.SetActive(false);
			return;
		}
		if (this.segments == null || this.segments.Count == 0)
		{
			return;
		}
		this.direction = Vector2.ClampMagnitude(this.direction + MonoSingleton<InputManager>.Instance.InputSource.WheelLook.ReadValue<Vector2>(), 1f);
		float num2 = Mathf.Repeat(Mathf.Atan2(this.direction.x, this.direction.y) * 57.29578f + 90f, 360f);
		if (Mathf.Approximately(num2, 360f))
		{
			num2 = 0f;
		}
		this.selectedSegment = ((this.direction.sqrMagnitude > 0f) ? ((int)(num2 / (360f / (float)this.segmentCount))) : this.selectedSegment);
		for (int i = 0; i < this.segments.Count; i++)
		{
			if (i == this.selectedSegment)
			{
				this.segments[i].SetActive(true);
			}
			else
			{
				this.segments[i].SetActive(false);
			}
		}
		if (this.selectedSegment != this.lastSelectedSegment)
		{
			Object.Instantiate<GameObject>(this.clickSound);
			this.lastSelectedSegment = this.selectedSegment;
			if (MonoSingleton<RumbleManager>.Instance)
			{
				MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.WeaponWheelTick);
			}
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x000E8CE9 File Offset: 0x000E6EE9
	public void Show()
	{
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.lastSelectedSegment = -1;
		base.gameObject.SetActive(true);
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x000E8D0C File Offset: 0x000E6F0C
	public void SetSegments(WeaponDescriptor[] weaponDescriptors, int[] slotIndexes)
	{
		int num = weaponDescriptors.Length;
		if (num == this.segmentCount)
		{
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				if (!(this.segments[i].descriptor == weaponDescriptors[i]) || this.segments[i].slotIndex != slotIndexes[i])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		this.segmentCount = num;
		this.lastSelectedSegment = -1;
		if (this.segments == null)
		{
			this.segments = new List<WheelSegment>(this.segmentCount);
		}
		foreach (WheelSegment wheelSegment in this.segments)
		{
			wheelSegment.DestroySegment();
		}
		this.segments.Clear();
		for (int j = 0; j < this.segmentCount; j++)
		{
			UICircle uicircle = new GameObject().AddComponent<UICircle>();
			uicircle.name = "Segment " + j.ToString();
			uicircle.Arc = 1f / (float)this.segmentCount - 0.005f;
			uicircle.ArcRotation = (int)(360f * ((float)j / (float)this.segmentCount) + 1.8f);
			uicircle.Fill = false;
			uicircle.transform.SetParent(base.transform, false);
			uicircle.rectTransform.anchorMin = Vector2.zero;
			uicircle.rectTransform.anchorMax = Vector2.one;
			uicircle.rectTransform.anchoredPosition = Vector2.zero;
			uicircle.rectTransform.sizeDelta = Vector2.zero;
			Outline outline = uicircle.gameObject.AddComponent<Outline>();
			outline.effectDistance = new Vector2(2f, -2f);
			outline.effectColor = Color.white;
			UICircle uicircle2 = new GameObject().AddComponent<UICircle>();
			uicircle2.name = "Segment Divider " + j.ToString();
			uicircle2.Arc = 0.005f;
			uicircle2.ArcRotation = (int)(360f * ((float)j / (float)this.segmentCount) + 1.8f - 0.9f);
			uicircle2.Fill = false;
			uicircle2.transform.SetParent(base.transform, false);
			uicircle2.rectTransform.anchorMin = Vector2.zero;
			uicircle2.rectTransform.anchorMax = Vector2.one;
			uicircle2.rectTransform.sizeDelta = new Vector2(256f, 256f);
			uicircle2.Thickness = 128f;
			Image image = new GameObject().AddComponent<Image>();
			image.name = "Icon " + j.ToString();
			image.sprite = weaponDescriptors[j].icon;
			image.transform.SetParent(uicircle.transform, false);
			float num2 = (float)j * 360f / (float)this.segmentCount;
			float num3 = uicircle.Arc * 360f / 2f;
			float num4 = num2 + num3;
			float num5 = num4 * 0.017453292f;
			float num6 = 112f;
			Vector2 vector = new Vector2(-Mathf.Cos(num5), Mathf.Sin(num5)) * num6;
			image.transform.localPosition = vector;
			float num7 = num4 + 180f;
			image.transform.localRotation = Quaternion.Euler(0f, 0f, -num7);
			Vector2 size = image.sprite.rect.size;
			image.rectTransform.sizeDelta = new Vector2(size.x, size.y) * 0.12f;
			Image image2 = new GameObject().AddComponent<Image>();
			image2.name = "Icon Outline " + j.ToString();
			image2.sprite = weaponDescriptors[j].glowIcon;
			image2.transform.SetParent(uicircle.transform, false);
			image2.transform.localPosition = image.transform.localPosition;
			image2.transform.localRotation = image.transform.localRotation;
			image2.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
			image2.transform.SetAsFirstSibling();
			WheelSegment wheelSegment2 = new WheelSegment
			{
				segment = uicircle,
				icon = image,
				iconGlow = image2,
				descriptor = weaponDescriptors[j],
				divider = uicircle2,
				slotIndex = slotIndexes[j]
			};
			this.segments.Add(wheelSegment2);
			wheelSegment2.SetActive(false);
		}
	}

	// Token: 0x0400278E RID: 10126
	private List<WheelSegment> segments;

	// Token: 0x0400278F RID: 10127
	public int segmentCount;

	// Token: 0x04002790 RID: 10128
	public GameObject clickSound;

	// Token: 0x04002791 RID: 10129
	public GameObject background;

	// Token: 0x04002792 RID: 10130
	private int selectedSegment;

	// Token: 0x04002793 RID: 10131
	private int lastSelectedSegment;

	// Token: 0x04002794 RID: 10132
	private Vector2 direction;
}
