using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

// Token: 0x020000AC RID: 172
public class CheatBinds : MonoSingleton<CheatBinds>
{
	// Token: 0x06000359 RID: 857 RVA: 0x00015270 File Offset: 0x00013470
	protected override void Awake()
	{
		base.Awake();
		this.registeredCheatBinds = new Dictionary<string, InputActionState>();
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00015284 File Offset: 0x00013484
	public void RestoreBinds(Dictionary<string, List<ICheat>> allRegisteredCheats)
	{
		foreach (KeyValuePair<string, List<ICheat>> keyValuePair in allRegisteredCheats)
		{
			foreach (ICheat cheat in keyValuePair.Value)
			{
				string @string = MonoSingleton<PrefsManager>.Instance.GetString("cheatBinding." + cheat.Identifier, string.Empty);
				if (string.IsNullOrEmpty(@string))
				{
					if (this.defaultBinds.ContainsKey(cheat.Identifier))
					{
						this.AddBinding(cheat.Identifier, this.defaultBinds[cheat.Identifier]);
					}
				}
				else if (!string.IsNullOrEmpty(@string) && @string != "blank")
				{
					this.AddBinding(cheat.Identifier, @string);
				}
			}
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00015394 File Offset: 0x00013594
	public void ResetCheatBind(string cheatIdentifier)
	{
		if (this.isRebinding)
		{
			this.CancelRebind();
		}
		if (this.registeredCheatBinds.ContainsKey(cheatIdentifier))
		{
			this.registeredCheatBinds[cheatIdentifier].Action.Disable();
			this.registeredCheatBinds[cheatIdentifier].Action.Dispose();
			this.registeredCheatBinds.Remove(cheatIdentifier);
		}
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cheatBinding." + cheatIdentifier);
		if (this.defaultBinds.ContainsKey(cheatIdentifier))
		{
			this.AddBinding(cheatIdentifier, this.defaultBinds[cheatIdentifier]);
		}
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0001542C File Offset: 0x0001362C
	public void CancelRebind()
	{
		this.rebindAction.Disable();
		this.rebindAction.Dispose();
		MonoSingleton<OptionsManager>.Instance.dontUnpause = false;
		this.isRebinding = false;
		MonoSingleton<CheatsManager>.Instance.UpdateCheatState(this.rebindCheat);
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00015468 File Offset: 0x00013668
	public void SetupRebind(ICheat targetCheat)
	{
		CheatBinds.<>c__DisplayClass10_0 CS$<>8__locals1 = new CheatBinds.<>c__DisplayClass10_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.targetCheat = targetCheat;
		this.isRebinding = true;
		MonoSingleton<OptionsManager>.Instance.dontUnpause = true;
		this.rebindCheat = CS$<>8__locals1.targetCheat;
		this.rebindAction = new InputAction(null, InputActionType.Value, "<Keyboard>/*", null, null, null);
		this.rebindAction.AddBinding("<Mouse>/<Button>", null, null, null);
		this.rebindAction.performed += CS$<>8__locals1.<SetupRebind>g__RebindHandler|0;
		this.rebindAction.Enable();
	}

	// Token: 0x0600035E RID: 862 RVA: 0x000154F4 File Offset: 0x000136F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.registeredCheatBinds == null)
		{
			return;
		}
		foreach (KeyValuePair<string, InputActionState> keyValuePair in this.registeredCheatBinds)
		{
			keyValuePair.Value.Action.Disable();
			keyValuePair.Value.Action.Dispose();
		}
		this.registeredCheatBinds.Clear();
	}

	// Token: 0x0600035F RID: 863 RVA: 0x0001557C File Offset: 0x0001377C
	private void Rebind(string cheatIdentifier, string path)
	{
		if (this.registeredCheatBinds.ContainsKey(cheatIdentifier))
		{
			this.registeredCheatBinds[cheatIdentifier].Action.Disable();
			this.registeredCheatBinds[cheatIdentifier].Action.Dispose();
			this.registeredCheatBinds.Remove(cheatIdentifier);
		}
		this.AddBinding(cheatIdentifier, path);
		MonoSingleton<PrefsManager>.Instance.SetString("cheatBinding." + cheatIdentifier, path);
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000155F0 File Offset: 0x000137F0
	private void AddBinding(string cheatIdentifier, string path)
	{
		this.registeredCheatBinds.Add(cheatIdentifier, new InputActionState(new InputAction(cheatIdentifier, InputActionType.Value, null, null, null, null)));
		this.registeredCheatBinds[cheatIdentifier].Action.AddBinding(path, null, null, null).WithGroup("Keyboard");
		this.registeredCheatBinds[cheatIdentifier].Action.performed += delegate(InputAction.CallbackContext context)
		{
			MonoSingleton<CheatsManager>.Instance.HandleCheatBind(cheatIdentifier);
		};
		this.registeredCheatBinds[cheatIdentifier].Action.Enable();
	}

	// Token: 0x06000361 RID: 865 RVA: 0x000156A0 File Offset: 0x000138A0
	public string ResolveCheatKey(string cheatIdentifier)
	{
		if (!this.registeredCheatBinds.ContainsKey(cheatIdentifier))
		{
			return null;
		}
		return this.registeredCheatBinds[cheatIdentifier].Action.bindings[0].ToDisplayString((InputBinding.DisplayStringOptions)0, null);
	}

	// Token: 0x04000448 RID: 1096
	public Dictionary<string, InputActionState> registeredCheatBinds;

	// Token: 0x04000449 RID: 1097
	public bool isRebinding;

	// Token: 0x0400044A RID: 1098
	private readonly Dictionary<string, string> defaultBinds = new Dictionary<string, string>
	{
		{ "ultrakill.flight", "<Keyboard>/b" },
		{ "ultrakill.noclip", "<Keyboard>/v" },
		{ "ultrakill.blind-enemies", "<Keyboard>/m" },
		{ "ultrakill.infinite-wall-jumps", "<Keyboard>/n" },
		{ "ultrakill.no-weapon-cooldown", "<Keyboard>/c" },
		{ "ultrakill.keep-enabled", "<Keyboard>/o" },
		{ "ultrakill.teleport-menu", "<Keyboard>/l" },
		{ "ultrakill.spawner-arm", "<Keyboard>/p" },
		{ "ultrakill.disable-enemy-spawns", "<Keyboard>/i" },
		{ "ultrakill.sandbox.physics", "<Keyboard>/j" },
		{ "ultrakill.sandbox.snapping", "<Keyboard>/h" }
	};

	// Token: 0x0400044B RID: 1099
	private readonly string[] bannedBinds = new string[] { "/Keyboard/home", "/Keyboard/backquote", "/Mouse/press", "/Mouse/leftButton", "/Mouse/rightButton" };

	// Token: 0x0400044C RID: 1100
	private InputAction rebindAction;

	// Token: 0x0400044D RID: 1101
	private ICheat rebindCheat;
}
