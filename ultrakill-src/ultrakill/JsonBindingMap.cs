using System;
using System.Collections.Generic;
using plog;
using UnityEngine.InputSystem;

// Token: 0x0200026C RID: 620
public class JsonBindingMap
{
	// Token: 0x06000D9A RID: 3482 RVA: 0x00066CE0 File Offset: 0x00064EE0
	public static JsonBindingMap From(InputActionAsset asset, InputControlScheme scheme)
	{
		JsonBindingMap jsonBindingMap = new JsonBindingMap
		{
			controlScheme = scheme.bindingGroup
		};
		foreach (InputAction inputAction in asset)
		{
			jsonBindingMap.AddAction(inputAction);
		}
		return jsonBindingMap;
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x00066D3C File Offset: 0x00064F3C
	public static JsonBindingMap From(InputActionAsset asset, InputActionAsset baseAsset, InputControlScheme scheme)
	{
		JsonBindingMap jsonBindingMap = new JsonBindingMap
		{
			controlScheme = scheme.bindingGroup
		};
		foreach (InputAction inputAction in asset)
		{
			InputAction inputAction2 = baseAsset.FindAction(inputAction.id);
			if (!inputAction.IsActionEqual(inputAction2, scheme.bindingGroup))
			{
				jsonBindingMap.AddAction(inputAction);
			}
		}
		return jsonBindingMap;
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00066DB8 File Offset: 0x00064FB8
	public void ApplyTo(InputActionAsset asset)
	{
		foreach (KeyValuePair<string, List<JsonBinding>> keyValuePair in this.modifiedActions)
		{
			string text = keyValuePair.Key;
			List<JsonBinding> value = keyValuePair.Value;
			string text2;
			if (JsonBindingMap.bindAliases.TryGetValue(text, out text2))
			{
				text = text2;
			}
			InputAction inputAction = asset.FindAction(text, false);
			if (inputAction == null)
			{
				JsonBindingMap.Log.Warning("Action " + text + " was found in saved bindings, but does not exist (action == null). Ignoring...", null, null, null);
				break;
			}
			inputAction.WipeAction(this.controlScheme);
			foreach (JsonBinding jsonBinding in value)
			{
				if (jsonBinding.isComposite)
				{
					if (jsonBinding.parts.Count != 0)
					{
						InputActionSetupExtensions.CompositeSyntax compositeSyntax = inputAction.AddCompositeBinding(jsonBinding.path, null, null);
						foreach (KeyValuePair<string, string> keyValuePair2 in jsonBinding.parts)
						{
							compositeSyntax.With(keyValuePair2.Key, keyValuePair2.Value, this.controlScheme, null);
						}
						inputAction.ChangeBinding(compositeSyntax.bindingIndex).WithGroup(this.controlScheme);
					}
				}
				else
				{
					inputAction.AddBinding(jsonBinding.path, null, null, this.controlScheme);
				}
			}
		}
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x00066F8C File Offset: 0x0006518C
	public void AddAction(InputAction action)
	{
		this.modifiedActions.Add(action.name, JsonBinding.FromAction(action, this.controlScheme));
	}

	// Token: 0x04001219 RID: 4633
	public static readonly Logger Log = new Logger("JsonBindingMap");

	// Token: 0x0400121A RID: 4634
	public string controlScheme;

	// Token: 0x0400121B RID: 4635
	public static Dictionary<string, string> bindAliases = new Dictionary<string, string>
	{
		{ "Slot 1", "Revolver" },
		{ "Slot 2", "Shotgun" },
		{ "Slot 3", "Nailgun" },
		{ "Slot 4", "Railcannon" },
		{ "Slot 5", "Rocket Launcher" },
		{ "Change Variation", "Next Variation" },
		{ "Last Weapon", "Last Used Weapon" }
	};

	// Token: 0x0400121C RID: 4636
	public Dictionary<string, List<JsonBinding>> modifiedActions = new Dictionary<string, List<JsonBinding>>();
}
