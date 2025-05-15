using System;
using UnityEngine;

// Token: 0x0200017C RID: 380
public class StudicControls : MonoBehaviour
{
	// Token: 0x060008E0 RID: 2272 RVA: 0x0002A3D0 File Offset: 0x000285D0
	private void Update()
	{
		base.transform.position += base.transform.right * this.speed * Time.deltaTime * Input.GetAxis("Horizontal") + base.transform.forward * this.speed * Time.deltaTime * Input.GetAxis("Vertical") + Vector3.up * this.speed * Time.deltaTime * (float)(Input.GetKey(KeyCode.Space) ? (Input.GetKey(KeyCode.LeftShift) ? (-1) : 1) : 0);
		base.transform.eulerAngles = new Vector3(Input.mousePosition.y / this.mouseScale, Input.mousePosition.x / this.mouseScale);
	}

	// Token: 0x040008AB RID: 2219
	public float speed = 10f;

	// Token: 0x040008AC RID: 2220
	public float mouseScale = 32f;
}
