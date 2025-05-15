using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200034A RID: 842
public class PlayerPrefButton : MonoBehaviour
{
	// Token: 0x06001365 RID: 4965 RVA: 0x0009BDC1 File Offset: 0x00099FC1
	private void Start()
	{
		this.CheckPref();
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0009BDC9 File Offset: 0x00099FC9
	public void SetValue(int value)
	{
		MonoSingleton<PrefsManager>.Instance.SetInt("cyberGrind.theme", value);
		this.CheckPref();
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0009BDE4 File Offset: 0x00099FE4
	public void CheckPref()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.theme", this.defaultValue);
		if (this.onIntValues != null && this.onIntValues.Length != 0)
		{
			if (@int < this.onIntValues.Length)
			{
				UnityEvent unityEvent = this.onIntValues[@int];
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
				return;
			}
			else if (@int == 0)
			{
				UnityEvent unityEvent2 = this.onIntValues[0];
				if (unityEvent2 == null)
				{
					return;
				}
				unityEvent2.Invoke();
				return;
			}
			else
			{
				UnityEvent unityEvent3 = this.onIntValues[this.onIntValues.Length - 1];
				if (unityEvent3 == null)
				{
					return;
				}
				unityEvent3.Invoke();
			}
		}
	}

	// Token: 0x04001ABB RID: 6843
	[Header("ONLY FOR CYBER GRIND THEME FUCK YOU FIELD IGNORED FUCK YOU")]
	public string playerPref;

	// Token: 0x04001ABC RID: 6844
	public int defaultValue;

	// Token: 0x04001ABD RID: 6845
	public UnityEvent[] onIntValues;
}
