using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
public class BulletCheck : MonoBehaviour
{
	// Token: 0x060002F7 RID: 759 RVA: 0x00011688 File Offset: 0x0000F888
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		CheckerType checkerType = this.type;
		if (checkerType == CheckerType.Streetcleaner)
		{
			this.sc = base.GetComponentInParent<Streetcleaner>();
			return;
		}
		if (checkerType != CheckerType.V2)
		{
			return;
		}
		this.v2 = base.GetComponentInParent<V2>();
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x000116E0 File Offset: 0x0000F8E0
	private void OnTriggerEnter(Collider other)
	{
		CheckerType checkerType = this.type;
		if (checkerType != CheckerType.Streetcleaner)
		{
			if (checkerType != CheckerType.V2)
			{
				return;
			}
			if (other.gameObject.layer == 14)
			{
				Projectile component = other.GetComponent<Projectile>();
				if (this.v2 == null)
				{
					this.v2 = base.GetComponentInParent<V2>();
				}
				if (component == null || component.safeEnemyType != EnemyType.V2 || this.difficulty > 2)
				{
					V2 v = this.v2;
					if (v == null)
					{
						return;
					}
					v.Dodge(other.transform);
				}
			}
		}
		else if (other.gameObject.layer == 14)
		{
			Grenade component2 = other.GetComponent<Grenade>();
			if (component2 != null)
			{
				component2.enemy = true;
				component2.CanCollideWithPlayer(true);
				Streetcleaner streetcleaner = this.sc;
				if (streetcleaner != null)
				{
					streetcleaner.DeflectShot();
				}
				Rigidbody component3 = other.GetComponent<Rigidbody>();
				float magnitude = component3.velocity.magnitude;
				component3.velocity = base.transform.right * magnitude;
				other.transform.forward = base.transform.right;
				this.aud.Play();
				return;
			}
			Streetcleaner streetcleaner2 = this.sc;
			if (streetcleaner2 == null)
			{
				return;
			}
			streetcleaner2.Dodge();
			return;
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00011804 File Offset: 0x0000FA04
	public void ForceDodge()
	{
		CheckerType checkerType = this.type;
		if (checkerType != CheckerType.Streetcleaner)
		{
			if (checkerType != CheckerType.V2)
			{
				return;
			}
			V2 v = this.v2;
			if (v == null)
			{
				return;
			}
			v.Dodge(base.transform);
			return;
		}
		else
		{
			Streetcleaner streetcleaner = this.sc;
			if (streetcleaner == null)
			{
				return;
			}
			streetcleaner.Dodge();
			return;
		}
	}

	// Token: 0x04000399 RID: 921
	public CheckerType type;

	// Token: 0x0400039A RID: 922
	private Streetcleaner sc;

	// Token: 0x0400039B RID: 923
	private V2 v2;

	// Token: 0x0400039C RID: 924
	private AudioSource aud;

	// Token: 0x0400039D RID: 925
	private int difficulty;
}
