using System;
using System.Collections.Generic;
using System.Text;
using SettingsMenu.Models;
using UnityEngine;
using UnityEngine.Events;

namespace SettingsMenu.Components.Pages
{
	// Token: 0x0200054A RID: 1354
	public class GraphicsSettings : SettingsLogicBase
	{
		// Token: 0x06001E86 RID: 7814 RVA: 0x000FBFF0 File Offset: 0x000FA1F0
		public override void Initialize(SettingsMenu settingsMenu)
		{
			this.settingsMenu = settingsMenu;
			int intLocal = MonoSingleton<PrefsManager>.Instance.GetIntLocal("frameRateLimit", 0);
			this.SetFrameRateLimit(intLocal);
			Screen.fullScreen = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("fullscreen", false);
			bool boolLocal = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("vSync", false);
			this.SetVSync(boolLocal);
			bool boolLocal2 = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("simpleExplosions", false);
			this.SetSimpleExplosions(boolLocal2);
			int @int = MonoSingleton<PrefsManager>.Instance.GetInt("simplifyEnemies", 0);
			this.SetSimplifyEnemies(@int);
			float @float = MonoSingleton<PrefsManager>.Instance.GetFloat("dithering", 0f);
			this.SetDithering(@float);
			float float2 = MonoSingleton<PrefsManager>.Instance.GetFloat("textureWarping", 0f);
			this.SetTextureWarping(float2);
			int int2 = MonoSingleton<PrefsManager>.Instance.GetInt("vertexWarping", 0);
			this.SetVertexWarping(int2);
			int int3 = MonoSingleton<PrefsManager>.Instance.GetInt("colorCompression", 0);
			this.SetColorCompression(int3);
			float float3 = MonoSingleton<PrefsManager>.Instance.GetFloat("gamma", 0f);
			this.SetGamma(float3);
			bool boolLocal3 = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("colorPalette", false);
			this.SetColorPalette(boolLocal3);
			GraphicsSettings.bloodEnabled = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("bloodEnabled", false);
			GraphicsSettings.disabledComputeShaders = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("disabledComputeShaders", false);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x000FC14B File Offset: 0x000FA34B
		private void Start()
		{
			this.InitializeResolutionDropdown();
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x000FC154 File Offset: 0x000FA354
		private void SetColorPalette(bool value)
		{
			if (!value)
			{
				MonoSingleton<PostProcessV2_Handler>.Instance.ColorPalette(false);
				return;
			}
			try
			{
				Texture2D texture2D = CustomPaletteSelector.LoadSavedPalette();
				if (texture2D)
				{
					MonoSingleton<PostProcessV2_Handler>.Instance.ApplyUserColorPalette(texture2D);
					MonoSingleton<PostProcessV2_Handler>.Instance.ColorPalette(true);
					Shader.SetGlobalInt("_ColorPrecision", 2048);
				}
				else
				{
					MonoSingleton<PostProcessV2_Handler>.Instance.ColorPalette(false);
				}
			}
			catch (Exception ex)
			{
				string text = "Error loading color palette: ";
				Exception ex2 = ex;
				Debug.LogError(text + ((ex2 != null) ? ex2.ToString() : null));
				MonoSingleton<PostProcessV2_Handler>.Instance.ColorPalette(false);
			}
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x000FC1F0 File Offset: 0x000FA3F0
		private void InitializeResolutionDropdown()
		{
			this.GetAvailableResolutions();
			SettingsDropdown settingsDropdown;
			if (this.resolutionItem == null || !this.settingsMenu.TryGetItemBuilderInstance<SettingsDropdown>(this.resolutionItem, out settingsDropdown))
			{
				return;
			}
			settingsDropdown.SetDropdownItems(this.availableResolutions.ConvertAll<string>((ValueTuple<Resolution, string> x) => x.Item2), true);
			settingsDropdown.SetDropdownValue(this.currentResolutionIndex, false);
			settingsDropdown.onValueChanged.AddListener(new UnityAction<int>(this.SetResolution));
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x000FC27C File Offset: 0x000FA47C
		public override void OnPrefChanged(string key, object value)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
			if (num <= 2954314656U)
			{
				if (num <= 1526030146U)
				{
					if (num != 373840175U)
					{
						if (num != 1345518424U)
						{
							if (num != 1526030146U)
							{
								return;
							}
							if (!(key == "colorCompression"))
							{
								return;
							}
							if (value is int)
							{
								int num2 = (int)value;
								this.SetColorCompression(num2);
								return;
							}
						}
						else
						{
							if (!(key == "fullscreen"))
							{
								return;
							}
							if (value is bool)
							{
								bool flag = (bool)value;
								Screen.fullScreen = flag;
								return;
							}
						}
					}
					else
					{
						if (!(key == "simpleExplosions"))
						{
							return;
						}
						if (value is bool)
						{
							bool flag2 = (bool)value;
							this.SetSimpleExplosions(flag2);
							return;
						}
					}
				}
				else if (num <= 2267364486U)
				{
					if (num != 2086904318U)
					{
						if (num != 2267364486U)
						{
							return;
						}
						if (!(key == "vSync"))
						{
							return;
						}
						if (value is bool)
						{
							bool flag3 = (bool)value;
							this.SetVSync(flag3);
							return;
						}
					}
					else
					{
						if (!(key == "simplifyEnemies"))
						{
							return;
						}
						if (value is int)
						{
							int num3 = (int)value;
							this.SetSimplifyEnemies(num3);
							return;
						}
					}
				}
				else if (num != 2870559238U)
				{
					if (num != 2954314656U)
					{
						return;
					}
					if (!(key == "disabledComputeShaders"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag4 = (bool)value;
						GraphicsSettings.disabledComputeShaders = flag4;
					}
				}
				else
				{
					if (!(key == "textureWarping"))
					{
						return;
					}
					if (value is float)
					{
						float num4 = (float)value;
						this.SetTextureWarping(num4);
						return;
					}
				}
			}
			else if (num <= 3569326331U)
			{
				if (num != 3469647222U)
				{
					if (num != 3492353034U)
					{
						if (num != 3569326331U)
						{
							return;
						}
						if (!(key == "dithering"))
						{
							return;
						}
						if (value is float)
						{
							float num5 = (float)value;
							this.SetDithering(num5);
							return;
						}
					}
					else
					{
						if (!(key == "gamma"))
						{
							return;
						}
						if (value is float)
						{
							float num6 = (float)value;
							this.SetGamma(num6);
							return;
						}
					}
				}
				else
				{
					if (!(key == "colorPaletteTexture"))
					{
						return;
					}
					this.SetColorPalette(MonoSingleton<PrefsManager>.Instance.GetBoolLocal("colorPalette", false));
					return;
				}
			}
			else if (num <= 4171980119U)
			{
				if (num != 3824007127U)
				{
					if (num != 4171980119U)
					{
						return;
					}
					if (!(key == "vertexWarping"))
					{
						return;
					}
					if (value is int)
					{
						int num7 = (int)value;
						this.SetVertexWarping(num7);
						return;
					}
				}
				else
				{
					if (!(key == "frameRateLimit"))
					{
						return;
					}
					if (value is int)
					{
						int num8 = (int)value;
						this.SetFrameRateLimit(num8);
						return;
					}
				}
			}
			else if (num != 4186093404U)
			{
				if (num != 4264400951U)
				{
					return;
				}
				if (!(key == "colorPalette"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag5 = (bool)value;
					this.SetColorPalette(flag5);
					return;
				}
			}
			else
			{
				if (!(key == "bloodEnabled"))
				{
					return;
				}
				if (value is bool)
				{
					bool flag6 = (bool)value;
					GraphicsSettings.bloodEnabled = flag6;
					return;
				}
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000FC5D4 File Offset: 0x000FA7D4
		private void SetGamma(float value)
		{
			Shader.SetGlobalFloat("_Gamma", value);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x000FC5E1 File Offset: 0x000FA7E1
		private void SetColorCompression(int value)
		{
			if (MonoSingleton<PrefsManager>.Instance.GetBoolLocal("colorPalette", false))
			{
				return;
			}
			Shader.SetGlobalFloat("_ColorPrecision", (float)GraphicsSettings.GetColorCompressionValue(value));
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x000FC608 File Offset: 0x000FA808
		public static float GetPixelizationValue(int option)
		{
			float num;
			switch (option)
			{
			case 0:
				num = 0f;
				break;
			case 1:
				num = 720f;
				break;
			case 2:
				num = 480f;
				break;
			case 3:
				num = 360f;
				break;
			case 4:
				num = 240f;
				break;
			case 5:
				num = 144f;
				break;
			case 6:
				num = 36f;
				break;
			default:
				num = 0f;
				break;
			}
			return num;
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000FC678 File Offset: 0x000FA878
		public static int GetColorCompressionValue(int option)
		{
			int num;
			switch (option)
			{
			case 0:
				num = 2048;
				break;
			case 1:
				num = 64;
				break;
			case 2:
				num = 32;
				break;
			case 3:
				num = 16;
				break;
			case 4:
				num = 8;
				break;
			case 5:
				num = 3;
				break;
			default:
				num = 2048;
				break;
			}
			return num;
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x000FC6CC File Offset: 0x000FA8CC
		public static float GetVertexWarpingValue(int option)
		{
			int num;
			switch (option)
			{
			case 0:
				num = 0;
				break;
			case 1:
				num = 400;
				break;
			case 2:
				num = 160;
				break;
			case 3:
				num = 80;
				break;
			case 4:
				num = 40;
				break;
			case 5:
				num = 16;
				break;
			default:
				num = 0;
				break;
			}
			return (float)num;
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x000FC720 File Offset: 0x000FA920
		private void SetTextureWarping(float value)
		{
			Shader.SetGlobalFloat("_TextureWarping", value);
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x000FC72D File Offset: 0x000FA92D
		private void SetVertexWarping(int value)
		{
			Shader.SetGlobalFloat("_StainWarping", (float)value);
			Shader.SetGlobalFloat("_VertexWarping", GraphicsSettings.GetVertexWarpingValue(value));
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x000FC74B File Offset: 0x000FA94B
		private void SetDithering(float value)
		{
			Shader.SetGlobalFloat("_DitherStrength", value);
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x000FC758 File Offset: 0x000FA958
		private void GetAvailableResolutions()
		{
			Resolution[] resolutions = Screen.resolutions;
			this.availableResolutions.Clear();
			this.currentResolutionIndex = 0;
			HashSet<ValueTuple<int, int>> hashSet = new HashSet<ValueTuple<int, int>>();
			StringBuilder stringBuilder = new StringBuilder(16);
			foreach (Resolution resolution in resolutions)
			{
				if (hashSet.Add(new ValueTuple<int, int>(resolution.width, resolution.height)))
				{
					stringBuilder.Clear();
					stringBuilder.Append(resolution.width).Append(" x ").Append(resolution.height);
					this.availableResolutions.Add(new ValueTuple<Resolution, string>(resolution, stringBuilder.ToString()));
					if (resolution.width == Screen.width && resolution.height == Screen.height)
					{
						this.currentResolutionIndex = this.availableResolutions.Count - 1;
					}
				}
			}
			this.availableResolutions.Sort(delegate(ValueTuple<Resolution, string> a, ValueTuple<Resolution, string> b)
			{
				int num = a.Item1.width.CompareTo(b.Item1.width);
				if (num == 0)
				{
					return a.Item1.height.CompareTo(b.Item1.height);
				}
				return num;
			});
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x000FC860 File Offset: 0x000FAA60
		public void SetResolution(int stuff)
		{
			Resolution item = this.availableResolutions[stuff].Item1;
			Screen.SetResolution(item.width, item.height, Screen.fullScreen);
			MonoSingleton<PrefsManager>.Instance.SetIntLocal("resolutionWidth", item.width);
			MonoSingleton<PrefsManager>.Instance.SetIntLocal("resolutionHeight", item.height);
			MonoSingleton<PrefsManager>.Instance.SetBoolLocal("fullscreen", Screen.fullScreen);
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x000FC8D8 File Offset: 0x000FAAD8
		private void SetFrameRateLimit(int stepValue)
		{
			int num;
			switch (stepValue)
			{
			case 0:
				num = -1;
				break;
			case 1:
				num = (int)(Screen.currentResolution.refreshRateRatio.value * 2.0);
				break;
			case 2:
				num = 30;
				break;
			case 3:
				num = 60;
				break;
			case 4:
				num = 120;
				break;
			case 5:
				num = 144;
				break;
			case 6:
				num = 240;
				break;
			case 7:
				num = 288;
				break;
			default:
				num = Application.targetFrameRate;
				break;
			}
			Application.targetFrameRate = num;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x000FC967 File Offset: 0x000FAB67
		private void SetVSync(bool value)
		{
			QualitySettings.vSyncCount = (value ? 1 : 0);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x000FC975 File Offset: 0x000FAB75
		private void SetSimpleExplosions(bool value)
		{
			Physics.IgnoreLayerCollision(23, 9, value);
			Physics.IgnoreLayerCollision(23, 27, value);
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00004AE3 File Offset: 0x00002CE3
		private void SetSimplifyEnemies(int value)
		{
		}

		// Token: 0x04002B0B RID: 11019
		[SerializeField]
		private SettingsItem resolutionItem;

		// Token: 0x04002B0C RID: 11020
		public static bool simpleNailPhysics = true;

		// Token: 0x04002B0D RID: 11021
		public static bool bloodEnabled = true;

		// Token: 0x04002B0E RID: 11022
		private SettingsMenu settingsMenu;

		// Token: 0x04002B0F RID: 11023
		private int currentResolutionIndex;

		// Token: 0x04002B10 RID: 11024
		private readonly List<ValueTuple<Resolution, string>> availableResolutions = new List<ValueTuple<Resolution, string>>();

		// Token: 0x04002B11 RID: 11025
		public static bool disabledComputeShaders;
	}
}
