using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NewBlood;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;

// Token: 0x02000271 RID: 625
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class InputManager : MonoSingleton<InputManager>
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000677EE File Offset: 0x000659EE
	// (set) Token: 0x06000DC2 RID: 3522 RVA: 0x000677F6 File Offset: 0x000659F6
	public global::PlayerInput InputSource { get; private set; }

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x000677FF File Offset: 0x000659FF
	// (set) Token: 0x06000DC4 RID: 3524 RVA: 0x00067807 File Offset: 0x00065A07
	public InputDevice LastButtonDevice { get; private set; }

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00067810 File Offset: 0x00065A10
	// (set) Token: 0x06000DC6 RID: 3526 RVA: 0x00067818 File Offset: 0x00065A18
	public bool IsRebinding { get; private set; }

	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00067824 File Offset: 0x00065A24
	private static IObservable<InputControl> onAnyInput
	{
		get
		{
			return from c in InputSystem.onEvent.Select(delegate(InputEventPtr e)
				{
					if (e.type != 1398030676 && e.type != 1145852993)
					{
						return null;
					}
					return e.GetFirstButtonPressOrNull(-1f, false);
				})
				where c != null && !c.noisy
				select c;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00067883 File Offset: 0x00065A83
	public Dictionary<string, KeyCode> Inputs
	{
		get
		{
			return this.inputsDictionary;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0006788B File Offset: 0x00065A8B
	private FileInfo savedBindingsFile
	{
		get
		{
			return new FileInfo(Path.Combine(PrefsManager.PrefsPath, "Binds.json"));
		}
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x000678A4 File Offset: 0x00065AA4
	protected override void Awake()
	{
		base.Awake();
		this.InputSource = new global::PlayerInput();
		this.defaultActions = InputActionAsset.FromJson(this.InputSource.Actions.asset.ToJson());
		if (this.savedBindingsFile.Exists)
		{
			JsonConvert.DeserializeObject<JsonBindingMap>(File.ReadAllText(this.savedBindingsFile.FullName)).ApplyTo(this.InputSource.Actions.asset);
		}
		this.legacyBindings = new InputManager.BindingInfo[]
		{
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Move.Action,
				DefaultKey = KeyCode.W,
				Name = "W"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Move.Action,
				Offset = 1,
				DefaultKey = KeyCode.S,
				Name = "S"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Move.Action,
				Offset = 2,
				DefaultKey = KeyCode.A,
				Name = "A"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Move.Action,
				Offset = 3,
				DefaultKey = KeyCode.D,
				Name = "D"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Jump.Action,
				DefaultKey = KeyCode.Space,
				Name = "Jump"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Dodge.Action,
				DefaultKey = KeyCode.LeftShift,
				Name = "Dodge"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slide.Action,
				DefaultKey = KeyCode.LeftControl,
				Name = "Slide"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Fire1.Action,
				DefaultKey = KeyCode.Mouse0,
				Name = "Fire1"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Fire2.Action,
				DefaultKey = KeyCode.Mouse1,
				Name = "Fire2"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Punch.Action,
				DefaultKey = KeyCode.F,
				Name = "Punch"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Hook.Action,
				DefaultKey = KeyCode.R,
				Name = "Hook"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.LastWeapon.Action,
				DefaultKey = KeyCode.Q,
				Name = "LastUsedWeapon"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.NextVariation.Action,
				DefaultKey = KeyCode.E,
				Name = "ChangeVariation"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.ChangeFist.Action,
				DefaultKey = KeyCode.G,
				Name = "ChangeFist"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot1.Action,
				DefaultKey = KeyCode.Alpha1,
				Name = "Slot1"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot2.Action,
				DefaultKey = KeyCode.Alpha2,
				Name = "Slot2"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot3.Action,
				DefaultKey = KeyCode.Alpha3,
				Name = "Slot3"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot4.Action,
				DefaultKey = KeyCode.Alpha4,
				Name = "Slot4"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot5.Action,
				DefaultKey = KeyCode.Alpha5,
				Name = "Slot5"
			},
			new InputManager.BindingInfo
			{
				Action = this.InputSource.Slot6.Action,
				DefaultKey = KeyCode.Alpha6,
				Name = "Slot6"
			}
		};
		this.UpgradeBindings();
		this.InputSource.Enable();
		this.ScrOn = MonoSingleton<PrefsManager>.Instance.GetBool("scrollEnabled", false);
		this.ScrWep = MonoSingleton<PrefsManager>.Instance.GetBool("scrollWeapons", false);
		this.ScrVar = MonoSingleton<PrefsManager>.Instance.GetBool("scrollVariations", false);
		this.ScrRev = MonoSingleton<PrefsManager>.Instance.GetBool("scrollReversed", false);
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00067D8F File Offset: 0x00065F8F
	protected override void OnEnable()
	{
		base.OnEnable();
		this.anyButtonListener = InputManager.onAnyInput.Subscribe(InputManager.ButtonPressListener.Instance);
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x00067DCC File Offset: 0x00065FCC
	private void OnDisable()
	{
		IDisposable disposable = this.anyButtonListener;
		if (disposable != null)
		{
			disposable.Dispose();
		}
		this.SaveBindings(this.InputSource.Actions.asset);
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x00067E20 File Offset: 0x00066020
	private void OnPrefChanged(string key, object value)
	{
		if (key == "scrollEnabled")
		{
			this.ScrOn = (bool)value;
			return;
		}
		if (key == "scrollWeapons")
		{
			this.ScrWep = (bool)value;
			return;
		}
		if (key == "scrollVariations")
		{
			this.ScrVar = (bool)value;
			return;
		}
		if (!(key == "scrollReversed"))
		{
			return;
		}
		this.ScrRev = (bool)value;
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00067E98 File Offset: 0x00066098
	public void ResetToDefault()
	{
		JsonBindingMap.From(this.defaultActions, this.InputSource.Actions.KeyboardMouseScheme).ApplyTo(this.InputSource.Actions.asset);
		this.InputSource.ValidateBindings(this.InputSource.Actions.KeyboardMouseScheme);
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x00067EF0 File Offset: 0x000660F0
	public void ResetToDefault(InputAction action, InputControlScheme controlScheme)
	{
		InputAction inputAction = this.defaultActions.FindAction(action.name, false);
		this.InputSource.Disable();
		action.WipeAction(controlScheme.bindingGroup);
		for (int i = 0; i < inputAction.bindings.Count; i++)
		{
			if (inputAction.BindingHasGroup(i, controlScheme.bindingGroup))
			{
				InputBinding inputBinding = inputAction.bindings[i];
				if (!inputBinding.isPartOfComposite)
				{
					if (inputBinding.isComposite)
					{
						InputActionSetupExtensions.CompositeSyntax compositeSyntax = action.AddCompositeBinding("2DVector", null, null);
						for (int j = i + 1; j < inputAction.bindings.Count; j++)
						{
							if (!inputAction.bindings[j].isPartOfComposite)
							{
								break;
							}
							InputBinding inputBinding2 = inputAction.bindings[j];
							compositeSyntax.With(inputBinding2.name, inputBinding2.path, controlScheme.bindingGroup, null);
						}
					}
					else
					{
						action.AddBinding(inputBinding).WithGroup(controlScheme.bindingGroup);
					}
				}
			}
		}
		Action<InputAction> action2 = this.actionModified;
		if (action2 != null)
		{
			action2(action);
		}
		this.SaveBindings(this.InputSource.Actions.asset);
		this.InputSource.Enable();
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00068045 File Offset: 0x00066245
	public bool PerformingCheatMenuCombo()
	{
		return MonoSingleton<CheatsController>.Instance.cheatsEnabled && this.LastButtonDevice is Gamepad && Gamepad.current != null && Gamepad.current.selectButton.isPressed;
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0006807C File Offset: 0x0006627C
	public void SaveBindings(InputActionAsset asset)
	{
		JsonBindingMap jsonBindingMap = JsonBindingMap.From(asset, this.defaultActions, this.InputSource.Actions.KeyboardMouseScheme);
		File.WriteAllText(this.savedBindingsFile.FullName, JsonConvert.SerializeObject(jsonBindingMap, Formatting.Indented));
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x000680C0 File Offset: 0x000662C0
	public void UpgradeBindings()
	{
		foreach (InputManager.BindingInfo bindingInfo in this.legacyBindings)
		{
			InputBinding inputBinding = InputBinding.MaskByGroup("Keyboard & Mouse");
			int bindingIndex = bindingInfo.Action.GetBindingIndex(inputBinding);
			this.Inputs[bindingInfo.Name] = (KeyCode)MonoSingleton<PrefsManager>.Instance.GetInt(bindingInfo.PrefName, (int)bindingInfo.DefaultKey);
			if (bindingIndex != -1 && MonoSingleton<PrefsManager>.Instance.HasKey(bindingInfo.PrefName))
			{
				KeyCode @int = (KeyCode)MonoSingleton<PrefsManager>.Instance.GetInt(bindingInfo.PrefName, 0);
				MonoSingleton<PrefsManager>.Instance.DeleteKey(bindingInfo.PrefName);
				ButtonControl button = LegacyInput.current.GetButton(@int);
				bindingInfo.Action.ChangeBinding(bindingIndex + bindingInfo.Offset).WithPath(button.path).WithGroup(this.InputSource.Actions.KeyboardMouseScheme.bindingGroup);
			}
		}
		foreach (InputAction inputAction in this.InputSource.Actions)
		{
			foreach (InputBinding inputBinding2 in inputAction.bindings)
			{
				if (inputBinding2.path.Contains("LegacyInput"))
				{
					string text = inputBinding2.path.Replace("/LegacyInput/", "<Keyboard>/").Replace("alpha", "");
					if (InputSystem.FindControl(text) != null)
					{
						inputAction.ChangeBinding(inputBinding2).WithPath(text);
					}
					else
					{
						inputAction.ChangeBinding(inputBinding2).Erase();
					}
				}
			}
		}
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x000682B8 File Offset: 0x000664B8
	public void CancelRebinding()
	{
		if (!this.IsRebinding)
		{
			return;
		}
		InputActionRebindingExtensions.RebindingOperation rebindingOperation = this.rebinding;
		if (rebindingOperation != null)
		{
			rebindingOperation.Cancel();
		}
		InputActionRebindingExtensions.RebindingOperation rebindingOperation2 = this.rebinding;
		if (rebindingOperation2 != null)
		{
			rebindingOperation2.Dispose();
		}
		this.rebinding = null;
		Action action = this.currentCancelCallback;
		if (action != null)
		{
			action();
		}
		this.currentCancelCallback = null;
		this.IsRebinding = false;
		this.InputSource.Enable();
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00068324 File Offset: 0x00066524
	public void WaitForButton(Action<string> onComplete, Action onCancel, List<string> allowedPaths = null)
	{
		this.InputSource.Disable();
		InputActionRebindingExtensions.RebindingOperation rebindingOperation = this.rebinding;
		if (rebindingOperation != null)
		{
			rebindingOperation.Cancel();
		}
		InputActionRebindingExtensions.RebindingOperation rebindingOperation2 = this.rebinding;
		if (rebindingOperation2 != null)
		{
			rebindingOperation2.Dispose();
		}
		this.currentCancelCallback = onCancel;
		this.IsRebinding = true;
		this.rebinding = new InputActionRebindingExtensions.RebindingOperation().OnApplyBinding(delegate(InputActionRebindingExtensions.RebindingOperation op, string path)
		{
			this.rebinding = null;
			op.Dispose();
			this.IsRebinding = false;
			this.currentCancelCallback = null;
			if (InputControlPath.TryFindControl(Keyboard.current, path, 0) == Keyboard.current.escapeKey)
			{
				Action onCancel2 = onCancel;
				if (onCancel2 != null)
				{
					onCancel2();
				}
			}
			else
			{
				Action<string> onComplete2 = onComplete;
				if (onComplete2 != null)
				{
					onComplete2(path);
				}
			}
			this.InputSource.Enable();
		}).WithControlsExcluding(LegacyInput.current.path).WithExpectedControlType<ButtonControl>()
			.WithMatchingEventsBeingSuppressed(true);
		if (allowedPaths != null)
		{
			foreach (string text in allowedPaths)
			{
				this.rebinding.WithControlsHavingToMatchPath(text);
			}
		}
		this.rebinding.Start();
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x00068418 File Offset: 0x00066618
	public void WaitForButtonSequence(Queue<string> partNames, Action<string> onBeginPart, Action<string, string> onCompletePart, Action onComplete, Action onCancel, List<string> allowedPaths = null)
	{
		if (partNames.Count != 0)
		{
			string part = partNames.Dequeue();
			Action<string> onBeginPart2 = onBeginPart;
			if (onBeginPart2 != null)
			{
				onBeginPart2(part);
			}
			this.WaitForButton(delegate(string path)
			{
				Action<string, string> onCompletePart2 = onCompletePart;
				if (onCompletePart2 != null)
				{
					onCompletePart2(part, path);
				}
				this.WaitForButtonSequence(partNames, onBeginPart, onCompletePart, onComplete, onCancel, null);
			}, onCancel, allowedPaths);
			return;
		}
		Action onComplete2 = onComplete;
		if (onComplete2 == null)
		{
			return;
		}
		onComplete2();
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x000684B8 File Offset: 0x000666B8
	public void ClearOtherActions(InputAction action, string path)
	{
		foreach (InputAction inputAction in this.InputSource.Actions)
		{
			if (inputAction != action)
			{
				int bindingIndex = inputAction.GetBindingIndex(null, path);
				if (bindingIndex != -1)
				{
					InputActionSetupExtensions.BindingSyntax bindingSyntax = inputAction.ChangeBinding(bindingIndex);
					if (bindingSyntax.binding.isPartOfComposite)
					{
						bindingSyntax = bindingSyntax.PreviousCompositeBinding(null);
					}
					bindingSyntax.Erase();
				}
			}
		}
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x00068540 File Offset: 0x00066740
	public void Rebind(InputAction action, int? existingIndex, Action onComplete, Action onCancel, InputControlScheme scheme)
	{
		List<string> list = scheme.deviceRequirements.Select((InputControlScheme.DeviceRequirement requirement) => requirement.controlPath).ToList<string>();
		this.WaitForButton(delegate(string path)
		{
			foreach (InputBinding inputBinding in action.bindings)
			{
				if (InputSystem.FindControl(inputBinding.path) == InputSystem.FindControl(path))
				{
					Action onComplete2 = onComplete;
					if (onComplete2 == null)
					{
						return;
					}
					onComplete2();
					return;
				}
			}
			InputActionSetupExtensions.BindingSyntax bindingSyntax;
			if (existingIndex != null)
			{
				int valueOrDefault = existingIndex.GetValueOrDefault();
				bindingSyntax = action.ChangeBinding(valueOrDefault);
			}
			else
			{
				bindingSyntax = action.AddBinding(default(InputBinding));
			}
			InputActionSetupExtensions.BindingSyntax bindingSyntax2 = bindingSyntax;
			bindingSyntax2.WithPath(path).WithGroup(scheme.bindingGroup);
			Action<InputAction> action2 = this.actionModified;
			if (action2 != null)
			{
				action2(action);
			}
			Action onComplete3 = onComplete;
			if (onComplete3 == null)
			{
				return;
			}
			onComplete3();
		}, delegate
		{
			Action onCancel2 = onCancel;
			if (onCancel2 == null)
			{
				return;
			}
			onCancel2();
		}, list);
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x000685D8 File Offset: 0x000667D8
	public void RebindComposite(InputAction action, int? existingIndex, Action<string> onBeginPart, Action onComplete, Action onCancel, InputControlScheme scheme)
	{
		List<string> list = scheme.deviceRequirements.Select((InputControlScheme.DeviceRequirement requirement) => requirement.controlPath).ToList<string>();
		if (action.expectedControlType == "Vector2")
		{
			string[] array = new string[] { "Up", "Down", "Left", "Right" };
			Dictionary<string, string> partPathDict = new Dictionary<string, string>();
			this.WaitForButtonSequence(new Queue<string>(array), onBeginPart, delegate(string part, string path)
			{
				partPathDict.Add(part, path);
			}, delegate
			{
				if (existingIndex != null)
				{
					int valueOrDefault = existingIndex.GetValueOrDefault();
					InputActionSetupExtensions.BindingSyntax bindingSyntax = action.ChangeBinding(valueOrDefault);
					using (Dictionary<string, string>.Enumerator enumerator = partPathDict.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, string> keyValuePair = enumerator.Current;
							bindingSyntax.NextPartBinding(keyValuePair.Key).WithPath(keyValuePair.Value).WithGroup(scheme.bindingGroup);
						}
						goto IL_010F;
					}
				}
				InputActionSetupExtensions.CompositeSyntax compositeSyntax = action.AddCompositeBinding("2DVector", null, null);
				foreach (KeyValuePair<string, string> keyValuePair2 in partPathDict)
				{
					compositeSyntax.With(keyValuePair2.Key, keyValuePair2.Value, scheme.bindingGroup, null);
				}
				action.AddBinding(default(InputBinding)).Erase();
				IL_010F:
				Action<InputAction> action2 = this.actionModified;
				if (action2 != null)
				{
					action2(action);
				}
				Action onComplete2 = onComplete;
				if (onComplete2 == null)
				{
					return;
				}
				onComplete2();
			}, onCancel, list);
			return;
		}
		Debug.LogError("Attempted to call RebindComposite on action with unsupported control type: '" + action.expectedControlType + "'");
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x000686DB File Offset: 0x000668DB
	public string GetBindingString(Guid actionId)
	{
		return this.GetBindingString(actionId.ToString());
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x000686F0 File Offset: 0x000668F0
	public string GetBindingString(string nameOrId)
	{
		ReadOnlyArray<InputBinding> bindings = this.InputSource.Actions.FindAction(nameOrId, false).bindings;
		string text = string.Empty;
		int num = 0;
		Queue<string> queue = new Queue<string>();
		InputControlScheme inputControlScheme = this.InputSource.Actions.KeyboardMouseScheme;
		foreach (InputControlScheme inputControlScheme2 in this.InputSource.Actions.controlSchemes)
		{
			if (inputControlScheme2.SupportsDevice(this.LastButtonDevice))
			{
				inputControlScheme = inputControlScheme2;
				break;
			}
		}
		for (int i = 0; i < bindings.Count; i++)
		{
			if (bindings[i].isComposite)
			{
				num = i;
			}
			else
			{
				InputControl inputControl = InputSystem.FindControl(bindings[i].path);
				if (inputControl != null)
				{
					if (bindings[i].isPartOfComposite)
					{
						int num2 = num + 1;
						while (num2 < bindings.Count && bindings[num2].isPartOfComposite)
						{
							if (num2 > num + 1)
							{
								text += " + ";
							}
							text += bindings[num2].ToDisplayString((InputBinding.DisplayStringOptions)0, null) ?? "?";
							num2++;
						}
						return text;
					}
					if (inputControlScheme.SupportsDevice(inputControl.device))
					{
						return bindings[i].ToDisplayString((InputBinding.DisplayStringOptions)0, null);
					}
				}
			}
		}
		if (queue.Count == 0)
		{
			return "";
		}
		Debug.Log(queue.Count);
		string text2 = queue.Dequeue() ?? "";
		while (queue.Count > 0)
		{
			text2 = text2 + "/" + queue.Dequeue();
		}
		return text2;
	}

	// Token: 0x04001248 RID: 4680
	public Dictionary<string, KeyCode> inputsDictionary = new Dictionary<string, KeyCode>();

	// Token: 0x0400124C RID: 4684
	public InputActionAsset defaultActions;

	// Token: 0x0400124D RID: 4685
	private IDisposable anyButtonListener;

	// Token: 0x0400124E RID: 4686
	public bool ScrOn;

	// Token: 0x0400124F RID: 4687
	public bool ScrWep;

	// Token: 0x04001250 RID: 4688
	public bool ScrVar;

	// Token: 0x04001251 RID: 4689
	public bool ScrRev;

	// Token: 0x04001252 RID: 4690
	public Action<InputAction> actionModified;

	// Token: 0x04001253 RID: 4691
	private InputManager.BindingInfo[] legacyBindings;

	// Token: 0x04001254 RID: 4692
	private InputActionRebindingExtensions.RebindingOperation rebinding;

	// Token: 0x04001255 RID: 4693
	private Action currentCancelCallback;

	// Token: 0x02000272 RID: 626
	private sealed class ButtonPressListener : IObserver<InputControl>
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x000688F3 File Offset: 0x00066AF3
		public static InputManager.ButtonPressListener Instance { get; } = new InputManager.ButtonPressListener();

		// Token: 0x06000DDD RID: 3549 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnCompleted()
		{
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnError(Exception error)
		{
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000688FC File Offset: 0x00066AFC
		public void OnNext(InputControl value)
		{
			if (value.device is LegacyInput)
			{
				return;
			}
			MonoSingleton<InputManager>.Instance.LastButtonDevice = value.device;
			if (MonoSingleton<InputManager>.Instance.IsRebinding && value.device is Gamepad)
			{
				MonoSingleton<InputManager>.Instance.CancelRebinding();
			}
		}
	}

	// Token: 0x02000273 RID: 627
	private class BindingInfo
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000DE2 RID: 3554 RVA: 0x00068956 File Offset: 0x00066B56
		public string PrefName
		{
			get
			{
				return "keyBinding." + this.Name;
			}
		}

		// Token: 0x04001257 RID: 4695
		public InputAction Action;

		// Token: 0x04001258 RID: 4696
		public string Name;

		// Token: 0x04001259 RID: 4697
		public int Offset;

		// Token: 0x0400125A RID: 4698
		public KeyCode DefaultKey;
	}
}
