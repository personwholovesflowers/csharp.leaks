using System;
using UnityEngine;

// Token: 0x02000122 RID: 290
public class DoorOpener : MonoBehaviour
{
	// Token: 0x0600055C RID: 1372 RVA: 0x000243A5 File Offset: 0x000225A5
	private void Awake()
	{
		this.colliderless = base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000243CA File Offset: 0x000225CA
	private void OnEnable()
	{
		if (this.colliderless)
		{
			this.Open();
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000243DA File Offset: 0x000225DA
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.Open();
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x000243F4 File Offset: 0x000225F4
	private void Open()
	{
		this.door.Open(false, true);
		if (this.oneTime)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0400076C RID: 1900
	public Door door;

	// Token: 0x0400076D RID: 1901
	public bool oneTime;

	// Token: 0x0400076E RID: 1902
	private bool colliderless;
}
