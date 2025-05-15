using System;
using System.Collections;
using UnityEngine;

namespace Sandbox.Arm
{
	// Token: 0x0200056A RID: 1386
	public abstract class ArmModeWithHeldPreview : ISandboxArmMode
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000FF954 File Offset: 0x000FDB54
		public virtual string Name
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0002D245 File Offset: 0x0002B445
		public virtual bool CanOpenMenu
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0002D245 File Offset: 0x0002B445
		public virtual bool Raycast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000FF954 File Offset: 0x000FDB54
		public virtual string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000FF958 File Offset: 0x000FDB58
		public virtual void OnEnable(SandboxArm arm)
		{
			this.hostArm = arm;
			this.hostArm.ResetAnimator();
			if (arm.animator.isActiveAndEnabled)
			{
				this.hostArm.animator.SetBool(ArmModeWithHeldPreview.Holding, true);
			}
			if (this.heldPreview)
			{
				this.heldPreview.SetActive(true);
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000FF9B3 File Offset: 0x000FDBB3
		protected IEnumerator HandClosedAnimationThing()
		{
			this.heldPreview.SetActive(false);
			yield return new WaitForSeconds(0.85f);
			this.heldPreview.SetActive(true);
			yield break;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x000FF9C4 File Offset: 0x000FDBC4
		public virtual void SetPreview(SpawnableObject obj)
		{
			this.hostArm.selectSound.Play();
			this.currentObject = obj;
			if (this.heldPreview)
			{
				Object.Destroy(this.heldPreview);
			}
			if (obj.preview)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(obj.preview, this.hostArm.holder, false);
				gameObject.transform.localPosition += obj.armOffset;
				gameObject.transform.Rotate(obj.armRotationOffset);
				this.heldPreview = gameObject;
				return;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(obj.gameObject, this.hostArm.holder, false);
			SandboxUtils.StripForPreview(gameObject2.transform, null, true);
			SandboxUtils.SetLayerDeep(gameObject2.transform, this.hostArm.holder.gameObject.layer);
			gameObject2.SetActive(true);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localScale *= 0.25f;
			this.heldPreview = gameObject2;
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void OnDisable()
		{
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x000FFAD8 File Offset: 0x000FDCD8
		public virtual void OnDestroy()
		{
			if (this.heldPreview)
			{
				Object.Destroy(this.heldPreview);
			}
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void Update()
		{
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void FixedUpdate()
		{
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x000FFAF4 File Offset: 0x000FDCF4
		public virtual void OnPrimaryDown()
		{
			if (!this.currentObject)
			{
				this.hostArm.menu.gameObject.SetActive(true);
				return;
			}
			this.hostArm.StartCoroutine(this.HandClosedAnimationThing());
			this.hostArm.jabSound.Play();
			this.hostArm.animator.SetTrigger(ArmModeWithHeldPreview.Punch);
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void OnPrimaryUp()
		{
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void OnSecondaryDown()
		{
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public virtual void OnSecondaryUp()
		{
		}

		// Token: 0x04002BA8 RID: 11176
		protected static readonly int Holding = Animator.StringToHash("Holding");

		// Token: 0x04002BA9 RID: 11177
		protected static readonly int Punch = Animator.StringToHash("Punch");

		// Token: 0x04002BAA RID: 11178
		protected SandboxArm hostArm;

		// Token: 0x04002BAB RID: 11179
		protected SpawnableObject currentObject;

		// Token: 0x04002BAC RID: 11180
		protected GameObject heldPreview;
	}
}
