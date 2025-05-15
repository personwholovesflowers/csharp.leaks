using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000323 RID: 803
public class OptionsCheckbox : MonoBehaviour
{
	// Token: 0x0600127F RID: 4735 RVA: 0x0009438C File Offset: 0x0009258C
	private void Awake()
	{
		this.toggle.SetIsOnWithoutNotify(MonoSingleton<PrefsManager>.Instance.GetBool(this.prefsKey, this.toggle.isOn));
		this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnChanged));
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x000943DB File Offset: 0x000925DB
	private void OnChanged(bool value)
	{
		MonoSingleton<PrefsManager>.Instance.SetBool(this.prefsKey, value);
	}

	// Token: 0x04001973 RID: 6515
	public Toggle toggle;

	// Token: 0x04001974 RID: 6516
	public string prefsKey;
}
