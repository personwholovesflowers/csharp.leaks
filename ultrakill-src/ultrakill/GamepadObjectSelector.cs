using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02000212 RID: 530
[DefaultExecutionOrder(1000)]
public class GamepadObjectSelector : MonoBehaviour
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00050107 File Offset: 0x0004E307
	private GameObject target
	{
		get
		{
			if (!(this.mainTarget != null) || !this.mainTarget.activeInHierarchy)
			{
				return this.fallbackTarget;
			}
			return this.mainTarget;
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00050131 File Offset: 0x0004E331
	private void OnEnable()
	{
		if (!this.dontMarkTop)
		{
			this.SetTop();
		}
		if (this.selectOnEnable)
		{
			InputManager instance = MonoSingleton<InputManager>.Instance;
			if (((instance != null) ? instance.LastButtonDevice : null) is Gamepad)
			{
				this.Activate();
			}
		}
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00050168 File Offset: 0x0004E368
	private void OnDisable()
	{
		if (GamepadObjectSelector.s_Selectors.Count > 0 && GamepadObjectSelector.s_Selectors.Peek() == this && EventSystem.current != null)
		{
			InputManager instance = MonoSingleton<InputManager>.Instance;
			if (((instance != null) ? instance.LastButtonDevice : null) is Gamepad)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
		this.PopTop();
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x000501CC File Offset: 0x0004E3CC
	private void Update()
	{
		if (GamepadObjectSelector.s_Selectors.Count == 0)
		{
			return;
		}
		if (GamepadObjectSelector.s_Selectors.Peek() != this)
		{
			return;
		}
		if (!(MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad))
		{
			return;
		}
		if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.activeInHierarchy)
		{
			return;
		}
		foreach (GamepadObjectSelector gamepadObjectSelector in GamepadObjectSelector.s_Selectors)
		{
			if (this.topOnly && GamepadObjectSelector.s_Selectors.Peek() != this)
			{
				break;
			}
			if (gamepadObjectSelector)
			{
				if (gamepadObjectSelector.firstChild && gamepadObjectSelector.SelectFirstChild(gamepadObjectSelector.target) != null)
				{
					break;
				}
				Selectable selectable;
				if (!(gamepadObjectSelector.target == null) && gamepadObjectSelector.target.TryGetComponent<Selectable>(out selectable) && selectable.isActiveAndEnabled && (gamepadObjectSelector.allowNonInteractable || selectable.interactable))
				{
					EventSystem.current.SetSelectedGameObject(gamepadObjectSelector.target);
					break;
				}
			}
		}
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00050300 File Offset: 0x0004E500
	public void Activate()
	{
		if (this.target == null)
		{
			this.mainTarget = base.gameObject;
		}
		GameObject target = this.target;
		if (this.firstChild && base.gameObject.activeInHierarchy && target != null)
		{
			base.StartCoroutine(this.SelectFirstChildOnNextFrame(target));
			return;
		}
		if (base.gameObject.activeInHierarchy && target != null)
		{
			EventSystem.current.SetSelectedGameObject(target);
		}
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x0005037C File Offset: 0x0004E57C
	public static void DisableTop()
	{
		GamepadObjectSelector gamepadObjectSelector = GamepadObjectSelector.s_Selectors.Peek();
		if (gamepadObjectSelector != null)
		{
			gamepadObjectSelector.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x000503A9 File Offset: 0x0004E5A9
	public void PopTop()
	{
		if (GamepadObjectSelector.s_Selectors.Count > 0 && GamepadObjectSelector.s_Selectors.Peek() == this)
		{
			GamepadObjectSelector.s_Selectors.Pop();
		}
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x000503D5 File Offset: 0x0004E5D5
	public void SetTop()
	{
		if (GamepadObjectSelector.s_Selectors.Count == 0 || GamepadObjectSelector.s_Selectors.Peek() != this)
		{
			GamepadObjectSelector.s_Selectors.Push(this);
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00050400 File Offset: 0x0004E600
	public void SetMainTarget(Selectable sel)
	{
		this.mainTarget = sel.gameObject;
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0005040E File Offset: 0x0004E60E
	private IEnumerator SelectFirstChildOnNextFrame(GameObject obj)
	{
		yield return null;
		this.SelectFirstChild(obj);
		yield break;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00050424 File Offset: 0x0004E624
	private GameObject SelectFirstChild(GameObject obj)
	{
		if (obj == null)
		{
			return null;
		}
		foreach (Selectable selectable in obj.GetComponentsInChildren<Selectable>())
		{
			if (!(selectable.gameObject == obj) && selectable.isActiveAndEnabled && selectable.navigation.mode != Navigation.Mode.None && (this.allowNonInteractable || selectable.interactable))
			{
				obj = selectable.gameObject;
				break;
			}
		}
		EventSystem.current.SetSelectedGameObject(obj);
		return obj;
	}

	// Token: 0x04000ECE RID: 3790
	private static Stack<GamepadObjectSelector> s_Selectors = new Stack<GamepadObjectSelector>();

	// Token: 0x04000ECF RID: 3791
	[SerializeField]
	private bool selectOnEnable = true;

	// Token: 0x04000ED0 RID: 3792
	[SerializeField]
	private bool firstChild;

	// Token: 0x04000ED1 RID: 3793
	[SerializeField]
	private bool allowNonInteractable;

	// Token: 0x04000ED2 RID: 3794
	[SerializeField]
	private bool topOnly;

	// Token: 0x04000ED3 RID: 3795
	[SerializeField]
	private bool dontMarkTop;

	// Token: 0x04000ED4 RID: 3796
	[SerializeField]
	[FormerlySerializedAs("target")]
	private GameObject mainTarget;

	// Token: 0x04000ED5 RID: 3797
	[SerializeField]
	private GameObject fallbackTarget;
}
