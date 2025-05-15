using System;
using System.Collections.Generic;
using System.Linq;
using plog;
using UnityEngine.InputSystem;

// Token: 0x02000269 RID: 617
public static class InputExtensions
{
	// Token: 0x06000D8C RID: 3468 RVA: 0x000668DC File Offset: 0x00064ADC
	public static string GetBindingDisplayStringWithoutOverride(this InputAction action, InputBinding binding, InputBinding.DisplayStringOptions options = (InputBinding.DisplayStringOptions)0)
	{
		if (binding.isPartOfComposite)
		{
			return binding.ToDisplayString(InputBinding.DisplayStringOptions.DontIncludeInteractions | InputBinding.DisplayStringOptions.IgnoreBindingOverrides, null);
		}
		string overridePath = binding.overridePath;
		binding.overridePath = null;
		action.ApplyBindingOverride(binding);
		string text = action.GetBindingDisplayString(binding, InputBinding.DisplayStringOptions.DontIncludeInteractions | InputBinding.DisplayStringOptions.IgnoreBindingOverrides).ToUpper();
		binding.overridePath = overridePath;
		action.ApplyBindingOverride(binding);
		return text;
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x00066934 File Offset: 0x00064B34
	public static void WipeAction(this InputAction action, string controlScheme)
	{
		new List<InputBinding>();
		InputActionSetupExtensions.BindingSyntax bindingSyntax = action.ChangeBindingWithGroup(controlScheme);
		while (bindingSyntax.valid)
		{
			bindingSyntax.Erase();
			bindingSyntax = action.ChangeBindingWithGroup(controlScheme);
		}
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0006696C File Offset: 0x00064B6C
	public static bool IsActionEqual(this InputAction action, InputAction baseAction, string controlScheme = null)
	{
		List<InputBinding> list = action.bindings.ToList<InputBinding>();
		List<InputBinding> list2 = baseAction.bindings.ToList<InputBinding>();
		if (controlScheme != null)
		{
			list = list.Where((InputBinding bind) => action.BindingHasGroup(bind, controlScheme)).ToList<InputBinding>();
			list2 = list2.Where((InputBinding bind) => baseAction.BindingHasGroup(bind, controlScheme)).ToList<InputBinding>();
		}
		if (list.Count != list2.Count)
		{
			return false;
		}
		for (int i = 0; i < list.Count; i++)
		{
			InputBinding inputBinding = list[i];
			InputBinding inputBinding2 = list2[i];
			if (!inputBinding.IsBindingEqual(inputBinding2))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00066A3C File Offset: 0x00064C3C
	public static bool IsBindingEqual(this InputBinding binding, InputBinding other)
	{
		return global::InputExtensions.AreStringsEqual(other.effectivePath, binding.effectivePath) && global::InputExtensions.AreStringsEqual(other.effectiveInteractions, binding.effectiveInteractions) && global::InputExtensions.AreStringsEqual(other.effectiveProcessors, binding.effectiveProcessors);
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x00066A88 File Offset: 0x00064C88
	public static bool BindingHasGroup(this InputAction action, InputBinding binding, string group)
	{
		return action.BindingHasGroup(action.GetBindingIndex(binding), group);
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00066A98 File Offset: 0x00064C98
	public static bool BindingHasGroup(this InputAction action, int i, string group)
	{
		InputBinding inputBinding = action.bindings[i];
		if (inputBinding.isComposite && action.bindings.Count > i + 1)
		{
			inputBinding = action.bindings[i + 1];
		}
		return inputBinding.groups != null && inputBinding.groups.Contains(group);
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x00066AFC File Offset: 0x00064CFC
	public static int[] GetBindingsWithGroup(this InputAction action, string group)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < action.bindings.Count; i++)
		{
			if (!action.bindings[i].isPartOfComposite && action.BindingHasGroup(i, group))
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00066B58 File Offset: 0x00064D58
	private static bool AreStringsEqual(string str1, string str2)
	{
		return (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2)) || string.Equals(str1, str2);
	}

	// Token: 0x04001211 RID: 4625
	private const bool DebugLogging = false;

	// Token: 0x04001212 RID: 4626
	private static readonly Logger Log = new Logger("Input");
}
