using System;
using UnityEngine;

// Token: 0x020000F8 RID: 248
public class CustomGroundProperties : MonoBehaviour
{
	// Token: 0x04000683 RID: 1667
	public float friction = 1f;

	// Token: 0x04000684 RID: 1668
	public float speedMultiplier = 1f;

	// Token: 0x04000685 RID: 1669
	public bool push;

	// Token: 0x04000686 RID: 1670
	public Vector3 pushForce;

	// Token: 0x04000687 RID: 1671
	public bool pushDirectionRelative;

	// Token: 0x04000688 RID: 1672
	public bool canJump = true;

	// Token: 0x04000689 RID: 1673
	public bool silentJumpFail;

	// Token: 0x0400068A RID: 1674
	public float jumpForceMultiplier = 1f;

	// Token: 0x0400068B RID: 1675
	public bool canWallJump = true;

	// Token: 0x0400068C RID: 1676
	public bool canSlide = true;

	// Token: 0x0400068D RID: 1677
	public bool silentSlideFail;

	// Token: 0x0400068E RID: 1678
	public bool canDash = true;

	// Token: 0x0400068F RID: 1679
	public bool silentDashFail;

	// Token: 0x04000690 RID: 1680
	public bool canClimbStep = true;

	// Token: 0x04000691 RID: 1681
	public bool launchable = true;

	// Token: 0x04000692 RID: 1682
	public bool forceCrouch;

	// Token: 0x04000693 RID: 1683
	public bool overrideFootsteps;

	// Token: 0x04000694 RID: 1684
	public AudioClip newFootstepSound;

	// Token: 0x04000695 RID: 1685
	public bool overrideSurfaceType;

	// Token: 0x04000696 RID: 1686
	public SurfaceType surfaceType;

	// Token: 0x04000697 RID: 1687
	public Color particleColor = Color.white;

	// Token: 0x04000698 RID: 1688
	public bool dontRotateCamera;
}
