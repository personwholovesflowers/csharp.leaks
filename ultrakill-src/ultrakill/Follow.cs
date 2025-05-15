using System;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class Follow : MonoBehaviour
{
	// Token: 0x06000A5F RID: 2655 RVA: 0x00048F98 File Offset: 0x00047198
	private void Awake()
	{
		if (this.restrictToColliderBounds != null && this.restrictToColliderBounds.Length != 0)
		{
			for (int i = 0; i < this.restrictToColliderBounds.Length; i++)
			{
				if (!(this.restrictToColliderBounds[i] == null))
				{
					Bounds bounds = this.area;
					if (this.area.size == Vector3.zero)
					{
						this.area = this.restrictToColliderBounds[i].bounds;
					}
					else
					{
						this.area.Encapsulate(this.restrictToColliderBounds[i].bounds);
					}
				}
			}
		}
		if (this.target != null)
		{
			return;
		}
		this.target = MonoSingleton<PlayerTracker>.Instance.GetPlayer();
		this.followingPlayer = true;
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0004904C File Offset: 0x0004724C
	private void Update()
	{
		if (!this.target)
		{
			if (this.destroyIfNoTarget)
			{
				Object.Destroy(base.gameObject);
			}
			return;
		}
		if (this.mimicRotation)
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			if (this.followingPlayer)
			{
				if (this.rotX)
				{
					eulerAngles.x = -MonoSingleton<CameraController>.Instance.rotationX;
				}
			}
			else if (this.rotX)
			{
				eulerAngles.x = this.target.eulerAngles.x;
			}
			if (this.rotY)
			{
				eulerAngles.y = this.target.eulerAngles.y;
			}
			if (this.rotZ)
			{
				eulerAngles.z = this.target.eulerAngles.z;
			}
			if (this.applyRotationLocally)
			{
				base.transform.localEulerAngles = eulerAngles;
			}
			else
			{
				base.transform.eulerAngles = eulerAngles;
			}
		}
		if (!this.mimicPosition)
		{
			return;
		}
		Vector3 vector = new Vector3(this.followX ? this.target.position.x : base.transform.position.x, this.followY ? this.target.position.y : base.transform.position.y, this.followZ ? this.target.position.z : base.transform.position.z);
		if (this.speed != 0f)
		{
			float num = this.speed * Time.deltaTime;
			if (this.applyPositionLocally)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, vector, num);
			}
			else
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector, num);
			}
			if (this.restrictToColliderBounds != null && this.restrictToColliderBounds.Length != 0)
			{
				base.transform.position = this.area.ClosestPoint(base.transform.position);
			}
			return;
		}
		if (this.restrictToColliderBounds != null && this.restrictToColliderBounds.Length != 0)
		{
			base.transform.position = this.area.ClosestPoint(vector);
			return;
		}
		if (this.applyPositionLocally)
		{
			base.transform.localPosition = vector;
			return;
		}
		base.transform.position = vector;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0004929D File Offset: 0x0004749D
	public void SetTarget(Transform newTarget)
	{
		this.target = newTarget;
		this.followingPlayer = false;
	}

	// Token: 0x04000DBE RID: 3518
	public float speed;

	// Token: 0x04000DBF RID: 3519
	public Transform target;

	// Token: 0x04000DC0 RID: 3520
	public bool mimicPosition = true;

	// Token: 0x04000DC1 RID: 3521
	public bool applyPositionLocally;

	// Token: 0x04000DC2 RID: 3522
	public bool followX = true;

	// Token: 0x04000DC3 RID: 3523
	public bool followY = true;

	// Token: 0x04000DC4 RID: 3524
	public bool followZ = true;

	// Token: 0x04000DC5 RID: 3525
	public bool mimicRotation;

	// Token: 0x04000DC6 RID: 3526
	public bool applyRotationLocally;

	// Token: 0x04000DC7 RID: 3527
	public bool rotX = true;

	// Token: 0x04000DC8 RID: 3528
	public bool rotY = true;

	// Token: 0x04000DC9 RID: 3529
	public bool rotZ = true;

	// Token: 0x04000DCA RID: 3530
	private bool followingPlayer;

	// Token: 0x04000DCB RID: 3531
	public Collider[] restrictToColliderBounds;

	// Token: 0x04000DCC RID: 3532
	private Bounds area;

	// Token: 0x04000DCD RID: 3533
	public bool destroyIfNoTarget;
}
