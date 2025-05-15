using System;
using Sandbox;
using UnityEngine;

// Token: 0x0200012E RID: 302
public class DualWieldPickup : MonoBehaviour, IAlter, IAlterOptions<float>, IAlterOptions<bool>
{
	// Token: 0x060005AF RID: 1455 RVA: 0x00027A37 File Offset: 0x00025C37
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.PickedUp();
		}
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x00027A54 File Offset: 0x00025C54
	private void PickedUp()
	{
		if (MonoSingleton<GunControl>.Instance)
		{
			Object.Instantiate<GameObject>(this.pickUpEffect, base.transform.position, Quaternion.identity);
			MonoSingleton<CameraController>.Instance.CameraShake(0.35f);
			if (!this.infiniteUses)
			{
				base.gameObject.SetActive(false);
			}
			if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
			{
				MonoSingleton<PlatformerMovement>.Instance.AddExtraHit(3);
				return;
			}
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(MonoSingleton<GunControl>.Instance.transform, true);
			gameObject.transform.localRotation = Quaternion.identity;
			DualWield[] componentsInChildren = MonoSingleton<GunControl>.Instance.GetComponentsInChildren<DualWield>();
			if (componentsInChildren != null && componentsInChildren.Length % 2 == 0)
			{
				gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				gameObject.transform.localScale = Vector3.one;
			}
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				gameObject.transform.localPosition = Vector3.zero;
			}
			else if (componentsInChildren.Length % 2 == 0)
			{
				gameObject.transform.localPosition = new Vector3((float)(componentsInChildren.Length / 2) * -1.5f, 0f, 0f);
			}
			else
			{
				gameObject.transform.localPosition = new Vector3((float)((componentsInChildren.Length + 1) / 2) * 1.5f, 0f, 0f);
			}
			DualWield dualWield = gameObject.AddComponent<DualWield>();
			dualWield.delay = 0.05f;
			dualWield.juiceAmount = this.juiceAmount;
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				dualWield.delay += (float)componentsInChildren.Length / 20f;
			}
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00027BE6 File Offset: 0x00025DE6
	public string alterKey
	{
		get
		{
			return "dual-wield-pickup";
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x060005B2 RID: 1458 RVA: 0x00027BED File Offset: 0x00025DED
	public string alterCategoryName
	{
		get
		{
			return "Dual Wield Pickup";
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00027BF4 File Offset: 0x00025DF4
	AlterOption<float>[] IAlterOptions<float>.options
	{
		get
		{
			return new AlterOption<float>[]
			{
				new AlterOption<float>
				{
					name = "Juice",
					key = "juice",
					value = this.juiceAmount,
					callback = delegate(float value)
					{
						this.juiceAmount = value;
					},
					constraints = new SliderConstraints
					{
						min = 0f,
						max = 100f,
						step = 1f
					}
				}
			};
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x060005B4 RID: 1460 RVA: 0x00027C70 File Offset: 0x00025E70
	AlterOption<bool>[] IAlterOptions<bool>.options
	{
		get
		{
			return new AlterOption<bool>[]
			{
				new AlterOption<bool>
				{
					name = "Infinite Uses",
					key = "infinite-uses",
					value = this.infiniteUses,
					callback = delegate(bool value)
					{
						this.infiniteUses = value;
					}
				}
			};
		}
	}

	// Token: 0x040007DF RID: 2015
	public bool infiniteUses;

	// Token: 0x040007E0 RID: 2016
	public float juiceAmount = 30f;

	// Token: 0x040007E1 RID: 2017
	public GameObject pickUpEffect;
}
