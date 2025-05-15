using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000127 RID: 295
public class SequelToggleButton : MonoBehaviour
{
	// Token: 0x060006F8 RID: 1784 RVA: 0x00024CCF File Offset: 0x00022ECF
	public void SetConfigurableToIndex()
	{
		if (this.fixType == SequelToggleButton.FixType.Prefix)
		{
			this.prefixIndexConfigurable.SetValue(this.index);
		}
		if (this.fixType == SequelToggleButton.FixType.Postfix)
		{
			this.postfixIndexConfigurable.SetValue(this.index);
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x00024D04 File Offset: 0x00022F04
	private void OnValidate()
	{
		this.SetTerm();
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x00024D0C File Offset: 0x00022F0C
	private void Start()
	{
		this.SetTerm();
		IntConfigurable intConfigurable = this.prefixIndexConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		IntConfigurable intConfigurable2 = this.postfixIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(intConfigurable2.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged += this.Instance_OnInputDeviceTypeChanged;
		this.OnValueChanged(null);
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x00005444 File Offset: 0x00003644
	private void Instance_OnInputDeviceTypeChanged(GameMaster.InputDevice input)
	{
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x00024D8C File Offset: 0x00022F8C
	private void OnDestroy()
	{
		IntConfigurable intConfigurable = this.prefixIndexConfigurable;
		intConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		IntConfigurable intConfigurable2 = this.postfixIndexConfigurable;
		intConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Remove(intConfigurable2.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		if (Singleton<GameMaster>.Instance != null)
		{
			Singleton<GameMaster>.Instance.OnInputDeviceTypeChanged -= this.Instance_OnInputDeviceTypeChanged;
		}
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x00024E0A File Offset: 0x0002300A
	public void OnValueChanged(LiveData ld)
	{
		if (this.fixType == SequelToggleButton.FixType.Postfix)
		{
			this.toggle.interactable = this.prefixIndexConfigurable.GetIntValue() != -1;
		}
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x00024E31 File Offset: 0x00023031
	public void SetIndex(int i)
	{
		this.index = i;
		this.SetTerm();
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x00024E40 File Offset: 0x00023040
	private void SetTerm()
	{
		if (this.fixType == SequelToggleButton.FixType.Prefix)
		{
			this.localize.Term = SequelTools.PrefixTerm(this.index);
		}
		if (this.fixType == SequelToggleButton.FixType.Postfix)
		{
			this.localize.Term = SequelTools.PostfixTerm(this.index);
		}
	}

	// Token: 0x04000738 RID: 1848
	public Toggle toggle;

	// Token: 0x04000739 RID: 1849
	public SequelToggleButton.FixType fixType;

	// Token: 0x0400073A RID: 1850
	[SerializeField]
	private int index;

	// Token: 0x0400073B RID: 1851
	[SerializeField]
	private IntConfigurable prefixIndexConfigurable;

	// Token: 0x0400073C RID: 1852
	[SerializeField]
	private IntConfigurable postfixIndexConfigurable;

	// Token: 0x0400073D RID: 1853
	public Localize localize;

	// Token: 0x020003CE RID: 974
	public enum FixType
	{
		// Token: 0x0400140E RID: 5134
		Prefix,
		// Token: 0x0400140F RID: 5135
		Postfix
	}
}
