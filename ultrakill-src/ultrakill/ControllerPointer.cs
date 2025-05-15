using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Token: 0x020000DD RID: 221
[DisallowMultipleComponent]
internal class ControllerPointer : MonoBehaviour
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600044E RID: 1102 RVA: 0x0001DD0F File Offset: 0x0001BF0F
	public UnityEvent OnPressed
	{
		get
		{
			return this.onPressed;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600044F RID: 1103 RVA: 0x0001DD17 File Offset: 0x0001BF17
	public UnityEvent OnReleased
	{
		get
		{
			return this.onReleased;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x06000450 RID: 1104 RVA: 0x0001DD1F File Offset: 0x0001BF1F
	public UnityEvent OnEnter
	{
		get
		{
			return this.onEnter;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001DD27 File Offset: 0x0001BF27
	public UnityEvent OnExit
	{
		get
		{
			return this.onExit;
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001DD30 File Offset: 0x0001BF30
	private void Awake()
	{
		if (this.onPressed == null)
		{
			this.onPressed = new UnityEvent();
		}
		if (this.onReleased == null)
		{
			this.onReleased = new UnityEvent();
		}
		if (this.onEnter == null)
		{
			this.onEnter = new UnityEvent();
		}
		if (this.onExit == null)
		{
			this.onExit = new UnityEvent();
		}
		this.results = new List<RaycastResult>();
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001DD94 File Offset: 0x0001BF94
	private void UpdateSlider()
	{
		Slider slider;
		if (base.TryGetComponent<Slider>(out slider))
		{
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
			{
				RectTransform component = slider.GetComponent<RectTransform>();
				Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
				Rect rect = component.rect;
				Vector2 vector2;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(component, vector, ControllerPointer.raycaster.eventCamera, out vector2))
				{
					if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && rect.Contains(vector2))
					{
						this.scrollState = true;
					}
					else if (!this.scrollState)
					{
						return;
					}
					float num = Mathf.InverseLerp(rect.x, rect.x + rect.width, vector2.x);
					slider.value = slider.minValue + num * (slider.maxValue - slider.minValue);
					return;
				}
			}
			else
			{
				this.scrollState = false;
			}
		}
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001DE9C File Offset: 0x0001C09C
	private void UpdateScrollbars()
	{
		ScrollRect scrollRect;
		if (base.TryGetComponent<ScrollRect>(out scrollRect))
		{
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
			{
				if (scrollRect.horizontal)
				{
					this.UpdateScrollbar(scrollRect.horizontalScrollbar);
				}
				if (scrollRect.vertical)
				{
					this.UpdateScrollbar(scrollRect.verticalScrollbar);
				}
			}
			else
			{
				this.scrollState = false;
			}
			RectTransform content = scrollRect.content;
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
			Vector2 vector2;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(content, vector, ControllerPointer.raycaster.eventCamera, out vector2) && content.rect.Contains(vector2))
			{
				if (scrollRect.horizontal)
				{
					scrollRect.horizontalScrollbar.value += Mouse.current.scroll.x.ReadValue() / 2f / content.rect.height;
				}
				if (scrollRect.vertical)
				{
					scrollRect.verticalScrollbar.value += Mouse.current.scroll.y.ReadValue() / 2f / content.rect.height;
				}
			}
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001DFE4 File Offset: 0x0001C1E4
	private void UpdateScrollbar(Scrollbar scroll)
	{
		RectTransform component = scroll.GetComponent<RectTransform>();
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
		Rect rect = component.rect;
		Vector2 vector2;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(component, vector, ControllerPointer.raycaster.eventCamera, out vector2))
		{
			if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame && rect.Contains(vector2))
			{
				this.scrollState = true;
			}
			else if (!this.scrollState)
			{
				return;
			}
			switch (scroll.direction)
			{
			case Scrollbar.Direction.LeftToRight:
				scroll.value = Mathf.InverseLerp(rect.x, rect.x + rect.width, vector2.x);
				return;
			case Scrollbar.Direction.RightToLeft:
				scroll.value = Mathf.InverseLerp(rect.x + rect.width, rect.x, vector2.x);
				break;
			case Scrollbar.Direction.BottomToTop:
				scroll.value = Mathf.InverseLerp(rect.y, rect.y + rect.height, vector2.y);
				return;
			case Scrollbar.Direction.TopToBottom:
				scroll.value = Mathf.InverseLerp(rect.y + rect.height, rect.y, vector2.y);
				return;
			default:
				return;
			}
		}
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001E12C File Offset: 0x0001C32C
	private void UpdateRaycasters()
	{
		if (ControllerPointer.raycasters != null && ControllerPointer.raycasters.Count > 0)
		{
			for (int i = ControllerPointer.raycasters.Count - 1; i >= 0; i--)
			{
				if (ControllerPointer.raycasters[i] != null && ControllerPointer.raycasters[i].gameObject.activeInHierarchy && ControllerPointer.raycasters[i].enabled)
				{
					ControllerPointer.raycaster = ControllerPointer.raycasters[i];
					return;
				}
			}
		}
		ControllerPointer.raycaster = null;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
	private void Update()
	{
		this.UpdateRaycasters();
		if (!EventSystem.current || !ControllerPointer.raycaster || !ControllerPointer.raycaster.eventCamera || (MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused))
		{
			return;
		}
		this.eventData = new PointerEventData(EventSystem.current)
		{
			button = PointerEventData.InputButton.Left,
			position = new Vector2((float)(ControllerPointer.raycaster ? ControllerPointer.raycaster.eventCamera.pixelWidth : Screen.width) / 2f, (float)(ControllerPointer.raycaster ? ControllerPointer.raycaster.eventCamera.pixelHeight : Screen.height) / 2f)
		};
		if (ControllerPointer.raycaster && ControllerPointer.ignoreFrame != Time.frameCount)
		{
			ControllerPointer.ignoreFrame = Time.frameCount;
			ControllerPointer.bestResult = null;
			this.results.Clear();
			ControllerPointer.raycaster.Raycast(this.eventData, this.results);
			foreach (RaycastResult raycastResult in this.results)
			{
				Text text;
				if (!raycastResult.gameObject.TryGetComponent<Text>(out text) && (ControllerPointer.bestResult == null || ControllerPointer.bestResult.Value.depth <= raycastResult.depth))
				{
					ControllerPointer.bestResult = new RaycastResult?(raycastResult);
				}
			}
		}
		this.UpdateEvents();
		this.UpdateSlider();
		this.UpdateScrollbars();
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001E364 File Offset: 0x0001C564
	private void UpdateEvents()
	{
		if (ControllerPointer.bestResult != null)
		{
			bool flag = this.entered;
			this.entered = ControllerPointer.bestResult.Value.gameObject == base.gameObject;
			if (this.entered && !flag)
			{
				ExecuteEvents.Execute<IPointerEnterHandler>(base.gameObject, this.eventData, ExecuteEvents.pointerEnterHandler);
				UnityEvent unityEvent = this.onEnter;
				if (unityEvent != null)
				{
					unityEvent.Invoke();
				}
			}
			if (this.entered && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
			{
				this.pointerDown = true;
				ExecuteEvents.Execute<IPointerDownHandler>(base.gameObject, this.eventData, ExecuteEvents.pointerDownHandler);
				ExecuteEvents.Execute<IPointerClickHandler>(base.gameObject, this.eventData, ExecuteEvents.pointerClickHandler);
				UnityEvent unityEvent2 = this.onPressed;
				if (unityEvent2 != null)
				{
					unityEvent2.Invoke();
				}
				this.dragPoint = new Vector2?(this.eventData.position);
			}
			if (this.pointerDown && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame)
			{
				this.pointerDown = false;
				ExecuteEvents.Execute<IPointerUpHandler>(base.gameObject, this.eventData, ExecuteEvents.pointerUpHandler);
				UnityEvent unityEvent3 = this.onReleased;
				if (unityEvent3 != null)
				{
					unityEvent3.Invoke();
				}
			}
			if (flag && !this.entered)
			{
				ExecuteEvents.Execute<IPointerExitHandler>(base.gameObject, this.eventData, ExecuteEvents.pointerExitHandler);
				UnityEvent unityEvent4 = this.onExit;
				if (unityEvent4 != null)
				{
					unityEvent4.Invoke();
				}
			}
			if (this.dragPoint != null)
			{
				Vector2 vector = this.eventData.position - this.dragPoint.Value;
				PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
				{
					button = PointerEventData.InputButton.Left,
					position = this.eventData.position,
					pressPosition = this.dragPoint.Value,
					delta = vector
				};
				if (this.pointerDown && this.entered && vector.sqrMagnitude >= this.dragThreshold * this.dragThreshold)
				{
					ExecuteEvents.Execute<IBeginDragHandler>(base.gameObject, pointerEventData, ExecuteEvents.beginDragHandler);
					this.dragging = true;
				}
				if (this.dragging)
				{
					ExecuteEvents.Execute<IDragHandler>(base.gameObject, pointerEventData, ExecuteEvents.dragHandler);
				}
				if (!this.pointerDown | !this.entered)
				{
					this.dragging = false;
					this.dragPoint = null;
					ExecuteEvents.Execute<IEndDragHandler>(base.gameObject, pointerEventData, ExecuteEvents.endDragHandler);
				}
			}
		}
	}

	// Token: 0x040005E9 RID: 1513
	private static RaycastResult? bestResult;

	// Token: 0x040005EA RID: 1514
	private PointerEventData eventData;

	// Token: 0x040005EB RID: 1515
	private static int ignoreFrame;

	// Token: 0x040005EC RID: 1516
	[SerializeField]
	private UnityEvent onPressed;

	// Token: 0x040005ED RID: 1517
	[SerializeField]
	private UnityEvent onReleased;

	// Token: 0x040005EE RID: 1518
	[SerializeField]
	private UnityEvent onEnter;

	// Token: 0x040005EF RID: 1519
	[SerializeField]
	private UnityEvent onExit;

	// Token: 0x040005F0 RID: 1520
	[SerializeField]
	private float dragThreshold;

	// Token: 0x040005F1 RID: 1521
	private bool entered;

	// Token: 0x040005F2 RID: 1522
	private bool pointerDown;

	// Token: 0x040005F3 RID: 1523
	private bool scrollState;

	// Token: 0x040005F4 RID: 1524
	public static GraphicRaycaster raycaster;

	// Token: 0x040005F5 RID: 1525
	public static List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

	// Token: 0x040005F6 RID: 1526
	private List<RaycastResult> results;

	// Token: 0x040005F7 RID: 1527
	private Vector2? dragPoint;

	// Token: 0x040005F8 RID: 1528
	private bool dragging;
}
