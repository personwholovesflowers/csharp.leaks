using System;
using System.Collections;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x02000613 RID: 1555
	public class Flight : ICheat
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x060022EB RID: 8939 RVA: 0x0010CE57 File Offset: 0x0010B057
		public string LongName
		{
			get
			{
				return "Flight";
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x0010CE5E File Offset: 0x0010B05E
		public string Identifier
		{
			get
			{
				return "ultrakill.flight";
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x0010CE65 File Offset: 0x0010B065
		public string ButtonEnabledOverride { get; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x0010CE6D File Offset: 0x0010B06D
		public string ButtonDisabledOverride { get; }

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x0010CE75 File Offset: 0x0010B075
		public string Icon
		{
			get
			{
				return "flight";
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0010CE7C File Offset: 0x0010B07C
		// (set) Token: 0x060022F1 RID: 8945 RVA: 0x0010CE84 File Offset: 0x0010B084
		public bool IsActive { get; private set; }

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0010CE8D File Offset: 0x0010B08D
		public bool DefaultState { get; }

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0010CE98 File Offset: 0x0010B098
		public void Enable(CheatsManager manager)
		{
			MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.noclip");
			MonoSingleton<CheatsManager>.Instance.DisableCheat("ultrakill.clash-mode");
			MonoSingleton<NewMovement>.Instance.enabled = false;
			this.rigidbody = MonoSingleton<NewMovement>.Instance.GetComponent<Rigidbody>();
			this.camera = MonoSingleton<CameraController>.Instance.transform;
			this.IsActive = true;
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0010CEF5 File Offset: 0x0010B0F5
		public void Disable()
		{
			this.IsActive = false;
			MonoSingleton<NewMovement>.Instance.enabled = true;
			this.rigidbody.useGravity = true;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0010CF15 File Offset: 0x0010B115
		public IEnumerator Coroutine(CheatsManager manager)
		{
			while (this.IsActive)
			{
				this.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0010CF24 File Offset: 0x0010B124
		private void Update()
		{
			float num = 1f;
			if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed)
			{
				num = 2.5f;
			}
			Vector3 vector = Vector3.zero;
			Vector2 vector2 = Vector2.ClampMagnitude(MonoSingleton<InputManager>.Instance.InputSource.Move.ReadValue<Vector2>(), 1f);
			vector += this.camera.right * vector2.x;
			vector += this.camera.forward * vector2.y;
			if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
			{
				vector += Vector3.up;
			}
			if (MonoSingleton<InputManager>.Instance.InputSource.Slide.IsPressed)
			{
				vector += Vector3.down;
			}
			this.rigidbody.velocity = vector * 30f * num;
			MonoSingleton<NewMovement>.Instance.enabled = false;
			this.rigidbody.isKinematic = false;
			this.rigidbody.useGravity = false;
		}

		// Token: 0x04002E4C RID: 11852
		private Rigidbody rigidbody;

		// Token: 0x04002E4D RID: 11853
		private Transform camera;
	}
}
