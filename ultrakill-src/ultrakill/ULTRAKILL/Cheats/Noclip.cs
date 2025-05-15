using System;
using System.Collections;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200061F RID: 1567
	public class Noclip : ICheat
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x0010D5A8 File Offset: 0x0010B7A8
		public string LongName
		{
			get
			{
				return "Noclip";
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x0010D5AF File Offset: 0x0010B7AF
		public string Identifier
		{
			get
			{
				return "ultrakill.noclip";
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06002371 RID: 9073 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonEnabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string ButtonDisabledOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x0010D5B6 File Offset: 0x0010B7B6
		public string Icon
		{
			get
			{
				return "noclip";
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public bool DefaultState
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x0010D5BD File Offset: 0x0010B7BD
		// (set) Token: 0x06002377 RID: 9079 RVA: 0x0010D5C5 File Offset: 0x0010B7C5
		public bool IsActive { get; private set; }

		// Token: 0x06002378 RID: 9080 RVA: 0x0010D5D0 File Offset: 0x0010B7D0
		public void Enable(CheatsManager manager)
		{
			MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.flight");
			MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.clash-mode");
			this.camera = MonoSingleton<CameraController>.Instance.cam.transform;
			this.rb = MonoSingleton<PlayerTracker>.Instance.GetRigidbody();
			this.kib = MonoSingleton<NewMovement>.Instance.GetComponent<KeepInBounds>();
			this.kib.enabled = false;
			this.vcb = MonoSingleton<NewMovement>.Instance.GetComponent<VerticalClippingBlocker>();
			this.vcb.enabled = false;
			this.IsActive = true;
		}

		// Token: 0x06002379 RID: 9081 RVA: 0x0010D65F File Offset: 0x0010B85F
		public void Disable()
		{
			MonoSingleton<NewMovement>.Instance.enabled = true;
			this.kib.enabled = true;
			this.vcb.enabled = true;
			this.rb.isKinematic = false;
			this.IsActive = false;
		}

		// Token: 0x0600237A RID: 9082 RVA: 0x0010D697 File Offset: 0x0010B897
		public IEnumerator Coroutine(CheatsManager manager)
		{
			Vector3 lastDirection = Vector3.zero;
			while (this.IsActive)
			{
				lastDirection = this.UpdateTick();
				yield return null;
			}
			this.rb.velocity = lastDirection;
			yield break;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x0010D6A8 File Offset: 0x0010B8A8
		private Vector3 UpdateTick()
		{
			float num = 1f;
			if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed)
			{
				num = 2.5f;
			}
			Vector3 vector = Vector3.zero;
			if (!GameStateManager.Instance.IsStateActive("alter-menu"))
			{
				float deltaTime = Time.deltaTime;
				Vector2 vector2 = Vector2.ClampMagnitude(MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>(), 1f);
				vector = this.camera.forward * vector2.y * 40f * num;
				vector += this.camera.right * vector2.x * 40f * num;
				if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
				{
					vector += new Vector3(0f, 40f, 0f) * 1f * num;
				}
				if (MonoSingleton<InputManager>.Instance.InputSource.Slide.IsPressed)
				{
					vector += new Vector3(0f, -40f, 0f) * 1f * num;
				}
				this.rb.position += vector * deltaTime;
			}
			MonoSingleton<NewMovement>.Instance.enabled = false;
			this.rb.isKinematic = true;
			return vector;
		}

		// Token: 0x04002E75 RID: 11893
		private Rigidbody rb;

		// Token: 0x04002E76 RID: 11894
		private KeepInBounds kib;

		// Token: 0x04002E77 RID: 11895
		private VerticalClippingBlocker vcb;

		// Token: 0x04002E78 RID: 11896
		private Transform camera;
	}
}
