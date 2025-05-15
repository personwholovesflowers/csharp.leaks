using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200005B RID: 91
public class AltPickUp : MonoBehaviour
{
	// Token: 0x060001BB RID: 443 RVA: 0x00008F63 File Offset: 0x00007163
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.GotActivated();
		}
	}

	// Token: 0x060001BC RID: 444 RVA: 0x00008F7D File Offset: 0x0000717D
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.GotActivated();
		}
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00008F98 File Offset: 0x00007198
	private void GotActivated()
	{
		GameProgressSaver.AddGear(this.pPref + "alt");
		MonoSingleton<PrefsManager>.Instance.SetInt("weapon." + this.pPref + "0", 2);
		MonoSingleton<GunSetter>.Instance.ResetWeapons(false);
		MonoSingleton<GunSetter>.Instance.ForceWeapon(this.pPref + "0");
		UnityEvent unityEvent = this.onPickUp;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040001D0 RID: 464
	public string pPref;

	// Token: 0x040001D1 RID: 465
	public UnityEvent onPickUp;
}
