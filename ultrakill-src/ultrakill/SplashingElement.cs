using System;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class SplashingElement : MonoBehaviour
{
	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06001854 RID: 6228 RVA: 0x000C6A5B File Offset: 0x000C4C5B
	public bool isSplashing
	{
		get
		{
			return this._isSplashing;
		}
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06001855 RID: 6229 RVA: 0x000C6A63 File Offset: 0x000C4C63
	public Vector3 splashPosition
	{
		get
		{
			return this._splashPosition;
		}
	}

	// Token: 0x06001856 RID: 6230 RVA: 0x000C6A6C File Offset: 0x000C4C6C
	public void FixedUpdate()
	{
		if (!this.previousElement)
		{
			return;
		}
		Vector3 position = base.transform.position;
		Vector3 position2 = this.previousElement.transform.position;
		Ray ray = new Ray(position, position2 - position);
		Ray ray2 = new Ray(position2, position - position2);
		float num = Vector3.Distance(position2, base.transform.position);
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(ray, out raycastHit, num, LayerMask.GetMask(new string[] { "Water" }));
		if (!flag)
		{
			flag = Physics.Raycast(ray2, out raycastHit, num, LayerMask.GetMask(new string[] { "Water" }));
		}
		this._isSplashing = flag;
		if (flag)
		{
			this._splashPosition = raycastHit.point;
		}
	}

	// Token: 0x04002226 RID: 8742
	public SplashingElement previousElement;

	// Token: 0x04002227 RID: 8743
	private bool _isSplashing;

	// Token: 0x04002228 RID: 8744
	private Vector3 _splashPosition;
}
