using System;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class PhysicsSceneStateEnforcer : MonoBehaviour
{
	// Token: 0x06000FCD RID: 4045 RVA: 0x00075C5A File Offset: 0x00073E5A
	public void SetMatchingObject(GameObject matchingObject)
	{
		this.matchingObject = matchingObject;
		this.SetMatchingObjectActive(base.gameObject.activeInHierarchy);
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x00075C74 File Offset: 0x00073E74
	private void OnEnable()
	{
		this.SetMatchingObjectActive(true);
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x00075C7D File Offset: 0x00073E7D
	private void OnDisable()
	{
		this.SetMatchingObjectActive(false);
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x00075C86 File Offset: 0x00073E86
	private void OnDestroy()
	{
		this.DestroyMatchingObject();
	}

	// Token: 0x06000FD1 RID: 4049 RVA: 0x00075C8E File Offset: 0x00073E8E
	private void SetMatchingObjectActive(bool active)
	{
		if (this.matchingObject != null && this.matchingObject.activeSelf != active)
		{
			this.matchingObject.SetActive(active);
		}
	}

	// Token: 0x06000FD2 RID: 4050 RVA: 0x00075CB8 File Offset: 0x00073EB8
	private void DestroyMatchingObject()
	{
		if (this.matchingObject != null)
		{
			Object.Destroy(this.matchingObject);
		}
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x00075CD3 File Offset: 0x00073ED3
	public void ForceUpdate()
	{
		if (this.matchingObject == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.matchingObject.SetActive(false);
		this.matchingObject.SetActive(true);
	}

	// Token: 0x0400155F RID: 5471
	private GameObject matchingObject;
}
