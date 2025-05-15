using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class Vibrate : MonoBehaviour
{
	// Token: 0x06001B28 RID: 6952 RVA: 0x000E21B8 File Offset: 0x000E03B8
	private void Start()
	{
		this.origPos = base.transform.localPosition;
		this.targetPos = this.origPos;
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000E21D8 File Offset: 0x000E03D8
	private void Update()
	{
		Vector3 vector = Vector3.zero;
		if (this.speed != 0f)
		{
			vector = base.transform.localPosition;
		}
		if (this.speed == 0f)
		{
			base.transform.localPosition = this.origPos + Random.insideUnitSphere * this.intensity;
			return;
		}
		if (vector == this.targetPos)
		{
			this.targetPos = this.origPos + Random.insideUnitSphere * this.intensity;
			return;
		}
		base.transform.localPosition = Vector3.MoveTowards(vector, this.targetPos, Time.deltaTime * this.speed);
	}

	// Token: 0x04002659 RID: 9817
	public float intensity;

	// Token: 0x0400265A RID: 9818
	private Vector3 origPos;

	// Token: 0x0400265B RID: 9819
	public float speed;

	// Token: 0x0400265C RID: 9820
	private Vector3 targetPos;
}
