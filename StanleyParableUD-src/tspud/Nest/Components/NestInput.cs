using System;
using System.Collections;
using Nest.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Nest.Components
{
	// Token: 0x02000249 RID: 585
	[AddComponentMenu("Nest/Components/Nest Input")]
	public class NestInput : MonoBehaviour
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000DCB RID: 3531 RVA: 0x0003DEC4 File Offset: 0x0003C0C4
		// (remove) Token: 0x06000DCC RID: 3532 RVA: 0x0003DEFC File Offset: 0x0003C0FC
		public event CastEvent EventFired;

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0003DF31 File Offset: 0x0003C131
		public bool HasFired
		{
			get
			{
				return this._fired;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x0003DF39 File Offset: 0x0003C139
		// (set) Token: 0x06000DCF RID: 3535 RVA: 0x0003DF41 File Offset: 0x0003C141
		public NestInput.EventType CurrentEventType
		{
			get
			{
				return this._eventType;
			}
			set
			{
				this._eventType = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x0003DF4C File Offset: 0x0003C14C
		public UnityEventBase CurrentEvent
		{
			get
			{
				NestInput.EventType eventType = this._eventType;
				switch (eventType)
				{
				case NestInput.EventType.Trigger:
					return this._event;
				case NestInput.EventType.Bool:
					break;
				case NestInput.EventType.Trigger | NestInput.EventType.Bool:
					goto IL_0072;
				case NestInput.EventType.Float:
					return this._eventValue;
				default:
					if (eventType != NestInput.EventType.Toggle)
					{
						if (eventType != NestInput.EventType.String)
						{
							goto IL_0072;
						}
						return this._eventString;
					}
					break;
				}
				if (this.Value.CurrentValue < 0.01f)
				{
					return this._event;
				}
				if (this.Value.CurrentValue > 0.99f)
				{
					return this._event2;
				}
				IL_0072:
				return this._event;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0003DFD1 File Offset: 0x0003C1D1
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0003DFEA File Offset: 0x0003C1EA
		public bool EventPosition
		{
			get
			{
				return Mathf.Abs(this.Value.CurrentValue) < 0.01f;
			}
			set
			{
				this.Value.CurrentValue = (float)(value ? 1 : 0);
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0003DFFF File Offset: 0x0003C1FF
		public virtual void Start()
		{
			this.Value = new FloatInterpolator(this._eventOffValue, this._eventOnValue, this._interpolation);
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003E020 File Offset: 0x0003C220
		public void Invoke()
		{
			if ((this.FireOnce && this._fired) || !base.enabled)
			{
				return;
			}
			this._fired = true;
			if (this.Delay > 0f)
			{
				base.StartCoroutine(this.InvokeDelay());
				return;
			}
			this.InvokeEvent();
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0003E06E File Offset: 0x0003C26E
		private IEnumerator InvokeDelay()
		{
			yield return new WaitForGameSeconds(this.Delay);
			this.InvokeEvent();
			yield break;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0003E080 File Offset: 0x0003C280
		public void InvokeEvent()
		{
			NestInput.EventType eventType = this._eventType;
			switch (eventType)
			{
			case NestInput.EventType.Trigger:
				this._event.Invoke();
				break;
			case NestInput.EventType.Bool:
				if (Mathf.Abs(this.Value.CurrentValue) < 0.01f)
				{
					this._event2.Invoke();
				}
				else if (Mathf.Abs(this.Value.CurrentValue) - 1f < 0.01f)
				{
					this._event.Invoke();
				}
				break;
			case NestInput.EventType.Trigger | NestInput.EventType.Bool:
				break;
			case NestInput.EventType.Float:
				this._eventValue.Invoke(this._parameterFloat);
				break;
			default:
				if (eventType != NestInput.EventType.Toggle)
				{
					if (eventType == NestInput.EventType.String)
					{
						this._eventString.Invoke(this._parameterString);
					}
				}
				else
				{
					this.Value.CurrentValue = 1f - this.Value.CurrentValue;
					if (Mathf.Abs(this.Value.CurrentValue) < 0.01f)
					{
						this._event2.Invoke();
					}
					else
					{
						this._event.Invoke();
					}
				}
				break;
			}
			if (this.EventFired != null)
			{
				this.EventFired(this);
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0003E1A8 File Offset: 0x0003C3A8
		public int[] FindGameObjectsReferences()
		{
			float currentValue = this.Value.CurrentValue;
			this.Value.CurrentValue = 0f;
			int num = this.CurrentEvent.GetPersistentEventCount();
			if (this._eventType == NestInput.EventType.Bool || this._eventType == NestInput.EventType.Toggle)
			{
				num += this._event2.GetPersistentEventCount();
			}
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				Object @object = null;
				if (i > this.CurrentEvent.GetPersistentEventCount())
				{
					@object = this._event2.GetPersistentTarget(i - this.CurrentEvent.GetPersistentEventCount());
				}
				else if (this.CurrentEvent.GetPersistentEventCount() > i)
				{
					@object = this.CurrentEvent.GetPersistentTarget(i);
				}
				else if (this._event2.GetPersistentEventCount() > i)
				{
					@object = this._event2.GetPersistentTarget(i);
				}
				if (@object is GameObject)
				{
					array[i] = @object.GetInstanceID();
				}
				else if (@object is Component)
				{
					array[i] = ((Component)@object).gameObject.GetInstanceID();
				}
			}
			this.Value.CurrentValue = currentValue;
			return array;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0003E2BF File Offset: 0x0003C4BF
		public void SetBool(bool value)
		{
			this.Value.TargetValue = (value ? this._eventOnValue : this._eventOffValue);
			this.Value.CurrentValue = this.Value.TargetValue;
		}

		// Token: 0x04000C5A RID: 3162
		[SerializeField]
		private NestInput.EventType _eventType = NestInput.EventType.Trigger;

		// Token: 0x04000C5B RID: 3163
		[SerializeField]
		private float _eventOffValue;

		// Token: 0x04000C5C RID: 3164
		[SerializeField]
		private float _eventOnValue = 1f;

		// Token: 0x04000C5D RID: 3165
		[SerializeField]
		private SerializableDictionaryBase<string, UnityEventBase> _events;

		// Token: 0x04000C5E RID: 3166
		[SerializeField]
		public UnityEvent _event;

		// Token: 0x04000C5F RID: 3167
		[SerializeField]
		private UnityEvent _event2;

		// Token: 0x04000C60 RID: 3168
		[SerializeField]
		public NestInput.ValueEvent _eventValue;

		// Token: 0x04000C61 RID: 3169
		[SerializeField]
		public float _parameterFloat;

		// Token: 0x04000C62 RID: 3170
		[SerializeField]
		public NestInput.StringEvent _eventString;

		// Token: 0x04000C63 RID: 3171
		[SerializeField]
		public string _parameterString;

		// Token: 0x04000C64 RID: 3172
		public bool FireOnce;

		// Token: 0x04000C65 RID: 3173
		[SerializeField]
		private FloatInterpolator.Config _interpolation;

		// Token: 0x04000C66 RID: 3174
		private bool _fired;

		// Token: 0x04000C68 RID: 3176
		public float Delay;

		// Token: 0x04000C69 RID: 3177
		protected FloatInterpolator Value;

		// Token: 0x0200043C RID: 1084
		[Serializable]
		public class ValueEvent : UnityEvent<float>
		{
		}

		// Token: 0x0200043D RID: 1085
		[Serializable]
		public class StringEvent : UnityEvent<string>
		{
		}

		// Token: 0x0200043E RID: 1086
		[Flags]
		public enum EventType
		{
			// Token: 0x040015CE RID: 5582
			Trigger = 1,
			// Token: 0x040015CF RID: 5583
			Bool = 2,
			// Token: 0x040015D0 RID: 5584
			Float = 4,
			// Token: 0x040015D1 RID: 5585
			Toggle = 16,
			// Token: 0x040015D2 RID: 5586
			String = 32
		}
	}
}
