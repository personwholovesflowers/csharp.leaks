using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class PlatformParent : MonoBehaviour
{
	// Token: 0x06000966 RID: 2406 RVA: 0x0002C44A File Offset: 0x0002A64A
	private void Awake()
	{
		this._body = base.GetComponent<Rigidbody>();
		this._collider = base.GetComponent<Collider>();
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0002C464 File Offset: 0x0002A664
	private void Start()
	{
		if (this.forceStanleyTriggerEnterOnSceneReady_HACK != null)
		{
			base.StartCoroutine(this.PerformStanleyMoveHack());
		}
		if (this.forceParentingOnStart)
		{
			StanleyController.Instance.ParentTo(base.transform);
		}
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0002C499 File Offset: 0x0002A699
	private IEnumerator PerformStanleyMoveHack()
	{
		if (this.forceStanleyTriggerEnterOnSceneReady_HACK != null)
		{
			if (this.performExtraStanleyMoveOnReady_HACK)
			{
				StanleyController.Instance.transform.position = new Vector3(-1000f, -1000f, -1000f);
				yield return null;
			}
			StanleyController.Instance.transform.position = this.forceStanleyTriggerEnterOnSceneReady_HACK.position;
		}
		yield break;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0002C4A8 File Offset: 0x0002A6A8
	public void SetForceParent(bool value)
	{
		this.forceParent = value;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0002C4B1 File Offset: 0x0002A6B1
	private void OnTriggerEnter(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.touchingColliders.Contains(col))
		{
			return;
		}
		this.touchingColliders.Add(col);
		StanleyController.Instance.transform.parent = base.transform;
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0002C4F1 File Offset: 0x0002A6F1
	private void OnTriggerStay(Collider col)
	{
		if (!col.CompareTag("Player"))
		{
			return;
		}
		if (this.touchingColliders.Contains(col))
		{
			return;
		}
		this.touchingColliders.Add(col);
		StanleyController.Instance.ParentTo(base.transform);
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0002C52C File Offset: 0x0002A72C
	private void OnTriggerExit(Collider col)
	{
		if (this.forceParent)
		{
			return;
		}
		if (this.touchingColliders.Contains(col))
		{
			this.touchingColliders.Remove(col);
			StanleyController.Instance.Deparent(false);
		}
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002C55D File Offset: 0x0002A75D
	public void Stop()
	{
		StanleyController.Instance.Deparent(false);
		this.touchingColliders = new List<Collider>();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002C581 File Offset: 0x0002A781
	public void GoAway()
	{
		StanleyController.Instance.Deparent(false);
		this.touchingColliders = new List<Collider>();
	}

	// Token: 0x04000947 RID: 2375
	[SerializeField]
	private bool forceParent;

	// Token: 0x04000948 RID: 2376
	[Header("Moves stanley to this transform on scene ready iff set")]
	[SerializeField]
	private Transform forceStanleyTriggerEnterOnSceneReady_HACK;

	// Token: 0x04000949 RID: 2377
	[SerializeField]
	private bool performExtraStanleyMoveOnReady_HACK;

	// Token: 0x0400094A RID: 2378
	private List<Collider> touchingColliders = new List<Collider>();

	// Token: 0x0400094B RID: 2379
	private Rigidbody _body;

	// Token: 0x0400094C RID: 2380
	private Collider _collider;

	// Token: 0x0400094D RID: 2381
	[SerializeField]
	public bool forceParentingOnStart;
}
