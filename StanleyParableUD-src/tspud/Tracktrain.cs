using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class Tracktrain : HammerEntity
{
	// Token: 0x06000A24 RID: 2596 RVA: 0x0002FD58 File Offset: 0x0002DF58
	private void Awake()
	{
		if (this.firstTargetObject != null)
		{
			PathTrack component = this.firstTargetObject.GetComponent<PathTrack>();
			if (component != null)
			{
				this.currentPath = component;
				if (this.currentPath.nextPathTrack)
				{
					this.nextPath = this.currentPath.nextPathTrack;
				}
				base.transform.position = this.currentPath.transform.position;
			}
		}
		if (this.initialSpeed != 0f)
		{
			this.currentSpeed = this.initialSpeed;
			this.currentPathSpeed = this.currentSpeed;
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002FDF4 File Offset: 0x0002DFF4
	private void Update()
	{
		if (this.currentSpeed == 0f)
		{
			return;
		}
		if (this.nextPath == null)
		{
			return;
		}
		if (this.velocityType != Tracktrain.VelocityTypes.Instantaneous)
		{
			if (this.nextPath.newSpeed != 0f)
			{
				float num = TimMaths.Vector3InverseLerp(this.currentPath.transform.position, this.nextPath.transform.position, base.transform.position);
				if (this.velocityType == Tracktrain.VelocityTypes.LinearBlend)
				{
					this.currentSpeed = Mathf.Lerp(this.currentPathSpeed, this.nextPath.newSpeed, num);
				}
				else
				{
					this.currentSpeed = Mathf.SmoothStep(this.currentPathSpeed, this.nextPath.newSpeed, Mathf.SmoothStep(0f, 1f, num));
				}
			}
		}
		else
		{
			this.currentSpeed = this.currentPathSpeed;
		}
		float num2 = this.currentSpeed * Singleton<GameMaster>.Instance.GameDeltaTime;
		Vector3 vector = Vector3.MoveTowards(base.transform.position, this.nextPath.transform.position, num2);
		if (vector == this.nextPath.transform.position)
		{
			this.nextPath.Passed();
			this.currentPath = this.nextPath;
			this.nextPath = this.nextPath.nextPathTrack;
			if (this.currentPath.newSpeed != 0f)
			{
				this.currentPathSpeed = this.currentPath.newSpeed;
			}
			if (this.nextPath)
			{
				float num3 = num2 - Vector3.Distance(base.transform.position, vector);
				if (num3 > 0f)
				{
					vector = Vector3.MoveTowards(base.transform.position, this.nextPath.transform.position, num3);
				}
			}
		}
		base.transform.position = vector;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002FFC4 File Offset: 0x0002E1C4
	public void Input_StartForward()
	{
		if (this.currentPath && this.currentPath.newSpeed != 0f)
		{
			this.currentSpeed = this.currentPath.newSpeed;
		}
		else
		{
			this.currentSpeed = this.maxSpeed;
		}
		this.currentPathSpeed = this.currentSpeed;
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0003001B File Offset: 0x0002E21B
	public void Input_Stop()
	{
		this.currentSpeed = 0f;
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x00030028 File Offset: 0x0002E228
	public void Input_SetSpeed(float s)
	{
		s /= 100f;
		s = Mathf.Clamp(s, 0f, this.maxSpeed);
		this.currentSpeed = s;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00030050 File Offset: 0x0002E250
	public void Input_TeleportToPathNode(string nodeName)
	{
		GameObject gameObject = GameObject.Find(nodeName);
		if (gameObject == null)
		{
			return;
		}
		PathTrack component = gameObject.GetComponent<PathTrack>();
		if (component == null)
		{
			return;
		}
		this.currentPath = component;
		if (this.currentPath.nextPathTrack)
		{
			this.nextPath = this.currentPath.nextPathTrack;
		}
		base.transform.position = this.currentPath.transform.position;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x000300C4 File Offset: 0x0002E2C4
	private void OnValidate()
	{
		if (this.firstTargetObject == null || this.firstTargetObject.name != this.firstTarget)
		{
			this.firstTargetObject = GameObject.Find(this.firstTarget);
		}
	}

	// Token: 0x04000A1D RID: 2589
	public bool fixedOrientation = true;

	// Token: 0x04000A1E RID: 2590
	public string firstTarget = "";

	// Token: 0x04000A1F RID: 2591
	[HideInInspector]
	public GameObject firstTargetObject;

	// Token: 0x04000A20 RID: 2592
	private PathTrack currentPath;

	// Token: 0x04000A21 RID: 2593
	private PathTrack nextPath;

	// Token: 0x04000A22 RID: 2594
	public Tracktrain.VelocityTypes velocityType;

	// Token: 0x04000A23 RID: 2595
	public float heightAboveTrack;

	// Token: 0x04000A24 RID: 2596
	public float initialSpeed;

	// Token: 0x04000A25 RID: 2597
	public float maxSpeed = 1f;

	// Token: 0x04000A26 RID: 2598
	private float currentSpeed;

	// Token: 0x04000A27 RID: 2599
	private float currentPathSpeed;

	// Token: 0x020003FE RID: 1022
	public enum VelocityTypes
	{
		// Token: 0x040014DC RID: 5340
		Instantaneous,
		// Token: 0x040014DD RID: 5341
		LinearBlend,
		// Token: 0x040014DE RID: 5342
		EaseInOut
	}
}
