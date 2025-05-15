using System;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class PunchZone : MonoBehaviour
{
	// Token: 0x0600144C RID: 5196 RVA: 0x000A4A41 File Offset: 0x000A2C41
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x000A4A50 File Offset: 0x000A2C50
	private void OnTriggerStay(Collider other)
	{
		if (this.active)
		{
			if (LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment))
			{
				this.aud.Play();
				this.active = false;
				return;
			}
			if (other.gameObject.CompareTag("Enemy"))
			{
				this.active = false;
				other.gameObject.GetComponent<EnemyIdentifierIdentifier>().eid.DeliverDamage(other.gameObject, (base.transform.position - other.transform.position).normalized * 10000f, other.transform.position, 1f, false, 1f, null, false, false);
			}
		}
	}

	// Token: 0x04001BDF RID: 7135
	public bool active;

	// Token: 0x04001BE0 RID: 7136
	private AudioSource aud;
}
