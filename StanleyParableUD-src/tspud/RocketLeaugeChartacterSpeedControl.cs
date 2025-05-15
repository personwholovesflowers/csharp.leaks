using System;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class RocketLeaugeChartacterSpeedControl : MonoBehaviour
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0002A4E7 File Offset: 0x000286E7
	// (set) Token: 0x060008E3 RID: 2275 RVA: 0x0002A4EE File Offset: 0x000286EE
	public static RocketLeaugeChartacterSpeedControl Instance { get; private set; }

	// Token: 0x060008E4 RID: 2276 RVA: 0x0002A4F6 File Offset: 0x000286F6
	private void Awake()
	{
		RocketLeaugeChartacterSpeedControl.Instance = this;
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x060008E5 RID: 2277 RVA: 0x0002A4FE File Offset: 0x000286FE
	// (set) Token: 0x060008E6 RID: 2278 RVA: 0x0002A506 File Offset: 0x00028706
	public float NormalizedSpeed { get; private set; }

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002A510 File Offset: 0x00028710
	private void Update()
	{
		this.forwardMovement = Singleton<GameMaster>.Instance.stanleyActions.Movement.Y;
		this.sideMovement = Singleton<GameMaster>.Instance.stanleyActions.Movement.X;
		if (this.forwardMovement != 0f || this.sideMovement != 0f)
		{
			this.timeMovingForward += Time.deltaTime;
		}
		else
		{
			this.timeMovingForward -= Time.deltaTime * 3f;
		}
		this.timeMovingForward = Mathf.Clamp(this.timeMovingForward, 0f, this.timeToMaxSpeed);
		this.NormalizedSpeed = Mathf.InverseLerp(0f, this.timeToMaxSpeed, this.timeMovingForward);
		this.characterSpeedMultiplier = Mathf.Lerp(1f, this.speedAtMax, this.NormalizedSpeed);
		StanleyController.Instance.SetMovementSpeedMultiplier(this.characterSpeedMultiplier);
		StanleyController.Instance.FieldOfViewAdditiveModifier = this.fovByNormalizedSpeed.Evaluate(this.NormalizedSpeed);
		StanleyController.Instance.WalkingSpeedAffectsFootstepSoundSpeedScale = this.footstepSoundSpeedScale;
	}

	// Token: 0x040008AD RID: 2221
	public float timeToMaxSpeed = 2.5f;

	// Token: 0x040008AE RID: 2222
	public float speedAtMax = 4f;

	// Token: 0x040008AF RID: 2223
	[Range(0f, 1f)]
	public float footstepSoundSpeedScale;

	// Token: 0x040008B0 RID: 2224
	public AnimationCurve fovByNormalizedSpeed;

	// Token: 0x040008B1 RID: 2225
	[Header("DEBUG")]
	public float timeMovingForward;

	// Token: 0x040008B2 RID: 2226
	public float characterSpeedMultiplier;

	// Token: 0x040008B3 RID: 2227
	public float forwardMovement;

	// Token: 0x040008B4 RID: 2228
	public float sideMovement;
}
