using System;
using UnityEngine;

// Token: 0x0200032C RID: 812
public class OutOfBoundsTargetSetter : MonoBehaviour
{
	// Token: 0x060012CD RID: 4813 RVA: 0x00095F48 File Offset: 0x00094148
	private void Start()
	{
		Collider collider;
		Rigidbody rigidbody;
		if (!base.TryGetComponent<Collider>(out collider) && !base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			this.Activate();
		}
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x00095F6F File Offset: 0x0009416F
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.Activate();
		}
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x00095F8C File Offset: 0x0009418C
	public void Activate()
	{
		bool flag = false;
		if (this.deathZones == null || this.deathZones.Length == 0)
		{
			this.deathZones = Object.FindObjectsOfType<DeathZone>();
		}
		else
		{
			flag = true;
		}
		foreach (DeathZone deathZone in this.deathZones)
		{
			if (deathZone && (!deathZone.dontChangeRespawnTarget || flag))
			{
				deathZone.respawnTarget = base.transform.position;
			}
		}
		if (this.oobs == null || this.oobs.Length == 0)
		{
			this.oobs = Object.FindObjectsOfType<OutOfBounds>();
		}
		OutOfBounds[] array2 = this.oobs;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].overrideResetPosition = base.transform.position;
		}
	}

	// Token: 0x040019C2 RID: 6594
	public DeathZone[] deathZones;

	// Token: 0x040019C3 RID: 6595
	public OutOfBounds[] oobs;
}
