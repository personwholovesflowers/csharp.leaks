using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class ExtendedFlycam : MonoBehaviour
{
	// Token: 0x06000040 RID: 64 RVA: 0x00003EE8 File Offset: 0x000020E8
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003EF8 File Offset: 0x000020F8
	private void Update()
	{
		if (Cursor.lockState != CursorLockMode.None)
		{
			this.rotationX += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
			this.rotationY += Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
		}
		this.rotationY = Mathf.Clamp(this.rotationY, -90f, 90f);
		Quaternion quaternion = Quaternion.AngleAxis(this.rotationX, Vector3.up);
		quaternion *= Quaternion.AngleAxis(this.rotationY, Vector3.left);
		base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, quaternion, Time.deltaTime * 5f);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * this.climbSpeed * this.fastMoveFactor * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * this.climbSpeed * this.fastMoveFactor * Time.deltaTime;
			}
		}
		else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			base.transform.position += base.transform.forward * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * (this.normalMoveSpeed * this.slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * this.climbSpeed * this.slowMoveFactor * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * this.climbSpeed * this.slowMoveFactor * Time.deltaTime;
			}
		}
		else
		{
			base.transform.position += base.transform.forward * this.normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			base.transform.position += base.transform.right * this.normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += Vector3.up * this.climbSpeed * Time.deltaTime;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position -= Vector3.up * this.climbSpeed * Time.deltaTime;
			}
		}
		if (Input.GetKeyDown(KeyCode.End) || Input.GetKeyDown(KeyCode.Escape))
		{
			if (Cursor.lockState == CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	// Token: 0x0400005C RID: 92
	public float cameraSensitivity = 90f;

	// Token: 0x0400005D RID: 93
	public float climbSpeed = 4f;

	// Token: 0x0400005E RID: 94
	public float normalMoveSpeed = 10f;

	// Token: 0x0400005F RID: 95
	public float slowMoveFactor = 0.25f;

	// Token: 0x04000060 RID: 96
	public float fastMoveFactor = 3f;

	// Token: 0x04000061 RID: 97
	private float rotationX;

	// Token: 0x04000062 RID: 98
	private float rotationY;
}
