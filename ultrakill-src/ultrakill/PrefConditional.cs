using System;
using UnityEngine;

// Token: 0x02000355 RID: 853
public class PrefConditional : MonoBehaviour
{
	// Token: 0x060013D3 RID: 5075 RVA: 0x0009E79C File Offset: 0x0009C99C
	private void Start()
	{
		this.CheckValue();
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x0009E7A4 File Offset: 0x0009C9A4
	public void CheckValue()
	{
		if (this.isInt)
		{
			if ((this.isLocal ? MonoSingleton<PrefsManager>.Instance.GetIntLocal(this.prefName, 0) : MonoSingleton<PrefsManager>.Instance.GetInt(this.prefName, 0)) > 0)
			{
				this.valueEvent.Invoke("");
				return;
			}
			this.valueEvent.Revert();
			return;
		}
		else
		{
			if (this.isLocal ? MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.prefName, false) : MonoSingleton<PrefsManager>.Instance.GetBool(this.prefName, false))
			{
				this.valueEvent.Invoke("");
				return;
			}
			this.valueEvent.Revert();
			return;
		}
	}

	// Token: 0x04001B3A RID: 6970
	public string prefName;

	// Token: 0x04001B3B RID: 6971
	public bool isLocal;

	// Token: 0x04001B3C RID: 6972
	public bool isInt;

	// Token: 0x04001B3D RID: 6973
	public UltrakillEvent valueEvent;
}
