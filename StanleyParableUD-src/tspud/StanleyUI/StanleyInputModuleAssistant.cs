using System;
using System.Collections;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StanleyUI
{
	// Token: 0x020001FA RID: 506
	[RequireComponent(typeof(InControlInputModule))]
	public class StanleyInputModuleAssistant : MonoBehaviour
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x000355C1 File Offset: 0x000337C1
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x000355C9 File Offset: 0x000337C9
		public ISelectableHolderScreen CurrentHolderScreen { get; private set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x000355D2 File Offset: 0x000337D2
		public static StanleyInputModuleAssistant Instance
		{
			get
			{
				return EventSystem.current.GetComponent<StanleyInputModuleAssistant>();
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000355E0 File Offset: 0x000337E0
		public static void SelectDefaultSelectable(bool forceSelectDefault)
		{
			if (!forceSelectDefault && StanleyInputModuleAssistant.Instance.CurrentHolderScreen.LastSelectable != null)
			{
				EventSystem.current.firstSelectedGameObject = StanleyInputModuleAssistant.Instance.CurrentHolderScreen.LastGameObjectOrNull();
			}
			else
			{
				EventSystem.current.firstSelectedGameObject = StanleyInputModuleAssistant.Instance.CurrentHolderScreen.DefaultGameObjectOrNull();
			}
			if (EventSystem.current.firstSelectedGameObject != null)
			{
				Selectable component = EventSystem.current.firstSelectedGameObject.GetComponent<Selectable>();
				StanleyInputModuleAssistant.Instance.StartCoroutine(StanleyInputModuleAssistant.WaitFrameAndSelect(component));
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00035670 File Offset: 0x00033870
		public static void RegisterScreenAsNewlyVisible(GameObject holderGameObject, bool forceSelectDefault = false)
		{
			ISelectableHolderScreen component = holderGameObject.GetComponent<ISelectableHolderScreen>();
			StanleyInputModuleAssistant.Instance.CurrentHolderScreen = component;
			StanleyInputModuleAssistant.SelectDefaultSelectable(forceSelectDefault);
			StanleyInputModuleAssistant.Instance.currentHolder_DEBUG = holderGameObject;
			StanleyInputModuleAssistant.Instance.currentLastSelectable_DEBUG = StanleyInputModuleAssistant.Instance.currentHolder_DEBUG.GetComponent<ISelectableHolderScreen>().LastSelectable;
			StanleyInputModuleAssistant.IScreenRegisterNotificationReciever[] components = holderGameObject.GetComponents<StanleyInputModuleAssistant.IScreenRegisterNotificationReciever>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].RegisteredScreenVisible();
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000356DB File Offset: 0x000338DB
		private static IEnumerator WaitFrameAndSelect(Selectable s)
		{
			EventSystem.current.SetSelectedGameObject(null);
			yield return null;
			EventSystem.current.SetSelectedGameObject(null);
			s.Select();
			yield break;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000356EA File Offset: 0x000338EA
		public static void RegisterUIElementSelection(Selectable selectedSelectable)
		{
			if (StanleyInputModuleAssistant.Instance.CurrentHolderScreen != null && StanleyInputModuleAssistant.Instance.CurrentHolderScreen.LastSelectable != null)
			{
				StanleyInputModuleAssistant.Instance.CurrentHolderScreen.LastSelectable = selectedSelectable;
				StanleyInputModuleAssistant.Instance.currentLastSelectable_DEBUG = selectedSelectable;
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0003572A File Offset: 0x0003392A
		private void Awake()
		{
			this.incontrolInput = base.GetComponent<InControlInputModule>();
			this.eventSystem = base.GetComponent<EventSystem>();
			this.incontrolInput.allowMouseInput = true;
			this.incontrolInput.allowTouchInput = true;
			this.incontrolInput.focusOnMouseHover = true;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00035768 File Offset: 0x00033968
		public void Move(MoveDirection direction)
		{
			AxisEventData axisEventData = new AxisEventData(EventSystem.current);
			axisEventData.moveDir = direction;
			axisEventData.selectedObject = EventSystem.current.currentSelectedGameObject;
			ExecuteEvents.Execute<IMoveHandler>(axisEventData.selectedObject, axisEventData, ExecuteEvents.moveHandler);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000357AC File Offset: 0x000339AC
		private void Update()
		{
			if (ReportUI.REPORTUIACTIVE)
			{
				return;
			}
			if (!GameMaster.PAUSEMENUACTIVE && !Singleton<GameMaster>.Instance.MenuManager.InAMenu)
			{
				return;
			}
			MoveDirection moveDirection = MoveDirection.None;
			if (Singleton<GameMaster>.Instance.stanleyActions.Enabled)
			{
				if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
				{
					moveDirection = MoveDirection.Down;
				}
				if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
				{
					moveDirection = MoveDirection.Up;
				}
				if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
				{
					moveDirection = MoveDirection.Left;
				}
				if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
				{
					moveDirection = MoveDirection.Right;
				}
			}
			if (moveDirection != MoveDirection.None)
			{
				GameMaster.CursorVisible = false;
				if (EventSystem.current.currentSelectedGameObject == null)
				{
					StanleyInputModuleAssistant.SelectDefaultSelectable(false);
				}
				else
				{
					float realtimeSinceStartup = Time.realtimeSinceStartup;
					if (this.lastMovePressed == MoveDirection.None)
					{
						this.Move(moveDirection);
						this.nextMoveRepeatTime = realtimeSinceStartup + base.GetComponent<InControlInputModule>().moveRepeatFirstDuration;
					}
					else if (realtimeSinceStartup >= this.nextMoveRepeatTime)
					{
						this.Move(moveDirection);
						this.nextMoveRepeatTime = realtimeSinceStartup + base.GetComponent<InControlInputModule>().moveRepeatDelayDuration;
					}
				}
			}
			else
			{
				this.nextMoveRepeatTime = 0f;
			}
			this.lastMovePressed = moveDirection;
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				ExecuteEvents.Execute<ISubmitHandler>(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
			}
		}

		// Token: 0x04000B44 RID: 2884
		private InControlInputModule incontrolInput;

		// Token: 0x04000B45 RID: 2885
		[Header("DEBUG ONLY")]
		private GameObject currentHolder_DEBUG;

		// Token: 0x04000B46 RID: 2886
		private Selectable currentLastSelectable_DEBUG;

		// Token: 0x04000B47 RID: 2887
		private EventSystem eventSystem;

		// Token: 0x04000B48 RID: 2888
		private float nextMoveRepeatTime;

		// Token: 0x04000B49 RID: 2889
		private MoveDirection lastMovePressed = MoveDirection.None;

		// Token: 0x0200041B RID: 1051
		public interface IScreenRegisterNotificationReciever
		{
			// Token: 0x06001850 RID: 6224
			void RegisteredScreenVisible();
		}
	}
}
