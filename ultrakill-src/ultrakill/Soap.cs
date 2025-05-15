using System;
using UnityEngine;

// Token: 0x02000414 RID: 1044
public class Soap : MonoBehaviour
{
	// Token: 0x060017BC RID: 6076 RVA: 0x000C2A69 File Offset: 0x000C0C69
	private void Start()
	{
		this.itid = base.GetComponent<ItemIdentifier>();
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x000C2A83 File Offset: 0x000C0C83
	private void FixedUpdate()
	{
		if (this.rb)
		{
			this.velocityBeforeCollision = this.rb.velocity;
		}
	}

	// Token: 0x060017BE RID: 6078 RVA: 0x000C2AA4 File Offset: 0x000C0CA4
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.itid.pickedUp && this.velocityBeforeCollision.magnitude > 15f)
		{
			EnemyIdentifierIdentifier enemyIdentifierIdentifier;
			if ((collision.gameObject.layer == 11 || collision.gameObject.layer == 10) && collision.gameObject.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
			{
				if (enemyIdentifierIdentifier.eid)
				{
					enemyIdentifierIdentifier.eid.DeliverDamage(collision.gameObject, Vector3.zero, collision.GetContact(0).point, 999999f, true, 0f, null, false, false);
				}
				this.rb.velocity = Vector3.zero;
				return;
			}
			Breakable breakable;
			if (collision.gameObject.TryGetComponent<Breakable>(out breakable) && !breakable.specialCaseOnly)
			{
				breakable.Break();
				this.rb.velocity = Vector3.zero;
				return;
			}
			Bleeder bleeder;
			if (collision.gameObject.TryGetComponent<Bleeder>(out bleeder))
			{
				bleeder.GetHit(base.transform.position, GoreType.Head, false);
			}
		}
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x000C2BA4 File Offset: 0x000C0DA4
	public void HitWith(GameObject target)
	{
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		if (target.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier))
		{
			enemyIdentifierIdentifier.eid.DeliverDamage(target, Vector3.zero, target.transform.position, 999999f, true, 0f, null, false, false);
		}
	}

	// Token: 0x04002134 RID: 8500
	private ItemIdentifier itid;

	// Token: 0x04002135 RID: 8501
	private Rigidbody rb;

	// Token: 0x04002136 RID: 8502
	private Vector3 velocityBeforeCollision;
}
