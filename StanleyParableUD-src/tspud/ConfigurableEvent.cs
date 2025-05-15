using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000033 RID: 51
public class ConfigurableEvent : MonoBehaviour
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000118 RID: 280 RVA: 0x00009125 File Offset: 0x00007325
	// (set) Token: 0x06000119 RID: 281 RVA: 0x00005444 File Offset: 0x00003644
	public Configurable Configurable
	{
		get
		{
			return this.configurable;
		}
		private set
		{
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600011A RID: 282 RVA: 0x0000912D File Offset: 0x0000732D
	private bool CanInvokeOnStart
	{
		get
		{
			return this.invokeBehaviour == ConfigurableEvent.InvokeBehaviour.AlwaysOnceStarted || this.invokeBehaviour == ConfigurableEvent.InvokeBehaviour.OnlyIfActiveAndEnabled;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600011B RID: 283 RVA: 0x00009142 File Offset: 0x00007342
	private bool CanInvoke
	{
		get
		{
			return this.invokeBehaviour != ConfigurableEvent.InvokeBehaviour.OnlyIfActiveAndEnabled || base.isActiveAndEnabled;
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00009155 File Offset: 0x00007355
	public void SetSelfInvokeOnValueChange(bool val)
	{
		this.selfInvokeOnValueChange = val;
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600011D RID: 285 RVA: 0x0000915E File Offset: 0x0000735E
	public bool SelfInvokeOnValueChange
	{
		get
		{
			return this.selfInvokeOnValueChange;
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00009168 File Offset: 0x00007368
	private void Awake()
	{
		this.debugComponent = base.GetComponent<ConfigurableEventDebug>();
		if (this.configurable != null)
		{
			Configurable configurable = this.configurable;
			configurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(configurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
			this.configurable.Init();
		}
	}

	// Token: 0x0600011F RID: 287 RVA: 0x000091C1 File Offset: 0x000073C1
	private void Start()
	{
		if (this.configurable != null)
		{
			this.configurable.Init();
		}
		if (this.CanInvoke && this.CanInvokeOnStart && this.selfInvokeOnValueChange)
		{
			this.Invoke();
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x000091FA File Offset: 0x000073FA
	private void OnDestroy()
	{
		if (this.configurable != null)
		{
			Configurable configurable = this.configurable;
			configurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(configurable.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
		}
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00009231 File Offset: 0x00007431
	private void OnValueChanged(LiveData data)
	{
		if (this.CanInvoke && this.selfInvokeOnValueChange)
		{
			this.Invoke();
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0000924C File Offset: 0x0000744C
	public void Invoke()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.debugComponent)
		{
			this.debugComponent.OnEvaluateDebug();
		}
		bool flag;
		switch (this.configurableType)
		{
		case Configurator.ConfiguratorTypes.Int:
			flag = this.EvaluateIntegerCondition();
			break;
		case Configurator.ConfiguratorTypes.Float:
			flag = this.EvaluateFloatCondition();
			break;
		case Configurator.ConfiguratorTypes.Boolean:
			flag = this.EvaluateBooleanCondition();
			break;
		case Configurator.ConfiguratorTypes.String:
			flag = this.EvaluateStringCondition();
			break;
		case Configurator.ConfiguratorTypes.Custom:
			return;
		default:
			return;
		}
		if (flag)
		{
			if (this.debugComponent)
			{
				this.debugComponent.OnConditionMetDebug();
			}
			this.onConditionMet.Invoke();
			return;
		}
		if (this.debugComponent)
		{
			this.debugComponent.OnConditionNotMetDebug();
		}
		this.onConditionNotMet.Invoke();
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00009310 File Offset: 0x00007510
	private bool EvaluateIntegerCondition()
	{
		switch (this.numberCondition)
		{
		case ConfigurableEvent.NumberEventCondition.IsSmallerThan:
			return this.configurable.GetIntValue() < this.testInteger;
		case ConfigurableEvent.NumberEventCondition.IsBiggerThan:
			return this.configurable.GetIntValue() > this.testInteger;
		case ConfigurableEvent.NumberEventCondition.IsEqual:
			return this.configurable.GetIntValue().Equals(this.testInteger);
		case ConfigurableEvent.NumberEventCondition.IsDifferentThan:
			return !this.configurable.GetIntValue().Equals(this.testFloat);
		default:
			return false;
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x000093A4 File Offset: 0x000075A4
	private bool EvaluateFloatCondition()
	{
		switch (this.numberCondition)
		{
		case ConfigurableEvent.NumberEventCondition.IsSmallerThan:
			return this.configurable.GetFloatValue() < this.testFloat;
		case ConfigurableEvent.NumberEventCondition.IsBiggerThan:
			return this.configurable.GetFloatValue() > this.testFloat;
		case ConfigurableEvent.NumberEventCondition.IsEqual:
			return this.configurable.GetFloatValue().Equals(this.testFloat);
		case ConfigurableEvent.NumberEventCondition.IsDifferentThan:
			return !this.configurable.GetFloatValue().Equals(this.testFloat);
		default:
			return false;
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00009430 File Offset: 0x00007630
	private bool EvaluateBooleanCondition()
	{
		if (this.configurable == null)
		{
			return false;
		}
		ConfigurableEvent.ToggleEventCondition toggleEventCondition = this.toggleCondition;
		if (toggleEventCondition != ConfigurableEvent.ToggleEventCondition.IsTrue)
		{
			return toggleEventCondition == ConfigurableEvent.ToggleEventCondition.IsFalse && this.configurable.GetBooleanValue().Equals(false);
		}
		return this.configurable.GetBooleanValue().Equals(true);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009488 File Offset: 0x00007688
	private bool EvaluateStringCondition()
	{
		ConfigurableEvent.StringEventCondition stringEventCondition = this.stringCondition;
		if (stringEventCondition != ConfigurableEvent.StringEventCondition.Equals)
		{
			return stringEventCondition == ConfigurableEvent.StringEventCondition.IsDifferentThan && !this.configurable.GetStringValue().Equals(this.testString);
		}
		return this.configurable.GetStringValue().Equals(this.testString);
	}

	// Token: 0x04000182 RID: 386
	[SerializeField]
	private Configurator.ConfiguratorTypes configurableType;

	// Token: 0x04000183 RID: 387
	[SerializeField]
	private Configurable configurable;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private int testInteger;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	private float testFloat;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	private string testString;

	// Token: 0x04000187 RID: 391
	[SerializeField]
	private ConfigurableEvent.NumberEventCondition numberCondition;

	// Token: 0x04000188 RID: 392
	[SerializeField]
	private ConfigurableEvent.ToggleEventCondition toggleCondition;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	private ConfigurableEvent.StringEventCondition stringCondition;

	// Token: 0x0400018A RID: 394
	[SerializeField]
	private bool selfInvokeOnValueChange;

	// Token: 0x0400018B RID: 395
	[SerializeField]
	public ConfigurableEvent.InvokeBehaviour invokeBehaviour = ConfigurableEvent.InvokeBehaviour.OnlyIfActiveAndEnabled;

	// Token: 0x0400018C RID: 396
	[SerializeField]
	private bool onlyInvokeIfActiveAndEnabled;

	// Token: 0x0400018D RID: 397
	[SerializeField]
	private UnityEvent onConditionMet;

	// Token: 0x0400018E RID: 398
	[SerializeField]
	private UnityEvent onConditionNotMet;

	// Token: 0x0400018F RID: 399
	private ConfigurableEventDebug debugComponent;

	// Token: 0x02000358 RID: 856
	public enum ToggleEventCondition
	{
		// Token: 0x0400120F RID: 4623
		IsTrue,
		// Token: 0x04001210 RID: 4624
		IsFalse
	}

	// Token: 0x02000359 RID: 857
	public enum NumberEventCondition
	{
		// Token: 0x04001212 RID: 4626
		IsSmallerThan,
		// Token: 0x04001213 RID: 4627
		IsBiggerThan,
		// Token: 0x04001214 RID: 4628
		IsEqual,
		// Token: 0x04001215 RID: 4629
		IsDifferentThan
	}

	// Token: 0x0200035A RID: 858
	public enum StringEventCondition
	{
		// Token: 0x04001217 RID: 4631
		Equals,
		// Token: 0x04001218 RID: 4632
		IsDifferentThan
	}

	// Token: 0x0200035B RID: 859
	public enum InvokeBehaviour
	{
		// Token: 0x0400121A RID: 4634
		AlwaysOnceStarted,
		// Token: 0x0400121B RID: 4635
		OnlyIfActiveAndEnabled,
		// Token: 0x0400121C RID: 4636
		AlwaysAfterStarted,
		// Token: 0x0400121D RID: 4637
		AfterStartedIfActiveAndEnabled
	}
}
