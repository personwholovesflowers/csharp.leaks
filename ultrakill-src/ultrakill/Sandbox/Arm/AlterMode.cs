using System;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Sandbox.Arm
{
	// Token: 0x02000569 RID: 1385
	public class AlterMode : ISandboxArmMode
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x000FF71B File Offset: 0x000FD91B
		public string Name
		{
			get
			{
				return "Alter";
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x000FF722 File Offset: 0x000FD922
		public bool CanOpenMenu
		{
			get
			{
				return this.selected == null;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x0002D245 File Offset: 0x0002B445
		public bool Raycast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x000FF730 File Offset: 0x000FD930
		public virtual string Icon
		{
			get
			{
				return "alter";
			}
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000FF738 File Offset: 0x000FD938
		public void EndSession()
		{
			if (this.selected == null)
			{
				return;
			}
			if (!this.selected.frozen)
			{
				this.selected.Resume();
			}
			if (this.selected.attachedParticles)
			{
				Object.Destroy(this.selected.attachedParticles.gameObject);
			}
			this.selected = null;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000FF79A File Offset: 0x000FD99A
		public void OnEnable(SandboxArm arm)
		{
			arm.ResetAnimator();
			if (arm.animator.isActiveAndEnabled)
			{
				arm.animator.SetBool(AlterMode.Point, true);
			}
			this.hostArm = arm;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000FF7C7 File Offset: 0x000FD9C7
		public void OnDisable()
		{
			if (this.selected && MonoSingleton<SandboxAlterMenu>.Instance)
			{
				MonoSingleton<SandboxAlterMenu>.Instance.Close();
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000FF7C7 File Offset: 0x000FD9C7
		public void OnDestroy()
		{
			if (this.selected && MonoSingleton<SandboxAlterMenu>.Instance)
			{
				MonoSingleton<SandboxAlterMenu>.Instance.Close();
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Update()
		{
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void FixedUpdate()
		{
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000FF7EC File Offset: 0x000FD9EC
		public void OnPrimaryDown()
		{
			if (!this.hostArm.hitSomething)
			{
				return;
			}
			SandboxSpawnableInstance sandboxSpawnableInstance;
			if (!this.hostArm.hit.collider.TryGetComponent<SandboxSpawnableInstance>(out sandboxSpawnableInstance))
			{
				Transform transform = this.hostArm.hit.collider.transform;
				if (transform.parent == null || !transform.parent.TryGetComponent<SandboxSpawnableInstance>(out sandboxSpawnableInstance))
				{
					return;
				}
			}
			if (this.selected)
			{
				return;
			}
			this.OpenProp(sandboxSpawnableInstance);
			this.hostArm.animator.SetTrigger(AlterMode.Tap);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000FF880 File Offset: 0x000FDA80
		public void OpenProp(SandboxSpawnableInstance prop)
		{
			if (SandboxArmDebug.DebugActive)
			{
				AlterMode.Log.Info(string.Format("Opening prop {0}", prop), null, null, null);
			}
			this.selected = prop;
			MonoSingleton<SandboxAlterMenu>.Instance.editedObject = prop;
			MonoSingleton<SandboxAlterMenu>.Instance.Show(prop, this);
			prop.attachedParticles = Object.Instantiate<GameObject>(this.hostArm.manipulateEffect, prop.transform, true);
			prop.attachedParticles.transform.position = prop.collider.bounds.center;
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnPrimaryUp()
		{
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000FF90A File Offset: 0x000FDB0A
		public void OnSecondaryDown()
		{
			if (this.selected == null)
			{
				return;
			}
			MonoSingleton<SandboxAlterMenu>.Instance.Close();
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnSecondaryUp()
		{
		}

		// Token: 0x04002BA3 RID: 11171
		private static readonly global::plog.Logger Log = new global::plog.Logger("AlterMode");

		// Token: 0x04002BA4 RID: 11172
		private SandboxArm hostArm;

		// Token: 0x04002BA5 RID: 11173
		private static readonly int Tap = Animator.StringToHash("Tap");

		// Token: 0x04002BA6 RID: 11174
		private static readonly int Point = Animator.StringToHash("Point");

		// Token: 0x04002BA7 RID: 11175
		private SandboxSpawnableInstance selected;
	}
}
