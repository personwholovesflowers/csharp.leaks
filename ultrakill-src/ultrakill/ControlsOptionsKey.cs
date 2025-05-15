using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020000E1 RID: 225
public class ControlsOptionsKey : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	// Token: 0x0600046C RID: 1132 RVA: 0x0001ED37 File Offset: 0x0001CF37
	public void OnSelect(BaseEventData eventData)
	{
		this.selected = true;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001ED40 File Offset: 0x0001CF40
	public void OnDeselect(BaseEventData eventData)
	{
		this.selected = false;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001ED49 File Offset: 0x0001CF49
	private void SubmitPressed(InputAction.CallbackContext ctx)
	{
		if (this.selected && this.bindingButtons.Count > 0)
		{
			this.bindingButtons[0].Select();
		}
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001ED74 File Offset: 0x0001CF74
	private void OnEnable()
	{
		MonoSingleton<InputManager>.Instance.InputSource.Actions.UI.Submit.performed += this.SubmitPressed;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001EDB0 File Offset: 0x0001CFB0
	private void OnDisable()
	{
		if (MonoSingleton<InputManager>.Instance)
		{
			MonoSingleton<InputManager>.Instance.InputSource.Actions.UI.Submit.performed -= this.SubmitPressed;
		}
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
	private void Update()
	{
		bool flag = MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad;
		this.blocker.SetActive(flag);
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001EE24 File Offset: 0x0001D024
	public void RebuildBindings(InputAction action, InputControlScheme controlScheme)
	{
		foreach (Button button in this.bindingButtons)
		{
			Object.Destroy(button.gameObject);
		}
		this.bindingButtons.Clear();
		int num = 0;
		int[] bindingsWithGroup = action.GetBindingsWithGroup(controlScheme.bindingGroup);
		Action <>9__3;
		Action <>9__5;
		for (int i = 0; i < bindingsWithGroup.Length; i++)
		{
			int num2 = bindingsWithGroup[i];
			InputBinding binding = action.bindings[num2];
			num++;
			string bindingDisplayStringWithoutOverride = action.GetBindingDisplayStringWithoutOverride(binding, InputBinding.DisplayStringOptions.DontIncludeInteractions);
			ValueTuple<Button, TextMeshProUGUI, Image, TooltipOnHover> valueTuple = this.BuildBindingButton(bindingDisplayStringWithoutOverride);
			Button item = valueTuple.Item1;
			TextMeshProUGUI txt2 = valueTuple.Item2;
			Image img2 = valueTuple.Item3;
			TooltipOnHover item2 = valueTuple.Item4;
			string text = txt2.text + "<br>";
			bool flag = false;
			if (binding.isComposite)
			{
				InputActionSetupExtensions.BindingSyntax bindingSyntax = action.ChangeBinding(binding).NextBinding();
				HashSet<string> hashSet = new HashSet<string>();
				while (bindingSyntax.valid)
				{
					if (!bindingSyntax.binding.isPartOfComposite)
					{
						break;
					}
					InputBinding[] conflicts = MonoSingleton<InputManager>.Instance.InputSource.GetConflicts(bindingSyntax.binding);
					if (conflicts.Length != 0 && !hashSet.Contains(bindingSyntax.binding.path))
					{
						flag = true;
						text = text + "<br>" + this.GenerateTooltip(action, bindingSyntax.binding, conflicts);
						hashSet.Add(bindingSyntax.binding.path);
					}
					bindingSyntax = bindingSyntax.NextBinding();
				}
			}
			else
			{
				InputBinding[] conflicts2 = MonoSingleton<InputManager>.Instance.InputSource.GetConflicts(binding);
				if (conflicts2.Length != 0)
				{
					flag = true;
					text = text + "<br>" + this.GenerateTooltip(action, binding, conflicts2);
				}
			}
			item2.text = text;
			item2.enabled = true;
			if (flag)
			{
				txt2.color = Color.red;
			}
			int index = num2;
			Action<string> <>9__2;
			Action <>9__4;
			Action <>9__6;
			item.onClick.AddListener(delegate
			{
				Color color = img2.color;
				img2.color = Color.red;
				if (binding.isComposite)
				{
					InputManager instance = MonoSingleton<InputManager>.Instance;
					InputAction action2 = action;
					int? num3 = new int?(index);
					Action<string> action3;
					if ((action3 = <>9__2) == null)
					{
						action3 = (<>9__2 = delegate(string part)
						{
							txt2.text = "PRESS " + part.ToUpper();
						});
					}
					Action action4;
					if ((action4 = <>9__3) == null)
					{
						action4 = (<>9__3 = delegate
						{
							this.RebuildBindings(action, controlScheme);
						});
					}
					Action action5;
					if ((action5 = <>9__4) == null)
					{
						action5 = (<>9__4 = delegate
						{
							action.ChangeBinding(index).Erase();
							Action<InputAction> actionModified = MonoSingleton<InputManager>.Instance.actionModified;
							if (actionModified == null)
							{
								return;
							}
							actionModified(action);
						});
					}
					instance.RebindComposite(action2, num3, action3, action4, action5, controlScheme);
					return;
				}
				InputManager instance2 = MonoSingleton<InputManager>.Instance;
				InputAction action6 = action;
				int? num4 = new int?(index);
				Action action7;
				if ((action7 = <>9__5) == null)
				{
					action7 = (<>9__5 = delegate
					{
						this.RebuildBindings(action, controlScheme);
					});
				}
				Action action8;
				if ((action8 = <>9__6) == null)
				{
					action8 = (<>9__6 = delegate
					{
						action.ChangeBinding(index).Erase();
						Action<InputAction> actionModified2 = MonoSingleton<InputManager>.Instance.actionModified;
						if (actionModified2 == null)
						{
							return;
						}
						actionModified2(action);
					});
				}
				instance2.Rebind(action6, num4, action7, action8, controlScheme);
			});
		}
		if (num < 4)
		{
			ValueTuple<Button, TextMeshProUGUI, Image> valueTuple2 = this.BuildNewBindButton();
			Button item3 = valueTuple2.Item1;
			TextMeshProUGUI txt = valueTuple2.Item2;
			Image img = valueTuple2.Item3;
			Action <>9__8;
			Action <>9__9;
			Action <>9__11;
			Action <>9__12;
			Action<string> <>9__10;
			item3.onClick.AddListener(delegate
			{
				img.color = Color.red;
				txt.color = Color.white;
				txt.text = "...";
				if (action.expectedControlType == "Button")
				{
					InputManager instance3 = MonoSingleton<InputManager>.Instance;
					InputAction action9 = action;
					int? num5 = null;
					Action action10;
					if ((action10 = <>9__8) == null)
					{
						action10 = (<>9__8 = delegate
						{
							this.RebuildBindings(action, controlScheme);
						});
					}
					Action action11;
					if ((action11 = <>9__9) == null)
					{
						action11 = (<>9__9 = delegate
						{
							this.RebuildBindings(action, controlScheme);
						});
					}
					instance3.Rebind(action9, num5, action10, action11, controlScheme);
					return;
				}
				if (action.expectedControlType == "Vector2")
				{
					InputManager instance4 = MonoSingleton<InputManager>.Instance;
					InputAction action12 = action;
					int? num6 = null;
					Action<string> action13;
					if ((action13 = <>9__10) == null)
					{
						action13 = (<>9__10 = delegate(string part)
						{
							txt.text = "PRESS " + part.ToUpper();
						});
					}
					Action action14;
					if ((action14 = <>9__11) == null)
					{
						action14 = (<>9__11 = delegate
						{
							this.RebuildBindings(action, controlScheme);
						});
					}
					Action action15;
					if ((action15 = <>9__12) == null)
					{
						action15 = (<>9__12 = delegate
						{
							this.RebuildBindings(action, controlScheme);
						});
					}
					instance4.RebindComposite(action12, num6, action13, action14, action15, controlScheme);
				}
			});
		}
		bool flag2 = action.IsActionEqual(MonoSingleton<InputManager>.Instance.defaultActions.FindAction(action.id), controlScheme.bindingGroup);
		this.restoreDefaultsButton.gameObject.SetActive(!flag2);
		this.restoreDefaultsButton.onClick.RemoveAllListeners();
		this.restoreDefaultsButton.onClick.AddListener(delegate
		{
			MonoSingleton<InputManager>.Instance.ResetToDefault(action, controlScheme);
			this.RebuildBindings(action, controlScheme);
		});
		Navigation navigation = this.selectable.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnRight = this.bindingButtons[0];
		this.selectable.navigation = navigation;
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x0001F1EC File Offset: 0x0001D3EC
	private ValueTuple<Button, TextMeshProUGUI, Image> BuildNewBindButton()
	{
		ValueTuple<Button, TextMeshProUGUI, Image, TooltipOnHover> valueTuple = this.BuildBindingButton("+");
		Button item = valueTuple.Item1;
		TextMeshProUGUI item2 = valueTuple.Item2;
		Image item3 = valueTuple.Item3;
		item2.color = this.faintTextColor;
		item2.fontSizeMax = 27f;
		return new ValueTuple<Button, TextMeshProUGUI, Image>(item, item2, item3);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x0001F23C File Offset: 0x0001D43C
	private string GenerateTooltip(InputAction action, InputBinding binding, InputBinding[] conflicts)
	{
		string text = action.GetBindingDisplayStringWithoutOverride(binding, InputBinding.DisplayStringOptions.DontIncludeInteractions).ToUpper();
		string text2 = "<color=red>" + text + " IS BOUND MULTIPLE TIMES:";
		HashSet<string> hashSet = new HashSet<string>();
		foreach (InputBinding inputBinding in conflicts)
		{
			if (!hashSet.Contains(inputBinding.action))
			{
				text2 += "<br>";
				text2 = text2 + "- " + inputBinding.action.ToUpper();
				hashSet.Add(inputBinding.action);
			}
		}
		return text2 + "</color>";
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001F2DC File Offset: 0x0001D4DC
	private ValueTuple<Button, TextMeshProUGUI, Image, TooltipOnHover> BuildBindingButton(string text)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.bindingButtonTemplate, this.bindingButtonParent);
		TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
		Button component = gameObject.GetComponent<Button>();
		Image component2 = gameObject.GetComponent<Image>();
		TooltipOnHover component3 = gameObject.GetComponent<TooltipOnHover>();
		componentInChildren.text = text;
		this.bindingButtons.Add(component);
		gameObject.SetActive(true);
		return new ValueTuple<Button, TextMeshProUGUI, Image, TooltipOnHover>(component, componentInChildren, component2, component3);
	}

	// Token: 0x0400060D RID: 1549
	public TextMeshProUGUI actionText;

	// Token: 0x0400060E RID: 1550
	public Button restoreDefaultsButton;

	// Token: 0x0400060F RID: 1551
	public GameObject bindingButtonTemplate;

	// Token: 0x04000610 RID: 1552
	public Transform bindingButtonParent;

	// Token: 0x04000611 RID: 1553
	public Selectable selectable;

	// Token: 0x04000612 RID: 1554
	public GameObject blocker;

	// Token: 0x04000613 RID: 1555
	private List<Button> bindingButtons = new List<Button>();

	// Token: 0x04000614 RID: 1556
	private bool selected;

	// Token: 0x04000615 RID: 1557
	private readonly Color faintTextColor = new Color(1f, 1f, 1f, 0.15f);
}
