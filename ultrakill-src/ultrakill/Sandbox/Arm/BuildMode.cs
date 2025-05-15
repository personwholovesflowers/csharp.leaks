using System;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Sandbox.Arm
{
	// Token: 0x0200056C RID: 1388
	public class BuildMode : ArmModeWithHeldPreview
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001F67 RID: 8039 RVA: 0x000FFBF8 File Offset: 0x000FDDF8
		public override string Name
		{
			get
			{
				return "Build";
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x000FFBFF File Offset: 0x000FDDFF
		public override void SetPreview(SpawnableObject obj)
		{
			base.SetPreview(obj);
			this.SetupBlockCreator(obj);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000FFC10 File Offset: 0x000FDE10
		private void SetupBlockCreator(SpawnableObject template)
		{
			this.firstBrushPositionSet = false;
			if (this.pointAIndicatorObject)
			{
				Object.Destroy(this.pointAIndicatorObject);
			}
			if (this.worldPreviewObject)
			{
				Object.Destroy(this.worldPreviewObject);
			}
			this.pointAIndicatorObject = Object.Instantiate<GameObject>(this.hostArm.axisPoint);
			if (this.pointBIndicatorObject)
			{
				Object.Destroy(this.pointBIndicatorObject);
			}
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000FFC84 File Offset: 0x000FDE84
		public override void Update()
		{
			base.Update();
			if (this.tickDelay > 0f)
			{
				this.tickDelay = Mathf.MoveTowards(this.tickDelay, 0f, Time.deltaTime);
			}
			Transform transform = MonoSingleton<CameraController>.Instance.transform;
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(transform.position, transform.forward, out raycastHit, 75f, this.hostArm.raycastLayers);
			if (!this.firstBrushPositionSet)
			{
				this.pointAIndicatorObject.SetActive(flag);
				this.pointAIndicatorObject.transform.position = this.CalculatePropPosition(raycastHit);
				return;
			}
			RaycastHit raycastHit2;
			bool flag2 = Physics.Raycast(transform.position, transform.forward, out raycastHit2, 5f, this.hostArm.raycastLayers);
			Vector3 vector = Vector3.zero;
			if (flag2)
			{
				vector = raycastHit2.point + new Vector3(0f, 0f, 0f);
			}
			else
			{
				vector = transform.position + transform.forward * 4.5f;
			}
			vector = SandboxUtils.SnapPos(vector, this.brushOffset, ULTRAKILL.Cheats.Snapping.SnappingEnabled ? 0.5f : 7.5f);
			this.pointBIndicatorObject.SetActive(true);
			this.pointBIndicatorObject.transform.position = vector;
			if (Mathf.Abs(this.firstBlockPos.x - vector.x) >= 1f)
			{
				this.secondBlockPos.x = vector.x;
			}
			if (Mathf.Abs(this.firstBlockPos.y - vector.y) >= 1f)
			{
				this.secondBlockPos.y = vector.y;
			}
			if (Mathf.Abs(this.firstBlockPos.z - vector.z) >= 1f)
			{
				this.secondBlockPos.z = vector.z;
			}
			if (this.secondBlockPos != this.previousSecondBlockPos)
			{
				if (this.tickDelay == 0f)
				{
					this.hostArm.tickSound.pitch = Random.Range(0.74f, 0.76f);
					this.hostArm.tickSound.Play();
					this.tickDelay = 0.05f;
				}
				this.previousSecondBlockPos = this.secondBlockPos;
				Vector3 vector2;
				Vector3 vector3;
				SandboxUtils.SmallerBigger(this.firstBlockPos, this.secondBlockPos, out vector2, out vector3);
				Vector3 vector4 = vector3 - vector2;
				this.worldPreviewObject.GetComponent<MeshFilter>().mesh = SandboxUtils.GenerateProceduralMesh(vector4, true);
				this.worldPreviewObject.transform.position = vector2;
			}
			this.worldPreviewObject.SetActive(true);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000FFF24 File Offset: 0x000FE124
		private Vector3 CalculatePropPosition(RaycastHit hit)
		{
			if (!ULTRAKILL.Cheats.Snapping.SnappingEnabled)
			{
				this.brushOffset = Vector3.zero;
			}
			Vector3 vector = Vector3.zero;
			if (hit.transform && SandboxUtils.GetProp(hit.collider.gameObject, true))
			{
				Vector3 vector2 = SandboxUtils.SnapPos(hit.transform.position);
				vector = -new Vector3(0f, vector2.y - hit.transform.position.y, 0f);
			}
			Vector3 vector3 = (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapPos(hit.point, vector, 0.5f) : hit.point);
			Plane plane = new Plane(hit.normal, vector3);
			float distanceToPoint = plane.GetDistanceToPoint(hit.point);
			vector3 += hit.normal * distanceToPoint;
			if (hit.transform && !SandboxUtils.GetProp(hit.collider.gameObject, true))
			{
				vector = hit.normal * distanceToPoint;
			}
			this.brushOffset = vector;
			return vector3;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x00100044 File Offset: 0x000FE244
		public override void OnPrimaryDown()
		{
			base.OnPrimaryDown();
			if (!this.currentObject)
			{
				return;
			}
			this.hostArm.jabSound.Play();
			this.hostArm.animator.SetTrigger(BuildMode.Punch);
			if (!this.firstBrushPositionSet && this.hostArm.hitSomething)
			{
				this.firstBlockPos = this.CalculatePropPosition(this.hostArm.hit);
				this.secondBlockPos = this.firstBlockPos + Vector3.one / 0.5f;
				this.previousSecondBlockPos = this.secondBlockPos;
				this.firstBrushPositionSet = true;
				this.pointBIndicatorObject = Object.Instantiate<GameObject>(this.hostArm.axisPoint);
				this.CreateWorldPreview();
				return;
			}
			this.hostArm.tickSound.Play();
			Vector3 vector;
			Vector3 vector2;
			SandboxUtils.SmallerBigger(this.firstBlockPos, this.secondBlockPos, out vector, out vector2);
			Vector3 vector3 = vector2 - vector;
			GameObject gameObject = SandboxUtils.CreateFinalBlock(this.currentObject, vector, vector3, this.currentObject.isWater);
			BrushBlock brushBlock;
			if (gameObject.TryGetComponent<BrushBlock>(out brushBlock))
			{
				SandboxSpawnableInstance component = gameObject.GetComponent<SandboxSpawnableInstance>();
				component.frozen = !SpawnPhysics.PhysicsDynamic;
				if (!SpawnPhysics.PhysicsDynamic && MonoSingleton<SandboxNavmesh>.Instance != null)
				{
					MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(component);
				}
				Rigidbody rigidbody;
				if (brushBlock.TryGetComponent<Rigidbody>(out rigidbody))
				{
					rigidbody.isKinematic = !SpawnPhysics.PhysicsDynamic;
				}
			}
			this.firstBrushPositionSet = false;
			this.SetupBlockCreator(this.currentObject);
			MonoSingleton<PresenceController>.Instance.AddToStatInt("sandbox_built_brushes", 1);
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x001001CA File Offset: 0x000FE3CA
		private void CreateWorldPreview()
		{
			this.worldPreviewObject = new GameObject("World Preview");
			this.worldPreviewObject.AddComponent<MeshFilter>();
			this.worldPreviewObject.AddComponent<MeshRenderer>().material = this.hostArm.previewMaterial;
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00100204 File Offset: 0x000FE404
		public override void OnDisable()
		{
			base.OnDisable();
			if (this.worldPreviewObject)
			{
				this.worldPreviewObject.SetActive(false);
			}
			if (this.pointAIndicatorObject)
			{
				this.pointAIndicatorObject.SetActive(false);
			}
			if (this.pointBIndicatorObject)
			{
				this.pointBIndicatorObject.SetActive(false);
			}
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00100264 File Offset: 0x000FE464
		public override void OnDestroy()
		{
			base.OnDestroy();
			if (this.worldPreviewObject)
			{
				Object.Destroy(this.worldPreviewObject);
			}
			if (this.pointAIndicatorObject)
			{
				Object.Destroy(this.pointAIndicatorObject);
			}
			if (this.pointBIndicatorObject)
			{
				Object.Destroy(this.pointBIndicatorObject);
			}
		}

		// Token: 0x04002BB0 RID: 11184
		private float tickDelay;

		// Token: 0x04002BB1 RID: 11185
		private bool firstBrushPositionSet;

		// Token: 0x04002BB2 RID: 11186
		private Vector3 firstBlockPos;

		// Token: 0x04002BB3 RID: 11187
		private Vector3 secondBlockPos;

		// Token: 0x04002BB4 RID: 11188
		private Vector3 previousSecondBlockPos;

		// Token: 0x04002BB5 RID: 11189
		private Vector3 brushOffset = Vector3.one;

		// Token: 0x04002BB6 RID: 11190
		private GameObject worldPreviewObject;

		// Token: 0x04002BB7 RID: 11191
		private GameObject pointAIndicatorObject;

		// Token: 0x04002BB8 RID: 11192
		private GameObject pointBIndicatorObject;

		// Token: 0x04002BB9 RID: 11193
		private new static readonly int Punch = Animator.StringToHash("Punch");
	}
}
