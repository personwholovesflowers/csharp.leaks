using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StanleyUI
{
	// Token: 0x02000200 RID: 512
	public class StanleyMenuKeybindingSelector : MonoBehaviour, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00035C27 File Offset: 0x00033E27
		private PlayerAction Action
		{
			get
			{
				return Singleton<GameMaster>.Instance.stanleyActions.GetPlayerActionByName(this.actionName);
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00035C3E File Offset: 0x00033E3E
		public void Awake()
		{
			Singleton<GameMaster>.Instance.stanleyActions.OnKeyBindingsLoaded += this.UpdateKeyText;
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00035C5C File Offset: 0x00033E5C
		private void Start()
		{
			this.SetState(StanleyMenuKeybindingSelector.State.Normal);
			this.listeningOptions = new BindingListenOptions
			{
				MaxAllowedBindingsPerType = 1U,
				IncludeControllers = false,
				IncludeUnknownControllers = false,
				IncludeNonStandardControls = false,
				IncludeKeys = true,
				IncludeModifiersAsFirstClassKeys = true,
				IncludeMouseButtons = true,
				IncludeMouseScrollWheel = false,
				UnsetDuplicateBindingsOnSet = false,
				AllowDuplicateBindingsPerSet = true,
				RejectRedundantBindings = true,
				OnBindingFound = new Func<PlayerAction, BindingSource, bool>(this.OnBindingFound),
				OnBindingAdded = new Action<PlayerAction, BindingSource>(this.OnBindingAdded),
				OnBindingEnded = new Action<PlayerAction>(this.OnBindingEnded)
			};
			this.UpdateKeyText();
			StringConfigurable stringConfigurable = this.allKeybindingsConfigurable;
			stringConfigurable.OnValueChanged = (Action<LiveData>)Delegate.Combine(stringConfigurable.OnValueChanged, new Action<LiveData>(this.KeyBindingsChanged));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00035D2B File Offset: 0x00033F2B
		private void KeyBindingsChanged(LiveData live)
		{
			this.UpdateKeyText();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00035D33 File Offset: 0x00033F33
		private void OnBindingAdded(PlayerAction action, BindingSource binding)
		{
			this.SetState(StanleyMenuKeybindingSelector.State.Normal);
			Singleton<GameMaster>.Instance.stanleyActions.SaveCustomKeyBindings(this.allKeybindingsConfigurable);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00035D51 File Offset: 0x00033F51
		private bool OnBindingFound(PlayerAction action, BindingSource binding)
		{
			if (binding == new KeyBindingSource(new Key[] { Key.Escape }) || this.BindingUsedAlready(binding))
			{
				action.StopListeningForBinding();
				this.UpdateKeyText();
				this.SetState(StanleyMenuKeybindingSelector.State.Normal);
				return false;
			}
			return true;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00035D8C File Offset: 0x00033F8C
		private bool BindingUsedAlready(BindingSource binding)
		{
			using (IEnumerator<PlayerAction> enumerator = Singleton<GameMaster>.Instance.stanleyActions.UsedInGameActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasBinding(binding))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00035DEC File Offset: 0x00033FEC
		private void OnBindingEnded(PlayerAction action)
		{
			this.ReenableAllInputs();
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00035DF4 File Offset: 0x00033FF4
		private void ReenableAllInputs()
		{
			Singleton<GameMaster>.Instance.stanleyActions.Enabled = true;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00035E06 File Offset: 0x00034006
		private void DisableAllInputs()
		{
			Singleton<GameMaster>.Instance.stanleyActions.Enabled = false;
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00035E18 File Offset: 0x00034018
		public void UpdateKeyText()
		{
			ReadOnlyCollection<BindingSource> bindings = this.Action.Bindings;
			if (bindings.Count >= this.bindingIndex)
			{
				BindingSource bindingSource = bindings[this.bindingIndex];
				if (bindingSource.DeviceClass == InputDeviceClass.Keyboard)
				{
					Key include = (bindingSource as KeyBindingSource).Control.GetInclude(0);
					string text = (StanleyActions.KeyControllerPair.HasSprite(include) ? "150" : "100");
					this.uiText.text = string.Concat(new string[]
					{
						"<size=",
						text,
						"%>",
						StanleyActions.KeyControllerPair.KeySpriteNameTag(include),
						"<size=100%>"
					});
					return;
				}
				if (bindingSource.DeviceClass == InputDeviceClass.Mouse)
				{
					this.uiText.text = "<size=150%>" + StanleyActions.KeyControllerPair.MouseSpriteNameTag((bindingSource as MouseBindingSource).Control) + "<size=100%>";
					return;
				}
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00035EF3 File Offset: 0x000340F3
		public void Activate()
		{
			if (this.state == StanleyMenuKeybindingSelector.State.Normal)
			{
				this.SetState(StanleyMenuKeybindingSelector.State.WaitingForKey);
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00035F04 File Offset: 0x00034104
		public void OnDeselect(BaseEventData eventData)
		{
			if (this.Action.IsListeningForBinding)
			{
				this.Action.StopListeningForBinding();
				this.SetState(StanleyMenuKeybindingSelector.State.Normal);
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00035F28 File Offset: 0x00034128
		private void SetState(StanleyMenuKeybindingSelector.State newState)
		{
			this.state = newState;
			if (this.state == StanleyMenuKeybindingSelector.State.WaitingForKey)
			{
				this.uiText.gameObject.SetActive(false);
				this.pressAnyKey.gameObject.SetActive(true);
				this.Action.ListenOptions = this.listeningOptions;
				this.Action.ListenForBindingReplacing(this.Action.Bindings[this.bindingIndex]);
				this.DisableAllInputs();
				return;
			}
			this.UpdateKeyText();
			this.uiText.gameObject.SetActive(true);
			this.pressAnyKey.gameObject.SetActive(false);
		}

		// Token: 0x04000B5D RID: 2909
		public string actionName = "";

		// Token: 0x04000B5E RID: 2910
		[Header("0 for main, 1 for alt")]
		public int bindingIndex;

		// Token: 0x04000B5F RID: 2911
		public StringConfigurable allKeybindingsConfigurable;

		// Token: 0x04000B60 RID: 2912
		public TextMeshProUGUI uiText;

		// Token: 0x04000B61 RID: 2913
		public TextMeshProUGUI pressAnyKey;

		// Token: 0x04000B62 RID: 2914
		[Header("Debug")]
		public StanleyMenuKeybindingSelector.State state;

		// Token: 0x04000B63 RID: 2915
		private BindingListenOptions listeningOptions;

		// Token: 0x0200041E RID: 1054
		public enum State
		{
			// Token: 0x04001565 RID: 5477
			Normal,
			// Token: 0x04001566 RID: 5478
			WaitingForKey
		}
	}
}
