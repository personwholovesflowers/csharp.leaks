using System;

// Token: 0x0200013F RID: 319
public class MovementSpeedHammerEntity : HammerEntity
{
	// Token: 0x06000778 RID: 1912 RVA: 0x00026580 File Offset: 0x00024780
	public void Input_SetMovementSpeed()
	{
		StanleyController.Instance.SetMovementSpeedMultiplier(this.movementSpeedMultiplier);
		StanleyController.Instance.WalkingSpeedAffectsFootstepSoundSpeed = this.walkingSpeedShouldAffectFootstepSpeed;
	}

	// Token: 0x0400079D RID: 1949
	public float movementSpeedMultiplier = 1f;

	// Token: 0x0400079E RID: 1950
	public bool walkingSpeedShouldAffectFootstepSpeed;
}
