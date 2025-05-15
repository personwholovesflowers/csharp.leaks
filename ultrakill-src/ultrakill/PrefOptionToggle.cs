using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000356 RID: 854
public class PrefOptionToggle : MonoBehaviour
{
	// Token: 0x060013D6 RID: 5078 RVA: 0x0009E84F File Offset: 0x0009CA4F
	private void Awake()
	{
		this.toggle.SetIsOnWithoutNotify(this.GetPref());
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggle));
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x0009E880 File Offset: 0x0009CA80
	private bool GetPref()
	{
		if (MonoSingleton<PrefsManager>.Instance == null)
		{
			return this.fallbackValue;
		}
		if (this.isLocal)
		{
			return MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.prefName, this.fallbackValue);
		}
		return MonoSingleton<PrefsManager>.Instance.GetBool(this.prefName, this.fallbackValue);
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x0009E8D6 File Offset: 0x0009CAD6
	private void OnToggle(bool value)
	{
		if (MonoSingleton<PrefsManager>.Instance == null)
		{
			return;
		}
		if (this.isLocal)
		{
			MonoSingleton<PrefsManager>.Instance.SetBoolLocal(this.prefName, value);
			return;
		}
		MonoSingleton<PrefsManager>.Instance.SetBool(this.prefName, value);
	}

	// Token: 0x04001B3E RID: 6974
	public string prefName;

	// Token: 0x04001B3F RID: 6975
	public bool isLocal;

	// Token: 0x04001B40 RID: 6976
	public bool fallbackValue;

	// Token: 0x04001B41 RID: 6977
	public Toggle toggle;
}
