using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class BackgroundCamera : MonoBehaviour
{
	// Token: 0x06000061 RID: 97 RVA: 0x00004B48 File Offset: 0x00002D48
	private void Awake()
	{
		BackgroundCamera.OnRotationUpdate = (Action<Vector3>)Delegate.Combine(BackgroundCamera.OnRotationUpdate, new Action<Vector3>(this.UpdateRotation));
		BackgroundCamera.OnAlignToTransform = (Action<Transform>)Delegate.Combine(BackgroundCamera.OnAlignToTransform, new Action<Transform>(this.AlignToTransform));
		BackgroundCamera.OnPositionUpdate = (Action<Vector3>)Delegate.Combine(BackgroundCamera.OnPositionUpdate, new Action<Vector3>(this.UpdatePosition));
		if (this.backgroundCamera != null)
		{
			this.moveTransform.localPosition = Vector3.zero;
		}
		if (this.targetCamera != null)
		{
			this.AssignTargetCamera(this.targetCamera);
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004BF0 File Offset: 0x00002DF0
	private void OnDestroy()
	{
		BackgroundCamera.OnRotationUpdate = (Action<Vector3>)Delegate.Remove(BackgroundCamera.OnRotationUpdate, new Action<Vector3>(this.UpdateRotation));
		BackgroundCamera.OnAlignToTransform = (Action<Transform>)Delegate.Remove(BackgroundCamera.OnAlignToTransform, new Action<Transform>(this.AlignToTransform));
		BackgroundCamera.OnPositionUpdate = (Action<Vector3>)Delegate.Remove(BackgroundCamera.OnPositionUpdate, new Action<Vector3>(this.UpdatePosition));
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00004C60 File Offset: 0x00002E60
	private void Start()
	{
		BackgroundCamera.CameraMovementTypes cameraMovementTypes = this.movementType;
		if (cameraMovementTypes == BackgroundCamera.CameraMovementTypes.RelativeTransform)
		{
			this.AssignMainCameraAsTargetTransform();
			return;
		}
		if (cameraMovementTypes != BackgroundCamera.CameraMovementTypes.CharacterController)
		{
			return;
		}
		this.GetCharacterControllerFromMainCamera();
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00004C8C File Offset: 0x00002E8C
	private void LateUpdate()
	{
		if (this.targetTransform == null)
		{
			this.AssignMainCameraAsTargetTransform();
		}
		this.backgroundCamera.fieldOfView = this.targetCamera.fieldOfView;
		BackgroundCamera.CameraMovementTypes cameraMovementTypes = this.movementType;
		if (cameraMovementTypes != BackgroundCamera.CameraMovementTypes.RelativeTransform)
		{
			if (cameraMovementTypes != BackgroundCamera.CameraMovementTypes.CharacterController)
			{
				return;
			}
			if (this.character == null)
			{
				this.GetCharacterControllerFromMainCamera();
			}
			if (this.moveTransform == null)
			{
				return;
			}
			Vector3 vector = Vector3.Scale(this.character.velocity * Time.deltaTime, this.parallaxStrength);
			this.moveTransform.Translate(this.yawTransform.TransformDirection(this.character.transform.InverseTransformDirection(vector)), Space.World);
			return;
		}
		else
		{
			if (this.moveTransform == null || this.targetCamera == null)
			{
				return;
			}
			if (!this.externalRotationUpdate)
			{
				this.moveTransform.rotation = this.targetTransform.rotation;
			}
			float num = this.sceneCenter.position.x - this.targetTransform.position.x;
			float num2 = this.sceneCenter.position.y - this.targetTransform.position.y;
			float num3 = this.sceneCenter.position.z - this.targetTransform.position.z;
			this.moveTransform.localPosition = new Vector3(-num * this.parallaxStrength.x, -num2 * this.parallaxStrength.y, -num3 * this.parallaxStrength.z);
			return;
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00004E20 File Offset: 0x00003020
	public void UpdateRotation(Vector3 rotationDelta)
	{
		if (!this.externalRotationUpdate)
		{
			return;
		}
		this.viewPitch += rotationDelta.x;
		this.viewPitch = Mathf.Clamp(this.viewPitch, -90f, 90f);
		Quaternion quaternion = Quaternion.AngleAxis(rotationDelta.y, Vector3.up);
		this.pitchTransform.localRotation = Quaternion.AngleAxis(this.viewPitch, Vector3.right);
		this.yawTransform.rotation = quaternion * this.yawTransform.rotation;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00004EAC File Offset: 0x000030AC
	public void UpdatePosition(Vector3 movement)
	{
		Vector3 vector = Vector3.Scale(movement, this.parallaxStrength);
		this.moveTransform.Translate(vector, Space.World);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00004ED4 File Offset: 0x000030D4
	public void AlignToTransform(Transform alignTransform)
	{
		BackgroundCamera.CameraMovementTypes cameraMovementTypes = this.movementType;
		if (cameraMovementTypes != BackgroundCamera.CameraMovementTypes.RelativeTransform && cameraMovementTypes == BackgroundCamera.CameraMovementTypes.CharacterController)
		{
			Quaternion quaternion = Quaternion.AngleAxis(alignTransform.eulerAngles.y, Vector3.up);
			this.yawTransform.rotation = quaternion * this.yawTransform.rotation;
			this.viewPitch = alignTransform.eulerAngles.x;
		}
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004F32 File Offset: 0x00003132
	public void GetCharacterControllerFromMainCamera()
	{
		this.character = Camera.main.gameObject.GetComponentInParent<CharacterController>();
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004F49 File Offset: 0x00003149
	public void AssignSceneCenter(Transform newSceneCenter)
	{
		this.sceneCenter = newSceneCenter;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00004F52 File Offset: 0x00003152
	public void AssignMainCameraAsTargetTransform()
	{
		if (Camera.main != null)
		{
			this.AssignTargetCamera(Camera.main);
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00004F6C File Offset: 0x0000316C
	public void AssignTargetCamera(Camera newTargetCamera)
	{
		this.targetCamera = newTargetCamera;
		this.targetTransform = newTargetCamera.transform;
		this.backgroundCamera.fieldOfView = newTargetCamera.fieldOfView;
	}

	// Token: 0x04000080 RID: 128
	public static Action<Vector3> OnRotationUpdate;

	// Token: 0x04000081 RID: 129
	public static Action<Vector3> OnPositionUpdate;

	// Token: 0x04000082 RID: 130
	public static Action<Transform> OnAlignToTransform;

	// Token: 0x04000083 RID: 131
	[SerializeField]
	private BackgroundCamera.CameraMovementTypes movementType;

	// Token: 0x04000084 RID: 132
	[SerializeField]
	private bool externalRotationUpdate;

	// Token: 0x04000085 RID: 133
	[SerializeField]
	private Camera backgroundCamera;

	// Token: 0x04000086 RID: 134
	[SerializeField]
	private Transform moveTransform;

	// Token: 0x04000087 RID: 135
	[SerializeField]
	private Transform pitchTransform;

	// Token: 0x04000088 RID: 136
	[SerializeField]
	private Transform yawTransform;

	// Token: 0x04000089 RID: 137
	[SerializeField]
	private CharacterController character;

	// Token: 0x0400008A RID: 138
	[SerializeField]
	private Camera targetCamera;

	// Token: 0x0400008B RID: 139
	[SerializeField]
	private Transform sceneCenter;

	// Token: 0x0400008C RID: 140
	[SerializeField]
	private Vector3 parallaxStrength = new Vector3(0.0005f, 5E-05f, 0.0005f);

	// Token: 0x0400008D RID: 141
	private Transform targetTransform;

	// Token: 0x0400008E RID: 142
	private float viewPitch;

	// Token: 0x02000342 RID: 834
	public enum CameraMovementTypes
	{
		// Token: 0x040011C0 RID: 4544
		RelativeTransform,
		// Token: 0x040011C1 RID: 4545
		CharacterController
	}
}
