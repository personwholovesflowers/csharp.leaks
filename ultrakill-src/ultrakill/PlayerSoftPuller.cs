using System;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class PlayerSoftPuller : MonoBehaviour
{
	// Token: 0x06001369 RID: 4969 RVA: 0x0009BE67 File Offset: 0x0009A067
	private void OnDisable()
	{
		this.playerIsIn = 0;
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x0009BE70 File Offset: 0x0009A070
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.playerIsIn++;
		}
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x0009BE97 File Offset: 0x0009A097
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.playerIsIn--;
		}
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x0009BEC0 File Offset: 0x0009A0C0
	private void FixedUpdate()
	{
		if (this.playerIsIn == 0 || MonoSingleton<NewMovement>.Instance.gc.onGround)
		{
			return;
		}
		Vector3 vector = base.transform.position - MonoSingleton<NewMovement>.Instance.transform.position;
		vector -= MonoSingleton<NewMovement>.Instance.rb.velocity / 2f;
		vector = new Vector3(this.useX ? vector.x : 0f, this.useY ? vector.y : 0f, this.useZ ? vector.z : 0f);
		MonoSingleton<NewMovement>.Instance.rb.AddForce(vector * this.pullAmount, ForceMode.VelocityChange);
	}

	// Token: 0x04001ABE RID: 6846
	public float pullAmount = 0.1f;

	// Token: 0x04001ABF RID: 6847
	public bool useX = true;

	// Token: 0x04001AC0 RID: 6848
	public bool useY = true;

	// Token: 0x04001AC1 RID: 6849
	public bool useZ = true;

	// Token: 0x04001AC2 RID: 6850
	private int playerIsIn;
}
