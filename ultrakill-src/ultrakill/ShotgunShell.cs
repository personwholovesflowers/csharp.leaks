using System;
using UnityEngine;

// Token: 0x020003FD RID: 1021
public class ShotgunShell : MonoBehaviour
{
	// Token: 0x060016FF RID: 5887 RVA: 0x000BAEAC File Offset: 0x000B90AC
	private void Start()
	{
		base.Invoke("TurnGib", 0.2f);
		base.Invoke("Remove", 2f);
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000BAED0 File Offset: 0x000B90D0
	private void TurnGib()
	{
		this.col = base.GetComponent<Collider>();
		this.col.enabled = true;
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 9;
		}
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x000BAF1C File Offset: 0x000B911C
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.hitGround && LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment))
		{
			this.hitGround = true;
			if (!this.aud)
			{
				this.aud = base.GetComponent<AudioSource>();
			}
			this.aud.pitch = Random.Range(0.85f, 1.15f);
			this.aud.Play();
		}
	}

	// Token: 0x06001702 RID: 5890 RVA: 0x000BAF89 File Offset: 0x000B9189
	private void OnCollisionExit(Collision collision)
	{
		if (LayerMaskDefaults.IsMatchingLayer(collision.gameObject.layer, LMD.Environment))
		{
			this.hitGround = false;
		}
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x000BAFA8 File Offset: 0x000B91A8
	private void Remove()
	{
		if (!this.hitGround || base.transform.position.magnitude > 1000f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.transform.SetParent(GoreZone.ResolveGoreZone(base.transform).gibZone, true);
		Object.Destroy(base.GetComponent<Rigidbody>());
		Object.Destroy(this.col);
	}

	// Token: 0x0400201D RID: 8221
	private bool hitGround;

	// Token: 0x0400201E RID: 8222
	private AudioSource aud;

	// Token: 0x0400201F RID: 8223
	private Collider col;
}
