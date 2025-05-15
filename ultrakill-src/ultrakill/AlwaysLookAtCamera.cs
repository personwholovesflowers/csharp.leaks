using System;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class AlwaysLookAtCamera : MonoBehaviour
{
	// Token: 0x060001BF RID: 447 RVA: 0x0000901C File Offset: 0x0000721C
	private void Start()
	{
		if (this.eid && this.eid.difficultyOverride >= 0)
		{
			this.difficulty = this.eid.difficultyOverride;
		}
		else
		{
			this.difficulty = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		}
		this.UpdateDifficulty();
		this.EnsureTargetExists();
		this.SlowUpdate();
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x00009080 File Offset: 0x00007280
	private void EnsureTargetExists()
	{
		if (this.target == null || !this.target.isValid || (this.overrideTarget != null && this.target.trackedTransform != this.overrideTarget))
		{
			this.target = ((this.overrideTarget == null) ? EnemyTarget.TrackPlayer() : new EnemyTarget(this.overrideTarget));
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x000090EE File Offset: 0x000072EE
	private void SlowUpdate()
	{
		base.Invoke("SlowUpdate", 0.5f);
		this.EnsureTargetExists();
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x00009108 File Offset: 0x00007308
	private void LateUpdate()
	{
		if (this.target == null || !this.target.isValid)
		{
			return;
		}
		if (this.dontRotateIfBlind && BlindEnemies.Blind)
		{
			return;
		}
		float num = this.speed;
		if (this.eid)
		{
			num *= this.eid.totalSpeedModifier;
		}
		if (this.difficultyVariance)
		{
			num *= this.difficultySpeedMultiplier;
		}
		Transform transform = ((this.preferCameraOverHead && this.target.isPlayer) ? MonoSingleton<CameraController>.Instance.cam.transform : this.target.headTransform);
		if (this.speed == 0f && this.useXAxis && this.useYAxis && this.useZAxis)
		{
			if (this.faceScreenInsteadOfCamera)
			{
				base.transform.rotation = transform.rotation;
				base.transform.Rotate(Vector3.up * 180f, Space.Self);
			}
			else
			{
				base.transform.LookAt(transform);
			}
		}
		else
		{
			Vector3 position = transform.position;
			if (!this.useXAxis)
			{
				position.x = base.transform.position.x;
			}
			if (!this.useYAxis)
			{
				position.y = base.transform.position.y;
			}
			if (!this.useZAxis)
			{
				position.z = base.transform.position.z;
			}
			Quaternion quaternion = Quaternion.LookRotation(position - base.transform.position, Vector3.up);
			if (this.maxAngle != 0f && Quaternion.Angle(base.transform.rotation, quaternion) > this.maxAngle)
			{
				return;
			}
			if (this.speed == 0f)
			{
				base.transform.rotation = quaternion;
			}
			if (this.easeIn)
			{
				float num2 = 1f;
				if (this.difficultyVariance)
				{
					if (this.difficulty == 1)
					{
						num2 = 0.8f;
					}
					else if (this.difficulty == 0)
					{
						num2 = 0.5f;
					}
				}
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * num * (Quaternion.Angle(base.transform.rotation, quaternion) * num2));
			}
			else
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, Time.deltaTime * num);
			}
		}
		if (this.maxXAxisFromParent != 0f)
		{
			base.transform.localRotation = Quaternion.Euler(Mathf.Clamp(base.transform.localRotation.eulerAngles.x, -this.maxXAxisFromParent, this.maxXAxisFromParent), base.transform.localRotation.eulerAngles.y, base.transform.localRotation.eulerAngles.z);
		}
		if (this.maxYAxisFromParent != 0f)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles.x, Mathf.Clamp(base.transform.localRotation.eulerAngles.y, -this.maxYAxisFromParent, this.maxYAxisFromParent), base.transform.localRotation.eulerAngles.z);
		}
		if (this.maxZAxisFromParent != 0f)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles.x, base.transform.localRotation.eulerAngles.y, Mathf.Clamp(base.transform.localRotation.eulerAngles.z, -this.maxZAxisFromParent, this.maxZAxisFromParent));
		}
		if (this.rotationOffset != Vector3.zero)
		{
			base.transform.localRotation = Quaternion.Euler(base.transform.localRotation.eulerAngles + this.rotationOffset);
		}
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x00009513 File Offset: 0x00007713
	public void ChangeOverrideTarget(EnemyTarget target)
	{
		this.target = target;
		this.overrideTarget = target.trackedTransform;
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x00009528 File Offset: 0x00007728
	public void ChangeOverrideTarget(Transform target)
	{
		this.target = new EnemyTarget(target);
		this.overrideTarget = target;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x00009540 File Offset: 0x00007740
	public void SnapToTarget()
	{
		this.EnsureTargetExists();
		if (this.target == null)
		{
			return;
		}
		Vector3 headPosition = this.target.headPosition;
		if (!this.useXAxis)
		{
			headPosition.x = base.transform.position.x;
		}
		if (!this.useYAxis)
		{
			headPosition.y = base.transform.position.y;
		}
		if (!this.useZAxis)
		{
			headPosition.z = base.transform.position.z;
		}
		Quaternion quaternion = Quaternion.LookRotation(headPosition - base.transform.position);
		base.transform.rotation = quaternion;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x000095E8 File Offset: 0x000077E8
	public void ChangeSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x000095F1 File Offset: 0x000077F1
	public void ChangeDifficulty(int newDiff)
	{
		this.difficulty = newDiff;
		this.UpdateDifficulty();
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x00009600 File Offset: 0x00007800
	public void UpdateDifficulty()
	{
		if (this.difficulty == 1)
		{
			this.difficultySpeedMultiplier = 0.8f;
			return;
		}
		if (this.difficulty == 0)
		{
			this.difficultySpeedMultiplier = 0.6f;
		}
	}

	// Token: 0x040001D2 RID: 466
	public Transform overrideTarget;

	// Token: 0x040001D3 RID: 467
	public EnemyTarget target;

	// Token: 0x040001D4 RID: 468
	[Space]
	[Tooltip("If the target is player (null), use the camera instead of the player head position. Helpful in third-person mode.")]
	public bool preferCameraOverHead;

	// Token: 0x040001D5 RID: 469
	[Tooltip("Copies camera's rotation instead of looking at the camera, this will mean the object always appears flat like a sprite.")]
	public bool faceScreenInsteadOfCamera;

	// Token: 0x040001D6 RID: 470
	public bool dontRotateIfBlind;

	// Token: 0x040001D7 RID: 471
	public float speed;

	// Token: 0x040001D8 RID: 472
	public bool easeIn;

	// Token: 0x040001D9 RID: 473
	public float maxAngle;

	// Token: 0x040001DA RID: 474
	[Space]
	public bool useXAxis = true;

	// Token: 0x040001DB RID: 475
	public bool useYAxis = true;

	// Token: 0x040001DC RID: 476
	public bool useZAxis = true;

	// Token: 0x040001DD RID: 477
	[Space]
	public Vector3 rotationOffset;

	// Token: 0x040001DE RID: 478
	[Space]
	public float maxXAxisFromParent;

	// Token: 0x040001DF RID: 479
	public float maxYAxisFromParent;

	// Token: 0x040001E0 RID: 480
	public float maxZAxisFromParent;

	// Token: 0x040001E1 RID: 481
	[Header("Enemy")]
	public EnemyIdentifier eid;

	// Token: 0x040001E2 RID: 482
	private int difficulty;

	// Token: 0x040001E3 RID: 483
	public bool difficultyVariance;

	// Token: 0x040001E4 RID: 484
	private float difficultySpeedMultiplier = 1f;
}
