using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class ExplosiveFish : MonoBehaviour
{
	// Token: 0x06000995 RID: 2453 RVA: 0x000428A8 File Offset: 0x00040AA8
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x000428B8 File Offset: 0x00040AB8
	private void Update()
	{
		if (this.rb.isKinematic)
		{
			return;
		}
		if (!this.activated)
		{
			this.timeSinceActivated = 0f;
		}
		this.activated = true;
		this.fire.SetActive(true);
		if (this.timeSinceActivated > 3f)
		{
			Object.Instantiate<GameObject>(this.explosion, base.transform.position, Quaternion.identity);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000C79 RID: 3193
	private Rigidbody rb;

	// Token: 0x04000C7A RID: 3194
	private bool activated;

	// Token: 0x04000C7B RID: 3195
	private TimeSince timeSinceActivated;

	// Token: 0x04000C7C RID: 3196
	[SerializeField]
	private GameObject fire;

	// Token: 0x04000C7D RID: 3197
	[SerializeField]
	private GameObject explosion;
}
