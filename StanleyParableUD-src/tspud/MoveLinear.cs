using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class MoveLinear : HammerEntity
{
	// Token: 0x0600076E RID: 1902 RVA: 0x00026344 File Offset: 0x00024544
	private void Awake()
	{
		if (this.startLocked)
		{
			this.locked = true;
		}
		this.closedPos = base.transform.localPosition;
		if (this.moveDistance == 0f)
		{
			Bounds bounds = base.GetComponent<Renderer>().bounds;
			this.moveDistance = Vector3.Scale(this.moveDir, bounds.extents * 2f).magnitude;
		}
		if (this.multiplyMoveDirectionWithTransformWorldToLocalMatrix)
		{
			this.moveDir = base.transform.worldToLocalMatrix * this.moveDir;
		}
		this.openPos = this.closedPos + this.moveDir * (this.moveDistance - this.lip);
		this.targetPos = this.startPos;
		this.currentPos = this.targetPos;
		base.transform.localPosition = Vector3.Lerp(this.closedPos, this.openPos, this.currentPos);
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00026448 File Offset: 0x00024648
	private void Update()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (this.currentPos == this.targetPos)
		{
			return;
		}
		if (this.speed == 0f)
		{
			return;
		}
		base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, Vector3.Lerp(this.closedPos, this.openPos, this.targetPos), this.speed * Singleton<GameMaster>.Instance.GameDeltaTime);
		this.currentPos = TimMaths.Vector3InverseLerp(this.closedPos, this.openPos, base.transform.localPosition);
		if (this.currentPos == 1f)
		{
			base.FireOutput(Outputs.OnFullyOpen);
			return;
		}
		if (this.currentPos == 0f)
		{
			base.FireOutput(Outputs.OnFullyClosed);
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0002650B File Offset: 0x0002470B
	public void Input_Open()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (this.locked)
		{
			return;
		}
		this.targetPos = 1f;
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0002652A File Offset: 0x0002472A
	public void Input_Close()
	{
		if (!this.isEnabled)
		{
			return;
		}
		if (this.locked)
		{
			return;
		}
		this.targetPos = 0f;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00026549 File Offset: 0x00024749
	public void Input_Lock()
	{
		this.locked = true;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00026552 File Offset: 0x00024752
	public void Input_Unlock()
	{
		this.locked = false;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0002655B File Offset: 0x0002475B
	public void Input_SetSpeed(float newSpeed)
	{
		newSpeed = (this.speed = newSpeed / 100f);
	}

	// Token: 0x04000791 RID: 1937
	public Vector3 moveDir;

	// Token: 0x04000792 RID: 1938
	public float moveDistance;

	// Token: 0x04000793 RID: 1939
	public float startPos;

	// Token: 0x04000794 RID: 1940
	public bool startLocked;

	// Token: 0x04000795 RID: 1941
	public float lip;

	// Token: 0x04000796 RID: 1942
	public float speed;

	// Token: 0x04000797 RID: 1943
	private bool locked;

	// Token: 0x04000798 RID: 1944
	private Vector3 closedPos;

	// Token: 0x04000799 RID: 1945
	private Vector3 openPos;

	// Token: 0x0400079A RID: 1946
	private float targetPos;

	// Token: 0x0400079B RID: 1947
	private float currentPos;

	// Token: 0x0400079C RID: 1948
	[SerializeField]
	private bool multiplyMoveDirectionWithTransformWorldToLocalMatrix;
}
