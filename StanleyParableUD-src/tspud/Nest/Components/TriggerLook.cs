using System;
using UnityEngine;

namespace Nest.Components
{
	// Token: 0x0200024A RID: 586
	[AddComponentMenu("Nest/Input/Trigger Look")]
	public class TriggerLook : NestInput
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0003E30D File Offset: 0x0003C50D
		public static RaycastHit[] Results
		{
			get
			{
				return TriggerLook.results;
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0003E314 File Offset: 0x0003C514
		public override void Start()
		{
			base.Start();
			if (TriggerLook.results == null)
			{
				TriggerLook.results = new RaycastHit[5];
			}
			if (this.Tracking == null)
			{
				this.Tracking = base.transform;
			}
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0003E348 File Offset: 0x0003C548
		public void FixedUpdate()
		{
			if (this.RestrictToTrigger && !this._withinTrigger)
			{
				return;
			}
			int num;
			if ((this.CameraEvent & (TriggerLook.CameraType.WithinCrosshair | TriggerLook.CameraType.OutsideCrosshair)) > (TriggerLook.CameraType)0 && (num = Physics.RaycastNonAlloc(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), TriggerLook.results)) > 0)
			{
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					RaycastHit raycastHit = TriggerLook.Results[i];
					if (!(raycastHit.transform == null))
					{
						flag |= raycastHit.transform == this.Tracking;
					}
				}
				if (this.Invoke(flag ? TriggerLook.CameraType.WithinCrosshair : TriggerLook.CameraType.OutsideCrosshair))
				{
					return;
				}
			}
			Vector3 vector = Camera.main.WorldToViewportPoint(base.transform.position);
			if (vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f && vector.y < 1f)
			{
				if (this._timeWithinView <= 0f)
				{
					this.Invoke(TriggerLook.CameraType.Enter);
				}
				this._timeWithinView += Time.deltaTime;
				this.Invoke(TriggerLook.CameraType.WithinView);
				return;
			}
			if (this._timeWithinView > 0f)
			{
				this.Invoke(TriggerLook.CameraType.Exit);
				this._timeWithinView = 0f;
				return;
			}
			this.Invoke(TriggerLook.CameraType.OutOfView);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0003E4A2 File Offset: 0x0003C6A2
		public void OnTriggerEnter(Collider other)
		{
			this._withinTrigger = other.CompareTag("Player");
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0003E4B5 File Offset: 0x0003C6B5
		public void OnTriggerExit(Collider other)
		{
			this._withinTrigger = !other.CompareTag("Player");
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0003E4CC File Offset: 0x0003C6CC
		public bool Invoke(TriggerLook.CameraType type)
		{
			if ((this.CameraEvent & type) == (TriggerLook.CameraType)0)
			{
				return false;
			}
			if (this._previousState == type && base.CurrentEventType != NestInput.EventType.Float)
			{
				return true;
			}
			this._previousState = type;
			if ((this._eventComposition.First & type) != (TriggerLook.CameraType)0)
			{
				base.SetBool(false);
			}
			else if ((this._eventComposition.Second & type) != (TriggerLook.CameraType)0)
			{
				base.SetBool(true);
			}
			if (this.TrackTimeWithinView)
			{
				this.Value.CurrentValue = this._timeWithinView;
			}
			base.Invoke();
			return true;
		}

		// Token: 0x04000C6A RID: 3178
		public TriggerLook.CameraType CameraEvent = TriggerLook.CameraType.WithinView;

		// Token: 0x04000C6B RID: 3179
		[NestOption]
		public bool RestrictToTrigger = true;

		// Token: 0x04000C6C RID: 3180
		[NestOption]
		public bool TrackTimeWithinView;

		// Token: 0x04000C6D RID: 3181
		[NestOption]
		public Transform Tracking;

		// Token: 0x04000C6E RID: 3182
		private float _timeWithinView;

		// Token: 0x04000C6F RID: 3183
		private bool _withinTrigger;

		// Token: 0x04000C70 RID: 3184
		private TriggerLook.CameraType _previousState;

		// Token: 0x04000C71 RID: 3185
		private static RaycastHit[] results;

		// Token: 0x04000C72 RID: 3186
		[SerializeField]
		private TriggerLook.TypeComposition _eventComposition;

		// Token: 0x02000440 RID: 1088
		[Flags]
		public enum CameraType
		{
			// Token: 0x040015D7 RID: 5591
			Enter = 1,
			// Token: 0x040015D8 RID: 5592
			WithinView = 2,
			// Token: 0x040015D9 RID: 5593
			OutOfView = 4,
			// Token: 0x040015DA RID: 5594
			WithinCrosshair = 8,
			// Token: 0x040015DB RID: 5595
			OutsideCrosshair = 16,
			// Token: 0x040015DC RID: 5596
			Exit = 32
		}

		// Token: 0x02000441 RID: 1089
		[Serializable]
		public struct TypeComposition
		{
			// Token: 0x040015DD RID: 5597
			public TriggerLook.CameraType First;

			// Token: 0x040015DE RID: 5598
			public TriggerLook.CameraType Second;
		}
	}
}
