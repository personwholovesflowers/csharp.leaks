using System;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class FireZone : MonoBehaviour
{
	// Token: 0x0600098C RID: 2444 RVA: 0x00042582 File Offset: 0x00040782
	private void Start()
	{
		if (this.HurtCooldownCollection == null)
		{
			this.HurtCooldownCollection = new HurtCooldownCollection();
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00042598 File Offset: 0x00040798
	private void OnTriggerStay(Collider other)
	{
		if (this.source == FlameSource.None)
		{
			return;
		}
		float num = 1f;
		bool flag = true;
		bool flag2 = false;
		bool flag3 = true;
		FlameSource flameSource = this.source;
		if (flameSource != FlameSource.Streetcleaner)
		{
			if (flameSource == FlameSource.Napalm)
			{
				flag2 = true;
			}
		}
		else
		{
			if (this.sc == null)
			{
				this.sc = base.GetComponentInParent<Streetcleaner>();
			}
			if (!this.sc.damaging || this.sc.eid.target == null)
			{
				return;
			}
			flag = this.sc.eid.target.isPlayer;
			flag2 = !flag;
			num = this.sc.eid.totalDamageModifier;
		}
		EnemyIdentifierIdentifier enemyIdentifierIdentifier;
		Flammable flammable;
		if (other.gameObject.CompareTag("Player"))
		{
			if (!this.canHurtPlayer)
			{
				return;
			}
			if (flag && this.HurtCooldownCollection.TryHurtCheckPlayer(true))
			{
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
				{
					MonoSingleton<PlatformerMovement>.Instance.Burn(false);
					return;
				}
				MonoSingleton<NewMovement>.Instance.GetHurt((int)((float)this.playerDamage * num), true, (this.source == FlameSource.Napalm) ? 0f : 0.35f, false, false, (this.source == FlameSource.Napalm) ? 0f : 0.35f, false);
				return;
			}
		}
		else if (other.TryGetComponent<EnemyIdentifierIdentifier>(out enemyIdentifierIdentifier) && enemyIdentifierIdentifier != null && enemyIdentifierIdentifier.eid != null)
		{
			if (!flag2)
			{
				return;
			}
			EnemyIdentifier eid = enemyIdentifierIdentifier.eid;
			if (this.HurtCooldownCollection.TryHurtCheckEnemy(eid, true))
			{
				eid.hitter = "fire";
				eid.DeliverDamage(other.gameObject, Vector3.zero, Vector3.zero, 1f, false, 0f, null, false, false);
				return;
			}
		}
		else if (other.TryGetComponent<Flammable>(out flammable))
		{
			if (!flag3)
			{
				return;
			}
			if (this.HurtCooldownCollection.TryHurtCheckFlammable(flammable, true))
			{
				flammable.Burn(10f, false);
			}
		}
	}

	// Token: 0x04000C6F RID: 3183
	public HurtCooldownCollection HurtCooldownCollection;

	// Token: 0x04000C70 RID: 3184
	public FlameSource source;

	// Token: 0x04000C71 RID: 3185
	public bool canHurtPlayer = true;

	// Token: 0x04000C72 RID: 3186
	public int playerDamage = 20;

	// Token: 0x04000C73 RID: 3187
	private Streetcleaner sc;
}
