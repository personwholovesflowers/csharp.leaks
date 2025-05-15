using System;
using System.Collections.Generic;
using I2.Loc;
using InControl;

// Token: 0x020001A1 RID: 417
public class StanleyActions : PlayerActionSet
{
	// Token: 0x06000975 RID: 2421 RVA: 0x0002C6A3 File Offset: 0x0002A8A3
	public PlayerAction ExtraAction(int index, bool overrideSimpleControls = false)
	{
		if (!Singleton<GameMaster>.Instance.UsingSimplifiedControls || overrideSimpleControls)
		{
			return this.ExtraActions[index];
		}
		return this.UseAction;
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000976 RID: 2422 RVA: 0x0002C6C3 File Offset: 0x0002A8C3
	public PlayerAction UseAction
	{
		get
		{
			if (Singleton<GameMaster>.Instance.InputDeviceType == GameMaster.InputDevice.KeyboardAndMouse || !Singleton<GameMaster>.Instance.UsingSimplifiedControls)
			{
				return this.Use;
			}
			return this.AnyButton;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002C6EA File Offset: 0x0002A8EA
	public PlayerAction JumpAction
	{
		get
		{
			if (!Singleton<GameMaster>.Instance.UsingSimplifiedControls)
			{
				return this.Jump;
			}
			return this.UseAction;
		}
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000978 RID: 2424 RVA: 0x0002C708 File Offset: 0x0002A908
	// (remove) Token: 0x06000979 RID: 2425 RVA: 0x0002C740 File Offset: 0x0002A940
	public event Action OnKeyBindingsSaved;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x0600097A RID: 2426 RVA: 0x0002C778 File Offset: 0x0002A978
	// (remove) Token: 0x0600097B RID: 2427 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
	public event Action OnKeyBindingsLoaded;

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600097C RID: 2428 RVA: 0x0002C7E5 File Offset: 0x0002A9E5
	public PlayerAction HoleTeleportAction
	{
		get
		{
			if (!Singleton<GameMaster>.Instance.UsingSimplifiedControls)
			{
				return this.ExtraActions[StanleyActions.HoleTeleportIndex];
			}
			return this.UseAction;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600097D RID: 2429 RVA: 0x0002C806 File Offset: 0x0002AA06
	public static int HoleTeleportIndex
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0002C809 File Offset: 0x0002AA09
	public StanleyActions.KeyControllerPairSpriteTags GetExtraActionBindingDescription(int i)
	{
		return StanleyActions.GetPlayerActionBindingDescription(this.ExtraAction(i, false));
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0002C818 File Offset: 0x0002AA18
	public StanleyActions.KeyControllerPairSpriteTags GetJumpBindingDescription()
	{
		return StanleyActions.GetPlayerActionBindingDescription(this.JumpAction);
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0002C825 File Offset: 0x0002AA25
	public StanleyActions.KeyControllerPairSpriteTags GetUseBindingDescription()
	{
		return StanleyActions.GetPlayerActionBindingDescription(this.UseAction);
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x0002C834 File Offset: 0x0002AA34
	private static StanleyActions.KeyControllerPairSpriteTags GetPlayerActionBindingDescription(PlayerAction a)
	{
		StanleyActions.KeyControllerPair keyControllerPair = default(StanleyActions.KeyControllerPair);
		BindingSource bindingSource = new List<BindingSource>(a.Bindings).Find((BindingSource x) => x.BindingSourceType == BindingSourceType.KeyBindingSource || x.BindingSourceType == BindingSourceType.MouseBindingSource);
		if (bindingSource == null)
		{
			keyControllerPair.key = Key.None;
		}
		else if (bindingSource is KeyBindingSource)
		{
			keyControllerPair.key = (bindingSource as KeyBindingSource).Control.GetInclude(0);
			keyControllerPair.isMouse = false;
		}
		else if (bindingSource is MouseBindingSource)
		{
			keyControllerPair.mouse = (bindingSource as MouseBindingSource).Control;
			keyControllerPair.isMouse = true;
		}
		else
		{
			keyControllerPair.key = Key.None;
		}
		DeviceBindingSource deviceBindingSource = new List<BindingSource>(a.Bindings).Find((BindingSource x) => x.BindingSourceType == BindingSourceType.DeviceBindingSource) as DeviceBindingSource;
		keyControllerPair.gamepadInput = ((deviceBindingSource != null) ? deviceBindingSource.Control : InputControlType.None);
		StanleyActions.KeyControllerPairSpriteTags defaultSpriteTags = keyControllerPair.GetDefaultSpriteTags();
		if (Singleton<GameMaster>.Instance.UsingSimplifiedControls)
		{
			defaultSpriteTags.GamepadSpriteTag = LocalizationManager.GetTranslation("Controls_AnyButton", true, 0, true, false, null, null);
		}
		return defaultSpriteTags;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0002C95A File Offset: 0x0002AB5A
	public int GetExtraActionInputsLength()
	{
		return StanleyActions.ExtraActionInputs.Length;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0002C964 File Offset: 0x0002AB64
	public StanleyActions()
	{
		this.MoveForward = base.CreatePlayerAction("Move Forward");
		this.MoveBackward = base.CreatePlayerAction("Move Backward");
		this.MoveLeft = base.CreatePlayerAction("Move Left");
		this.MoveRight = base.CreatePlayerAction("Move Right");
		this.Movement = base.CreateTwoAxisPlayerAction(this.MoveLeft, this.MoveRight, this.MoveBackward, this.MoveForward);
		this.LookUp = base.CreatePlayerAction("Look Up");
		this.LookDown = base.CreatePlayerAction("Look Down");
		this.LookLeft = base.CreatePlayerAction("Look Left");
		this.LookRight = base.CreatePlayerAction("Look Right");
		this.View = base.CreateTwoAxisPlayerAction(this.LookLeft, this.LookRight, this.LookDown, this.LookUp);
		this.Up = base.CreatePlayerAction("Up");
		this.Down = base.CreatePlayerAction("Down");
		this.Left = base.CreatePlayerAction("Left");
		this.Right = base.CreatePlayerAction("Right");
		this.Crouch = base.CreatePlayerAction("Crouch");
		this.Jump = base.CreatePlayerAction("Jump");
		this.Use = base.CreatePlayerAction("Interact With Item");
		this.Autowalk = base.CreatePlayerAction("AutoWalk");
		this.Start = base.CreatePlayerAction("Start");
		this.FastForward = base.CreatePlayerAction("Fast Forward Time");
		this.SlowDown = base.CreatePlayerAction("Slow Time");
		this.ExtraActions = new PlayerAction[StanleyActions.ExtraActionInputs.Length];
		for (int i = 0; i < StanleyActions.ExtraActionInputs.Length; i++)
		{
			this.ExtraActions[i] = base.CreatePlayerAction("ExtraAction" + (i + 1));
		}
		this.MenuTabLeft = base.CreatePlayerAction("Menu Tab Left");
		this.MenuTabRight = base.CreatePlayerAction("Menu Tab Right");
		this.MenuOpen = base.CreatePlayerAction("Menu Open");
		this.MenuBack = base.CreatePlayerAction("Menu Back");
		this.MenuConfirm = base.CreatePlayerAction("Menu Confirm");
		this.AnyButton = base.CreatePlayerAction("Any Button");
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x06000984 RID: 2436 RVA: 0x0002CBAC File Offset: 0x0002ADAC
	public IEnumerable<PlayerAction> UsedInGameActions
	{
		get
		{
			yield return this.MoveForward;
			yield return this.MoveBackward;
			yield return this.MoveLeft;
			yield return this.MoveRight;
			yield return this.Use;
			if (Singleton<GameMaster>.Instance.UsingAutowalk)
			{
				yield return this.Autowalk;
			}
			yield return this.Crouch;
			if (!Singleton<GameMaster>.Instance.UsingSimplifiedControls)
			{
				yield return this.Jump;
			}
			yield break;
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0002CBBC File Offset: 0x0002ADBC
	public static StanleyActions CreateWithDefaultBindings()
	{
		StanleyActions stanleyActions = new StanleyActions();
		stanleyActions.BindKeyboardDefaults();
		stanleyActions.BindControllerButtons();
		return stanleyActions;
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0002CBD0 File Offset: 0x0002ADD0
	public void BindControllerButtons()
	{
		this.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
		this.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
		this.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
		this.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
		this.MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
		this.MoveBackward.AddDefaultBinding(InputControlType.LeftStickDown);
		this.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
		this.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
		this.Up.AddDefaultBinding(InputControlType.DPadUp);
		this.Down.AddDefaultBinding(InputControlType.DPadDown);
		this.Left.AddDefaultBinding(InputControlType.DPadLeft);
		this.Right.AddDefaultBinding(InputControlType.DPadRight);
		this.Crouch.AddDefaultBinding(InputControlType.LeftTrigger);
		this.Crouch.AddDefaultBinding(InputControlType.LeftBumper);
		this.Jump.AddDefaultBinding(StanleyActions.JumpButton);
		this.Use.AddDefaultBinding(StanleyActions.ConfirmButton);
		this.FastForward.AddDefaultBinding(InputControlType.LeftBumper);
		this.SlowDown.AddDefaultBinding(InputControlType.RightBumper);
		for (int i = 0; i < StanleyActions.ExtraActionInputs.Length; i++)
		{
			this.ExtraActions[i].AddDefaultBinding(StanleyActions.ExtraActionInputs[i].gamepadInput);
		}
		this.MenuTabLeft.AddDefaultBinding(InputControlType.LeftBumper);
		this.MenuTabRight.AddDefaultBinding(InputControlType.RightBumper);
		this.MenuBack.AddDefaultBinding(StanleyActions.BackButton);
		this.MenuBack.AddDefaultBinding(InputControlType.Command);
		this.MenuBack.AddDefaultBinding(InputControlType.Options);
		this.MenuBack.AddDefaultBinding(InputControlType.Back);
		this.MenuOpen.AddDefaultBinding(InputControlType.Command);
		this.MenuOpen.AddDefaultBinding(InputControlType.Options);
		this.MenuConfirm.AddDefaultBinding(StanleyActions.ConfirmButton);
		this.MenuConfirm.AddDefaultBinding(InputControlType.Select);
		this.AnyButton.AddBinding(new DeviceBindingSource(InputControlType.Action1));
		this.AnyButton.AddBinding(new DeviceBindingSource(InputControlType.Action2));
		this.AnyButton.AddBinding(new DeviceBindingSource(InputControlType.Action3));
		this.AnyButton.AddBinding(new DeviceBindingSource(InputControlType.Action4));
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000987 RID: 2439 RVA: 0x0002CDCF File Offset: 0x0002AFCF
	private static InputControlType ConfirmButton
	{
		get
		{
			return PlatformGamepad.ConfirmButton;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000988 RID: 2440 RVA: 0x0002CDD6 File Offset: 0x0002AFD6
	private static InputControlType BackButton
	{
		get
		{
			return PlatformGamepad.BackButton;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000989 RID: 2441 RVA: 0x0002CDDD File Offset: 0x0002AFDD
	private static InputControlType JumpButton
	{
		get
		{
			return PlatformGamepad.JumpButton;
		}
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0002CDE4 File Offset: 0x0002AFE4
	public void BindKeyboardDefaults()
	{
		this.MoveForward.AddDefaultBinding(new Key[] { Key.W });
		this.MoveBackward.AddDefaultBinding(new Key[] { Key.S });
		this.MoveLeft.AddDefaultBinding(new Key[] { Key.A });
		this.MoveRight.AddDefaultBinding(new Key[] { Key.D });
		this.Crouch.AddDefaultBinding(new Key[] { Key.LeftControl });
		this.Jump.AddDefaultBinding(new Key[] { Key.Space });
		this.Use.AddDefaultBinding(new Key[] { Key.E });
		this.Use.AddDefaultBinding(Mouse.LeftButton);
		this.Autowalk.AddDefaultBinding(Mouse.RightButton);
		this.FastForward.AddDefaultBinding(new Key[] { Key.Shift });
		this.SlowDown.AddDefaultBinding(new Key[] { Key.Tab });
		for (int i = 0; i < StanleyActions.ExtraActionInputs.Length; i++)
		{
			this.ExtraActions[i].AddDefaultBinding(new Key[] { StanleyActions.ExtraActionInputs[i].key });
		}
		this.MenuTabLeft.AddDefaultBinding(new Key[] { Key.Q });
		this.MenuTabRight.AddDefaultBinding(new Key[] { Key.E });
		this.MenuBack.AddDefaultBinding(new Key[] { Key.Escape });
		this.MenuOpen.AddDefaultBinding(new Key[] { Key.Escape });
		this.MenuConfirm.AddDefaultBinding(new Key[] { Key.Return });
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0002CF74 File Offset: 0x0002B174
	public void UnBindAll()
	{
		this.MoveForward.ClearBindings();
		this.MoveBackward.ClearBindings();
		this.MoveLeft.ClearBindings();
		this.MoveRight.ClearBindings();
		this.LookUp.ClearBindings();
		this.LookDown.ClearBindings();
		this.LookLeft.ClearBindings();
		this.LookRight.ClearBindings();
		this.Up.ClearBindings();
		this.Down.ClearBindings();
		this.Left.ClearBindings();
		this.Right.ClearBindings();
		this.Crouch.ClearBindings();
		this.Jump.ClearBindings();
		this.Use.ClearBindings();
		this.Autowalk.ClearBindings();
		this.FastForward.ClearBindings();
		this.SlowDown.ClearBindings();
		for (int i = 0; i < StanleyActions.ExtraActionInputs.Length; i++)
		{
			this.ExtraActions[i].ClearBindings();
		}
		this.MenuTabLeft.ClearBindings();
		this.MenuTabRight.ClearBindings();
		this.MenuOpen.ClearBindings();
		this.MenuBack.ClearBindings();
		this.MenuConfirm.ClearBindings();
		this.AnyButton.ClearBindings();
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0002D0A8 File Offset: 0x0002B2A8
	public void ResetKeyBindings(StringConfigurable keybindingsConfigurable)
	{
		base.Reset();
		this.SaveCustomKeyBindings(keybindingsConfigurable);
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0002D0B8 File Offset: 0x0002B2B8
	public void SaveCustomKeyBindings(StringConfigurable keybindingsConfigurable)
	{
		string text = Convert.ToBase64String(base.SaveData());
		keybindingsConfigurable.SetValue(text);
		keybindingsConfigurable.SaveToDiskAll();
		Action onKeyBindingsSaved = this.OnKeyBindingsSaved;
		if (onKeyBindingsSaved == null)
		{
			return;
		}
		onKeyBindingsSaved();
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0002D0F0 File Offset: 0x0002B2F0
	public void LoadCustomKeyBindings(StringConfigurable keybindingsConfigurable)
	{
		keybindingsConfigurable.Init();
		string stringValue = keybindingsConfigurable.GetStringValue();
		if (string.IsNullOrEmpty(stringValue))
		{
			return;
		}
		byte[] array = Convert.FromBase64String(stringValue);
		base.LoadData(array);
		Action onKeyBindingsLoaded = this.OnKeyBindingsLoaded;
		if (onKeyBindingsLoaded == null)
		{
			return;
		}
		onKeyBindingsLoaded();
	}

	// Token: 0x04000953 RID: 2387
	public PlayerAction MoveForward;

	// Token: 0x04000954 RID: 2388
	public PlayerAction MoveBackward;

	// Token: 0x04000955 RID: 2389
	public PlayerAction MoveLeft;

	// Token: 0x04000956 RID: 2390
	public PlayerAction MoveRight;

	// Token: 0x04000957 RID: 2391
	public PlayerTwoAxisAction Movement;

	// Token: 0x04000958 RID: 2392
	public PlayerAction LookUp;

	// Token: 0x04000959 RID: 2393
	public PlayerAction LookDown;

	// Token: 0x0400095A RID: 2394
	public PlayerAction LookLeft;

	// Token: 0x0400095B RID: 2395
	public PlayerAction LookRight;

	// Token: 0x0400095C RID: 2396
	public PlayerTwoAxisAction View;

	// Token: 0x0400095D RID: 2397
	public PlayerAction Up;

	// Token: 0x0400095E RID: 2398
	public PlayerAction Down;

	// Token: 0x0400095F RID: 2399
	public PlayerAction Left;

	// Token: 0x04000960 RID: 2400
	public PlayerAction Right;

	// Token: 0x04000961 RID: 2401
	public PlayerAction Crouch;

	// Token: 0x04000962 RID: 2402
	protected PlayerAction Jump;

	// Token: 0x04000963 RID: 2403
	protected PlayerAction Use;

	// Token: 0x04000964 RID: 2404
	public PlayerAction Autowalk;

	// Token: 0x04000965 RID: 2405
	public PlayerAction Start;

	// Token: 0x04000966 RID: 2406
	protected PlayerAction AnyButton;

	// Token: 0x04000967 RID: 2407
	protected PlayerAction[] ExtraActions;

	// Token: 0x04000968 RID: 2408
	public PlayerAction MenuTabLeft;

	// Token: 0x04000969 RID: 2409
	public PlayerAction MenuTabRight;

	// Token: 0x0400096A RID: 2410
	public PlayerAction MenuConfirm;

	// Token: 0x0400096B RID: 2411
	public PlayerAction MenuBack;

	// Token: 0x0400096C RID: 2412
	public PlayerAction MenuOpen;

	// Token: 0x0400096F RID: 2415
	private static StanleyActions.KeyControllerPair[] ExtraActionInputs = new StanleyActions.KeyControllerPair[]
	{
		new StanleyActions.KeyControllerPair
		{
			key = Key.F,
			gamepadInput = InputControlType.Action1
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.G,
			gamepadInput = InputControlType.Action2
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.H,
			gamepadInput = InputControlType.Action3
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.J,
			gamepadInput = InputControlType.Action4
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.G,
			gamepadInput = InputControlType.Action4
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.B,
			gamepadInput = InputControlType.DPadUp
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.C,
			gamepadInput = InputControlType.DPadDown
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.Q,
			gamepadInput = InputControlType.DPadLeft
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.I,
			gamepadInput = InputControlType.DPadRight
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.K,
			gamepadInput = InputControlType.Action1
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.L,
			gamepadInput = InputControlType.Action2
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.M,
			gamepadInput = InputControlType.Action3
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.N,
			gamepadInput = InputControlType.Action4
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.O,
			gamepadInput = InputControlType.DPadUp
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.P,
			gamepadInput = InputControlType.DPadDown
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.R,
			gamepadInput = InputControlType.DPadLeft
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.T,
			gamepadInput = InputControlType.DPadRight
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.U,
			gamepadInput = InputControlType.Action1
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.V,
			gamepadInput = InputControlType.Action2
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.X,
			gamepadInput = InputControlType.Action3
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.Y,
			gamepadInput = InputControlType.Action4
		},
		new StanleyActions.KeyControllerPair
		{
			key = Key.Z,
			gamepadInput = InputControlType.DPadUp
		}
	};

	// Token: 0x04000970 RID: 2416
	public PlayerAction FastForward;

	// Token: 0x04000971 RID: 2417
	public PlayerAction SlowDown;

	// Token: 0x020003F0 RID: 1008
	public struct KeyControllerPair
	{
		// Token: 0x060017AA RID: 6058 RVA: 0x00079B44 File Offset: 0x00077D44
		public StanleyActions.KeyControllerPairSpriteTags GetDefaultSpriteTags()
		{
			return new StanleyActions.KeyControllerPairSpriteTags
			{
				KeyboardSpriteTag = (this.isMouse ? StanleyActions.KeyControllerPair.MouseSpriteNameTag(this.mouse) : StanleyActions.KeyControllerPair.KeySpriteNameTag(this.key)),
				GamepadSpriteTag = StanleyActions.KeyControllerPair.SpriteNameTag(this.gamepadInput.ToString())
			};
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00079B9E File Offset: 0x00077D9E
		private static string SpriteNameTag(string insert)
		{
			return "<sprite name=\"" + insert + "\">";
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00079BB0 File Offset: 0x00077DB0
		public static string KeyTag(string playerActionName)
		{
			if (playerActionName.Length != 1)
			{
				return playerActionName.ToLower();
			}
			return playerActionName;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00079BC4 File Offset: 0x00077DC4
		public static bool HasSprite(Key key)
		{
			if (key <= Key.Return)
			{
				switch (key)
				{
				case Key.Shift:
				case Key.Alt:
				case Key.Control:
				case Key.LeftShift:
				case Key.LeftAlt:
				case Key.LeftControl:
				case Key.RightShift:
				case Key.RightAlt:
				case Key.RightControl:
				case Key.A:
				case Key.B:
				case Key.C:
				case Key.D:
				case Key.E:
				case Key.F:
				case Key.G:
				case Key.H:
				case Key.I:
				case Key.J:
				case Key.K:
				case Key.L:
				case Key.M:
				case Key.N:
				case Key.O:
				case Key.P:
				case Key.Q:
				case Key.R:
				case Key.S:
				case Key.T:
				case Key.U:
				case Key.V:
				case Key.W:
				case Key.X:
				case Key.Y:
				case Key.Z:
					break;
				case Key.Command:
				case Key.LeftCommand:
				case Key.RightCommand:
				case Key.Escape:
				case Key.F1:
				case Key.F2:
				case Key.F3:
				case Key.F4:
				case Key.F5:
				case Key.F6:
				case Key.F7:
				case Key.F8:
				case Key.F9:
				case Key.F10:
				case Key.F11:
				case Key.F12:
				case Key.Key0:
				case Key.Key1:
				case Key.Key2:
				case Key.Key3:
				case Key.Key4:
				case Key.Key5:
				case Key.Key6:
				case Key.Key7:
				case Key.Key8:
				case Key.Key9:
					return false;
				default:
					if (key != Key.Return)
					{
						return false;
					}
					break;
				}
			}
			else if (key != Key.Space && key - Key.LeftArrow > 3)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00079CEB File Offset: 0x00077EEB
		public static string KeySpriteNameTag(Key key)
		{
			if (!StanleyActions.KeyControllerPair.HasSprite(key))
			{
				return key.ToString();
			}
			return StanleyActions.KeyControllerPair.SpriteNameTag(StanleyActions.KeyControllerPair.KeyTag(key.ToString()));
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00079D1C File Offset: 0x00077F1C
		public static string MouseSpriteNameTag(Mouse mouse)
		{
			switch (mouse)
			{
			case Mouse.LeftButton:
				return StanleyActions.KeyControllerPair.SpriteNameTag("left_mouse_button_stanley");
			case Mouse.RightButton:
				return StanleyActions.KeyControllerPair.SpriteNameTag("right_mouse_button_stanley");
			case Mouse.MiddleButton:
				return StanleyActions.KeyControllerPair.SpriteNameTag("middle_mouse_button");
			case Mouse.Button4:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_4_button");
			case Mouse.Button5:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_5_button");
			case Mouse.Button6:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_6_button");
			case Mouse.Button7:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_7_button");
			case Mouse.Button8:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_8_button");
			case Mouse.Button9:
				return StanleyActions.KeyControllerPair.SpriteNameTag("mouse_9_button");
			}
			return StanleyActions.KeyControllerPair.SpriteNameTag(mouse.ToString());
		}

		// Token: 0x0400147C RID: 5244
		public Key key;

		// Token: 0x0400147D RID: 5245
		public Mouse mouse;

		// Token: 0x0400147E RID: 5246
		public bool isMouse;

		// Token: 0x0400147F RID: 5247
		public InputControlType gamepadInput;
	}

	// Token: 0x020003F1 RID: 1009
	public struct KeyControllerPairSpriteTags
	{
		// Token: 0x04001480 RID: 5248
		public string KeyboardSpriteTag;

		// Token: 0x04001481 RID: 5249
		public string GamepadSpriteTag;
	}
}
