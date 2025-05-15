using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002C5 RID: 709
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x0600125A RID: 4698 RVA: 0x00063168 File Offset: 0x00061368
		public string GetParameterValue(string ParamName)
		{
			if (this._Params != null)
			{
				int i = 0;
				int count = this._Params.Count;
				while (i < count)
				{
					if (this._Params[i].Name == ParamName)
					{
						return this._Params[i].Value;
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000631C4 File Offset: 0x000613C4
		public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
		{
			bool flag = false;
			int i = 0;
			int count = this._Params.Count;
			while (i < count)
			{
				if (this._Params[i].Name == ParamName)
				{
					LocalizationParamsManager.ParamValue paramValue = this._Params[i];
					paramValue.Value = ParamValue;
					this._Params[i] = paramValue;
					flag = true;
					break;
				}
				i++;
			}
			if (!flag)
			{
				this._Params.Add(new LocalizationParamsManager.ParamValue
				{
					Name = ParamName,
					Value = ParamValue
				});
			}
			if (localize)
			{
				this.OnLocalize();
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x0006325C File Offset: 0x0006145C
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00063280 File Offset: 0x00061480
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00063290 File Offset: 0x00061490
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000632B0 File Offset: 0x000614B0
		public void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();

		// Token: 0x04000E93 RID: 3731
		public bool _IsGlobalManager;

		// Token: 0x0200049B RID: 1179
		[Serializable]
		public struct ParamValue
		{
			// Token: 0x04001769 RID: 5993
			public string Name;

			// Token: 0x0400176A RID: 5994
			public string Value;
		}
	}
}
