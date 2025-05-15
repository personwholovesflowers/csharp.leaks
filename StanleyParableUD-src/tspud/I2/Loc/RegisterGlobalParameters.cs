using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002CE RID: 718
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06001277 RID: 4727 RVA: 0x00063290 File Offset: 0x00061490
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06001278 RID: 4728 RVA: 0x000632B0 File Offset: 0x000614B0
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x06001279 RID: 4729 RVA: 0x00008FD9 File Offset: 0x000071D9
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
