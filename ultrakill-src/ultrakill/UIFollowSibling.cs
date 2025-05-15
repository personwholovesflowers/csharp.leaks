using System;
using UnityEngine;

// Token: 0x020003EC RID: 1004
public class UIFollowSibling : MonoBehaviour
{
	// Token: 0x06001693 RID: 5779 RVA: 0x000B5471 File Offset: 0x000B3671
	private void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		if (this.target == null)
		{
			Debug.LogWarning("UIFollowSibling: Target is not assigned.");
		}
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x000B5498 File Offset: 0x000B3698
	private void Update()
	{
		if (this.target != null)
		{
			this.rectTransform.position = this.target.position + this.offset;
			this.rectTransform.rotation = this.target.rotation;
			this.rectTransform.localScale = this.target.localScale;
		}
	}

	// Token: 0x04001F2C RID: 7980
	[SerializeField]
	private RectTransform target;

	// Token: 0x04001F2D RID: 7981
	[SerializeField]
	private Vector3 offset = Vector3.zero;

	// Token: 0x04001F2E RID: 7982
	private RectTransform rectTransform;
}
