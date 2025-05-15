using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class DistanceBasedCull : MonoBehaviour
{
	// Token: 0x06000A09 RID: 2569 RVA: 0x0002F7AB File Offset: 0x0002D9AB
	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		yield return null;
		if (CullForSwitchController.IsSwitchEnvironment)
		{
			this.SetupDistanceBasedCull();
			this.c = null;
		}
		yield break;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0002F7BC File Offset: 0x0002D9BC
	[ContextMenu("SetupDistanceBasedCull")]
	private void SetupDistanceBasedCull()
	{
		this.c = Camera.main;
		float[] layerCullDistances = this.c.layerCullDistances;
		layerCullDistances[this.distanceCullLayer] = this.cullDistance;
		this.c.layerCullDistances = layerCullDistances;
		this.c.layerCullSpherical = true;
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0002F808 File Offset: 0x0002DA08
	[ContextMenu("DisplayDistanceBasedCull")]
	private void DisplayDistanceBasedCull()
	{
		this.c = Camera.main;
		for (int i = 0; i < 32; i++)
		{
			Debug.Log(string.Concat(new object[]
			{
				i,
				" ",
				LayerMask.LayerToName(i),
				"\t",
				this.c.layerCullDistances[i]
			}));
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0002F874 File Offset: 0x0002DA74
	[ContextMenu("FindAllCullitems")]
	private void FindAllCullItems()
	{
		this.c = Camera.main;
		this.cullItems = new List<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in Resources.FindObjectsOfTypeAll<MeshRenderer>())
		{
			if (meshRenderer.gameObject.layer == this.distanceCullLayer)
			{
				this.cullItems.Add(meshRenderer);
			}
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002F8D0 File Offset: 0x0002DAD0
	private void Update()
	{
		if (this.c == null)
		{
			return;
		}
		foreach (MeshRenderer meshRenderer in this.cullItems)
		{
			bool flag = (this.c.transform.position - meshRenderer.transform.position).sqrMagnitude > this.cullDistance * this.cullDistance;
			if (flag && meshRenderer.enabled)
			{
				meshRenderer.enabled = false;
			}
			if (!flag && !meshRenderer.enabled)
			{
				meshRenderer.enabled = true;
			}
		}
	}

	// Token: 0x04000A05 RID: 2565
	[SerializeField]
	public float cullDistance = 20f;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	public int distanceCullLayer = 30;

	// Token: 0x04000A07 RID: 2567
	public Camera c;

	// Token: 0x04000A08 RID: 2568
	public List<MeshRenderer> cullItems;
}
