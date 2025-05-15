using System;
using UnityEngine;

namespace Train
{
	// Token: 0x0200051E RID: 1310
	public class ConnectedTram : MonoBehaviour
	{
		// Token: 0x06001DDC RID: 7644 RVA: 0x000F9062 File Offset: 0x000F7262
		private void Awake()
		{
			this.thisTram = base.GetComponent<Tram>();
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000F9070 File Offset: 0x000F7270
		public void UpdateTram(TramPath parentPath)
		{
			if (parentPath == null)
			{
				return;
			}
			this.thisTram.currentPath = new TramPath(parentPath.start, parentPath.end);
			float num = this.offset / parentPath.DistanceTotal;
			float num2 = parentPath.Progress - num;
			if (num2 < 0f)
			{
				float num3 = -num2 * parentPath.DistanceTotal;
				TramPath tramPath = new TramPath(parentPath.start, parentPath.end);
				while (num3 > 0f && tramPath.start.GetDestination(false) != null)
				{
					tramPath = new TramPath(tramPath.start.GetDestination(false), tramPath.start);
					if (num3 <= tramPath.DistanceTotal)
					{
						this.thisTram.currentPath = tramPath;
						this.thisTram.currentPath.distanceTravelled = tramPath.DistanceTotal - num3;
						num3 = 0f;
					}
					else
					{
						num3 -= tramPath.DistanceTotal;
					}
				}
				this.thisTram.UpdateWorldRotation();
				this.thisTram.UpdateWorldPosition();
				return;
			}
			if (!this.thisTram.currentPath.Equals(parentPath))
			{
				this.thisTram.currentPath = new TramPath(parentPath.start, parentPath.end);
			}
			this.thisTram.currentPath.distanceTravelled = num2 * this.thisTram.currentPath.DistanceTotal;
			this.thisTram.UpdateWorldRotation();
			this.thisTram.UpdateWorldPosition();
		}

		// Token: 0x04002A55 RID: 10837
		public float offset = 10f;

		// Token: 0x04002A56 RID: 10838
		private Tram thisTram;
	}
}
