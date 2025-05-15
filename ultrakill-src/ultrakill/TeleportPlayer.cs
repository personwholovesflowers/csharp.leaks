using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class TeleportPlayer : MonoBehaviour
{
	// Token: 0x06001A2A RID: 6698 RVA: 0x000D7E18 File Offset: 0x000D6018
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.PerformTheTeleport(other.transform);
		}
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000D7E38 File Offset: 0x000D6038
	public void PerformTheTeleport()
	{
		if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			this.PerformTheTeleport(MonoSingleton<NewMovement>.Instance.transform);
			return;
		}
		this.PerformTheTeleport(MonoSingleton<PlatformerMovement>.Instance.transform);
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x000D7E68 File Offset: 0x000D6068
	private void PerformTheTeleport(Transform target)
	{
		if ((MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS && MonoSingleton<NewMovement>.Instance.dead) || (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer && MonoSingleton<PlatformerMovement>.Instance.dead))
		{
			return;
		}
		if (MonoSingleton<NewMovement>.Instance && MonoSingleton<NewMovement>.Instance.ridingRocket)
		{
			MonoSingleton<NewMovement>.Instance.ridingRocket.PlayerRideEnd();
		}
		if (this.affectPosition)
		{
			Vector3 position = target.position;
			Vector3 vector = target.position;
			if (this.dontDetachPlayerFromMovementParent)
			{
				MonoSingleton<PlayerMovementParenting>.Instance.LockMovementParentTeleport(true);
			}
			if (this.notRelative)
			{
				if (this.includeOffsetFromCollider)
				{
					vector = this.objectivePosition - (base.transform.position - target.position);
				}
				else
				{
					vector = this.objectivePosition;
				}
			}
			else if (this.relativeToCollider)
			{
				vector = base.transform.position + this.relativePosition;
			}
			else
			{
				vector = target.position + this.relativePosition;
			}
			target.position = vector;
			if (this.teleportWithPlayer != null && this.teleportWithPlayer.Length != 0)
			{
				for (int i = 0; i < this.teleportWithPlayer.Length; i++)
				{
					if (this.teleportWithPlayer[i] != null)
					{
						this.teleportWithPlayer[i].position += vector - position;
					}
				}
			}
			if (this.dontDetachPlayerFromMovementParent)
			{
				MonoSingleton<PlayerMovementParenting>.Instance.LockMovementParentTeleport(false);
			}
		}
		if (this.affectRotation)
		{
			if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
			{
				if (this.notRelativeRotation)
				{
					MonoSingleton<CameraController>.Instance.rotationY = this.objectiveRotation.y;
					MonoSingleton<CameraController>.Instance.rotationX = this.objectiveRotation.x;
					MonoSingleton<NewMovement>.Instance.dodgeDirection = Quaternion.AngleAxis(this.rotationDelta.y, Vector3.up) * Vector3.forward;
				}
				else
				{
					MonoSingleton<CameraController>.Instance.rotationY += this.rotationDelta.y;
					MonoSingleton<CameraController>.Instance.rotationX += this.rotationDelta.x;
					MonoSingleton<NewMovement>.Instance.dodgeDirection = Quaternion.AngleAxis(this.rotationDelta.y, Vector3.up) * MonoSingleton<NewMovement>.Instance.dodgeDirection;
				}
				MonoSingleton<NewMovement>.Instance.transform.rotation = Quaternion.Euler(0f, MonoSingleton<CameraController>.Instance.rotationY, 0f);
				MonoSingleton<NewMovement>.Instance.rb.rotation = Quaternion.Euler(0f, MonoSingleton<CameraController>.Instance.rotationY, 0f);
				MonoSingleton<CameraController>.Instance.transform.localRotation = Quaternion.Euler(MonoSingleton<CameraController>.Instance.rotationX, 0f, 0f);
			}
			else
			{
				if (this.notRelativeRotation)
				{
					MonoSingleton<PlatformerMovement>.Instance.rotationY = this.objectiveRotation.y;
					MonoSingleton<PlatformerMovement>.Instance.rotationX = this.objectiveRotation.x;
				}
				else
				{
					MonoSingleton<PlatformerMovement>.Instance.rotationY += this.rotationDelta.y;
					MonoSingleton<PlatformerMovement>.Instance.rotationX += this.rotationDelta.x;
				}
				MonoSingleton<PlatformerMovement>.Instance.transform.rotation = Quaternion.Euler(0f, MonoSingleton<CameraController>.Instance.rotationY, 0f);
				MonoSingleton<CameraController>.Instance.transform.localRotation = Quaternion.Euler(MonoSingleton<CameraController>.Instance.rotationX, 0f, 0f);
			}
		}
		if (this.resetPlayerSpeed && MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			MonoSingleton<NewMovement>.Instance.StopMovement();
		}
		if (this.cancelGroundSlam && MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.FPS)
		{
			MonoSingleton<NewMovement>.Instance.gc.heavyFall = false;
		}
		if (this.teleportEffect)
		{
			Object.Instantiate<GameObject>(this.teleportEffect, target.position, Quaternion.identity);
		}
		this.onTeleportPlayer.Invoke("");
	}

	// Token: 0x040024A3 RID: 9379
	public bool affectPosition = true;

	// Token: 0x040024A4 RID: 9380
	public Vector3 relativePosition;

	// Token: 0x040024A5 RID: 9381
	public bool notRelative;

	// Token: 0x040024A6 RID: 9382
	public bool relativeToCollider;

	// Token: 0x040024A7 RID: 9383
	public Vector3 objectivePosition;

	// Token: 0x040024A8 RID: 9384
	public bool includeOffsetFromCollider;

	// Token: 0x040024A9 RID: 9385
	public bool affectRotation;

	// Token: 0x040024AA RID: 9386
	public bool notRelativeRotation;

	// Token: 0x040024AB RID: 9387
	public Vector2 rotationDelta;

	// Token: 0x040024AC RID: 9388
	public Vector2 objectiveRotation;

	// Token: 0x040024AD RID: 9389
	public bool resetPlayerSpeed;

	// Token: 0x040024AE RID: 9390
	public bool cancelGroundSlam;

	// Token: 0x040024AF RID: 9391
	public bool dontDetachPlayerFromMovementParent;

	// Token: 0x040024B0 RID: 9392
	public Transform[] teleportWithPlayer;

	// Token: 0x040024B1 RID: 9393
	public GameObject teleportEffect;

	// Token: 0x040024B2 RID: 9394
	public UltrakillEvent onTeleportPlayer;
}
