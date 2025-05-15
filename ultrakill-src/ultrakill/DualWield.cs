using System;
using UnityEngine;

// Token: 0x0200012D RID: 301
public class DualWield : MonoBehaviour
{
	// Token: 0x060005A7 RID: 1447 RVA: 0x00027771 File Offset: 0x00025971
	private void Awake()
	{
		this.gc = MonoSingleton<GunControl>.Instance;
		this.meter = MonoSingleton<PowerUpMeter>.Instance;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0002778C File Offset: 0x0002598C
	private void Start()
	{
		this.defaultPosition = base.transform.localPosition;
		if (this.juiceAmount == 0f)
		{
			this.juiceAmount = 30f;
		}
		if (this.meter.juice < this.juiceAmount)
		{
			this.meter.latestMaxJuice = this.juiceAmount;
			this.meter.juice = this.juiceAmount;
		}
		this.meter.powerUpColor = new Color(1f, 0.6f, 0f);
		this.juiceGiven = true;
		MonoSingleton<FistControl>.Instance.forceNoHold++;
		this.gc.dualWieldCount++;
		if (this.gc.currentWeapon)
		{
			WeaponPos componentInChildren = this.gc.currentWeapon.GetComponentInChildren<WeaponPos>();
			if (componentInChildren)
			{
				componentInChildren.CheckPosition();
			}
			this.UpdateWeapon(this.gc.currentWeapon);
		}
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x00027884 File Offset: 0x00025A84
	private void OnEnable()
	{
		this.gc.OnWeaponChange += this.UpdateWeapon;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0002789D File Offset: 0x00025A9D
	private void OnDisable()
	{
		this.gc.OnWeaponChange -= this.UpdateWeapon;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x000278B6 File Offset: 0x00025AB6
	private void Update()
	{
		if (this.juiceGiven && this.meter.juice <= 0f)
		{
			this.EndPowerUp();
			return;
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000278DC File Offset: 0x00025ADC
	private void UpdateWeapon(GameObject newObject)
	{
		if (this.currentWeapon)
		{
			Object.Destroy(this.currentWeapon);
		}
		WeaponIdentifier weaponIdentifier;
		if (this.gc.currentWeapon && this.gc.currentWeapon.TryGetComponent<WeaponIdentifier>(out weaponIdentifier))
		{
			this.copyTarget = this.gc.currentWeapon;
			this.currentWeapon = Object.Instantiate<GameObject>(this.gc.currentWeapon, base.transform);
			if (this.currentWeapon.TryGetComponent<WeaponIdentifier>(out weaponIdentifier))
			{
				weaponIdentifier.delay = this.delay;
				weaponIdentifier.duplicate = true;
				base.transform.localPosition = this.defaultPosition + weaponIdentifier.duplicateOffset;
			}
		}
		else
		{
			this.copyTarget = null;
		}
		if (this.copyTarget)
		{
			this.currentWeapon.SetActive(newObject.activeInHierarchy);
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x000279BC File Offset: 0x00025BBC
	public void EndPowerUp()
	{
		if (this.gc.currentWeapon)
		{
			WeaponPos componentInChildren = this.gc.currentWeapon.GetComponentInChildren<WeaponPos>();
			if (componentInChildren)
			{
				componentInChildren.CheckPosition();
			}
		}
		if (MonoSingleton<FistControl>.Instance.forceNoHold > 0)
		{
			MonoSingleton<FistControl>.Instance.forceNoHold--;
		}
		this.gc.dualWieldCount--;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040007D7 RID: 2007
	private GunControl gc;

	// Token: 0x040007D8 RID: 2008
	private PowerUpMeter meter;

	// Token: 0x040007D9 RID: 2009
	public float juiceAmount;

	// Token: 0x040007DA RID: 2010
	private bool juiceGiven;

	// Token: 0x040007DB RID: 2011
	private GameObject copyTarget;

	// Token: 0x040007DC RID: 2012
	private GameObject currentWeapon;

	// Token: 0x040007DD RID: 2013
	public float delay;

	// Token: 0x040007DE RID: 2014
	private Vector3 defaultPosition;
}
