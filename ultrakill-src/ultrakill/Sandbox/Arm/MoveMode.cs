using System;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sandbox.Arm
{
	// Token: 0x0200056F RID: 1391
	public class MoveMode : ISandboxArmMode, ISandboxArmDebugGUI
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x001004F7 File Offset: 0x000FE6F7
		public string Name
		{
			get
			{
				return "Move";
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x001004FE File Offset: 0x000FE6FE
		public bool CanOpenMenu
		{
			get
			{
				return this.manipulatedObject == null;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x00100509 File Offset: 0x000FE709
		public virtual string Icon
		{
			get
			{
				return "move";
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x0002D245 File Offset: 0x0002B445
		public virtual bool Raycast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x00100510 File Offset: 0x000FE710
		public virtual void OnEnable(SandboxArm arm)
		{
			arm.ResetAnimator();
			if (arm.animator.isActiveAndEnabled)
			{
				arm.animator.SetBool(MoveMode.Manipulating, true);
			}
			this.hostArm = arm;
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnDisable()
		{
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnDestroy()
		{
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x00100540 File Offset: 0x000FE740
		public void Update()
		{
			this.IntegrityCheck();
			if (this.manipulatedObject == null)
			{
				return;
			}
			if (ExperimentalArmRotation.Enabled)
			{
				Quaternion quaternion = Quaternion.identity;
				if (MonoSingleton<InputManager>.Instance.InputSource.NextVariation.IsPressed)
				{
					Vector2 vector = Mouse.current.delta.ReadValue();
					quaternion = Quaternion.AngleAxis(vector.x * -0.1f, Vector3.up) * Quaternion.AngleAxis(vector.y * 0.1f, Vector3.right);
					this.manipulatedObject.rotationOffset = quaternion * this.manipulatedObject.rotationOffset;
					MonoSingleton<CameraController>.Instance.activated = false;
				}
				else
				{
					MonoSingleton<CameraController>.Instance.activated = true;
				}
				this.manipulatedObject.target.rotation = MonoSingleton<CameraController>.Instance.transform.rotation * this.manipulatedObject.rotationOffset;
			}
			else
			{
				Vector3 vector2 = new Vector3(this.manipulatedObject.originalRotation.x, MonoSingleton<CameraController>.Instance.rotationY + this.manipulatedObject.simpleRotationOffset, this.manipulatedObject.originalRotation.z);
				vector2 = (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapRotation(vector2) : vector2);
				this.manipulatedObject.target.eulerAngles = vector2;
			}
			if (ExperimentalArmRotation.Enabled)
			{
				this.targetPos = MonoSingleton<CameraController>.Instance.transform.position + MonoSingleton<CameraController>.Instance.transform.forward * this.manipulatedObject.distance;
				if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
				{
					this.targetPos = SandboxUtils.SnapPos(this.targetPos);
				}
				Vector3 vector3 = this.targetPos - this.manipulatedObject.target.position;
				this.manipulatedObject.particles.transform.position = this.manipulatedObject.collider.bounds.center;
				this.manipulatedObject.target.position += vector3 * 15.5f * Time.deltaTime;
				this.objectVelocity = vector3;
			}
			else
			{
				Vector3 vector4 = new Vector3(this.manipulatedObject.originalRotation.x, MonoSingleton<CameraController>.Instance.rotationY + this.manipulatedObject.simpleRotationOffset, this.manipulatedObject.originalRotation.z);
				vector4 = (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapRotation(vector4) : vector4);
				this.manipulatedObject.target.eulerAngles = vector4;
				Vector3 vector5 = Quaternion.Euler(0f, -(this.manipulatedObject.originalRotation.y - vector4.y), 0f) * (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapPos(this.manipulatedObject.positionOffset) : this.manipulatedObject.positionOffset);
				Vector3 vector6 = this.manipulatedObject.target.position - vector5;
				Vector3 vector7 = MonoSingleton<CameraController>.Instance.transform.position + MonoSingleton<CameraController>.Instance.transform.forward * this.manipulatedObject.distance;
				this.targetPos = vector7;
				if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
				{
					vector7 = SandboxUtils.SnapPos(vector7);
				}
				Vector3 vector8 = vector7 - vector6;
				this.manipulatedObject.particles.transform.position = this.manipulatedObject.collider.bounds.center;
				this.manipulatedObject.target.position += vector8 * 15.5f * Time.deltaTime;
				this.objectVelocity = vector8;
			}
			float y = Mouse.current.scroll.ReadValue().y;
			this.hostArm.animator.SetFloat(MoveMode.PushZ, y);
			this.manipulatedObject.distance += Mathf.Clamp(this.manipulatedObject.distance, 1f, 10f) / 10f * y * 0.05f;
			this.manipulatedObject.distance = Mathf.Max(0f, this.manipulatedObject.distance);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void FixedUpdate()
		{
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00100980 File Offset: 0x000FEB80
		public void OnPrimaryDown()
		{
			if (this.manipulatedObject != null)
			{
				return;
			}
			if (!this.hostArm.hitSomething)
			{
				return;
			}
			SandboxSpawnableInstance prop = SandboxUtils.GetProp(this.hostArm.hit.collider.gameObject, false);
			if (!prop)
			{
				return;
			}
			prop.Pause(true);
			MonoSingleton<GunControl>.Instance.activated = false;
			this.manipulatedObject = new MoveMode.ManipulatedObject(this.hostArm.hit, prop)
			{
				particles = Object.Instantiate<GameObject>(this.hostArm.manipulateEffect)
			};
			this.hostArm.animator.SetBool(MoveMode.Pinched, true);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x00100A20 File Offset: 0x000FEC20
		public void OnPrimaryUp()
		{
			if (this.manipulatedObject == null)
			{
				return;
			}
			Vector3 vector = this.objectVelocity;
			this.manipulatedObject.particles.transform.position = this.manipulatedObject.collider.bounds.center;
			this.manipulatedObject.target.position += vector * 15.5f * Time.deltaTime;
			if (SandboxArmDebug.DebugActive)
			{
				MoveMode.Log.Info(string.Format("Target Position: {0}\n", this.targetPos) + string.Format("Object's Position: {0}\n", this.manipulatedObject.target.position) + string.Format("Position Delta: {0}", vector), null, null, null);
			}
			this.ReleaseManipulatedObject(vector * 6.5f, null);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00100B14 File Offset: 0x000FED14
		public bool OnGUI()
		{
			if (this.manipulatedObject == null)
			{
				if (this.hostArm.hitSomething && this.hostArm.hit.collider != null)
				{
					GUILayout.Label("Hit: " + this.hostArm.hit.collider.name, Array.Empty<GUILayoutOption>());
					GUILayout.Space(8f);
					int layer = this.hostArm.hit.collider.gameObject.layer;
					GUILayout.Label("Layer: " + layer.ToString() + " - " + LayerMask.LayerToName(layer), Array.Empty<GUILayoutOption>());
					GUILayout.Label("Tag: " + this.hostArm.hit.collider.tag, Array.Empty<GUILayoutOption>());
				}
				else
				{
					GUILayout.Label("No raycast hit.", Array.Empty<GUILayoutOption>());
				}
				return true;
			}
			GUILayout.Label("Moving: " + this.manipulatedObject.target.name, Array.Empty<GUILayoutOption>());
			GUILayout.Space(8f);
			string text = "Computed Target Point: ";
			Vector3 vector = this.targetPos;
			GUILayout.Label(text + vector.ToString(), Array.Empty<GUILayoutOption>());
			GUILayout.Label("Object's (Pivot) Position " + this.manipulatedObject.target.position.ToString(), Array.Empty<GUILayoutOption>());
			string text2 = "Velocity: ";
			vector = this.objectVelocity;
			GUILayout.Label(text2 + vector.ToString(), Array.Empty<GUILayoutOption>());
			return true;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x00100CB8 File Offset: 0x000FEEB8
		public void OnSecondaryDown()
		{
			if (this.manipulatedObject == null)
			{
				return;
			}
			if (this.manipulatedObject.spawnable != null && this.manipulatedObject.spawnable.disallowFreezing)
			{
				return;
			}
			if (this.manipulatedObject.particles)
			{
				Object.Destroy(this.manipulatedObject.particles);
			}
			MonoSingleton<GunControl>.Instance.activated = true;
			this.hostArm.animator.SetBool(MoveMode.Pinched, false);
			this.hostArm.animator.SetTrigger(MoveMode.Crush);
			SandboxSpawnableInstance component = this.manipulatedObject.target.GetComponent<SandboxSpawnableInstance>();
			component.Pause(true);
			if (this.manipulatedObject.target.CompareTag("Untagged"))
			{
				this.manipulatedObject.target.tag = "Floor";
			}
			if (MonoSingleton<SandboxNavmesh>.Instance != null)
			{
				MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(component);
			}
			this.hostArm.freezeSound.pitch = Random.Range(1f, 1.05f);
			this.hostArm.freezeSound.Play();
			GameObject gameObject = new GameObject("Ghost Effect Wrapper")
			{
				transform = 
				{
					position = this.manipulatedObject.collider.bounds.center
				}
			};
			SandboxUtils.StripForPreview(Object.Instantiate<Transform>(this.manipulatedObject.target, gameObject.transform, true), this.hostArm.previewMaterial, true);
			gameObject.gameObject.AddComponent<SandboxGhostEffect>();
			MonoSingleton<CameraController>.Instance.activated = true;
			this.manipulatedObject = null;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void OnSecondaryUp()
		{
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x00100E4C File Offset: 0x000FF04C
		private void IntegrityCheck()
		{
			if (this.manipulatedObject == null)
			{
				return;
			}
			if (this.manipulatedObject.target != null && this.manipulatedObject.collider != null)
			{
				return;
			}
			MoveMode.Log.Warning("Integrity check failed, releasing manipulated object", null, null, null);
			this.ReleaseManipulatedObject(Vector3.zero, null);
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x00100EB0 File Offset: 0x000FF0B0
		private void ReleaseManipulatedObject(Vector3 velocity, Quaternion? deltaRot = null)
		{
			Object.Destroy(this.manipulatedObject.particles);
			MonoSingleton<GunControl>.Instance.activated = true;
			this.hostArm.animator.SetBool(MoveMode.Pinched, false);
			MonoSingleton<CameraController>.Instance.activated = true;
			SandboxSpawnableInstance sandboxSpawnableInstance;
			if (this.manipulatedObject.target != null && this.manipulatedObject.target.TryGetComponent<SandboxSpawnableInstance>(out sandboxSpawnableInstance))
			{
				if (!sandboxSpawnableInstance.alwaysFrozen)
				{
					sandboxSpawnableInstance.Resume();
				}
				else
				{
					sandboxSpawnableInstance.frozen = true;
				}
			}
			if (this.manipulatedObject.rigidbody && (!this.manipulatedObject.spawnable.sourceObject || !this.manipulatedObject.spawnable.sourceObject.alwaysKinematic))
			{
				this.manipulatedObject.rigidbody.isKinematic = false;
				this.manipulatedObject.rigidbody.velocity = velocity;
				this.manipulatedObject.rigidbody.angularVelocity = ((deltaRot != null) ? deltaRot.GetValueOrDefault().eulerAngles : Vector3.zero);
			}
			this.manipulatedObject = null;
		}

		// Token: 0x04002BBD RID: 11197
		private static readonly global::plog.Logger Log = new global::plog.Logger("MoveMode");

		// Token: 0x04002BBE RID: 11198
		private static readonly int Manipulating = Animator.StringToHash("Manipulating");

		// Token: 0x04002BBF RID: 11199
		private static readonly int Pinched = Animator.StringToHash("Pinched");

		// Token: 0x04002BC0 RID: 11200
		private static readonly int PushZ = Animator.StringToHash("PushZ");

		// Token: 0x04002BC1 RID: 11201
		private static readonly int Crush = Animator.StringToHash("Crush");

		// Token: 0x04002BC2 RID: 11202
		private SandboxArm hostArm;

		// Token: 0x04002BC3 RID: 11203
		private MoveMode.ManipulatedObject manipulatedObject;

		// Token: 0x04002BC4 RID: 11204
		private Vector3 targetPos;

		// Token: 0x04002BC5 RID: 11205
		private Vector3 objectVelocity;

		// Token: 0x02000570 RID: 1392
		public class ManipulatedObject
		{
			// Token: 0x06001FA0 RID: 8096 RVA: 0x00101028 File Offset: 0x000FF228
			public ManipulatedObject(RaycastHit hit, SandboxSpawnableInstance propOverwrite = null)
			{
				GameObject gameObject = (propOverwrite ? propOverwrite.gameObject : hit.collider.gameObject);
				this.target = gameObject.transform;
				this.rigidbody = gameObject.GetComponent<Rigidbody>();
				this.collider = gameObject.GetComponent<Collider>();
				if (this.collider == null)
				{
					SandboxPropPart sandboxPropPart;
					if (gameObject.TryGetComponent<SandboxPropPart>(out sandboxPropPart))
					{
						this.collider = sandboxPropPart.parent.GetComponent<Collider>();
					}
					if (this.collider == null)
					{
						this.collider = gameObject.GetComponentInChildren<Collider>();
					}
				}
				this.positionOffset = this.target.position - hit.point;
				CameraController instance = MonoSingleton<CameraController>.Instance;
				this.distance = Vector3.Distance(instance.transform.position, hit.point);
				this.originalRotation = this.target.eulerAngles;
				this.rotationOffset = Quaternion.Inverse(instance.transform.rotation) * this.target.rotation;
				this.simpleRotationOffset = this.originalRotation.y - instance.rotationY;
				if (this.rigidbody)
				{
					this.rigidbody.isKinematic = true;
				}
				this.spawnable = (propOverwrite ? propOverwrite : SandboxUtils.GetProp(gameObject, false));
			}

			// Token: 0x04002BC6 RID: 11206
			public Transform target;

			// Token: 0x04002BC7 RID: 11207
			public SandboxSpawnableInstance spawnable;

			// Token: 0x04002BC8 RID: 11208
			public GameObject particles;

			// Token: 0x04002BC9 RID: 11209
			public Rigidbody rigidbody;

			// Token: 0x04002BCA RID: 11210
			public Collider collider;

			// Token: 0x04002BCB RID: 11211
			public Vector3 positionOffset;

			// Token: 0x04002BCC RID: 11212
			public float distance;

			// Token: 0x04002BCD RID: 11213
			public Vector3 originalRotation;

			// Token: 0x04002BCE RID: 11214
			public float simpleRotationOffset;

			// Token: 0x04002BCF RID: 11215
			public Quaternion rotationOffset;
		}
	}
}
