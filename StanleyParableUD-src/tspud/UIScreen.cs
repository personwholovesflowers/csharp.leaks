using System;
using StanleyUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E2 RID: 482
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Animator))]
public class UIScreen : MonoBehaviour, ISelectableHolderScreen
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00033897 File Offset: 0x00031A97
	// (set) Token: 0x06000B07 RID: 2823 RVA: 0x0003389F File Offset: 0x00031A9F
	public bool active { get; private set; }

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000B08 RID: 2824 RVA: 0x000338A8 File Offset: 0x00031AA8
	public Selectable DefaultSelectable
	{
		get
		{
			if (!(this.defaultSelectable == null))
			{
				return this.defaultSelectable.GetComponent<Selectable>();
			}
			return null;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06000B09 RID: 2825 RVA: 0x000338C5 File Offset: 0x00031AC5
	// (set) Token: 0x06000B0A RID: 2826 RVA: 0x000338CD File Offset: 0x00031ACD
	public Selectable LastSelectable { get; set; }

	// Token: 0x06000B0B RID: 2827 RVA: 0x000338D8 File Offset: 0x00031AD8
	private void Awake()
	{
		this.cGroup = base.GetComponent<CanvasGroup>();
		this.childCanvases = base.GetComponentsInChildren<Canvas>();
		if (this.startHidden)
		{
			this.cGroup.alpha = 0f;
			this.cGroup.interactable = false;
			this.cGroup.blocksRaycasts = false;
			this.SetChildCanvases(false);
		}
		else
		{
			this.OnCall();
		}
		if (this.reference != null)
		{
			UIScreenReference uiscreenReference = this.reference;
			uiscreenReference.OnCall = (Action)Delegate.Combine(uiscreenReference.OnCall, new Action(this.OnCall));
			UIScreenReference uiscreenReference2 = this.reference;
			uiscreenReference2.OnClose = (Action)Delegate.Combine(uiscreenReference2.OnClose, new Action(this.OnClose));
			UIScreenReference uiscreenReference3 = this.reference;
			uiscreenReference3.OnShow = (Action)Delegate.Combine(uiscreenReference3.OnShow, new Action(this.OnShow));
			UIScreenReference uiscreenReference4 = this.reference;
			uiscreenReference4.OnHide = (Action)Delegate.Combine(uiscreenReference4.OnHide, new Action(this.OnHide));
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x000339EC File Offset: 0x00031BEC
	private void SetChildCanvases(bool status)
	{
		Canvas[] array = this.childCanvases;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].gameObject.SetActive(status);
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00033A1C File Offset: 0x00031C1C
	private void OnDestroy()
	{
		UIScreenReference uiscreenReference = this.reference;
		uiscreenReference.OnCall = (Action)Delegate.Remove(uiscreenReference.OnCall, new Action(this.OnCall));
		UIScreenReference uiscreenReference2 = this.reference;
		uiscreenReference2.OnClose = (Action)Delegate.Remove(uiscreenReference2.OnClose, new Action(this.OnClose));
		UIScreenReference uiscreenReference3 = this.reference;
		uiscreenReference3.OnShow = (Action)Delegate.Remove(uiscreenReference3.OnShow, new Action(this.OnShow));
		UIScreenReference uiscreenReference4 = this.reference;
		uiscreenReference4.OnHide = (Action)Delegate.Remove(uiscreenReference4.OnHide, new Action(this.OnHide));
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00033AC8 File Offset: 0x00031CC8
	private void Update()
	{
		if (this.active && this.canBeCanceled && this.backButton != null && Singleton<GameMaster>.Instance.stanleyActions.MenuBack.WasPressed)
		{
			ExecuteEvents.Execute<IPointerClickHandler>(this.backButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
		}
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x00033B29 File Offset: 0x00031D29
	private void OnCall()
	{
		this.SetChildCanvases(true);
		StanleyInputModuleAssistant.RegisterScreenAsNewlyVisible(base.gameObject, true);
		this.OnCallEvent.Invoke();
		this.active = true;
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x00033B50 File Offset: 0x00031D50
	private void OnClose()
	{
		this.SetChildCanvases(false);
		this.OnCloseEvent.Invoke();
		this.active = false;
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00033B6B File Offset: 0x00031D6B
	private void OnHide()
	{
		this.SetChildCanvases(false);
		this.OnHideEvent.Invoke();
		this.active = false;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00033B86 File Offset: 0x00031D86
	private void OnShow()
	{
		this.SetChildCanvases(true);
		StanleyInputModuleAssistant.RegisterScreenAsNewlyVisible(base.gameObject, false);
		this.OnShowEvent.Invoke();
		this.active = true;
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00033BAD File Offset: 0x00031DAD
	public void CallReference()
	{
		this.reference.Call();
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00033BBA File Offset: 0x00031DBA
	public void CloseReference()
	{
		this.reference.Close();
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00033BC7 File Offset: 0x00031DC7
	public void MoveToScreenWithReference(UIScreenReference nextScreen)
	{
		nextScreen.CallWithPrevious(this.reference);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00033BD5 File Offset: 0x00031DD5
	public void MoveToScreen(UIScreenReference nextScreen)
	{
		nextScreen.Call();
		this.OnClose();
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00033BE3 File Offset: 0x00031DE3
	public void MoveToPrevious()
	{
		this.reference.CloseAndCallPrevious();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00033BF0 File Offset: 0x00031DF0
	public void MoveToPreviousIfActive()
	{
		if (this.active)
		{
			this.reference.CloseAndCallPrevious();
		}
	}

	// Token: 0x04000AE5 RID: 2789
	[SerializeField]
	private UIScreenReference reference;

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	private bool startHidden = true;

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private bool canBeCanceled;

	// Token: 0x04000AE8 RID: 2792
	[SerializeField]
	private UnityEvent OnCallEvent;

	// Token: 0x04000AE9 RID: 2793
	[SerializeField]
	private UnityEvent OnCloseEvent;

	// Token: 0x04000AEA RID: 2794
	[SerializeField]
	private UnityEvent OnShowEvent;

	// Token: 0x04000AEB RID: 2795
	[SerializeField]
	private UnityEvent OnHideEvent;

	// Token: 0x04000AEC RID: 2796
	[SerializeField]
	private Selectable backButton;

	// Token: 0x04000AED RID: 2797
	private CanvasGroup cGroup;

	// Token: 0x04000AEE RID: 2798
	[Header("Controller Selection Stuff")]
	[SerializeField]
	private GameObject defaultSelectable;

	// Token: 0x04000AF0 RID: 2800
	[SerializeField]
	private Canvas[] childCanvases;
}
