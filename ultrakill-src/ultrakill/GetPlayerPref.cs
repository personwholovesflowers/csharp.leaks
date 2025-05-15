using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000229 RID: 553
public class GetPlayerPref : MonoBehaviour
{
	// Token: 0x06000BDC RID: 3036 RVA: 0x000535CC File Offset: 0x000517CC
	private void Awake()
	{
		string text = this.pref;
		if (!(text == "DisCha"))
		{
			if (!(text == "ShoUseTut"))
			{
				if (!(text == "MainMenuEncorePopUp"))
				{
					this.pref = "weapon." + this.pref;
					if (MonoSingleton<PrefsManager>.Instance.GetInt(this.pref, 1) == this.valueToCheckFor)
					{
						UnityEvent unityEvent = this.onCheckSuccess;
						if (unityEvent == null)
						{
							return;
						}
						unityEvent.Invoke();
						return;
					}
					else
					{
						UnityEvent unityEvent2 = this.onCheckFail;
						if (unityEvent2 == null)
						{
							return;
						}
						unityEvent2.Invoke();
						return;
					}
				}
				else
				{
					this.pref = "MainMenuEncorePopUp";
					if (MonoSingleton<PrefsManager>.Instance.GetInt(this.pref, 0) == this.valueToCheckFor)
					{
						UnityEvent unityEvent3 = this.onCheckSuccess;
						if (unityEvent3 == null)
						{
							return;
						}
						unityEvent3.Invoke();
						return;
					}
					else
					{
						UnityEvent unityEvent4 = this.onCheckFail;
						if (unityEvent4 == null)
						{
							return;
						}
						unityEvent4.Invoke();
						return;
					}
				}
			}
			else
			{
				this.pref = "hideShotgunPopup";
				if (MonoSingleton<PrefsManager>.Instance.GetBool(this.pref, false))
				{
					UnityEvent unityEvent5 = this.onCheckSuccess;
					if (unityEvent5 == null)
					{
						return;
					}
					unityEvent5.Invoke();
					return;
				}
				else
				{
					UnityEvent unityEvent6 = this.onCheckFail;
					if (unityEvent6 == null)
					{
						return;
					}
					unityEvent6.Invoke();
					return;
				}
			}
		}
		else if (PlayerPrefs.GetInt(this.pref, 0) == this.valueToCheckFor)
		{
			UnityEvent unityEvent7 = this.onCheckSuccess;
			if (unityEvent7 == null)
			{
				return;
			}
			unityEvent7.Invoke();
			return;
		}
		else
		{
			UnityEvent unityEvent8 = this.onCheckFail;
			if (unityEvent8 == null)
			{
				return;
			}
			unityEvent8.Invoke();
			return;
		}
	}

	// Token: 0x04000F74 RID: 3956
	public string pref;

	// Token: 0x04000F75 RID: 3957
	public int valueToCheckFor;

	// Token: 0x04000F76 RID: 3958
	public UnityEvent onCheckSuccess;

	// Token: 0x04000F77 RID: 3959
	public UnityEvent onCheckFail;
}
