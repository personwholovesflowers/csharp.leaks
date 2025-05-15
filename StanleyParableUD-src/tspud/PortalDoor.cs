using System;
using UnityEngine;

// Token: 0x0200016B RID: 363
public class PortalDoor : HammerEntity
{
	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000879 RID: 2169 RVA: 0x000282B7 File Offset: 0x000264B7
	public EasyPortal EasyPortal
	{
		get
		{
			return this.easyPortal;
		}
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000282BF File Offset: 0x000264BF
	private void Start()
	{
		if (!this.isDrawing && this.easyPortal != null)
		{
			this.easyPortal.SetStatus(false);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00005444 File Offset: 0x00003644
	private void OnValidate()
	{
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x000282E3 File Offset: 0x000264E3
	public void Input_Open()
	{
		this.isDrawing = true;
		if (this.easyPortal != null)
		{
			this.easyPortal.SetStatus(true);
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00028306 File Offset: 0x00026506
	public void Input_Close()
	{
		this.isDrawing = false;
		if (this.easyPortal != null)
		{
			this.easyPortal.SetStatus(false);
		}
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00028329 File Offset: 0x00026529
	public void Input_SetPartner()
	{
		this.Input_Close();
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00028334 File Offset: 0x00026534
	public void Input_SetPartner(string newPartner)
	{
		this.destination = newPartner;
		GameObject gameObject = GameObject.Find(this.destination);
		if (!gameObject)
		{
			return;
		}
		PortalDoor component = gameObject.GetComponent<PortalDoor>();
		if (!component)
		{
			return;
		}
		this.destinationChild.position = component.transform.position;
		this.destinationChild.rotation = Quaternion.LookRotation(-component.transform.forward, component.transform.up);
		if (this.easyPortal != null)
		{
			this.easyPortal.SetNewDestination(component.transform);
		}
		component.destination != base.name;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x000283E0 File Offset: 0x000265E0
	private void OnDrawGizmosSelected()
	{
		Vector3 position = base.transform.position;
		Vector3 position2 = this.destinationChild.position;
		int num = 10;
		this.gizmoOffset += 0.1f;
		if (this.gizmoOffset >= (float)num)
		{
			this.gizmoOffset = 0f;
		}
		(position2 - position) / (float)num;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)(i + Mathf.FloorToInt(this.gizmoOffset));
			if (num2 > (float)num)
			{
				num2 -= (float)num;
			}
			Gizmos.color = Color.Lerp(Color.red, Color.green, 1f - num2 / (float)num);
			Gizmos.DrawLine(Vector3.Lerp(position, position2, (float)i / (float)num), Vector3.Lerp(position, position2, ((float)i + 1f) / (float)num));
		}
	}

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private EasyPortal easyPortal;

	// Token: 0x0400084D RID: 2125
	public string destination;

	// Token: 0x0400084E RID: 2126
	public Transform destinationChild;

	// Token: 0x0400084F RID: 2127
	public bool isDrawing;

	// Token: 0x04000850 RID: 2128
	private float gizmoOffset;
}
