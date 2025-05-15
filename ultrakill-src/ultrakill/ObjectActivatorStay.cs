using System;
using UnityEngine;

// Token: 0x0200031C RID: 796
public class ObjectActivatorStay : MonoBehaviour
{
	// Token: 0x0600125D RID: 4701 RVA: 0x00093A14 File Offset: 0x00091C14
	private void OnTriggerEnter(Collider other)
	{
		if ((!this.forEnemies && !this.activated && other.gameObject.CompareTag("Player")) || (this.forEnemies && !this.activated && other.gameObject.CompareTag("Enemy")))
		{
			if (this.oneTime)
			{
				this.activated = true;
			}
			base.Invoke("Activate", this.delay);
		}
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x00093A88 File Offset: 0x00091C88
	private void OnTriggerStay(Collider other)
	{
		if (!this.oneTime && ((!this.forEnemies && !this.activated && other.gameObject.CompareTag("Player")) || (this.forEnemies && !this.activated && other.gameObject.CompareTag("Enemy"))) && ((this.toActivate.Length != 0 && !this.toActivate[0].activeSelf) || (this.toDisActivate.Length != 0 && this.toDisActivate[0].activeSelf)))
		{
			this.Activate();
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x00093B18 File Offset: 0x00091D18
	private void OnTriggerExit(Collider other)
	{
		if (this.disableOnExit)
		{
			foreach (GameObject gameObject in this.toDisActivate)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(true);
				}
			}
			foreach (GameObject gameObject2 in this.toActivate)
			{
				if (gameObject2 != null)
				{
					gameObject2.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x00093B80 File Offset: 0x00091D80
	private void Activate()
	{
		foreach (GameObject gameObject in this.toDisActivate)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
		foreach (GameObject gameObject2 in this.toActivate)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
			}
		}
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x00093BDF File Offset: 0x00091DDF
	private void OnDisable()
	{
		base.CancelInvoke("Activate");
	}

	// Token: 0x0400195A RID: 6490
	public bool oneTime;

	// Token: 0x0400195B RID: 6491
	public bool skippable;

	// Token: 0x0400195C RID: 6492
	public bool disableOnExit;

	// Token: 0x0400195D RID: 6493
	private bool activated;

	// Token: 0x0400195E RID: 6494
	public float delay;

	// Token: 0x0400195F RID: 6495
	public GameObject[] toActivate;

	// Token: 0x04001960 RID: 6496
	public GameObject[] toDisActivate;

	// Token: 0x04001961 RID: 6497
	public bool forEnemies;
}
