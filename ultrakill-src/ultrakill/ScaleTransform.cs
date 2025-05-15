using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003C6 RID: 966
public class ScaleTransform : MonoBehaviour
{
	// Token: 0x060015F8 RID: 5624 RVA: 0x000B21A0 File Offset: 0x000B03A0
	private void Update()
	{
		if (base.transform.localScale != this.target)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.target, this.speed * Time.deltaTime);
			if (base.transform.localScale == this.target)
			{
				UnityEvent unityEvent = this.onComplete;
				if (unityEvent == null)
				{
					return;
				}
				unityEvent.Invoke();
			}
		}
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x000B221A File Offset: 0x000B041A
	public void SetTransformX(float target)
	{
		base.transform.localScale = new Vector3(target, base.transform.localScale.y, base.transform.localScale.z);
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x000B224D File Offset: 0x000B044D
	public void SetTransformY(float target)
	{
		base.transform.localScale = new Vector3(base.transform.localScale.x, target, base.transform.localScale.z);
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x000B2280 File Offset: 0x000B0480
	public void SetTransformZ(float target)
	{
		base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, target);
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x0005FB42 File Offset: 0x0005DD42
	public void SetScaleToZero()
	{
		base.transform.localScale = Vector3.zero;
	}

	// Token: 0x04001E47 RID: 7751
	public Vector3 target;

	// Token: 0x04001E48 RID: 7752
	public float speed;

	// Token: 0x04001E49 RID: 7753
	public UnityEvent onComplete;
}
