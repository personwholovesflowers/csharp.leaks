using System;
using System.Collections.Generic;
using System.Linq;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x02000240 RID: 576
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class GunControl : MonoSingleton<GunControl>
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000C62 RID: 3170 RVA: 0x000588F0 File Offset: 0x00056AF0
	// (remove) Token: 0x06000C63 RID: 3171 RVA: 0x00058928 File Offset: 0x00056B28
	public event Action<GameObject> OnWeaponChange;

	// Token: 0x06000C64 RID: 3172 RVA: 0x00058960 File Offset: 0x00056B60
	private void Start()
	{
		this.inman = MonoSingleton<InputManager>.Instance;
		this.currentVariationIndex = PlayerPrefs.GetInt("CurVar", 0);
		this.currentSlotIndex = PlayerPrefs.GetInt("CurSlo", 1);
		this.lastVariationIndex = PlayerPrefs.GetInt("LasVar", 69);
		this.lastSlotIndex = PlayerPrefs.GetInt("LasSlo", 69);
		Debug.Log(string.Format("Last Slot is {0}", this.lastSlotIndex));
		this.aud = base.GetComponent<AudioSource>();
		this.variationMemory = MonoSingleton<PrefsManager>.Instance.GetBool("variationMemory", false);
		this.slots.Add(this.slot1);
		this.slots.Add(this.slot2);
		this.slots.Add(this.slot3);
		this.slots.Add(this.slot4);
		this.slots.Add(this.slot5);
		this.slots.Add(this.slot6);
		if (this.currentSlotIndex > this.slots.Count)
		{
			this.currentSlotIndex = 1;
		}
		int num = 0;
		foreach (List<GameObject> list in this.slots)
		{
			foreach (GameObject gameObject in list)
			{
				if (gameObject != null)
				{
					this.allWeapons.Add(gameObject);
					this.slotDict.Add(gameObject, num);
				}
			}
			if (list.Count != 0)
			{
				this.noWeapons = false;
			}
			num++;
		}
		if (this.currentWeapon == null && this.slots[this.currentSlotIndex - 1].Count > this.currentVariationIndex)
		{
			this.currentWeapon = this.slots[this.currentSlotIndex - 1][this.currentVariationIndex];
		}
		else if (this.currentWeapon == null && this.slot1.Count != 0)
		{
			this.currentSlotIndex = 1;
			this.currentVariationIndex = 0;
			this.currentWeapon = this.slot1[0];
		}
		this.shud = MonoSingleton<StyleHUD>.Instance;
		this.UpdateWeaponList(true);
		for (int i = 0; i < this.slots.Count; i++)
		{
			int @int = PlayerPrefs.GetInt("Slot" + i.ToString() + "Var", -1);
			if (@int >= 0 && @int < this.slots[i].Count)
			{
				this.retainedVariations.Add(i, @int);
			}
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x00058C30 File Offset: 0x00056E30
	protected override void OnDestroy()
	{
		foreach (KeyValuePair<int, int> keyValuePair in this.retainedVariations)
		{
			PlayerPrefs.SetInt("Slot" + keyValuePair.Key.ToString() + "Var", keyValuePair.Value);
		}
		base.OnDestroy();
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x00058CAC File Offset: 0x00056EAC
	protected override void OnEnable()
	{
		base.OnEnable();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00058CD4 File Offset: 0x00056ED4
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00058CF8 File Offset: 0x00056EF8
	private void OnPrefChanged(string id, object value)
	{
		if (id == "variationMemory" && value is bool)
		{
			bool flag = (bool)value;
			this.variationMemory = flag;
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00058D28 File Offset: 0x00056F28
	private void CalculateSlotCount()
	{
		List<WeaponDescriptor> list = new List<WeaponDescriptor>();
		List<int> list2 = new List<int>();
		foreach (List<GameObject> list3 in this.slots)
		{
			GameObject gameObject = list3.FirstOrDefault<GameObject>();
			if (!(gameObject == null))
			{
				WeaponIcon component = gameObject.GetComponent<WeaponIcon>();
				if (component != null)
				{
					list.Add(component.weaponDescriptor);
				}
				list2.Add(this.slots.IndexOf(list3));
			}
		}
		MonoSingleton<WeaponWheel>.Instance.SetSegments(list.ToArray(), list2.ToArray());
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00058DDC File Offset: 0x00056FDC
	private void Update()
	{
		if (this.activated && !GameStateManager.Instance.PlayerInputLocked)
		{
			global::PlayerInput inputSource = this.inman.InputSource;
			if (this.headShotComboTime > 0f)
			{
				this.headShotComboTime = Mathf.MoveTowards(this.headShotComboTime, 0f, Time.deltaTime);
			}
			else
			{
				this.headshots = 0;
			}
			if (this.lastSlotIndex == 0)
			{
				this.lastSlotIndex = 69;
			}
			if (!MonoSingleton<OptionsManager>.Instance.inIntro && !MonoSingleton<OptionsManager>.Instance.paused && !MonoSingleton<NewMovement>.Instance.dead)
			{
				if (inputSource.NextWeapon.IsPressed && inputSource.PrevWeapon.IsPressed)
				{
					this.hookCombo = true;
					if (MonoSingleton<WeaponWheel>.Instance.gameObject.activeSelf)
					{
						MonoSingleton<WeaponWheel>.Instance.gameObject.SetActive(false);
					}
				}
				if (((inputSource.NextWeapon.IsPressed && inputSource.NextWeapon.HoldTime >= 0.25f && !inputSource.PrevWeapon.IsPressed) || (inputSource.PrevWeapon.IsPressed && inputSource.PrevWeapon.HoldTime >= 0.25f && !inputSource.NextWeapon.IsPressed) || (inputSource.LastWeapon.IsPressed && inputSource.LastWeapon.HoldTime >= 0.25f) || (inputSource.PreviousVariation.IsPressed && inputSource.PreviousVariation.HoldTime >= 0.25f)) && !this.hookCombo)
				{
					MonoSingleton<WeaponWheel>.Instance.Show();
				}
			}
			if (this.inman.InputSource.Slot1.WasPerformedThisFrame)
			{
				if (this.slot1.Count > 0 && this.slot1[0] != null && (this.slot1.Count > 1 || this.currentSlotIndex != 1))
				{
					this.SwitchWeapon(1, null, false, false, false);
				}
			}
			else if (this.inman.InputSource.Slot2.WasPerformedThisFrame)
			{
				if (this.slot2.Count > 0 && this.slot2[0] != null && (this.slot2.Count > 1 || this.currentSlotIndex != 2))
				{
					this.SwitchWeapon(2, null, false, false, false);
				}
			}
			else if (this.inman.InputSource.Slot3.WasPerformedThisFrame && (this.slot3.Count > 1 || this.currentSlotIndex != 3))
			{
				if (this.slot3.Count > 0 && this.slot3[0] != null)
				{
					this.SwitchWeapon(3, null, false, false, false);
				}
			}
			else if (this.inman.InputSource.Slot4.WasPerformedThisFrame && (this.slot4.Count > 1 || this.currentSlotIndex != 4))
			{
				if (this.slot4.Count > 0 && this.slot4[0] != null)
				{
					this.SwitchWeapon(4, null, false, false, false);
				}
			}
			else if (this.inman.InputSource.Slot5.WasPerformedThisFrame && (this.slot5.Count > 1 || this.currentSlotIndex != 5))
			{
				if (this.slot5.Count > 0 && this.slot5[0] != null)
				{
					this.SwitchWeapon(5, null, false, false, false);
				}
			}
			else if (this.inman.InputSource.Slot6.WasPerformedThisFrame && (this.slot6.Count > 1 || this.currentSlotIndex != 6))
			{
				if (this.slot6.Count > 0 && this.slot6[0] != null)
				{
					this.SwitchWeapon(6, null, true, false, false);
				}
			}
			else if (this.inman.InputSource.LastWeapon.WasCanceledThisFrame && this.inman.InputSource.LastWeapon.HoldTime < 0.25f && this.lastSlotIndex != 69)
			{
				if (this.slots[this.lastSlotIndex - 1] != null)
				{
					this.SwitchWeapon(this.lastSlotIndex, null, true, false, false);
				}
			}
			else if (this.inman.InputSource.NextVariation.WasPerformedThisFrame && this.slots[this.currentSlotIndex - 1].Count > 1)
			{
				this.SwitchWeapon(this.currentSlotIndex, new int?(this.currentVariationIndex + 1), false, false, true);
			}
			else if (this.inman.InputSource.PreviousVariation.WasCanceledThisFrame && inputSource.PreviousVariation.HoldTime < 0.25f && this.slots[this.currentSlotIndex - 1].Count > 1)
			{
				this.SwitchWeapon(this.currentSlotIndex, new int?(this.currentVariationIndex - 1), false, false, true);
			}
			else if (this.inman.InputSource.SelectVariant1.WasPerformedThisFrame)
			{
				this.SwitchWeapon(this.currentSlotIndex, new int?(0), false, false, false);
			}
			else if (this.inman.InputSource.SelectVariant2.WasPerformedThisFrame)
			{
				this.SwitchWeapon(this.currentSlotIndex, new int?(1), false, false, false);
			}
			else if (this.inman.InputSource.SelectVariant3.WasPerformedThisFrame)
			{
				this.SwitchWeapon(this.currentSlotIndex, new int?(2), false, false, false);
			}
			else if (!this.noWeapons)
			{
				float num = Mouse.current.scroll.ReadValue().y;
				if (this.inman.ScrRev)
				{
					num *= -1f;
				}
				if (inputSource.NextWeapon.HoldTime < 0.25f && !this.hookCombo && ((num > 0f && this.inman.ScrOn) || inputSource.NextWeapon.WasCanceledThisFrame) && this.scrollCooldown <= 0f)
				{
					int num2 = 0;
					if (this.inman.ScrWep && this.inman.ScrVar)
					{
						using (List<List<GameObject>>.Enumerator enumerator = this.slots.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Count > 0)
								{
									num2++;
								}
							}
						}
					}
					bool flag = false;
					if (this.inman.ScrVar)
					{
						if (this.slots[this.currentSlotIndex - 1].Count > this.currentVariationIndex + 1 || ((!this.inman.ScrWep || num2 <= 1) && this.slots[this.currentSlotIndex - 1].Count > 1))
						{
							this.SwitchWeapon(this.currentSlotIndex, new int?(this.currentVariationIndex + 1), false, false, true);
							this.scrollCooldown = 0.5f;
							flag = true;
						}
						else if (!this.inman.ScrWep)
						{
							flag = true;
						}
					}
					if (!flag && this.inman.ScrWep)
					{
						if (!flag && this.currentSlotIndex < this.slots.Count)
						{
							for (int i = this.currentSlotIndex; i < this.slots.Count; i++)
							{
								if (this.slots[i].Count > 0)
								{
									flag = true;
									this.SwitchWeapon(i + 1, null, false, true, false);
									this.scrollCooldown = 0.5f;
									break;
								}
							}
						}
						if (!flag)
						{
							int j = 0;
							while (j < this.currentSlotIndex)
							{
								if (this.slots[j].Count > 0)
								{
									if (j != this.currentSlotIndex - 1)
									{
										this.SwitchWeapon(j + 1, null, false, true, false);
										this.scrollCooldown = 0.5f;
										break;
									}
									break;
								}
								else
								{
									j++;
								}
							}
						}
					}
				}
				else if (inputSource.PrevWeapon.HoldTime < 0.25f && !this.hookCombo && ((num < 0f && this.inman.ScrOn) || inputSource.PrevWeapon.WasCanceledThisFrame) && this.scrollCooldown <= 0f)
				{
					int num3 = 0;
					if (this.inman.ScrWep && this.inman.ScrVar)
					{
						using (List<List<GameObject>>.Enumerator enumerator = this.slots.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Count > 0)
								{
									num3++;
								}
							}
						}
					}
					if ((this.inman.ScrWep && !this.inman.ScrVar) || (this.inman.ScrWep && num3 > 1))
					{
						if (this.inman.ScrVar)
						{
							if (this.currentVariationIndex != 0)
							{
								GameObject gameObject = this.slots[this.currentSlotIndex - 1][this.currentVariationIndex - 1];
								this.ForceWeapon(gameObject, true);
								this.scrollCooldown = 0.5f;
							}
							else if (this.currentSlotIndex == 1)
							{
								int k = this.slots.Count - 1;
								while (k >= 0)
								{
									if (this.slots[k].Count > 0)
									{
										if (k != this.currentSlotIndex - 1)
										{
											GameObject gameObject2 = this.slots[k][this.slots[k].Count - 1];
											this.ForceWeapon(gameObject2, true);
											this.scrollCooldown = 0.5f;
											break;
										}
										break;
									}
									else
									{
										k--;
									}
								}
							}
							else
							{
								bool flag2 = false;
								for (int l = this.currentSlotIndex - 2; l >= 0; l--)
								{
									if (this.slots[l].Count > 0)
									{
										GameObject gameObject3 = this.slots[l][this.slots[l].Count - 1];
										this.ForceWeapon(gameObject3, true);
										this.scrollCooldown = 0.5f;
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									int m = this.slots.Count - 1;
									while (m >= 0)
									{
										if (this.slots[m].Count > 0)
										{
											if (m != this.currentSlotIndex - 1)
											{
												GameObject gameObject4 = this.slots[m][this.slots[m].Count - 1];
												this.ForceWeapon(gameObject4, true);
												this.scrollCooldown = 0.5f;
												break;
											}
											break;
										}
										else
										{
											m--;
										}
									}
								}
							}
						}
						else if (this.currentSlotIndex == 1)
						{
							int n = this.slots.Count - 1;
							while (n >= 0)
							{
								if (this.slots[n].Count > 0)
								{
									if (n != this.currentSlotIndex - 1)
									{
										this.SwitchWeapon(n + 1, null, false, true, false);
										this.scrollCooldown = 0.5f;
										break;
									}
									break;
								}
								else
								{
									n--;
								}
							}
						}
						else
						{
							bool flag3 = false;
							for (int num4 = this.currentSlotIndex - 2; num4 >= 0; num4--)
							{
								if (this.slots[num4].Count > 0)
								{
									this.SwitchWeapon(num4 + 1, null, false, true, false);
									this.scrollCooldown = 0.5f;
									flag3 = true;
									break;
								}
							}
							if (!flag3)
							{
								int num5 = this.slots.Count - 1;
								while (num5 >= 0)
								{
									if (this.slots[num5].Count > 0)
									{
										if (num5 != this.currentSlotIndex - 1)
										{
											this.SwitchWeapon(num5 + 1, null, false, true, false);
											this.scrollCooldown = 0.5f;
											break;
										}
										break;
									}
									else
									{
										num5--;
									}
								}
							}
						}
					}
					else if (this.slots[this.currentSlotIndex - 1].Count > 1)
					{
						this.SwitchWeapon(this.currentSlotIndex, new int?(this.currentVariationIndex - 1), false, false, true);
						this.scrollCooldown = 0.5f;
					}
				}
			}
			if (this.hookCombo && !inputSource.NextWeapon.IsPressed && !inputSource.PrevWeapon.IsPressed)
			{
				this.hookCombo = false;
			}
		}
		if (this.scrollCooldown > 0f)
		{
			this.scrollCooldown = Mathf.MoveTowards(this.scrollCooldown, 0f, Time.deltaTime * 5f);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00059ADC File Offset: 0x00057CDC
	private void OnGUI()
	{
		if (!GunControlDebug.GunControlActivated)
		{
			return;
		}
		GUILayout.Label("Gun Control", Array.Empty<GUILayoutOption>());
		GUILayout.Label("Last Used Slot: " + this.lastSlotIndex.ToString(), Array.Empty<GUILayoutOption>());
		GUILayout.Label("Current Slot: " + this.currentSlotIndex.ToString(), Array.Empty<GUILayoutOption>());
		GUILayout.Label("Current Variation: " + this.currentVariationIndex.ToString(), Array.Empty<GUILayoutOption>());
		GUILayout.Space(12f);
		GUILayout.Label("Retained Variations:", Array.Empty<GUILayoutOption>());
		foreach (KeyValuePair<int, int> keyValuePair in this.retainedVariations)
		{
			GUILayout.Label(keyValuePair.Key.ToString() + ": " + keyValuePair.Value.ToString(), Array.Empty<GUILayoutOption>());
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00059BE8 File Offset: 0x00057DE8
	private void RetainVariation(int slot, int variationIndex)
	{
		if (this.retainedVariations.ContainsKey(slot))
		{
			this.retainedVariations[slot] = variationIndex;
			return;
		}
		this.retainedVariations.Add(slot, this.currentVariationIndex);
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00059C18 File Offset: 0x00057E18
	private int loop(int x, int m)
	{
		int num = x % m;
		if (num >= 0)
		{
			return num;
		}
		return num + m;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00059C34 File Offset: 0x00057E34
	public void SwitchWeapon(int targetSlotIndex, int? targetVariationIndex = null, bool useRetainedVariation = false, bool cycleSlot = false, bool cycleVariation = false)
	{
		if (this.slots.Count == 0)
		{
			Debug.LogWarning("Tried to switch weapon with no slots");
			return;
		}
		targetSlotIndex = Mathf.Clamp(targetSlotIndex, 1, this.slots.Count);
		if (cycleSlot)
		{
			targetSlotIndex = this.loop(targetSlotIndex - 1, this.slots.Count) + 1;
		}
		List<GameObject> list = this.slots[targetSlotIndex - 1];
		if (list.Count == 0)
		{
			Debug.LogWarning("Tried to switch weapon to slot with no variations");
			return;
		}
		if (this.currentWeapon != null)
		{
			this.currentWeapon.SetActive(false);
		}
		if (cycleVariation)
		{
			targetVariationIndex = new int?(this.loop(targetVariationIndex.GetValueOrDefault(), list.Count));
		}
		if (targetSlotIndex != this.currentSlotIndex)
		{
			this.lastSlotIndex = this.currentSlotIndex;
		}
		int? num = targetVariationIndex;
		int num2 = this.currentVariationIndex;
		if (!((num.GetValueOrDefault() == num2) & (num != null)))
		{
			this.lastVariationIndex = this.currentVariationIndex;
		}
		if (targetVariationIndex != null)
		{
			int valueOrDefault = targetVariationIndex.GetValueOrDefault();
			this.currentSlotIndex = targetSlotIndex;
			this.currentVariationIndex = this.loop(valueOrDefault, list.Count);
		}
		else if (this.currentSlotIndex == targetSlotIndex)
		{
			int @int = MonoSingleton<PrefsManager>.Instance.GetInt("WeaponRedrawBehaviour", 0);
			switch (@int)
			{
			case 0:
				num2 = this.loop(this.currentVariationIndex + 1, this.slots[targetSlotIndex - 1].Count);
				break;
			case 1:
				num2 = 0;
				break;
			case 2:
				num2 = this.currentVariationIndex;
				break;
			default:
				<PrivateImplementationDetails>.ThrowSwitchExpressionException(@int);
				break;
			}
			this.currentVariationIndex = num2;
			Debug.Log(string.Format("Index: {0}, Slot Count: {1}", this.currentVariationIndex, list.Count));
		}
		else
		{
			int num3;
			if ((useRetainedVariation || this.variationMemory) && this.retainedVariations.TryGetValue(targetSlotIndex - 1, out num3) && num3 >= 0 && num3 < list.Count)
			{
				targetVariationIndex = new int?(num3);
			}
			this.currentSlotIndex = targetSlotIndex;
			this.currentVariationIndex = targetVariationIndex.GetValueOrDefault();
		}
		this.RetainVariation(this.currentSlotIndex - 1, this.currentVariationIndex);
		if (!this.noWeapons && this.currentVariationIndex < this.slots[this.currentSlotIndex - 1].Count)
		{
			this.currentWeapon = this.slots[this.currentSlotIndex - 1][this.currentVariationIndex];
			this.currentWeapon.SetActive(true);
			this.aud.Play();
			PlayerPrefs.SetInt("CurVar", this.currentVariationIndex);
			PlayerPrefs.SetInt("CurSlo", this.currentSlotIndex);
			PlayerPrefs.SetInt("LasVar", this.lastVariationIndex);
			PlayerPrefs.SetInt("LasSlo", this.lastSlotIndex);
		}
		Action<GameObject> onWeaponChange = this.OnWeaponChange;
		if (onWeaponChange != null)
		{
			onWeaponChange(this.currentWeapon);
		}
		this.shud.SnapFreshnessSlider();
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00059F14 File Offset: 0x00058114
	public void ForceWeapon(GameObject weapon, bool setActive = true)
	{
		new List<GameObject>();
		foreach (List<GameObject> list in this.slots)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].name == weapon.name + "(Clone)" || list[i].name == weapon.name)
				{
					if (this.currentWeapon != null)
					{
						this.currentWeapon.SetActive(false);
					}
					this.currentSlotIndex = this.slots.IndexOf(list) + 1;
					this.currentVariationIndex = i;
					this.RetainVariation(this.currentSlotIndex - 1, this.currentVariationIndex);
					this.currentWeapon = list[this.currentVariationIndex];
					if (setActive)
					{
						this.currentWeapon.SetActive(true);
					}
					this.aud.Play();
					break;
				}
			}
		}
		Action<GameObject> onWeaponChange = this.OnWeaponChange;
		if (onWeaponChange == null)
		{
			return;
		}
		onWeaponChange(this.currentWeapon);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x0005A04C File Offset: 0x0005824C
	public void NoWeapon()
	{
		if (this.currentWeapon != null)
		{
			this.currentWeapon.SetActive(false);
			this.rememberedSlot = this.currentSlotIndex;
			this.activated = false;
		}
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x0005A07C File Offset: 0x0005827C
	public void YesWeapon()
	{
		if (this.slots[this.currentSlotIndex - 1].Count > this.currentVariationIndex && this.slots[this.currentSlotIndex - 1][this.currentVariationIndex] != null)
		{
			this.currentWeapon = this.slots[this.currentSlotIndex - 1][this.currentVariationIndex];
			this.currentWeapon.SetActive(true);
		}
		else if (this.slots[this.currentSlotIndex - 1].Count > 0)
		{
			this.currentWeapon = this.slots[this.currentSlotIndex - 1][0];
			this.currentVariationIndex = 0;
			this.RetainVariation(this.currentSlotIndex - 1, this.currentVariationIndex);
			this.currentWeapon.SetActive(true);
		}
		else
		{
			int num = -1;
			for (int i = 0; i < this.currentSlotIndex; i++)
			{
				if (this.slots[i].Count > 0)
				{
					num = i;
				}
			}
			if (num == -1)
			{
				num = 99;
				for (int j = this.currentSlotIndex; j < this.slots.Count; j++)
				{
					if (this.slots[j].Count > 0 && j < num)
					{
						num = j;
					}
				}
			}
			if (num != 99)
			{
				this.currentWeapon = this.slots[num][0];
				this.currentSlotIndex = num + 1;
				this.currentVariationIndex = 0;
			}
			else
			{
				this.noWeapons = true;
			}
		}
		if (this.currentWeapon != null)
		{
			this.currentWeapon.SetActive(false);
			this.activated = true;
			this.currentWeapon.SetActive(true);
		}
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0005A234 File Offset: 0x00058434
	public void AddKill()
	{
		if (this.killCharge < this.killMeter.maxValue)
		{
			this.killCharge += 1f;
			if (this.killCharge > this.killMeter.maxValue)
			{
				this.killCharge = this.killMeter.maxValue;
			}
			this.killMeter.value = this.killCharge;
		}
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0005A29B File Offset: 0x0005849B
	public void ClearKills()
	{
		this.killCharge = 0f;
		this.killMeter.value = this.killCharge;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x0005A2BC File Offset: 0x000584BC
	public void UpdateWeaponList(bool firstTime = false)
	{
		this.allWeapons.Clear();
		this.noWeapons = true;
		this.slotDict.Clear();
		int num = 0;
		foreach (List<GameObject> list in this.slots)
		{
			foreach (GameObject gameObject in list)
			{
				if (gameObject != null)
				{
					this.allWeapons.Add(gameObject);
					this.slotDict.Add(gameObject, num);
					if (this.noWeapons)
					{
						this.noWeapons = false;
					}
				}
			}
			num++;
		}
		this.UpdateWeaponIcon(firstTime);
		if (MonoSingleton<RailcannonMeter>.Instance != null)
		{
			MonoSingleton<RailcannonMeter>.Instance.CheckStatus();
		}
		if (this.shud == null)
		{
			this.shud = MonoSingleton<StyleHUD>.Instance;
		}
		this.shud.ResetFreshness();
		this.CalculateSlotCount();
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0005A3D8 File Offset: 0x000585D8
	public void UpdateWeaponIcon(bool firstTime = false)
	{
		if (this.gunPanel == null || this.gunPanel.Length == 0)
		{
			return;
		}
		if (this.noWeapons || !MonoSingleton<PrefsManager>.Instance.GetBool("weaponIcons", false) || MapInfoBase.Instance.hideStockHUD)
		{
			foreach (GameObject gameObject in this.gunPanel)
			{
				if (gameObject)
				{
					gameObject.SetActive(false);
				}
			}
			return;
		}
		foreach (GameObject gameObject2 in this.gunPanel)
		{
			if (gameObject2 != null && (!firstTime || gameObject2 != this.gunPanel[0]))
			{
				gameObject2.SetActive(true);
			}
		}
	}

	// Token: 0x0400104E RID: 4174
	private InputManager inman;

	// Token: 0x0400104F RID: 4175
	public bool activated = true;

	// Token: 0x04001050 RID: 4176
	private int rememberedSlot;

	// Token: 0x04001051 RID: 4177
	public int currentVariationIndex;

	// Token: 0x04001052 RID: 4178
	public int currentSlotIndex;

	// Token: 0x04001053 RID: 4179
	public GameObject currentWeapon;

	// Token: 0x04001054 RID: 4180
	public List<List<GameObject>> slots = new List<List<GameObject>>();

	// Token: 0x04001055 RID: 4181
	public List<GameObject> slot1 = new List<GameObject>();

	// Token: 0x04001056 RID: 4182
	public List<GameObject> slot2 = new List<GameObject>();

	// Token: 0x04001057 RID: 4183
	public List<GameObject> slot3 = new List<GameObject>();

	// Token: 0x04001058 RID: 4184
	public List<GameObject> slot4 = new List<GameObject>();

	// Token: 0x04001059 RID: 4185
	public List<GameObject> slot5 = new List<GameObject>();

	// Token: 0x0400105A RID: 4186
	public List<GameObject> slot6 = new List<GameObject>();

	// Token: 0x0400105B RID: 4187
	public List<GameObject> allWeapons = new List<GameObject>();

	// Token: 0x0400105C RID: 4188
	public Dictionary<GameObject, int> slotDict = new Dictionary<GameObject, int>();

	// Token: 0x0400105D RID: 4189
	public List<WeaponIcon> currentWeaponIcons = new List<WeaponIcon>();

	// Token: 0x0400105E RID: 4190
	private AudioSource aud;

	// Token: 0x0400105F RID: 4191
	public float killCharge;

	// Token: 0x04001060 RID: 4192
	public Slider killMeter;

	// Token: 0x04001061 RID: 4193
	public bool noWeapons = true;

	// Token: 0x04001062 RID: 4194
	public int lastSlotIndex = 69;

	// Token: 0x04001063 RID: 4195
	public int lastVariationIndex = 69;

	// Token: 0x04001064 RID: 4196
	private Dictionary<int, int> retainedVariations = new Dictionary<int, int>();

	// Token: 0x04001065 RID: 4197
	public float headShotComboTime;

	// Token: 0x04001066 RID: 4198
	public int headshots;

	// Token: 0x04001067 RID: 4199
	private bool hookCombo;

	// Token: 0x04001068 RID: 4200
	private StyleHUD shud;

	// Token: 0x04001069 RID: 4201
	public GameObject[] gunPanel;

	// Token: 0x0400106A RID: 4202
	private float scrollCooldown;

	// Token: 0x0400106B RID: 4203
	private const float WeaponWheelTime = 0.25f;

	// Token: 0x0400106C RID: 4204
	[HideInInspector]
	public int dualWieldCount;

	// Token: 0x0400106D RID: 4205
	[HideInInspector]
	public bool stayUnarmed;

	// Token: 0x0400106E RID: 4206
	[HideInInspector]
	public bool variationMemory;
}
