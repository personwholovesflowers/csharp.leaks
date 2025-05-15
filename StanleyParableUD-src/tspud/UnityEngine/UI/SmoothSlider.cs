using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x020001F3 RID: 499
	[AddComponentMenu("UI/Smooth Slider", 33)]
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class SmoothSlider : Selectable, IDragHandler, IEventSystemHandler, IInitializePotentialDragHandler, ICanvasElement, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x06000B71 RID: 2929 RVA: 0x00034989 File Offset: 0x00032B89
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000349AC File Offset: 0x00032BAC
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x000349F9 File Offset: 0x00032BF9
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x00034A01 File Offset: 0x00032C01
		public RectTransform fillRect
		{
			get
			{
				return this.m_FillRect;
			}
			set
			{
				if (SmoothSlider.SetClass<RectTransform>(ref this.m_FillRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x00034A1D File Offset: 0x00032C1D
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x00034A25 File Offset: 0x00032C25
		public RectTransform handleRect
		{
			get
			{
				return this.m_HandleRect;
			}
			set
			{
				if (SmoothSlider.SetClass<RectTransform>(ref this.m_HandleRect, value))
				{
					this.UpdateCachedReferences();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00034A41 File Offset: 0x00032C41
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x00034A49 File Offset: 0x00032C49
		public SmoothSlider.Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				if (SmoothSlider.SetStruct<SmoothSlider.Direction>(ref this.m_Direction, value))
				{
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00034A5F File Offset: 0x00032C5F
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x00034A67 File Offset: 0x00032C67
		public float minValue
		{
			get
			{
				return this.m_MinValue;
			}
			set
			{
				if (SmoothSlider.SetStruct<float>(ref this.m_MinValue, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00034A8A File Offset: 0x00032C8A
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x00034A92 File Offset: 0x00032C92
		public float maxValue
		{
			get
			{
				return this.m_MaxValue;
			}
			set
			{
				if (SmoothSlider.SetStruct<float>(ref this.m_MaxValue, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00034AB5 File Offset: 0x00032CB5
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00034ABD File Offset: 0x00032CBD
		public bool wholeNumbers
		{
			get
			{
				return this.m_WholeNumbers;
			}
			set
			{
				if (SmoothSlider.SetStruct<bool>(ref this.m_WholeNumbers, value))
				{
					this.Set(this.m_Value, true);
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00034AE0 File Offset: 0x00032CE0
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00034AFC File Offset: 0x00032CFC
		public virtual float value
		{
			get
			{
				if (this.wholeNumbers)
				{
					return Mathf.Round(this.m_Value);
				}
				return this.m_Value;
			}
			set
			{
				this.Set(value, true);
			}
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00034B06 File Offset: 0x00032D06
		public virtual void SetValueWithoutNotify(float input)
		{
			this.Set(input, false);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00034B10 File Offset: 0x00032D10
		public virtual void SetValueWithoutNotify(int input)
		{
			this.Set((float)input, false);
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00034B1B File Offset: 0x00032D1B
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00034B4D File Offset: 0x00032D4D
		public float normalizedValue
		{
			get
			{
				if (Mathf.Approximately(this.minValue, this.maxValue))
				{
					return 0f;
				}
				return Mathf.InverseLerp(this.minValue, this.maxValue, this.value);
			}
			set
			{
				this.value = Mathf.Lerp(this.minValue, this.maxValue, value);
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x00034B67 File Offset: 0x00032D67
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00034B6F File Offset: 0x00032D6F
		public SmoothSlider.SliderEvent onValueChanged
		{
			get
			{
				return this.m_OnValueChanged;
			}
			set
			{
				this.m_OnValueChanged = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00034B78 File Offset: 0x00032D78
		private float m_StepSize
		{
			get
			{
				return (this.wholeNumbers ? 1f : ((this.maxValue - this.minValue) * this.m_CustomStepSize)) * this.m_StepSizeMultiplier;
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00034BA4 File Offset: 0x00032DA4
		protected SmoothSlider()
		{
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void Rebuild(CanvasUpdate executing)
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void LayoutComplete()
		{
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00005444 File Offset: 0x00003644
		public virtual void GraphicUpdateComplete()
		{
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00034BFE File Offset: 0x00032DFE
		protected override void OnEnable()
		{
			base.OnEnable();
			this.UpdateCachedReferences();
			this.Set(this.m_Value, false);
			this.UpdateVisuals();
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00034C1F File Offset: 0x00032E1F
		protected override void OnDisable()
		{
			this.m_Tracker.Clear();
			base.OnDisable();
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00034C32 File Offset: 0x00032E32
		protected virtual void Update()
		{
			if (this.m_DelayedUpdateVisuals)
			{
				this.m_DelayedUpdateVisuals = false;
				this.Set(this.m_Value, false);
				this.UpdateVisuals();
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00034C58 File Offset: 0x00032E58
		protected override void OnDidApplyAnimationProperties()
		{
			this.m_Value = this.ClampValue(this.m_Value);
			float num = this.normalizedValue;
			if (this.m_FillContainerRect != null)
			{
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					num = this.m_FillImage.fillAmount;
				}
				else
				{
					num = (this.reverseValue ? (1f - this.m_FillRect.anchorMin[(int)this.axis]) : this.m_FillRect.anchorMax[(int)this.axis]);
				}
			}
			else if (this.m_HandleContainerRect != null)
			{
				num = (this.reverseValue ? (1f - this.m_HandleRect.anchorMin[(int)this.axis]) : this.m_HandleRect.anchorMin[(int)this.axis]);
			}
			this.UpdateVisuals();
			if (num != this.normalizedValue)
			{
				UISystemProfilerApi.AddMarker("Slider.value", this);
				this.onValueChanged.Invoke(this.m_Value);
			}
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00034D7C File Offset: 0x00032F7C
		private void UpdateCachedReferences()
		{
			if (this.m_FillRect && this.m_FillRect != (RectTransform)base.transform)
			{
				this.m_FillTransform = this.m_FillRect.transform;
				this.m_FillImage = this.m_FillRect.GetComponent<Image>();
				if (this.m_FillTransform.parent != null)
				{
					this.m_FillContainerRect = this.m_FillTransform.parent.GetComponent<RectTransform>();
				}
			}
			else
			{
				this.m_FillRect = null;
				this.m_FillContainerRect = null;
				this.m_FillImage = null;
			}
			if (this.m_HandleRect && this.m_HandleRect != (RectTransform)base.transform)
			{
				this.m_HandleTransform = this.m_HandleRect.transform;
				if (this.m_HandleTransform.parent != null)
				{
					this.m_HandleContainerRect = this.m_HandleTransform.parent.GetComponent<RectTransform>();
					return;
				}
			}
			else
			{
				this.m_HandleRect = null;
				this.m_HandleContainerRect = null;
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00034E80 File Offset: 0x00033080
		private float ClampValue(float input)
		{
			float num = Mathf.Clamp(input, this.minValue, this.maxValue);
			if (this.wholeNumbers)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00034EB0 File Offset: 0x000330B0
		protected virtual void Set(float input, bool sendCallback = true)
		{
			float num = this.ClampValue(input);
			if (this.m_Value == num)
			{
				return;
			}
			this.m_Value = num;
			this.UpdateVisuals();
			if (sendCallback)
			{
				UISystemProfilerApi.AddMarker("Slider.value", this);
				this.m_OnValueChanged.Invoke(num);
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00034EF6 File Offset: 0x000330F6
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (!this.IsActive())
			{
				return;
			}
			this.UpdateVisuals();
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00034F0D File Offset: 0x0003310D
		private SmoothSlider.Axis axis
		{
			get
			{
				if (this.m_Direction != SmoothSlider.Direction.LeftToRight && this.m_Direction != SmoothSlider.Direction.RightToLeft)
				{
					return SmoothSlider.Axis.Vertical;
				}
				return SmoothSlider.Axis.Horizontal;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000B95 RID: 2965 RVA: 0x00034F23 File Offset: 0x00033123
		private bool reverseValue
		{
			get
			{
				return this.m_Direction == SmoothSlider.Direction.RightToLeft || this.m_Direction == SmoothSlider.Direction.TopToBottom;
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00034F3C File Offset: 0x0003313C
		private void UpdateVisuals()
		{
			this.m_Tracker.Clear();
			if (this.m_FillContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_FillRect, DrivenTransformProperties.Anchors);
				Vector2 zero = Vector2.zero;
				Vector2 one = Vector2.one;
				if (this.m_FillImage != null && this.m_FillImage.type == Image.Type.Filled)
				{
					this.m_FillImage.fillAmount = this.normalizedValue;
				}
				else if (this.reverseValue)
				{
					zero[(int)this.axis] = 1f - this.normalizedValue;
				}
				else
				{
					one[(int)this.axis] = this.normalizedValue;
				}
				this.m_FillRect.anchorMin = zero;
				this.m_FillRect.anchorMax = one;
			}
			if (this.m_HandleContainerRect != null)
			{
				this.m_Tracker.Add(this, this.m_HandleRect, DrivenTransformProperties.Anchors);
				Vector2 zero2 = Vector2.zero;
				Vector2 one2 = Vector2.one;
				zero2[(int)this.axis] = (one2[(int)this.axis] = (this.reverseValue ? (1f - this.normalizedValue) : this.normalizedValue));
				this.m_HandleRect.anchorMin = zero2;
				this.m_HandleRect.anchorMax = one2;
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0003508C File Offset: 0x0003328C
		private void UpdateDrag(PointerEventData eventData, Camera cam)
		{
			RectTransform rectTransform = this.m_HandleContainerRect ?? this.m_FillContainerRect;
			if (rectTransform != null && rectTransform.rect.size[(int)this.axis] > 0f)
			{
				Vector2 vector;
				if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, cam, out vector))
				{
					return;
				}
				vector -= rectTransform.rect.position;
				float num = Mathf.Clamp01((vector - this.m_Offset)[(int)this.axis] / rectTransform.rect.size[(int)this.axis]);
				this.normalizedValue = (this.reverseValue ? (1f - num) : num);
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00035158 File Offset: 0x00033358
		private bool MayDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00035178 File Offset: 0x00033378
		public override void OnPointerDown(PointerEventData eventData)
		{
			SimpleEvent simpleEvent = this.onPointerDownEvent;
			if (simpleEvent != null)
			{
				simpleEvent.Call();
			}
			if (!this.MayDrag(eventData))
			{
				return;
			}
			base.OnPointerDown(eventData);
			this.m_Offset = Vector2.zero;
			if (this.m_HandleContainerRect != null && RectTransformUtility.RectangleContainsScreenPoint(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.enterEventCamera))
			{
				Vector2 vector;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandleRect, eventData.pointerPressRaycast.screenPosition, eventData.pressEventCamera, out vector))
				{
					this.m_Offset = vector;
					return;
				}
			}
			else
			{
				this.UpdateDrag(eventData, eventData.pressEventCamera);
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00035213 File Offset: 0x00033413
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.MayDrag(eventData))
			{
				return;
			}
			this.UpdateDrag(eventData, eventData.pressEventCamera);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0003522C File Offset: 0x0003342C
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			SimpleEvent simpleEvent = this.onPointerUpEvent;
			if (simpleEvent == null)
			{
				return;
			}
			simpleEvent.Call();
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x00035248 File Offset: 0x00033448
		public override void OnMove(AxisEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			if (this.m_SliderDurationCheck == null)
			{
				this.m_SliderDurationCheck = base.StartCoroutine(this.SliderDurationCheck());
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.axis == SmoothSlider.Axis.Horizontal && this.FindSelectableOnLeft() == null)
				{
					this.Set(this.reverseValue ? (this.value + this.m_StepSize) : (this.value - this.m_StepSize), true);
					this.m_NumSteps++;
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Up:
				if (this.axis == SmoothSlider.Axis.Vertical && this.FindSelectableOnUp() == null)
				{
					this.Set(this.reverseValue ? (this.value - this.m_StepSize) : (this.value + this.m_StepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Right:
				if (this.axis == SmoothSlider.Axis.Horizontal && this.FindSelectableOnRight() == null)
				{
					this.Set(this.reverseValue ? (this.value - this.m_StepSize) : (this.value + this.m_StepSize), true);
					this.m_NumSteps++;
					return;
				}
				base.OnMove(eventData);
				return;
			case MoveDirection.Down:
				if (this.axis == SmoothSlider.Axis.Vertical && this.FindSelectableOnDown() == null)
				{
					this.Set(this.reverseValue ? (this.value + this.m_StepSize) : (this.value - this.m_StepSize), true);
					return;
				}
				base.OnMove(eventData);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x000353EC File Offset: 0x000335EC
		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == SmoothSlider.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0003541C File Offset: 0x0003361C
		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == SmoothSlider.Axis.Horizontal)
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0003544C File Offset: 0x0003364C
		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == SmoothSlider.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0003547C File Offset: 0x0003367C
		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.axis == SmoothSlider.Axis.Vertical)
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000354AB File Offset: 0x000336AB
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x000354B4 File Offset: 0x000336B4
		public void SetDirection(SmoothSlider.Direction direction, bool includeRectLayouts)
		{
			SmoothSlider.Axis axis = this.axis;
			bool reverseValue = this.reverseValue;
			this.direction = direction;
			if (!includeRectLayouts)
			{
				return;
			}
			if (this.axis != axis)
			{
				RectTransformUtility.FlipLayoutAxes(base.transform as RectTransform, true, true);
			}
			if (this.reverseValue != reverseValue)
			{
				RectTransformUtility.FlipLayoutOnAxis(base.transform as RectTransform, (int)this.axis, true, true);
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00035516 File Offset: 0x00033716
		private IEnumerator SliderDurationCheck()
		{
			while (Singleton<GameMaster>.Instance.stanleyActions.Left.IsPressed || Singleton<GameMaster>.Instance.stanleyActions.Right.IsPressed || Singleton<GameMaster>.Instance.stanleyActions.MoveLeft.IsPressed || Singleton<GameMaster>.Instance.stanleyActions.MoveRight.IsPressed)
			{
				if (this.m_NumSteps >= this.numStepsThreshold)
				{
					this.m_StepSizeMultiplier = (float)this.largeStepSize;
				}
				yield return null;
			}
			this.m_NumSteps = 0;
			this.m_StepSizeMultiplier = 1f;
			this.m_SliderDurationCheck = null;
			yield break;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00035525 File Offset: 0x00033725
		Transform ICanvasElement.get_transform()
		{
			return base.transform;
		}

		// Token: 0x04000B27 RID: 2855
		[SerializeField]
		private RectTransform m_FillRect;

		// Token: 0x04000B28 RID: 2856
		[SerializeField]
		private RectTransform m_HandleRect;

		// Token: 0x04000B29 RID: 2857
		[Space]
		[SerializeField]
		private SmoothSlider.Direction m_Direction;

		// Token: 0x04000B2A RID: 2858
		[SerializeField]
		private float m_MinValue;

		// Token: 0x04000B2B RID: 2859
		[SerializeField]
		private float m_MaxValue = 1f;

		// Token: 0x04000B2C RID: 2860
		[SerializeField]
		private bool m_WholeNumbers;

		// Token: 0x04000B2D RID: 2861
		[SerializeField]
		protected float m_Value;

		// Token: 0x04000B2E RID: 2862
		[Space]
		[SerializeField]
		private SmoothSlider.SliderEvent m_OnValueChanged = new SmoothSlider.SliderEvent();

		// Token: 0x04000B2F RID: 2863
		private Image m_FillImage;

		// Token: 0x04000B30 RID: 2864
		private Transform m_FillTransform;

		// Token: 0x04000B31 RID: 2865
		private RectTransform m_FillContainerRect;

		// Token: 0x04000B32 RID: 2866
		private Transform m_HandleTransform;

		// Token: 0x04000B33 RID: 2867
		private RectTransform m_HandleContainerRect;

		// Token: 0x04000B34 RID: 2868
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04000B35 RID: 2869
		private DrivenRectTransformTracker m_Tracker;

		// Token: 0x04000B36 RID: 2870
		private bool m_DelayedUpdateVisuals;

		// Token: 0x04000B37 RID: 2871
		public float m_CustomStepSize = 0.05f;

		// Token: 0x04000B38 RID: 2872
		[Tooltip("The value that stepSize will be multiplied by when the player has changed the slider for a bit.")]
		public int largeStepSize = 10;

		// Token: 0x04000B39 RID: 2873
		[Tooltip("How many slider value steps does the player have to change before largeStepSize is used?")]
		public int numStepsThreshold = 15;

		// Token: 0x04000B3A RID: 2874
		private float m_StepSizeMultiplier = 1f;

		// Token: 0x04000B3B RID: 2875
		private int m_NumSteps;

		// Token: 0x04000B3C RID: 2876
		private Coroutine m_SliderDurationCheck;

		// Token: 0x04000B3D RID: 2877
		[SerializeField]
		private SimpleEvent onPointerDownEvent;

		// Token: 0x04000B3E RID: 2878
		[SerializeField]
		private SimpleEvent onPointerUpEvent;

		// Token: 0x02000415 RID: 1045
		public enum Direction
		{
			// Token: 0x04001548 RID: 5448
			LeftToRight,
			// Token: 0x04001549 RID: 5449
			RightToLeft,
			// Token: 0x0400154A RID: 5450
			BottomToTop,
			// Token: 0x0400154B RID: 5451
			TopToBottom
		}

		// Token: 0x02000416 RID: 1046
		[Serializable]
		public class SliderEvent : UnityEvent<float>
		{
		}

		// Token: 0x02000417 RID: 1047
		private enum Axis
		{
			// Token: 0x0400154D RID: 5453
			Horizontal,
			// Token: 0x0400154E RID: 5454
			Vertical
		}
	}
}
