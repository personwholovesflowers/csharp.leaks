using System;
using UnityEngine;
using UnityEngine.UI;

namespace SettingsMenu.Components.Pages
{
	// Token: 0x0200054C RID: 1356
	public class HUDSettings : SettingsLogicBase
	{
		// Token: 0x06001E9F RID: 7839 RVA: 0x000FCA14 File Offset: 0x000FAC14
		public override void Initialize(SettingsMenu settingsMenu)
		{
			this.hudCons = Object.FindObjectsOfType<HudController>();
			foreach (HudController hudController in this.hudCons)
			{
				if (!hudController.altHud)
				{
					this.masks = hudController.GetComponentsInChildren<Mask>(true);
					break;
				}
			}
			bool @bool = MonoSingleton<PrefsManager>.Instance.GetBool("hudAlwaysOnTop", false);
			this.AlwaysOnTop(@bool);
			bool bool2 = MonoSingleton<PrefsManager>.Instance.GetBool("powerUpMeter", false);
			this.SetPowerUpMeter(bool2);
			HUDSettings.weaponIconEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("weaponIcons", false);
			HUDSettings.railcannonMeterEnabled = MonoSingleton<PrefsManager>.Instance.GetBool("railcannonMeter", false);
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000FCABC File Offset: 0x000FACBC
		public override void OnPrefChanged(string key, object value)
		{
			if (!(key == "hudAlwaysOnTop"))
			{
				if (!(key == "powerUpMeter"))
				{
					if (!(key == "weaponIcons"))
					{
						if (!(key == "railcannonMeter"))
						{
							return;
						}
						HUDSettings.railcannonMeterEnabled = (bool)value;
						RailcannonMeter instance = MonoSingleton<RailcannonMeter>.Instance;
						if (instance == null)
						{
							return;
						}
						instance.CheckStatus();
					}
					else
					{
						HUDSettings.weaponIconEnabled = (bool)value;
						RailcannonMeter instance2 = MonoSingleton<RailcannonMeter>.Instance;
						if (instance2 == null)
						{
							return;
						}
						instance2.CheckStatus();
						return;
					}
				}
				else if (value is bool)
				{
					bool flag = (bool)value;
					this.SetPowerUpMeter(flag);
					return;
				}
			}
			else if (value is bool)
			{
				bool flag2 = (bool)value;
				this.AlwaysOnTop(flag2);
				return;
			}
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000FCB63 File Offset: 0x000FAD63
		private void SetPowerUpMeter(bool value)
		{
			HUDSettings.powerUpMeterEnabled = value;
			if (MonoSingleton<PowerUpMeter>.Instance)
			{
				MonoSingleton<PowerUpMeter>.Instance.UpdateMeter();
			}
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000FCB84 File Offset: 0x000FAD84
		private void AlwaysOnTop(bool stuff)
		{
			if (stuff)
			{
				this.hudMaterial.SetFloat("_ZTest", 8f);
				this.hudTextMaterial.SetFloat("_ZTest", 8f);
			}
			else
			{
				this.hudMaterial.SetFloat("_ZTest", 4f);
				this.hudTextMaterial.SetFloat("_ZTest", 4f);
			}
			if (this.masks != null)
			{
				foreach (Mask mask in this.masks)
				{
					if (mask.enabled)
					{
						mask.enabled = false;
						mask.enabled = true;
					}
				}
			}
		}

		// Token: 0x04002B15 RID: 11029
		public static bool powerUpMeterEnabled;

		// Token: 0x04002B16 RID: 11030
		public static bool railcannonMeterEnabled;

		// Token: 0x04002B17 RID: 11031
		public static bool weaponIconEnabled;

		// Token: 0x04002B18 RID: 11032
		public Material hudMaterial;

		// Token: 0x04002B19 RID: 11033
		public Material hudTextMaterial;

		// Token: 0x04002B1A RID: 11034
		private HudController[] hudCons;

		// Token: 0x04002B1B RID: 11035
		private Mask[] masks;
	}
}
