using System;
using System.Linq;
using UnityEngine;

namespace Nest.Components
{
	// Token: 0x02000247 RID: 583
	[AddComponentMenu("Nest/Components/Collider Responder")]
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	public class ColliderResponder : NestInput
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x0003DDF0 File Offset: 0x0003BFF0
		private void OnCollisionEnter(Collision collision)
		{
			if (!this._tagValues.Contains(collision.gameObject.tag))
			{
				return;
			}
			if (collision.rigidbody != null)
			{
				this._force = collision.rigidbody.velocity.magnitude;
			}
			this.Invoke(ColliderResponder.CollisionType.Enter);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0003DE44 File Offset: 0x0003C044
		private void OnCollisionExit(Collision collision)
		{
			if (!this._tagValues.Contains(collision.gameObject.tag))
			{
				return;
			}
			this.Invoke(ColliderResponder.CollisionType.Exit);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0003DE66 File Offset: 0x0003C066
		private void OnCollisionStay(Collision collision)
		{
			if (!this._tagValues.Contains(collision.gameObject.tag))
			{
				return;
			}
			this.Invoke(ColliderResponder.CollisionType.Stay);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0003DE88 File Offset: 0x0003C088
		public void Invoke(ColliderResponder.CollisionType type)
		{
			if ((this.CollisionEvent & type) == (ColliderResponder.CollisionType)0)
			{
				return;
			}
			this.Value.CurrentValue = this._force;
			base.Invoke();
		}

		// Token: 0x04000C56 RID: 3158
		public ColliderResponder.CollisionType CollisionEvent = ColliderResponder.CollisionType.Enter;

		// Token: 0x04000C57 RID: 3159
		private float _force;

		// Token: 0x04000C58 RID: 3160
		[SerializeField]
		public int TagMask = -1;

		// Token: 0x04000C59 RID: 3161
		[SerializeField]
		private string[] _tagValues;

		// Token: 0x0200043B RID: 1083
		[Flags]
		public enum CollisionType
		{
			// Token: 0x040015C9 RID: 5577
			Enter = 1,
			// Token: 0x040015CA RID: 5578
			Stay = 2,
			// Token: 0x040015CB RID: 5579
			Exit = 4,
			// Token: 0x040015CC RID: 5580
			EnterAndExit = 5
		}
	}
}
