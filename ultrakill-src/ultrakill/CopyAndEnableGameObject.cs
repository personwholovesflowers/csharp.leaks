using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class CopyAndEnableGameObject : MonoBehaviour
{
	// Token: 0x0600048A RID: 1162 RVA: 0x0001F933 File Offset: 0x0001DB33
	private void Start()
	{
		if (this.onEnable)
		{
			this.Activate();
		}
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001F944 File Offset: 0x0001DB44
	public void Activate()
	{
		GameObject gameObject = Object.Instantiate<GameObject>(this.target, this.target.transform.position, this.target.transform.rotation);
		if (this.target.transform.parent)
		{
			gameObject.transform.SetParent(this.target.transform.parent, true);
		}
		gameObject.SetActive(true);
	}

	// Token: 0x0400062F RID: 1583
	public GameObject target;

	// Token: 0x04000630 RID: 1584
	public bool onEnable;
}
