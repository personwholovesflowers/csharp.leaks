using System;
using System.Collections.Generic;
using plog;
using UnityEngine.InputSystem;

namespace GameConsole
{
	// Token: 0x0200059F RID: 1439
	public class Binds
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x00104AAB File Offset: 0x00102CAB
		private Logger Log { get; } = new Logger("Binds");

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x00104AB3 File Offset: 0x00102CB3
		public bool OpenPressed
		{
			get
			{
				return this.SafeWasPerformed("open");
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x00104AC0 File Offset: 0x00102CC0
		public bool SubmitPressed
		{
			get
			{
				return this.SafeWasPerformed("submit");
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x00104ACD File Offset: 0x00102CCD
		public bool AutocompletePressed
		{
			get
			{
				return this.SafeWasPerformed("autocomplete");
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x00104ADA File Offset: 0x00102CDA
		public bool CommandHistoryUpPressed
		{
			get
			{
				return this.SafeWasPerformed("command_history_up");
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600203D RID: 8253 RVA: 0x00104AE7 File Offset: 0x00102CE7
		public bool CommandHistoryDownPressed
		{
			get
			{
				return this.SafeWasPerformed("command_history_down");
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x00104AF4 File Offset: 0x00102CF4
		public bool ScrollUpPressed
		{
			get
			{
				return this.SafeWasPerformed("scroll_up");
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00104B01 File Offset: 0x00102D01
		public bool ScrollDownPressed
		{
			get
			{
				return this.SafeWasPerformed("scroll_down");
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x00104B0E File Offset: 0x00102D0E
		public bool ScrollToBottomPressed
		{
			get
			{
				return this.SafeWasPerformed("scroll_to_bottom");
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x00104B1B File Offset: 0x00102D1B
		public bool ScrollToTopPressed
		{
			get
			{
				return this.SafeWasPerformed("scroll_to_top");
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x00104B28 File Offset: 0x00102D28
		public bool ScrollUpHeld
		{
			get
			{
				return this.SafeIsHeld("scroll_up");
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x00104B35 File Offset: 0x00102D35
		public bool ScrollDownHeld
		{
			get
			{
				return this.SafeIsHeld("scroll_down");
			}
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00104B44 File Offset: 0x00102D44
		private bool SafeWasPerformed(string key)
		{
			InputActionState inputActionState;
			return this.registeredBinds != null && this.registeredBinds.TryGetValue(key, out inputActionState) && inputActionState.WasPerformedThisFrame;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x00104B74 File Offset: 0x00102D74
		private bool SafeIsHeld(string key)
		{
			InputActionState inputActionState;
			return this.registeredBinds != null && this.registeredBinds.TryGetValue(key, out inputActionState) && inputActionState.IsPressed;
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x00104BA4 File Offset: 0x00102DA4
		public void Initialize()
		{
			this.Log.Parent = Console.Log;
			this.registeredBinds = new Dictionary<string, InputActionState>();
			foreach (KeyValuePair<string, string> keyValuePair in this.defaultBinds)
			{
				InputActionState inputActionState = new InputActionState(new InputAction(keyValuePair.Key, InputActionType.Value, null, null, null, null));
				this.registeredBinds.Add(keyValuePair.Key, inputActionState);
				inputActionState.Action.AddBinding(MonoSingleton<PrefsManager>.Instance.GetString("consoleBinding." + keyValuePair.Key, keyValuePair.Value), null, null, null).WithGroup("Keyboard");
				inputActionState.Action.Enable();
			}
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00104C84 File Offset: 0x00102E84
		public void Rebind(string key, string bind)
		{
			if (!this.defaultBinds.ContainsKey(key))
			{
				this.Log.Info("Invalid console bind key: " + key, null, null, null);
				return;
			}
			string text = "consoleBinding." + key;
			MonoSingleton<PrefsManager>.Instance.SetString(text, bind);
			InputActionState inputActionState;
			if (this.registeredBinds.TryGetValue(key, out inputActionState))
			{
				inputActionState.Action.Disable();
				inputActionState.Action.Dispose();
			}
			inputActionState = new InputActionState(new InputAction(key, InputActionType.Value, null, null, null, null));
			this.registeredBinds[key] = inputActionState;
			inputActionState.Action.AddBinding(bind, null, null, null).WithGroup("Keyboard");
			inputActionState.Action.Enable();
			MonoSingleton<Console>.Instance.UpdateDisplayString();
		}

		// Token: 0x04002CA7 RID: 11431
		public Dictionary<string, InputActionState> registeredBinds;

		// Token: 0x04002CA8 RID: 11432
		public Dictionary<string, string> defaultBinds = new Dictionary<string, string>
		{
			{ "open", "/Keyboard/f8" },
			{ "submit", "/Keyboard/enter" },
			{ "command_history_up", "/Keyboard/upArrow" },
			{ "command_history_down", "/Keyboard/downArrow" },
			{ "scroll_up", "/Keyboard/pageUp" },
			{ "scroll_down", "/Keyboard/pageUp" },
			{ "scroll_to_bottom", "/Keyboard/home" },
			{ "scroll_to_top", "/Keyboard/end" },
			{ "autocomplete", "/Keyboard/tab" }
		};
	}
}
