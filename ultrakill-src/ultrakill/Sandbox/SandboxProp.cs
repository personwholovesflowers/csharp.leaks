using System;
using UnityEngine;

namespace Sandbox
{
	// Token: 0x02000564 RID: 1380
	public class SandboxProp : SandboxSpawnableInstance
	{
		// Token: 0x06001F17 RID: 7959 RVA: 0x000FEAA7 File Offset: 0x000FCCA7
		private void Start()
		{
			this.timeSinceLastImpact = 0f;
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000FEABC File Offset: 0x000FCCBC
		private void OnCollisionEnter(Collision other)
		{
			if (this.rigidbody == null)
			{
				return;
			}
			if (this.rigidbody.isKinematic)
			{
				return;
			}
			if (other.impulse.magnitude < 3f)
			{
				return;
			}
			if (this.timeSinceLastImpact < 0.1f)
			{
				return;
			}
			this.timeSinceLastImpact = 0f;
			MonoSingleton<PhysicsSounds>.Instance.ImpactAt(other.GetContact(0).point, other.impulse.magnitude, this.physicsMaterial);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000FEB4C File Offset: 0x000FCD4C
		public SavedProp SaveProp()
		{
			SavedGeneric savedGeneric;
			SavedProp savedProp = (savedGeneric = new SavedProp());
			base.BaseSave(ref savedGeneric);
			return savedProp;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000FEB68 File Offset: 0x000FCD68
		private void OnCollisionStay(Collision other)
		{
			this.OnCollisionEnter(other);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000FEB74 File Offset: 0x000FCD74
		public override void Pause(bool freeze = true)
		{
			base.Pause(freeze);
			Rigidbody rigidbody;
			if (base.TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody.isKinematic = true;
			}
			this.collider.gameObject.isStatic = true;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000FEBAC File Offset: 0x000FCDAC
		public override void Resume()
		{
			base.Resume();
			Rigidbody rigidbody;
			if (base.TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody.isKinematic = false;
			}
			this.collider.gameObject.isStatic = false;
		}

		// Token: 0x04002B81 RID: 11137
		[SerializeField]
		private PhysicsSounds.PhysMaterial physicsMaterial;

		// Token: 0x04002B82 RID: 11138
		[SerializeField]
		private bool enableImpactDamage;

		// Token: 0x04002B83 RID: 11139
		public bool forceFullWorldPreview;

		// Token: 0x04002B84 RID: 11140
		private TimeSince timeSinceLastImpact;
	}
}
