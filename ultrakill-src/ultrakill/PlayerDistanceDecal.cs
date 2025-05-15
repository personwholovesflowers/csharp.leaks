using System;
using UnityEngine;

// Token: 0x02000344 RID: 836
public class PlayerDistanceDecal : MonoBehaviour
{
	// Token: 0x06001340 RID: 4928 RVA: 0x0009B3A7 File Offset: 0x000995A7
	private void OnDisable()
	{
		if (this.currentDecal != null)
		{
			Object.Destroy(this.currentDecal);
		}
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x0009B3C4 File Offset: 0x000995C4
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			if (this.col == null)
			{
				this.col = base.GetComponent<Collider>();
			}
			if (this.camObj == null)
			{
				this.camObj = MonoSingleton<CameraController>.Instance.gameObject;
			}
			if (this.currentDecal == null)
			{
				this.currentDecal = Object.Instantiate<GameObject>(this.decal, base.transform.position, Quaternion.identity);
			}
			this.currentDecal.transform.position = this.col.ClosestPointOnBounds(this.camObj.transform.position);
			this.currentDecal.transform.LookAt(this.camObj.transform.position);
			this.currentDecal.transform.position += this.currentDecal.transform.forward * 0.1f;
		}
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0009B3A7 File Offset: 0x000995A7
	private void OnCollisionExit(Collision collision)
	{
		if (this.currentDecal != null)
		{
			Object.Destroy(this.currentDecal);
		}
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x0009B4D0 File Offset: 0x000996D0
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (this.col == null)
			{
				this.col = base.GetComponent<Collider>();
			}
			if (this.camObj == null)
			{
				this.camObj = MonoSingleton<CameraController>.Instance.gameObject;
			}
			if (this.currentDecal == null)
			{
				this.currentDecal = Object.Instantiate<GameObject>(this.decal, base.transform.position, Quaternion.identity);
			}
			this.currentDecal.transform.position = this.col.ClosestPointOnBounds(this.camObj.transform.position);
			this.currentDecal.transform.LookAt(this.camObj.transform.position);
			this.currentDecal.transform.position += this.currentDecal.transform.forward * 0.1f;
		}
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x0009B3A7 File Offset: 0x000995A7
	private void OnTriggerExit(Collider other)
	{
		if (this.currentDecal != null)
		{
			Object.Destroy(this.currentDecal);
		}
	}

	// Token: 0x04001A98 RID: 6808
	public GameObject decal;

	// Token: 0x04001A99 RID: 6809
	private GameObject currentDecal;

	// Token: 0x04001A9A RID: 6810
	private Collider col;

	// Token: 0x04001A9B RID: 6811
	private GameObject camObj;
}
