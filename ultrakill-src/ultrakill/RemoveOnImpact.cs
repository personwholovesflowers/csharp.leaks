using System;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class RemoveOnImpact : MonoBehaviour
{
	// Token: 0x060014C5 RID: 5317 RVA: 0x000A7901 File Offset: 0x000A5B01
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(this.otherTag))
		{
			base.Invoke("RemoveSelf", this.timeUntilRemove);
		}
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x0000A719 File Offset: 0x00008919
	private void RemoveSelf()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001C92 RID: 7314
	public string otherTag;

	// Token: 0x04001C93 RID: 7315
	public float timeUntilRemove;
}
