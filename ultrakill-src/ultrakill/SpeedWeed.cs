using System;
using System.Text;
using UnityEngine;

// Token: 0x0200042C RID: 1068
public class SpeedWeed : MonoBehaviour
{
	// Token: 0x0600180A RID: 6154 RVA: 0x000C3AC0 File Offset: 0x000C1CC0
	private void FixedUpdate()
	{
		float num = (base.transform.position - this.lastPosition).magnitude / Time.deltaTime;
		this.lastPosition = base.transform.position;
		if (Math.Abs(num - this.lastSpeed) < 0.1f)
		{
			return;
		}
		TimeSince timeSince = this.timeSinceLastSpeedChange;
		this.timeSinceLastSpeedChange = 0f;
		this.lastSpeed = num;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Speed: ");
		stringBuilder.Append(num);
		if (timeSince > 1f)
		{
			stringBuilder.Append(" (changed after ");
			stringBuilder.Append(timeSince);
			stringBuilder.Append("s)");
		}
	}

	// Token: 0x040021AE RID: 8622
	private Vector3 lastPosition;

	// Token: 0x040021AF RID: 8623
	private float lastSpeed;

	// Token: 0x040021B0 RID: 8624
	private TimeSince timeSinceLastSpeedChange;
}
