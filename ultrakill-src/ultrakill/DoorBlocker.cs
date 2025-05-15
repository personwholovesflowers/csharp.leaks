using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class DoorBlocker : MonoBehaviour
{
	// Token: 0x06000549 RID: 1353 RVA: 0x00023C1C File Offset: 0x00021E1C
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Door"))
		{
			Door componentInParent = collision.gameObject.GetComponentInParent<Door>();
			if (componentInParent != null)
			{
				this.blockedDoor = componentInParent;
			}
			if (this.aud == null)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			if (this.aud != null)
			{
				this.aud.Play();
			}
			if (componentInParent != null)
			{
				componentInParent.Close(false);
			}
		}
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00023C9C File Offset: 0x00021E9C
	private void OnDestroy()
	{
		if (this.blockedDoor != null && this.blockedDoor.gameObject.activeInHierarchy && base.gameObject.scene.isLoaded)
		{
			this.blockedDoor.Open(false, true);
		}
	}

	// Token: 0x0400075B RID: 1883
	private AudioSource aud;

	// Token: 0x0400075C RID: 1884
	private Door blockedDoor;
}
