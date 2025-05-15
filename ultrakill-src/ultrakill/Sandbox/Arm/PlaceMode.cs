using System;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Sandbox.Arm
{
	// Token: 0x02000571 RID: 1393
	public class PlaceMode : ArmModeWithHeldPreview
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0010117F File Offset: 0x000FF37F
		public override string Name
		{
			get
			{
				return "Place";
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0002D245 File Offset: 0x0002B445
		public override bool Raycast
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x00101186 File Offset: 0x000FF386
		public override void SetPreview(SpawnableObject obj)
		{
			base.SetPreview(obj);
			if (this.worldPreviewObject != null)
			{
				Object.Destroy(this.worldPreviewObject);
			}
			this.CreateWorldPreview(obj);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x001011B0 File Offset: 0x000FF3B0
		private void CreateWorldPreview(SpawnableObject obj)
		{
			MeshFilter meshFilter;
			SandboxProp sandboxProp;
			if (obj.gameObject.TryGetComponent<MeshFilter>(out meshFilter) && obj.gameObject.TryGetComponent<SandboxProp>(out sandboxProp) && !sandboxProp.forceFullWorldPreview)
			{
				Mesh sharedMesh = meshFilter.sharedMesh;
				this.CreateWorldPreview();
				this.worldPreviewObject.transform.localRotation = obj.gameObject.transform.localRotation;
				this.worldPreviewObject.GetComponent<MeshFilter>().mesh = sharedMesh;
				this.worldPreviewObject.GetComponent<MeshRenderer>().material = this.hostArm.previewMaterial;
				return;
			}
			this.worldPreviewObject = Object.Instantiate<GameObject>(obj.gameObject);
			SandboxUtils.StripForPreview(this.worldPreviewObject.transform, this.hostArm.previewMaterial, true);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0010126A File Offset: 0x000FF46A
		private void CreateWorldPreview()
		{
			this.worldPreviewObject = new GameObject("World Preview");
			this.worldPreviewObject.AddComponent<MeshFilter>();
			this.worldPreviewObject.AddComponent<MeshRenderer>().material = this.hostArm.previewMaterial;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x001012A4 File Offset: 0x000FF4A4
		public override void Update()
		{
			base.Update();
			if (!this.worldPreviewObject)
			{
				return;
			}
			this.worldPreviewObject.SetActive(this.hostArm.hitSomething);
			if (!this.hostArm.hitSomething)
			{
				return;
			}
			Vector3 vector = ((this.currentObject.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Enemy) ? Vector3.up : this.hostArm.hit.normal);
			this.worldPreviewObject.transform.SetPositionAndRotation(this.CalculatePropPosition(this.hostArm.hit), this.CalculatePropRotation(vector, this.currentObject.gameObject.transform.rotation));
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0010134C File Offset: 0x000FF54C
		private Quaternion CalculatePropRotation(Vector3 normal, Quaternion baseRotation)
		{
			float num = MonoSingleton<CameraController>.Instance.rotationY;
			if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
			{
				num /= 90f;
				num = Mathf.Round(num);
				num *= 90f;
			}
			return Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.AngleAxis(num, Vector3.up) * baseRotation;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x001013A4 File Offset: 0x000FF5A4
		private Vector3 CalculatePropPosition(RaycastHit hit)
		{
			if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
			{
				Vector3 vector = Vector3.zero;
				if (hit.transform && SandboxUtils.GetProp(hit.collider.gameObject, true))
				{
					Vector3 vector2 = SandboxUtils.SnapPos(hit.transform.position);
					vector = -new Vector3(0f, vector2.y - hit.transform.position.y, 0f);
				}
				Vector3 vector3 = SandboxUtils.SnapPos(hit.point, vector, 0.5f);
				Plane plane = new Plane(hit.normal, vector3);
				float distanceToPoint = plane.GetDistanceToPoint(hit.point);
				vector3 += hit.normal * distanceToPoint;
				if (this.currentObject && this.currentObject.spawnOffset != 0f)
				{
					vector3 += hit.normal * this.currentObject.spawnOffset;
				}
				return vector3;
			}
			if (this.currentObject && this.currentObject.spawnOffset != 0f)
			{
				return hit.point + hit.normal * this.currentObject.spawnOffset;
			}
			return hit.point;
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x001014F4 File Offset: 0x000FF6F4
		public override void OnPrimaryDown()
		{
			base.OnPrimaryDown();
			if (!this.currentObject)
			{
				return;
			}
			if (!this.hostArm.hitSomething)
			{
				return;
			}
			Vector3 vector = ((this.currentObject.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Enemy) ? Vector3.up : this.hostArm.hit.normal);
			GameObject gameObject = Object.Instantiate<GameObject>(this.currentObject.gameObject, this.CalculatePropPosition(this.hostArm.hit), this.CalculatePropRotation(vector, this.currentObject.gameObject.transform.localRotation), this.hostArm.GetGoreZone().transform);
			SandboxProp sandboxProp;
			if (gameObject.TryGetComponent<SandboxProp>(out sandboxProp))
			{
				sandboxProp.sourceObject = this.currentObject;
			}
			bool flag = this.currentObject.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Enemy;
			if (flag)
			{
				gameObject.AddComponent<SandboxEnemy>().sourceObject = this.currentObject;
				MonoSingleton<PresenceController>.Instance.AddToStatInt("sandbox_spawned_enemies", 1);
				if (MonoSingleton<SandboxNavmesh>.Instance)
				{
					MonoSingleton<SandboxNavmesh>.Instance.EnsurePositionWithinBounds(gameObject.transform.position);
				}
			}
			else
			{
				MonoSingleton<PresenceController>.Instance.AddToStatInt("sandbox_spawned_props", 1);
			}
			SandboxSpawnableInstance sandboxSpawnableInstance;
			if (gameObject.TryGetComponent<SandboxSpawnableInstance>(out sandboxSpawnableInstance))
			{
				sandboxSpawnableInstance.frozen = !flag && !SpawnPhysics.PhysicsDynamic;
				if (this.currentObject.defaultSettings != null && this.currentObject.defaultSettings.Length != 0)
				{
					sandboxSpawnableInstance.ApplyAlterOptions(this.currentObject.defaultSettings);
				}
			}
			gameObject.SetActive(true);
			Rigidbody rigidbody;
			if (this.currentObject.spawnableObjectType != SpawnableObject.SpawnableObjectDataType.Enemy && gameObject.TryGetComponent<Rigidbody>(out rigidbody))
			{
				rigidbody.isKinematic = !SpawnPhysics.PhysicsDynamic;
				if (this.currentObject.spawnableType == SpawnableType.Prop && !SpawnPhysics.PhysicsDynamic && MonoSingleton<SandboxNavmesh>.Instance != null)
				{
					MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(sandboxProp);
				}
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x001016BA File Offset: 0x000FF8BA
		public override void OnEnable(SandboxArm arm)
		{
			base.OnEnable(arm);
			if (this.worldPreviewObject != null)
			{
				this.worldPreviewObject.SetActive(true);
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x001016DD File Offset: 0x000FF8DD
		public override void OnDisable()
		{
			base.OnDisable();
			if (this.worldPreviewObject != null)
			{
				this.worldPreviewObject.SetActive(false);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x001016FF File Offset: 0x000FF8FF
		public override void OnDestroy()
		{
			base.OnDestroy();
			if (this.worldPreviewObject != null)
			{
				Object.Destroy(this.worldPreviewObject);
			}
		}

		// Token: 0x04002BD0 RID: 11216
		private GameObject worldPreviewObject;
	}
}
