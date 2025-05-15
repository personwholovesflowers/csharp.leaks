using System;
using TMPro;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class PlatformSettings : MonoBehaviour
{
	// Token: 0x060007AA RID: 1962 RVA: 0x00026D1D File Offset: 0x00024F1D
	private void SetToDebug_PS4()
	{
		this.SetToDebug("ps4");
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00026D2A File Offset: 0x00024F2A
	private void SetToDebug_PS5()
	{
		this.SetToDebug("ps5");
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00026D37 File Offset: 0x00024F37
	private void SetToDebug_XBOX()
	{
		this.SetToDebug("xbox");
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00026D44 File Offset: 0x00024F44
	private void SetToDebug_Switch()
	{
		this.SetToDebug("switch");
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00026D51 File Offset: 0x00024F51
	private void SetToDebug_Standalone()
	{
		this.SetToDebug("standalone");
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x00026D5E File Offset: 0x00024F5E
	private void SetToDebug_Actual()
	{
		this.SetToDebug("");
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00026D6B File Offset: 0x00024F6B
	// (set) Token: 0x060007B1 RID: 1969 RVA: 0x00026D72 File Offset: 0x00024F72
	public static PlatformSettings Instance { get; private set; }

	// Token: 0x060007B2 RID: 1970 RVA: 0x00026D7C File Offset: 0x00024F7C
	public TMP_SpriteAsset GetSpriteSheetForInputDevice(GameMaster.InputDevice inputDevice)
	{
		RuntimePlatform runtimePlatformWithDebugIfInEditor = this.GetRuntimePlatformWithDebugIfInEditor();
		if (runtimePlatformWithDebugIfInEditor != RuntimePlatform.PS4)
		{
			if (runtimePlatformWithDebugIfInEditor != RuntimePlatform.XboxOne)
			{
				switch (runtimePlatformWithDebugIfInEditor)
				{
				case RuntimePlatform.Switch:
					return this.switchControllerSprites;
				case RuntimePlatform.GameCoreScarlett:
				case RuntimePlatform.GameCoreXboxOne:
					goto IL_003F;
				case RuntimePlatform.PS5:
					goto IL_0038;
				}
				switch (inputDevice)
				{
				default:
					return this.keyboardSprites;
				case GameMaster.InputDevice.GamepadXBOXOneOrGeneric:
					return this.xboxControllerSprites;
				case GameMaster.InputDevice.GamepadPS4:
				case GameMaster.InputDevice.GamepadPS5:
					return this.playstationControllerSprites;
				case GameMaster.InputDevice.GamepadSwitch:
					return this.switchControllerSprites;
				}
			}
			IL_003F:
			return this.xboxControllerSprites;
		}
		IL_0038:
		return this.playstationControllerSprites;
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x060007B3 RID: 1971 RVA: 0x00026E0F File Offset: 0x0002500F
	// (set) Token: 0x060007B4 RID: 1972 RVA: 0x00026E16 File Offset: 0x00025016
	public static RuntimePlatform debugSafeRuntimePlatform { get; private set; }

	// Token: 0x060007B5 RID: 1973 RVA: 0x00026E1E File Offset: 0x0002501E
	private void SetToDebug(string platform)
	{
		this.debugPlatformInEditor.Init();
		this.debugPlatformInEditor.SetValue(platform);
		this.debugPlatformInEditor.SaveToDiskAll();
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00026E44 File Offset: 0x00025044
	private void Awake()
	{
		PlatformSettings.Instance = this;
		this.isStandalone.Init();
		this.isConsole.Init();
		this.isSwitch.Init();
		this.isPlaystation4.Init();
		this.isPlaystation5.Init();
		this.isPlaystationAny.Init();
		this.isXBOX.Init();
		this.debugPlatformInEditor.Init();
		this.isStandalone.SaveToDiskAll();
		this.isConsole.SaveToDiskAll();
		this.isSwitch.SaveToDiskAll();
		this.isPlaystation4.SaveToDiskAll();
		this.isPlaystation5.SaveToDiskAll();
		this.isPlaystationAny.SaveToDiskAll();
		this.isXBOX.SaveToDiskAll();
		this.debugPlatformInEditor.SaveToDiskAll();
		PlatformSettings.debugSafeRuntimePlatform = Application.platform;
		this.CheckPlatforms();
		StringConfigurable stringConfigurable = this.debugPlatformInEditor;
		stringConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(stringConfigurable.OnValueChanged, new Action<LiveData>(this.CheckPlatforms));
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00026F3E File Offset: 0x0002513E
	private void OnDestroy()
	{
		StringConfigurable stringConfigurable = this.debugPlatformInEditor;
		stringConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Remove(stringConfigurable.OnValueChanged, new Action<LiveData>(this.CheckPlatforms));
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00026F67 File Offset: 0x00025167
	private void Start()
	{
		this.CheckPlatforms();
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00026F70 File Offset: 0x00025170
	public RuntimePlatform GetRuntimePlatformWithDebugIfInEditor()
	{
		RuntimePlatform runtimePlatform = Application.platform;
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			string text = this.debugPlatformInEditor.GetStringValue().ToLower();
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1582198420U)
			{
				if (num <= 360195552U)
				{
					if (num != 323435638U)
					{
						if (num != 360195552U)
						{
							return runtimePlatform;
						}
						if (!(text == "ps4"))
						{
							return runtimePlatform;
						}
					}
					else
					{
						if (!(text == "standalone"))
						{
							return runtimePlatform;
						}
						return RuntimePlatform.WindowsPlayer;
					}
				}
				else if (num != 376973171U)
				{
					if (num != 1582198420U)
					{
						return runtimePlatform;
					}
					if (!(text == "ps"))
					{
						return runtimePlatform;
					}
				}
				else
				{
					if (!(text == "ps5"))
					{
						return runtimePlatform;
					}
					goto IL_0142;
				}
			}
			else if (num <= 2217645753U)
			{
				if (num != 2144789592U)
				{
					if (num != 2217645753U)
					{
						return runtimePlatform;
					}
					if (!(text == "playstation"))
					{
						return runtimePlatform;
					}
				}
				else
				{
					if (!(text == "xbox"))
					{
						return runtimePlatform;
					}
					return RuntimePlatform.GameCoreScarlett;
				}
			}
			else if (num != 2480955249U)
			{
				if (num != 2706832996U)
				{
					if (num != 2723610615U)
					{
						return runtimePlatform;
					}
					if (!(text == "playstation4"))
					{
						return runtimePlatform;
					}
				}
				else
				{
					if (!(text == "playstation5"))
					{
						return runtimePlatform;
					}
					goto IL_0142;
				}
			}
			else
			{
				if (!(text == "switch"))
				{
					return runtimePlatform;
				}
				return RuntimePlatform.Switch;
			}
			return RuntimePlatform.PS4;
			IL_0142:
			runtimePlatform = RuntimePlatform.PS5;
		}
		return runtimePlatform;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00026F67 File Offset: 0x00025167
	private void CheckPlatforms(LiveData d)
	{
		this.CheckPlatforms();
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x000270D4 File Offset: 0x000252D4
	[ContextMenu("Check Platforms")]
	private void CheckPlatforms()
	{
		RuntimePlatform runtimePlatformWithDebugIfInEditor = this.GetRuntimePlatformWithDebugIfInEditor();
		bool flag = true;
		if (runtimePlatformWithDebugIfInEditor <= RuntimePlatform.LinuxPlayer)
		{
			if (runtimePlatformWithDebugIfInEditor > RuntimePlatform.WindowsPlayer && runtimePlatformWithDebugIfInEditor != RuntimePlatform.WindowsEditor && runtimePlatformWithDebugIfInEditor != RuntimePlatform.LinuxPlayer)
			{
				goto IL_00A0;
			}
		}
		else
		{
			if (runtimePlatformWithDebugIfInEditor > RuntimePlatform.PS4)
			{
				if (runtimePlatformWithDebugIfInEditor != RuntimePlatform.XboxOne)
				{
					switch (runtimePlatformWithDebugIfInEditor)
					{
					case RuntimePlatform.Switch:
						this.SetConfigurables(false, false, false, false, true);
						goto IL_00A0;
					case RuntimePlatform.Lumin:
					case RuntimePlatform.Stadia:
					case RuntimePlatform.CloudRendering:
						goto IL_00A0;
					case RuntimePlatform.GameCoreScarlett:
					case RuntimePlatform.GameCoreXboxOne:
						break;
					case RuntimePlatform.PS5:
						this.SetConfigurables(false, false, true, false, false);
						flag = false;
						goto IL_00A0;
					default:
						goto IL_00A0;
					}
				}
				this.SetConfigurables(false, false, false, true, false);
				goto IL_00A0;
			}
			if (runtimePlatformWithDebugIfInEditor != RuntimePlatform.LinuxEditor)
			{
				if (runtimePlatformWithDebugIfInEditor != RuntimePlatform.PS4)
				{
					goto IL_00A0;
				}
				this.SetConfigurables(false, true, false, false, false);
				flag = false;
				goto IL_00A0;
			}
		}
		this.SetConfigurables(true, false, false, false, false);
		IL_00A0:
		PlatformSettings.debugSafeRuntimePlatform = runtimePlatformWithDebugIfInEditor;
		this.calledAchievements.SetValue(flag);
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x00027194 File Offset: 0x00025394
	private void SetConfigurables(bool standalone = false, bool ps4 = false, bool ps5 = false, bool xbox = false, bool swit = false)
	{
		this.isStandalone.SetValue(standalone);
		this.isConsole.SetValue(ps4 || ps5 || xbox || swit);
		this.isPlaystationAny.SetValue(ps4 || ps5);
		this.isPlaystation4.SetValue(ps4);
		this.isPlaystation5.SetValue(ps5);
		this.isXBOX.SetValue(xbox);
		this.isSwitch.SetValue(swit);
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00027201 File Offset: 0x00025401
	public static StanleyPlatform GetCurrentRunningPlatform()
	{
		return PlatformSettings.GetStanleyPlatform(PlatformSettings.debugSafeRuntimePlatform);
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00027210 File Offset: 0x00025410
	public static StanleyPlatform GetStanleyPlatform(RuntimePlatform runtimePlatform)
	{
		if (runtimePlatform <= RuntimePlatform.LinuxPlayer)
		{
			if (runtimePlatform <= RuntimePlatform.WindowsEditor)
			{
				if (runtimePlatform > RuntimePlatform.WindowsPlayer && runtimePlatform != RuntimePlatform.WindowsEditor)
				{
					return StanleyPlatform.NoVariation;
				}
			}
			else
			{
				if (runtimePlatform == RuntimePlatform.XBOX360)
				{
					return StanleyPlatform.XBOX360;
				}
				if (runtimePlatform != RuntimePlatform.LinuxPlayer)
				{
					return StanleyPlatform.NoVariation;
				}
			}
		}
		else
		{
			if (runtimePlatform > RuntimePlatform.PS4)
			{
				if (runtimePlatform != RuntimePlatform.XboxOne)
				{
					switch (runtimePlatform)
					{
					case RuntimePlatform.Switch:
						return StanleyPlatform.Switch;
					case RuntimePlatform.Lumin:
					case RuntimePlatform.Stadia:
					case RuntimePlatform.CloudRendering:
						return StanleyPlatform.NoVariation;
					case RuntimePlatform.GameCoreScarlett:
					case RuntimePlatform.GameCoreXboxOne:
						break;
					case RuntimePlatform.PS5:
						return StanleyPlatform.Playstation5;
					default:
						return StanleyPlatform.NoVariation;
					}
				}
				return StanleyPlatform.XBOXone;
			}
			if (runtimePlatform != RuntimePlatform.LinuxEditor)
			{
				if (runtimePlatform != RuntimePlatform.PS4)
				{
					return StanleyPlatform.NoVariation;
				}
				return StanleyPlatform.Playstation4;
			}
		}
		return StanleyPlatform.PC;
	}

	// Token: 0x040007C7 RID: 1991
	[InspectorButton("SetToDebug_Standalone", null)]
	public BooleanConfigurable isStandalone;

	// Token: 0x040007C8 RID: 1992
	[InspectorButton("SetToDebug_Actual", null)]
	public BooleanConfigurable isConsole;

	// Token: 0x040007C9 RID: 1993
	[InspectorButton("SetToDebug_Switch", null)]
	public BooleanConfigurable isSwitch;

	// Token: 0x040007CA RID: 1994
	public BooleanConfigurable isPlaystationAny;

	// Token: 0x040007CB RID: 1995
	[InspectorButton("SetToDebug_PS4", null)]
	public BooleanConfigurable isPlaystation4;

	// Token: 0x040007CC RID: 1996
	[InspectorButton("SetToDebug_PS5", null)]
	public BooleanConfigurable isPlaystation5;

	// Token: 0x040007CD RID: 1997
	[InspectorButton("SetToDebug_XBOX", null)]
	public BooleanConfigurable isXBOX;

	// Token: 0x040007CE RID: 1998
	public BooleanConfigurable calledAchievements;

	// Token: 0x040007CF RID: 1999
	public StringConfigurable debugPlatformInEditor;

	// Token: 0x040007D0 RID: 2000
	[Header("Sprite Sheet Data")]
	public TMP_SpriteAsset keyboardSprites;

	// Token: 0x040007D1 RID: 2001
	public TMP_SpriteAsset xboxControllerSprites;

	// Token: 0x040007D2 RID: 2002
	public TMP_SpriteAsset xboxSeXControllerSprites;

	// Token: 0x040007D3 RID: 2003
	public TMP_SpriteAsset playstationControllerSprites;

	// Token: 0x040007D4 RID: 2004
	public TMP_SpriteAsset switchControllerSprites;
}
