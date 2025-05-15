using System;
using UnityEngine;

namespace Sandbox.Arm
{
	// Token: 0x0200056D RID: 1389
	public class DestroyMode : ISandboxArmMode
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x001002E3 File Offset: 0x000FE4E3
		public string Name
		{
			get
			{
				return "Destroy";
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0002D245 File Offset: 0x0002B445
		public bool CanOpenMenu
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x0002D245 File Offset: 0x0002B445
		public bool Raycast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x001002EA File Offset: 0x000FE4EA
		public virtual string Icon
		{
			get
			{
				return "destroy";
			}
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x001002F1 File Offset: 0x000FE4F1
		public void OnEnable(SandboxArm arm)
		{
			this.hostArm = arm;
			arm.ResetAnimator();
			if (arm.animator.isActiveAndEnabled)
			{
				arm.animator.SetBool(DestroyMode.Point, true);
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnDisable()
		{
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnDestroy()
		{
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Update()
		{
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void FixedUpdate()
		{
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00100320 File Offset: 0x000FE520
		public void OnPrimaryDown()
		{
			if (this.hostArm.hit.collider == null)
			{
				return;
			}
			EnemyIdentifierIdentifier component = this.hostArm.hit.collider.GetComponent<EnemyIdentifierIdentifier>();
			GameObject gameObject;
			if (component && component.eid)
			{
				SandboxEnemy componentInParent = component.eid.GetComponentInParent<SandboxEnemy>();
				gameObject = (componentInParent ? componentInParent.gameObject : component.eid.gameObject);
			}
			else
			{
				SandboxSpawnableInstance prop = SandboxUtils.GetProp(this.hostArm.hit.collider.gameObject, false);
				if (prop == null)
				{
					DietProp dietProp = this.hostArm.hit.collider.GetComponent<DietProp>();
					if (!dietProp)
					{
						return;
					}
					if (dietProp.parent != null)
					{
						dietProp = dietProp.parent;
					}
					gameObject = dietProp.gameObject;
				}
				else
				{
					gameObject = prop.gameObject;
				}
			}
			SandboxSpawnableInstance sandboxSpawnableInstance;
			Rigidbody rigidbody;
			if (this.hostArm.hit.collider.TryGetComponent<SandboxSpawnableInstance>(out sandboxSpawnableInstance) && this.hostArm.hit.collider.TryGetComponent<Rigidbody>(out rigidbody) && rigidbody.isKinematic && MonoSingleton<SandboxNavmesh>.Instance && (!component || !component.eid))
			{
				MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(sandboxSpawnableInstance);
			}
			Object.Instantiate<GameObject>(this.hostArm.genericBreakParticles).transform.position = this.hostArm.hit.collider.bounds.center;
			Object.Destroy(gameObject);
			this.hostArm.destroySound.Play();
			this.hostArm.animator.SetTrigger(DestroyMode.Tap);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnPrimaryUp()
		{
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnSecondaryDown()
		{
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnSecondaryUp()
		{
		}

		// Token: 0x04002BBA RID: 11194
		private SandboxArm hostArm;

		// Token: 0x04002BBB RID: 11195
		private static readonly int Tap = Animator.StringToHash("Tap");

		// Token: 0x04002BBC RID: 11196
		private static readonly int Point = Animator.StringToHash("Point");
	}
}
