using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200003F RID: 63
public class DualBoolConfigurableEvent : MonoBehaviour
{
	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000149 RID: 329 RVA: 0x0000999D File Offset: 0x00007B9D
	private IEnumerable<BooleanConfigurable> Configurables
	{
		get
		{
			yield return this.configurableA;
			yield return this.configurableB;
			yield break;
		}
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000099B0 File Offset: 0x00007BB0
	private void Awake()
	{
		foreach (BooleanConfigurable booleanConfigurable in this.Configurables)
		{
			if (booleanConfigurable != null)
			{
				BooleanConfigurable booleanConfigurable2 = booleanConfigurable;
				booleanConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Combine(booleanConfigurable2.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
				booleanConfigurable.Init();
			}
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009A28 File Offset: 0x00007C28
	private void Start()
	{
		foreach (BooleanConfigurable booleanConfigurable in this.Configurables)
		{
			if (booleanConfigurable != null)
			{
				booleanConfigurable.Init();
			}
		}
		if (this.invokeOnStart)
		{
			this.Invoke();
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00009A8C File Offset: 0x00007C8C
	private void OnDestroy()
	{
		foreach (BooleanConfigurable booleanConfigurable in this.Configurables)
		{
			if (booleanConfigurable != null)
			{
				BooleanConfigurable booleanConfigurable2 = booleanConfigurable;
				booleanConfigurable2.OnValueChanged = (Action<LiveData>)Delegate.Remove(booleanConfigurable2.OnValueChanged, new Action<LiveData>(this.OnValueChanged));
			}
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00009B00 File Offset: 0x00007D00
	private void OnValueChanged(LiveData data)
	{
		if (base.enabled && this.invokeOnValueChange)
		{
			this.Invoke();
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00009B18 File Offset: 0x00007D18
	public void Invoke()
	{
		bool flag = this.configurableA != null && this.configurableA.GetBooleanValue();
		bool flag2 = this.configurableB != null && this.configurableB.GetBooleanValue();
		bool flag3;
		switch (this.operation)
		{
		case DualBoolConfigurableEvent.Operator.AND:
			flag3 = flag && flag2;
			break;
		case DualBoolConfigurableEvent.Operator.OR:
			flag3 = flag || flag2;
			break;
		case DualBoolConfigurableEvent.Operator.NAND:
			flag3 = !flag || !flag2;
			break;
		case DualBoolConfigurableEvent.Operator.NOR:
			flag3 = !flag && !flag2;
			break;
		case DualBoolConfigurableEvent.Operator.XOR:
			flag3 = flag ^ flag2;
			break;
		case DualBoolConfigurableEvent.Operator.NXOR_EQUALITY:
			flag3 = flag == flag2;
			break;
		case DualBoolConfigurableEvent.Operator.ALWAYS_TRUE:
			flag3 = true;
			break;
		case DualBoolConfigurableEvent.Operator.ALWAYS_FALSE:
			flag3 = true;
			break;
		case DualBoolConfigurableEvent.Operator.ONLY_A:
			flag3 = flag;
			break;
		case DualBoolConfigurableEvent.Operator.ONLY_B:
			flag3 = flag2;
			break;
		case DualBoolConfigurableEvent.Operator.ONLY_A_NEGATED:
			flag3 = !flag;
			break;
		case DualBoolConfigurableEvent.Operator.ONLY_B_NEGATED:
			flag3 = !flag2;
			break;
		case DualBoolConfigurableEvent.Operator.A__AND__B_NEG:
			flag3 = flag & !flag2;
			break;
		case DualBoolConfigurableEvent.Operator.A_NEG__AND__B:
			flag3 = !flag && flag2;
			break;
		case DualBoolConfigurableEvent.Operator.A__OR__B_NEG:
			flag3 = flag | !flag2;
			break;
		case DualBoolConfigurableEvent.Operator.A_NEG__OR__B:
			flag3 = !flag || flag2;
			break;
		default:
			return;
		}
		DualBoolConfigurableEvent.BooleanUnityEvent booleanUnityEvent = this.onEvaluate;
		if (booleanUnityEvent != null)
		{
			booleanUnityEvent.Invoke(flag3);
		}
		DualBoolConfigurableEvent.BooleanUnityEvent booleanUnityEvent2 = this.onEvaluateNegated;
		if (booleanUnityEvent2 != null)
		{
			booleanUnityEvent2.Invoke(!flag3);
		}
		if (flag3)
		{
			UnityEvent unityEvent = this.onConditionMet;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke();
			return;
		}
		else
		{
			UnityEvent unityEvent2 = this.onConditionNotMet;
			if (unityEvent2 == null)
			{
				return;
			}
			unityEvent2.Invoke();
			return;
		}
	}

	// Token: 0x040001AD RID: 429
	[SerializeField]
	private BooleanConfigurable configurableA;

	// Token: 0x040001AE RID: 430
	[SerializeField]
	private DualBoolConfigurableEvent.Operator operation;

	// Token: 0x040001AF RID: 431
	[SerializeField]
	private BooleanConfigurable configurableB;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	private bool invokeOnValueChange;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	private bool invokeOnStart;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private DualBoolConfigurableEvent.BooleanUnityEvent onEvaluate;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private DualBoolConfigurableEvent.BooleanUnityEvent onEvaluateNegated;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	private UnityEvent onConditionMet;

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private UnityEvent onConditionNotMet;

	// Token: 0x0200035D RID: 861
	[Serializable]
	public class BooleanUnityEvent : UnityEvent<bool>
	{
	}

	// Token: 0x0200035E RID: 862
	public enum Operator
	{
		// Token: 0x04001225 RID: 4645
		AND,
		// Token: 0x04001226 RID: 4646
		OR,
		// Token: 0x04001227 RID: 4647
		NAND,
		// Token: 0x04001228 RID: 4648
		NOR,
		// Token: 0x04001229 RID: 4649
		XOR,
		// Token: 0x0400122A RID: 4650
		NXOR_EQUALITY,
		// Token: 0x0400122B RID: 4651
		ALWAYS_TRUE,
		// Token: 0x0400122C RID: 4652
		ALWAYS_FALSE,
		// Token: 0x0400122D RID: 4653
		ONLY_A,
		// Token: 0x0400122E RID: 4654
		ONLY_B,
		// Token: 0x0400122F RID: 4655
		ONLY_A_NEGATED,
		// Token: 0x04001230 RID: 4656
		ONLY_B_NEGATED,
		// Token: 0x04001231 RID: 4657
		A__AND__B_NEG,
		// Token: 0x04001232 RID: 4658
		A_NEG__AND__B,
		// Token: 0x04001233 RID: 4659
		A__OR__B_NEG,
		// Token: 0x04001234 RID: 4660
		A_NEG__OR__B
	}
}
