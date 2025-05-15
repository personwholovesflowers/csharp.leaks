using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class InstantiateObject : MonoBehaviour
{
	// Token: 0x06000DF4 RID: 3572 RVA: 0x00068D38 File Offset: 0x00066F38
	private void OnEnable()
	{
		if (this.instantiateOnEnable)
		{
			this.Instantiate();
		}
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00068D48 File Offset: 0x00066F48
	public void Instantiate()
	{
		if (this.removePreviousOnInstantiate)
		{
			foreach (GameObject gameObject in this.createdObjects)
			{
				Object.Destroy(gameObject);
			}
			this.createdObjects.Clear();
		}
		GameObject gameObject2 = Object.Instantiate<GameObject>(this.source);
		if (this.useOwnPosition)
		{
			gameObject2.transform.position = base.transform.position;
		}
		if (this.useOwnRotation)
		{
			if (this.combineRotations)
			{
				gameObject2.transform.rotation *= base.transform.rotation;
			}
			else
			{
				gameObject2.transform.rotation = base.transform.rotation;
			}
		}
		if (this.reParent)
		{
			gameObject2.transform.SetParent(base.transform);
			if (this.useOwnPosition)
			{
				gameObject2.transform.localPosition = Vector3.zero;
			}
			if (this.useOwnRotation)
			{
				gameObject2.transform.localRotation = Quaternion.identity;
			}
		}
		this.createdObjects.Add(gameObject2);
		InstantiateObjectMode instantiateObjectMode = this.mode;
		if (instantiateObjectMode != InstantiateObjectMode.ForceEnable)
		{
			if (instantiateObjectMode == InstantiateObjectMode.ForceDisable)
			{
				gameObject2.SetActive(false);
			}
		}
		else
		{
			gameObject2.SetActive(true);
		}
		if (this.parentToGoreZone)
		{
			if (this.gz == null)
			{
				this.gz = GoreZone.ResolveGoreZone(base.transform);
			}
			gameObject2.transform.SetParent(this.gz.transform, true);
		}
	}

	// Token: 0x04001276 RID: 4726
	[SerializeField]
	private bool instantiateOnEnable = true;

	// Token: 0x04001277 RID: 4727
	[SerializeField]
	private GameObject source;

	// Token: 0x04001278 RID: 4728
	[SerializeField]
	private InstantiateObjectMode mode;

	// Token: 0x04001279 RID: 4729
	[SerializeField]
	private bool removePreviousOnInstantiate = true;

	// Token: 0x0400127A RID: 4730
	[SerializeField]
	private bool reParent = true;

	// Token: 0x0400127B RID: 4731
	[SerializeField]
	private bool useOwnPosition = true;

	// Token: 0x0400127C RID: 4732
	[SerializeField]
	private bool useOwnRotation = true;

	// Token: 0x0400127D RID: 4733
	[SerializeField]
	private bool combineRotations;

	// Token: 0x0400127E RID: 4734
	[SerializeField]
	private bool parentToGoreZone;

	// Token: 0x0400127F RID: 4735
	private GoreZone gz;

	// Token: 0x04001280 RID: 4736
	private List<GameObject> createdObjects = new List<GameObject>();
}
