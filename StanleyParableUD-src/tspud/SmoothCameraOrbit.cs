using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x06000088 RID: 136 RVA: 0x000057BF File Offset: 0x000039BF
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06000089 RID: 137 RVA: 0x000057BF File Offset: 0x000039BF
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x0600008A RID: 138 RVA: 0x000057C8 File Offset: 0x000039C8
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x0600008B RID: 139 RVA: 0x000058F8 File Offset: 0x00003AF8
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening);
			base.transform.rotation = this.rotation;
			this.idleTimer = 0f;
			this.idleSmooth = 0f;
		}
		else
		{
			this.idleTimer += 0.02f;
			if (this.idleTimer > this.autoRotate && this.autoRotate > 0f)
			{
				this.idleSmooth += (0.02f + this.idleSmooth) * 0.005f;
				this.idleSmooth = Mathf.Clamp(this.idleSmooth, 0f, 1f);
				this.xDeg += this.xSpeed * Time.deltaTime * this.idleSmooth * this.autoRotateSpeed;
			}
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening * 2f);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, 0.02f * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00005C21 File Offset: 0x00003E21
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x040000A9 RID: 169
	public Transform target;

	// Token: 0x040000AA RID: 170
	public Vector3 targetOffset;

	// Token: 0x040000AB RID: 171
	public float distance = 5f;

	// Token: 0x040000AC RID: 172
	public float maxDistance = 20f;

	// Token: 0x040000AD RID: 173
	public float minDistance = 0.6f;

	// Token: 0x040000AE RID: 174
	public float xSpeed = 200f;

	// Token: 0x040000AF RID: 175
	public float ySpeed = 200f;

	// Token: 0x040000B0 RID: 176
	public int yMinLimit = -80;

	// Token: 0x040000B1 RID: 177
	public int yMaxLimit = 80;

	// Token: 0x040000B2 RID: 178
	public int zoomRate = 40;

	// Token: 0x040000B3 RID: 179
	public float panSpeed = 0.3f;

	// Token: 0x040000B4 RID: 180
	public float zoomDampening = 5f;

	// Token: 0x040000B5 RID: 181
	public float autoRotate = 1f;

	// Token: 0x040000B6 RID: 182
	public float autoRotateSpeed = 0.1f;

	// Token: 0x040000B7 RID: 183
	private float xDeg;

	// Token: 0x040000B8 RID: 184
	private float yDeg;

	// Token: 0x040000B9 RID: 185
	private float currentDistance;

	// Token: 0x040000BA RID: 186
	private float desiredDistance;

	// Token: 0x040000BB RID: 187
	private Quaternion currentRotation;

	// Token: 0x040000BC RID: 188
	private Quaternion desiredRotation;

	// Token: 0x040000BD RID: 189
	private Quaternion rotation;

	// Token: 0x040000BE RID: 190
	private Vector3 position;

	// Token: 0x040000BF RID: 191
	private float idleTimer;

	// Token: 0x040000C0 RID: 192
	private float idleSmooth;
}
