using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class PhysBallsocket : HammerEntity
{
	// Token: 0x060007A3 RID: 1955 RVA: 0x00026B9C File Offset: 0x00024D9C
	private void OnValidate()
	{
		if (this.connectedEntity == null || this.connectedEntity.name != this.connectedEntityName)
		{
			GameObject gameObject = GameObject.Find(this.connectedEntityName);
			if (gameObject)
			{
				this.connectedEntity = gameObject;
			}
			else
			{
				this.connectedEntity = null;
			}
		}
		if (this.connectedEntity)
		{
			Rigidbody rigidbody = this.connectedEntity.GetComponent<Rigidbody>();
			Rigidbody rigidbody2 = base.GetComponent<Rigidbody>();
			ConfigurableJoint configurableJoint = base.GetComponent<ConfigurableJoint>();
			if (!rigidbody)
			{
				rigidbody = this.connectedEntity.AddComponent<Rigidbody>();
			}
			if (!rigidbody2)
			{
				rigidbody2 = base.gameObject.AddComponent<Rigidbody>();
			}
			if (!configurableJoint)
			{
				configurableJoint = base.gameObject.AddComponent<ConfigurableJoint>();
			}
			rigidbody2.constraints = RigidbodyConstraints.FreezePosition;
			rigidbody2.useGravity = false;
			rigidbody.drag = 0.01f;
			configurableJoint.connectedBody = rigidbody;
			configurableJoint.xMotion = ConfigurableJointMotion.Locked;
			configurableJoint.yMotion = ConfigurableJointMotion.Locked;
			configurableJoint.zMotion = ConfigurableJointMotion.Locked;
		}
	}

	// Token: 0x040007C3 RID: 1987
	public string connectedEntityName = "";

	// Token: 0x040007C4 RID: 1988
	[SerializeField]
	[HideInInspector]
	private GameObject connectedEntity;
}
