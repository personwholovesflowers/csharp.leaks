using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200008E RID: 142
public class AnyConfigurableChangedEvent : MonoBehaviour
{
	// Token: 0x06000364 RID: 868 RVA: 0x00016B7C File Offset: 0x00014D7C
	private void Start()
	{
		foreach (Configurable configurable in this.configurables)
		{
			configurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(configurable.OnValueChanged, new Action<LiveData>(this.ValueChangedEvent));
		}
	}

	// Token: 0x06000365 RID: 869 RVA: 0x00016BC4 File Offset: 0x00014DC4
	private void OnDestroy()
	{
		foreach (Configurable configurable in this.configurables)
		{
			if (configurable != null)
			{
				Configurable configurable2 = configurable;
				configurable2.OnValueChanged = (Action<LiveData>)Delegate.Remove(configurable2.OnValueChanged, new Action<LiveData>(this.ValueChangedEvent));
			}
		}
	}

	// Token: 0x06000366 RID: 870 RVA: 0x00016C15 File Offset: 0x00014E15
	private void ValueChangedEvent(LiveData liveData)
	{
		UnityEvent onAnyConfigurableChange = this.OnAnyConfigurableChange;
		if (onAnyConfigurableChange == null)
		{
			return;
		}
		onAnyConfigurableChange.Invoke();
	}

	// Token: 0x0400035A RID: 858
	public Configurable[] configurables;

	// Token: 0x0400035B RID: 859
	public bool invokeCheckOnStart;

	// Token: 0x0400035C RID: 860
	public UnityEvent OnAnyConfigurableChange;
}
