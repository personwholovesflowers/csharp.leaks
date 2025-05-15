using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class CheckForScroller : MonoBehaviour
{
	// Token: 0x060003B5 RID: 949 RVA: 0x00016E80 File Offset: 0x00015080
	private void Start()
	{
		this.cdat = MonoSingleton<ComponentsDatabase>.Instance;
		if (!this.checkOnStart)
		{
			return;
		}
		if (this.cdat && this.cdat.scrollers.Count > 0)
		{
			Collider[] array = Physics.OverlapSphere(base.transform.position, 1f, LayerMaskDefaults.Get(LMD.Environment));
			if (array.Length != 0)
			{
				foreach (Collider collider in array)
				{
					ScrollingTexture scrollingTexture;
					if (this.cdat.scrollers.Contains(collider.transform) && collider.transform.TryGetComponent<ScrollingTexture>(out scrollingTexture))
					{
						scrollingTexture.attachedObjects.Add(base.transform);
					}
				}
			}
		}
		if (!this.checkOnCollision)
		{
			Object.Destroy(this);
		}
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00016F44 File Offset: 0x00015144
	private void OnCollisionEnter(Collision col)
	{
		if (!this.checkOnCollision)
		{
			return;
		}
		ScrollingTexture scrollingTexture;
		if (this.cdat && this.cdat.scrollers.Count > 0 && this.cdat.scrollers.Contains(col.transform) && col.transform.TryGetComponent<ScrollingTexture>(out scrollingTexture))
		{
			this.scroller = scrollingTexture;
			if (this.asRigidbody)
			{
				if (!this.rb)
				{
					this.rb = base.GetComponent<Rigidbody>();
				}
				scrollingTexture.touchingRbs.Add(this.rb);
				Vector3 force = scrollingTexture.force;
				if (scrollingTexture.relativeDirection)
				{
					force = new Vector3(scrollingTexture.force.x * scrollingTexture.transform.forward.x, scrollingTexture.force.y * scrollingTexture.transform.forward.y, scrollingTexture.force.z * scrollingTexture.transform.forward.z);
				}
				this.rb.AddForce(force, ForceMode.VelocityChange);
				return;
			}
			scrollingTexture.attachedObjects.Add(base.transform);
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00017074 File Offset: 0x00015274
	private void OnCollisionExit(Collision col)
	{
		if (!this.checkOnCollision)
		{
			return;
		}
		if (this.scroller && col.transform == this.scroller.transform)
		{
			if (this.asRigidbody)
			{
				if (!this.rb)
				{
					this.rb = base.GetComponent<Rigidbody>();
				}
				this.scroller.touchingRbs.Remove(this.rb);
				return;
			}
			this.scroller.attachedObjects.Remove(base.transform);
		}
	}

	// Token: 0x04000489 RID: 1161
	public bool checkOnStart = true;

	// Token: 0x0400048A RID: 1162
	public bool checkOnCollision = true;

	// Token: 0x0400048B RID: 1163
	private ScrollingTexture scroller;

	// Token: 0x0400048C RID: 1164
	public bool asRigidbody;

	// Token: 0x0400048D RID: 1165
	private Rigidbody rb;

	// Token: 0x0400048E RID: 1166
	private ComponentsDatabase cdat;
}
