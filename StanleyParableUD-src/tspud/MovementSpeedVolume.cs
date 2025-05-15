using System;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class MovementSpeedVolume : MonoBehaviour
{
	// Token: 0x06000960 RID: 2400 RVA: 0x0002C308 File Offset: 0x0002A508
	private void Awake()
	{
		this.position = base.transform.position;
		this.halfExtents = new Vector3(base.transform.localScale.x, base.transform.localScale.y, base.transform.localScale.z);
		this.halfExtents *= 0.5f;
		this.orientation = base.transform.rotation;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0002C388 File Offset: 0x0002A588
	private void Update()
	{
		if (Physics.OverlapBox(this.position, this.halfExtents, this.orientation, this.layerMask).Length != 0)
		{
			if (!this.touchingLastFrame)
			{
				StanleyController.Instance.SetMovementSpeedMultiplier(this.movementSpeedMultiplier);
				StanleyController.Instance.WalkingSpeedAffectsFootstepSoundSpeed = this.walkingSpeedShouldAffectFootstepSpeed;
			}
			this.touchingLastFrame = true;
			return;
		}
		if (this.touchingLastFrame && this.exitColliderMode == MovementSpeedVolume.ExitColliderMode.ResetToOneOnExit)
		{
			StanleyController.Instance.SetMovementSpeedMultiplier(1f);
		}
		this.touchingLastFrame = false;
	}

	// Token: 0x0400093C RID: 2364
	public float movementSpeedMultiplier = 1f;

	// Token: 0x0400093D RID: 2365
	public bool walkingSpeedShouldAffectFootstepSpeed;

	// Token: 0x0400093E RID: 2366
	public MovementSpeedVolume.ExitColliderMode exitColliderMode;

	// Token: 0x0400093F RID: 2367
	private Vector3 halfExtents;

	// Token: 0x04000940 RID: 2368
	private Vector3 position;

	// Token: 0x04000941 RID: 2369
	private Quaternion orientation;

	// Token: 0x04000942 RID: 2370
	private LayerMask layerMask = 512;

	// Token: 0x04000943 RID: 2371
	private bool touchingLastFrame;

	// Token: 0x04000944 RID: 2372
	private bool isEnabled = true;

	// Token: 0x020003EE RID: 1006
	public enum ExitColliderMode
	{
		// Token: 0x04001477 RID: 5239
		ResetToOneOnExit,
		// Token: 0x04001478 RID: 5240
		DoesNotResetOnExit
	}
}
