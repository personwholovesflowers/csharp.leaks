using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200032B RID: 811
public class OutOfBounds : MonoBehaviour
{
	// Token: 0x060012CB RID: 4811 RVA: 0x00095BDC File Offset: 0x00093DDC
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && this.targets != AffectedSubjects.EnemiesOnly)
		{
			if ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && MonoSingleton<NewMovement>.Instance.hp > 0) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && !MonoSingleton<PlatformerMovement>.Instance.dead))
			{
				if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
				{
					MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.zero;
				}
				else
				{
					MonoSingleton<PlatformerMovement>.Instance.rb.velocity = Vector3.zero;
				}
				if (this.sman == null)
				{
					this.sman = MonoSingleton<StatsManager>.Instance;
				}
				if (MonoSingleton<NewMovement>.Instance.ridingRocket)
				{
					MonoSingleton<NewMovement>.Instance.ridingRocket.PlayerRideEnd();
				}
				MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Whoops, sorry about that.", "", "", 0, false, false, true);
				if (this.overrideResetPosition != Vector3.zero)
				{
					other.transform.position = this.overrideResetPosition + Vector3.up * 1.25f;
				}
				else if (this.sman.currentCheckPoint != null)
				{
					other.transform.position = this.sman.currentCheckPoint.transform.position + Vector3.up * 1.25f;
					if (this.sman.currentCheckPoint.toActivate != null)
					{
						this.sman.currentCheckPoint.toActivate.SetActive(true);
					}
					Door[] doorsToUnlock = this.sman.currentCheckPoint.doorsToUnlock;
					for (int i = 0; i < doorsToUnlock.Length; i++)
					{
						doorsToUnlock[i].Unlock();
					}
				}
				else
				{
					other.transform.position = this.sman.spawnPos;
					foreach (GameObject gameObject in this.toActivate)
					{
						if (gameObject != null)
						{
							gameObject.SetActive(true);
						}
					}
					foreach (GameObject gameObject2 in this.toDisactivate)
					{
						if (gameObject2 != null)
						{
							gameObject2.SetActive(false);
						}
					}
					foreach (Door door in this.toUnlock)
					{
						if (door != null)
						{
							door.Unlock();
						}
					}
				}
				UnityEvent unityEvent = this.toEvent;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
				return;
			}
		}
		else if (this.targets != AffectedSubjects.PlayerOnly)
		{
			if (other.gameObject.layer == 10 || other.gameObject.layer == 9)
			{
				if (other.gameObject.layer == 10)
				{
					EnemyIdentifier componentInParent = other.gameObject.GetComponentInParent<EnemyIdentifier>();
					if (componentInParent && !componentInParent.dead)
					{
						return;
					}
					if (!componentInParent && other.gameObject.CompareTag("Coin"))
					{
						Coin component = other.GetComponent<Coin>();
						if (component == null)
						{
							return;
						}
						component.GetDeleted();
						return;
					}
				}
				other.gameObject.SetActive(false);
				other.transform.position = Vector3.zero;
				other.transform.localScale = Vector3.zero;
				return;
			}
			if (other.gameObject.CompareTag("Enemy") && base.GetComponentInChildren<DeathZone>() == null)
			{
				EnemyIdentifier component2 = other.gameObject.GetComponent<EnemyIdentifier>();
				if (!component2.dead)
				{
					if (component2.specialOob)
					{
						component2.SendMessage("OutOfBounds", SendMessageOptions.DontRequireReceiver);
						return;
					}
					component2.InstaKill();
				}
			}
		}
	}

	// Token: 0x040019BB RID: 6587
	public AffectedSubjects targets;

	// Token: 0x040019BC RID: 6588
	private StatsManager sman;

	// Token: 0x040019BD RID: 6589
	public Vector3 overrideResetPosition;

	// Token: 0x040019BE RID: 6590
	public GameObject[] toActivate;

	// Token: 0x040019BF RID: 6591
	public GameObject[] toDisactivate;

	// Token: 0x040019C0 RID: 6592
	public Door[] toUnlock;

	// Token: 0x040019C1 RID: 6593
	public UnityEvent toEvent;
}
