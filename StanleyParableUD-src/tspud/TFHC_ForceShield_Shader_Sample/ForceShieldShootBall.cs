using System;
using UnityEngine;

namespace TFHC_ForceShield_Shader_Sample
{
	// Token: 0x020002FF RID: 767
	public class ForceShieldShootBall : MonoBehaviour
	{
		// Token: 0x060013E7 RID: 5095 RVA: 0x00069390 File Offset: 0x00067590
		private void Update()
		{
			if (Input.GetButtonDown("Fire1"))
			{
				Vector3 vector = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.distance);
				vector = Camera.main.ScreenToWorldPoint(vector);
				Rigidbody rigidbody = Object.Instantiate<Rigidbody>(this.bullet, base.transform.position, Quaternion.identity);
				rigidbody.transform.LookAt(vector);
				rigidbody.AddForce(rigidbody.transform.forward * this.speed);
			}
		}

		// Token: 0x04000F8E RID: 3982
		public Rigidbody bullet;

		// Token: 0x04000F8F RID: 3983
		public Transform origshoot;

		// Token: 0x04000F90 RID: 3984
		public float speed = 1000f;

		// Token: 0x04000F91 RID: 3985
		private float distance = 10f;
	}
}
