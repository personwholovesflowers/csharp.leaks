using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020003ED RID: 1005
public class SettingsRestoreDefaultButton : MonoBehaviour
{
	// Token: 0x06001696 RID: 5782 RVA: 0x000B5514 File Offset: 0x000B3714
	public void RestoreDefault()
	{
		UnityEvent unityEvent = this.customToggleEvent;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.defaultFloat != null)
		{
			this.slider.value = this.defaultFloat.Value / this.valueToPrefMultiplier;
			return;
		}
		if (this.defaultBool != null)
		{
			this.toggle.isOn = this.defaultBool.Value;
			return;
		}
		if (this.defaultInt != null)
		{
			if (this.dropdown != null)
			{
				this.dropdown.value = this.defaultInt.Value;
			}
			if (this.integerSlider && this.slider != null)
			{
				this.slider.value = (float)this.defaultInt.Value;
			}
		}
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000B55E0 File Offset: 0x000B37E0
	public void SetNavigation(Selectable mainSelectable)
	{
		Navigation navigation = mainSelectable.navigation;
		Selectable component = this.buttonContainer.GetComponent<Selectable>();
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnRight = component;
		mainSelectable.navigation = navigation;
		Navigation navigation2 = component.navigation;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation2.selectOnLeft = mainSelectable;
		component.navigation = navigation2;
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000B5638 File Offset: 0x000B3838
	private void Start()
	{
		if (MonoSingleton<PrefsManager>.Instance.defaultValues.ContainsKey(this.settingKey))
		{
			object obj = MonoSingleton<PrefsManager>.Instance.defaultValues[this.settingKey];
			if (obj is float)
			{
				float num = (float)obj;
				this.defaultFloat = new float?(num);
			}
			else if (obj is bool)
			{
				bool flag = (bool)obj;
				this.defaultBool = new bool?(flag);
			}
			else if (obj is int)
			{
				int num2 = (int)obj;
				this.defaultInt = new int?(num2);
			}
		}
		if (this.slider != null)
		{
			if (this.integerSlider)
			{
				if (this.defaultInt == null)
				{
					this.defaultInt = new int?(0);
				}
			}
			else if (this.defaultFloat == null)
			{
				this.defaultFloat = new float?(0f);
			}
			this.slider.onValueChanged.AddListener(delegate(float _)
			{
				this.UpdateSelf();
			});
		}
		if (this.toggle != null)
		{
			if (this.defaultBool == null)
			{
				this.defaultBool = new bool?(false);
			}
			this.toggle.onValueChanged.AddListener(delegate(bool _)
			{
				this.UpdateSelf();
			});
		}
		if (this.dropdown != null)
		{
			if (this.defaultInt == null)
			{
				this.defaultInt = new int?(0);
			}
			this.dropdown.onValueChanged.AddListener(delegate(int _)
			{
				this.UpdateSelf();
			});
		}
		this.UpdateSelf();
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x000B57C4 File Offset: 0x000B39C4
	private void UpdateSelf()
	{
		if (this.defaultInt == null && this.defaultBool == null && this.defaultFloat == null)
		{
			this.buttonContainer.SetActive(false);
			return;
		}
		if (this.defaultFloat != null && this.slider != null)
		{
			if (Math.Abs(this.defaultFloat.Value - this.slider.value * this.valueToPrefMultiplier) < this.sliderTolerance)
			{
				this.buttonContainer.SetActive(false);
				return;
			}
			this.buttonContainer.SetActive(true);
			return;
		}
		else
		{
			if (this.defaultBool == null || !(this.toggle != null))
			{
				if (this.defaultInt != null && (this.dropdown != null || (this.integerSlider && this.slider != null)))
				{
					int? num = this.ReadCurrentInt();
					if (num != null)
					{
						int value = this.defaultInt.Value;
						int? num2 = num;
						if (!((value == num2.GetValueOrDefault()) & (num2 != null)))
						{
							this.buttonContainer.SetActive(true);
							return;
						}
					}
					this.buttonContainer.SetActive(false);
					return;
				}
				return;
			}
			if (this.defaultBool.Value == this.toggle.isOn)
			{
				this.buttonContainer.SetActive(false);
				return;
			}
			this.buttonContainer.SetActive(true);
			return;
		}
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x000B592C File Offset: 0x000B3B2C
	private int? ReadCurrentInt()
	{
		if (this.dropdown != null)
		{
			return new int?(this.dropdown.value);
		}
		if (this.slider != null && this.integerSlider)
		{
			return new int?((int)this.slider.value);
		}
		return null;
	}

	// Token: 0x04001F2F RID: 7983
	public GameObject buttonContainer;

	// Token: 0x04001F30 RID: 7984
	public string settingKey;

	// Token: 0x04001F31 RID: 7985
	[Header("Float")]
	public Slider slider;

	// Token: 0x04001F32 RID: 7986
	public float valueToPrefMultiplier = 1f;

	// Token: 0x04001F33 RID: 7987
	public float sliderTolerance = 0.01f;

	// Token: 0x04001F34 RID: 7988
	public bool integerSlider;

	// Token: 0x04001F35 RID: 7989
	[Header("Integer")]
	public TMP_Dropdown dropdown;

	// Token: 0x04001F36 RID: 7990
	[Header("Boolean")]
	public Toggle toggle;

	// Token: 0x04001F37 RID: 7991
	[SerializeField]
	private UnityEvent customToggleEvent;

	// Token: 0x04001F38 RID: 7992
	private float? defaultFloat;

	// Token: 0x04001F39 RID: 7993
	private int? defaultInt;

	// Token: 0x04001F3A RID: 7994
	private bool? defaultBool;
}
