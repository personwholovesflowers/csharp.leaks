using System;

namespace SettingsMenu.Models
{
	// Token: 0x0200052A RID: 1322
	[Serializable]
	public struct PreferenceKey
	{
		// Token: 0x06001E1A RID: 7706 RVA: 0x000FA387 File Offset: 0x000F8587
		public readonly bool IsValid()
		{
			return !string.IsNullOrEmpty(this.key);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000FA397 File Offset: 0x000F8597
		public readonly bool GetBoolValue(bool fallbackValue = false)
		{
			if (!this.isLocal)
			{
				return MonoSingleton<PrefsManager>.Instance.GetBool(this.key, fallbackValue);
			}
			return MonoSingleton<PrefsManager>.Instance.GetBoolLocal(this.key, fallbackValue);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000FA3C4 File Offset: 0x000F85C4
		public readonly int GetIntValue(int fallbackValue = 0)
		{
			if (!this.isLocal)
			{
				return MonoSingleton<PrefsManager>.Instance.GetInt(this.key, fallbackValue);
			}
			return MonoSingleton<PrefsManager>.Instance.GetIntLocal(this.key, fallbackValue);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000FA3F1 File Offset: 0x000F85F1
		public readonly float GetFloatValue(float fallbackValue = 0f)
		{
			if (!this.isLocal)
			{
				return MonoSingleton<PrefsManager>.Instance.GetFloat(this.key, fallbackValue);
			}
			return MonoSingleton<PrefsManager>.Instance.GetFloatLocal(this.key, fallbackValue);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000FA41E File Offset: 0x000F861E
		public readonly string GetStringValue(string fallbackValue = "")
		{
			if (!this.isLocal)
			{
				return MonoSingleton<PrefsManager>.Instance.GetString(this.key, fallbackValue);
			}
			return MonoSingleton<PrefsManager>.Instance.GetStringLocal(this.key, fallbackValue);
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000FA44B File Offset: 0x000F864B
		public readonly void SetBoolValue(bool value)
		{
			if (this.isLocal)
			{
				MonoSingleton<PrefsManager>.Instance.SetBoolLocal(this.key, value);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetBool(this.key, value);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000FA478 File Offset: 0x000F8678
		public readonly void SetIntValue(int value)
		{
			if (this.isLocal)
			{
				MonoSingleton<PrefsManager>.Instance.SetIntLocal(this.key, value);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetInt(this.key, value);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000FA4A5 File Offset: 0x000F86A5
		public readonly void SetFloatValue(float value)
		{
			if (this.isLocal)
			{
				MonoSingleton<PrefsManager>.Instance.SetFloatLocal(this.key, value);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetFloat(this.key, value);
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000FA4D2 File Offset: 0x000F86D2
		public readonly void SetStringValue(string value)
		{
			if (this.isLocal)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal(this.key, value);
				return;
			}
			MonoSingleton<PrefsManager>.Instance.SetString(this.key, value);
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000FA500 File Offset: 0x000F8700
		public readonly void SetValue<T>(T value)
		{
			if (value is bool)
			{
				bool flag = value as bool;
				this.SetBoolValue(flag);
				return;
			}
			if (value is int)
			{
				int num = value as int;
				this.SetIntValue(num);
				return;
			}
			if (value is float)
			{
				float num2 = value as float;
				this.SetFloatValue(num2);
				return;
			}
			string text = value as string;
			if (text == null)
			{
				throw new ArgumentException("Unsupported value type");
			}
			this.SetStringValue(text);
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000FA5A8 File Offset: 0x000F87A8
		public override string ToString()
		{
			if (this.isLocal)
			{
				return "(Local)" + this.key;
			}
			return this.key;
		}

		// Token: 0x04002A8D RID: 10893
		public string key;

		// Token: 0x04002A8E RID: 10894
		public bool isLocal;
	}
}
