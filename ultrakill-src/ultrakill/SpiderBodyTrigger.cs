using System;
using UnityEngine;

// Token: 0x0200042E RID: 1070
public class SpiderBodyTrigger : MonoBehaviour
{
	// Token: 0x06001828 RID: 6184 RVA: 0x000C5BC7 File Offset: 0x000C3DC7
	private void Start()
	{
		this.spbody = base.transform.parent.GetComponentInChildren<SpiderBody>(true);
		this.UpdatePosition();
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000C5BE6 File Offset: 0x000C3DE6
	private void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x000C5BF0 File Offset: 0x000C3DF0
	private void UpdatePosition()
	{
		if (this.spbody != null)
		{
			base.transform.SetPositionAndRotation(this.spbody.transform.position, this.spbody.transform.rotation);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000C5C44 File Offset: 0x000C3E44
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 12)
		{
			if (!this.spbody)
			{
				this.spbody = base.transform.parent.GetComponentInChildren<SpiderBody>();
			}
			if (this.spbody)
			{
				this.spbody.TriggerHit(other);
			}
		}
	}

	// Token: 0x040021F5 RID: 8693
	private SpiderBody spbody;
}
