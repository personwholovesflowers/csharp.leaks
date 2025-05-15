using System;
using UnityEngine;

// Token: 0x020000F0 RID: 240
public class CrateCoin : MonoBehaviour
{
	// Token: 0x060004AF RID: 1199 RVA: 0x0001FF78 File Offset: 0x0001E178
	private void Start()
	{
		this.startDirection = Random.insideUnitSphere;
		this.speed = (float)Random.Range(20, 35);
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0001FF95 File Offset: 0x0001E195
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.caught = true;
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001FFB0 File Offset: 0x0001E1B0
	private void Update()
	{
		if (this.caught)
		{
			this.speed += Time.deltaTime * 25f;
			base.transform.position = Vector3.MoveTowards(base.transform.position, MonoSingleton<PlayerTracker>.Instance.GetPlayer().position, Time.deltaTime * this.speed);
			if (Vector3.Distance(base.transform.position, MonoSingleton<PlayerTracker>.Instance.GetPlayer().position) < 1f)
			{
				this.caught = false;
				CrateCounter instance = MonoSingleton<CrateCounter>.Instance;
				if (instance != null)
				{
					instance.AddCoin();
				}
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
				{
					Object.Instantiate<GameObject>(this.getEffect, base.transform.position, Quaternion.identity);
				}
				else
				{
					MonoSingleton<PlatformerMovement>.Instance.CoinGet();
				}
				Object.Destroy(base.gameObject);
				return;
			}
		}
		else
		{
			if (this.speed > 0f)
			{
				this.speed = Mathf.MoveTowards(this.speed, 0f, Time.deltaTime * 50f);
			}
			base.transform.position += this.startDirection * this.speed * Time.deltaTime;
		}
	}

	// Token: 0x04000659 RID: 1625
	[SerializeField]
	private GameObject getEffect;

	// Token: 0x0400065A RID: 1626
	private bool caught;

	// Token: 0x0400065B RID: 1627
	private float speed = 25f;

	// Token: 0x0400065C RID: 1628
	private Vector3 startDirection;
}
