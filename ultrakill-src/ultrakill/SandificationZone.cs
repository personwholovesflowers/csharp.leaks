using System;
using UnityEngine;

// Token: 0x020003BE RID: 958
public class SandificationZone : MonoBehaviour
{
	// Token: 0x060015CF RID: 5583 RVA: 0x000B12B2 File Offset: 0x000AF4B2
	private void Start()
	{
		this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x000B12CC File Offset: 0x000AF4CC
	private void Enter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			if (this.difficulty >= 3)
			{
				if (MonoSingleton<NewMovement>.Instance.hp > 10)
				{
					MonoSingleton<NewMovement>.Instance.ForceAntiHP((float)(100 - MonoSingleton<NewMovement>.Instance.hp + 10), false, false, true, false);
					return;
				}
				MonoSingleton<NewMovement>.Instance.ForceAntiHP(99f, false, false, true, false);
				return;
			}
			else if (this.difficulty == 2)
			{
				if (MonoSingleton<NewMovement>.Instance.hp > 10)
				{
					MonoSingleton<NewMovement>.Instance.ForceAntiHP((float)(Mathf.RoundToInt(MonoSingleton<NewMovement>.Instance.antiHp) + 10), false, false, true, false);
					return;
				}
				MonoSingleton<NewMovement>.Instance.ForceAntiHP(99f, false, false, true, false);
				return;
			}
		}
		else if (other.gameObject.layer == 10 || other.gameObject.layer == 11)
		{
			EnemyIdentifierIdentifier component = other.gameObject.GetComponent<EnemyIdentifierIdentifier>();
			if (component && component.eid && !component.eid.dead)
			{
				component.eid.Sandify(false);
				if (this.buffHealth)
				{
					component.eid.HealthBuff(this.healthBuff);
				}
				if (this.buffDamage)
				{
					component.eid.DamageBuff(this.damageBuff);
				}
				if (this.buffSpeed)
				{
					component.eid.SpeedBuff(this.speedBuff);
				}
				component.eid.UpdateBuffs(false, true);
			}
		}
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x000B1441 File Offset: 0x000AF641
	private void OnTriggerEnter(Collider other)
	{
		this.Enter(other);
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x000B144A File Offset: 0x000AF64A
	private void OnCollisionEnter(Collision collision)
	{
		this.Enter(collision.collider);
	}

	// Token: 0x04001E09 RID: 7689
	private int difficulty;

	// Token: 0x04001E0A RID: 7690
	[HideInInspector]
	public bool buffHealth;

	// Token: 0x04001E0B RID: 7691
	[HideInInspector]
	public float healthBuff = 1f;

	// Token: 0x04001E0C RID: 7692
	[HideInInspector]
	public bool buffDamage;

	// Token: 0x04001E0D RID: 7693
	[HideInInspector]
	public float damageBuff = 1f;

	// Token: 0x04001E0E RID: 7694
	public bool buffSpeed;

	// Token: 0x04001E0F RID: 7695
	public float speedBuff = 1f;
}
