using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200003E RID: 62
public class Configurator : MonoBehaviour
{
	// Token: 0x06000134 RID: 308 RVA: 0x000095A3 File Offset: 0x000077A3
	protected void Start()
	{
		if (this.configurable != null)
		{
			this.AssignConfigurable(this.configurable);
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x000095BF File Offset: 0x000077BF
	private void OnDestroy()
	{
		if (this.configurable != null)
		{
			Configurable configurable = this.configurable;
			configurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(configurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		}
	}

	// Token: 0x06000136 RID: 310 RVA: 0x000095F6 File Offset: 0x000077F6
	private void OnEnable()
	{
		if (this.updateOnEnable && this.configurable != null)
		{
			this.configurable.ForceUpdate();
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000961C File Offset: 0x0000781C
	public void AssignConfigurable(Configurable newConfigurable)
	{
		if (this.configurable != null)
		{
			Configurable configurable = this.configurable;
			configurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(configurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		}
		this.configurable = newConfigurable;
		this.configurable.Init();
		string text = this.configurable.PrintValue();
		this.OnPrintValue.Invoke(text);
		Configurable configurable2 = this.configurable;
		configurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(configurable2.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		this.configurable.ForceUpdate();
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000096BA File Offset: 0x000078BA
	protected void UpdateDeviationStatus(bool deviates)
	{
		if (this.configuratorValueEqualsSavedValue && deviates)
		{
			this.OnValueDeviated.Invoke();
			this.configuratorValueEqualsSavedValue = false;
		}
		if (!this.configuratorValueEqualsSavedValue && !deviates)
		{
			this.OnValueMatched.Invoke();
			this.configuratorValueEqualsSavedValue = true;
		}
	}

	// Token: 0x06000139 RID: 313 RVA: 0x000096F8 File Offset: 0x000078F8
	public virtual void IncreaseValue()
	{
		Configurator.ConfiguratorTypes configuratorTypes = this.configuratorType;
		if (configuratorTypes != Configurator.ConfiguratorTypes.Int)
		{
			if (configuratorTypes == Configurator.ConfiguratorTypes.Boolean)
			{
				this.BooleanValueToggle();
			}
		}
		else
		{
			this.IntValueIncrease();
		}
		this.SaveValue();
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000972C File Offset: 0x0000792C
	public virtual void DecreaseValue()
	{
		Configurator.ConfiguratorTypes configuratorTypes = this.configuratorType;
		if (configuratorTypes != Configurator.ConfiguratorTypes.Int)
		{
			if (configuratorTypes == Configurator.ConfiguratorTypes.Boolean)
			{
				this.BooleanValueToggle();
			}
		}
		else
		{
			this.IntValueDecrease();
		}
		this.SaveValue();
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000975D File Offset: 0x0000795D
	public void IntValueIncrease()
	{
		if (this.configurable is IntConfigurable)
		{
			(this.configurable as IntConfigurable).IncreaseValue();
		}
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000977C File Offset: 0x0000797C
	public void IntValueDecrease()
	{
		if (this.configurable is IntConfigurable)
		{
			(this.configurable as IntConfigurable).DecreaseValue();
		}
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000979B File Offset: 0x0000799B
	public void IntValueChange(int value)
	{
		if (this.configurable is IntConfigurable)
		{
			(this.configurable as IntConfigurable).SetValue(value);
		}
	}

	// Token: 0x0600013E RID: 318 RVA: 0x000097BB File Offset: 0x000079BB
	public void IntValueChangeRounded(float value)
	{
		if (this.configurable is IntConfigurable)
		{
			(this.configurable as IntConfigurable).SetValue(Mathf.RoundToInt(value));
		}
	}

	// Token: 0x0600013F RID: 319 RVA: 0x000097E0 File Offset: 0x000079E0
	public void FloatValueChange(float value)
	{
		if (this.configurable is FloatConfigurable)
		{
			(this.configurable as FloatConfigurable).SetValue(value);
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00009800 File Offset: 0x00007A00
	public void BooleanValueChange(bool value)
	{
		if (this.configurable is BooleanConfigurable)
		{
			(this.configurable as BooleanConfigurable).SetValue(value);
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x00009820 File Offset: 0x00007A20
	public void BooleanValueToggle()
	{
		if (this.configurable is BooleanConfigurable)
		{
			BooleanConfigurable booleanConfigurable = this.configurable as BooleanConfigurable;
			bool booleanValue = booleanConfigurable.GetBooleanValue();
			booleanConfigurable.SetValue(!booleanValue);
		}
	}

	// Token: 0x06000142 RID: 322 RVA: 0x00009855 File Offset: 0x00007A55
	public void StringValueChange(string value)
	{
		if (this.configurable is StringConfigurable)
		{
			(this.configurable as StringConfigurable).SetValue(value);
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00009875 File Offset: 0x00007A75
	public void SaveValue()
	{
		this.configurable.SaveToDiskAll();
		this.configuratorValueEqualsSavedValue = true;
		this.UpdateDeviationStatus(false);
		this.ApplyData();
	}

	// Token: 0x06000144 RID: 324 RVA: 0x00005444 File Offset: 0x00003644
	public virtual void ApplyData()
	{
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00009896 File Offset: 0x00007A96
	public virtual void PrintValue(Configurable _configurable)
	{
		this.OnPrintValue.Invoke(_configurable.PrintValue());
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000098A9 File Offset: 0x00007AA9
	public virtual void PrintValueWithThisConfigurable()
	{
		this.OnPrintValue.Invoke(this.configurable.PrintValue());
	}

	// Token: 0x06000147 RID: 327 RVA: 0x000098C4 File Offset: 0x00007AC4
	protected void OnValueChanged(LiveData arg)
	{
		switch (this.configurable.GetConfigurableType())
		{
		case ConfigurableTypes.Int:
			this.OnIntValueChanged.Invoke(arg.IntValue);
			break;
		case ConfigurableTypes.Float:
			this.OnFloatValueChanged.Invoke(arg.FloatValue);
			break;
		case ConfigurableTypes.Boolean:
			this.OnBooleanValueChanged.Invoke(arg.BooleanValue);
			this.OnBooleanValueChangedInverse.Invoke(!arg.BooleanValue);
			break;
		case ConfigurableTypes.String:
			this.OnStringValueChanged.Invoke(arg.StringValue);
			break;
		}
		if (this.liveChange)
		{
			this.SaveValue();
		}
		else
		{
			this.UpdateDeviationStatus(this.configurable.DeviatesFromSavedValue);
		}
		this.PrintValue(this.configurable);
	}

	// Token: 0x040001A0 RID: 416
	[SerializeField]
	private Configurator.ConfiguratorTypes configuratorType;

	// Token: 0x040001A1 RID: 417
	private bool liveChange = true;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private bool updateOnEnable = true;

	// Token: 0x040001A3 RID: 419
	[SerializeField]
	protected Configurable configurable;

	// Token: 0x040001A4 RID: 420
	[SerializeField]
	protected IntValueChangedEvent OnIntValueChanged;

	// Token: 0x040001A5 RID: 421
	[SerializeField]
	protected FloatValueChangedEvent OnFloatValueChanged;

	// Token: 0x040001A6 RID: 422
	[SerializeField]
	protected BooleanValueChangedEvent OnBooleanValueChanged;

	// Token: 0x040001A7 RID: 423
	[SerializeField]
	protected BooleanValueChangedEvent OnBooleanValueChangedInverse;

	// Token: 0x040001A8 RID: 424
	[SerializeField]
	protected StringValueChangedEvent OnStringValueChanged;

	// Token: 0x040001A9 RID: 425
	[SerializeField]
	protected PrintValueEvent OnPrintValue;

	// Token: 0x040001AA RID: 426
	[SerializeField]
	protected UnityEvent OnValueDeviated;

	// Token: 0x040001AB RID: 427
	[SerializeField]
	protected UnityEvent OnValueMatched;

	// Token: 0x040001AC RID: 428
	private bool configuratorValueEqualsSavedValue = true;

	// Token: 0x0200035C RID: 860
	public enum ConfiguratorTypes
	{
		// Token: 0x0400121F RID: 4639
		Int,
		// Token: 0x04001220 RID: 4640
		Float,
		// Token: 0x04001221 RID: 4641
		Boolean,
		// Token: 0x04001222 RID: 4642
		String,
		// Token: 0x04001223 RID: 4643
		Custom
	}
}
