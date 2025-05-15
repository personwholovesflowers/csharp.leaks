using System;
using UnityEngine;

// Token: 0x0200035A RID: 858
public static class PrefsManagerHelper
{
	// Token: 0x060013FD RID: 5117 RVA: 0x0009FEA8 File Offset: 0x0009E0A8
	public static void SetPref(string key, object value, bool noSteamCloud)
	{
		if (value is float)
		{
			float num = (float)value;
			if (noSteamCloud)
			{
				MonoSingleton<PrefsManager>.Instance.SetFloatLocal(key, num);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetFloat(key, num);
			return;
		}
		else if (value is int)
		{
			int num2 = (int)value;
			if (noSteamCloud)
			{
				MonoSingleton<PrefsManager>.Instance.SetIntLocal(key, num2);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetInt(key, num2);
			return;
		}
		else
		{
			string text = value as string;
			if (text != null)
			{
				if (noSteamCloud)
				{
					MonoSingleton<PrefsManager>.Instance.SetStringLocal(key, text);
					return;
				}
				MonoSingleton<PrefsManager>.Instance.SetString(key, text);
				return;
			}
			else
			{
				if (!(value is bool))
				{
					Debug.LogError("Unsupported type for PrefsManagerHelper.SetPref");
					return;
				}
				bool flag = (bool)value;
				if (noSteamCloud)
				{
					MonoSingleton<PrefsManager>.Instance.SetBoolLocal(key, flag);
					return;
				}
				MonoSingleton<PrefsManager>.Instance.SetBool(key, flag);
				return;
			}
		}
	}
}
